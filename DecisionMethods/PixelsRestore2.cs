using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DecisionMethods
{
    public class PixelsRestore2 : PixelsRestoreBase
    {
        public static int[,] FindPixels(int[,] bytes, bool[,] mask, int m, double n)
        {
            byte[][,] channelBytes = new byte[3][,] { new byte[bytes.GetLength(0), bytes.GetLength(1)], new byte[bytes.GetLength(0), bytes.GetLength(1)], new byte[bytes.GetLength(0), bytes.GetLength(1)] };
            int[,] maskResult = new int[bytes.GetLength(0), bytes.GetLength(1)];
            int[,] result = new int[bytes.GetLength(0), bytes.GetLength(1)];

            for (byte ch = 0; ch < channelBytes.GetLength(0); ch++)
            {
                for (int i = 0; i < channelBytes[ch].GetLength(0); i++)
                    for (int j = 0; j < channelBytes[ch].GetLength(1); j++)
                        channelBytes[ch][i, j] = (byte)((bytes[i, j] & (0xFF << ch * 8)) >> (ch * 8));
            }

            for (int i = 0; i < bytes.GetLength(0); i++)
                for (int j = 0; j < bytes.GetLength(1); j++)
                    if (i > 1 && j > 1 && i < bytes.GetLength(0) - 2 && j < bytes.GetLength(1) - 2)
                    {
                        if (mask[i, j])
                        {
                            for (byte ch = 0; ch < 3; ch++)
                            {
                                var b = restorePixel(channelBytes[ch], mask, i, j, m, false);
                                result[i, j] ^= b << ch * 8;
                            }
                        }
                        else
                        {
                            result[i, j] = bytes[i, j];
                        }
                    }
                    else
                    {
                        result[i, j] = bytes[i, j];
                    }

            return result;
        }

        private static int restorePixel(byte[,] channelBytes, bool[,] mask, int i, int j, int m, bool firstMethod)
        {
            double byteMask = 0xFF / m;

            // K1
            double[] cx = new double[8];
            double[] U = new double[8];
            for (int k = 0; k < 8; k++)
            {
                var p = Point.GetPosition(new Point(i, j), k);
                cx[k] += 1 - Math.Abs(channelBytes[i, j] - averagePixel(channelBytes, mask, p.i, p.j, m)) / m;
            }
            for (int x = 0; x < 8; x++)
                U[x] = cx[x] / cx.Sum();

            //K2
            double[] cy = new double[8];
            double[] V = new double[8];
            for (int k = 0; k < 8; k++)
            {
                var p = Point.GetPosition(new Point(i, j), k);
                var color = (int)(channelBytes[p.i, p.j] / byteMask);
                cy[k] = findColorCount(channelBytes, p.i, p.j, m, color);
            }
            for (int x = 0; x < 8; x++)
                V[x] = cy[x] / cy.Sum();

            //K3
            double[] cz = new double[8];
            double[] W = new double[8];
            for (int k = 0; k < 8; k++)
            {
                var p1 = Point.GetPosition(new Point(i, j), k);
                var p2 = Point.GetPosition(new Point(i, j), 7 - k);
                var dc = (byte)Math.Abs(channelBytes[p1.i, p1.j] - channelBytes[p2.i, p2.j]);
                cz[k] = 1 - dc / m;
            }
            for (int x = 0; x < 8; x++)
                W[x] = cz[x] / cz.Sum();

            double[] pc = new double[8];
            for (int x = 0; x < 8; x++)
                pc[x] = U[x] + V[x] + W[x];

            if (firstMethod)
            {
                var pos = findMaxElemPosition(pc);
                var p = Point.GetPosition(new Point(i, j), pos);
                return channelBytes[p.i, p.j];
            }
            else
            {
                double sum = 0;
                for (int k = 0; k < 8; k++)
                {
                    var p = Point.GetPosition(new Point(i, j), k);
                    sum += channelBytes[p.i, p.j] * pc[k];
                }
                return (int)(sum/3);
            }
        }
    }
}