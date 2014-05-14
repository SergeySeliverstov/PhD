using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Drawing;

namespace Tools
{
    public class ImageTransform
    {
        public static void AddNoise(int[,] imageBytes, int amount)
        {
            Random TempRandom = new Random();
            for (int x = 0; x < imageBytes.GetLength(0); ++x)
            {
                for (int y = 0; y < imageBytes.GetLength(1); ++y)
                {
                    int R = ((imageBytes[x, y] >> 16) & 0xFF) + TempRandom.Next(-amount, amount + 2);
                    int G = ((imageBytes[x, y] >> 8) & 0xFF) + TempRandom.Next(-amount, amount + 2);
                    int B = (imageBytes[x, y] & 0xFF) + TempRandom.Next(-amount, amount + 2);
                    R = R > 255 ? 255 : R;
                    R = R < 0 ? 0 : R;
                    G = G > 255 ? 255 : G;
                    G = G < 0 ? 0 : G;
                    B = B > 255 ? 255 : B;
                    B = B < 0 ? 0 : B;
                    imageBytes[x, y] = (R << 16) | (G << 8) | B;
                }
            }
        }

        public static void AddNoise(int[,] imageBytes, int amount, int percent)
        {
            Random TempRandom = new Random();
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);
            for (int i = 0; i < percent * imageWidth * imageHeight / 100; ++i)
            {
                int x = TempRandom.Next(0, imageWidth);
                int y = TempRandom.Next(0, imageHeight);
                int R = ((imageBytes[x, y] >> 16) & 0xFF);
                int G = ((imageBytes[x, y] >> 8) & 0xFF);
                int B = (imageBytes[x, y] & 0xFF);
                switch (TempRandom.Next(1, 3))
                {
                    case 1:
                        R += TempRandom.Next(-amount, amount + 2);
                        R = R > 255 ? 255 : R;
                        R = R < 0 ? 0 : R;
                        break;
                    case 2:
                        G += TempRandom.Next(-amount, amount + 2);
                        G = G > 255 ? 255 : G;
                        G = G < 0 ? 0 : G;
                        break;
                    case 3:
                        B += TempRandom.Next(-amount, amount + 2);
                        B = B > 255 ? 255 : B;
                        B = B < 0 ? 0 : B;
                        break;
                }
                imageBytes[x, y] = (R << 16) | (G << 8) | B;
            }
        }

        public static void AddMask(int[,] imageBytes, decimal percent)
        {
            Random rnd = new Random();
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);
            for (int u = 0; u < (percent / 100) * imageWidth * imageHeight; u++)
            {
                int x = rnd.Next(0, imageBytes.GetLength(0));
                int y = rnd.Next(0, imageBytes.GetLength(1));
                imageBytes[x, y] = Consts.MaskColor;
            }
        }

        public static bool[,] Pollute(int[,] imageBytes, decimal percent, bool saltPepper)
        {
            Random rnd = new Random();
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);
            bool[,] polluteMask = new bool[imageWidth, imageHeight];
            for (int i = 0; i < (percent / 100) * imageWidth * imageHeight; i++)
            {
                int x = 0;
                int y = 0;
                do
                {
                    x = rnd.Next(0, imageWidth);
                    y = rnd.Next(0, imageHeight);
                } while (polluteMask[x, y]);

                if (saltPepper)
                    imageBytes[x, y] = (int)(Math.Round((decimal)rnd.Next(0, 100) / 100)) * 0xFFFFFF;
                else
                    imageBytes[x, y] = (int)rnd.Next(0, 0xFFFFFF);
                polluteMask[x, y] = true;
            }

            return polluteMask;
        }

        public static void Invert(int[,] imageBytes)
        {
            for (int x = 0; x < imageBytes.GetLength(0); ++x)
                for (int y = 0; y < imageBytes.GetLength(1); ++y)
                    imageBytes[x, y] ^= 0x00FFFFFF;
        }

        public static int[,] InvertRecreate(int[,] imageBytes)
        {
            var array = ArrayTools.CopyArray<int>(imageBytes);
            Invert(array);
            return array;
        }

        public static void Transpose(int[,] A)
        {
            int[,] B = new int[A.GetLength(0), A.GetLength(1)];

            int halfI = B.GetLength(0) / 2;
            int halfJ = B.GetLength(1) / 2;

            for (int i = 0; i < B.GetLength(0); i++)
                for (int j = 0; j < B.GetLength(1); j++)
                    B[i > halfI ? i - halfI : i + halfI - 1, j > halfJ ? j - halfJ : j + halfJ - 1] = A[i, j];

            for (int i = 0; i < B.GetLength(0); i++)
                for (int j = 0; j < B.GetLength(1); j++)
                    A[i, j] = B[i, j];
        }

        public static int[,] TransposeRecreate(int[,] imageBytes)
        {
            var array = ArrayTools.CopyArray<int>(imageBytes);
            Transpose(imageBytes);
            return imageBytes;
        }

        public static void ConvertToGrayscale(int[,] A)
        {
            for (int i = 0; i < A.GetLength(0); i++)
                for (int j = 0; j < A.GetLength(1); j++)
                {

                    MyColor color = new MyColor(A[i, j]);
                    int r = color.R;
                    int g = color.G;
                    int b = color.B;

                    color.R = (byte)((r + g + b) / 3);
                    color.G = (byte)((r + g + b) / 3);
                    color.B = (byte)((r + g + b) / 3);

                    A[i, j] = color.Color;
                }
        }

        public static int[,] ConvertToGrayscaleRecreate(int[,] imageBytes)
        {
            var array = ArrayTools.CopyArray<int>(imageBytes);
            ConvertToGrayscale(imageBytes);
            return imageBytes;
        }

        public static Complex[,] DecreaseIntensive(Complex[,] A)
        {
            Complex[,] B = new Complex[A.GetLength(0), A.GetLength(1)];
            int b = 4335;

            for (int i = 0; i < B.GetLength(0); i++)
                for (int j = 0; j < B.GetLength(1); j++)
                    B[i, j] = b * A[i, j] / A[0, 0];

            return B;
        }

        public static Bitmap BoolToBitmap(bool[,] inputMask)
        {
            int[,] mask = new int[inputMask.GetLength(0), inputMask.GetLength(1)];
            for (int i = 0; i < mask.GetLength(0); i++)
                for (int j = 0; j < mask.GetLength(1); j++)
                    mask[i, j] = inputMask[i, j] ? 0 : 0xFFFFFF;

            MyImage maskImage = new MyImage(mask);
            return maskImage.Bitmap;
        }
    }
}
