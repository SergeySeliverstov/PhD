using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using Tools;

namespace DataMining
{
    public partial class Form1 : Form
    {
        DataMining dataMining;

        public Form1()
        {
            InitializeComponent();
            cbCollectionMethod.SelectedIndex = 0;
            cbTemplate.SelectedIndex = 0;
            cbMaxDepth.SelectedIndex = 2;
        }

        private void fillParameters()
        {
            if (dataMining != null)
            {
                dataMining.CollectionMethod = cbCollectionMethod.SelectedIndex;
                dataMining.Template = cbTemplate.SelectedIndex;
                dataMining.CropPixels = cbCropPixels.Checked;
                dataMining.MaxAccuracy = (int)nudMaxAcc.Value;
                dataMining.MaxDepth = int.Parse(cbMaxDepth.SelectedItem.ToString());
                dataMining.PollutePercent = (int)nudPollutePercent.Value;
                dataMining.UseMask = cbUseMask.Checked;
                dataMining.UseLimit = cbLimit.Checked;
                dataMining.WSM = cbWSM.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var myImage = new MyImage();
                myImage.Bitmap = new Bitmap(ofd.FileName);

                dataMining = new DataMining();
                dataMining.MyImage = myImage;
                dataMining.UpdateLog += (o, eo) =>
                {
                    richTextBox1.Text += eo.Object;

                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                };
                dataMining.UpdateProgress += (o, oe) =>
                {
                    progressBar1.Value = oe.Object;
                    Application.DoEvents();
                };

                fillParameters();

                ShowImage(myImage.Bitmap);
                ShowMask(dataMining.PollutedMask);
            }
        }

        private void ShowImage(Image bmpImage)
        {
            pictureBox1.Image = bmpImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fillParameters();
            dataMining.Pollute(cbSaltPepper.Checked);
            ShowImage(dataMining.MyImage.Bitmap);
            ShowMask(dataMining.PollutedMask);
        }

        private void ShowMask(bool[,] p)
        {

            int[,] mask_ = new int[p.GetLength(0), p.GetLength(1)];

            for (int i = 0; i < mask_.GetLength(0); i++)
                for (int j = 0; j < mask_.GetLength(1); j++)
                {
                    if (p[i, j])
                    {
                        mask_[i, j] = 0x00FF00;
                    }
                }
            MyImage mask_im = new MyImage(mask_);
            pictureBox2.Image = mask_im.Bitmap;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fillParameters();
            dataMining.CreatePairs();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fillParameters();
            dataMining.RestoreImage();
            ShowImage(dataMining.MyImage.Bitmap);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            fillParameters();
            dataMining.FindPixels();
            dataMining.GetPollutionStatistics();
            ShowMask(dataMining.PollutedMask);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += string.Format("Flags: M={0}, C={1}, W={2}\n", dataMining.UseMask ? 1 : 0, dataMining.CropPixels ? 1 : 0, dataMining.WSM ? 1 : 0);
            dataMining.GetMetrics();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.png|*.png";
            sfd.DefaultExt = "png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(sfd.FileName, ImageFormat.Png);
            }

        }
    }
}