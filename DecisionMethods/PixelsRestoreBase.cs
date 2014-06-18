using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DecisionMethods
{
    public class PixelsRestoreBase
    {
        internal static int averagePixel(int[,] bytes, bool[,] mask, int i, int j)
        {
            var colors = new List<MyColor>();

            if (!mask[i - 1, j - 1])
                colors.Add(new MyColor(bytes[i - 1, j - 1]));
            if (!mask[i - 1, j])
                colors.Add(new MyColor(bytes[i - 1, j]));
            if (!mask[i - 1, j + 1])
                colors.Add(new MyColor(bytes[i - 1, j + 1]));
            if (!mask[i, j - 1])
                colors.Add(new MyColor(bytes[i, j - 1]));
            if (!mask[i, j + 1])
                colors.Add(new MyColor(bytes[i, j + 1]));
            if (!mask[i + 1, j - 1])
                colors.Add(new MyColor(bytes[i + 1, j - 1]));
            if (!mask[i + 1, j])
                colors.Add(new MyColor(bytes[i + 1, j]));
            if (!mask[i + 1, j + 1])
                colors.Add(new MyColor(bytes[i + 1, j + 1]));

            if (colors.Count != 0)
                return new MyColor((byte)colors.Average(c => c.R), (byte)colors.Average(c => c.G), (byte)colors.Average(c => c.B)).Color;
            else
                return bytes[i, j];
        }

        internal static int averagePixel(byte[,] bytes, bool[,] mask, int i, int j, int m)
        {
            double byteMask = 0xFF / m;

            Dictionary<byte, byte> K1 = new Dictionary<byte, byte>();
            if (!mask[i - 1, j - 1])
                addToDictionary(K1, bytes[i - 1, j - 1] / byteMask);
            if (!mask[i - 1, j])
                addToDictionary(K1, bytes[i - 1, j] / byteMask);
            if (!mask[i - 1, j + 1])
                addToDictionary(K1, bytes[i - 1, j + 1] / byteMask);
            if (!mask[i, j - 1])
                addToDictionary(K1, bytes[i, j - 1] / byteMask);
            if (!mask[i, j + 1])
                addToDictionary(K1, bytes[i, j + 1] / byteMask);
            if (!mask[i + 1, j - 1])
                addToDictionary(K1, bytes[i + 1, j - 1] / byteMask);
            if (!mask[i + 1, j])
                addToDictionary(K1, bytes[i + 1, j] / byteMask);
            if (!mask[i + 1, j + 1])
                addToDictionary(K1, bytes[i + 1, j + 1] / byteMask);

            byte maxColor = K1.OrderByDescending(d => d.Value).Select(d => d.Key).FirstOrDefault();

            return findColorAverage(bytes, i, j, m, maxColor);
        }

        internal static int findColorAverage(byte[,] bytes, int i, int j, int m, int color)
        {
            double byteMask = 0xFF / m;

            var colors = new List<int>();

            if ((byte)(bytes[i - 1, j - 1] / byteMask) == color)
                colors.Add(bytes[i - 1, j - 1]);
            if ((byte)(bytes[i - 1, j] / byteMask) == color)
                colors.Add(bytes[i - 1, j]);
            if ((byte)(bytes[i - 1, j + 1] / byteMask) == color)
                colors.Add(bytes[i - 1, j + 1]);
            if ((byte)(bytes[i, j - 1] / byteMask) == color)
                colors.Add(bytes[i, j - 1]);
            if ((byte)(bytes[i, j + 1] / byteMask) == color)
                colors.Add(bytes[i, j + 1]);
            if ((byte)(bytes[i + 1, j - 1] / byteMask) == color)
                colors.Add(bytes[i + 1, j - 1]);
            if ((byte)(bytes[i + 1, j - 1] / byteMask) == color)
                colors.Add(bytes[i + 1, j]);
            if ((byte)(bytes[i + 1, j + 1] / byteMask) == color)
                colors.Add(bytes[i + 1, j + 1]);

            if (colors.Count != 0)
                return (int)colors.Average();
            else
                return bytes[i, j];
        }

        internal static int findColorCount(byte[,] bytes, int i, int j, int m, int color)
        {
            double byteMask = 0xFF / m;

            var colors = new List<int>();

            if ((byte)(bytes[i - 1, j - 1] / byteMask) == color)
                colors.Add(bytes[i - 1, j - 1]);
            if ((byte)(bytes[i - 1, j] / byteMask) == color)
                colors.Add(bytes[i - 1, j]);
            if ((byte)(bytes[i - 1, j + 1] / byteMask) == color)
                colors.Add(bytes[i - 1, j + 1]);
            if ((byte)(bytes[i, j - 1] / byteMask) == color)
                colors.Add(bytes[i, j - 1]);
            if ((byte)(bytes[i, j + 1] / byteMask) == color)
                colors.Add(bytes[i, j + 1]);
            if ((byte)(bytes[i + 1, j - 1] / byteMask) == color)
                colors.Add(bytes[i + 1, j - 1]);
            if ((byte)(bytes[i + 1, j - 1] / byteMask) == color)
                colors.Add(bytes[i + 1, j]);
            if ((byte)(bytes[i + 1, j + 1] / byteMask) == color)
                colors.Add(bytes[i + 1, j + 1]);

            return colors.Count;
        }

        internal static void addToDictionary(Dictionary<byte, byte> dct, double color)
        {
            if (!dct.ContainsKey((byte)color))
                dct.Add((byte)color, 1);
            else
                dct[(byte)color]++;
        }

        internal static T getElem<T>(T[,] array, int i, int j, int c)
        {
            switch (c)
            {
                case 0: return array[i - 1, j - 1];
                case 1: return array[i, j - 1];
                case 2: return array[i + 1, j - 1];
                case 3: return array[i - 1, j];
                case 4: return array[i + 1, j];
                case 5: return array[i - 1, j + 1];
                case 6: return array[i, j + 1];
                case 7: return array[i + 1, j + 1];
            }
            return array[i, j];
        }

        internal static int findMaxElemPosition(double[] array)
        {
            var max = array[0];
            var pos = 0;
            for (int i = 0; i < array.Length; i++)
                if (array[i] > max)
                {
                    max = array[i];
                    pos = i;
                }
            return pos;
        }
    }
}
