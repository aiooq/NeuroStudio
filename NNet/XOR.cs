using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNet
{
    class XOR
    {
        static public double[] x = { 0, 0, 1, 1 };
        static public double[] y = { 0, 1, 0, 1 };
        static public double[] xor = { 0, 1, 1, 0 };
        static public double[] xor2 = { 0.2, 0.5, 0.5, 0.2 };

        static public NeuroNet NN = new NeuroNet();

        static public void Init()
        {
            //for (int i = 0; i < xor.Length; i++)
            //{
            //    NN.data_initial.Add(new Data());
            //    NN.data_initial[i].In.Add(x[i]);      // x   
            //    NN.data_initial[i].In.Add(y[i]);      // y
            //    NN.data_initial[i].Out.Add(xor[i]);   // xor
            //    NN.data_initial[i].Out.Add(xor2[i]);   // xor2
            //}

            //NN.Init("name", "5");
        }
        static public void Learning(int eras = 1000)
        {
            NN.Fit(eras);
        }
    }
}
