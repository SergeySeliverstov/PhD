using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools;

namespace DataMining
{
    public class DataMining
    {
        private CountCollection pairs;
        private MyImage myImage;

        private int maxDepth;
        private int pollutePercent;
        private bool useMask;
        private int collectionMethod;
        private int template;
        private bool cropPixels;
        private int maxAccuracy;

        private int[,] pollutedImage;
        private bool[,] pollutedMask;
        private bool[,] pollutedMaskOriginal;

        public event EventHandler<ObjectEventArgs<int>> UpdateProgress;
        public event EventHandler<ObjectEventArgs<string>> UpdateLog;

        public MyImage MyImage
        {
            get
            {
                return myImage;
            }
            set
            {
                myImage = value;
                if (pollutedMask == null)
                    pollutedMask = new bool[myImage.ImageWidth, myImage.ImageHeight];
                if (pollutedMaskOriginal == null)
                    pollutedMaskOriginal = new bool[myImage.ImageWidth, myImage.ImageHeight];
            }
        }

        public int MaxDepth
        {
            get
            {
                return maxDepth;
            }
            set
            {
                maxDepth = value;
            }
        }

        public int PollutePercent
        {
            get
            {
                return pollutePercent;
            }
            set
            {
                pollutePercent = value;
            }
        }

        public int[,] PollutedImage
        {
            get
            {
                return pollutedImage;
            }
            set
            {
                pollutedImage = value;
            }
        }

        public bool[,] PollutedMask
        {
            get
            {
                return pollutedMask;
            }
            set
            {
                pollutedMask = value;
            }
        }

        public bool[,] PollutedMaskOriginal
        {
            get
            {
                return pollutedMaskOriginal;
            }
            set
            {
                pollutedMaskOriginal = value;
            }
        }

        public bool UseMask
        {
            get
            {
                return useMask;
            }
            set
            {
                useMask = value;
            }
        }

        public int CollectionMethod
        {
            get
            {
                return collectionMethod;
            }
            set
            {
                collectionMethod = value;
            }
        }

        public int Template
        {
            get
            {
                return template;
            }
            set
            {
                template = value;
            }
        }

        public bool CropPixels
        {
            get
            {
                return cropPixels;
            }
            set
            {
                cropPixels = value;
            }
        }

        public int MaxAccuracy
        {
            get
            {
                return maxAccuracy;
            }
            set
            {
                maxAccuracy = value;
            }
        }

        private void updateProgress(int value)
        {
            if (UpdateProgress != null)
                UpdateProgress.Invoke(this, new ObjectEventArgs<int>(value));
        }

        private void updateLog(string value)
        {
            if (UpdateLog != null)
                UpdateLog.Invoke(this, new ObjectEventArgs<string>(value));
        }

        public void Pollute()
        {
            pollutedMask = Tools.ImageTransform.Pollute(myImage.ImageBytes, pollutePercent, useMask);
            pollutedMaskOriginal = Tools.Tools.CopyArray<bool>(pollutedMask);
            pollutedImage = Tools.Tools.CopyArray<int>(myImage.ImageBytes);

            int count0 = 0;
            for (int i = 0; i < myImage.ImageWidth; i++)
                for (int j = 0; j < myImage.ImageHeight; j++)
                    if (pollutedMask[i, j])
                        count0++;

            updateLog(" --- Загрязнение --- \n");
            //updateLog("Сломал: " + count0 + "\n");
        }

        private int getDepthByTemplate()
        {
            switch (template)
            {
                case 0:
                    return maxDepth;
                case 1:
                    return 9;
                case 2:
                case 3:
                    return 5;
            }
            return 0;
        }

        public void CreatePairs()
        {
            switch (collectionMethod)
            {
                case 0:
                    pairs = new CountCollectionTree(getDepthByTemplate());
                    break;
                case 1:
                    pairs = new CountCollectionList(getDepthByTemplate());
                    break;
            }

            updateProgress(0);
            for (int i = 0; i < myImage.ImageWidth; i++)
                for (int j = 0; j < myImage.ImageHeight; j++)
                {
                    int[][] pixelsTemplate = null;
                    switch (template)
                    {
                        case 0:
                            pixelsTemplate = Templates.GetPixelsByTempate(myImage.ImageBytes, i, j, maxDepth, false, cropPixels);
                            break;
                        case 1:
                            pixelsTemplate = Templates.GetPixelsByTempateRectangle(myImage.ImageBytes, i, j, false, cropPixels);
                            break;
                        case 2:
                            pixelsTemplate = Templates.GetPixelsByTempateCross(myImage.ImageBytes, i, j, false, cropPixels);
                            break;
                        case 3:
                            pixelsTemplate = Templates.GetPixelsByTempateDiCross(myImage.ImageBytes, i, j, false, cropPixels);
                            break;
                    }
                    foreach (var values in pixelsTemplate)
                        pairs.AddItem(values);
                    updateProgress((int)(100 * (myImage.ImageHeight * i + j) / (myImage.ImageWidth * myImage.ImageHeight)));
                }
            updateProgress(100);
            updateLog("CreatePairs Done!\n");
        }

        public void RestoreImage()
        {
            if (pairs == null)
                return;

            for (int depth = maxDepth; depth > 2; depth--)
            {
                updateProgress(0);
                for (int i = 0; i < myImage.ImageWidth; i++)
                    for (int j = 0; j < myImage.ImageHeight; j++)
                    {
                        if (pollutedMask[i, j])
                        {
                            int[][] find = null;
                            switch (template)
                            {
                                case 0:
                                    find = Templates.GetPixelsByTempate(myImage.ImageBytes, i, j, depth, true, cropPixels);
                                    break;
                                case 1:
                                    find = Templates.GetPixelsByTempateRectangle(myImage.ImageBytes, i, j, true, cropPixels);
                                    break;
                                case 2:
                                    find = Templates.GetPixelsByTempateCross(myImage.ImageBytes, i, j, true, cropPixels);
                                    break;
                                case 3:
                                    find = Templates.GetPixelsByTempateDiCross(myImage.ImageBytes, i, j, true, cropPixels);
                                    break;
                            }
                            int? color = pairs.FindColor(find);
                            if (color != null)
                            {
                                myImage.ImageBytes[i, j] = color.Value;
                                pollutedMask[i, j] = false;
                            }
                        }
                        updateProgress((int)(100 * (myImage.ImageHeight * i + j) / (myImage.ImageWidth * myImage.ImageHeight)));
                    }
                updateProgress(100);
                updateLog(string.Format("RestoreImage Done Depth = {0}!\n", depth));
            }

            if (pollutedImage != null)
            {
                int count = 0;
                int count2 = 0;
                for (int i = 0; i < myImage.ImageWidth; i++)
                    for (int j = 0; j < myImage.ImageHeight; j++)
                    {
                        if (cropPixels)
                        {
                            if ((myImage.ImageBytes[i, j] & Tools.Consts.CropMask) != (myImage.OriginalImageBytes[i, j] & Tools.Consts.CropMask) &&
                                (pollutedImage[i, j] & Tools.Consts.CropMask) != (myImage.OriginalImageBytes[i, j] & Tools.Consts.CropMask))
                                count++;
                            if (pollutedMask[i, j])
                                count2++;
                        }
                        else
                        {
                            if (myImage.ImageBytes[i, j] != myImage.OriginalImageBytes[i, j] &&
                                pollutedImage[i, j] != myImage.OriginalImageBytes[i, j])
                                count++;
                            if (pollutedMask[i, j])
                                count2++;
                        }
                    }

                updateLog(" --- Восстановление --- \n");
                //updateLog("Не совпало: " + count + "\n");
                //updateLog("Не восстановил: " + count2 + "\n");
            }
        }

        public void FindPixels()
        {
            if (pairs == null)
                return;

            updateProgress(0);

            for (int i = 0; i < myImage.ImageWidth; i++)
                for (int j = 0; j < myImage.ImageHeight; j++)
                {
                    if (template == 0)
                    {
                        var pixelsTemplate5 = Templates.GetPixelsByTempate(myImage.ImageBytes, i, j, maxDepth, false, cropPixels);
                        var tt5 = pairs.FindItems(pixelsTemplate5);
                        if (tt5 == null || tt5.Count == 0)
                        {
                            continue;
                        }

                        var pixelsTemplate4 = Templates.GetPixelsByTempate(myImage.ImageBytes, i, j, maxDepth, true, cropPixels);
                        var tt4 = pairs.FindItems(pixelsTemplate4);

                        decimal acc = tt5.Sum(t => t.count);
                        decimal totalAcc = tt4.Sum(t => t.count);

                        if (totalAcc != 0)
                        {
                            if (acc / totalAcc < (decimal)maxAccuracy / 100)
                                pollutedMask[i, j] = true;
                        }
                    }
                    if (template == 1)
                    {
                        var pixelsTemplate5 = Templates.GetPixelsByTempateRectangle(myImage.ImageBytes, i, j, false, cropPixels);
                        var tt5 = pairs.FindItems(pixelsTemplate5);
                        if (tt5 == null || tt5.Count == 0)
                        {
                            continue;
                        }

                        var pixelsTemplate4 = Templates.GetPixelsByTempateRectangle(myImage.ImageBytes, i, j, true, cropPixels);
                        var tt4 = pairs.FindItems(pixelsTemplate4);

                        decimal acc = tt5.Sum(t => t.count);
                        decimal totalAcc = tt4.Sum(t => t.count);

                        if (totalAcc != 0)
                        {
                            if (acc / totalAcc < (decimal)maxAccuracy / 100)
                                pollutedMask[i, j] = true;
                        }
                    }
                    if (template == 2)
                    {
                        var pixelsTemplate5 = Templates.GetPixelsByTempateCross(myImage.ImageBytes, i, j, false, cropPixels);
                        var tt5 = pairs.FindItems(pixelsTemplate5);
                        if (tt5 == null || tt5.Count == 0)
                        {
                            continue;
                        }

                        var pixelsTemplate4 = Templates.GetPixelsByTempateCross(myImage.ImageBytes, i, j, true, cropPixels);
                        var tt4 = pairs.FindItems(pixelsTemplate4);

                        decimal acc = tt5.Sum(t => t.count);
                        decimal totalAcc = tt4.Sum(t => t.count);

                        if (totalAcc != 0)
                        {
                            if (acc / totalAcc < (decimal)maxAccuracy / 100)
                                pollutedMask[i, j] = true;
                        }
                    }
                    if (template == 3)
                    {
                        var pixelsTemplate5 = Templates.GetPixelsByTempateDiCross(myImage.ImageBytes, i, j, false, cropPixels);
                        var tt5 = pairs.FindItems(pixelsTemplate5);
                        if (tt5 == null || tt5.Count == 0)
                        {
                            continue;
                        }

                        var pixelsTemplate4 = Templates.GetPixelsByTempateDiCross(myImage.ImageBytes, i, j, true, cropPixels);
                        var tt4 = pairs.FindItems(pixelsTemplate4);

                        decimal acc = tt5.Sum(t => t.count);
                        decimal totalAcc = tt4.Sum(t => t.count);

                        if (totalAcc != 0)
                        {
                            if (acc / totalAcc < (decimal)maxAccuracy / 100)
                                pollutedMask[i, j] = true;
                        }
                    }

                    updateProgress((int)(100 * (myImage.ImageHeight * i + j) / (myImage.ImageWidth * myImage.ImageHeight)));
                }

            updateProgress(100);
            updateLog("FindPixels Done!\n");

            if (pollutedMask != null)
            {
                int count0 = 0;
                int count1 = 0;
                int count2 = 0;
                for (int i = 0; i < myImage.ImageWidth; i++)
                    for (int j = 0; j < myImage.ImageHeight; j++)
                    {
                        if (pollutedMaskOriginal[i, j])
                            count0++;
                        if (pollutedMaskOriginal[i, j] && !pollutedMask[i, j])
                            count1++;
                        if (pollutedMask[i, j] && !pollutedMaskOriginal[i, j])
                            count2++;
                    }

                updateLog(" --- Поиск точек --- \n");
                //updateLog("Нашел: " + count0 + "\n");
                //updateLog("Не нашел: " + count1 + "\n");
                //updateLog("Нашел не те: " + count2 + "\n");
            }
        }

        public void GetMetrics()
        {
            updateLog(Tools.Metrics.GetUnifiedMetrics(myImage, MetricsMode.Simple));
        }

        public string GetMetricsText(MetricsMode mode)
        {
            return Tools.Metrics.GetUnifiedMetrics(myImage, mode);
        }
    }
}
