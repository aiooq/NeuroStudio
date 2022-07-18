using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;

namespace NNet
{
    public class NeuroNet : Classifire
    {
        private double delta, root_mse, mse;
        private int[] neurones_hidden_layers;                        // Кол-во нейронов в скрытых слоях
        private int input_recurrent;                                 // Кол-во доп. входов рекурентной сети
        private int next, last, n_shift = 1;                         // Нейрон смещения (On = 1 / Off = 0)

        public NeuroNet()
        {

        }
        public NeuroNet(string name)
        {
            this.name = name;
        }

        public override void Fit(object epochs)
        {
            while(thread_create.IsAlive)
                Thread.Sleep(100);

            long epochs_int = Convert.ToInt64(epochs);
            for (epoch = 0; epoch < epochs_int; epoch++)
            {
                for (int ii = 0; ii < data.train.Count; ii++)
                {
                    GetNet(data.train, ii);
                    SetNet(ii);
                }

                // Переделать под извлечение структуры данных и прогон отдельно, а не внутри обучения
                // Тренировочная ошибка
                data.error_train = GetNetToErr(data.train);
                errors_learn.Add(data.error_train);

                // Тестовая ошибка
                data.error_test = GetNetToErr(data.test);
                errors_test.Add(data.error_test);
            }

            Predict();
            Evaluate();
        }
        public override void Predict()
        {
            while (thread_create.IsAlive)
                Thread.Sleep(100);

            data.error_train = GetNetToErr(data.train);
            data.DataDenorm(data.train);
        }
        public override void Evaluate()
        {
            while (thread_create.IsAlive)
                Thread.Sleep(100);

            data.error_test = GetNetToErr(data.test);
            data.DataDenorm(data.test);
        }
        public override bool Configurate()
        {
            if (data.train.Count <= 0) return (false);

            type = ENUM_CLASSIFIRIES.NN;

            string[] struct_hidden_layers = layers_hidden.Split(',');
            Array.Resize<int>(ref neurones_hidden_layers, struct_hidden_layers.Length);
            for (int i = 0; i < struct_hidden_layers.Length; i++)
            {
                neurones_hidden_layers[i] = Math.Abs(Convert.ToInt32(struct_hidden_layers[i]));
            }

            if (neurones_hidden_layers.Length <= 0) return (false);

            input_recurrent = 0;
            switch (type)
            {
                case ENUM_CLASSIFIRIES.NN:
                    break;
                case ENUM_CLASSIFIRIES.RNN:
                    for (int i = 0; i < neurones_hidden_layers.Length; i++)
                    {
                        input_recurrent += neurones_hidden_layers[i];
                    }
                    input_recurrent += data.train[0].outs_norm.Count;
                    break;
                case ENUM_CLASSIFIRIES.CNN:
                    break;
                case ENUM_CLASSIFIRIES.GRU:
                    break;
                case ENUM_CLASSIFIRIES.LSTM:
                    break;
                default:
                    break;
            }

            layers.Clear();

            // Создаем слои c заданным кол-вом нейронов и синапсов
            layers.Add(new Layer(data.train[0].inputs.Count + n_shift + input_recurrent, neurones_hidden_layers[0]));        // входной слой

            for (int i = 0; i < neurones_hidden_layers.Length - 1; i++)
            {
                layers.Add(new Layer(neurones_hidden_layers[i] + n_shift, neurones_hidden_layers[i + 1]));                 // скрытые слои
            }

            layers.Add(new Layer(neurones_hidden_layers[neurones_hidden_layers.Length - 1] + n_shift, data.train[0].outs_norm.Count));    // скрытый предпоследний слой

            layers.Add(new Layer(data.train[0].outs_norm.Count, 0, true));                                      // выходной слой

            last = layers.Count - 1;

            return (true);
        }
        public override ENUM_CLASSIFIRIES GetNeuroType() { return (ENUM_CLASSIFIRIES.NN); }
        
        private double GetNet(List<DataSet.Data> data, int index)
        {
            // Замерить производительность и переделать если потребуется 
            //--- Start ---//
            for (int i = layers[0].N.Count - (1 + n_shift + input_recurrent); i >= 0; i--)
            {
                layers[0].N[i].A = data[index].inputs[i];                                       // Передаем на вход сети нормализованные данные
            }
            //--- End ---//

            // Запускаем рекурентность сети
            int n_end = layers[0].N.Count - (1 + n_shift); // Конечный индекс выходов, подаваемых на вход

            switch (type)
            {
                case ENUM_CLASSIFIRIES.NN:
                    break;
                case ENUM_CLASSIFIRIES.RNN:
                    for (int i = layers.Count - 1; i >= 1; i--)
                    {
                        for (int ii = layers[i].N.Count - (1 + n_shift); ii >= 0; ii--)
                        {
                            layers[0].N[n_end].A = layers[i].N[ii].Ad;
                            n_end--;
                        }
                    }
                    break;
                case ENUM_CLASSIFIRIES.CNN:
                    break;
                case ENUM_CLASSIFIRIES.GRU:
                    break;
                case ENUM_CLASSIFIRIES.LSTM:
                    break;
                default:
                    break;
            }

            layers[0].N[layers[0].N.Count - 1].A = n_shift;

            int count_L = layers.Count - n_shift;

            int count_N = 0;
            int last = 0;

            for (int i = 1; i < count_L; i++)                                       // Перебираем слои от 1
            {
                last = i - 1;
                count_N = layers[i].N.Count - n_shift;

                layers[i].N[layers[i].N.Count - 1].A = n_shift;                               // Восстанавливаем единицу у нейрона смещения

                for (int ii = 0; ii < count_N; ii++)                                // Перебираем нейроны текущего слоя
                {
                    layers[i].N[ii].A = 0;

                    for (int iii = 0; iii < layers[last].N.Count; iii++)                 // Перебираем нейроны прошлого слоя                   
                    {
                        layers[i].N[ii].A += layers[last].N[iii].A * (double)layers[last].N[iii].W[ii];    // Вычисляем нейрон в соответствии с синапсами нейронов в прошлом слое
                    }

                    layers[i].N[ii].A = Math.Nonlineary(layers[i].N[ii].A, ENUM_NONLINEARY.SIGMOIDA);
                    layers[i].N[ii].Ad = layers[i].N[ii].A;
                }
            }

            // В случае если задан нейрон смещения, отдельно обрабатываем последний слой
            if (n_shift == 1)
            {
                last = count_L - 1;

                for (int i = 0; i < layers[count_L].N.Count; i++)                         // Перебираем нейроны текущего слоя
                {
                    layers[count_L].N[i].A = 0;

                    for (int ii = 0; ii < layers[last].N.Count; ii++)                     // Перебираем нейроны прошлого слоя                   
                    {
                        layers[count_L].N[i].A += layers[last].N[ii].A * (double)layers[last].N[ii].W[i];   // Вычисляем нейрон в соответствии с синапсами нейронов в прошлом слое
                    }

                    layers[count_L].N[i].A = Math.Nonlineary(layers[count_L].N[i].A, ENUM_NONLINEARY.SIGMOIDA);
                    layers[count_L].N[i].Ad = layers[count_L].N[i].A;
                }
            }

            mse = 0;
            count_L = layers.Count - 1;
            for (int i = 0; i < data[index].outs_norm.Count; i++)
            {
                mse += System.Math.Pow((data[index].outs_norm[i] - layers[count_L].N[i].A), 2);
            }
            mse /= data[index].outs_norm.Count;

            return (mse);
        }
        private void SetNet(int index)
        {
            // Корректируем последний выходной слой
            for (int i = 0; i < layers[last].N.Count; i++)
            {
                layers[last].N[i].A = (data.train[index].outs_norm[i] - layers[last].N[i].A) * Math.Derivative(layers[last].N[i].A, ENUM_NONLINEARY.SIGMOIDA);
            }

            // Корректируем скрытые слои
            delta = 0;
            for (int i = last - 1; i >= 0; i--)
            {
                next = i + 1;
                for (int ii = 0; ii < layers[i].N.Count; ii++)
                {
                    layers[i].N[ii].A *= speed;

                    for (int iii = 0; iii < layers[i].N[ii].W.Count; iii++)
                    {
                        delta += layers[next].N[iii].A * (double)layers[i].N[ii].W[iii];
                        layers[i].N[ii].Wd[iii] = layers[i].N[ii].A * layers[next].N[iii].A + moment * layers[i].N[ii].Wd[iii];
                        layers[i].N[ii].W[iii] += layers[i].N[ii].Wd[iii];
                    }

                    layers[i].N[ii].A = Math.Derivative(layers[i].N[ii].A, ENUM_NONLINEARY.SIGMOIDA) * delta;
                    delta = 0;
                }
            }
        }
        private double GetNetToErr(List<DataSet.Data> data)
        {
            root_mse = 0;
            for (int i = 0; i < data.Count; i++)
            {
                data[i].loss = GetNet(data, i);
                root_mse += data[i].loss;

                for (int ii = 0; ii < data[i].outs_norm.Count; ii++)
                    data[i].outs_neuro[ii] = layers[layers.Count - 1].N[ii].A;
            }
            return (System.Math.Pow((root_mse / data.Count), 0.5));
        }
        
        //private void CreateFormula()
        //{
        //    int count_L = layers.Count - n_shift;

        //    int count_N = 0;
        //    int last = 0;

        //    List<string> formula = new List<string>();
        //    string half_formula = "";

        //    for (int i = 1; i < count_L; i++)                                       // Перебираем слои от 1
        //    {
        //        last = i - 1;
        //        count_N = layers[i].N.Count - n_shift;

        //        for (int ii = 0; ii < count_N; ii++)                                // Перебираем нейроны текущего слоя
        //        {
        //            half_formula = "";
        //            for (int iii = 0; iii < layers[last].N.Count; iii++)                 // Перебираем нейроны прошлого слоя                   
        //            {
        //                if (i == 1) half_formula += "A" + (iii + 1).ToString() + "*" + layers[last].N[iii].W[ii].ToString();
        //                else half_formula += "N" + i.ToString() + "." + iii.ToString() + "*" + layers[last].N[iii].W[ii].ToString();
        //                if ((iii + 1) < layers[last].N.Count) half_formula += "+";
        //            }
        //            formula.Add("N" + (i + 1).ToString() + "." + ii.ToString() + "=" + half_formula);
        //        }
        //        formula.Add("N" + (i + 1).ToString() + "." + count_N.ToString() + "=" + "1");
        //    }

        //    // В случае если задан нейрон смещения, отдельно обрабатываем последний слой
        //    if (n_shift == 1)
        //    {
        //        last = count_L - 1;

        //        for (int i = 0; i < layers[count_L].N.Count; i++)                         // Перебираем нейроны текущего слоя
        //        {
        //            half_formula = "";
        //            for (int ii = 0; ii < layers[last].N.Count; ii++)                     // Перебираем нейроны прошлого слоя                   
        //            {
        //                half_formula += "N" + (last + 1).ToString() + "." + ii.ToString() + "*" + layers[last].N[ii].W[i].ToString();
        //                if ((ii + 1) < layers[last].N.Count) half_formula += "+";
        //            }
        //            formula.Add("B" + i.ToString() + "=" + half_formula);
        //        }
        //    }

        //    FormulaEdit(formula);
        //}
        //private void FormulaEdit(List<string> formula)
        //{
        //    string[] sep = { "=" };
        //    for (int i = 0; i < formula.Count; i++)
        //    {
        //        formula[i] = formula[i].ToString().Replace(",", "."); // ,->.

        //        string[] formula_split = formula[i].ToString().Split(sep,StringSplitOptions.None);

        //        formula[i]=formula[i].ToString().Replace(formula_split[0]+"=", "");

        //        if (formula_split[1] == "1")
        //        {
        //            FormulaReplace(formula, formula_split[0], formula_split[1]);
        //            continue;
        //        }

        //        formula[i] = "(1/(1+EXP(-" + formula[i] + ")))";
        //        formula_split[1] = formula[i];

        //        FormulaReplace(formula, formula_split[0], formula_split[1]);

        //        if (formula_split[0].IndexOf("B") >= 0) formula[i] = formula_split[0] + "=" + formula[i];
        //    }
        //}
        //private void FormulaReplace(List<string> formula, string old_value, string new_value)
        //{
        //    for (int i = 0; i < formula.Count; i++)
        //        formula[i] = formula[i].ToString().Replace(old_value, new_value);
        //}
    }
}
