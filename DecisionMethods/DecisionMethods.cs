using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DecisionMethods
{
    public class DecisionMethods
    {
        private List<KeyValuePair<Tools.ColorChannel, int>> colors;

        private MyImage myImage;
        public MyImage MyImage
        {
            get
            {
                return myImage;
            }
        }

        private bool[,] pollutedMask;
        public bool[,] PollutedMask
        {
            get
            {
                return pollutedMask;
            }
        }

        public bool SaveInMask { get; set; }

        public DecisionMethods(int[,] imageBytes)
        {
            this.myImage = new MyImage(imageBytes);
            pollutedMask = new bool[myImage.ImageWidth, myImage.ImageHeight];
        }

        public DecisionMethods(MyImage myImage)
        {
            this.myImage = myImage;
            pollutedMask = new bool[myImage.ImageWidth, myImage.ImageHeight];
        }

        void checkColor(int i, int j)
        {
            var color = myImage.ImageBytes[i, j];
            if (SaveInMask && !pollutedMask[i, j] || !SaveInMask && color != Tools.Consts.MaskColor)
            {
                var r = (color & Tools.Consts.RedMask) >> 16;
                var g = (color & Tools.Consts.GreenMask) >> 8;
                var b = color & Tools.Consts.BlueMask;
                colors.Add(new KeyValuePair<Tools.ColorChannel, int>((r > g && r > b) ? Tools.ColorChannel.R : (g > b) ? Tools.ColorChannel.G : Tools.ColorChannel.B, color));
            }
        }

        public void Pollute(decimal percent)
        {
            pollutedMask = Tools.ImageTransform.Pollute(myImage.ImageBytes, percent, SaveInMask);
        }

        //public void FindPixels()
        //{
        //    for (int i = 2; i < myImage.ImageWidth - 2; i++)
        //        for (int j = 2; j < myImage.ImageHeight - 2; j++)
        //        {
        //            pollutedMask[i, j] = CheckForCriterion(myImage.ImageBytes, i, j) != Criterion.Good;
        //        }
        //}

        public void FindPixels(int m, double n, double k, bool color)
        {
            pollutedMask = FoundPixels.FindPixels(myImage.ImageBytes, m, n, k, color);
        }

        public MyImage RestoreImageByStatistics(Matrix criterions, List<Matrix> statistics)
        {
            MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

            int[,] newImageBytes = new int[myImage.ImageWidth, myImage.ImageHeight];
            for (int i = 2; i < myImage.ImageWidth - 2; i++)
                for (int j = 2; j < myImage.ImageHeight - 2; j++)
                    if (SaveInMask && pollutedMask[i, j] || !SaveInMask && myImage.ImageBytes[i, j] == Tools.Consts.MaskColor)
                    {
                        decimal min;
                        int minNumber;
                        //var criterion = CheckForCriterion(newImageBytes, i, j);
                        //fp.
                        var criterion = 4;
                        min = MatrixTools.FindMinDiag(statistics[(int)criterion], out minNumber);
                        newImageBytes[i, j] = restorePixel(i, j, minNumber);
                    }

            for (int i = 2; i < myImage.ImageWidth - 2; i++)
                for (int j = 2; j < myImage.ImageHeight - 2; j++)
                {
                    if (SaveInMask && pollutedMask[i, j] || !SaveInMask && myImage.ImageBytes[i, j] == Tools.Consts.MaskColor)
                    {
                        newImage.ImageBytes[i, j] = newImageBytes[i, j];
                    }
                }

            return newImage;
        }

        public MyImage RestoreImage(List<Matrix> matrix)
        {
            MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

            return newImage;
        }

        public MyImage RestoreImage(int method)
        {
            MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

            for (int x = 2; x < myImage.ImageWidth - 2; x++)
                for (int y = 2; y < myImage.ImageHeight - 2; y++)
                    if (pollutedMask[x, y])
                    {
                        newImage.ImageBytes[x, y] = restorePixel(x, y, method);
                    }

            return newImage;
        }

        private int restorePixel(int x, int y, int method)
        {
            colors = new List<KeyValuePair<Tools.ColorChannel, int>>();

            // 1 уровень
            checkColor(x - 1, y - 1);
            checkColor(x - 1, y);
            checkColor(x - 1, y + 1);
            checkColor(x, y - 1);
            checkColor(x, y + 1);
            checkColor(x + 1, y - 1);
            checkColor(x + 1, y);
            checkColor(x + 1, y + 1);

            // 2 уровень
            checkColor(x - 2, y - 2);
            checkColor(x - 2, y - 1);
            checkColor(x - 2, y);
            checkColor(x - 2, y + 1);
            checkColor(x - 2, y + 2);
            checkColor(x - 1, y - 2);
            checkColor(x - 1, y + 2);
            checkColor(x, y - 2);
            checkColor(x, y + 2);
            checkColor(x + 1, y - 2);
            checkColor(x + 1, y + 2);
            checkColor(x + 2, y - 2);
            checkColor(x + 2, y - 1);
            checkColor(x + 2, y);
            checkColor(x + 2, y + 1);
            checkColor(x + 2, y + 2);

            if (colors.Count() > 0)
            {
                var maxCount = colors.GroupBy(i => i.Key).Max(i => i.Count());
                var maxColor = colors.GroupBy(i => i.Key).FirstOrDefault(i => i.Count() == maxCount).Key;
                int newColor = 0;
                var mask = maxColor == Tools.ColorChannel.R ? Tools.Consts.RedMask : maxColor == Tools.ColorChannel.G ? Tools.Consts.GreenMask : Tools.Consts.BlueMask;
                switch (method)
                {
                    case 0:
                        newColor = colors.Where(i => i.Key == maxColor).FirstOrDefault().Value;
                        break;
                    case 1:
                        newColor = colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & mask).FirstOrDefault().Value;
                        break;
                    case 2:
                        newColor = colors.Where(i => i.Key == maxColor).OrderByDescending(i => i.Value & mask).FirstOrDefault().Value;
                        break;
                    case 3:
                        var newColor1 = new MyColor(colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & mask).FirstOrDefault().Value);
                        var newColor2 = new MyColor(colors.Where(i => i.Key == maxColor).OrderByDescending(i => i.Value & mask).FirstOrDefault().Value);
                        byte r = (byte)((newColor1.R + newColor2.R) >> 1);
                        byte g = (byte)((newColor1.G + newColor2.G) >> 1);
                        byte b = (byte)((newColor1.B + newColor2.B) >> 1);
                        newColor = new MyColor(r, g, b).Color;
                        break;
                    case 4:
                        var list = colors.Where(i => i.Key == maxColor).OrderBy(i => i.Value & mask).ToList();
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
            return myImage.ImageBytes[x, y];
        }

        public string GetMetrics(MetricsMode mode)
        {
            return Tools.Metrics.GetUnifiedMetrics(myImage, mode);
        }

        //private bool equal(int color1, int color2)
        //{
        //    return (color1 & Tools.Consts.CropMask) == (color2 & Tools.Consts.CropMask);
        //}

        //private bool equal(int color1, int color2, int color3)
        //{
        //    return equal(color1, color2) && equal(color2, color3);
        //}

        //public Criterion CheckForCriterion(int[,] a, int i, int j)
        //{
        //    var vLine1 = equal(a[i - 1, j - 1], a[i - 1, j], a[i - 1, j + 1]);
        //    var vLine2 = equal(a[i, j - 1], a[i, j], a[i, j + 1]);
        //    var vLine3 = equal(a[i + 1, j - 1], a[i + 1, j], a[i + 1, j + 1]);

        //    var hLine1 = equal(a[i - 1, j - 1], a[i, j - 1], a[i + 1, j - 1]);
        //    var hLine2 = equal(a[i - 1, j], a[i, j], a[i + 1, j]);
        //    var hLine3 = equal(a[i - 1, j + 1], a[i, j + 1], a[i + 1, j + 1]);

        //    var vLine2x = equal(a[i, j - 1], a[i, j + 1]);
        //    var hLine2x = equal(a[i - 1, j], a[i + 1, j]);

        //    // + + +
        //    // + + +
        //    // + + +
        //    if (vLine1 && vLine2 && vLine3 && hLine1 && hLine2 && hLine3)
        //        return Criterion.Good;

        //    // + + *    * * *    * + +    + + +
        //    // + + * or + + + or * + + or + + +
        //    // + + *    + + +    * + +    * * *
        //    if (vLine1 && vLine2 || hLine2 && hLine3 || hLine2 && hLine3 || hLine1 && hLine2)
        //        return Criterion.Good;

        //    // * + *    * * *
        //    // * + * or + + +
        //    // * + *    * * *
        //    if (vLine2 || hLine2)
        //        return Criterion.Good;

        //    // + + +
        //    // + X +
        //    // + + +
        //    if (vLine1 && vLine2x && vLine3 && hLine1 && hLine2x && hLine3)
        //        return Criterion.Fill;

        //    // + + *    * * *    * + +    + + +
        //    // + X * or + X + or * X + or + X +
        //    // + + *    + + +    * + +    * * *
        //    if (vLine1 && vLine2x || hLine2x && hLine3 || vLine2x && vLine3 || hLine1 && hLine2x)
        //        return Criterion.Border;

        //    // * + *    * * *
        //    // * X * or + X +
        //    // * + *    * * *
        //    if (vLine2x || hLine2x)
        //        return Criterion.Line;

        //    return Criterion.None;
        //}
    }
}
