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
    public partial class ExportForm : Form
    {
        public bool IsProgramSpecific = false;
        public bool ShareWeb = false;

        public ExportForm(string programName)
        {
            InitializeComponent();
            this.Icon = Primary.PrimaryIcon;

            ExportProgramSpecificLabel.Text = String.Format(ExportProgramSpecificLabel.Text, programName);
        }

        private void ImportNewLabel_Click(object sender, EventArgs e)
        {
            ExportProgramSpecificCheckBox.PerformClick();
        }

        private void ImportOpenLabel_Click(object sender, EventArgs e)
        {
            ExportUniversalCheckBox.PerformClick();
        }

        private void ShareWebLabel_Click(object sender, EventArgs e)
        {
            ShareWebCheckBox.Checked = !ShareWebCheckBox.Checked;
        }

        private void ExportProgramSpecificCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            IsProgramSpecific = ExportProgramSpecificCheckBox.Checked;
        }

        private void ShareWebCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ShareWeb = ShareWebCheckBox.Checked;
        }
    }
}
