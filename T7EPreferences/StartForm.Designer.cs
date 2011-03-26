namespace T7EPreferences
{
    partial class StartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.StartLabel = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExitPanel = new System.Windows.Forms.Panel();
            this.DonatePictureBox = new System.Windows.Forms.PictureBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.UpdateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importJumplistPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateCheckWorker = new System.ComponentModel.BackgroundWorker();
            this.StartImportButton = new T7ECommon.CommandLinkButton();
            this.StartOpenButton = new T7ECommon.CommandLinkButton();
            this.StartNewButton = new T7ECommon.CommandLinkButton();
            this.ExitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DonatePictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartLabel
            // 
            this.StartLabel.AutoSize = true;
            this.StartLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartLabel.Location = new System.Drawing.Point(11, 12);
            this.StartLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.StartLabel.Name = "StartLabel";
            this.StartLabel.Size = new System.Drawing.Size(270, 20);
            this.StartLabel.TabIndex = 0;
            this.StartLabel.Text = "Welcome to Jumplist Extender.";
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(305, 12);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(94, 29);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "&Close";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // ExitPanel
            // 
            this.ExitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ExitPanel.Controls.Add(this.DonatePictureBox);
            this.ExitPanel.Controls.Add(this.linkLabel1);
            this.ExitPanel.Controls.Add(this.ExitButton);
            this.ExitPanel.Controls.Add(this.UpdateLinkLabel);
            this.ExitPanel.Location = new System.Drawing.Point(0, 292);
            this.ExitPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ExitPanel.Name = "ExitPanel";
            this.ExitPanel.Size = new System.Drawing.Size(422, 68);
            this.ExitPanel.TabIndex = 3;
            // 
            // DonatePictureBox
            // 
            this.DonatePictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DonatePictureBox.Enabled = false;
            this.DonatePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("DonatePictureBox.Image")));
            this.DonatePictureBox.Location = new System.Drawing.Point(208, 14);
            this.DonatePictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.DonatePictureBox.Name = "DonatePictureBox";
            this.DonatePictureBox.Size = new System.Drawing.Size(92, 26);
            this.DonatePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DonatePictureBox.TabIndex = 3;
            this.DonatePictureBox.TabStop = false;
            this.DonatePictureBox.Visible = false;
            this.DonatePictureBox.Click += new System.EventHandler(this.DonatePictureBox_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 19);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(153, 17);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "&Visit the official website";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // UpdateLinkLabel
            // 
            this.UpdateLinkLabel.AutoSize = true;
            this.UpdateLinkLabel.Enabled = false;
            this.UpdateLinkLabel.Location = new System.Drawing.Point(12, 19);
            this.UpdateLinkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.UpdateLinkLabel.Name = "UpdateLinkLabel";
            this.UpdateLinkLabel.Size = new System.Drawing.Size(253, 17);
            this.UpdateLinkLabel.TabIndex = 2;
            this.UpdateLinkLabel.TabStop = true;
            this.UpdateLinkLabel.Text = "&Version {0} is released! &Download now.";
            this.UpdateLinkLabel.Visible = false;
            this.UpdateLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UpdateLinkLabel_LinkClicked);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(415, 26);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.importJumplistPackToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // importJumplistPackToolStripMenuItem
            // 
            this.importJumplistPackToolStripMenuItem.Name = "importJumplistPackToolStripMenuItem";
            this.importJumplistPackToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importJumplistPackToolStripMenuItem.Size = new System.Drawing.Size(258, 22);
            this.importJumplistPackToolStripMenuItem.Text = "Import Jumplist Pack";
            this.importJumplistPackToolStripMenuItem.Click += new System.EventHandler(this.importJumplistPackToolStripMenuItem_Click);
            // 
            // UpdateCheckWorker
            // 
            this.UpdateCheckWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UpdateCheckWorker_DoWork);
            // 
            // StartImportButton
            // 
            this.StartImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StartImportButton.BackColor = System.Drawing.Color.Transparent;
            this.StartImportButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.StartImportButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StartImportButton.HelpText = "Import into a new or existing program.";
            this.StartImportButton.Location = new System.Drawing.Point(11, 202);
            this.StartImportButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartImportButton.Name = "StartImportButton";
            this.StartImportButton.Size = new System.Drawing.Size(388, 75);
            this.StartImportButton.TabIndex = 2;
            this.StartImportButton.Text = "I&mport a jumplist pack";
            this.StartImportButton.UseVisualStyleBackColor = false;
            this.StartImportButton.Click += new System.EventHandler(this.StartImportButton_Click);
            // 
            // StartOpenButton
            // 
            this.StartOpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StartOpenButton.BackColor = System.Drawing.Color.Transparent;
            this.StartOpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.StartOpenButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StartOpenButton.HelpText = "Open from a list of previously modified programs.";
            this.StartOpenButton.Location = new System.Drawing.Point(11, 122);
            this.StartOpenButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartOpenButton.Name = "StartOpenButton";
            this.StartOpenButton.Size = new System.Drawing.Size(388, 75);
            this.StartOpenButton.TabIndex = 1;
            this.StartOpenButton.Text = "&Open a previously saved jumplist";
            this.StartOpenButton.UseVisualStyleBackColor = false;
            this.StartOpenButton.Click += new System.EventHandler(this.StartOpenButton_Click);
            // 
            // StartNewButton
            // 
            this.StartNewButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.StartNewButton.BackColor = System.Drawing.Color.Transparent;
            this.StartNewButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.StartNewButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StartNewButton.HelpText = "Start from a program shortcut or EXE file";
            this.StartNewButton.Location = new System.Drawing.Point(11, 42);
            this.StartNewButton.Margin = new System.Windows.Forms.Padding(2);
            this.StartNewButton.Name = "StartNewButton";
            this.StartNewButton.Size = new System.Drawing.Size(388, 75);
            this.StartNewButton.TabIndex = 0;
            this.StartNewButton.Text = "Start a &new jumplist";
            this.StartNewButton.UseVisualStyleBackColor = false;
            this.StartNewButton.Click += new System.EventHandler(this.StartNewButton_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(415, 348);
            this.Controls.Add(this.StartImportButton);
            this.Controls.Add(this.ExitPanel);
            this.Controls.Add(this.StartOpenButton);
            this.Controls.Add(this.StartLabel);
            this.Controls.Add(this.StartNewButton);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Jumplist Extender";
            this.Shown += new System.EventHandler(this.StartForm_Shown);
            this.ExitPanel.ResumeLayout(false);
            this.ExitPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DonatePictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private T7ECommon.CommandLinkButton StartNewButton;
        private System.Windows.Forms.Label StartLabel;
        private T7ECommon.CommandLinkButton StartOpenButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel ExitPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private T7ECommon.CommandLinkButton StartImportButton;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolStripMenuItem importJumplistPackToolStripMenuItem;
        private System.Windows.Forms.LinkLabel UpdateLinkLabel;
        private System.ComponentModel.BackgroundWorker UpdateCheckWorker;
        private System.Windows.Forms.PictureBox DonatePictureBox;
    }
}