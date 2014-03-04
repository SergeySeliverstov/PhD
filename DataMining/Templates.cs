using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataMining
{
    public class Templates
    {
        public static int[][] GetPixelsByTempate(int[,] imageBytes, int i, int j, int depth, bool skipCentralPixel, bool cropPixels)
        {
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);

            List<int[]> find = new List<int[]>();
            foreach (int k in new int[] { 0 })
            //foreach (int k in new int[] { 0, depth - 1 })
            //for (int k = 0; k < depth; k++)
            {
                if (i - k >= 0 && i - k + depth < imageWidth)
                {
                    int[] values = new int[depth - (skipCentralPixel ? 1 : 0)];
                    int y = 0;
                    for (int r = 0; r < depth; r++)
                        if (skipCentralPixel && r != k || !skipCentralPixel)
                            values[y++] = cropPixels ? imageBytes[i - k + r, j] & Tools.Consts.CropMask : imageBytes[i - k + r, j];

                    find.Add(values);
                }
                if (j - k >= 0 && j - k + depth < imageHeight)
                {
                    int[] values = new int[depth - (skipCentralPixel ? 1 : 0)];
                    int y = 0;
                    for (int r = 0; r < depth; r++)
                        if (skipCentralPixel && k != r || !skipCentralPixel)
                            values[y++] = cropPixels ? imageBytes[i, j - k + r] & Tools.Consts.CropMask : imageBytes[i, j - k + r];
                    find.Add(values);
                }
            }
            return find.ToArray();
        }

        public static int[][] GetPixelsByTempateRectangle(int[,] imageBytes, int i, int j, bool skipCentralPixel, bool cropPixels)
        {
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);

            if (i == 0 || j == 0 || i == imageHeight - 1 || j == imageWidth - 1)
                return new List<int[]>().ToArray();

            List<int[]> find = new List<int[]>();
            {
                int[] values = new int[8 + (skipCentralPixel ? 0 : 1)];

                values[0] = cropPixels ? imageBytes[i - 1, j - 1] & Tools.Consts.CropMask : imageBytes[i - 1, j - 1];
                values[1] = cropPixels ? imageBytes[i - 1, j] & Tools.Consts.CropMask : imageBytes[i - 1, j];
                values[2] = cropPixels ? imageBytes[i - 1, j + 1] & Tools.Consts.CropMask : imageBytes[i - 1, j + 1];
                values[3] = cropPixels ? imageBytes[i, j - 1] & Tools.Consts.CropMask : imageBytes[i, j - 1];
                values[4] = cropPixels ? imageBytes[i, j + 1] & Tools.Consts.CropMask : imageBytes[i, j + 1];
                values[5] = cropPixels ? imageBytes[i + 1, j - 1] & Tools.Consts.CropMask : imageBytes[i + 1, j - 1];
                values[6] = cropPixels ? imageBytes[i + 1, j] & Tools.Consts.CropMask : imageBytes[i + 1, j];
                values[7] = cropPixels ? imageBytes[i + 1, j + 1] & Tools.Consts.CropMask : imageBytes[i + 1, j + 1];
                if (!skipCentralPixel)
                    values[8] = cropPixels ? imageBytes[i, j] & Tools.Consts.CropMask : imageBytes[i, j];

                find.Add(values);
            }
            return find.ToArray();
        }

        public static int[][] GetPixelsByTempateCross(int[,] imageBytes, int i, int j, bool skipCentralPixel, bool cropPixels)
        {
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);

            if (i == 0 || j == 0 || i == imageHeight - 1 || j == imageWidth - 1)
                return new List<int[]>().ToArray();

            List<int[]> find = new List<int[]>();
            {
                int[] values = new int[4 + (skipCentralPixel ? 0 : 1)];

                values[0] = cropPixels ? imageBytes[i - 1, j] & Tools.Consts.CropMask : imageBytes[i - 1, j];
                values[1] = cropPixels ? imageBytes[i, j - 1] & Tools.Consts.CropMask : imageBytes[i, j - 1];
                values[2] = cropPixels ? imageBytes[i, j + 1] & Tools.Consts.CropMask : imageBytes[i, j + 1];
                values[3] = cropPixels ? imageBytes[i + 1, j] & Tools.Consts.CropMask : imageBytes[i + 1, j];
                if (!skipCentralPixel)
                    values[4] = cropPixels ? imageBytes[i, j] & Tools.Consts.CropMask : imageBytes[i, j];

                find.Add(values);
            }
            return find.ToArray();
        }

        public static int[][] GetPixelsByTempateDiCross(int[,] imageBytes, int i, int j, bool skipCentralPixel, bool cropPixels)
        {
            int imageWidth = imageBytes.GetLength(0);
            int imageHeight = imageBytes.GetLength(1);

            if (i == 0 || j == 0 || i == imageHeight - 1 || j == imageWidth - 1)
                return new List<int[]>().ToArray();

            List<int[]> find = new List<int[]>();
            {
                int[] values = new int[4 + (skipCentralPixel ? 0 : 1)];

                values[0] = cropPixels ? imageBytes[i - 1, j - 1] & Tools.Consts.CropMask : imageBytes[i - 1, j - 1];
                values[1] = cropPixels ? imageBytes[i - 1, j + 1] & Tools.Consts.CropMask : imageBytes[i - 1, j + 1];
                values[2] = cropPixels ? imageBytes[i + 1, j - 1] & Tools.Consts.CropMask : imageBytes[i + 1, j - 1];
                values[3] = cropPixels ? imageBytes[i + 1, j + 1] & Tools.Consts.CropMask : imageBytes[i + 1, j + 1];
                if (!skipCentralPixel)
                    values[4] = cropPixels ? imageBytes[i, j] & Tools.Consts.CropMask : imageBytes[i, j];

                find.Add(values);
            }
            return find.ToArray();
        }
    }
}
