using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AForge.Math;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AForgeTools
{
    public static class Conversion
    {
        public static int[] ComplexToInt(Complex[] array)
        {
            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = (int)array[i].Magnitude;
            return result;
        }

        public static Complex[] IntToComplex(int[] array)
        {
            Complex[] result = new Complex[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = new Complex(array[i], 0);
            return result;
        }

        public static int[,] ComplexToInt(Complex[,] array)
        {
            int[,] result = new int[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    result[i, j] = (int)array[i, j].Magnitude;
            return result;
        }

        public static Complex[,] IntToComplex(int[,] array)
        {
            Complex[,] result = new Complex[array.GetLength(0), array.GetLength(1)];
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    result[i, j] = new Complex((float)-1 * array[i, j] / 255, 0);
            return result;
        }

        public static Bitmap MakeGrayscale3(Bitmap oldbmp)
        {
            using (var ms = new MemoryStream())
            {
                //oldbmp.Save(ms, ImageFormat.Gif);
                //ms.Position = 0;
                //Bitmap orig = new Bitmap(ms);
                Bitmap clone = oldbmp.Clone(new Rectangle(0, 0, oldbmp.Width, oldbmp.Height), PixelFormat.Format8bppIndexed);
                return clone;
            }
        }
    }
}
