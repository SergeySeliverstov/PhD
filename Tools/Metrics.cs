﻿using System;
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
                int jk = k & (K - 1);

                double innerResult = 0;
                for (int i = iLength * ik / K; i < iLength * (ik + 1) / K; i++)
                    for (int j = jLength * jk / K; j < jLength * (jk + 1) / K; j++)
                    {
                        innerResult += Math.Pow(Math.Abs(originalImage[i, j] - modifiedImage[i, j]), gamma);
                    }

                result += Math.Pow(innerResult / Math.Pow(iLength * jLength, 2), 1 / (double)gamma);
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

            for (int i = center(w); i < iLength - center(w) + 1; i++)
                for (int j = center(w); j < jLength - center(w) + 1; j++)
                    result += Math.Pow(minFromWArea(cutArray(originalImage, i, j, w)), 2) + Math.Pow(minFromWArea(cutArray(modifiedImage, i, j, w)), 2);

            result = Math.Sqrt(result / (2 * Math.Pow(iLength * jLength - w, 2)));

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

        private static double minFromWArea(byte[,] array)
        {
            double result = double.MaxValue;

            int iCenter = center(array.GetLength(0));
            int jCenter = center(array.GetLength(1));

            for (int i = 0; i < iCenter; i++)
                for (int j = 0; j < jCenter; j++)
                    //if (i != iCenter && j != jCenter)
                    result = Math.Min(result, d(array[iCenter, jCenter], array[i, j]));

            return result;
        }

        private static double d(double a, double b)
        {
            return Math.Abs(a - b);
        }

        private static int center(int w)
        {
            return (int)Math.Floor((double)w / 2) + 1;
        }

        public static string GetUnifiedMetrics(MyImage myImage, MetricsMode mode = MetricsMode.Simple)
        {
            double r0 = Metrics.MM(myImage.OriginalImageR, myImage.ImageR, 10, 10);
            double g0 = Metrics.MM(myImage.OriginalImageG, myImage.ImageG, 10, 10);
            double b0 = Metrics.MM(myImage.OriginalImageB, myImage.ImageB, 10, 10);
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
    }
}
