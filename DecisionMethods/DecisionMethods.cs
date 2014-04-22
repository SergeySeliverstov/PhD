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

        public void Pollute(decimal percent, bool saltPepper)
        {
            var tmpMask = Tools.ImageTransform.Pollute(myImage.ImageBytes, percent, saltPepper);
            if (SaveInMask)
                pollutedMask = tmpMask;
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
            pollutedMask = PixelsSearching.FindPixels(myImage.ImageBytes, m, n, k, color);
        }

        public MyImage RestorePixels(int m, double n)
        {
            MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

            int[,] newImageBytes = PixelsRestore.FindPixels(myImage.ImageBytes, pollutedMask, m, n);
            for (int i = 0; i < myImage.ImageWidth; i++)
                for (int j = 0; j < myImage.ImageHeight; j++)
                    newImage.ImageBytes[i, j] = newImageBytes[i, j];

            return newImage;
        }

        //public MyImage RestoreImageByStatistics(Matrix criterions, List<Matrix> statistics)
        //{
        //    MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

        //    int[,] newImageBytes = new int[myImage.ImageWidth, myImage.ImageHeight];
        //    for (int i = 2; i < myImage.ImageWidth - 2; i++)
        //        for (int j = 2; j < myImage.ImageHeight - 2; j++)
        //            if (SaveInMask && pollutedMask[i, j] || !SaveInMask && myImage.ImageBytes[i, j] == Tools.Consts.MaskColor)
        //            {
        //                decimal min;
        //                int minNumber;
        //                //var criterion = CheckForCriterion(newImageBytes, i, j);
        //                //fp.
        //                var criterion = 4;
        //                min = MatrixTools.FindMinDiag(statistics[(int)criterion], out minNumber);
        //                newImageBytes[i, j] = restorePixel(i, j, minNumber);
        //            }

        //    for (int i = 2; i < myImage.ImageWidth - 2; i++)
        //        for (int j = 2; j < myImage.ImageHeight - 2; j++)
        //        {
        //            if (SaveInMask && pollutedMask[i, j] || !SaveInMask && myImage.ImageBytes[i, j] == Tools.Consts.MaskColor)
        //            {
        //                newImage.ImageBytes[i, j] = newImageBytes[i, j];
        //            }
        //        }

        //    return newImage;
        //}

        //public MyImage RestoreImage(List<Matrix> matrix)
        //{
        //    MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

        //    return newImage;
        //}

        //public MyImage RestoreImage(int method)
        //{
        //    MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

        //    for (int x = 2; x < myImage.ImageWidth - 2; x++)
        //        for (int y = 2; y < myImage.ImageHeight - 2; y++)
        //            if (pollutedMask[x, y])
        //            {
        //                newImage.ImageBytes[x, y] = restorePixel(x, y, method);
        //            }

        //    return newImage;
        //}
        
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
