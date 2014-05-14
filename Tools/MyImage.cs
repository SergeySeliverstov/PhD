using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace Tools
{
    public class MyImage
    {
        private int[,] imageBytes;
        private int[,] originalImageBytes;

        public MyImage()
        {
        }

        public MyImage(int[,] bytes, int[,] originalBytes = null)
        {
            imageBytes = ArrayTools.CopyArray<int>(bytes);

            if (originalBytes == null)
                originalImageBytes = new int[bytes.GetLength(0), bytes.GetLength(1)];
            else
                originalImageBytes = ArrayTools.CopyArray<int>(originalBytes);
        }


        public int ImageHeight
        {
            get { return imageBytes.GetLength(1); }
        }

        public int ImageWidth
        {
            get { return imageBytes.GetLength(0); }
        }

        public int[,] ImageBytes
        {
            get { return imageBytes; }
        }

        public int[,] OriginalImageBytes
        {
            get { return originalImageBytes; }
        }

        public byte[,] ImageR { get { return getChannel(imageBytes, ColorChannel.R); } }
        public byte[,] ImageG { get { return getChannel(imageBytes, ColorChannel.G); } }
        public byte[,] ImageB { get { return getChannel(imageBytes, ColorChannel.B); } }
        public byte[,] OriginalImageR { get { return getChannel(originalImageBytes, ColorChannel.R); } }
        public byte[,] OriginalImageG { get { return getChannel(originalImageBytes, ColorChannel.G); } }
        public byte[,] OriginalImageB { get { return getChannel(originalImageBytes, ColorChannel.B); } }

        private byte[,] getChannel(int[,] bytes, ColorChannel channel)
        {
            byte[,] result = new byte[bytes.GetLength(0), bytes.GetLength(1)];
            for (int j = 0; j < ImageWidth; j++)
                for (int i = 0; i < ImageHeight; i++)
                    switch (channel)
                    {
                        case ColorChannel.R:
                            result[j, i] = (byte)((bytes[j, i] >> 16) & 0xFF);
                            break;
                        case ColorChannel.G:
                            result[j, i] = (byte)((bytes[j, i] >> 8) & 0xFF);
                            break;
                        case ColorChannel.B:
                            result[j, i] = (byte)(bytes[j, i] & 0xFF);
                            break;
                    }
            return result;
        }

        public Bitmap Bitmap
        {
            set
            {
                if (value != null)
                {
                    Bitmap myImage = new Bitmap(value);
                    BitmapData imageData = myImage.LockBits(new Rectangle(0, 0, myImage.Width, myImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);
                    int stride = imageData.Stride;
                    IntPtr Scan0 = imageData.Scan0;

                    unsafe
                    {
                        byte* p = (byte*)(void*)Scan0;

                        int nOffset = stride - myImage.Width * 4;
                        int nWidth = myImage.Width;

                        imageBytes = new int[myImage.Width, myImage.Height];

                        for (int y = 0; y < myImage.Height; y++)
                        {
                            for (int x = 0; x < nWidth; x++)
                            {
                                ImageBytes[x, y] = (p[0] << 16) | (p[1] << 8) | p[2];
                                p += 4;
                            }
                            p += nOffset;
                        }
                    }

                    originalImageBytes = ArrayTools.CopyArray<int>(imageBytes);
                }
            }
            get
            {
                Bitmap b = new Bitmap(ImageWidth, ImageHeight, PixelFormat.Format32bppRgb);

                var BoundsRect = new Rectangle(0, 0, ImageWidth, ImageHeight);
                BitmapData bmpData = b.LockBits(BoundsRect,
                                                ImageLockMode.WriteOnly,
                                                b.PixelFormat);

                IntPtr ptr = bmpData.Scan0;

                int bytes = bmpData.Stride * b.Height;
                var rgbValues = new byte[bytes];

                for (int j = 0; j < ImageWidth; j++)
                    for (int i = 0; i < ImageHeight; i++)
                    {
                        rgbValues[4 * (i * ImageWidth + j)] = (byte)((imageBytes[j, i] >> 16) & 0xFF);
                        rgbValues[4 * (i * ImageWidth + j) + 1] = (byte)((imageBytes[j, i] >> 8) & 0xFF);
                        rgbValues[4 * (i * ImageWidth + j) + 2] = (byte)(imageBytes[j, i] & 0xFF);
                    }

                Marshal.Copy(rgbValues, 0, ptr, bytes);
                b.UnlockBits(bmpData);
                return b;
            }
        }
    }
}
