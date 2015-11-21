namespace T7EPreferences
{
    partial class ExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            this.ImportPanel = new System.Windows.Forms.Panel();
            this.ShareWebIcon = new System.Windows.Forms.PictureBox();
            this.ShareWebLabel = new System.Windows.Forms.Label();
            this.ShareWebCheckBox = new System.Windows.Forms.CheckBox();
            this.ExportUniversalLabel = new System.Windows.Forms.Label();
            this.ExportProgramSpecificLabel = new System.Windows.Forms.Label();
            this.ExportUniversalCheckBox = new System.Windows.Forms.RadioButton();
            this.ExportProgramSpecificCheckBox = new System.Windows.Forms.RadioButton();
            this.ExportLabel = new System.Windows.Forms.Label();
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ImportPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShareWebIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // ImportPanel
            // 
            this.ImportPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImportPanel.BackColor = System.Drawing.SystemColors.Window;
            this.ImportPanel.Controls.Add(this.ExportUniversalLabel);
            this.ImportPanel.Controls.Add(this.ExportProgramSpecificLabel);
            this.ImportPanel.Controls.Add(this.ExportUniversalCheckBox);
            this.ImportPanel.Controls.Add(this.ExportProgramSpecificCheckBox);
            this.ImportPanel.Controls.Add(this.ExportLabel);
            this.ImportPanel.Controls.Add(this.ShareWebIcon);
            this.ImportPanel.Controls.Add(this.ShareWebLabel);
            this.ImportPanel.Controls.Add(this.ShareWebCheckBox);
            this.ImportPanel.Location = new System.Drawing.Point(0, 0);
            this.ImportPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ImportPanel.Name = "ImportPanel";
            this.ImportPanel.Size = new System.Drawing.Size(335, 125);
            this.ImportPanel.TabIndex = 0;
            // 
            // ShareWebIcon
            // 
            this.ShareWebIcon.Enabled = false;
            this.ShareWebIcon.Image = ((System.Drawing.Image)(resources.GetObject("ShareWebIcon.Image")));
            this.ShareWebIcon.Location = new System.Drawing.Point(10, 150);
            this.ShareWebIcon.Margin = new System.Windows.Forms.Padding(2);
            this.ShareWebIcon.Name = "ShareWebIcon";
            this.ShareWebIcon.Size = new System.Drawing.Size(16, 16);
            this.ShareWebIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ShareWebIcon.TabIndex = 9;
            this.ShareWebIcon.TabStop = false;
            this.ShareWebIcon.Visible = false;
            this.ShareWebIcon.Click += new System.EventHandler(this.ShareWebLabel_Click);
            // 
            // ShareWebLabel
            // 
            this.ShareWebLabel.Enabled = false;
            this.ShareWebLabel.Location = new System.Drawing.Point(29, 144);
            this.ShareWebLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ShareWebLabel.Name = "ShareWebLabel";
            this.ShareWebLabel.Size = new System.Drawing.Size(290, 27);
            this.ShareWebLabel.TabIndex = 8;
            this.ShareWebLabel.Text = "Your pack will be made available for visitors to download and enjoy!";
            this.ShareWebLabel.Visible = false;
            this.ShareWebLabel.Click += new System.EventHandler(this.ShareWebLabel_Click);
            // 
            // ShareWebCheckBox
            // 
            this.ShareWebCheckBox.AutoSize = true;
            this.ShareWebCheckBox.Enabled = false;
            this.ShareWebCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShareWebCheckBox.Location = new System.Drawing.Point(12, 125);
            this.ShareWebCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.ShareWebCheckBox.Name = "ShareWebCheckBox";
            this.ShareWebCheckBox.Size = new System.Drawing.Size(248, 17);
            this.ShareWebCheckBox.TabIndex = 7;
            this.ShareWebCheckBox.Text = "Share your pack on the official website";
            this.ShareWebCheckBox.UseVisualStyleBackColor = true;
            this.ShareWebCheckBox.Visible = false;
            this.ShareWebCheckBox.CheckedChanged += new System.EventHandler(this.ShareWebCheckBox_CheckedChanged);
            // 
            // ExportUniversalLabel
            // 
            this.ExportUniversalLabel.AutoSize = true;
            this.ExportUniversalLabel.Location = new System.Drawing.Point(29, 95);
            this.ExportUniversalLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExportUniversalLabel.Name = "ExportUniversalLabel";
            this.ExportUniversalLabel.Size = new System.Drawing.Size(223, 13);
            this.ExportUniversalLabel.TabIndex = 6;
            this.ExportUniversalLabel.Text = "The pack will be compatible with all programs.";
            this.ExportUniversalLabel.Click += new System.EventHandler(this.ImportOpenLabel_Click);
            // 
            // ExportProgramSpecificLabel
            // 
            this.ExportProgramSpecificLabel.Location = new System.Drawing.Point(29, 47);
            this.ExportProgramSpecificLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExportProgramSpecificLabel.Name = "ExportProgramSpecificLabel";
            this.ExportProgramSpecificLabel.Size = new System.Drawing.Size(290, 27);
            this.ExportProgramSpecificLabel.TabIndex = 3;
            this.ExportProgramSpecificLabel.Text = "The pack will be compatible with {0} only.";
            this.ExportProgramSpecificLabel.Click += new System.EventHandler(this.ImportNewLabel_Click);
            // 
            // ExportUniversalCheckBox
            // 
            this.ExportUniversalCheckBox.AutoSize = true;
            this.ExportUniversalCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportUniversalCheckBox.Location = new System.Drawing.Point(12, 77);
            this.ExportUniversalCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.ExportUniversalCheckBox.Name = "ExportUniversalCheckBox";
            this.ExportUniversalCheckBox.Size = new System.Drawing.Size(176, 17);
            this.ExportUniversalCheckBox.TabIndex = 2;
            this.ExportUniversalCheckBox.TabStop = true;
            this.ExportUniversalCheckBox.Text = "Export as a universal pack";
            this.ExportUniversalCheckBox.UseVisualStyleBackColor = true;
            // 
            // ExportProgramSpecificCheckBox
            // 
            this.ExportProgramSpecificCheckBox.AutoSize = true;
            this.ExportProgramSpecificCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportProgramSpecificCheckBox.Location = new System.Drawing.Point(12, 29);
            this.ExportProgramSpecificCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.ExportProgramSpecificCheckBox.Name = "ExportProgramSpecificCheckBox";
            this.ExportProgramSpecificCheckBox.Size = new System.Drawing.Size(218, 17);
            this.ExportProgramSpecificCheckBox.TabIndex = 1;
            this.ExportProgramSpecificCheckBox.TabStop = true;
            this.ExportProgramSpecificCheckBox.Text = "Export as a program-specific pack";
            this.ExportProgramSpecificCheckBox.UseVisualStyleBackColor = true;
            this.ExportProgramSpecificCheckBox.CheckedChanged += new System.EventHandler(this.ExportProgramSpecificCheckBox_CheckedChanged);
            // 
            // ExportLabel
            // 
            this.ExportLabel.AutoSize = true;
            this.ExportLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportLabel.Location = new System.Drawing.Point(9, 7);
            this.ExportLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ExportLabel.Name = "ExportLabel";
            this.ExportLabel.Size = new System.Drawing.Size(275, 17);
            this.ExportLabel.TabIndex = 0;
            this.ExportLabel.Text = "How do you want to export the pack?";
            // 
            // OkButton
            // 
            this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OkButton.Location = new System.Drawing.Point(164, 138);
            this.OkButton.Margin = new System.Windows.Forms.Padding(2);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 1;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(243, 138);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 2;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(329, 172);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.ImportPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "ExportForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export a Jumplist Pack";
            this.ImportPanel.ResumeLayout(false);
            this.ImportPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShareWebIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ImportPanel;
        private System.Windows.Forms.Label ExportLabel;
        private System.Windows.Forms.RadioButton ExportProgramSpecificCheckBox;
        private System.Windows.Forms.Label ExportProgramSpecificLabel;
        private System.Windows.Forms.RadioButton ExportUniversalCheckBox;
        private System.Windows.Forms.Label ExportUniversalLabel;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label ShareWebLabel;
        private System.Windows.Forms.PictureBox ShareWebIcon;
        public System.Windows.Forms.CheckBox ShareWebCheckBox;
    }
}