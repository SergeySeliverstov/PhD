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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            richTextBox1.LoadFile("Docs\\MainText.rtf");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            new HelpForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new H3Form().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
