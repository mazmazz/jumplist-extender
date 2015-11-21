using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using T7ECommon;

namespace T7EPreferences
{
    public partial class Donate : Form
    {
        private bool DeliberateShow;

        public Donate(bool deliberate)
        {
            InitializeComponent();

            DeliberateShow = deliberate;

            this.Icon = Primary.PrimaryIcon;

            // donatedialogdisable handled in show event

            // dummying out donate dialog
            bool donateBalloonDisable = true;// false;
            //bool.TryParse(Common.ReadPref("DonateBalloonDisable"), out donateBalloonDisable);

            bool donateBalloonShown = true;// false;
            //bool.TryParse(Common.ReadPref("DonateBalloonShown"), out donateBalloonShown);
            checkBox2.Enabled = checkBox2.Visible = donateBalloonShown;
            checkBox2.Checked = donateBalloonDisable;

            
        }

        private string getDefaultBrowser()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                //trim off quotes
                browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                if (!browser.EndsWith("exe"))
                {
                    //get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(".exe") + 4);
                }
            }
            finally
            {
                if (key != null) key.Close();
            }
            return browser;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(getDefaultBrowser(), Common.WebPath_DonateSite);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start(getDefaultBrowser(), Common.WebPath_DonateSite);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Common.WritePref("DonateBalloonDisable", checkBox2.Checked.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AskDonateDialogDisable()
        {
            // Don't deal with codes now. Just confirm disable
            Common.WritePref("DonateDialogDisable", true.ToString());
            Common.WritePref("DonateBalloonDisable", true.ToString());
            checkBox2.Checked = true;
            MessageBox.Show("Donate window will not pop up until the next update. If you donated, thank you!", "Donate", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        
        private void shortcutMenuDisable_Click(object sender, EventArgs e)
        {
            AskDonateDialogDisable();
        }

        private void Donate_Shown(object sender, EventArgs e)
        {
            // close should be handled by calling class
        }
    }
}
