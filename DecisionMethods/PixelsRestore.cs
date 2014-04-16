using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionMethods
{
    public class PixelsRestore
    {
        public static int[,] FindPixels(int[,] bytes, bool[,] mask, int m, double n)
        {
            byte[,] channelBytes = new byte[bytes.GetLength(0), bytes.GetLength(1)];
            int[,] result = new int[bytes.GetLength(0), bytes.GetLength(1)];

            for (byte ch = 0; ch < 3; ch++)
            {
                for (int i = 0; i < channelBytes.GetLength(0); i++)
                    for (int j = 0; j < channelBytes.GetLength(1); j++)
                        channelBytes[i, j] = (byte)((bytes[i, j] & (0xFF << ch * 8)) >> (ch * 8));

                for (int i = 0; i < channelBytes.GetLength(0); i++)
                    for (int j = 0; j < channelBytes.GetLength(1); j++)
                        if (i > 1 && j > 1 && i < channelBytes.GetLength(0) - 1 && j < channelBytes.GetLength(1) - 1)
                            if (mask[i, j])
                                result[i, j] ^= FindColor(channelBytes, i, j, m, n) << ch * 8;
                            else
                                result[i, j] = bytes[i, j];
            }

            return result;
        }

        public static int FindColor(byte[,] channelBytes, int i, int j, int m, double n)
        {
            double byteMask = 0xFF / m;

            double r1 = n / (n + 2);
            double r2 = 1 / (n + 2);
            double r3 = r2;

            // K1
            Dictionary<byte, byte> K1 = new Dictionary<byte, byte>();
            addToDictionary(K1, channelBytes[i - 1, j - 1] / byteMask);
            addToDictionary(K1, channelBytes[i - 1, j] / byteMask);
            addToDictionary(K1, channelBytes[i - 1, j + 1] / byteMask);
            addToDictionary(K1, channelBytes[i, j - 1] / byteMask);
            addToDictionary(K1, channelBytes[i, j + 1] / byteMask);
            addToDictionary(K1, channelBytes[i + 1, j - 1] / byteMask);
            addToDictionary(K1, channelBytes[i + 1, j] / byteMask);
            addToDictionary(K1, channelBytes[i + 1, j + 1] / byteMask);

            byte maxColor = K1.OrderByDescending(d => d.Value).Select(d => d.Key).FirstOrDefault();

            // K2
            double dc1 = Math.Abs((channelBytes[i - 1, j - 1] / byteMask + channelBytes[i - 1, j] / byteMask + channelBytes[i - 1, j + 1] / byteMask) / 3 -
                                 (channelBytes[i + 1, j - 1] / byteMask - channelBytes[i + 1, j] / byteMask - channelBytes[i + 1, j + 1] / byteMask) / 3);

            // K3
            double dc2 = Math.Abs((channelBytes[i - 1, j - 1] / byteMask + channelBytes[i, j - 1] / byteMask + channelBytes[i + 1, j - 1] / byteMask) / 3 -
                                  (channelBytes[i - 1, j + 1] / byteMask - channelBytes[i, j + 1] / byteMask - channelBytes[i + 1, j + 1] / byteMask) / 3);

            double p1 = (double)K1[maxColor] / 8;
            double p2 = 1 - dc1 / m;
            double p3 = 1 - dc2 / m;

            double q1 = 1 - p1;
            double q2 = 1 - p2;
            double q3 = 1 - p3;

            double pA = r1 * p1 + r2 * p2 + r3 * p3;
            double pB = r1 * q1 + r2 * q2 + r3 * q3;

            return pA > pB ? (int)(maxColor * byteMask) : (int)((dc1 + dc2) * byteMask / 2);
        }

        private static void addToDictionary(Dictionary<byte, byte> dct, double color)
        {
            if (!dct.ContainsKey((byte)color))
                dct.Add((byte)color, 1);
            else
                dct[(byte)color]++;
        }
    }
}
