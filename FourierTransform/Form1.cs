using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Tools;

namespace FourierTransform
{
    public partial class Form1 : Form
    {
        MyImage myImage;
        MyImage mySign;

        public Form1()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (((Control)sender).Name == "bOpen1")
                {
                    myImage = new MyImage();
                    myImage.Bitmap = new Bitmap(ofd.FileName);
                    ShowImage(pictureBox1, myImage.Bitmap);
                }
                if (((Control)sender).Name == "bOpen2")
                {
                    mySign = new MyImage();
                    mySign.Bitmap = new Bitmap(ofd.FileName);
                    ShowImage(pictureBox2, mySign.Bitmap);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                myImage.Bitmap.Save(sfd.FileName);
            }
        }

        private void ShowImage(PictureBox pictureBox, Image bmpImage)
        {
            pictureBox.Image = bmpImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Tools.ImageTransform.AddNoise(myImage.ImageBytes, 128);
            ShowImage(pictureBox1, myImage.Bitmap);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Tools.ImageTransform.AddNoise(myImage.ImageBytes, 0, 10);
            ShowImage(pictureBox1, myImage.Bitmap);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Tools.ImageTransform.Invert(myImage.ImageBytes);
            ShowImage(pictureBox1, myImage.Bitmap);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var signPicture = Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(Signs.CreateSignFromImage(mySign.ImageBytes, myImage.ImageWidth, myImage.ImageHeight)));

            if (signPicture != null)
            {
                ShowImage(pictureBox2, new MyImage(Tools.Conversion.ComplexToInt(signPicture)).Bitmap);

                var tt = Tools.Conversion.IntToComplex(myImage.ImageBytes);

                for (int i = 0; i < signPicture.GetLength(0); i++)
                    for (int j = 0; j < signPicture.GetLength(1); j++)
                        tt[i, j] += signPicture[i, j];

                var picture1 = Tools.Conversion.ComplexToInt(tt);

                ShowImage(pictureBox3, new MyImage(picture1).Bitmap);

                var yy = Tools.FourierTransform.IDFT2(Tools.Conversion.IntToComplex(picture1));
                //for (int i = 0; i < picture1.GetLength(0); i++)
                //    for (int j = 0; j < picture1.GetLength(1); j++)
                //        yy[i, j] *= 2000;

                var picture2 = Tools.Conversion.ComplexToInt(yy);
                for (int i = 0; i < picture1.GetLength(0); i++)
                    for (int j = 0; j < picture1.GetLength(1); j++)
                    {
                        MyColor color = new MyColor(picture2[i, j]);
                        color.R *= 100 % 256;
                        color.G *= 100 % 256;
                        color.B *= 100 % 256;
                        picture2[i, j] = color.Color;
                    }

                ShowImage(pictureBox4, new MyImage(picture2).Bitmap);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var signPicture = Signs.CreateSignFromImage(mySign.ImageBytes, myImage.ImageWidth, myImage.ImageHeight);
            if (signPicture != null)
            {
                ShowImage(pictureBox2, new MyImage(signPicture).Bitmap);

                var tt = Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(myImage.ImageBytes));

                for (int i = 0; i < signPicture.GetLength(0); i++)
                    for (int j = 0; j < signPicture.GetLength(1); j++)
                        tt[i, j] += signPicture[i, j];

                var picture1 = Tools.Conversion.ComplexToInt(Tools.FourierTransform.IDFT2(tt));

                ShowImage(pictureBox3, new MyImage(picture1).Bitmap);

                var yy = Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(picture1));
                //for (int i = 0; i < picture1.GetLength(0); i++)
                //    for (int j = 0; j < picture1.GetLength(1); j++)
                //        yy[i, j] *= 2000;

                var picture2 = Tools.Conversion.ComplexToInt(yy);
                for (int i = 0; i < picture1.GetLength(0); i++)
                    for (int j = 0; j < picture1.GetLength(1); j++)
                    {
                        MyColor color = new MyColor(picture2[i, j]);
                        color.R = (byte)(10 * color.R % 256);
                        color.G = (byte)(10 * color.G % 256);
                        color.B = (byte)(10 * color.B % 256);
                        picture2[i, j] = color.Color;
                    }

                ShowImage(pictureBox4, new MyImage(picture2).Bitmap);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tbLog.Text += Tools.Metrics.GetUnifiedMetrics(myImage, MetricsMode.Simple);
            tbLog.SelectionStart = tbLog.Text.Length;
            tbLog.ScrollToCaret();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var signPicture = Signs.CreateSignFromImagePerl(256, 1, mySign.ImageBytes, 0);
            if (signPicture != null)
            {
                //ShowImage(pictureBox3, new MyImage(signPicture).Bitmap);

                var picture = Tools.ImageTransform.TransposeRecreate(Tools.Conversion.ComplexToInt(Tools.FourierTransform.IDFT2(Tools.Conversion.IntToComplex(signPicture))));

                ShowImage(pictureBox3, new MyImage(picture).Bitmap);

                var picture2 = Tools.Conversion.ComplexToInt(Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(picture)));

                ShowImage(pictureBox4, new MyImage(picture2).Bitmap);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var signPicture = Signs.CreateSignFromImage(mySign.ImageBytes, 256, 256);
            if (signPicture != null)
            {
                //ShowImage(pictureBox2, new MyImage(signPicture).Bitmap);

                var picture = Tools.ImageTransform.TransposeRecreate(Tools.ImageTransform.ConvertToGrayscaleRecreate(Tools.Conversion.ComplexToInt(Tools.ImageTransform.DecreaseIntensive(Tools.FourierTransform.IDFT2(Tools.Conversion.IntToComplex(signPicture))))));

                ShowImage(pictureBox2, new MyImage(picture).Bitmap);

                var picture2 = new int[myImage.ImageWidth, myImage.ImageHeight];
                for (int i = 0; i < signPicture.GetLength(0); i++)
                    for (int j = 0; j < signPicture.GetLength(1); j++)
                        picture2[i, j] = picture[i, j] + myImage.ImageBytes[i, j];

                ShowImage(pictureBox3, new MyImage(picture2).Bitmap);

                var picture3 = Tools.Conversion.ComplexToInt(Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(picture2)));

                ShowImage(pictureBox4, new MyImage(picture3).Bitmap);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var signPicture = Signs.CreateSignFromImage(mySign.ImageBytes, 256, 256);
            if (signPicture != null)
            {
                var greyPicture = new MyImage(signPicture);
                Bitmap greyBitmap = AForgeTools.Conversion.MakeGrayscale3(greyPicture.Bitmap);
                greyPicture.Bitmap = greyBitmap;

                //Bitmap origBitmap = greyPicture.Bitmap;
                //Bitmap greyBitmap = new Bitmap(greyPicture.ImageWidth, greyPicture.ImageHeight, PixelFormat.Format8bppIndexed);
                //for (int x = 0; x < greyBitmap.Width; x++)
                //{
                //    for (int y = 0; y < greyBitmap.Height; y++)
                //    {
                //        Color pixelColor = greyBitmap.GetPixel(x, y);
                //        Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                //        greyBitmap.SetPixel(x, y, newColor); // Now greyscale
                //    }
                //}
                //greyPicture.Bitmap = greyBitmap;

                //ShowImage(pictureBox2, new MyImage(signPicture).Bitmap);
                var complexSign = AForgeTools.Conversion.IntToComplex(greyPicture.ImageBytes);
                AForge.Math.FourierTransform.DFT2(complexSign, AForge.Math.FourierTransform.Direction.Forward);
                var picture = AForgeTools.Conversion.ComplexToInt(complexSign);

                //var picture = Transforms.Transp(Transforms.ConvertToGrayscale(Tools.Conversion.ComplexToInt(Transforms.DecreaseIntensive(Tools.FourierTransform.IDFT2(Tools.Conversion.IntToComplex(signPicture))))));

                ShowImage(pictureBox2, new MyImage(picture).Bitmap);

                @AForge.Imaging.ComplexImage complexImage = AForge.Imaging.ComplexImage.FromBitmap(greyBitmap);
                complexImage.ForwardFourierTransform();
                ShowImage(pictureBox3, complexImage.ToBitmap());

                //var picture2 = new int[myImage.ImageWidth, myImage.ImageHeight];
                //for (int i = 0; i < signPicture.GetLength(0); i++)
                //    for (int j = 0; j < signPicture.GetLength(1); j++)
                //        picture2[i, j] = picture[i, j] + myImage.ImageBytes[i, j];

                //ShowImage(pictureBox3, new MyImage(picture2).Bitmap);

                //var picture3 = Tools.Conversion.ComplexToInt(Tools.FourierTransform.DFT2(Tools.Conversion.IntToComplex(picture2)));

                //ShowImage(pictureBox4, new MyImage(picture3).Bitmap);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var img = Tools.Tools.CopyArray<int>(myImage.ImageBytes);

            for (int i = 0; i < img.GetLength(0); i++)
                for (int j = 0; j < img.GetLength(1); j++)
                    img[i, j] *= (int)Math.Pow(-1, i + j);

            var imgComplex = Tools.Conversion.IntToComplex(img);
            var four = Tools.FourierTransform.DFT2(imgComplex);
            var fourInt = Tools.Conversion.ComplexToInt(four);
            pictureBox3.Image = new MyImage(fourInt).Bitmap;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var img = Tools.Tools.CopyArray<int>(myImage.ImageBytes);

            for (int i = 0; i < img.GetLength(0); i++)
                for (int j = 0; j < img.GetLength(1); j++)
                    img[i, j] *= (int)Math.Pow(-1, i + j);

            var imgComplex = Tools.Conversion.IntToComplex(img);
            var four = Tools.FourierTransform.DFT2(imgComplex);
            var fourInt = Tools.Conversion.ComplexToInt(four);
            pictureBox3.Image = new MyImage(fourInt).Bitmap;

            var fourComplex = Tools.Conversion.IntToComplex(fourInt);
            var imgComplex2 = Tools.FourierTransform.IDFT2(four);
            var img2 = Tools.Conversion.ComplexToInt(imgComplex2);

            for (int i = 0; i < img.GetLength(0); i++)
                for (int j = 0; j < img.GetLength(1); j++)
                    img2[i, j] *= (int)Math.Pow(-1, i + j);

            pictureBox4.Image = new MyImage(img2).Bitmap;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var img = Tools.Tools.CopyArray<int>(myImage.ImageBytes);

            for (int i = 0; i < img.GetLength(0); i++)
                for (int j = 0; j < img.GetLength(1); j++)
                    img[i, j] *= (int)Math.Pow(-1, i + j);

            var imgComplex = Tools.Conversion.IntToComplex(img);
            var four = Tools.FourierTransform.DFT2(imgComplex);
            var fourInt = Tools.Conversion.ComplexToInt(four);


            var sign = Signs.CreateSignFromImage(mySign.ImageBytes, img.GetLength(0), img.GetLength(1));

            var color = fourInt[img.GetLength(0) / 2, img.GetLength(1) / 2];
            for (int i = 0; i < img.GetLength(0); i++)
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    //fourInt[i, j] /= color;
                    fourInt[i, j] += sign[i, j];
                }

            pictureBox3.Image = new MyImage(fourInt).Bitmap;

            var fourComplex = Tools.Conversion.IntToComplex(fourInt);
            var imgComplex2 = Tools.FourierTransform.IDFT2(fourComplex);
            var img2 = Tools.Conversion.ComplexToInt(imgComplex2);

            for (int i = 0; i < img.GetLength(0); i++)
                for (int j = 0; j < img.GetLength(1); j++)
                    img2[i, j] *= (int)Math.Pow(-1, i + j);

            pictureBox4.Image = new MyImage(img2).Bitmap;
        }
    }
}
