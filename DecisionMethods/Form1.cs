﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Tools;

namespace DecisionMethods
{
    public partial class Form1 : Form
    {
        DecisionMethods dm;
        bool[,] savedMask;
        bool[,] savedMask2;

        private List<KeyValuePair<Tools.ColorChannel, int>> colors;

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
                    var myImage = new MyImage();
                    myImage.Bitmap = new Bitmap(ofd.FileName);
                    dm = new DecisionMethods(myImage);
                    dm.SaveInMask = cbUseMask.Checked;

                    ShowImage(pictureBox1, dm.MyImage.Bitmap);
                }
            }
        }

        private void ShowImage(PictureBox pictureBox, Image bmpImage)
        {
            pictureBox.Image = bmpImage;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tbLog.Text += "Original: " + Tools.Metrics.GetUnifiedMetrics(dm.MyImage);

            dm.SaveInMask = cbUseMask.Checked;
            dm.Pollute(nudPercent.Value, cbSaltAndPepper.Checked);
            savedMask = Tools.ArrayTools.CopyArray<bool>(dm.PollutedMask);
            ShowImage(pictureBox1, dm.MyImage.Bitmap);

            tbLog.Text += "Polluted: " + Tools.Metrics.GetUnifiedMetrics(dm.MyImage) + "\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //tbLog.Text += "Broken: " + Tools.Metrics.GetUnifiedMetrics(dm.MyImage) + "\n";

            var image4 = dm.RestorePixels((int)mRestore.Value, (double)nRestore.Value);
            ShowImage(pictureBox3, image4.Bitmap);
            tbLog.Text += "New method: " + Tools.Metrics.GetUnifiedMetrics(image4);

            var image5 = dm.RestorePixelsOld(3);
            //ShowImage(pictureBox3, image4.Bitmap);
            tbLog.Text += "Avg: " + Tools.Metrics.GetUnifiedMetrics(image5);

            var image6 = dm.RestorePixelsOld(5);
            ShowImage(pictureBox4, image6.Bitmap);
            tbLog.Text += "Sqrt: " + Tools.Metrics.GetUnifiedMetrics(image6);

            //var statisticsFileName = "Statistics.xml";
            //var optimizedStatisticsFileName = "OptimizedStatistics.xml";
            //var criterionsFileName = "Criterions.xml";
            //var optimizedCriterionsFileName = "OptimizedCriterions.xml";

            //var optimizedStatistics = XmlTools.Load<List<Matrix>>(optimizedStatisticsFileName);
            //var statistics = XmlTools.Load<List<Matrix>>(statisticsFileName);
            //var optimizedCriterions = XmlTools.Load<Matrix>(optimizedCriterionsFileName);
            //var criterions = XmlTools.Load<Matrix>(criterionsFileName);

            //var image6 = dm.RestoreImageByStatistics(criterions, statistics);
            //ShowImage(pictureBox8, image6.Bitmap);
            //tbLog.Text += "By Statistics: " + Tools.Metrics.GetUnifiedMetrics(image6);
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender is PictureBox)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ((PictureBox)sender).Image.Save(sfd.FileName);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dm.SaveInMask = cbUseMask.Checked;
            dm.FindPixels((int)m.Value, (double)n.Value, (double)k.Value, color.Checked);
            pictureBox2.Image = ImageTransform.BoolToBitmap(dm.PollutedMask);

            savedMask2 = Tools.ArrayTools.CopyArray<bool>(dm.PollutedMask);

            tbLog.Text += Tools.Metrics.MatrixDifference(savedMask, savedMask2, MetricsMode.Simple);
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }
    }
}
