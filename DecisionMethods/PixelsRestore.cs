using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DecisionMethods
{
    public class PixelsRestore
    {
        List<KeyValuePair<Tools.ColorChannel, int>> colors;

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

        //private int restorePixel(int x, int y, int method)
        //{
        //    colors = new List<KeyValuePair<Tools.ColorChannel, int>>();

        //    // 1 уровень
        //    checkColor(x - 1, y - 1);
        //    checkColor(x - 1, y);
        //    checkColor(x - 1, y + 1);
        //    checkColor(x, y - 1);
        //    checkColor(x, y + 1);
        //    checkColor(x + 1, y - 1);
        //    checkColor(x + 1, y);
        //    checkColor(x + 1, y + 1);

        //    // 2 уровень
        //    checkColor(x - 2, y - 2);
        //    checkColor(x - 2, y - 1);
        //    checkColor(x - 2, y);
        //    checkColor(x - 2, y + 1);
        //    checkColor(x - 2, y + 2);
        //    checkColor(x - 1, y - 2);
        //    checkColor(x - 1, y + 2);
        //    checkColor(x, y - 2);
        //    checkColor(x, y + 2);
        //    checkColor(x + 1, y - 2);
        //    checkColor(x + 1, y + 2);
        //    checkColor(x + 2, y - 2);
        //    checkColor(x + 2, y - 1);
        //    checkColor(x + 2, y);
        //    checkColor(x + 2, y + 1);
        //    checkColor(x + 2, y + 2);

        //    if (colors.Count() > 0)
        //    {
        //        var maxCount = colors.GroupBy(i => i.Key).Max(i => i.Count());
        //        var maxColor = colors.GroupBy(i => i.Key).FirstOrDefault(i => i.Count() == maxCount).Key;
        //        int newColor = 0;
        //        var mask = maxColor == Tools.ColorChannel.R ? Tools.Consts.RedMask : maxColor == Tools.ColorChannel.G ? Tools.Consts.GreenMask : Tools.Consts.BlueMask;
        //        switch (method)
        //        {
        //            case 0:
        //                newColor = colors.Where(i => i.Key == maxColor).FirstOrDefault().Value;
        //                break;
        //            case 1:
        //                newColor = colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & mask).FirstOrDefault().Value;
        //                break;
        //            case 2:
        //                newColor = colors.Where(i => i.Key == maxColor).OrderByDescending(i => i.Value & mask).FirstOrDefault().Value;
        //                break;
        //            case 3:
        //                var newColor1 = new MyColor(colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & mask).FirstOrDefault().Value);
        //                var newColor2 = new MyColor(colors.Where(i => i.Key == maxColor).OrderByDescending(i => i.Value & mask).FirstOrDefault().Value);
        //                byte r = (byte)((newColor1.R + newColor2.R) >> 1);
        //                byte g = (byte)((newColor1.G + newColor2.G) >> 1);
        //                byte b = (byte)((newColor1.B + newColor2.B) >> 1);
        //                newColor = new MyColor(r, g, b).Color;
        //                break;
        //            case 4:
        //                var list = colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & mask).ToList();
        //                newColor = list[list.Count / 2 + (list.Count % 2 == 1 ? 0 : -1)].Value;
        //                break;
        //            case 5:
        //                int r1 = 0;
        //                int g1 = 0;
        //                int b1 = 0;

        //                var newColorsCollection = colors.Where(i => i.Key == maxColor).Select(i => i.Value).ToList();
        //                int count = newColorsCollection.Count;
        //                foreach (int color in newColorsCollection)
        //                {
        //                    var colorX = new MyColor(color);
        //                    r1 += colorX.R * colorX.R;
        //                    g1 += colorX.G * colorX.G;
        //                    b1 += colorX.B * colorX.B;
        //                }

        //                newColor = new MyColor((byte)(Math.Sqrt(r1 / count)), (byte)(Math.Sqrt(g1 / count)), (byte)(Math.Sqrt(b1 / count))).Color;
        //                break;
        //        }

        //        return newColor;
        //    }
        //    return myImage.ImageBytes[x, y];
        //}

        //void checkColor(int i, int j)
        //{
        //    var color = myImage.ImageBytes[i, j];
        //    if (SaveInMask && !pollutedMask[i, j] || !SaveInMask && color != Tools.Consts.MaskColor)
        //    {
        //        var r = (color & Tools.Consts.RedMask) >> 16;
        //        var g = (color & Tools.Consts.GreenMask) >> 8;
        //        var b = color & Tools.Consts.BlueMask;
        //        colors.Add(new KeyValuePair<Tools.ColorChannel, int>((r > g && r > b) ? Tools.ColorChannel.R : (g > b) ? Tools.ColorChannel.G : Tools.ColorChannel.B, color));
        //    }
        //}
    }
}
