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

        public void Pollute(decimal percent, bool saltPepper)
        {
            var tmpMask = Tools.ImageTransform.Pollute(myImage.ImageBytes, percent, saltPepper);
            if (SaveInMask)
                pollutedMask = tmpMask;
        }

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

        public MyImage RestorePixelsOld(int method)
        {
            MyImage newImage = new MyImage(myImage.ImageBytes, myImage.OriginalImageBytes);

            int[,] newImageBytes = PixelsRestore.FindPixelsOld(myImage.ImageBytes, pollutedMask, method);
            for (int i = 0; i < myImage.ImageWidth; i++)
                for (int j = 0; j < myImage.ImageHeight; j++)
                    newImage.ImageBytes[i, j] = newImageBytes[i, j];

            return newImage;
        }
        
        public string GetMetrics(MetricsMode mode)
        {
            return Tools.Metrics.GetUnifiedMetrics(myImage, mode);
        }        
    }
}
