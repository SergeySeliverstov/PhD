using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecisionMethods
{
    public class PixelsSearching
    {
        public static bool[,] FindPixels(int[,] bytes, int m, double n, double k, bool color)
        {
            byte[,] channelBytes = new byte[bytes.GetLength(0), bytes.GetLength(1)];
            int[,] tmpResult = new int[bytes.GetLength(0), bytes.GetLength(1)];
            bool[,] mask = new bool[bytes.GetLength(0), bytes.GetLength(1)];

            for (byte ch = 0; ch < (color ? 3 : 1); ch++)
            {
                for (int i = 0; i < channelBytes.GetLength(0); i++)
                    for (int j = 0; j < channelBytes.GetLength(1); j++)
                        channelBytes[i, j] = (byte)((bytes[i, j] & (0xFF << ch * 8)) >> (ch * 8));

                for (int i = 0; i < channelBytes.GetLength(0); i++)
                    for (int j = 0; j < channelBytes.GetLength(1); j++)
                        if (i > 1 && j > 1 && i < channelBytes.GetLength(0) - 1 && j < channelBytes.GetLength(1) - 1)
                            tmpResult[i, j] += PixelIsBroken(channelBytes, i, j, m, n, k) ? 1 : 0;
            }

            for (int i = 0; i < channelBytes.GetLength(0); i++)
                for (int j = 0; j < channelBytes.GetLength(1); j++)
                    mask[i, j] = tmpResult[i, j] >= (color ? 2 : 1);

            return mask;
        }

        public static bool PixelIsBroken(byte[,] channelBytes, int i, int j, int m, double n, double k)
        {
            double byteMask = 0xFF / m;
            //byte byteMask = 1;

            double r1 = n * k / (n * k + k + 1);
            double r2 = k / (n * k + k + 1);
            double r3 = 1 / (n * k + k + 1);

            // K1
            int x = 0;
            x += (int)(channelBytes[i - 1, j] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;
            x += (int)(channelBytes[i + 1, j] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;
            x += (int)(channelBytes[i, j - 1] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;
            x += (int)(channelBytes[i, j + 1] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;

            // K2
            int y = 0;
            y += (int)(channelBytes[i - 1, j - 1] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;
            y += (int)(channelBytes[i + 1, j - 1] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;
            y += (int)(channelBytes[i - 1, j + 1] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;
            y += (int)(channelBytes[i + 1, j + 1] / byteMask) == (int)(channelBytes[i, j] / byteMask) ? 1 : 0;

            // K3
            double c0 = 0;
            for (int ix = i - 1; ix <= i + 1; ix++)
                for (int jx = j - 1; jx <= j + 1; jx++)
                    if (!(ix == i && jx == j))
                        c0 += (int)(channelBytes[ix, jx] / byteMask);
            c0 /= 8;
            double dc = Math.Abs((int)(channelBytes[i, j] / byteMask) - c0);

            double p1 = 1 - (double)x / 4;
            double p2 = 1 - (double)y / 4;
            double p3 = dc / (m - 1);

            double q1 = 1 - p1;
            double q2 = 1 - p2;
            double q3 = 1 - p3;

            double py = r1 * p1 + r2 * p2 + r3 * p3;
            double pn = r1 * q1 + r2 * q2 + r3 * q3;

            return py > pn;
        }
    }
}
