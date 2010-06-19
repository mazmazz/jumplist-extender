using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Runtime.InteropServices;
using T7ECommon;

namespace T7EPreferences
{
    public partial class ManageForm : Form
    {
        [DllImport("shell32.dll")]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyIcon(IntPtr hIcon);

        public enum ManageFormMode
        {
            ManageApp = 0,
            OpenApp
        }

        private ManageFormMode FormMode;
        ImageList ManageListViewImageList = new ImageList();
        Primary PrimaryParent;

        public ManageForm(Primary parent, ManageFormMode formMode)
        {
            InitializeComponent();
            this.Icon = Primary.PrimaryIcon;

            FormMode = formMode;
            PrimaryParent = parent;

            if (formMode == ManageFormMode.OpenApp)
            {
                // This used to do something cool, but I merged the modes since then.
                // Now all this does is change the label.
                Text = "Select a program";
                ManageLabel.Text = "Select a program to open.";
            }

            ManageListView.SmallImageList = ManageListViewImageList;

            // Populate ManageListBox with programs
            PopulateManageListBox();
            UpdateButtonStatus();
            if (ManageListView.Items.Count > 0) ManageListView.Items[0].Selected = true;
        }

        private void PopulateManageListBox()
        {
            ManageListView.Items.Clear();

            // The App lists are already populated. Just grab their info.
            foreach (string appId in Common.AppIds)
            {
                ListViewItem lvi;
                string[] itemFields = new string[3];
                itemFields[2] = appId;

                // Get app name
                try
                {
                    using(XmlReader reader = XmlReader.Create(Path.Combine(Common.Path_AppData, appId + "\\AppSettings.xml"))) {
                        while (reader.Read())
                            if (reader.IsStartElement("appSettings"))
                                break;

                        itemFields[0] = reader["name"].Length <= 0 ? "[Unnamed Program]"
                            : reader["name"];
                        itemFields[1] = reader["path"];
                    }
                }
                catch (Exception e) { continue; }
                 
                lvi = new ListViewItem(itemFields);
                lvi.Group = ManageListView.Groups[0];
                ManageListView.Items.Add(lvi);

                // Also, get the icon
                Icon appIcon = SystemIcons.WinLogo;
                IntPtr appIconPt = IntPtr.Zero;
                try
                {
                    // why icon.extractassociatedicon fails?
                    //appIcon = Icon.ExtractAssociatedIcon(itemFields[1]);
                    appIconPt = ExtractIcon(IntPtr.Zero, itemFields[1], 0);
                    appIcon = Icon.FromHandle(appIconPt);
                }
                catch (Exception e)
                {
                    //MessageBox.Show("ManageListBox could not be populated.");
                }

                ManageListViewImageList.Images.Add(appIcon.ToBitmap());
                DestroyIcon(appIconPt);

                ManageListView.Items[ManageListView.Items.Count - 1].ImageIndex = ManageListViewImageList.Images.Count - 1;
            }

            // Also check for DisabledAppList.xml entries
            string disabledAppListXmlPath = Path.Combine(Common.Path_AppData, "DisabledAppList.xml");
            if (!File.Exists(disabledAppListXmlPath)) return;

            XmlDocument disabledAppList = new XmlDocument();
            disabledAppList.Load(disabledAppListXmlPath);

            foreach (XmlElement disabledApp in disabledAppList.GetElementsByTagName("app"))
            {
                ListViewItem lvi;
                string[] itemFields = new string[3];
                itemFields[2] = disabledApp.GetAttribute("id");
                // TODO: Get app name, not appprocessname
                itemFields[0] = disabledApp.GetAttribute("name");
                // TODO: Get app path (itemFields[1])
                itemFields[1] = disabledApp.GetAttribute("path");

                lvi = new ListViewItem(itemFields);
                lvi.Group = ManageListView.Groups[1];
                ManageListView.Items.Add(lvi);

                // Also, get the icon
                Icon appIcon = SystemIcons.WinLogo;
                IntPtr appIconPt = IntPtr.Zero;
                try
                {
                    // why icon.extractassociatedicon fails?
                    //appIcon = Icon.ExtractAssociatedIcon(itemFields[1]);
                    appIconPt = ExtractIcon(IntPtr.Zero, itemFields[1], 0);
                    appIcon = Icon.FromHandle(appIconPt);
                }
                catch (Exception e)
                {
                    MessageBox.Show("ManageListBox could not be populated.");
                }

                ManageListViewImageList.Images.Add(appIcon.ToBitmap());
                DestroyIcon(appIconPt);

                ManageListView.Items[ManageListView.Items.Count - 1].ImageIndex = ManageListViewImageList.Images.Count - 1;
            }
        }

        public string SelectedAppId = "";

        private void OpenSelectedApp()
        {
            if (ManageListView.SelectedItems.Count < 1) return;
            ListViewItem.ListViewSubItemCollection subItems = ManageListView.SelectedItems[0].SubItems;
            SelectedAppId = subItems[2].Text;
        }

        private void EnableSelectedApp()
        {
            if (ManageListView.SelectedItems.Count <= 0) return;
            string appName = ManageListView.SelectedItems[0].SubItems[0].Text;
            string appPath = ManageListView.SelectedItems[0].SubItems[1].Text;
            string appId = ManageListView.SelectedItems[0].SubItems[2].Text;

            // Enable app
            Preferences.EnableApp(appId, appPath);

            // Refresh applist
            PopulateManageListBox();
            UpdateButtonStatus();
        }

        private void DeleteSelectedApp()
        {
            if (ManageListView.SelectedItems.Count <= 0) return;
            string appName = ManageListView.SelectedItems[0].SubItems[0].Text;
            string appPath = ManageListView.SelectedItems[0].SubItems[1].Text;
            string appId = ManageListView.SelectedItems[0].SubItems[2].Text;

            DialogResult deleteDialogResult = MessageBox.Show("Are you sure you want to delete \"" +
                appName +
                "\" permanently?",
                "Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Exclamation);

            if (deleteDialogResult != System.Windows.Forms.DialogResult.Yes) return;

            if (PrimaryParent.CurrentAppId != null && PrimaryParent.CurrentAppId.Equals(appId))
                if (!PrimaryParent.ClearAppLoaded(true, false)) return; 

            // Delete app from internal, applist.xml, and appdata
            if (ManageListView.SelectedItems[0].Group == ManageListView.Groups[1])
                Preferences.DeleteApp(appId, appPath, true);
            else
                Preferences.DeleteApp(appId, appPath);

            // Erase the old jumplist from the shell
            Preferences.EraseJumplist(appId); // Erase the old jumplist, too.

            // Refresh applist
            PopulateManageListBox();
            UpdateButtonStatus();
        }

        private void DisableSelectedApp()
        {
            if (ManageListView.SelectedItems.Count <= 0) return;
            string appName = ManageListView.SelectedItems[0].SubItems[0].Text;
            string appPath = ManageListView.SelectedItems[0].SubItems[1].Text;
            string appId = ManageListView.SelectedItems[0].SubItems[2].Text;

            if (PrimaryParent.CurrentAppId != null && PrimaryParent.CurrentAppId.Equals(appId))
            {
                if (!PrimaryParent.ClearAppLoaded(true)) return; // Asks "do you want save?"
            }

            Preferences.DisableApp(appId, appPath, appName);

            // Refresh applist
            PopulateManageListBox();
            UpdateButtonStatus();
        }

        private void ManageListView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Do a hit test for the current mouse position
                ListViewHitTestInfo hitInfo = ManageListView.HitTest(ManageListView.PointToClient(MousePosition));

                // Check if any items are selected
                if (0 == ManageListView.SelectedItems.Count)
                {
                    // No items selected
                }
                else
                {
                    FileOpenButton.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FileOpenButton_Click(object sender, EventArgs e)
        {
            OpenSelectedApp();
        }

        private void ManageListView_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (ManageListView.SelectedItems.Count > 0)
                        FileOpenButton.PerformClick();
                    break;
                case Keys.Delete:
                    if (ManageListView.SelectedItems.Count > 0)
                        DeleteSelectedApp();
                    break;
            }
        }

        private void ManageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStatus();
        }

        void UpdateButtonStatus()
        {
            // Gray out buttons
            if (ManageListView.SelectedItems.Count <= 0)
            {
                ManageExportButton.Enabled =
                    ManageDisableButton.Enabled =
                    ManageDeleteWideButton.Enabled =
                    FileOpenButton.Enabled =
                    ManageEnableButton.Enabled =
                    ManageEnableButton.Visible =
                    false;

                ManageImportButton.Enabled = 
                    ManageDisableButton.Visible =
                    true;
            }
            else
            {
                ManageExportButton.Enabled =
                    ManageImportButton.Enabled =
                    ManageDisableButton.Enabled =
                    ManageDisableButton.Visible =
                    ManageDeleteWideButton.Enabled =
                    FileOpenButton.Enabled =
                    true;

                ManageEnableButton.Visible =
                        ManageEnableButton.Enabled =
                        false;

                if (ManageListView.SelectedItems.Count > 1)
                {
                    ManageImportButton.Enabled =
                        FileOpenButton.Enabled =
                        false;
                }

                if (ManageListView.SelectedItems[0].Group == ManageListView.Groups[1])
                {
                    ManageImportButton.Enabled =
                        FileOpenButton.Enabled =
                        ManageDisableButton.Enabled =
                        ManageDisableButton.Visible =
                        false;

                    ManageEnableButton.Visible =
                        ManageEnableButton.Enabled =
                        true;
                }
            }
        }

        private void ManageDeleteWideButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedApp();
        }

        private void ManageDisableButton_Click(object sender, EventArgs e)
        {
            DisableSelectedApp();
        }

        private void ManageEnableButton_Click(object sender, EventArgs e)
        {
            EnableSelectedApp();
        }
    }
}
