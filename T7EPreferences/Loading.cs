using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace T7EPreferences
{
    public partial class Loading : Form
    {
        public Loading(string loadingText, bool showCancel, bool showLogo)
        {
            InitializeComponent();

            if (loadingText != null && loadingText.Length > 0)
            {
                LoadingLabel.Text = loadingText;
                Text = loadingText; // Window title
            }

            if (!showCancel)
            {
                LoadingCancel.Enabled = LoadingCancel.Visible = false;
                Height = Height - 30;
            }

            if (!showLogo)
            {
                LoadingLogo.Visible = false;
                Height = Height - 95;
            }

            LoadingProgress.Style = ProgressBarStyle.Marquee; // Show a marquee until a value is passed
        }

        public void PassValueToProgressBar(int progValue)
        {
        }

        private void LoadingCancel_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}
