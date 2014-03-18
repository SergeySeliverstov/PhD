namespace DataMining
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.nudPollutePercent = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.nudMaxAcc = new System.Windows.Forms.NumericUpDown();
            this.cbCollectionMethod = new System.Windows.Forms.ComboBox();
            this.cbUseMask = new System.Windows.Forms.CheckBox();
            this.cbCropPixels = new System.Windows.Forms.CheckBox();
            this.button8 = new System.Windows.Forms.Button();
            this.cbTemplate = new System.Windows.Forms.ComboBox();
            this.button9 = new System.Windows.Forms.Button();
            this.cbMaxDepth = new System.Windows.Forms.ComboBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbLimit = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPollutePercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(459, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Open";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(459, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(91, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Pollute Image";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(445, 223);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(460, 154);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(91, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Create Pairs";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(460, 212);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(91, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Restore Image";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(459, 270);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(192, 200);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Text = "";
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 476);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(587, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 7;
            // 
            // nudPollutePercent
            // 
            this.nudPollutePercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudPollutePercent.Location = new System.Drawing.Point(561, 44);
            this.nudPollutePercent.Name = "nudPollutePercent";
            this.nudPollutePercent.Size = new System.Drawing.Size(91, 20);
            this.nudPollutePercent.TabIndex = 8;
            this.nudPollutePercent.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(608, 496);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 9;
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button7.Location = new System.Drawing.Point(460, 183);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(91, 23);
            this.button7.TabIndex = 10;
            this.button7.Text = "Find pixels";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // nudMaxAcc
            // 
            this.nudMaxAcc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudMaxAcc.Location = new System.Drawing.Point(562, 183);
            this.nudMaxAcc.Name = "nudMaxAcc";
            this.nudMaxAcc.Size = new System.Drawing.Size(91, 20);
            this.nudMaxAcc.TabIndex = 8;
            this.nudMaxAcc.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // cbCollectionMethod
            // 
            this.cbCollectionMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCollectionMethod.FormattingEnabled = true;
            this.cbCollectionMethod.Items.AddRange(new object[] {
            "Tree",
            "List"});
            this.cbCollectionMethod.Location = new System.Drawing.Point(459, 70);
            this.cbCollectionMethod.Name = "cbCollectionMethod";
            this.cbCollectionMethod.Size = new System.Drawing.Size(193, 21);
            this.cbCollectionMethod.TabIndex = 11;
            // 
            // cbUseMask
            // 
            this.cbUseMask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUseMask.AutoSize = true;
            this.cbUseMask.Location = new System.Drawing.Point(562, 158);
            this.cbUseMask.Name = "cbUseMask";
            this.cbUseMask.Size = new System.Drawing.Size(52, 17);
            this.cbUseMask.TabIndex = 12;
            this.cbUseMask.Text = "Mask";
            this.cbUseMask.UseVisualStyleBackColor = true;
            // 
            // cbCropPixels
            // 
            this.cbCropPixels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCropPixels.AutoSize = true;
            this.cbCropPixels.Checked = true;
            this.cbCropPixels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCropPixels.Location = new System.Drawing.Point(562, 216);
            this.cbCropPixels.Name = "cbCropPixels";
            this.cbCropPixels.Size = new System.Drawing.Size(78, 17);
            this.cbCropPixels.TabIndex = 13;
            this.cbCropPixels.Text = "Crop Pixels";
            this.cbCropPixels.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.Location = new System.Drawing.Point(561, 12);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(91, 23);
            this.button8.TabIndex = 14;
            this.button8.Text = "Save";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // cbTemplate
            // 
            this.cbTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbTemplate.FormattingEnabled = true;
            this.cbTemplate.Items.AddRange(new object[] {
            "Lines",
            "Rectangle",
            "Cross",
            "DiCross"});
            this.cbTemplate.Location = new System.Drawing.Point(459, 97);
            this.cbTemplate.Name = "cbTemplate";
            this.cbTemplate.Size = new System.Drawing.Size(193, 21);
            this.cbTemplate.TabIndex = 15;
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button9.Location = new System.Drawing.Point(460, 241);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(191, 23);
            this.button9.TabIndex = 16;
            this.button9.Text = "Give me some metrics!";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // cbMaxDepth
            // 
            this.cbMaxDepth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMaxDepth.FormattingEnabled = true;
            this.cbMaxDepth.Items.AddRange(new object[] {
            "5",
            "4",
            "3",
            "2"});
            this.cbMaxDepth.Location = new System.Drawing.Point(459, 124);
            this.cbMaxDepth.Name = "cbMaxDepth";
            this.cbMaxDepth.Size = new System.Drawing.Size(194, 21);
            this.cbMaxDepth.TabIndex = 17;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Location = new System.Drawing.Point(3, 232);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(445, 223);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 18;
            this.pictureBox2.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(451, 458);
            this.tableLayoutPanel1.TabIndex = 19;
            // 
            // cbLimit
            // 
            this.cbLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLimit.AutoSize = true;
            this.cbLimit.Location = new System.Drawing.Point(616, 158);
            this.cbLimit.Name = "cbLimit";
            this.cbLimit.Size = new System.Drawing.Size(47, 17);
            this.cbLimit.TabIndex = 12;
            this.cbLimit.Text = "Limit";
            this.cbLimit.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 526);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cbMaxDepth);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.cbTemplate);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.cbCropPixels);
            this.Controls.Add(this.cbLimit);
            this.Controls.Add(this.cbUseMask);
            this.Controls.Add(this.cbCollectionMethod);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudMaxAcc);
            this.Controls.Add(this.nudPollutePercent);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Mayorov-Zilbernagel";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPollutePercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.NumericUpDown nudPollutePercent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.NumericUpDown nudMaxAcc;
        private System.Windows.Forms.ComboBox cbCollectionMethod;
        private System.Windows.Forms.CheckBox cbUseMask;
        private System.Windows.Forms.CheckBox cbCropPixels;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ComboBox cbTemplate;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.ComboBox cbMaxDepth;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox cbLimit;
    }
}

