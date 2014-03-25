using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public enum MetricsMode
    {
        Simple = 1,
        Detail = 2,
        CSVSimple = 3,
        CSVDetail = 4,
        MSEOnly = 5
    }

    public class Metrics
    {
        public static double MM(byte[,] originalImage, byte[,] modifiedImage, int K, int gamma)
        {
            double result = 0;

            int iLength = originalImage.GetLength(0);
            int jLength = originalImage.GetLength(1);

            for (int k = 0; k < K * K; k++)
            {
                int ik = k / K;
                int jk = k % K;

                double innerResult = 0;
                for (int i = iLength * ik / K; i < iLength * (ik + 1) / K; i++)
                    for (int j = jLength * jk / K; j < jLength * (jk + 1) / K; j++)
                    {
                        innerResult += Math.Pow(d(originalImage[i, j], modifiedImage[i, j]), gamma);
                    }

                result += Math.Pow(innerResult / (iLength * jLength), 1 / (double)gamma);
            }

            return result / K;
        }

        public static double MSE(byte[,] originalImage, byte[,] modifiedImage)
        {
            return MM(originalImage, modifiedImage, 1, 2);
        }

        public static double DON(byte[,] originalImage, byte[,] modifiedImage, int w)
        {
            double result = 0;

            int iLength = originalImage.GetLength(0);
            int jLength = originalImage.GetLength(1);

            for (int i = center(w); i < iLength - center(w); i += w)
                for (int j = center(w); j < jLength - center(w); j += w)
                {
                    var A = minFromWArea(cutArray(originalImage, i, j, w), modifiedImage[i, j]);
                    var B = minFromWArea(cutArray(modifiedImage, i, j, w), originalImage[i, j]);
                    result += Math.Pow(A, 2) + Math.Pow(B, 2);
                }

            result = Math.Sqrt(result / (double)(2 * (iLength - w) * (jLength - w)));

            return result;
        }

        private static byte[,] cutArray(byte[,] array, int iCenter, int jCenter, int w)
        {
            byte[,] resultArray = new byte[w, w];

            for (int i = 0; i < w; i++)
                for (int j = 0; j < w; j++)
                    resultArray[i, j] = array[iCenter + i - center(w), jCenter + j - center(w)];

            return resultArray;
        }

        private static double minFromWArea(byte[,] array, byte value)
        {
            double result = double.MaxValue;

            int iCenter = center(array.GetLength(0));
            int jCenter = center(array.GetLength(1));

            for (int i = 0; i <= iCenter; i++)
                for (int j = 0; j <= jCenter; j++)
                    result = Math.Min(result, d(value, array[i, j]));

            return result;
        }

        private static double d(double a, double b)
        {
            return Math.Abs(a - b);
        }

        private static int center(int w)
        {
            return (int)Math.Floor((double)w / 2);
        }

        public static string GetUnifiedMetrics(MyImage myImage, MetricsMode mode = MetricsMode.Simple)
        {
            double r0 = Metrics.MM(myImage.OriginalImageR, myImage.ImageR, 3, 9);
            double g0 = Metrics.MM(myImage.OriginalImageG, myImage.ImageG, 3, 9);
            double b0 = Metrics.MM(myImage.OriginalImageB, myImage.ImageB, 3, 9);
            double r1 = Metrics.MSE(myImage.OriginalImageR, myImage.ImageR);
            double g1 = Metrics.MSE(myImage.OriginalImageG, myImage.ImageG);
            double b1 = Metrics.MSE(myImage.OriginalImageB, myImage.ImageB);
            double r2 = Metrics.DON(myImage.OriginalImageR, myImage.ImageR, 5);
            double g2 = Metrics.DON(myImage.OriginalImageG, myImage.ImageG, 5);
            double b2 = Metrics.DON(myImage.OriginalImageB, myImage.ImageB, 5);

            string text = string.Empty;
            switch (mode)
            {
                case MetricsMode.Simple:
                    text += string.Format("MM = {0}\r\n", (r0 + g0 + b0) / 3);
                    text += string.Format("MSE = {0}\r\n", (r1 + g1 + b1) / 3);
                    text += string.Format("DON = {0}\r\n", (r2 + g2 + b2) / 3);
                    text += "\r\n";
                    break;
                case MetricsMode.Detail:
                    text += string.Format("MM:\r\nR = {0}\r\nG = {1}\r\nB = {2}\r\n", r0, g0, b0);
                    text += string.Format("MSE:\r\nR = {0}\r\nG = {1}\r\nB = {2}\r\n", r1, g1, b1);
                    text += string.Format("DON:\r\nR = {0}\r\nG = {1}\r\nB = {2}\r\n", r2, g2, b2);
                    text += "\r\n";
                    break;
                case MetricsMode.CSVSimple:
                    text += string.Join(Consts.CSVDivider, (r0 + g0 + b0) / 3, (r1 + g1 + b1) / 3, (r2 + g2 + b2) / 3);
                    break;
                case MetricsMode.CSVDetail:
                    text += string.Join(Consts.CSVDivider, r0, g0, b0, r1, g1, b1, r2, g2, b2);
                    break;
                case MetricsMode.MSEOnly:
                    text += (r1 + g1 + b1) / 3;
                    break;
            }

            return text;
        }

        public static string MatrixDifference(bool[,] array1, bool[,] array2, MetricsMode mode)
        {
            if (array1.GetLength(0) != array2.GetLength(0) || array1.GetLength(1) != array2.GetLength(1))
                return "Wrong dimmensions";

            int broken = 0;
            int found = 0;
            int match = 0;
            int notFound = 0;
            int wrongFound = 0;
            for (int i = 0; i < array1.GetLength(0); i++)
                for (int j = 0; j < array1.GetLength(1); j++)
                {
                    if (array1[i, j])
                        broken++;
                    if (array2[i, j])
                        found++;
                    if (array1[i, j] && array2[i, j])
                        match++;
                    if (array1[i, j] && !array2[i, j])
                        notFound++;
                    if (!array1[i, j] && array2[i, j])
                        wrongFound++;
                }

            if (mode == MetricsMode.CSVDetail || mode == MetricsMode.CSVSimple)
                return string.Join(Consts.CSVDivider, broken, found, match, notFound, wrongFound);
            else
                return string.Format("Broken: {0}\r\nFound {7}\r\n\r\nMatch: {1} ({4:N02}%)\r\nNot found: {2} ({5:N02}%)\r\nWrong found: {3} ({6:N02}%)\r\n", broken, match, notFound, wrongFound, 100 * (double)match / broken, 100 * (double)notFound / broken, 100 * (double)wrongFound / found, found);
        }
    }
}
