using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurveTracer
{
    public class CurveTracer
    {
        private static double[,] coefficients = new double[,]
            { {	0,	        -0.291375,	0,       	1.092658,	0,      	-5.1062086,	0	},
            {	0.0454545,	-0.262238,	-0.339938,	0.728439,	2.003205,	-1.5318624,	-12.765522	},
            {	0.0909091,	-0.174825,	-0.55847,	-0.18211,	2.003205,	4.5955891,	-0.9118072	},
            {	0.1363636,	-0.029138,	-0.534189,	-1.092658,	-0.500801,	3.7020048,	15.045106	},
            {	0.1818182,	0.174825,	-0.145688,	-1.092658,	-3.004808,	-6.1274442,	-10.485929	},
            {	0.2272727,	0.437063,	0.728439,	1.092658,	1.502404,	1.9148344,	2.2795937	} };

        private static double[] X = new double[] { -1.0, -0.8, -0.6, -0.4, -0.2, 0, 0.2, 0.4, 0.6, 0.8, 1.0 };

        private static double[] a(double[] Ugs, double[] k0, double x)
        {
            double[] Kodd = new double[6];
            double[] Keven = new double[6];

            for (int i = 0; i < 6; i++)
            {
                Kodd[i] = k0[5 + i] - k0[5 - i];
                Keven[i] = k0[5 + i] + k0[5 - i];
                if (i == 0)
                    Keven[i] /= 2;
            }

            double D0 = 0;
            for (int i = 0; i < 11; i++)
            {
                D0 += k0[i];
            }
            D0 /= 11;

            double[] D = new double[8];

            for (int i = 0; i < 8; i++)
            {
                if (i == 0)
                {
                    D[i] = D0;
                }
                else
                {
                    if (i % 2 != 0)
                        for (int j = 0; j < 6; j++)
                            D[i] += Kodd[j] * coefficients[j, i - 1];
                    else
                        for (int j = 0; j < 6; j++)
                            D[i] += Keven[j] * coefficients[j, i - 1];
                }
            }

            double[] A = new double[8];
            A[0] = D[0] - 0.4 * D[2] + 0.1152 * D[4] - 0.0279273 * D[6];
            A[1] = D[1] - 0.712 * D[3] + 0.3050667 * D[5] - 0.0932073 * D[7];
            A[2] = D[2] - D[4] + 0.5474909 * D[6];
            A[3] = D[3] - 1.2666667 * D[5] + 0.8185399 * D[7];
            A[4] = D[4] - 1.5090991 * D[6];
            A[5] = D[5] - 1.7230769 * D[7];
            A[6] = D[6];
            A[7] = D[7];

            return A;
        }

        public static double B(double[] Ugs, double[] k0, double x)
        {
            double[] A = a(Ugs, k0, x);

            double result = 0;
            for (int i = 0; i < 8; i++)
            {
                if (X[5] == Ugs[5])
                    result += A[i] * Math.Pow(2 * x / (10 * (Ugs[6] - Ugs[5])), i);
                else
                    if (Ugs[0] == 0)
                        result += A[i] * Math.Pow(2 * (x - Math.Abs(Ugs.Max(qi => qi)) / 2) / (10 * (Ugs[6] - Ugs[5])), i);
                    else
                        result += A[i] * Math.Pow(2 * (x - Ugs.Average(qi => qi)) / (10 * (Ugs[6] - Ugs[5])), i);
            }

            return result;
        }

        public static double B2(double[] Ugs, double[] k0, double x)
        {
            double[] A = a(Ugs, k0, x);

            double result = 0;
            for (int i = 2; i < 8; i++)
            {
                if (X[5] == Ugs[5])
                    result += i * (i - 1) * A[i] * Math.Pow(2 * x / (10 * (Ugs[6] - Ugs[5])), i - 2);
                else
                    if (Ugs[0] == 0)
                        result += i * (i - 1) * A[i] * Math.Pow(2 * (x - Math.Abs(Ugs.Max(qi => qi)) / 2) / (10 * (Ugs[6] - Ugs[5])), i - 2);
                    else
                        result += i * (i - 1) * A[i] * Math.Pow(2 * (x - Ugs.Average(qi => qi)) / (10 * (Ugs[6] - Ugs[5])), i - 2);
            }

            return result;
        }        
    }
}

