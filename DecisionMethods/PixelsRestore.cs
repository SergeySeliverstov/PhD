using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DecisionMethods
{
    public class PixelsRestore
    {
        public static int[,] FindPixels(int[,] bytes, bool[,] mask, int m, double n)
        {
            byte[,] channelBytes = new byte[bytes.GetLength(0), bytes.GetLength(1)];
            int[,] maskResult = new int[bytes.GetLength(0), bytes.GetLength(1)];
            int[,] result = new int[bytes.GetLength(0), bytes.GetLength(1)];

            for (byte ch = 0; ch < 3; ch++)
            {
                for (int i = 0; i < channelBytes.GetLength(0); i++)
                    for (int j = 0; j < channelBytes.GetLength(1); j++)
                        channelBytes[i, j] = (byte)((bytes[i, j] & (0xFF << ch * 8)) >> (ch * 8));

                for (int i = 1; i < channelBytes.GetLength(0) - 1; i++)
                    for (int j = 1; j < channelBytes.GetLength(1) - 1; j++)
                        if (mask[i, j])
                            maskResult[i, j] += findColor(channelBytes, i, j, m, n);
            }

            for (int i = 0; i < bytes.GetLength(0); i++)
                for (int j = 0; j < bytes.GetLength(1); j++)
                    if (i > 1 && j > 1 && i < bytes.GetLength(0) - 2 && j < bytes.GetLength(1) - 2)
                    {
                        if (mask[i, j])
                        {
                            if (maskResult[i, j] > 1)
                                result[i, j] = restorePixel(bytes, mask, i, j, 4);
                            else
                                result[i, j] = averagePixel(bytes, i, j);
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

        public static int[,] FindPixelsOld(int[,] bytes, bool[,] mask, int method)
        {
            int[,] result = new int[bytes.GetLength(0), bytes.GetLength(1)];

            for (int i = 0; i < bytes.GetLength(0); i++)
                for (int j = 0; j < bytes.GetLength(1); j++)
                    if (i > 1 && j > 1 && i < bytes.GetLength(0) - 2 && j < bytes.GetLength(1) - 2)
                    {
                        if (mask[i, j])
                            result[i, j] = restorePixel(bytes, mask, i, j, method);
                        else
                            result[i, j] = bytes[i, j];
                    }
                    else
                    {
                        result[i, j] = bytes[i, j];
                    }

            return result;
        }

        private static int findColor(byte[,] channelBytes, int i, int j, int m, double n)
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

            return pA > pB ? 1 : 0;
        }

        private static void addToDictionary(Dictionary<byte, byte> dct, double color)
        {
            if (!dct.ContainsKey((byte)color))
                dct.Add((byte)color, 1);
            else
                dct[(byte)color]++;
        }

        private static int restorePixel(int[,] imageBytes, bool[,] mask, int x, int y, int method)
        {
            var colors = new List<KeyValuePair<Tools.ColorChannel, int>>();

            // 1 уровень
            checkColor(imageBytes, colors, mask, x - 1, y - 1);
            checkColor(imageBytes, colors, mask, x - 1, y);
            checkColor(imageBytes, colors, mask, x - 1, y + 1);
            checkColor(imageBytes, colors, mask, x, y - 1);
            checkColor(imageBytes, colors, mask, x, y + 1);
            checkColor(imageBytes, colors, mask, x + 1, y - 1);
            checkColor(imageBytes, colors, mask, x + 1, y);
            checkColor(imageBytes, colors, mask, x + 1, y + 1);

            // 2 уровень
            checkColor(imageBytes, colors, mask, x - 2, y - 2);
            checkColor(imageBytes, colors, mask, x - 2, y - 1);
            checkColor(imageBytes, colors, mask, x - 2, y);
            checkColor(imageBytes, colors, mask, x - 2, y + 1);
            checkColor(imageBytes, colors, mask, x - 2, y + 2);
            checkColor(imageBytes, colors, mask, x - 1, y - 2);
            checkColor(imageBytes, colors, mask, x - 1, y + 2);
            checkColor(imageBytes, colors, mask, x, y - 2);
            checkColor(imageBytes, colors, mask, x, y + 2);
            checkColor(imageBytes, colors, mask, x + 1, y - 2);
            checkColor(imageBytes, colors, mask, x + 1, y + 2);
            checkColor(imageBytes, colors, mask, x + 2, y - 2);
            checkColor(imageBytes, colors, mask, x + 2, y - 1);
            checkColor(imageBytes, colors, mask, x + 2, y);
            checkColor(imageBytes, colors, mask, x + 2, y + 1);
            checkColor(imageBytes, colors, mask, x + 2, y + 2);

            if (colors.Count() > 0)
            {
                var maxCount = colors.GroupBy(i => i.Key).Max(i => i.Count());
                var maxColor = colors.GroupBy(i => i.Key).FirstOrDefault(i => i.Count() == maxCount).Key;
                int newColor = 0;
                var maskX = maxColor == Tools.ColorChannel.R ? Tools.Consts.RedMask : maxColor == Tools.ColorChannel.G ? Tools.Consts.GreenMask : Tools.Consts.BlueMask;
                switch (method)
                {
                    case 0:
                        newColor = colors.Where(i => i.Key == maxColor).FirstOrDefault().Value;
                        break;
                    case 1:
                        newColor = colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & maskX).FirstOrDefault().Value;
                        break;
                    case 2:
                        newColor = colors.Where(i => i.Key == maxColor).OrderByDescending(i => i.Value & maskX).FirstOrDefault().Value;
                        break;
                    case 3:
                        var newColor1 = new MyColor(colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & maskX).FirstOrDefault().Value);
                        var newColor2 = new MyColor(colors.Where(i => i.Key == maxColor).OrderByDescending(i => i.Value & maskX).FirstOrDefault().Value);
                        byte r = (byte)((newColor1.R + newColor2.R) >> 1);
                        byte g = (byte)((newColor1.G + newColor2.G) >> 1);
                        byte b = (byte)((newColor1.B + newColor2.B) >> 1);
                        newColor = new MyColor(r, g, b).Color;
                        break;
                    case 4:
                        var list = colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & maskX).ToList();
                        newColor = list[list.Count / 2 + (list.Count % 2 == 1 ? 0 : -1)].Value;
                        break;
                    case 5:
                        int r1 = 0;
                        int g1 = 0;
                        int b1 = 0;

                        var newColorsCollection = colors.Where(i => i.Key == maxColor).Select(i => i.Value).ToList();
                        int count = newColorsCollection.Count;
                        foreach (int color in newColorsCollection)
                        {
                            var colorX = new MyColor(color);
                            r1 += colorX.R * colorX.R;
                            g1 += colorX.G * colorX.G;
                            b1 += colorX.B * colorX.B;
                        }

                        newColor = new MyColor((byte)(Math.Sqrt(r1 / count)), (byte)(Math.Sqrt(g1 / count)), (byte)(Math.Sqrt(b1 / count))).Color;
                        break;
                }

                return newColor;
            }
            return imageBytes[x, y];
        }

        private static void checkColor(int[,] imageBytes, List<KeyValuePair<Tools.ColorChannel, int>> colors, bool[,] mask, int i, int j)
        {
            if (!mask[i, j])
            {
                MyColor c = new MyColor(imageBytes[i, j]);
                colors.Add(new KeyValuePair<Tools.ColorChannel, int>((c.R > c.G && c.R > c.B) ? Tools.ColorChannel.R : (c.G > c.B) ? Tools.ColorChannel.G : Tools.ColorChannel.B, imageBytes[i, j]));
            }
        }

        private static int averagePixel(int[,] bytes, int i, int j)
        {
            var colors = new List<MyColor>();

            colors.Add(new MyColor(bytes[i - 1, j - 1]));
            colors.Add(new MyColor(bytes[i - 1, j]));
            colors.Add(new MyColor(bytes[i - 1, j + 1]));
            colors.Add(new MyColor(bytes[i, j - 1]));
            colors.Add(new MyColor(bytes[i, j + 1]));
            colors.Add(new MyColor(bytes[i + 1, j - 1]));
            colors.Add(new MyColor(bytes[i + 1, j]));
            colors.Add(new MyColor(bytes[i + 1, j + 1]));

            return new MyColor((byte)colors.Average(c => c.R), (byte)colors.Average(c => c.G), (byte)colors.Average(c => c.B)).Color;
        }
    }
}