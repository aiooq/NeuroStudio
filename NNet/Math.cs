using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNet
{
    static class Math
    {
        static public Random rand = new Random();
        static public double RandomDouble(double min = -1, double max = 1)
        {
            return (rand.NextDouble() * (max - min) + min);
        }
        
        static public double NormalizationLine(double value, double x_min = -10, double x_max = 10, double y_min = 0.0, double y_max = 1.0)
        {
            return (((value - x_min) * (y_max - y_min)) / (x_max - x_min) + y_min);
        }
        static public void NormalizationLine(List<double> values, double x_min = -10, double x_max = 10, double y_min = 0.0, double y_max = 1.0)
        {
            if (x_min == x_max) return;

            x_max -= x_min;
            y_max -= y_min;

            for (int i = 0; i < values.Count; i++)
            {
                values[i] = ((values[i] - x_min) * y_max) / x_max + y_min;
            }
        }
       
        static public List<double> Normalization(List<double> values_original, ref double expectation, ref double dispersion, ref double max, ENUM_NONLINEARY mode = ENUM_NONLINEARY.SIGMOIDA)
        {
            List<double> values_norm = new List<double>(values_original);

            max = CovarianceToNull(values_norm);

            dispersion = System.Math.Abs(Dispersion(values_norm));

            expectation = values_norm.Average();
            double dispersion_sqrt = System.Math.Sqrt(dispersion);

            for (int i = 0; i < values_norm.Count; i++)
            {
                values_norm[i] = Nonlineary(((values_norm[i]/* - expectation*/) / dispersion_sqrt), mode);
            }

            expectation = values_original.Average();
            return (values_norm);
        }
        static public List<double> Denormalization(List<double> values_norm, double expectation, double dispersion, double max, ENUM_NONLINEARY mode = ENUM_NONLINEARY.SIGMOIDA)
        {
            List<double> values_original = new List<double>(values_norm);

            double dispersion_sqrt = System.Math.Sqrt(dispersion);

            for (int i = 0; i < values_original.Count; i++)
            {
                values_original[i] = Denonlineary(values_original[i], mode);
                values_original[i] = (values_original[i] /*+ expectation*/ * dispersion_sqrt) * max + expectation;
            }

            return (values_original);
        }
        static public double Denormalization(double values_norm, double expectation, double dispersion, double max, ENUM_NONLINEARY mode = ENUM_NONLINEARY.SIGMOIDA)
        {
            double dispersion_sqrt = System.Math.Sqrt(dispersion);

            values_norm = Denonlineary(values_norm, mode);
            values_norm = (values_norm * dispersion_sqrt) * max + expectation;

            return (values_norm);
        }

        static public double CovarianceToNull(List<double> values)
        {
            // Выравнимание ковариации выборки к нулю
            double math_expectation = values.Average();
            for (int i = 0; i < values.Count; i++)
            {
                values[i] -= math_expectation;
            }

            double value_max = System.Math.Abs(values.Max());
            for (int i = 0; i < values.Count; i++)
            {
                values[i] /= value_max;
            }

            return (value_max);
        }
        static public double Dispersion(List<double> values)
        {
            double value = 0;
            double math_expectation = values.Average();

            for (int i = 0; i < values.Count; i++)
            {
                value += System.Math.Pow((values[i] - math_expectation),2);
            }

            return((value / values.Count)-1);
        }
        
        static public double Nonlineary(double value, ENUM_NONLINEARY type)
        {
            switch (type)
            {
                case ENUM_NONLINEARY.SIGMOIDA:  
                    return (1 / (1 + System.Math.Exp(-value)));
                case ENUM_NONLINEARY.TANGENS:
                    value = System.Math.Exp(value * 2);
                    return ((value - 1) / (value + 1));
            }

            return (value);
        }
        static public double Denonlineary(double value, ENUM_NONLINEARY type)
        {
            switch (type)
            {
                case ENUM_NONLINEARY.SIGMOIDA: return (-System.Math.Log((1/value)-1));             // Сигмоидная от 0 до 1
                case ENUM_NONLINEARY.TANGENS: return (System.Math.Log(System.Math.Sqrt(1 - System.Math.Pow(value, 2)) / (1 - value)));
            }

            return (value);
        }
        
        static public double Derivative(double value, ENUM_NONLINEARY type)
        {
            switch (type)
            {
                case ENUM_NONLINEARY.SIGMOIDA: return ((1 - value) * value);                            // Сигмоидная от 0 до 1
                case ENUM_NONLINEARY.TANGENS: return (1 - System.Math.Pow(value, 2));                   // Гиперболический тангенс от -1 до 1
            }

            return (value);
        }
        
        static public double Abs(double value)
        {
            if (value < 0) return (value *= -1); else return (value);
        }
        static public int Abs(int value)
        {
            if (value < 0) return (value *= -1); else return (value);
        }

        static public int GetPrecision(double value)
        {
            string[] arr = value.ToString().Split(',');
            if (arr.Length == 1) return (0);
            return (arr[1].Length);
        }
    }
}
