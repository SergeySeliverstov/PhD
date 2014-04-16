using System;
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
        public Form1()
        {
            InitializeComponent();

            double[] Ugs = new double[] { -3.5, -2.8, -2.1, -1.4, -0.7, 0, 0.7, 1.4, 2.1, 2.8, 3.5 };
            //double[] Ugs = new double[] { -12, -11.3, -10.6, -9.9, -9.2, -8.5, -7.8, -7.1, -6.4, -5.7, -5 };
            double[] k0 = new double[] { 0, 4.12, 16.1, 19, 19.8, 19.78, 19.52, 19.08, 18.5, 17.9, 17.32 };
            double shift = (Ugs[6] - Ugs[5]) / 100;

            int size = (int)Math.Round((Math.Abs(Ugs[10] - Ugs[0])) / shift) + 1;
            double[] x = new double[size];
            double[] y = new double[size];
            double[] y2 = new double[size];
            double[] y3 = new double[size];

            for (double u = Ugs[0]; u < Ugs[10]; u += shift)
            {
                int i = (int)Math.Round(Math.Abs(Ugs[0] - u) / shift);
                x[i] = u;
                y[i] = CurveTracer.B(Ugs, k0, u);
                y2[i] = CurveTracer.B2(Ugs, k0, u);
                y3[i] = y2[i] / (2 * y[i]);
            }

            pictureBox1.Image = (new MyImage(func(x, y))).Bitmap;
            pictureBox2.Image = (new MyImage(func(x, y3))).Bitmap;

            //pictureBox2.Image = (new MyImage(Tools.Tools.SumArrays(func(x, y), func(x, y2)))).Bitmap;

            xMin1.Text = Ugs[0].ToString("F");
            xMin2.Text = Ugs[0].ToString("F");
            xMax1.Text = Ugs[Ugs.Length - 1].ToString("F");
            xMax2.Text = Ugs[Ugs.Length - 1].ToString("F");

            yMin1.Text = y.Min().ToString("F");
            yMin2.Text = y3.Min().ToString("F");
            yMax1.Text = y.Max().ToString("F");
            yMax2.Text = y3.Max().ToString("F");
        }

        private int[,] func(double[] x, double[] y)
        {
            int size = x.Length;

            double xScale = (x.Max() - x.Min()) / size;
            double yScale = (y.Max() - y.Min()) / size;

            int[,] pic = new int[size + 1, size + 1];

            for (int i = 0; i < pic.GetLength(0); i++)
                for (int j = 0; j < pic.GetLength(0); j++)
                    pic[i, j] = 0xFFFFFF;

            for (int i = 0; i < pic.GetLength(0); i += 50)
                for (int j = 0; j < pic.GetLength(0); j++)
                    pic[i, j] = 0x888888;

            for (int i = 0; i < pic.GetLength(0); i++)
                for (int j = 0; j < pic.GetLength(0); j += 50)
                    pic[i, j] = 0x888888;

            for (int i = 0; i < x.Length; i++)
                pic[(int)((x[i] - x.Min()) / xScale), (int)((y.Max() - y[i]) / yScale)] = 0;

            return pic;
        }
    }
}
