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
        public Donate(bool deliberate)
        {
            InitializeComponent();

            /*if (!foreground && parent.Location.Y != null && parent.Location.Y > 0
                && parent.Height != null && parent.Height > 0)
                this.Location = new Point(
                    Screen.PrimaryScreen.WorkingArea.Width - (parent.Location.X - parent.Width) > this.Width ? 
                    (parent.Location.X+parent.Width)+(((Screen.PrimaryScreen.WorkingArea.Width-(parent.Location.X+parent.Width))/2)-(this.Width/2))
                    : Screen.PrimaryScreen.WorkingArea.Width - this.Width
                    , (parent.Location.Y + (parent.Size.Height / 2)) - (this.Height / 2));
            else
                CenterToScreen();*/

            //if (Screen.PrimaryScreen.WorkingArea.Width - Location.X < this.Width) this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Location.Y);
            this.Icon = Primary.PrimaryIcon;
            CenterToScreen();
            bool donateDialogDisable = false;
            bool.TryParse(Common.ReadPref("DonateDialogDisable"), out donateDialogDisable);
            checkBox2.Checked = donateDialogDisable;
            if (deliberate) {
                bool donateBalloonShown = false;
                bool.TryParse(Common.ReadPref("DonateBalloonShown"), out donateBalloonShown);
                checkBox2.Enabled = checkBox2.Visible = donateBalloonShown;
                checkBox2.Text = "Disable balloon alert"; 
            }
            else { if (donateDialogDisable) { Close(); } }
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
            Process.Start(getDefaultBrowser(), "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=4JEHM8794PVQE");
            button1.Text = "OK :)";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            Process.Start(getDefaultBrowser(), "https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=4JEHM8794PVQE");
            button1.Text = "OK :)";
        }

        public bool DisableForm = false;

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            DisableForm = checkBox2.Checked;
            /*if (DisableForm && button1.Text == "OK")
            {
                button1.Text = "OK";
            }
            else if(!DisableForm && button1.Text == "Aww :(")
            {
                button1.Text = "OK";
            }*/
            Common.WritePref("DonateDialogDisable", DisableForm.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
