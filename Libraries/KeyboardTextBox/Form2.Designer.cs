namespace KeyboardTextBox
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBox1 = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(13, 84);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(267, 22);
            textBox1.TabIndex = 0;
            textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(textBox1_KeyDown);
            textBox1.Leave += new System.EventHandler(textBox1_Leave);
            textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(textBox1_KeyUp);
            textBox1.Enter += new System.EventHandler(textBox1_Enter);
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(13, 13);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(267, 22);
            textBox2.TabIndex = 1;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(textBox2);
            this.Controls.Add(textBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private static System.Windows.Forms.TextBox textBox2;
        private static System.Windows.Forms.TextBox textBox1;
    }
}