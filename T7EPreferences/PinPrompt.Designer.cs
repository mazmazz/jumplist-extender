namespace T7EPreferences
{
    partial class PinPrompt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PinPrompt));
            this.TitleLabel = new System.Windows.Forms.Label();
            this.PinPictureBox = new System.Windows.Forms.PictureBox();
            this.DescLabel = new System.Windows.Forms.Label();
            this.RunButton = new System.Windows.Forms.Button();
            this.RunButtonPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.CloseButton = new System.Windows.Forms.Button();
            this.UnpinPictureBox = new System.Windows.Forms.PictureBox();
            this.HidePromptCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PinPictureBox)).BeginInit();
            this.RunButtonPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnpinPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.TitleLabel.Size = new System.Drawing.Size(344, 56);
            this.TitleLabel.TabIndex = 1;
            this.TitleLabel.Text = "Run the app and pin to taskbar to finish saving the jump list.";
            // 
            // PinPictureBox
            // 
            this.PinPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PinPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("PinPictureBox.Image")));
            this.PinPictureBox.Location = new System.Drawing.Point(0, 128);
            this.PinPictureBox.Name = "PinPictureBox";
            this.PinPictureBox.Size = new System.Drawing.Size(344, 164);
            this.PinPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PinPictureBox.TabIndex = 2;
            this.PinPictureBox.TabStop = false;
            // 
            // DescLabel
            // 
            this.DescLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.DescLabel.Location = new System.Drawing.Point(0, 56);
            this.DescLabel.Name = "DescLabel";
            this.DescLabel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.DescLabel.Size = new System.Drawing.Size(344, 39);
            this.DescLabel.TabIndex = 3;
            this.DescLabel.Text = "The app needs to be run first and then pinned to taskbar before the jump list com" +
    "mands will work.";
            // 
            // RunButton
            // 
            this.RunButton.AutoSize = true;
            this.RunButton.BackColor = System.Drawing.SystemColors.Control;
            this.RunButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RunButton.Location = new System.Drawing.Point(60, 0);
            this.RunButton.Margin = new System.Windows.Forms.Padding(2);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(224, 23);
            this.RunButton.TabIndex = 4;
            this.RunButton.Text = "Run {0}";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // RunButtonPanel
            // 
            this.RunButtonPanel.AutoSize = true;
            this.RunButtonPanel.Controls.Add(this.RunButton);
            this.RunButtonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.RunButtonPanel.Location = new System.Drawing.Point(0, 95);
            this.RunButtonPanel.Name = "RunButtonPanel";
            this.RunButtonPanel.Padding = new System.Windows.Forms.Padding(60, 0, 60, 10);
            this.RunButtonPanel.Size = new System.Drawing.Size(344, 33);
            this.RunButtonPanel.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.56977F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.43023F));
            this.tableLayoutPanel1.Controls.Add(this.CloseButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.HidePromptCheckBox, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 292);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(344, 49);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(256, 13);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(0, 0, 13, 13);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Cancel";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // UnpinPictureBox
            // 
            this.UnpinPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnpinPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("UnpinPictureBox.Image")));
            this.UnpinPictureBox.Location = new System.Drawing.Point(0, 128);
            this.UnpinPictureBox.Name = "UnpinPictureBox";
            this.UnpinPictureBox.Size = new System.Drawing.Size(344, 164);
            this.UnpinPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.UnpinPictureBox.TabIndex = 7;
            this.UnpinPictureBox.TabStop = false;
            this.UnpinPictureBox.Visible = false;
            // 
            // HidePromptCheckBox
            // 
            this.HidePromptCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.HidePromptCheckBox.AutoSize = true;
            this.HidePromptCheckBox.Enabled = false;
            this.HidePromptCheckBox.Location = new System.Drawing.Point(13, 17);
            this.HidePromptCheckBox.Margin = new System.Windows.Forms.Padding(13, 0, 0, 15);
            this.HidePromptCheckBox.Name = "HidePromptCheckBox";
            this.HidePromptCheckBox.Size = new System.Drawing.Size(172, 17);
            this.HidePromptCheckBox.TabIndex = 1;
            this.HidePromptCheckBox.Text = "Don\'t show this message again";
            this.HidePromptCheckBox.UseVisualStyleBackColor = true;
            this.HidePromptCheckBox.Visible = false;
            // 
            // PinPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(344, 341);
            this.Controls.Add(this.PinPictureBox);
            this.Controls.Add(this.UnpinPictureBox);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.RunButtonPanel);
            this.Controls.Add(this.DescLabel);
            this.Controls.Add(this.TitleLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PinPrompt";
            this.ShowInTaskbar = false;
            this.Text = "PinPrompt";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PinPrompt_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.PinPictureBox)).EndInit();
            this.RunButtonPanel.ResumeLayout(false);
            this.RunButtonPanel.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnpinPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.PictureBox PinPictureBox;
        private System.Windows.Forms.Label DescLabel;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Panel RunButtonPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.PictureBox UnpinPictureBox;
        private System.Windows.Forms.CheckBox HidePromptCheckBox;
    }
}