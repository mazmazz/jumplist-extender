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
    public partial class HelpDialog : Form
    {
        public HelpDialog(string title, string helpText)
        {
            InitializeComponent();
            this.Icon = Primary.PrimaryIcon;

            Text = title;
            TitleLabel.Text = title;
            InfoTextBox.Text = helpText;
            InfoTextBox.SelectionLength = 0;

            //OkButton.Focus();
            OkButton.Click += new EventHandler(OkButton_Click);
        }

        void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
