namespace T7EBackground
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
            this.components = new System.ComponentModel.Container();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TrayIconContextMenuOpenSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.visitTheOfficialWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToVersion0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.TrayIconContextMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.DonateTimer = new System.Windows.Forms.Timer(this.components);
            this.TrayIconContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.ContextMenuStrip = this.TrayIconContextMenuStrip;
            this.TrayIcon.Text = "Jumplist Extender";
            this.TrayIcon.Visible = true;
            this.TrayIcon.BalloonTipClicked += new System.EventHandler(this.TrayIcon_BalloonTipClicked_Update);
            this.TrayIcon.DoubleClick += new System.EventHandler(this.TrayIconContextMenuOpenSettings_Click);
            // 
            // TrayIconContextMenuStrip
            // 
            this.TrayIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayIconContextMenuOpenSettings,
            this.visitTheOfficialWebsiteToolStripMenuItem,
            this.donateToolStripMenuItem,
            this.updateToVersion0ToolStripMenuItem,
            this.toolStripSeparator1,
            this.TrayIconContextMenuExit});
            this.TrayIconContextMenuStrip.Name = "TrayIconContextMenuStrip";
            this.TrayIconContextMenuStrip.Size = new System.Drawing.Size(203, 142);
            // 
            // TrayIconContextMenuOpenSettings
            // 
            this.TrayIconContextMenuOpenSettings.Font = new System.Drawing.Font("Tahoma", 8.400001F, System.Drawing.FontStyle.Bold);
            this.TrayIconContextMenuOpenSettings.Name = "TrayIconContextMenuOpenSettings";
            this.TrayIconContextMenuOpenSettings.Size = new System.Drawing.Size(202, 22);
            this.TrayIconContextMenuOpenSettings.Text = "&Open Settings";
            this.TrayIconContextMenuOpenSettings.Click += new System.EventHandler(this.TrayIconContextMenuOpenSettings_Click);
            // 
            // visitTheOfficialWebsiteToolStripMenuItem
            // 
            this.visitTheOfficialWebsiteToolStripMenuItem.Name = "visitTheOfficialWebsiteToolStripMenuItem";
            this.visitTheOfficialWebsiteToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.visitTheOfficialWebsiteToolStripMenuItem.Text = "&Visit the Official Website";
            this.visitTheOfficialWebsiteToolStripMenuItem.Visible = false;
            this.visitTheOfficialWebsiteToolStripMenuItem.Click += new System.EventHandler(this.visitTheOfficialWebsiteToolStripMenuItem_Click);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.donateToolStripMenuItem.Text = "&Donate";
            this.donateToolStripMenuItem.Visible = false;
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // updateToVersion0ToolStripMenuItem
            // 
            this.updateToVersion0ToolStripMenuItem.Enabled = false;
            this.updateToVersion0ToolStripMenuItem.Name = "updateToVersion0ToolStripMenuItem";
            this.updateToVersion0ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.updateToVersion0ToolStripMenuItem.Text = "&Update to Version {0}";
            this.updateToVersion0ToolStripMenuItem.Visible = false;
            this.updateToVersion0ToolStripMenuItem.Click += new System.EventHandler(this.updateToVersion0ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
            // 
            // TrayIconContextMenuExit
            // 
            this.TrayIconContextMenuExit.Name = "TrayIconContextMenuExit";
            this.TrayIconContextMenuExit.Size = new System.Drawing.Size(202, 22);
            this.TrayIconContextMenuExit.Text = "E&xit";
            this.TrayIconContextMenuExit.Click += new System.EventHandler(this.TrayIconContextMenuExit_Click);
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Interval = 86400000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // DonateTimer
            // 
            this.DonateTimer.Interval = 1800000;
            this.DonateTimer.Tick += new System.EventHandler(this.DonateTimer_Tick);
            // 
            // Primary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 218);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Primary";
            this.Text = "Primary";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Primary_FormClosing);
            this.Load += new System.EventHandler(this.Primary_Load);
            this.Resize += new System.EventHandler(this.Primary_Resize);
            this.TrayIconContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayIconContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem TrayIconContextMenuOpenSettings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem TrayIconContextMenuExit;
        private System.Windows.Forms.Timer UpdateTimer;
        private System.Windows.Forms.ToolStripMenuItem updateToVersion0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitTheOfficialWebsiteToolStripMenuItem;
        private System.Windows.Forms.Timer DonateTimer;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
    }
}