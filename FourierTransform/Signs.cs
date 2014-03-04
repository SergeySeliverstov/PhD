using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace FourierTransform
{
    public class Signs
    {
        public static int[,] CreateSignFromImage(int[,] sign, int imageWidth, int imageHeight)
        {
            int M = 33;
            int N = 33;
            double Q = 1;

            if (sign.GetLength(0) > (imageWidth / 2) || sign.GetLength(1) > (imageHeight / 2))
                return null;

            int[,] result = new int[imageWidth, imageHeight];

            for (int i = 0; i < sign.GetLength(0); i++)
                for (int j = 0; j < sign.GetLength(1); j++)
                {
                    result[imageWidth / 2 + M + i, N + j] = (int)(Q * (double)(sign[i, j]));
                    result[imageWidth / 2 - M - i - 1, imageHeight - N - j - 1] = (int)(Q * (double)sign[i, j]);

                    //MyColor color = new MyColor(sign[i, j]);
                    //color.R = (byte)(Q * (double)color.R);
                    //color.G = (byte)(Q * (double)color.G);
                    //color.B = (byte)(Q * (double)color.B);
                    //result[M + i, N + j] = color.Color;
                    //result[imageWidth - M - i - 1, imageHeight - N - j - 1] = color.Color;
                }

            return result;
        }

        public static int[,] CreateSignFromImage2(int[,] sign, int imageWidth, int imageHeight)
        {
            int M = 33;
            int N = 33;
            double Q = 0.005;

            if (sign.GetLength(0) > (imageWidth / 2) || sign.GetLength(1) > (imageHeight / 2))
                return null;

            var signDFT = Tools.FourierTransform.IDFT2(Tools.Conversion.IntToComplex(sign));

            Complex[,] result = new Complex[imageWidth, imageHeight];

            for (int i = 0; i < signDFT.GetLength(0); i++)
                for (int j = 0; j < signDFT.GetLength(1); j++)
                {
                    result[M + i, j + N] = Q * signDFT[i, j];
                    result[imageWidth - M - i - 1, imageHeight - N - j - 1] = Q * signDFT[i, j];
                }

            return Tools.Conversion.ComplexToInt(result);
        }

        public static int[,] CreateSignFromImage3(int[,] sign)
        {
            int M = 12;
            int N = 21;
            double Q = 0.0005;

            var signDFT = Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(sign));

            Complex[,] result = new Complex[2 * (signDFT.GetLength(0) + M), 2 * (signDFT.GetLength(1) + N)];

            int halfX = result.GetLength(0) / 2;
            int halfY = result.GetLength(1) / 2;
            for (int i = 0; i < signDFT.GetLength(0); i++)
                for (int j = 0; j < signDFT.GetLength(1); j++)
                {
                    result[halfX + (M + i), halfY - (N + j)] = Q * signDFT[i, j];
                    result[halfX - (M + i), halfY + (N + j)] = Q * signDFT[i, j];
                }

            return Tools.Conversion.ComplexToInt(result);
        }

        public static int[,] CreateSignFromImagePerl(int Q, int mode, int[,] A, int a0)
        {
            int M = 33;
            int N = 33;
            int NN = 256;
            int Q2 = Q / 2;
            //mode = 1;
            int[,] w1 = new int[NN, NN];
            int max = 0;

            //int[,] B = ComplexToInt(DFT2(IntToComplex(A)));
            var B = A;

            for (int y = 0; y < B.GetLength(0); y++)
            {
                for (int x = 0; x < B.GetLength(1); x++)
                {
                    w1[x + M, y + N] = B[x, y];
                    w1[NN - x - M - 1, NN - y - N - 1] = B[x, y];
                }
            }
            int[,] R = new int[NN, NN];
            if (mode == 2)
            {
                w1 = Tools.Conversion.ComplexToInt(Tools.FourierTransform.IDFT2(Tools.Conversion.IntToComplex(w1)));
                //&d2cdfti(\@w1,\@w2,\NN,\PI);                
                for (int y = 0; y < NN; y++)
                {
                    for (int x = 0; x < NN; x++)
                    {
                        R[x, y] = w1[x, y];
                        if (R[x, y] > max) { max = R[x, y]; }
                    }
                }
                Tools.ImageTransform.Transpose(w1);
                //&I__N(\NN,\@R,\@w1);
            }

            int[,] OUT = new int[NN, NN];
            for (int y = 0; y < NN; y++)
            {
                for (int x = 0; x < NN; x++)
                {
                    if (mode == 2)
                    {
                        OUT[x, y] = a0 + (int)(((double)Q2 * w1[x, y]) / max);
                    }
                    else
                    {
                        OUT[x, y] = w1[x, y];
                    }
                }
            }

            return OUT;
        }

        private void d2cdfti(int[,] Wx, int[,] Wy, int N, int PI)
        {
            int i, j;
            int[] coeff = new int[2 * Wx.GetLength(0)];
            for (j = 0; j < N; j++)
            {
                for (i = 0; i < N; i++)
                {
                    coeff[2 * i] = Wx[i, j];
                    coeff[2 * i + 1] = Wy[i, j];
                }
                var fft = Tools.FourierTransform.IDFT(Tools.Conversion.IntToComplex(coeff));

                for (i = 0; i < N; i++)
                {
                    Wx[i, j] = (int)fft[2 * i].Magnitude;
                    Wy[i, j] = (int)fft[2 * i + 1].Magnitude;
                }
            }
        }


    }
}
