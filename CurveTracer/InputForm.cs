using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CurveTracer
{
    public partial class InputForm : Form
    {
        public double[] Ugs;
        public double[] k0;

        public InputForm()
        {
            Ugs = new double[] { -3.5, -2.8, -2.1, -1.4, -0.7, 0, 0.7, 1.4, 2.1, 2.8, 3.5 };
            //Ugs = new double[] { -12, -11.3, -10.6, -9.9, -9.2, -8.5, -7.8, -7.1, -6.4, -5.7, -5 };
            k0 = new double[] { 0, 4.12, 16.1, 19, 19.8, 19.78, 19.52, 19.08, 18.5, 17.9, 17.32 };

            InitializeComponent();
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            dgv.Rows.Add(2);

            dgv.Rows[0].Cells[0].Value = "Uзи";
            dgv.Rows[1].Cells[0].Value = "k0";

            for (int i = 0; i < Ugs.Length; i++)
            {
                dgv.Rows[0].Cells[i + 1].Value = Ugs[i];
                dgv.Rows[1].Cells[i + 1].Value = k0[i];
            }
        }

        private void bCalc_Click(object sender, EventArgs e)
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

                var form = new Form1();

                form.Ugs = Ugs;
                form.k0 = k0;

                form.Show();
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
    }
}
