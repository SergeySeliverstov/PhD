using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Ugs = new double[] { -3.5, -2.8, -2.1, -1.4, -0.7, 0, 0.7, 1.4, 2.1, 2.8, 3.5 };
            //Ugs = new double[] { -12, -11.3, -10.6, -9.9, -9.2, -8.5, -7.8, -7.1, -6.4, -5.7, -5 };
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

            var newH3 = new List<double>();
            for (var i = 0; i < x.Length; i++)
                newH3.Add(h3[i] > -k0.Max() && h3[i] < k0.Max() ? h3[i] : 0);

            var newH2 = new List<double>();
            for (var i = 0; i < x.Length; i++)
                newH2.Add(h2[i] > -k0.Max() && h2[i] < k0.Max() ? h2[i] : 0);

            double minX = 0;
            double min = double.MinValue;
            for (var i = 0; i < x.Length; i++)
            {
                var value = y[i] - Math.Abs(h3[i]);
                if (value > min)
                {
                    min = value;
                    minX = x[i];
                }
            }

            var vertMinX = new double[size];
            var vertMinY = new double[size];
            for (var i = 0; i < x.Length; i++)
            {
                vertMinX[i] = minX;
                vertMinY[i] = -k0.Max() + i * 2 * k0.Max() / size;
            }

            pictureBox1.Image = (new MyImage(FuncTools.FuncsToBytes(delta, new Func(x, y), new Func(x, newH3.ToArray()), new Func(vertMinX, vertMinY)))).Bitmap;
            pictureBox2.Image = (new MyImage(FuncTools.FuncsToBytes(delta, new Func(x, y), new Func(x, newH3.ToArray()), new Func(x, newH2.ToArray())))).Bitmap;

            x1.Text = Ugs[0].ToString("F1");
            x2.Text = Ugs[1].ToString("F1");
            x3.Text = Ugs[2].ToString("F1");
            x4.Text = Ugs[3].ToString("F1");
            x5.Text = Ugs[4].ToString("F1");
            x6.Text = Ugs[5].ToString("F1");
            x7.Text = Ugs[6].ToString("F1");
            x8.Text = Ugs[7].ToString("F1");
            x9.Text = Ugs[8].ToString("F1");
            x10.Text = Ugs[9].ToString("F1");
            x11.Text = Ugs[10].ToString("F1");

            yMin1.Text = Math.Min(y.Min(), newH3.Min()).ToString("F");
            yMax1.Text = Math.Max(y.Max(), newH3.Max()).ToString("F");

            yMin2.Text = Math.Min(y.Min(), y1.Min()).ToString("F");
            yMax2.Text = Math.Max(y.Max(), y1.Max()).ToString("F");

            listBox1.Items.Clear();
            listBox1.Items.Add(new MyListBoxItem(Color.Green, "██ - K0"));
            listBox1.Items.Add(new MyListBoxItem(Color.Red, "██ - H3"));
            listBox1.Items.Add(new MyListBoxItem(Color.Blue, "██ - Опт"));

            listBox2.Items.Clear();
            listBox2.Items.Add(new MyListBoxItem(Color.Green, "██ - K0"));
            listBox2.Items.Add(new MyListBoxItem(Color.Red, "██ - H3"));
            listBox2.Items.Add(new MyListBoxItem(Color.Blue, "██ - H2"));
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
    }
}
