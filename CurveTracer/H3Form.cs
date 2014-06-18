using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tools;

namespace CurveTracer
{
    public partial class H3Form : Form
    {
        public double[] Ugs;
        public double[] k0;

        public H3Form()
        {
            InitializeComponent();

            //Ugs = new double[] { -3.5, -2.8, -2.1, -1.4, -0.7, 0, 0.7, 1.4, 2.1, 2.8, 3.5 };
            Ugs = new double[] { -12, -11.3, -10.6, -9.9, -9.2, -8.5, -7.8, -7.1, -6.4, -5.7, -5 };
            //Ugs = new double[] { 0.5, 0.52, 0.54, 0.56, 0.58, 0.6, 0.62, 0.64, 0.66, 0.68, 0.7 };
            k0 = new double[] { 0, 4.12, 16.1, 19, 19.8, 19.78, 19.52, 19.08, 18.5, 17.9, 17.32 };

            loadRows();
            updateRows();
            showGraph();
        }

        private void loadRows()
        {
            dgv.Rows.Add(2);

            dgv.Rows[0].Cells[0].Value = "Uзи";
            dgv.Rows[1].Cells[0].Value = "k0";
        }

        private void updateRows()
        {
            for (int i = 0; i < Ugs.Length; i++)
            {
                dgv.Rows[0].Cells[i + 1].Value = Ugs[i];
                dgv.Rows[1].Cells[i + 1].Value = k0[i];
            }
        }

        private void updateArrays()
        {
            try
            {
                for (int i = 0; i < Ugs.Length; i++)
                {
                    Ugs[i] = double.Parse(dgv.Rows[0].Cells[i + 1].Value.ToString());
                    k0[i] = double.Parse(dgv.Rows[1].Cells[i + 1].Value.ToString());
                }

                for (int i = 0; i < Ugs.Length - 1; i++)
                {
                    if (Math.Round(Ugs[i + 1] - Ugs[i]) != Math.Round(Ugs[1] - Ugs[0]))
                        throw new Exception("Шаг в Uзи должен быть одинаковым");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(string.Format("Необходимо ввести численные значения.\nРазделитель целой и дробной части - \"{0}\"", Application.CurrentCulture.NumberFormat.CurrencyDecimalSeparator));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showGraph()
        {
            Ugs = CurveTracer.Norm(Ugs);

            var multiplier = 100;
            var delta = Ugs[6] - Ugs[5];
            var shift = delta / multiplier;

            var size = (int)Math.Round((Math.Abs(Ugs[10] - Ugs[0])) / shift) + 1;
            var x = new double[size];
            var y = new double[size];
            var y1 = new double[size];
            var y2 = new double[size];
            var h2 = new double[size];
            var h3 = new double[size];

            for (var u = Ugs[0]; u < Ugs[10]; u += shift)
            {
                var i = (int)Math.Round(Math.Abs(Ugs[0] - u) / shift);
                x[i] = u;
                y[i] = CurveTracer.B(Ugs, k0, u, 0);
                y1[i] = CurveTracer.B(Ugs, k0, u, 1);
                y2[i] = CurveTracer.B(Ugs, k0, u, 2);
                h2[i] = y1[i] / (2 * y[i]);
                h3[i] = y2[i] / (2 * y[i]);

            }

            //var ko = new double[size];
            //for (var i=0 ; i < x.Length; i++ )
            //    ko[i] = y[i] + (1 / 4) * y2[i] + (1 / 16) * y2[i];

            var newH3x = new List<double>();
            var newH3y = new List<double>();
            for (var i = 0; i < x.Length; i++)
                if (h3[i] > -k0.Max() && h3[i] < k0.Max())
                {
                    newH3y.Add(h3[i]);
                    newH3x.Add(x[i]);
                }

            var newH2x = new List<double>();
            var newH2y = new List<double>();
            for (var i = 0; i < x.Length; i++)
                if (h2[i] > -k0.Max() && h2[i] < k0.Max())
                {
                    newH2y.Add(h2[i]);
                    newH2x.Add(x[i]);
                }

            double min = double.MinValue;
            int minPos = 0;
            for (var i = 0; i < x.Length; i++)
            {
                var value = y[i] - Math.Abs(h3[i]);
                if (value > min)
                {
                    min = value;
                    minPos = i;
                }
            }

            var vertMinX = new double[size];
            var vertMinY = new double[size];
            for (var i = 0; i < x.Length; i++)
            {
                vertMinX[i] = x[minPos];
                vertMinY[i] = -k0.Max() + i * 2 * k0.Max() / size;
            }

            var funcs1 = new Func[] { new Func(x, y), new Func(newH3x.ToArray(), newH3y.ToArray()), new Func(vertMinX, vertMinY) };
            var bitmap1 = FuncTools.CreateBitmap(funcs1);
            FuncTools.FuncsToLines(bitmap1, delta, funcs1);
            FuncTools.AddAxis(bitmap1, delta, funcs1);
            FuncTools.AddLabels(bitmap1, delta, funcs1);
            FuncTools.AddTextInPhisicalCoords(bitmap1, new Coord(funcs1), x[minPos], y[minPos], string.Format("B0: {0:F2}", y[minPos]));
            FuncTools.AddTextInPhisicalCoords(bitmap1, new Coord(funcs1), x[minPos], newH3y[minPos], string.Format("H3:  {0:F4}", newH3y[minPos]));
            FuncTools.AddTextInPhisicalCoords(bitmap1, new Coord(funcs1), x[minPos], y[minPos] - 3, string.Format("Uзи: {0:F2} ,В", x[minPos]));

            pictureBox1.Image = bitmap1;

            var funcs2 = new Func[] { new Func(x, y), new Func(newH3x.ToArray(), newH3y.ToArray()), new Func(newH2x.ToArray(), newH2y.ToArray()), new Func(vertMinX, vertMinY) };
            var bitmap2 = FuncTools.CreateBitmap(funcs2);
            FuncTools.FuncsToLines(bitmap2, delta, funcs2);
            FuncTools.AddAxis(bitmap2, delta, funcs2);
            FuncTools.AddLabels(bitmap2, delta, funcs2);
            FuncTools.AddTextInPhisicalCoords(bitmap2, new Coord(funcs2), x[minPos], y[minPos], string.Format("B0: {0:F2}", y[minPos]));
            FuncTools.AddTextInPhisicalCoords(bitmap2, new Coord(funcs2), x[minPos], newH3y[minPos], string.Format("H3:  {0:F4}", newH3y[minPos]));
            FuncTools.AddTextInPhisicalCoords(bitmap2, new Coord(funcs2), x[minPos], y[minPos] - 3, string.Format("Uзи: {0:F2} ,В", x[minPos]));
            FuncTools.AddTextInPhisicalCoords(bitmap2, new Coord(funcs2), x[minPos], newH2y[minPos] + 2, string.Format("H2: {0:F3} ,В", newH2y[minPos]));

            pictureBox2.Image = bitmap2;

            listBox1.Items.Clear();
            listBox1.Items.Add(new MyListBoxItem(Color.Green, "██ - B0"));
            listBox1.Items.Add(new MyListBoxItem(Color.Red, "██ - H3"));
            listBox1.Items.Add(new MyListBoxItem(Color.Blue, "██ - Опт"));

            listBox2.Items.Clear();
            listBox2.Items.Add(new MyListBoxItem(Color.Green, "██ - B0"));
            listBox2.Items.Add(new MyListBoxItem(Color.Red, "██ - H3"));
            listBox2.Items.Add(new MyListBoxItem(Color.Blue, "██ - H2"));
            listBox2.Items.Add(new MyListBoxItem(Color.Brown, "██ - Опт"));
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            MyListBoxItem item = listBox1.Items[e.Index] as MyListBoxItem;
            if (item != null)
                e.Graphics.DrawString(item.Message, listBox1.Font, new SolidBrush(item.ItemColor), 0, e.Index * listBox1.ItemHeight);
        }

        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            MyListBoxItem item = listBox2.Items[e.Index] as MyListBoxItem;
            if (item != null)
                e.Graphics.DrawString(item.Message, listBox1.Font, new SolidBrush(item.ItemColor), 0, e.Index * listBox1.ItemHeight);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "png";
            dialog.AddExtension = true;
            dialog.Filter = "Png Files(*.png)|*.png";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var fileName = dialog.FileName;
                if (tabControl1.SelectedTab == tabPage1)
                    pictureBox1.Image.Save(fileName, ImageFormat.Png);
                if (tabControl1.SelectedTab == tabPage2)
                    pictureBox2.Image.Save(fileName, ImageFormat.Png);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateArrays();
            showGraph();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new HelpForm();
            form.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
