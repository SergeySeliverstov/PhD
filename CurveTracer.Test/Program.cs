using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurveTracer;

namespace CurveTracer.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //double[] Ugs = new double[] { -3.5, -2.8, -2.1, -1.4, -0.7, 0, 0.7, 1.4, 2.1, 2.8, 3.5 };
            double[] Ugs = new double[] { -12, -11.3, -10.6, -9.9, -9.2, -8.5, -7.8, -7.1, -6.4, -5.7, -5 };
            double[] k0 = new double[] { 0, 4.12, 16.1, 19, 19.8, 19.78, 19.52, 19.08, 18.5, 17.9, 17.32 };
            double shift = (Ugs[6] - Ugs[5]);
            int size = (int)Math.Round((Math.Abs(Ugs[10] - Ugs[0])) / shift) + 1;

            double[] result = new double[size];
            double[] result2 = new double[size];
            double[] result3 = new double[size];
            for (double u = Ugs[0]; u < Ugs[10]; u += shift)
            {
                int element = (int)Math.Round(Math.Abs(Ugs[0] - u) / shift);
                result[element] = CurveTracer.B(Ugs, k0, u);
                result2[element] = CurveTracer.B2(Ugs, k0, u);
                result3[element] = result[element] / (2 * result2[element]);
            }
        }
    }
}
