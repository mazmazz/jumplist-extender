using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Diagnostics;

using T7ECommon;

namespace T7EPreferences
{
    public partial class StartForm : Form
    {
        Primary PrimaryForm;
        public StartForm(Primary primaryFormRef)
        {
            InitializeComponent();
            PrimaryForm = primaryFormRef;
            
            this.Icon = Primary.PrimaryIcon;

            if (Common.AppCount <= 0 && Preferences.DisabledList_AppCount() <= 0) StartOpenButton.Enabled = false;
            else StartOpenButton.Enabled = true;

            // Check to show donate link
            if (!Common.PrefExists("InstallDate"))
            {
                if(Common.AppCount > 0) Common.WritePref("InstallDate", DateTime.Today.Year.ToString()+"-"+DateTime.Today.Month.ToString()+"-"+DateTime.Today.Day.ToString()
                    , "InstallUpgrade", false.ToString());
                else Common.WritePref("InstallDate", DateTime.Today.AddDays(-1).ToString("u")
                    , "InstallUpgrade", false.ToString());
            }

            DateTime installDate = DateTime.Today;
            bool installUpgrade = false;
            DateTime.TryParse(Common.ReadPref("InstallDate"), out installDate);
            bool.TryParse(Common.ReadPref("InstallUpgrade"), out installUpgrade);
            if ((DateTime.Today - installDate).Days >= 3 || installUpgrade == true)
            {
                // Also, display the donate icon
                // This might be made invisible by later linklabels.
                DonatePictureBox.Enabled = DonatePictureBox.Visible = true;
            }

            // Check for new version
            UpdateCheckWorker.RunWorkerAsync();
            CheckNewVersion();
        }

        private string VersionHttpLink = "";

        private void CheckNewVersion()
        {
            if (!File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))) return;
            if (!Common.SanitizeUpdateResponse(File.ReadAllText(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))))
            {
                File.Delete(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"));
                return;
            }
            
            try
            {
                string[] versionCheckParts = File.ReadAllText(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")).Split('|');
                if (versionCheckParts == null || versionCheckParts.Length < 2) return;

                if (Assembly.GetExecutingAssembly().GetName().Version.ToString().Equals(versionCheckParts[0])) return;
                else
                {
                    // If UpdateCheck has 0.x.x.x|http://link|0.x-B, get the third slice
                    // else, substr from the first version string.
                    UpdateLinkLabel.Text = String.Format(UpdateLinkLabel.Text, 
                        versionCheckParts.Length >= 3 ? versionCheckParts[2]
                        : versionCheckParts[0].Substring(0, 3));
                    UpdateLinkLabel.Enabled = UpdateLinkLabel.Visible = true;
                    linkLabel1.Enabled = linkLabel1.Visible 
                        = DonatePictureBox.Enabled = DonatePictureBox.Visible = false;
                    VersionHttpLink = versionCheckParts[1];
                }
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ /* Fail silently */ }
        }

        private void StartNewButton_Click(object sender, EventArgs e)
        {
            PrimaryForm.StartDialogResult = 1;
        }

        private void StartOpenButton_Click(object sender, EventArgs e)
        {
            PrimaryForm.StartDialogResult = 2;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewButton.PerformClick();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartOpenButton.PerformClick();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            /*System.Diagnostics.Process.Start("explorer.exe", "\"Common.WebPath_PacksSite"");
            this.WindowState = FormWindowState.Minimized;*/

            Process.Start("explorer.exe", "\"" + Common.WebPath_OfficialSite + "\"");
        }

        private void StartImportButton_Click(object sender, EventArgs e)
        {
            PrimaryForm.StartDialogResult = 3;
        }

        private void importJumplistPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartImportButton.PerformClick();
        }

        private void UpdateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", VersionHttpLink);
        }

        private void UpdateCheckWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))
                && File.GetCreationTime(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")).Day - DateTime.Now.Day <= -1)
                || !File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))
                || !Common.SanitizeUpdateResponse(File.ReadAllText(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))))
            {
                try {
                    string updateResult = new WebClient().DownloadString(Common.WebPath_UpdateUrl);
                    
                    if(Common.SanitizeUpdateResponse(updateResult))
                        File.WriteAllText(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"), updateResult);                   
                }
                catch (Exception ee) { /* Fail silently. */ }
            }
        }

        private void DonatePictureBox_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "\"" + Common.WebPath_DonateSite + "\"");
            this.WindowState = FormWindowState.Minimized;
            //PrimaryForm.ShowDonateDialog(true);
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            DateTime installDate = DateTime.Today;
            bool installUpgrade = false;
            DateTime.TryParse(Common.ReadPref("InstallDate"), out installDate);
            bool.TryParse(Common.ReadPref("InstallUpgrade"), out installUpgrade);
            if ((DateTime.Today - installDate).Days >= 3 || installUpgrade == true)
            {
                // if disabledialog, then show will hide
                bool donateDialogDisable = false;
                bool.TryParse(Common.ReadPref("DonateDialogDisable"), out donateDialogDisable);
                if (!donateDialogDisable)
                {
                    Donate donationWindow = new Donate(false);
                    donationWindow.Show();
                }
            }
        }

        
    }
}
