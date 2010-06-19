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
    public partial class ImportForm : Form
    {
        public bool NewProgram = false;
        public bool ReplaceJumplist = false;

        public ImportForm(string appName)
        {
            InitializeComponent();
            this.Icon = Primary.PrimaryIcon;

            ImportOpenLabel.Text = String.Format(ImportOpenLabel.Text, appName);
        }

        private void ImportNewLabel_Click(object sender, EventArgs e)
        {
            ImportNewRadioButton.PerformClick();
        }

        private void ImportOpenLabel_Click(object sender, EventArgs e)
        {
            ImportOpenRadioButton.PerformClick();
        }

        private void ImportOpenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            NewProgram = !ImportOpenRadioButton.Checked;

            panel1.Enabled = ImportOpenRadioButton.Checked;
        }

        private void ImportOpenJumplistReplaceRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ReplaceJumplist = ImportOpenJumplistReplaceRadioButton.Checked;
        }
    }
}
