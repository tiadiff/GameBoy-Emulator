namespace gbz
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            closeLabel = new Label();
            flatSlider1 = new FlatSlider();
            panel1 = new Panel();
            panel2 = new Panel();
            SuspendLayout();
            // 
            // closeLabel
            // 
            closeLabel.AutoSize = true;
            closeLabel.BackColor = Color.Transparent;
            closeLabel.Cursor = Cursors.Hand;
            closeLabel.Font = new Font("Courier New", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            closeLabel.ForeColor = Color.Gray;
            closeLabel.Location = new Point(21, 706);
            closeLabel.Name = "closeLabel";
            closeLabel.Size = new Size(26, 27);
            closeLabel.TabIndex = 0;
            closeLabel.Text = "X";
            closeLabel.Click += closeLabel_Click;
            // 
            // flatSlider1
            // 
            flatSlider1.BackColor = Color.Transparent;
            flatSlider1.Cursor = Cursors.Hand;
            flatSlider1.Location = new Point(346, 373);
            flatSlider1.Name = "flatSlider1";
            flatSlider1.Size = new Size(106, 20);
            flatSlider1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Location = new Point(113, 96);
            panel1.Name = "panel1";
            panel1.Size = new Size(265, 225);
            panel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.Cursor = Cursors.Hand;
            panel2.Location = new Point(59, 602);
            panel2.Name = "panel2";
            panel2.Size = new Size(72, 50);
            panel2.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            BackgroundImage = Properties.Resources.Gemini_Generated_Image_37dto437dto437dt;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(489, 754);
            ControlBox = false;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(flatSlider1);
            Controls.Add(closeLabel);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label closeLabel;
        private FlatSlider flatSlider1;
        private Panel panel1;
        private Panel panel2;
    }
}
