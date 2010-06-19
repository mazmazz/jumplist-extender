namespace NSISInstaller
{
    partial class Primary
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
            this.JumplistListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // JumplistListBox
            // 
            this.JumplistListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.JumplistListBox.FormattingEnabled = true;
            this.JumplistListBox.ItemHeight = 16;
            this.JumplistListBox.Location = new System.Drawing.Point(300, 0);
            this.JumplistListBox.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistListBox.Name = "JumplistListBox";
            this.JumplistListBox.Size = new System.Drawing.Size(250, 420);
            this.JumplistListBox.TabIndex = 1;
            // 
            // WinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 268);
            this.Controls.Add(this.JumplistListBox);
            this.Name = "WinForm";
            this.Text = "Jumplist Extender";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Shown += new System.EventHandler(this.WinForm_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox JumplistListBox;
    }
}