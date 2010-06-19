namespace T7EPreferences
{
    partial class ImportForm
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
            this.ImportPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ImportOpenJumplistAddRadioButton = new System.Windows.Forms.RadioButton();
            this.ImportOpenJumplistReplaceRadioButton = new System.Windows.Forms.RadioButton();
            this.ImportOpenJumplistLabel = new System.Windows.Forms.Label();
            this.ImportOpenLabel = new System.Windows.Forms.Label();
            this.ImportNewLabel = new System.Windows.Forms.Label();
            this.ImportOpenRadioButton = new System.Windows.Forms.RadioButton();
            this.ImportNewRadioButton = new System.Windows.Forms.RadioButton();
            this.ImportLabel = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ImportPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ImportPanel
            // 
            this.ImportPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ImportPanel.Controls.Add(this.panel1);
            this.ImportPanel.Controls.Add(this.ImportOpenJumplistLabel);
            this.ImportPanel.Controls.Add(this.ImportOpenLabel);
            this.ImportPanel.Controls.Add(this.ImportNewLabel);
            this.ImportPanel.Controls.Add(this.ImportOpenRadioButton);
            this.ImportPanel.Controls.Add(this.ImportNewRadioButton);
            this.ImportPanel.Controls.Add(this.ImportLabel);
            this.ImportPanel.Location = new System.Drawing.Point(0, 0);
            this.ImportPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ImportPanel.Name = "ImportPanel";
            this.ImportPanel.Size = new System.Drawing.Size(419, 245);
            this.ImportPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ImportOpenJumplistAddRadioButton);
            this.panel1.Controls.Add(this.ImportOpenJumplistReplaceRadioButton);
            this.panel1.Location = new System.Drawing.Point(39, 181);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 51);
            this.panel1.TabIndex = 10;
            // 
            // ImportOpenJumplistAddRadioButton
            // 
            this.ImportOpenJumplistAddRadioButton.AutoSize = true;
            this.ImportOpenJumplistAddRadioButton.Checked = true;
            this.ImportOpenJumplistAddRadioButton.Location = new System.Drawing.Point(0, 0);
            this.ImportOpenJumplistAddRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.ImportOpenJumplistAddRadioButton.Name = "ImportOpenJumplistAddRadioButton";
            this.ImportOpenJumplistAddRadioButton.Size = new System.Drawing.Size(194, 21);
            this.ImportOpenJumplistAddRadioButton.TabIndex = 7;
            this.ImportOpenJumplistAddRadioButton.TabStop = true;
            this.ImportOpenJumplistAddRadioButton.Text = "Add to the current jumplist";
            this.ImportOpenJumplistAddRadioButton.UseVisualStyleBackColor = true;
            // 
            // ImportOpenJumplistReplaceRadioButton
            // 
            this.ImportOpenJumplistReplaceRadioButton.AutoSize = true;
            this.ImportOpenJumplistReplaceRadioButton.Location = new System.Drawing.Point(0, 28);
            this.ImportOpenJumplistReplaceRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.ImportOpenJumplistReplaceRadioButton.Name = "ImportOpenJumplistReplaceRadioButton";
            this.ImportOpenJumplistReplaceRadioButton.Size = new System.Drawing.Size(205, 21);
            this.ImportOpenJumplistReplaceRadioButton.TabIndex = 8;
            this.ImportOpenJumplistReplaceRadioButton.Text = "Replace the current jumplist";
            this.ImportOpenJumplistReplaceRadioButton.UseVisualStyleBackColor = true;
            this.ImportOpenJumplistReplaceRadioButton.CheckedChanged += new System.EventHandler(this.ImportOpenJumplistReplaceRadioButton_CheckedChanged);
            // 
            // ImportOpenJumplistLabel
            // 
            this.ImportOpenJumplistLabel.AutoSize = true;
            this.ImportOpenJumplistLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportOpenJumplistLabel.Location = new System.Drawing.Point(36, 156);
            this.ImportOpenJumplistLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ImportOpenJumplistLabel.Name = "ImportOpenJumplistLabel";
            this.ImportOpenJumplistLabel.Size = new System.Drawing.Size(67, 17);
            this.ImportOpenJumplistLabel.TabIndex = 9;
            this.ImportOpenJumplistLabel.Text = "Jumplist";
            // 
            // ImportOpenLabel
            // 
            this.ImportOpenLabel.Location = new System.Drawing.Point(36, 115);
            this.ImportOpenLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ImportOpenLabel.Name = "ImportOpenLabel";
            this.ImportOpenLabel.Size = new System.Drawing.Size(361, 41);
            this.ImportOpenLabel.TabIndex = 6;
            this.ImportOpenLabel.Text = "Import the pack into {0}, and choose how to import it below:";
            this.ImportOpenLabel.Click += new System.EventHandler(this.ImportOpenLabel_Click);
            // 
            // ImportNewLabel
            // 
            this.ImportNewLabel.AutoSize = true;
            this.ImportNewLabel.Location = new System.Drawing.Point(36, 60);
            this.ImportNewLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ImportNewLabel.Name = "ImportNewLabel";
            this.ImportNewLabel.Size = new System.Drawing.Size(301, 17);
            this.ImportNewLabel.TabIndex = 3;
            this.ImportNewLabel.Text = "Select a new program to import your pack into.";
            this.ImportNewLabel.Click += new System.EventHandler(this.ImportNewLabel_Click);
            // 
            // ImportOpenRadioButton
            // 
            this.ImportOpenRadioButton.AutoSize = true;
            this.ImportOpenRadioButton.Checked = true;
            this.ImportOpenRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportOpenRadioButton.Location = new System.Drawing.Point(15, 91);
            this.ImportOpenRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.ImportOpenRadioButton.Name = "ImportOpenRadioButton";
            this.ImportOpenRadioButton.Size = new System.Drawing.Size(309, 21);
            this.ImportOpenRadioButton.TabIndex = 2;
            this.ImportOpenRadioButton.TabStop = true;
            this.ImportOpenRadioButton.Text = "Import into the currently open program";
            this.ImportOpenRadioButton.UseVisualStyleBackColor = true;
            this.ImportOpenRadioButton.CheckedChanged += new System.EventHandler(this.ImportOpenRadioButton_CheckedChanged);
            // 
            // ImportNewRadioButton
            // 
            this.ImportNewRadioButton.AutoSize = true;
            this.ImportNewRadioButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportNewRadioButton.Location = new System.Drawing.Point(15, 36);
            this.ImportNewRadioButton.Margin = new System.Windows.Forms.Padding(2);
            this.ImportNewRadioButton.Name = "ImportNewRadioButton";
            this.ImportNewRadioButton.Size = new System.Drawing.Size(218, 21);
            this.ImportNewRadioButton.TabIndex = 1;
            this.ImportNewRadioButton.Text = "Import into a new program";
            this.ImportNewRadioButton.UseVisualStyleBackColor = true;
            // 
            // ImportLabel
            // 
            this.ImportLabel.AutoSize = true;
            this.ImportLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ImportLabel.Location = new System.Drawing.Point(11, 9);
            this.ImportLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ImportLabel.Name = "ImportLabel";
            this.ImportLabel.Size = new System.Drawing.Size(320, 20);
            this.ImportLabel.TabIndex = 0;
            this.ImportLabel.Text = "How do you want to import the pack?";
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(205, 261);
            this.OkButton.Margin = new System.Windows.Forms.Padding(2);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(94, 29);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(304, 261);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 29);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(411, 304);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ImportPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ImportForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import a Jumplist Pack";
            this.ImportPanel.ResumeLayout(false);
            this.ImportPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ImportPanel;
        private System.Windows.Forms.Label ImportLabel;
        private System.Windows.Forms.RadioButton ImportNewRadioButton;
        private System.Windows.Forms.Label ImportNewLabel;
        private System.Windows.Forms.RadioButton ImportOpenRadioButton;
        private System.Windows.Forms.Label ImportOpenLabel;
        private System.Windows.Forms.Label ImportOpenJumplistLabel;
        private System.Windows.Forms.RadioButton ImportOpenJumplistReplaceRadioButton;
        private System.Windows.Forms.RadioButton ImportOpenJumplistAddRadioButton;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Panel panel1;
    }
}