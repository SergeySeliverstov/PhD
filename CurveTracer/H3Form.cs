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
    public partial class H3Form : Form
    {
        public double[] Ugs;
        public double[] k0;

        public H3Form()
        {
            InitializeComponent();

            Ugs = new double[] { -3.5, -2.8, -2.1, -1.4, -0.7, 0, 0.7, 1.4, 2.1, 2.8, 3.5 };
            //Ugs = new double[] { -12, -11.3, -10.6, -9.9, -9.2, -8.5, -7.8, -7.1, -6.4, -5.7, -5 };
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
            var multiplier = 100;
            var delta = Ugs[6] - Ugs[5];
            var shift = delta / multiplier;

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
                //if (y[i] > 0)// && x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                {
                    newY.Add(y[i]);
                    newX.Add(x[i]);
                }
            }

            var newY3 = new List<double>();
            var newX3 = new List<double>();
            for (var i = 0; i < x.Length; i++)
            {
                //if (x[i] >= x[1 * multiplier] && x[i] <= x[x.Length - 1 * multiplier - 1])
                {
                    newY3.Add(y3[i] > -k0.Max() && y3[i] < k0.Max() ? y3[i] : 0);
                    newX3.Add(x[i]);
                }
            }

            pictureBox1.Image = (new MyImage(FuncTools.FuncToBytesSum(newX.ToArray(), newY.ToArray(), newX3.ToArray(), newY3.ToArray(), delta))).Bitmap;

            xMin1.Text = Math.Min(newX.Min(), newX3.Min()).ToString("F");
            xMax1.Text = Math.Max(newX.Max(), newX3.Max()).ToString("F");
            yMin1.Text = Math.Min(newY.Min(), newY3.Min()).ToString("F");
            yMax1.Text = Math.Max(newY.Max(), newY3.Max()).ToString("F");
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            updateArrays();
            showGraph();
        }
    }
}
