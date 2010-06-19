namespace T7EPreferences
{
    partial class ManageForm
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Programs", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Disabled Programs", System.Windows.Forms.HorizontalAlignment.Left);
            this.ManageLabel = new System.Windows.Forms.Label();
            this.ManageListView = new System.Windows.Forms.ListView();
            this.ManageAppNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ManageAppPathColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ManageAppIdColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ManageDisableButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.ExitPanel = new System.Windows.Forms.Panel();
            this.FileOpenButton = new System.Windows.Forms.Button();
            this.ManageDeleteWideButton = new System.Windows.Forms.Button();
            this.ManagePanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ManageImportButton = new System.Windows.Forms.Button();
            this.ManageExportButton = new System.Windows.Forms.Button();
            this.ManageEnableButton = new System.Windows.Forms.Button();
            this.ExitPanel.SuspendLayout();
            this.ManagePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManageLabel
            // 
            this.ManageLabel.AutoSize = true;
            this.ManageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManageLabel.Location = new System.Drawing.Point(7, 9);
            this.ManageLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ManageLabel.Name = "ManageLabel";
            this.ManageLabel.Size = new System.Drawing.Size(294, 17);
            this.ManageLabel.TabIndex = 0;
            this.ManageLabel.Text = "Manage your existing program settings.";
            // 
            // ManageListView
            // 
            this.ManageListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ManageListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ManageAppNameColumn,
            this.ManageAppPathColumn,
            this.ManageAppIdColumn});
            listViewGroup1.Header = "Programs";
            listViewGroup1.Name = "EnabledGroup";
            listViewGroup2.Header = "Disabled Programs";
            listViewGroup2.Name = "DisabledGroup";
            this.ManageListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.ManageListView.Location = new System.Drawing.Point(10, 31);
            this.ManageListView.Margin = new System.Windows.Forms.Padding(2);
            this.ManageListView.MultiSelect = false;
            this.ManageListView.Name = "ManageListView";
            this.ManageListView.Size = new System.Drawing.Size(373, 323);
            this.ManageListView.TabIndex = 0;
            this.ManageListView.UseCompatibleStateImageBehavior = false;
            this.ManageListView.View = System.Windows.Forms.View.SmallIcon;
            this.ManageListView.SelectedIndexChanged += new System.EventHandler(this.ManageListView_SelectedIndexChanged);
            this.ManageListView.DoubleClick += new System.EventHandler(this.ManageListView_DoubleClick);
            this.ManageListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManageListView_KeyDown);
            // 
            // ManageAppNameColumn
            // 
            this.ManageAppNameColumn.Text = "Program Name";
            this.ManageAppNameColumn.Width = 150;
            // 
            // ManageAppPathColumn
            // 
            this.ManageAppPathColumn.Text = "Program Path";
            this.ManageAppPathColumn.Width = 241;
            // 
            // ManageAppIdColumn
            // 
            this.ManageAppIdColumn.Width = 0;
            // 
            // ManageDisableButton
            // 
            this.ManageDisableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ManageDisableButton.BackColor = System.Drawing.SystemColors.Control;
            this.ManageDisableButton.Location = new System.Drawing.Point(229, 358);
            this.ManageDisableButton.Margin = new System.Windows.Forms.Padding(2);
            this.ManageDisableButton.Name = "ManageDisableButton";
            this.ManageDisableButton.Size = new System.Drawing.Size(75, 23);
            this.ManageDisableButton.TabIndex = 3;
            this.ManageDisableButton.Text = "Dis&able";
            this.ManageDisableButton.UseVisualStyleBackColor = true;
            this.ManageDisableButton.Click += new System.EventHandler(this.ManageDisableButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitButton.Location = new System.Drawing.Point(308, 10);
            this.ExitButton.Margin = new System.Windows.Forms.Padding(2);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 1;
            this.ExitButton.Text = "&Cancel";
            this.ExitButton.UseVisualStyleBackColor = true;
            // 
            // ExitPanel
            // 
            this.ExitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ExitPanel.Controls.Add(this.FileOpenButton);
            this.ExitPanel.Controls.Add(this.ExitButton);
            this.ExitPanel.Location = new System.Drawing.Point(0, 389);
            this.ExitPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ExitPanel.Name = "ExitPanel";
            this.ExitPanel.Size = new System.Drawing.Size(400, 43);
            this.ExitPanel.TabIndex = 5;
            // 
            // FileOpenButton
            // 
            this.FileOpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FileOpenButton.BackColor = System.Drawing.SystemColors.Control;
            this.FileOpenButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.FileOpenButton.Location = new System.Drawing.Point(229, 10);
            this.FileOpenButton.Margin = new System.Windows.Forms.Padding(2);
            this.FileOpenButton.Name = "FileOpenButton";
            this.FileOpenButton.Size = new System.Drawing.Size(75, 23);
            this.FileOpenButton.TabIndex = 0;
            this.FileOpenButton.Text = "&Open";
            this.FileOpenButton.UseVisualStyleBackColor = true;
            this.FileOpenButton.Click += new System.EventHandler(this.FileOpenButton_Click);
            // 
            // ManageDeleteWideButton
            // 
            this.ManageDeleteWideButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ManageDeleteWideButton.BackColor = System.Drawing.SystemColors.Control;
            this.ManageDeleteWideButton.Location = new System.Drawing.Point(308, 358);
            this.ManageDeleteWideButton.Margin = new System.Windows.Forms.Padding(2);
            this.ManageDeleteWideButton.Name = "ManageDeleteWideButton";
            this.ManageDeleteWideButton.Size = new System.Drawing.Size(75, 23);
            this.ManageDeleteWideButton.TabIndex = 4;
            this.ManageDeleteWideButton.Text = "&Delete";
            this.ManageDeleteWideButton.UseVisualStyleBackColor = true;
            this.ManageDeleteWideButton.Click += new System.EventHandler(this.ManageDeleteWideButton_Click);
            // 
            // ManagePanel
            // 
            this.ManagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ManagePanel.Controls.Add(this.label1);
            this.ManagePanel.Controls.Add(this.ManageDeleteWideButton);
            this.ManagePanel.Controls.Add(this.ManageImportButton);
            this.ManagePanel.Controls.Add(this.ManageExportButton);
            this.ManagePanel.Controls.Add(this.ManageDisableButton);
            this.ManagePanel.Controls.Add(this.ManageListView);
            this.ManagePanel.Controls.Add(this.ManageLabel);
            this.ManagePanel.Controls.Add(this.ManageEnableButton);
            this.ManagePanel.Location = new System.Drawing.Point(0, 0);
            this.ManagePanel.Margin = new System.Windows.Forms.Padding(2);
            this.ManagePanel.Name = "ManagePanel";
            this.ManagePanel.Size = new System.Drawing.Size(400, 389);
            this.ManagePanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(175, 363);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Settings:";
            // 
            // ManageImportButton
            // 
            this.ManageImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ManageImportButton.BackColor = System.Drawing.SystemColors.Control;
            this.ManageImportButton.Enabled = false;
            this.ManageImportButton.Location = new System.Drawing.Point(63, 358);
            this.ManageImportButton.Margin = new System.Windows.Forms.Padding(2);
            this.ManageImportButton.Name = "ManageImportButton";
            this.ManageImportButton.Size = new System.Drawing.Size(75, 23);
            this.ManageImportButton.TabIndex = 1;
            this.ManageImportButton.Text = "I&mport";
            this.ManageImportButton.UseVisualStyleBackColor = true;
            this.ManageImportButton.Visible = false;
            // 
            // ManageExportButton
            // 
            this.ManageExportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ManageExportButton.BackColor = System.Drawing.SystemColors.Control;
            this.ManageExportButton.Enabled = false;
            this.ManageExportButton.Location = new System.Drawing.Point(142, 358);
            this.ManageExportButton.Margin = new System.Windows.Forms.Padding(2);
            this.ManageExportButton.Name = "ManageExportButton";
            this.ManageExportButton.Size = new System.Drawing.Size(75, 23);
            this.ManageExportButton.TabIndex = 2;
            this.ManageExportButton.Text = "E&xport";
            this.ManageExportButton.UseVisualStyleBackColor = true;
            this.ManageExportButton.Visible = false;
            // 
            // ManageEnableButton
            // 
            this.ManageEnableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ManageEnableButton.BackColor = System.Drawing.SystemColors.Control;
            this.ManageEnableButton.Enabled = false;
            this.ManageEnableButton.Location = new System.Drawing.Point(229, 358);
            this.ManageEnableButton.Margin = new System.Windows.Forms.Padding(2);
            this.ManageEnableButton.Name = "ManageEnableButton";
            this.ManageEnableButton.Size = new System.Drawing.Size(75, 23);
            this.ManageEnableButton.TabIndex = 3;
            this.ManageEnableButton.Text = "Enable";
            this.ManageEnableButton.UseVisualStyleBackColor = true;
            this.ManageEnableButton.Visible = false;
            this.ManageEnableButton.Click += new System.EventHandler(this.ManageEnableButton_Click);
            // 
            // ManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(394, 430);
            this.Controls.Add(this.ExitPanel);
            this.Controls.Add(this.ManagePanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Programs";
            this.ExitPanel.ResumeLayout(false);
            this.ManagePanel.ResumeLayout(false);
            this.ManagePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManageLabel;
        private System.Windows.Forms.ListView ManageListView;
        private System.Windows.Forms.ColumnHeader ManageAppNameColumn;
        private System.Windows.Forms.ColumnHeader ManageAppPathColumn;
        private System.Windows.Forms.Button ManageDisableButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Panel ExitPanel;
        private System.Windows.Forms.Button ManageDeleteWideButton;
        private System.Windows.Forms.Button FileOpenButton;
        private System.Windows.Forms.Panel ManagePanel;
        private System.Windows.Forms.ColumnHeader ManageAppIdColumn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ManageEnableButton;
        private System.Windows.Forms.Button ManageImportButton;
        private System.Windows.Forms.Button ManageExportButton;
    }
}