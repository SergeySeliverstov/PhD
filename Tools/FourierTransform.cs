using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Tools
{
    public class FourierTransform
    {
        public static Complex[,] DFT2(Complex[,] input)
        {
            return DFT2Body(input, false);
        }

        public static Complex[,] IDFT2(Complex[,] input)
        {
            return DFT2Body(input, true);
        }

        public static Complex[] DFT(Complex[] input)
        {
            return DFTBody(input, false);
        }

        public static Complex[] IDFT(Complex[] input)
        {
            return DFTBody(input, true);
        }

        private static Complex[] DFTBody(Complex[] input, bool inverse)
        {
            Int32 Count = input.Length;
            Complex[] y = new Complex[Count];

            int kn = inverse ? 1 : -1;

            for (Int32 k = 0; k < Count; ++k)
            {
                Complex tmp = Complex.Zero;

                for (Int32 j = 0; j < Count; ++j)
                {
                    Complex n = ((kn * 2.0 * Math.PI * k * j) / Count) * Complex.ImaginaryOne;
                    tmp += input[j] * Complex.Exp(n);
                }

                if (inverse)
                    y[k] = tmp / Count;
                else
                    y[k] = tmp;
            }
            return y;
        }

        private static Complex[,] DFT2Body(Complex[,] input, bool inverse)
        {
            Complex[,] output1 = new Complex[input.GetLength(0), input.GetLength(1)];
            Complex[,] output2 = new Complex[input.GetLength(0), input.GetLength(1)];

            for (int i = 0; i < input.GetLength(0); i++)
            {
                Complex[] temp = new Complex[input.GetLength(1)];
                for (int j = 0; j < input.GetLength(1); j++)
                    temp[j] = input[i, j];

                Complex[] y = DFTBody(temp, inverse);

                for (int j = 0; j < input.GetLength(1); j++)
                    output1[i, j] = y[j];
            }

            for (int j = 0; j < input.GetLength(1); j++)
            {
                Complex[] temp = new Complex[input.GetLength(0)];
                for (int i = 0; i < input.GetLength(0); i++)
                    temp[i] = output1[i, j];

                Complex[] y = DFTBody(temp, inverse);

                for (int i = 0; i < input.GetLength(0); i++)
                    output2[i, j] = y[i];
            }

            return output2;
        }
    }
}
