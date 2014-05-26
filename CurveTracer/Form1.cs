using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools;

namespace CurveTracer
{
    public partial class Form1 : Form
    {
        public double[] Ugs;
        public double[] k0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var multiplier = 100;
            var delta = Ugs[6] - Ugs[5];
            var shift = delta / multiplier;

            var size = (int)Math.Round((Math.Abs(Ugs[10] - Ugs[0])) / shift) + 1;
            var x = new double[size];
            var y = new double[size];
            var y1 = new double[size];
            var y2 = new double[size];
            var h3 = new double[size];

            for (var u = Ugs[0]; u < Ugs[10]; u += shift)
            {
                var i = (int)Math.Round(Math.Abs(Ugs[0] - u) / shift);
                x[i] = u;
                y[i] = CurveTracer.B(Ugs, k0, u, 0);
                y1[i] = CurveTracer.B(Ugs, k0, u, 1);
                y2[i] = CurveTracer.B(Ugs, k0, u, 2);
                h3[i] = y2[i] / (2 * y[i]);
            }

            var newY = new List<double>();
            var newX = new List<double>();
            for (var i = 0; i < x.Length; i++)
            {
                //if (y1[i] > 0 && x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                if (x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                {
                    newY.Add(y1[i]);
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
                    newY3.Add(h3[i]);
                    newX3.Add(x[i]);
                }
            }

            pictureBox1.Image = (new MyImage(FuncTools.FuncsToBytes(delta, new Func(newX.ToArray(), newY.ToArray())))).Bitmap;
            pictureBox2.Image = (new MyImage(FuncTools.FuncsToBytes(delta, new Func(newX2.ToArray(), newY2.ToArray())))).Bitmap;
            pictureBox3.Image = (new MyImage(FuncTools.FuncsToBytes(delta, new Func(newX3.ToArray(), newY3.ToArray())))).Bitmap;
            pictureBox4.Image = (new MyImage(FuncTools.FuncsToBytes(delta, new Func(newX.ToArray(), newY.ToArray()), new Func(newX3.ToArray(), newY3.ToArray())))).Bitmap;

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
