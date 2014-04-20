﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace CurveTracer
{
    public partial class Form1 : Form
    {
        public double[] Ugs;
        public double[] k0;
        int multiplier;
        double shift;

        public Form1()
        {
            InitializeComponent();
        }

        private int[,] funcSum(double[] x, double[] y, double[] x3, double[] y3)
        {
            var size = x.Length;

            var xMax = Math.Max(x.Max(), x3.Max());
            var xMin = Math.Min(x.Min(), x3.Min());

            var yMax = Math.Max(y.Max(), y3.Max());
            var yMin = Math.Min(y.Min(), y3.Min());

            var xScale = (xMax - xMin) / size;
            var yScale = (yMax - yMin) / size;

            var pic = new int[size + 1, size + 1];

            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0xFFFFFF;

            for (var i = 0; i < pic.GetLength(0); i += size / 20)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0x888888;

            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j += size / 20)
                    pic[i, j] = 0x888888;

            for (var i = 0; i < x.Length; i++)
            {
                pic[(int)Math.Floor((x[i] - xMin) / xScale), (int)Math.Floor((yMax - y[i]) / yScale)] = 0;
                pic[(int)Math.Floor((x[i] - xMin) / xScale), (int)Math.Floor((yMax - y3[i]) / yScale)] = 0;
            }

            return pic;
        }

        private int[,] func(double[] x, double[] y)
        {
            var size = x.Length;

            var xScale = (x.Max() - x.Min()) / size;
            var yScale = (y.Max() - y.Min()) / size;

            var pic = new int[size + 1, size + 1];

            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0xFFFFFF;

            for (var i = 0; i < pic.GetLength(0); i += size / 20)
                for (var j = 0; j < pic.GetLength(1); j++)
                    pic[i, j] = 0x888888;

            for (var i = 0; i < pic.GetLength(0); i++)
                for (var j = 0; j < pic.GetLength(1); j += size / 20)
                    pic[i, j] = 0x888888;

            var xMin = x.Min();
            var yMax = y.Max();

            for (var i = 0; i < x.Length; i++)
                pic[(int)Math.Floor((x[i] - xMin) / xScale), (int)Math.Floor((yMax - y[i]) / yScale)] = 0;

            return pic;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            multiplier = 100;
            shift = (Ugs[6] - Ugs[5]) / multiplier;

            var size = (int)Math.Round((Math.Abs(Ugs[10] - Ugs[0])) / shift) + 1;
            var x = new double[size];
            var y = new double[size];
            var y2 = new double[size];
            var y3 = new double[size];

            for (var u = Ugs[0]; u < Ugs[10]; u += shift)
            {
                var i = (int)Math.Round(Math.Abs(Ugs[0] - u) / shift);
                x[i] = u;
                y[i] = CurveTracer.B(Ugs, k0, u);
                y2[i] = CurveTracer.B2(Ugs, k0, u);
                y3[i] = y2[i] / (2 * y[i]);
            }

            var newY = new List<double>();
            var newX = new List<double>();
            for (var i = 0; i < x.Length; i++)
            {
                if (y[i] > 0 && x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                {
                    newY.Add(y[i]);
                    newX.Add(x[i]);
                }
            }

            var newY2 = new List<double>();
            var newX2 = new List<double>();
            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                {
                    newY2.Add(y2[i]);
                    newX2.Add(x[i]);
                }
            }

            var newY3 = new List<double>();
            var newX3 = new List<double>();
            for (var i = 0; i < x.Length; i++)
            {
                if (x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                {
                    newY3.Add(y3[i]);
                    newX3.Add(x[i]);
                }
            }

            pictureBox1.Image = (new MyImage(func(newX.ToArray(), newY.ToArray()))).Bitmap;
            pictureBox2.Image = (new MyImage(func(newX2.ToArray(), newY2.ToArray()))).Bitmap;
            pictureBox3.Image = (new MyImage(func(newX3.ToArray(), newY3.ToArray()))).Bitmap;
            pictureBox4.Image = (new MyImage(funcSum(newX.ToArray(), newY.ToArray(), newX3.ToArray(), newY3.ToArray()))).Bitmap;

            //pictureBox2.Image = (new MyImage(Tools.Tools.SumArrays(func(x, y), func(x, y2)))).Bitmap;

            xMin1.Text = newX.Min().ToString("F");
            xMax1.Text = newX.Max().ToString("F");
            yMin1.Text = newY.Min().ToString("F");
            yMax1.Text = newY.Max().ToString("F");

            xMin2.Text = newX2.Min().ToString("F");
            xMax2.Text = newX2.Max().ToString("F");
            yMin2.Text = newY2.Min().ToString("F");
            yMax2.Text = newY2.Max().ToString("F");

            xMin3.Text = newX3.Min().ToString("F");
            xMax3.Text = newX3.Max().ToString("F");
            yMin3.Text = newY3.Min().ToString("F");
            yMax3.Text = newY3.Max().ToString("F");

            xMin4.Text = Math.Min(newX.Min(), newX3.Min()).ToString("F");
            xMax4.Text = Math.Max(newX.Max(), newX3.Max()).ToString("F");
            yMin4.Text = Math.Min(newY.Min(), newY3.Min()).ToString("F");
            yMax4.Text = Math.Max(newY.Max(), newY3.Max()).ToString("F");
        }
    }
}