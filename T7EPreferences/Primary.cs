using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ImageComboBox;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;
//using Microsoft.WindowsAPICodePack.Controls.WindowsForms;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;
using T7ECommon;
using ICSharpCode.SharpZipLib.Zip;
using System.Reflection;

namespace T7EPreferences
{
    public partial class Primary : Form
    {
        bool Programmatic = false;

        public string CurrentAppId;
        public static string _CurrentAppName;
        public static string _CurrentAppPath;
        public string CurrentAppProcessName;
        public string CurrentAppWindowClassName;

        T7EJumplistItem _CurrentJumplistItem;

        bool _AppLoaded = false;
        public bool _JumplistEnabled; // VIP

        int EditCounter = 0;
        string LnkName = "";

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (TabControl.SelectedIndex != 1) return base.ProcessCmdKey(ref msg, keyData);

            if (keyData == (Keys.Control | Keys.Alt | Keys.S))
            {
                ActiveControl = null;
                saveAndApplyToTaskbarToolStripMenuItem.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Add)
                || keyData == (Keys.Control | Keys.Oemplus))
            {
                ActiveControl = null;
                AddJumplistTask();
                JumplistListBox.Select();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Subtract)
                || keyData == (Keys.Control | Keys.OemMinus))
            {
                if (CurrentJumplistItem.ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks) return true;

                ActiveControl = null;
                DialogResult deleteResult = MessageBox.Show("Do you want to delete " + CurrentJumplistItem.ItemName + "?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (deleteResult == System.Windows.Forms.DialogResult.Yes)
                    deleteToolStripMenuItem.PerformClick();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Up))
            {
                ActiveControl = null;
                JumplistUpButton.PerformClick();
                JumplistListBox.Select();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Down))
            {
                ActiveControl = null;
                JumplistDownButton.PerformClick();
                JumplistListBox.Select();
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.J))
            {
                ActiveControl = null;
                JumplistListBox.Select();
                return true;
            }

            else if (keyData == (Keys.Alt | Keys.Up))
            {
                ActiveControl = null;
                JumplistListBox.SelectedIndex = JumplistListBox.SelectedIndex - 1 < 0 ? 0 : JumplistListBox.SelectedIndex-1;
                ItemNameTextBox.Focus();
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.Down))
            {
                ActiveControl = null;
                JumplistListBox.SelectedIndex = JumplistListBox.SelectedIndex + 1 >= JumplistListBox.Items.Count ? JumplistListBox.Items.Count-1 : JumplistListBox.SelectedIndex+1;
                ItemNameTextBox.Focus();
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.Left))
            {
                ActiveControl = null;
                JumplistListBox.Select();
                return true;
            }
            else if (keyData == (Keys.Alt | Keys.Right))
            {
                ActiveControl = null;
                ItemNameTextBox.Select();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        #region P/Invokes
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int PickIconDlg(IntPtr hwndOwner, StringBuilder lpstrFile, int nMaxFile, ref int lpdwIconIndex);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern uint ExtractIconEx(string szFileName, int nIconIndex,
           IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        #endregion

        #region Start-Up
        public enum StartDialogResults
        {
            None = 0,
            NewApp,
            OpenApp,
            OpenPack
        }
        public int StartDialogResult = 0;

        static public Icon PrimaryIcon;

        public Primary()
        {
            InitializeComponent();
            PrimaryIcon = new Icon(Properties.Resources.PrimaryIcon, 256, 256);
            this.Icon = PrimaryIcon;

            Common.WindowHandle_Primary = this.Handle;
            Preferences.PrimaryParent = this;
            //Common.TaskbarManagerInstance.SetApplicationIdForSpecificWindow(Common.WindowHandle_Primary, "T7E.Meta");
            //T7EBackground needs to set our appid. We set our appid manually when we apply the jumplist,
            //but that's because the operation sets our appid to something different.
            Common.ReadTemplates();

            TaskKBDKeyboardTextBox.SetParent(this);

            ReadAppList();

            // Check for args. Are we loading a jumplist pack?
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1
                && Path.GetExtension(args[1]).Equals(".jlp", StringComparison.OrdinalIgnoreCase)
                && File.Exists(args[1]))
            {
                if (!ImportPack("the current program", args[1]))
                {
                    if (!Visible) { ShowStartDialog(); Hide(); }
                    else AppReallyNew = false;
                }
            }

            else
                ShowStartDialog();

            // After everything shows, show the donate dialog
            DateTime installDate = DateTime.Today;
            bool installUpgrade = false;
            DateTime.TryParse(Common.ReadPref("InstallDate"), out installDate);
            bool.TryParse(Common.ReadPref("InstallUpgrade"), out installUpgrade);
            if ((DateTime.Today - installDate).Days >= 3 || installUpgrade == true)
            {
                bool donateDialogDisable = false;
                bool.TryParse(Common.ReadPref("DonateDialogDisable"), out donateDialogDisable);
                if (!donateDialogDisable) ShowDonateDialog(false);
            }

            // And update check
            CheckUpdateString();
        }

        public void ShowDonateDialog(bool foreground)
        {
            Donate donateForm = new Donate(this, foreground);
            donateForm.Show();
            if(!foreground) Interop.SetForegroundWindow(Handle); // Set window back
        }

        private void ShowStartDialog()
        {
            StartDialog_Label: 
            using (Form startForm = new StartForm(this))
            {
                DialogResult startResult = startForm.ShowDialog();

                if (startResult == DialogResult.Cancel)
                {
                    // Exit the app
                    Environment.Exit(-1);
                }

                // Else, DialogResult.OK is OK.
                startForm.Hide();
                startForm.Dispose();
                switch ((StartDialogResults)StartDialogResult)
                {
                    case StartDialogResults.NewApp:
                        bool openAppResult = OpenProgramFile();
                        if (!openAppResult)
                            goto StartDialog_Label;
                        else
                        {
                            SelectMainAppWindow(true);
                            AppReallyNew = false;
                        }
                        break;
                    case StartDialogResults.OpenApp:
                        bool openManageAppResult = OpenManageProgramFile();
                        if (!openManageAppResult)
                            goto StartDialog_Label;
                        break;
                    case StartDialogResults.OpenPack:
                        if (!ImportPack("the current program"))
                        {
                            // Check if edit window is visible. If it isn't, show.
                            if (!Visible) goto StartDialog_Label;
                            else AppReallyNew = false;
                        }
                        break;
                    default:
                        Environment.Exit(-1);
                        break;
                }
            }
        }
        #endregion

        /////////////////////////////////

        #region File/XML Utilities
        private bool OpenManageProgramFile()
        {
            ManageForm openForm = new ManageForm(this, ManageForm.ManageFormMode.OpenApp);
            DialogResult openResult = openForm.ShowDialog();
            if (openResult != DialogResult.OK
                || openForm.SelectedAppId.Length <= 0)
            {
                openForm.Dispose();
                if (!Visible) Application.Exit(); // TODO: Make this show start dialog
                return false;
            }

            OpenAppId(openForm.SelectedAppId, "");
            return true;
        }

        private bool OpenProgramFile()
        {
            return OpenProgramFile("");
        }

        private bool OpenProgramFile(string startPath)
        {
            OpenFileDialog programFileDialog = new OpenFileDialog();
            programFileDialog.Filter = "Program files (*.exe;*.lnk)|*.exe|All files (*.*)|*.*";
            programFileDialog.FilterIndex = 0;
            if (Directory.Exists(startPath))
                programFileDialog.InitialDirectory = startPath;
            else
                programFileDialog.InitialDirectory =
                File.Exists(Path.Combine(Common.Path_AppData, "Programs.library-ms")) ?
                Path.Combine(Common.Path_AppData, "Programs.library-ms")
                : Path.Combine(Common.EnvPath_AllUsersProfile, "Microsoft\\Windows\\Start Menu");
            programFileDialog.Title = "Select program file";
            programFileDialog.AutoUpgradeEnabled = true;
            //programFileDialog.ShowHelp = true;
            programFileDialog.DereferenceLinks = false;//false
            
            DialogResult fileResult = programFileDialog.ShowDialog();

            if (fileResult != DialogResult.OK)
            {
                programFileDialog.Dispose();
                return false;
            }

            programFileDialog.Dispose();
            string programFileName = programFileDialog.FileName;

            // Try to resolve it as an mSI shortcut
            //string msiOutput = MsiShortcutParser.ParseShortcut(programFileName);
            //if (msiOutput.Length > 0) programFileName = msiOutput;

            // If programFileName is a shortcut, get its target path
            LnkName = "";
            if (Path.GetExtension(programFileName).ToLower() == ".lnk")
            {
                LnkName = programFileName;
                // Also set CurrentAppName to the lnk name
                //CurrentAppName = Path.GetFileNameWithoutExtension(programFileName);
                programFileName = Common.ResolveLnkPath(programFileName);

                if (Directory.Exists(programFileName))
                    return OpenProgramFile(programFileName);
            }

            string appId = Common.GetAppIdForProgramFile(programFileName);
            return OpenAppId(appId, programFileName);
        }

        public bool ConfirmSave()
        {
            switch (MessageBox.Show("Do you want to save the program settings for " + (
                CurrentAppName != null && CurrentAppName.Length > 0 ?
                CurrentAppName
                : CurrentAppProcessName
                ) + "?", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.None, MessageBoxDefaultButton.Button1))
            {
                case System.Windows.Forms.DialogResult.Yes:
                    Preferences.SaveApp(this);
                    goto case System.Windows.Forms.DialogResult.No;
                case System.Windows.Forms.DialogResult.No:
                    return true;

                case System.Windows.Forms.DialogResult.Cancel: default: return false;
            }
        }


        public bool ClearAppLoaded()
        {
            return _ClearAppLoaded(true);
        }

        public bool ClearAppLoaded(bool hideWindow)
        {
            if (_ClearAppLoaded(true))
            {
                if(hideWindow) Hide();
                return true;
            }
            else return false;
        }

        public bool ClearAppLoaded(bool hideWindow, bool askSave)
        {
            if (_ClearAppLoaded(askSave))
            {
                if (hideWindow) Hide();
                return true;
            }
            else return false;
        }

        private bool _ClearAppLoaded(bool askSave)
        {
            if (!AppLoaded) return true;
            bool result = false;

            if (askSave)
            {
                if (!ConfirmSave()) return false;
            }

            // Clear
            CurrentAppId = "";
            CurrentAppName = "";
            CurrentAppPath = "";
            CurrentAppProcessName = "";
            CurrentAppWindowClassName = "";
            EditCounter = 0;

            JumplistListBox.Items.Clear();

            AppLoaded = false;
            AppReallyNew = false;

            result = true;
            return result;
        }

        public bool OpenAppId(string appId, string appPath)
        {
            if(AppLoaded) {
                if(!ClearAppLoaded())
                    return false;
            }

            Common.Log("Opening AppId: " + appId, 1);

            // Check if appId is in AppList. If it's not, ask for application name.

            // Do the same thing for app paths.
            // TODO: I wanted support for same-name/different-appid, but T7EBackground's compaerLists
            // are structured where you CAN'T HAVE two process names. Fix that.
            
            string processName = Path.GetFileNameWithoutExtension(appPath);
            for (int i = 0; i < Common.AppCount; i++)
            {
                if (Common.AppIds[i] == appId
                    || Common.AppProcessNames[i].Equals(processName, StringComparison.OrdinalIgnoreCase))
                {
                    Common.Log("AppId exists!");
                    appId = Common.AppIds[i]; // for matching appprocessnames

                    if (ReadAppSettings(appId))
                    {
                        Common.Log("App settings successfully read!");
                        FileOpenApp.Text = String.Format(
                            "Open the shortcut with {0}",
                            CurrentAppName != null && CurrentAppName.Length > 0 ?
                            CurrentAppName
                            : CurrentAppProcessName
                        );
                        ProgramNameLabel.Text =
                            CurrentAppWindowClassName != null && CurrentAppWindowClassName.Length > 0 ?
                            "Program &Title:"
                            : "Program &Title: (Change if this text does NOT appear on program's title bar)";
                        // Apploaded was already set. // All the data is loaded. Just populate!
                        goto EndLoad;
                    }
                    else break;
                }
            }

            // else, AppId doesn't exist. Start a new session.
            if (CreateNewAppSettings(appId, appPath))
            {
                Common.Log("App settings are new and successfully created!");
                // AppLoaded was already set.
            }

            Common.Log("Finished opening AppId.", 0);

        EndLoad:
            EditCounter = 0;

            // Select first task
            JumplistListBox.SelectedIndex = 0;
            foreach (T7EJumplistItem jlio in JumplistListBox.Items)
                if (jlio.ItemType != T7EJumplistItem.ItemTypeVar.Category
                    && jlio.ItemType != T7EJumplistItem.ItemTypeVar.CategoryTasks
                    && jlio.ItemType != T7EJumplistItem.ItemTypeVar.Separator)
                {
                    JumplistListBox.SelectedItem = jlio;
                    break;
                }

            // And show the window
            Show();
            WindowState = FormWindowState.Normal;
            Activate();
            JumplistListBox.Select();

            return true;
        }

        private void RecoverAppSettingsDir(string appId, string appPath)
        {
            if (appId == null || appId.Length <= 0) return;
            if (!Directory.Exists(Path.Combine(Common.Path_AppData, appId))) return;

            DialogResult recoverResult = MessageBox.Show(Path.GetFileName(appPath) + "'s jumplist settings were found corrupted. Do you want to recover the remaining data?", "Recover", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (recoverResult != System.Windows.Forms.DialogResult.Yes) return;

            FolderBrowserDialog recoverBrowser = new FolderBrowserDialog();
            recoverBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            recoverBrowser.ShowNewFolderButton = true;
            recoverBrowser.Description = "Select the folder you want to save the recovered data in.";
            recoverBrowser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DialogResult folderResult = recoverBrowser.ShowDialog();
            if (folderResult != DialogResult.OK)
            {
                recoverBrowser.Dispose();
                return;
            }

            string folderName = Path.Combine(recoverBrowser.SelectedPath, appId);
            recoverBrowser.Dispose();

            // Copy directory to folder
            if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);
            foreach (string filePath in Directory.GetFileSystemEntries(Path.Combine(Common.Path_AppData, appId)))
            {
                if(File.Exists(filePath))
                    File.Copy(filePath, Path.Combine(folderName, Path.GetFileName(filePath)));
            }

            Directory.Delete(Path.Combine(Common.Path_AppData, appId), true);

            Process.Start("explorer.exe", folderName);
        }

        public bool AppReallyNew = false;

        private bool CreateNewAppSettings(string appId, string appPath) 
        {
            bool result = false;

            // 1. Check if AppId folder and AppSettings.xml exists.
            // If it does, add entry to AppList.xml and load app settings.
            // 2. If AppSettings.xml does not exist,
            // Create new app in memory. Changes will be committed when saved.

            // Add entry to internal AppList
            Common.AppIds.Add(appId);
            Common.AppProcessNames.Add(Path.GetFileNameWithoutExtension(appPath));
            Common.AppCount++;

            string appIdPath = Path.Combine(Common.Path_AppData, appId);
            if (Directory.Exists(appIdPath))
            {
                if (File.Exists(Path.Combine(appIdPath, "AppSettings.xml")))
                {
                    // Load app settings
                    if (ReadAppSettings(appId))
                    {
                        AppLoaded = true;
                        return true;
                    }
                }

                // Else, delete the directory and start fresh.
                // Notepad.exe's jumplist settings were found corrupted. Do you want to recover
                // the remaining data?
                // TODO
                foreach (string filePath in Directory.GetFileSystemEntries(appIdPath))
                {
                    // Are the XML files of a size greater than zero?
                    if ((filePath.Equals("AppSettings.xml", StringComparison.OrdinalIgnoreCase)
                        || filePath.Equals("Jumplist.xml", StringComparison.OrdinalIgnoreCase)
                        && new FileInfo(filePath).Length > 0)

                        || filePath.IndexOf(".ahk") > 0)
                    {
                        RecoverAppSettingsDir(appId, appPath);
                        break;
                    }
                }
                Directory.Delete(appIdPath, true);
            }
            
            CurrentAppId = appId;

            if (File.Exists(LnkName)) // This might have been filled with LNK name earlier.
                CurrentAppName = char.ToUpper(Path.GetFileNameWithoutExtension(LnkName)[0]) + Path.GetFileNameWithoutExtension(LnkName).Substring(1);
            else
            {
                CurrentAppName = FileVersionInfo.GetVersionInfo(appPath).ProductName;
                if(CurrentAppName == null 
                    || CurrentAppName.Length <= 0
                    || (CurrentAppName.IndexOf("Microsoft") >= 0
                    && CurrentAppName.IndexOf("Windows") >= 0
                    && CurrentAppName.IndexOf("Operating") >= 0
                    && CurrentAppName.IndexOf("System") >= 0))
                    CurrentAppName = char.ToUpper(Path.GetFileNameWithoutExtension(appPath)[0]) + Path.GetFileNameWithoutExtension(appPath).Substring(1);
            }

            CurrentAppPath = appPath;
            CurrentAppProcessName = Path.GetFileNameWithoutExtension(appPath);
            Common.Log(CurrentAppId + " | " + CurrentAppName + " | " + CurrentAppPath + " | " + CurrentAppProcessName);

            // Now load blank app
            T7EJumplistItem jumplistItem = new T7EJumplistItem(this);
            jumplistItem.ItemName = "Tasks";
            jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.CategoryTasks;
            JumplistListBox.Items.Add(jumplistItem);

            jumplistItem = new T7EJumplistItem(this);
            jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.Task;
            jumplistItem.ItemName = "";
            jumplistItem.StringToItemIcon("Use program icon");
            jumplistItem.TaskAction = T7EJumplistItem.ActionType.Keyboard;
            jumplistItem.TaskKBDString = "";
            JumplistListBox.Items.Add(jumplistItem);

            // BUT, set the jumplist tab control to index 0, to show "new program" page
            // First app experience
            AppLoaded = true; // Workaround: Set tabcontol AFTER apploaded
            AppReallyNew = true;

            TabControl.SelectedIndex = 0;
            ProgramSettingsLabel.Text = String.Format(
                "Verify the program settings below.",
                CurrentAppProcessName
                );

            // Select a program window first.
            /*Programmatic = true;
            SelectMainAppWindow(true);
            Programmatic = false;*/



            ActiveControl = ProgramNameTextBox;

            return true;
        }

        #region XML Utilities
        private void ReadAppList()
        {
            if (!File.Exists(Path.Combine(Common.Path_AppData, "AppList.xml")))
                return;

            /////////// XML CODE
            using (XmlReader reader = XmlReader.Create(Path.Combine(Common.Path_AppData, "AppList.xml")))
            {
                // Find <appList>. If found, break.
                // We just do this to find if there's appList at all, first.
                // If there's not, XmlReader seeks to the end, and it can't read anymore
                while (reader.Read())
                    if (reader.IsStartElement("appList"))
                        break;

                // Under <appList>, find all <app> tags. Register their attributes
                while (reader.Read())
                {
                    if (reader.IsStartElement("app"))
                    {
                        // Try if the app's AppSettings.xml is parsable.
                        // If not, don't add the app.
                        string appId = reader.GetAttribute("id");
                        string appIdPath = Path.Combine(Common.Path_AppData, appId + "\\AppSettings.xml");
                        if (!File.Exists(appIdPath) || new FileInfo(appIdPath).Length <= 0) {
                            RecoverAppSettingsDir(appId, reader.GetAttribute("processName")); // you really just need the path filename
                            continue; 
                        }

                        Common.AppIds.Add(reader.GetAttribute("id"));
                        Common.AppProcessNames.Add(reader.GetAttribute("processName"));
                        //AppSingleModes.Add(Common.EvaluateBoolString(reader.GetAttribute("singleMode")));
                        //AppSpecialFeatures.Add(Common.EvaluateBoolString(reader.GetAttribute("specialFeatures")));
                        Common.AppCount++;

                        Common.Log("App added to list! | "
                            + Common.AppCount + " | "
                            + Common.AppIds.Last() + " | "
                            + Common.AppProcessNames.Last() + " | "
                            //+ AppSingleModes.Last().ToString() + " | "
                            //+ AppSpecialFeatures.Last().ToString()
                        );
                    }
                }
            }
            ///////////
        }

        private bool ReadAppSettings(string appId)
        {
            Common.Log("Reading app settings for: " + appId, 1);
            if (!File.Exists(Path.Combine(Common.Path_AppData, appId + "\\AppSettings.xml")))
                return false;

            using (XmlReader reader = XmlReader.Create(Path.Combine(Common.Path_AppData, appId + "\\AppSettings.xml")))
            {
                while (reader.Read())
                    if (reader.IsStartElement("appSettings"))
                        break;

                // Fill in CurrentApp data
                CurrentAppId = appId;
                CurrentAppName = reader["name"];
                CurrentAppPath = reader["path"];
                CurrentAppProcessName = Path.GetFileName(CurrentAppPath);
                CurrentAppWindowClassName = reader["className"];
                Common.Log(CurrentAppId + " | " + CurrentAppName + " | " + CurrentAppPath + " | " + CurrentAppProcessName);

                // Look for <jumpList>
                while (reader.Read())
                {
                    if (reader.IsStartElement("jumpList"))
                    {
                        reader.Read(); // Read to text node
                        string jumplistPath = Path.Combine(Common.Path_AppData, CurrentAppId + "\\" + reader.Value); Common.WriteDebug(jumplistPath);
                        ReadJumpList(jumplistPath);
                        break; // Since <jumpList> is all we're looking for right now
                    }
                }
            }

            Common.Log("Finished reading app settings!", -1);
            AppLoaded = true;
            return true;
        }

        public bool PackLoading = false;
        public string PackTempPath = "";

        public void ReadJumpList(string jumplistPath)
        {
            Common.Log("Reading jumplist at: " + jumplistPath, 1);
            if (!File.Exists(jumplistPath)) return;
            T7EJumplistItem jumplistItem = new T7EJumplistItem(this);

            // TODO:
            // 1a. T7EPreferences, if no icon path is set, tough: It's empty. Add button to
            // app to set icon to program's icon

            // Get XML file from appdata\[AppId]\jumplist.xml
            XmlReader reader = XmlReader.Create(jumplistPath);

            // Jumplist Packs: If user decides to ADD to list, these vars will correspond
            // to the positions where JLE should add new items
            // If JumplistListBox.Items.Length is 0, safe to assume no pack is being loaded.
            int currentItemPosition = 0;
            int tasksCategoryItemPosition = 0;
            int oldListBoxCount = 0;
            // Find the tasks category. Set currentItemPosition to its position.
            for (int i = 0; i < JumplistListBox.Items.Count; i++)
            {
                if (((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                {
                    currentItemPosition = i;
                    tasksCategoryItemPosition = i + 1;
                    oldListBoxCount = JumplistListBox.Items.Count;
                    break;
                }
            }

            try
            {
                // Find <jumpList>. If found, break.
                // We just do this to find if jumpList is the top element, first.
                // If there's not, XmlReader seeks to the end, and it can't read anymore
                while (reader.Read())
                    if (reader.IsStartElement("jumpList"))
                        break;

                // Under <jumpList>, find the following tags <category> and <tasks>.
                while (reader.Read())
                {
                    if (reader.IsStartElement("category"))
                    {
                        jumplistItem = new T7EJumplistItem(this);
                        jumplistItem.ItemName = reader["name"];
                        jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.Category;
                        JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                        currentItemPosition++;
                    }

                    else if (reader.IsStartElement("tasksCategory"))
                    {
                        if (tasksCategoryItemPosition > 0)
                        {// If there's already a tasks category, skip it.
                            currentItemPosition = JumplistListBox.Items.Count;
                            // add a separator, IF tasksCategory is not the end of the thing
                            if (tasksCategoryItemPosition < oldListBoxCount)
                            {
                                jumplistItem = new T7EJumplistItem(this);
                                jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.Separator;
                                JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                                currentItemPosition++;
                            }
                        }
                        else
                        {
                            jumplistItem = new T7EJumplistItem(this);
                            jumplistItem.ItemName = "Tasks";
                            jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.CategoryTasks;
                            JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                            currentItemPosition++;
                        }
                    }

                    else if (reader.IsStartElement("task"))
                    {
                        jumplistItem = new T7EJumplistItem(this);
                        jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.Task;
                        jumplistItem.ItemName = reader["name"];
                        // Now read for <icon> and <action>
                        while (reader.Read())
                        {
                            if (reader.IsStartElement("icon"))
                            {
                                // Consider translating this to norm. values
                                reader.Read();
                                string iconString = reader.Value;
                                if (PackLoading && File.Exists(Path.Combine(PackTempPath, iconString)))
                                {
                                    // HACK of the century: No matter what, the icon text WILL BE "icon.ico"
                                    // No "icon.ico|0" bullcrap
                                    // Copy the icon over to AppData\Icons\Imported
                                    
                                    File.Copy(Path.Combine(PackTempPath, iconString), Path.Combine(Common.Path_AppData, "Icons\\Imported\\"+iconString), true);
                                    iconString = Path.Combine(Common.Path_AppData, "Icons\\Imported\\" + iconString);
                                }
                                jumplistItem.StringToItemIcon(iconString);
                            }

                            else if (reader.IsStartElement("action"))
                            {
                                switch (reader["type"])
                                {
                                    case "T7E_TYPE_KBD":
                                        jumplistItem.TaskAction = T7EJumplistItem.ActionType.Keyboard;
                                        bool.TryParse(reader["ignoreAbsent"], out jumplistItem.TaskKBDIgnoreAbsent);
                                        bool.TryParse(reader["ignoreCurrent"], out jumplistItem.TaskKBDIgnoreCurrent);
                                        bool.TryParse(reader["newWindow"], out jumplistItem.TaskKBDNew);
                                        bool.TryParse(reader["isShortcut"], out jumplistItem.TaskKBDShortcutMode);
                                        reader.Read(); // Read to text node
                                        jumplistItem.TaskKBDString = reader.Value;
                                        break;

                                    case "T7E_TYPE_CMD":
                                        jumplistItem.TaskAction = T7EJumplistItem.ActionType.CommandLine;
                                        bool.TryParse(reader["showWindow"], out jumplistItem.TaskCMDShowWindow);
                                        reader.Read(); // Read to text node
                                        string cmdString = "";
                                        if (PackLoading) cmdString = Preferences.ReplaceVarsToPaths(reader.Value);
                                        else cmdString = Common.ReplaceEnvVarToExpandedPath(reader.Value);

                                        jumplistItem.StringToItemCmd(cmdString);
                                        break;

                                    case "T7E_TYPE_AHK":
                                        jumplistItem.TaskAction = T7EJumplistItem.ActionType.AutoHotKey;
                                        reader.Read(); // Read to text node
                                        string ahkPath;

                                        // If PackLoading, just load the AHK from the temp path
                                        if (PackLoading)
                                            ahkPath = Path.Combine(PackTempPath, reader.Value);
                                        else
                                            ahkPath = Path.Combine(Common.Path_AppData, CurrentAppId + "\\" + reader.Value);

                                        if (!File.Exists(ahkPath)) { MessageBox.Show(ahkPath);  break; }
                                        using (StreamReader ahkReader = new StreamReader(ahkPath))
                                        {
                                            jumplistItem.TaskAHKScript = ahkReader.ReadToEnd();
                                            if (PackLoading)
                                                jumplistItem.TaskAHKScript = Preferences.ReplaceVarsToPaths(jumplistItem.TaskAHKScript);
                                        }
                                        break;
                                }
                            }

                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "task")
                                break;
                        }

                        JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                        currentItemPosition++;
                    }

                    else if (reader.IsStartElement("link"))
                    {
                        jumplistItem = new T7EJumplistItem(this);
                        jumplistItem.ItemName = reader["name"];
                        bool.TryParse(reader["runWithApp"], out jumplistItem.FileRunWithApp);

                        // Now read for <icon> and <action>
                        while (reader.Read())
                        {
                            if (reader.IsStartElement("icon"))
                            {
                                reader.Read();
                                string iconString = reader.Value;
                                if (PackLoading && File.Exists(Path.Combine(PackTempPath, iconString)))
                                {
                                    // HACK of the century: No matter what, the icon text WILL BE "icon.ico"
                                    // No "icon.ico|0" bullcrap
                                    // Copy the icon over to AppData\Icons\Imported

                                    File.Copy(Path.Combine(PackTempPath, iconString), Path.Combine(Common.Path_AppData, "Icons\\Imported\\" + iconString), true);
                                    iconString = Path.Combine(Common.Path_AppData, "Icons\\Imported\\" + iconString);
                                }
                                jumplistItem.StringToItemIcon(iconString);
                            }

                            else if (reader.IsStartElement("location"))
                            {
                                reader.Read(); // Get text node
                                string locationPath = "";
                                if (PackLoading) locationPath = Preferences.ReplaceVarsToPaths(reader.Value);
                                else locationPath = Common.ReplaceEnvVarToExpandedPath(reader.Value);

                                if (!File.Exists(locationPath) && !Directory.Exists(locationPath)) break;
                                else jumplistItem.FilePath = locationPath;

                                // Check if directory
                                //FileAttributes attr = File.GetAttributes(locationPath);
                                //if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                                jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.FileFolder;
                            }

                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "link")
                                break;
                        }

                        JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                        currentItemPosition++;
                    }

                    else if (reader.IsStartElement("separator"))
                    {
                        jumplistItem = new T7EJumplistItem(this);
                        jumplistItem.ItemType = T7EJumplistItem.ItemTypeVar.Separator;
                        JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                        currentItemPosition++;
                    }

                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "jumpList")
                        break;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show("ReadJumpList error!\r\n"
                    + e.Message + "Jumplist will stop being read. ");
            }

            reader.Close();

            Common.Log("Finished reading jumplist.", -1);
        }
        #endregion

        private void OpenCurrentJumplistItem()
        {
            ItemNameTextBox.Text = _CurrentJumplistItem.ItemName;
            ItemIconComboBox.Text = _CurrentJumplistItem.ItemIconToString();
            ItemIconPictureBox.Image = (Image)(_CurrentJumplistItem.ItemIconBitmap);
            if (CurrentJumplistItem.ItemIconToString() == "Don't use an icon")
            {
                ItemIconComboBox.Location = new Point(ItemIconPictureBox.Location.X, ItemIconComboBox.Location.Y);
                ItemIconComboBox.Width = IconButtonBrowse.Location.X - ItemIconPictureBox.Location.X - 8;
                ItemIconPictureBox.Visible = false;
                ItemIconComboBox.SelectionLength = 0;
            }
            else
            {
                ItemIconComboBox.Location = new Point(ItemIconPictureBox.Location.X + ItemIconPictureBox.Width + 4,
                    ItemIconComboBox.Location.Y);
                ItemIconComboBox.Width = IconButtonBrowse.Location.X - ItemIconPictureBox.Location.X - ItemIconPictureBox.Width - 4 - 8;
                ItemIconPictureBox.Visible = true;
                ItemIconComboBox.SelectionLength = 0;
            }
            Programmatic = true;
            ItemTypeComboBox.SelectedIndex = (int)_CurrentJumplistItem.ItemType == 4 ?
                (int)T7EJumplistItem.ItemTypeVar.Category
                : (int)_CurrentJumplistItem.ItemType;
            Programmatic = false;
            // Fires off display of item panel

            FileTextBox.Text = _CurrentJumplistItem.FilePath;
            FileOpenDefault.Checked = !_CurrentJumplistItem.FileRunWithApp;
            FileOpenApp.Checked = _CurrentJumplistItem.FileRunWithApp;

            TaskActionComboBox.SelectedIndex = (int)_CurrentJumplistItem.TaskAction;
            // Fires off display of task panel
            TaskKBDIgnoreAbsentCheckBox.Checked = _CurrentJumplistItem.TaskKBDIgnoreAbsent;
            TaskKBDIgnoreCurrentCheckBox.Checked = _CurrentJumplistItem.TaskKBDIgnoreCurrent;
            TaskKBDNewCheckBox.Checked = _CurrentJumplistItem.TaskKBDNew;
            if (_CurrentJumplistItem.TaskKBDShortcutMode)
                TaskKBDSwitchToShortcutMode();
            else
                TaskKBDSwitchToTextMode();
            //
            TaskCMDTextBox.Text = _CurrentJumplistItem.ItemCmdToString();
            TaskCMDShowWindowCheckbox.Checked = _CurrentJumplistItem.TaskCMDShowWindow;
            //
            TaskAHKTextBox.Text = _CurrentJumplistItem.TaskAHKScript;

            // And then leave it to the textbox blurs to update values.
            // Upon save, take the data from the jumplist items
        }

        public void RefreshJumplistItem()
        {
            JumplistListBox.DisplayMember = "ItemName";
            JumplistListBox.DisplayMember = "";
        }

        public void ToggleFormState() {
            if (AppLoaded)
            {
                // Fill in app fields, enable fields, select jumplist object
                ProgramNameTextBox.Text = CurrentAppName;
                ProgramPathTextBox.Text = CurrentAppPath;
                ProgramSettingsPanel.Enabled = true;
                TabControl.SelectedIndex = 1;

                JumplistEnabled = true; // Fires off jumplist enabling
                JumplistListBox.SelectedIndex = 0; // Fires off jumplist loading.
            }
        }

        public void ToggleJumplistFormState()
        {
            if (JumplistEnabled)
            {
                // Jumplists are enabled. Enable jumplist panel, check factors to
                // see if ItemPanel and TaskGroupBox/FileGroupBox are to be visible
                JumplistPanel.Enabled = true;
                ItemTypePanel.Enabled = true;
                ItemPanel.Enabled = true;
            }
            else
            {
                JumplistPanel.Enabled = false;
                ItemTypePanel.Enabled = false;
                ItemPanel.Enabled = false;
            }
        }

        public void ToggleJumplistButtonState()
        {
            int selectedIndex = JumplistListBox.SelectedIndex;
            if (selectedIndex < 0) return;
            int totalItems = JumplistListBox.Items.Count;

            switch (((T7EJumplistItem)JumplistListBox.Items[selectedIndex]).ItemType)
            {
                case T7EJumplistItem.ItemTypeVar.CategoryTasks:
                    JumplistDeleteButton.Enabled
                        = JumplistUpButton.Enabled
                        = JumplistDownButton.Enabled
                        = false;
                    break;

                /*case T7EJumplistItem.ItemTypeVar.Category:
                    // Disable "Down" if TasksCategory is below
                    if (selectedIndex + 1 < totalItems
                        && ((T7EJumplistItem)JumplistListBox.Items[selectedIndex + 1]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks
                        )
                        JumplistDownButton.Enabled = false;
                    else
                        JumplistDownButton.Enabled = true;

                    JumplistDeleteButton.Enabled
                        = JumplistUpButton.Enabled
                        = true;
                    break;*/

                case T7EJumplistItem.ItemTypeVar.Separator:
                    // Disable "Up" if TasksCategory is above
                    if (selectedIndex - 1 >= 0
                        && ((T7EJumplistItem)JumplistListBox.Items[selectedIndex - 1]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks
                        )
                        JumplistUpButton.Enabled = false;
                    else
                        JumplistUpButton.Enabled = true;

                    JumplistDeleteButton.Enabled
                        = JumplistDownButton.Enabled
                        = true;
                    break;

                /*case T7EJumplistItem.ItemTypeVar.FileFolder:
                case T7EJumplistItem.ItemTypeVar.Task:
                    // Disable "up" if Category (BUT NOT TasksCategory) is above, and
                    // it's the very first item
                    if (selectedIndex - 1 <= 0
                        && ((T7EJumplistItem)JumplistListBox.Items[0]).ItemType == T7EJumplistItem.ItemTypeVar.Category
                        )
                        JumplistUpButton.Enabled = false;
                    else
                        JumplistUpButton.Enabled = true;

                    JumplistDeleteButton.Enabled
                        = JumplistDownButton.Enabled
                        = true;
                    break;*/

                // If not taskscategory or separator, set them all to true
                // Unless the item is first or last in the list: See later
                default:
                    JumplistDeleteButton.Enabled
                        = JumplistUpButton.Enabled
                        = JumplistDownButton.Enabled
                        = true;
                    break;
            }

            if (selectedIndex == 0)
                JumplistUpButton.Enabled = false;
            else if (selectedIndex + 1 == totalItems)
                JumplistDownButton.Enabled = false;
        }

        public void ToggleJumplistItemFormState()
        {
            if (JumplistListBox.SelectedIndex < 0) return;
            // Depending on the item type, enable fields and show grupboxes
            switch (CurrentJumplistItem.ItemType)
            {
                case T7EJumplistItem.ItemTypeVar.CategoryTasks:
                    ItemNameTextBox.Enabled 
                        = ItemTypeComboBox.Enabled
                        = false;

                    ItemNameTextBox.Enabled
                        = ItemIconComboBox.Enabled
                        = IconButtonBrowse.Enabled
                        = ItemTypeComboBox.Enabled
                        = ItemTypePanel.Enabled
                        = ItemTypePanel.Visible
                    = false;
                    break;

                case T7EJumplistItem.ItemTypeVar.Separator:
                    ItemNameTextBox.Enabled
                        = ItemIconComboBox.Enabled
                        = IconButtonBrowse.Enabled
                        = ItemTypePanel.Enabled
                        = ItemTypePanel.Visible
                    = false;
                    ItemTypeComboBox.Enabled = true;
                    break;
                case T7EJumplistItem.ItemTypeVar.Category:
                    ItemNameTextBox.Enabled 
                        = ItemTypeComboBox.Enabled
                        = true;

                    ItemIconComboBox.Enabled
                        = IconButtonBrowse.Enabled
                        = ItemTypePanel.Enabled
                        = ItemTypePanel.Visible
                    = false;
                    break;

                case T7EJumplistItem.ItemTypeVar.Task:
                    ItemNameTextBox.Enabled
                        = ItemIconComboBox.Enabled
                        = IconButtonBrowse.Enabled
                        = ItemTypeComboBox.Enabled
                    = true;

                    ItemTypePanel.Enabled = true;
                    ItemTypePanel.Visible = true;
                    FileGroupBox.Enabled = false;
                    FileGroupBox.Visible = false;

                    TaskGroupBox.Enabled = true;
                    TaskGroupBox.Visible = true;
                    break;
                case T7EJumplistItem.ItemTypeVar.FileFolder:
                    ItemNameTextBox.Enabled
                        = ItemIconComboBox.Enabled
                        = IconButtonBrowse.Enabled
                        = ItemTypeComboBox.Enabled
                    = true;

                    ItemTypePanel.Enabled = true;
                    ItemTypePanel.Visible = true;
                    TaskGroupBox.Enabled = false;
                    TaskGroupBox.Visible = false;

                    FileGroupBox.Enabled = true;
                    FileGroupBox.Visible = true;
                    break;
            }
        }

        public void ToggleJumplistItemTaskFormState()
        {
            switch ((T7EJumplistItem.ActionType)TaskActionComboBox.SelectedIndex)
            {
                case T7EJumplistItem.ActionType.Keyboard:
                    TaskKBDPanel.Enabled
                        = TaskKBDPanel.Visible
                    = true;

                    TaskCMDPanel.Enabled
                        = TaskCMDPanel.Visible
                        = TaskAHKPanel.Enabled
                        = TaskAHKPanel.Visible
                    = false;
                    break;
                case T7EJumplistItem.ActionType.CommandLine:
                    TaskCMDPanel.Enabled
                        = TaskCMDPanel.Visible
                    = true;

                    TaskKBDPanel.Enabled
                        = TaskKBDPanel.Visible
                        = TaskAHKPanel.Enabled
                        = TaskAHKPanel.Visible
                    = false;
                    break;
                case T7EJumplistItem.ActionType.AutoHotKey:
                    TaskAHKPanel.Enabled
                        = TaskAHKPanel.Visible
                    = true;

                    TaskCMDPanel.Enabled
                        = TaskCMDPanel.Visible
                        = TaskKBDPanel.Enabled
                        = TaskKBDPanel.Visible
                    = false;
                    break;
            }
        }
        #endregion

        #region Data Utilities
        private void OpenIconFile()
        {
            OpenFileDialog iconFileDialog = new OpenFileDialog();
            iconFileDialog.Filter = "All icon files (*.png;*.jpg;*.bmp;*.gif;*.ico;*.exe;*.dll;*.lnk)|*.png;*.jpg;*.gif;*.bmp;*.ico;*.exe;*.dll|Image files (*.png;*.jpg;*.bmp;*.gif;*.ico)|*.png;*.jpg;*.bmp;*.gif;*.ico|Program files (*.exe;*.dll;*.lnk)|*.exe;*.dll;*.lnk|All files (*.*)|*.*";
            iconFileDialog.FilterIndex = 0;
            if(Directory.Exists(Path.Combine(Common.Path_AppData, "Icons")))
                iconFileDialog.InitialDirectory = Path.Combine(Common.Path_AppData, "Icons");
            else // TODO: Consider adding "Icons" library.
                iconFileDialog.InitialDirectory = Path.Combine(Common.EnvPath_AllUsersProfile, "Microsoft\\Windows\\Start Menu");
            iconFileDialog.Title = "Select picture to convert to icon";
            iconFileDialog.AutoUpgradeEnabled = true;
            iconFileDialog.ShowHelp = true;
            iconFileDialog.DereferenceLinks = true;

            if (iconFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            string programFileName = iconFileDialog.FileName;
            iconFileDialog.Dispose();

            // If programFileName is a shortcut, get its target path
            if (Path.GetExtension(programFileName).ToLower() == ".lnk")
            {
                programFileName = Common.ResolveLnkPath(programFileName);
            }

            // Get iconResourceNum from iconpickdialog
            string programFileExtension = Path.GetExtension(programFileName).ToLower();
            switch (programFileExtension)
            {
                case ".dll":
                case ".exe":
                case ".ico":
                    int iconIndex = 0;
                    int iconCount = (int)ExtractIconEx(programFileName, -1, null, null, 0);

                    StringBuilder sb = new StringBuilder(programFileName, 500);

                    if (iconCount > 1)
                    {
                        int retVal = PickIconDlg(this.Handle, sb, sb.Capacity, ref iconIndex);
                        programFileName = sb.ToString();
                        if (retVal > 0) ChangeIcon(programFileName + "|" + iconIndex.ToString());
                    }
                    else
                    {
                        ChangeIcon(programFileName + "|" + iconIndex.ToString());
                    }
                    break;

                case ".png":
                case ".jpg":
                case ".gif":
                case ".bmp":
                    ChangeIcon(programFileName);
                    break;
            }
        }

        private void ChangeIcon(string iconString)
        {
            if (IconStringIsDifferent(iconString))
                CurrentJumplistItem.StringToItemIcon(iconString);
            // Icon path/index is different. Fires off bitmap creation

            //if (ItemIconComboBox.Focused)
            //IconButtonBrowse.Focus();
            ItemIconComboBox.Text = CurrentJumplistItem.ItemIconToString();
            ItemIconPictureBox.Image = (Image)(CurrentJumplistItem.ItemIconBitmap);
            //ItemIconComboBox.ImageList.Images[0] = (Image)(CurrentJumplistItem.ItemIconBitmap);
            //ItemIconComboBox.Invalidate();
            //ItemIconImage
        }

        private bool IconStringIsDifferent(string iconString)
        {
            string[] iconStringSplit = iconString.Split('|');

            string iconPath = "";
            int iconIndex = 0;
            string origIconPath = CurrentJumplistItem.ItemIconPath;
            int origIconIndex = CurrentJumplistItem.ItemIconIndex;

            if (iconString.Equals("Use program icon", StringComparison.CurrentCultureIgnoreCase))
            {
                iconPath = CurrentAppPath;
                iconIndex = 0;
            }
            else if (iconString.Equals("Don't use an icon", StringComparison.CurrentCultureIgnoreCase))
            {
                iconPath = "";
                iconIndex = 0;
            }
            else
            {
                iconPath = iconStringSplit.Length < 1 ? "" : iconStringSplit[0];
                if (iconStringSplit.Length >= 2)
                    int.TryParse(iconStringSplit[1], out iconIndex);
            }

            if (iconPath.Equals(origIconPath, StringComparison.CurrentCultureIgnoreCase)
                && iconIndex == origIconIndex)
                return false;
            else
                return true;

        }

        private void OpenFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            
            fileDialog.Filter = "All files (*.*)|*.*";
            fileDialog.FilterIndex = 0;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileDialog.Title = "Select a file.";
            fileDialog.AutoUpgradeEnabled = true;
            fileDialog.ShowHelp = true;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                fileDialog.Dispose();
                return;
            }

            string programFileName = fileDialog.FileName;

            //if(programFileName.Equals("Select a file, or click \"OK\" to select the directory."))

            CurrentJumplistItem.FilePath = programFileName;
            FileTextBox.Text = programFileName;
            fileDialog.Dispose();
        }

        private void CheckJumplistItemPosition()
        {
            // Finally, check if the first item doesn't break a rule. If it's not a cat,
            // MAKE it a cat.
            if (!Programmatic && JumplistListBox.Items.Count >= 1
                && ((T7EJumplistItem)JumplistListBox.Items[0]).ItemType != T7EJumplistItem.ItemTypeVar.Category
                && ((T7EJumplistItem)JumplistListBox.Items[0]).ItemType != T7EJumplistItem.ItemTypeVar.CategoryTasks
            )
            {
                ((T7EJumplistItem)JumplistListBox.Items[0]).ItemType = T7EJumplistItem.ItemTypeVar.Category; // 'nuff said.
                RefreshJumplistItem();
            }

            int currentIndex = JumplistListBox.SelectedIndex;
            bool typeMatch = false;
            if (CurrentJumplistItem == null) return;
            switch (CurrentJumplistItem.ItemType)
            {
                //case T7EJumplistItem.ItemTypeVar.CategoryTasks:
                case T7EJumplistItem.ItemTypeVar.Category:
                    // Is it below the Tasks category?
                    for (int i = currentIndex; i >= 0; i--)
                    {
                        if (((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                        {
                            object currentItem = JumplistListBox.SelectedItem;
                            JumplistListBox.Items.RemoveAt(currentIndex);
                            JumplistListBox.Items.Insert(i, currentItem);
                            JumplistListBox.SelectedIndex = i;
                            break;
                        }
                    }
                    break;
                case T7EJumplistItem.ItemTypeVar.Separator:
                    // Is there tasks category above us? Move below tasks category.
                    for (int i = currentIndex; i >= 0; i--)
                    {
                        if (((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                            typeMatch = true;
                    }

                    if (typeMatch) break;
                    
                    // Else, there is no tasks category. Move it below one.
                    for (int j = currentIndex; j < JumplistListBox.Items.Count; j++)
                    {
                        if (((T7EJumplistItem)JumplistListBox.Items[j]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                        {
                            object currentItem = JumplistListBox.SelectedItem;
                            JumplistListBox.Items.RemoveAt(currentIndex);
                            JumplistListBox.Items.Insert(j, currentItem);
                            JumplistListBox.SelectedIndex = j;
                            break;
                        }
                    }
                    break;
                case T7EJumplistItem.ItemTypeVar.Task:
                case T7EJumplistItem.ItemTypeVar.FileFolder:
                    // Is there no category above us? Move below closest category
                    for (int i = currentIndex; i >= 0; i--)
                    {
                        if (((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks
                            || ((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.Category
                        )
                            typeMatch = true;
                    }

                    if (typeMatch) break;

                    // Else, there is no category. Move it below one.
                    // Don't move it below one: Make it the category
                    for (int j = currentIndex; j < JumplistListBox.Items.Count; j++)
                    {
                        if (((T7EJumplistItem)JumplistListBox.Items[j]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks
                            || ((T7EJumplistItem)JumplistListBox.Items[j]).ItemType == T7EJumplistItem.ItemTypeVar.Category)
                        {
                            object currentItem = JumplistListBox.SelectedItem;
                            JumplistListBox.Items.RemoveAt(currentIndex);
                            JumplistListBox.Items.Insert(j, currentItem);
                            JumplistListBox.SelectedIndex = j;
                            break;
                        }
                    }
                    break;
            }
        }
        #endregion

        /////////////////////////////////
        
        private int EditCount = 0;

        #region UI-Handlers
        private void MenuToolsAbout_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog(this);
        }

        private void MenuFileOpen_Click(object sender, EventArgs e)
        {
            OpenManageProgramFile();
        }

        private void JumplistListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Read selected item, populate item fields
            CurrentJumplistItem = (T7EJumplistItem)JumplistListBox.SelectedItem;
                // Fires off jumplist reading.
            ToggleJumplistButtonState();
            ToggleJumplistItemFormState();
        }

        //////////

        private void ItemTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_CurrentJumplistItem.ItemType != T7EJumplistItem.ItemTypeVar.CategoryTasks)
                _CurrentJumplistItem.ItemType = (T7EJumplistItem.ItemTypeVar)ItemTypeComboBox.SelectedIndex;
            ToggleJumplistItemFormState();

            RefreshJumplistItem();

            if (!Programmatic) { Programmatic = true; CheckJumplistItemPosition(); Programmatic = false; }

            //EditCount++;
        }

        private void TaskActionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CurrentJumplistItem.TaskAction = (T7EJumplistItem.ActionType)TaskActionComboBox.SelectedIndex;
            ToggleJumplistItemTaskFormState();

            //EditCount++;
        }
        #endregion

        #region Data-Modifying Handlers
        bool AppLoaded
        {
            get { return _AppLoaded; }
            set
            {
                _AppLoaded = value;
                ToggleFormState();
            }
        }

        public string CurrentAppName
        {
            get { return _CurrentAppName; }
            set { _CurrentAppName = value; ProgramNameTextBox.Text = value; }
        }

        public string CurrentAppPath
        {
            get { return _CurrentAppPath; }
            set { _CurrentAppPath = value; ProgramPathTextBox.Text = value; }
        }

        private void ProgramNameTextBox_Leave(object sender, EventArgs e)
        {
            ChangeAppName(((TextBox)sender).Text);
        }

        private void ChangeAppName(string newAppName)
        {
            string oldAppName = _CurrentAppName;

            if (oldAppName == newAppName) return;

            // Replace AHK appname
            foreach (T7EJumplistItem jlio in JumplistListBox.Items)
            {
                jlio.TaskAHKScript = jlio.TaskAHKScript.Replace(
                    "JLE_AppName := \"" + oldAppName + "\"",
                    "JLE_AppName := \"" + newAppName + "\""
                    );

                jlio.TaskAHKScript = jlio.TaskAHKScript.Replace(
                    "; AutoHotKey Script - For \"" + oldAppName + "\"",
                    "; AutoHotKey Script - For \"" + newAppName + "\""
                    );

                if (CurrentJumplistItem.Equals(jlio)) // Update UI elements
                    TaskAHKTextBox.Text = jlio.TaskAHKScript;
            }

            //Everything else is auto-generated.

            _CurrentAppName = newAppName;
            ProgramNameTextBox.Text = newAppName;

            ChangeStatusBarText("Program name changed successfully! If applicable, check your AutoHotKey scripts to verify changes.");
        }

        System.ComponentModel.BackgroundWorker StatusBarBackgroundWorker;

        private void ChangeStatusBarText(string statusText)
        {
            ToolBarStatusLabel.Text = statusText;
            StatusBarBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            StatusBarBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(StatusBarBackgroundWorker_DoWork);
            StatusBarBackgroundWorker.RunWorkerAsync(null);
        }

        void StatusBarBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(10000);
            ToolBarStatusLabel.Text = "Ready";
        }

        private void ProgramPathTextBox_Leave(object sender, EventArgs e)
        {
            ChangeAppPath(((TextBox)sender).Text);
        }

        private void ChangeAppPath(string newAppPath)
        {
            string oldAppPath = _CurrentAppPath;

            if (oldAppPath == newAppPath) return;
            if (!File.Exists(newAppPath)) { ProgramPathTextBox.Text = oldAppPath; return; }

            bool processNameNotSame = !Path.GetFileName(oldAppPath).Equals(Path.GetFileName(newAppPath), StringComparison.OrdinalIgnoreCase);
            string oldAppId = ""; string newAppId = "";
            if (processNameNotSame)
            {
                
                // 
                //ActiveControl = ProgramNewOKButton;
                // Only have to ask if the save directory actually exists.
                if (!Programmatic && Directory.Exists(Path.Combine(Common.Path_AppData, oldAppId)))
                {
                    if (MessageBox.Show("The settings for " + (CurrentAppName.Length > 0 ? CurrentAppName : CurrentAppProcessName)
                        + " must be saved before changing the program path. Do you want to save the settings now?"
                        , "Save"
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Information
                        , MessageBoxDefaultButton.Button1) != System.Windows.Forms.DialogResult.Yes)
                    {
                        ProgramPathTextBox.Text = oldAppPath; return;
                    } // else
                }

                oldAppId = CurrentAppId;
                newAppId = Common.GetAppIdForProgramFile(newAppPath);

                // Check if this program is already registered.
                if (Directory.Exists(Path.Combine(Common.Path_AppData, newAppId)))
                {
                    MessageBox.Show("The program settings for " + Path.GetFileNameWithoutExtension(newAppPath) + " already exist.", "Program settings already exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            foreach (T7EJumplistItem jlio in JumplistListBox.Items)
            {
                // Replace "Use current icon" entries
                if (jlio.ItemIconPath.Equals(oldAppPath, StringComparison.CurrentCultureIgnoreCase)
                    && jlio.ItemIconIndex == 0)
                {
                    if (CurrentJumplistItem.Equals(jlio))
                    {
                        ChangeIcon(newAppPath + "|0"); // Also changes UI elements
                        ItemIconComboBox.Text = "Use program icon"; // Workaround, since ChangeIcon() uses old app path
                    }
                    else
                        jlio.StringToItemIcon(newAppPath + "|0"); // Just changes data
                }

                // Replace AHK appname
                string oldAppPathTest = oldAppPath;
                string newAppPathTest = newAppPath;
                jlio.TaskAHKScript = jlio.TaskAHKScript.Replace(
                    "JLE_AppPath := \"" + oldAppPathTest + "\"",
                    "JLE_AppPath := \"" + newAppPathTest + "\"");
                jlio.TaskAHKScript = jlio.TaskAHKScript.Replace(
                    "JLE_AppProcessName := \"" + Path.GetFileName(oldAppPath) + "\"",
                    "JLE_AppProcessName := \"" + Path.GetFileName(newAppPath) + "\""
                    );

                // While we're here, we're changing the appid too.
                if (processNameNotSame)
                {
                    jlio.TaskAHKScript = jlio.TaskAHKScript.Replace(
                        "JLE_AppId := \"" + oldAppId + "\"",
                        "JLE_AppId := \"" + newAppId + "\""
                        );
                }

                if (CurrentJumplistItem.Equals(jlio)) // Update UI elements
                    TaskAHKTextBox.Text = jlio.TaskAHKScript;
            }

            // Replace appprocessname
            string oldAppProcessName = Path.GetFileNameWithoutExtension(oldAppPath);
            for (int i = 0; i < Common.AppProcessNames.Count; i++)
            {
                if (Common.AppProcessNames[i].Equals(oldAppProcessName, StringComparison.OrdinalIgnoreCase))
                {
                    Common.AppProcessNames[i] = Path.GetFileNameWithoutExtension(newAppPath);
                    break;
                }
            }

            _CurrentAppPath = newAppPath;

            if (processNameNotSame)
            {
                // also replace appid
                for (int i = 0; i < Common.AppIds.Count; i++)
                {
                    if (Common.AppIds[i].Equals(oldAppId, StringComparison.OrdinalIgnoreCase))
                    {
                        Common.AppIds[i] = newAppId;
                        break;
                    }
                }

                CurrentAppId = newAppId; // and update teh UI element
                CurrentAppProcessName = Path.GetFileNameWithoutExtension(newAppPath);

                // Move appsettings dir, save app
                // We only have to do this if the app is actually saved.
                if(Directory.Exists(Path.Combine(Common.Path_AppData, oldAppId))) {
                    Directory.Move(Path.Combine(Common.Path_AppData, oldAppId), Path.Combine(Common.Path_AppData, newAppId));
                    Preferences.SaveApp(this); // This also rewrites AppList.xml, so T7EBackground picks up on the change.                
                    Preferences.ApplyJumplistToTaskbar(this); // And apply the jumplist to the new appid
                    Preferences.EraseJumplist(oldAppId); // Erase the old jumplist, too.
                }
            }

            ProgramPathTextBox.Text = CurrentAppPath;
            ChangeStatusBarText("Program path changed successfully! If applicable, check your AutoHotKey scripts to verify changes.");
        }

        ////////////

        public bool JumplistEnabled
        {
            get { return _JumplistEnabled; }
            set
            {
                _JumplistEnabled = value;
                Programmatic = true; 
                JumplistEnableCheckBox.Checked = value;
                Programmatic = false;
                ToggleJumplistFormState();
            }
        }

        private void JumplistEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!Programmatic)
            {
                _JumplistEnabled = JumplistEnableCheckBox.Checked;
                ToggleJumplistFormState();
            }
        }

        T7EJumplistItem CurrentJumplistItem
        {
            get { return _CurrentJumplistItem; }
            set
            {
                _CurrentJumplistItem = value;

                if(_CurrentJumplistItem != null)
                    OpenCurrentJumplistItem();
                // else
                //    blankjumplist.
            }
        }

        ////////////

        private void ItemNameTextBox_Leave(object sender, EventArgs e)
        {
            CurrentJumplistItem.ItemName = ItemNameTextBox.Text;
            RefreshJumplistItem();
        }

        private void ItemIconComboBox_Leave(object sender, EventArgs e)
        {
            ChangeIcon(ItemIconComboBox.Text);
            if (CurrentJumplistItem.ItemIconToString() == "Don't use an icon")
            {
                ItemIconComboBox.Location = new Point(ItemIconPictureBox.Location.X, ItemIconComboBox.Location.Y);
                ItemIconComboBox.Width = IconButtonBrowse.Location.X - ItemIconPictureBox.Location.X - 8;
                ItemIconPictureBox.Visible = false;
            }
            else
            {
                ItemIconComboBox.Location = new Point(ItemIconPictureBox.Location.X + ItemIconPictureBox.Width + 4,
                    ItemIconComboBox.Location.Y);
                ItemIconComboBox.Width = IconButtonBrowse.Location.X - ItemIconPictureBox.Location.X - ItemIconPictureBox.Width - 4 - 8;
                ItemIconPictureBox.Visible = true;
            }
        }

        private void TaskAHKTextBox_Leave(object sender, EventArgs e)
        {
            // Make this a Leave, or TextChanged handler?
            CurrentJumplistItem.TaskAHKScript = TaskAHKTextBox.Text;
        }

        private void TaskAHKSaveCopyButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveAhkDialog = new SaveFileDialog();
            saveAhkDialog.Filter = "AutoHotKey scripts (*.ahk;*.txt)|*.ahk;*.txt|All files (*.*)|*.*";
            saveAhkDialog.FilterIndex = 0;
            saveAhkDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveAhkDialog.Title = "Save AutoHotKey script";
            saveAhkDialog.AutoUpgradeEnabled = true;
            saveAhkDialog.ShowHelp = true;


            if (saveAhkDialog.ShowDialog() != DialogResult.OK)
            {
                saveAhkDialog.Dispose();
                return;
            }

            string fileName = saveAhkDialog.FileName;

            TextWriter tw = new StreamWriter(fileName);
            tw.Write(TaskAHKTextBox.Text);
            tw.Close();

            saveAhkDialog.Dispose();
        }

        string TaskAHKExternalTempName = "";
        FileSystemWatcher TaskAHKExternalWatcher;
        Process TaskAHKExternalProcess;

        [DllImport("shell32.dll", EntryPoint = "FindExecutable")]
        public static extern long FindExecutableA(
            string lpFile, string lpDirectory, StringBuilder lpResult);

        public static string FindExecutable(
            string pv_strFilename)
        {
            StringBuilder objResultBuffer =
                new StringBuilder(1024);
            long lngResult = 0;

            lngResult =
                FindExecutableA(pv_strFilename,
                string.Empty, objResultBuffer);

            if (lngResult >= 32)
            {
                return objResultBuffer.ToString();
            }

            return "";
        }

        private void TaskAHKOpenExternalButton_Click(object sender, EventArgs e)
        {
            TaskAHKTextBox.SelectAll();
            TaskAHKTextBox.Focus();
            Clipboard.SetText(TaskAHKTextBox.Text);
            return;

            // Too lazy to do any actual temp file managing
            // The issue is, what if you want to switch to another jumplist item, WHILE Notepad is open?
            // Extender can only work on one jumplist item at a time. By the time Notepad saves,
            // Extender doesn't know where to save the AHK.
            // I can rectify this, but not now. Rather just tell the person to "Copy to Clipboard."
            // TODO

            TaskAHKExternalTempName = Path.GetTempFileName();
            TextWriter tw = new StreamWriter(TaskAHKExternalTempName);
            tw.Write(TaskAHKTextBox.Text);
            tw.Close();

            TaskAHKExternalWatcher = new FileSystemWatcher(Path.GetDirectoryName(TaskAHKExternalTempName), Path.GetFileName(TaskAHKExternalTempName));
            TaskAHKExternalWatcher.NotifyFilter = NotifyFilters.LastWrite;
            TaskAHKExternalWatcher.Changed += new FileSystemEventHandler(TaskAHKExternalWatcher_Changed);
            TaskAHKExternalWatcher.EnableRaisingEvents = true;

            // Find better ways to get the associated app. For now, default to notepad.
            TaskAHKExternalProcess = Process.Start("notepad.exe", "\"" + TaskAHKExternalTempName + "\"");
            TaskAHKExternalProcess.Exited += new EventHandler(TaskAHKExternalProcess_Exited);
        }

        void TaskAHKExternalProcess_Exited(object sender, EventArgs e)
        {
            TaskAHKExternalWatcher.Dispose();
            //File.Delete(TaskAHKExternalTempName);
            TaskAHKExternalTempName = "";
            TaskAHKExternalProcess.Dispose();
        }

        void TaskAHKExternalWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            //Crc32 crc32Object = new Crc32();
            //crc32Object.ComputeHash(

            DialogResult ahkResult = MessageBox.Show("Do you want to apply the recently saved changes for " + CurrentJumplistItem.ItemName + "?", "Apply AutoHotKey script", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

            if (ahkResult == DialogResult.No) return;
            // else yes

            TextReader ahkReader = new StreamReader(TaskAHKExternalTempName);
            TaskAHKTextBox.Focus();
            TaskAHKTextBox.Text = ahkReader.ReadToEnd();
            ActiveControl = null;
            ahkReader.Close();
        }

        private void TaskKBDTextBox_Leave(object sender, EventArgs e)
        {
            CurrentJumplistItem.TextStringToTaskKBDString(TaskKBDTextBox.Text);
        }

        private void TaskCMDTextBox_Leave(object sender, EventArgs e)
        {
            CurrentJumplistItem.StringToItemCmd(TaskCMDTextBox.Text);
        }

        private void TaskCMDShowWindowCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CurrentJumplistItem.TaskCMDShowWindow = TaskCMDShowWindowCheckbox.Checked;
        }

        private void FileTextBox_Leave(object sender, EventArgs e)
        {
            CurrentJumplistItem.FilePath = FileTextBox.Text;
        }

        private void TaskKBDHelpButton_Click(object sender, EventArgs e)
        {
            string helpTitle = "Keyboard Stroke Help";
            string helpText = @"Type simple keyboard shortcuts or macros exactly as you want them to appear. There are two input modes provided: SHORTCUT MODE and TEXT MODE.

========================

SHORTCUT MODE

Allows you to press keyboard shortcuts exactly as they will be sent to the program. For example:

SIMPLE SHORTCUTS: Press Ctrl+C to send Ctrl+C to the program.

------------------

MENU SHORTCUTS: To access ""Save As"" in some programs, press Alt+F, then A. 

This accesses the ""File"" menu by pressing Alt+F, and selects the ""Save As"" entry by pressing the A key. Most program menus are accessed using similar combinations.

------------------

""RIBBON""/OFFICE SHORTCUTS: Press Alt by itself, then the separate letter(s) to reach your desired command. 

For instance, to access the ""Page Margin"" window in Word 2007, press Alt by itself, then P, then M, then A. 

Alt activates Word's menu shortcuts; P selects ""Page Layout;"" M selects ""Margins;"" and A selects ""Custom Margins."" Most Office 2007, 2010, and other ""Ribbon"" programs use similar combinations.

------------------

Note that the below keys, by themselves, cannot be recorded as shortcuts:
* Arrow keys, Home/End, Page Up/Page Down
* Backspace, Delete
* Tab, Shift+Tab
* Alt+Tab

The above keys will have their usual effect when editing, but they can be recorded when put together with the Control, Alt, Shift, or Windows keys. To record these keys by themselves, please see TEXT MODE.

========================

TEXT MODE

Type text macros or advanced keyboard shortcuts, exactly as they will shown. One line break is interpreted as one {Enter} key. You can type commonly repeated text this way -- for example:

John Doe
123 Fake St
New York, NY 10001

Advanced keyboard shortcuts and unrecordable keys (see above) can also be typed here, in AutoHotKey format. 
The aforementioned keys are inputted between {} brackets. Keys can also be preceded by Control, Alt, Shift, and Windows keys, by using certain symbols. Examples:

Up ==> {Up}
Backspace ==> {Backspace}
Tab ==> {Tab}
Ctrl + O ==> ^o
Alt + P ==> !p
Shift + Tab ==> +{tab}
Ctrl + Alt + Shift + Enter ==> ^!+{enter}
Win + D ==> #d

For more information on valid AHK keys and keystrokes, visit:
http://www.autohotkey.com/docs/commands/Send.htm

For more advanced features than these two modes allow, use ""Run AutoHotKey Script.""
";

            HelpDialog helpDialog = new HelpDialog(helpTitle, helpText);
            helpDialog.Show();
        }

        private void TaskKBDNewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CurrentJumplistItem.TaskKBDNew = TaskKBDNewCheckBox.Checked;
        }

        private void IconButtonBrowse_Click(object sender, EventArgs e)
        {
            OpenIconFile();
        }

        private void ItemIconComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemIconComboBox.Text == "Select icon from file (or type in path to file...)")
            {
                OpenIconFile();
            }
            else
            {
                ChangeIcon(ItemIconComboBox.Text);
            }

            if (CurrentJumplistItem.ItemIconToString() == "Don't use an icon")
            {
                ItemIconComboBox.Location = new Point(ItemIconPictureBox.Location.X, ItemIconComboBox.Location.Y);
                ItemIconComboBox.Width = IconButtonBrowse.Location.X - ItemIconPictureBox.Location.X - 8;
                ItemIconPictureBox.Visible = false;
            }
            else
            {
                ItemIconComboBox.Location = new Point(ItemIconPictureBox.Location.X + ItemIconPictureBox.Width + 4,
                    ItemIconComboBox.Location.Y);
                ItemIconComboBox.Width = IconButtonBrowse.Location.X - ItemIconPictureBox.Location.X - ItemIconPictureBox.Width - 4 - 8;
                ItemIconPictureBox.Visible = true;
            }
        }
        #endregion

        private void JumplistUpButton_Click(object sender, EventArgs e)
        {
            int currentIndex = JumplistListBox.SelectedIndex;
            if (currentIndex <= 0) return;
            object currentItem = JumplistListBox.SelectedItem;
            JumplistListBox.Items.RemoveAt(currentIndex);
            JumplistListBox.Items.Insert(currentIndex-1 < 0 ? 0 : currentIndex-1, currentItem);
            JumplistListBox.SelectedIndex = currentIndex-1 < 0 ? 0 : currentIndex-1;

            CheckJumplistItemPosition();
        }

        private void JumplistDownButton_Click(object sender, EventArgs e)
        {
            int currentIndex = JumplistListBox.SelectedIndex;
            if (currentIndex < 0
                || currentIndex + 1 >= JumplistListBox.Items.Count) return;
            object currentItem = JumplistListBox.SelectedItem;
            JumplistListBox.Items.RemoveAt(currentIndex);
            JumplistListBox.Items.Insert(currentIndex + 1, currentItem);
            JumplistListBox.SelectedIndex = currentIndex + 1;

            // If jumplist item above is taskscategory, set this one to task
            if (((T7EJumplistItem)JumplistListBox.Items[currentIndex]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks
                && ((T7EJumplistItem)JumplistListBox.SelectedItem).ItemType == T7EJumplistItem.ItemTypeVar.Category) 
            {
                ((T7EJumplistItem)JumplistListBox.SelectedItem).ItemType = T7EJumplistItem.ItemTypeVar.Task;
                RefreshJumplistItem();
            }

            CheckJumplistItemPosition();
        }

        private void JumplistDeleteButton_Click(object sender, EventArgs e)
        {
            DeleteCurrentJumplistItem();
        }

        private void DeleteCurrentJumplistItem()
        {
            if (((T7EJumplistItem)JumplistListBox.SelectedItem).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks) return;

            int currentIndex = JumplistListBox.SelectedIndex;
            JumplistListBox.Items.RemoveAt(currentIndex);
            CheckJumplistItemPosition();
            if (currentIndex >= JumplistListBox.Items.Count) currentIndex--;
            JumplistListBox.SelectedIndex = currentIndex;
            JumplistListBox.SelectedIndex = currentIndex;
        }

        private void MenuFileSave_Click(object sender, EventArgs e)
        {
            saveAndApplyToTaskbarToolStripMenuItem_Click(sender, e);
            return;

            Preferences.SaveApp(this);
            AppReallyNew = false;
            ChangeStatusBarText("Settings saved");
            EditCounter++;
        }

        private void saveAndApplyToTaskbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preferences.SaveApp(this);
            Preferences.ApplyJumplistToTaskbar(this);
            AppReallyNew = false;
            ChangeStatusBarText("Settings saved to taskbar! Right-click your program's shortcut to test it.");
            EditCounter++;
        }

        private void FileBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(OpenProgramFile())
                SelectMainAppWindow(true);
        }

        private void TabControl_SizeChanged(object sender, EventArgs e)
        {
            TabControl.Invalidate(); // issue 7: Tab control doesn't redraw when maximize
        }

        private void MenuFileExport_Click(object sender, EventArgs e)
        {
            ExportPack();
        }

        private bool ImportPack()
        {
            return ImportPack("", "");
        }
        private bool ImportPack(string appName)
        {
            return ImportPack(appName, "");
        }
        private bool ImportPack(string appName, string packPath)
        {
            // 1. Show an open dialog
            if (packPath == null || packPath.Length <= 0
                || !File.Exists(packPath))
            {
                OpenFileDialog fileDialog = new OpenFileDialog();

                fileDialog.Filter = "Jumplist Pack (*.jlp)|*.jlp|All files (*.*)|*.*";
                fileDialog.FilterIndex = 0;
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                fileDialog.Title = "Open";
                fileDialog.AutoUpgradeEnabled = true;
                fileDialog.ShowHelp = true;

                if (fileDialog.ShowDialog() != DialogResult.OK)
                {
                    fileDialog.Dispose();
                    return false;
                }
                packPath = fileDialog.FileName;
                fileDialog.Dispose();
            }

            // Preload jumplist pack information
            string[] packInfo = Preferences.LoadJumplistPackInfo(packPath);
            if (packInfo[0].Equals("Fail:{21d2ea6f-857c-4616-a0ba-e0d5a7475c3a}"))
            {
                MessageBox.Show(Path.GetFileName(packPath) + " was detected as corrupt. Try to download the Jumplist Pack again."
                    , "Error"
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Error);
                return false;
            }

            // 2. Show the import form, asking:
            // Import to new app?
            // Import to existing app? (if CurrentAppId is not empty)
            bool loadNewApp = false;
            bool replaceJumplist = false;
            string prospectiveAppName = appName != null && appName.Length > 0 ? appName : CurrentAppName;

            // See if app already has a list. If it does, load it.
            if (!AppLoaded && packInfo[1].Length > 0)
            {
                for (int j = 0; j < Common.AppProcessNames.Count; j++)
                {
                    if (Common.AppProcessNames[j].Equals(Path.GetFileNameWithoutExtension(packInfo[1]), StringComparison.OrdinalIgnoreCase))
                    {
                        OpenAppId(Common.AppIds[j], ""); // If successful, AppLoaded becomes true
                        prospectiveAppName = CurrentAppName;
                        break;
                    }
                }
            }

            if (AppLoaded) // ???
            {
                ImportForm importF = new ImportForm(prospectiveAppName);
                DialogResult importFResult = importF.ShowDialog();
                if (importFResult != System.Windows.Forms.DialogResult.OK) { importF.Dispose(); return false; }

                loadNewApp = importF.NewProgram;
                if (!loadNewApp) replaceJumplist = importF.ReplaceJumplist;
                else replaceJumplist = false; // Must default to "add", for accidental "existing" lists
                importF.Dispose();
            }
            else
            {
                loadNewApp = true;
                replaceJumplist = false;
            }

            // Load new app here, if required
            // TODO: For app-specific packs: If they have pre-loaded fields -- name, path, class --
            // LOAD THEM UP, INSTEAD OF DOING A STUPID "SELECT WINDOW"
            // But is this desirable? Is it better for the user to select the window?
            if (loadNewApp)
            {
                if (!OpenProgramFile()) return false; // Closes open app, opens new app
                else if (AppReallyNew)
                {

                    //MessageBox.Show(packInfo[0] + "|" + packInfo[1] + "|" + packInfo[2]);
                    if (packInfo[0].Length <= 0) SelectMainAppWindow(true);
                    else
                    {
                        ChangeAppName(packInfo[0]);
                        ChangeAppPath(packInfo[1]);
                        ChangeAppClass(packInfo[2]);
                    }
                } // If pack is app-specific, it'll have name and path and class fields. Just load those.
            }
            else
            {
                // Depending on attributes, check them out.
                // Compare the pack path and loaded path.
                if (packInfo[1].Length > 0 && !Path.GetFileName(packInfo[1]).Equals(Path.GetFileName(CurrentAppPath), StringComparison.OrdinalIgnoreCase))
                {
                    if (MessageBox.Show("This pack was not designed for " + CurrentAppName + ". Do you want to import it anyway?"
                        , "Import"
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Exclamation
                        , MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                    {
                        return false;
                    }

                }
            }

            // If replace jumplist, erase the old one
            if (replaceJumplist
                || AppReallyNew)
            {
                JumplistListBox.Items.Clear();
            }

            // Load the pack!
            Preferences.LoadJumplistPack(packPath);

            //if (AppReallyNew) saveAndApplyToTaskbarToolStripMenuItem.PerformClick(); // One-click importing for new apps

            // Select the jumplist tab when done!
            if(loadNewApp) TabControl.SelectedIndex = 1;
            // Select the first task
            for (int i = 0; i < JumplistListBox.Items.Count; i++)
            {
                if (((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.Task
                    || ((T7EJumplistItem)JumplistListBox.Items[i]).ItemType == T7EJumplistItem.ItemTypeVar.FileFolder)
                {
                    JumplistListBox.SelectedIndex = i;
                    return true;
                }
            }
            JumplistListBox.SelectedIndex = 0;
            return true;
        }

        private void ExportPack()
        {
            // 1. Show a save dialog
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.Filter = "Jumplist Pack (*.jlp)|*.jlp|All files (*.*)|*.*";
            fileDialog.FilterIndex = 0;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            fileDialog.Title = "Save";
            fileDialog.AutoUpgradeEnabled = true;
            fileDialog.ShowHelp = true;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                fileDialog.Dispose();
                return;
            }

            string packFileName = fileDialog.FileName;
            fileDialog.Dispose();

            // 2. Show a form that asks users:
            // IsProgramSpecific?
            // UploadToWeb?

            ExportForm exportF = new ExportForm(CurrentAppName);
            DialogResult exportResult = exportF.ShowDialog();

            if (exportResult != System.Windows.Forms.DialogResult.OK) { exportF.Dispose(); return; }

            bool isProgramSpecific = exportF.IsProgramSpecific;
            bool shareWeb = exportF.ShareWeb;
            exportF.Dispose();

            // 3. Create the pack
            Preferences.CreateJumplistPack(packFileName, isProgramSpecific);
            string statusBarText = "Jumplist pack saved";
            if(shareWeb) statusBarText = statusBarText + "! Uploading to web...";
            else statusBarText = statusBarText + " to \"" + packFileName + "\"!";
            ChangeStatusBarText(statusBarText);

            // 4. If upload to web, upload.
            if (shareWeb) { 
                Preferences.UploadJumplistPack(packFileName);
                ChangeStatusBarText("Finished uploading jumplist pack to web!");
            }
        }

        private void TaskAHKHelpButton_Click(object sender, EventArgs e)
        {
            string helpTitle = "AutoHotKey Script Help";
            string helpText = @"Using AutoHotkey, a BASIC-like scripting language, you can automate any action you'd like!

Scripts run by themselves -- the app is not started automatically along with the script. If you want the app to run, it must be scripted to happen. The lines to do this are included by default.
If you do not want the app to start with the script, remove the included ""JLE_XX()"" function lines.

========================

HELP LINKS

Tutorials, Command listing:
http://www.autohotkey.com/docs
Active, friendly help community:
http://www.autohotkey.com/forum

========================

JUMPLIST EXTENDER-SPECIFIC VARIABLES

Use these variables in your scripts to assist in some tasks:

JLE_AppProcessName -- App's process name, e.g. notepad.exe
JLE_AppName -- App's window name, e.g. Notepad
JLE_AppPath -- App's path, e.g. C:\Windows\System32\notepad.exe
JLE_AppWindowClassName -- App's class name, e.g. Microsoft-Windows-SnippingTool-Edit. If applicable, use this to identify program windows.
JLE_AppId -- Windows 7-assigned AppId, e.g. T7E.Notepad. Usually not needed.

========================

JUMPLIST EXTENDER-SPECIFIC FUNCTIONS

Use these functions to handle your program's windows:

========================

JLE_CheckProcessWindowExists
Checks if any instance of process is running. If not, it starts process
Also checks if any process windows exist.
Params: processName -- notepad.exe
        processPath -- C:\Windows\system32\notepad.exe
        processWindowName -- Notepad
        processWindowClass -- microsoft_windows_edit
        startNewProcess -- -1 for ""Don't start at all;"" 0 for ""Start if process doesn't exist;"" 1 for ""Start always""
        waitSeconds -- -1 for ""Default 5 seconds;"" 0 for ""Instantaneous;"" greater than 0 for # of seconds to wait for process
Returns: 1 -- Process and window exists
         0 -- Process and window do not exist

========================

JLE_GetMostRecentWindow
Gets window handle of most recent window for windowName and processName, and checks if it truly belongs to processName
Params: processName -- notepad.exe
        processWindowName -- Notepad
        processWindowClass -- microsoft_windows_edit
Returns: [hWnd] -- Recent window successfully gotten
         0 -- Recent window not successfully gotten

========================

JLE_SendKeystrokeToWindow
Sends an AHK-formatted keystroke to the specified window handle
Params: windowHandle -- Window handle of desired window
        keystroke -- Desired keystroke to send, in AHK text format. e.g. ^o; {space}.
        ** See http://www.autohotkey.com/docs/commands/Send.htm 
Returns: 1 -- Keystroke sent successfully
         0 -- Keystroke failed.";

            HelpDialog helpForm = new HelpDialog(helpTitle, helpText);
            helpForm.Show();
        }

        private void ProgramNewOKButton_Click(object sender, EventArgs e)
        {
            TabControl.SelectedIndex = 1;
            ItemNameTextBox.Focus();
        }

        private void newCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentIndex = JumplistListBox.SelectedIndex;
            T7EJumplistItem jlio = new T7EJumplistItem(this);
            jlio.ItemType = T7EJumplistItem.ItemTypeVar.Category;
            

            /*
            if (JumplistImageList.Images.Count-1 < currentIndex + 1)
             * JumplistImageList.Images.Add((Image)(new Bitmap(16, 16))); // This should always be currentIndex+1
            else
                JumplistImageList.Images[currentIndex + 1] = (Image)(new Bitmap(16, 16));
            */

            JumplistListBox.Items.Insert(currentIndex + 1, jlio);
            JumplistListBox.SelectedIndex = currentIndex + 1;

            CheckJumplistItemPosition();

            // Insert a new task, too
            currentIndex = JumplistListBox.SelectedIndex;
            T7EJumplistItem jlio_item = new T7EJumplistItem(this);
            jlio_item.ItemType = T7EJumplistItem.ItemTypeVar.Task;
            jlio_item.StringToItemIcon("Use program icon");
            JumplistListBox.Items.Insert(currentIndex + 1, jlio_item);
            JumplistListBox.SelectedIndex = currentIndex;

            ActiveControl = ItemNameTextBox;
        }

        private void newSeparatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentIndex = JumplistListBox.SelectedIndex;
            T7EJumplistItem jlio = new T7EJumplistItem(this);
            jlio.ItemType = T7EJumplistItem.ItemTypeVar.Separator;

            /*
            if (JumplistImageList.Images.Count-1 < currentIndex + 1) 
                JumplistImageList.Images.Add((Image)(new Bitmap(16, 16))); // This should always be currentIndex+1
            else
                JumplistImageList.Images[currentIndex + 1] = (Image)(new Bitmap(16, 16));
            */

            JumplistListBox.Items.Insert(currentIndex + 1, jlio);
            JumplistListBox.SelectedIndex = currentIndex + 1;
            CheckJumplistItemPosition();
        }

        private void newFileFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int currentIndex = JumplistListBox.SelectedIndex;
            T7EJumplistItem jlio = new T7EJumplistItem(this);
            jlio.StringToItemIcon("Use program icon"); // Change this to file icon?
            jlio.ItemType = T7EJumplistItem.ItemTypeVar.FileFolder;

            /*
            if (JumplistImageList.Images.Count-1 < currentIndex + 1) 
                JumplistImageList.Images.Add((Image)(new Bitmap(16, 16))); // This should always be currentIndex+1
            else
                JumplistImageList.Images[currentIndex + 1] = (Image)(new Bitmap(16, 16));
            */

            JumplistListBox.Items.Insert(currentIndex + 1, jlio);
            JumplistListBox.SelectedIndex = currentIndex + 1;
            CheckJumplistItemPosition();

            ActiveControl = ItemNameTextBox;
        }

        private void newTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddJumplistTask();
        }

        private void AddJumplistTask()
        {
            int currentIndex = JumplistListBox.SelectedIndex;
            T7EJumplistItem jlio = new T7EJumplistItem(this);
            jlio.StringToItemIcon("Use program icon");
            jlio.ItemType = T7EJumplistItem.ItemTypeVar.Task;

            /*
            if (JumplistImageList.Images.Count-1 < currentIndex + 1) 
                JumplistImageList.Images.Add((Image)(new Bitmap(16, 16))); // This should always be currentIndex+1
            else
                JumplistImageList.Images[currentIndex + 1] = (Image)(new Bitmap(16, 16));
            */

            JumplistListBox.Items.Insert(currentIndex + 1, jlio);
            JumplistListBox.SelectedIndex = currentIndex + 1;
            CheckJumplistItemPosition();

            ActiveControl = ItemNameTextBox;
        }

        private void FolderBrowseButton_Click(object sender, EventArgs e)
        {
            OpenFolder();
        }

        private void OpenFolder()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderDialog.ShowNewFolderButton = true;
            folderDialog.Description = "Select a folder.";
            folderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            DialogResult folderResult = folderDialog.ShowDialog();
            if(folderResult != DialogResult.OK) {
                folderDialog.Dispose();
                return;
            }

            string folderName = folderDialog.SelectedPath;
            CurrentJumplistItem.FilePath = folderName;
            FileTextBox.Text = folderName;
            folderDialog.Dispose();
            return;
        }

        private void TaskCMDBrowseButton_Click(object sender, EventArgs e)
        {
            OpenCmdFile();
        }

        private void OpenCmdFile()
        {
            OpenCmdFile("");
        }
        private void OpenCmdFile(string startPath)
        {
            OpenFileDialog programFileDialog = new OpenFileDialog();
            programFileDialog.Filter = "Program files (*.exe;*.lnk)|*.exe|All files (*.*)|*.*";
            programFileDialog.FilterIndex = 0;
            if (Directory.Exists(startPath))
                programFileDialog.InitialDirectory = startPath;
            else
                programFileDialog.InitialDirectory =
                File.Exists(Path.Combine(Common.Path_AppData, "Programs.library-ms")) ?
                Path.Combine(Common.Path_AppData, "Programs.library-ms")
                : Path.Combine(Common.EnvPath_AllUsersProfile, "Microsoft\\Windows\\Start Menu");
            programFileDialog.Title = "Select program file";
            programFileDialog.AutoUpgradeEnabled = true;
            programFileDialog.ShowHelp = true;
            programFileDialog.DereferenceLinks = false;//false

            DialogResult fileResult = programFileDialog.ShowDialog();

            if (fileResult != DialogResult.OK)
            {
                programFileDialog.Dispose();
                return;
            }

            programFileDialog.Dispose();
            string programFileName = programFileDialog.FileName;

            // Try to resolve it as an mSI shortcut
            //string msiOutput = MsiShortcutParser.ParseShortcut(programFileName);
            //if (msiOutput.Length > 0) programFileName = msiOutput;

            // If programFileName is a shortcut, get its target path
            LnkName = "";
            if (Path.GetExtension(programFileName).ToLower() == ".lnk")
            {
                LnkName = programFileName;
                // Also set CurrentAppName to the lnk name
                //CurrentAppName = Path.GetFileNameWithoutExtension(programFileName);
                programFileName = Common.ResolveLnkPath(programFileName);

                if (Directory.Exists(programFileName))
                {
                    OpenCmdFile(programFileName);
                    return;
                }
            }

            CurrentJumplistItem.StringToItemCmd("\""+programFileName+"\"");
            TaskCMDTextBox.Text = CurrentJumplistItem.ItemCmdToString();
            programFileDialog.Dispose();
        }

        private void TaskCMDHelpButton_Click(object sender, EventArgs e)
        {
            string helpTitle = "Command Line Help";
            string helpText = @"Use the command line to execute programs and parameters.

To pass parameters to the current program, just type the parameters:
/param1 /param2

To pass parameters to another program, quote the program path and type the parameters:
""C:\Path to program\program.exe"" /param1 /param2

You can pass any file to the command line, as well:
""C:\Path to file\file.txt""
";

            HelpDialog helpDialog = new HelpDialog(helpTitle, helpText);
            helpDialog.Show();
        }

        private void ItemNameTextBox_Enter(object sender, EventArgs e)
        {
            switch (ItemNameTextBox.Text)
            {
                case "[Unnamed Task]":
                case "[Unnamed Shortcut]":
                case "[Unnamed Category]":
                    ItemNameTextBox.Text = "";
                    break;
            }
        }

        private void Primary_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(EditCounter <= 0)
                if (!ClearAppLoaded()) e.Cancel = true; // Ask to save, first
        }

        private void MenuFileExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change UI element app names

            if (TabControl.SelectedIndex == 0)
            {
                ProgramSettingsLabel.Text = String.Format(
                    "Program settings for {0}.",
                    CurrentAppName != null && CurrentAppName.Length > 0 ?
                    CurrentAppName
                    : CurrentAppProcessName
                    );
            }
            else if (TabControl.SelectedIndex == 1)
            {
                FileOpenApp.Text = String.Format(
                    "Open the shortcut with {0}",
                    CurrentAppName != null && CurrentAppName.Length > 0 ?
                    CurrentAppName
                    : CurrentAppProcessName
                    );
                ItemNameTextBox.Focus();
            }
        }

        private void FileOpenApp_CheckedChanged(object sender, EventArgs e)
        {
            _CurrentJumplistItem.FileRunWithApp = FileOpenApp.Checked;
        }

        private void Text_SelectAll(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A) ((TextBox)sender).SelectAll();
        }

        private void JumplistListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (CurrentJumplistItem.ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks) return;

                DialogResult deleteResult = MessageBox.Show("Do you want to delete " + CurrentJumplistItem.ItemName + "?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (deleteResult == System.Windows.Forms.DialogResult.Yes)
                    JumplistDeleteButton_Click(sender, e);
            }
        }

        private void TaskKBDSwitchTextButton_Click(object sender, EventArgs e)
        {
            TaskKBDSwitchToTextMode();
        }

        private void TaskKBDSwitchToTextMode()
        {
            TaskKBDShortcutPanel.Enabled = TaskKBDShortcutPanel.Visible = false;
            TaskKBDTextPanel.Enabled = TaskKBDTextPanel.Visible = true;
            TaskKBDSettingsPanel.Location = new Point(TaskKBDSettingsPanel.Location.X, TaskKBDTextPanel.Height + 4);
            TaskKBDSettingsPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            TaskKBDSwitchTextButton.Enabled = TaskKBDSwitchTextButton.Visible = false;
            TaskKBDSwitchShortcutButton.Enabled = TaskKBDSwitchShortcutButton.Visible = true;

            TaskKBDTextBox.Text = CurrentJumplistItem.TaskKBDStringToTextString();

            CurrentJumplistItem.TaskKBDShortcutMode = false;
        }

        private void TaskKBDSwitchShortcutButton_Click(object sender, EventArgs e)
        {
            TaskKBDSwitchToShortcutMode();
        }

        private void TaskKBDSwitchToShortcutMode()
        {
            TaskKBDTextPanel.Enabled = TaskKBDTextPanel.Visible = false;
            TaskKBDShortcutPanel.Enabled = TaskKBDShortcutPanel.Visible = true;
            TaskKBDSettingsPanel.Location = new Point(TaskKBDSettingsPanel.Location.X, TaskKBDShortcutPanel.Height + 4);
            TaskKBDSettingsPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            TaskKBDSwitchTextButton.Enabled = TaskKBDSwitchTextButton.Visible = true;
            TaskKBDSwitchShortcutButton.Enabled = TaskKBDSwitchShortcutButton.Visible = false;

            TaskKBDKeyboardTextBox.Text = CurrentJumplistItem.TaskKBDStringToShortcutString();
            int selectStart = TaskKBDKeyboardTextBox.Text.Length - 1;
            TaskKBDKeyboardTextBox.SelectionStart = selectStart < TaskKBDKeyboardTextBox.Text.Length && selectStart >= 0 ? selectStart : 0;

            CurrentJumplistItem.TaskKBDShortcutMode = true;
        }

        private void TaskKBDKeyboardTextBox_Leave(object sender, EventArgs e)
        {
            CurrentJumplistItem.ShortcutStringToTaskKBDString(TaskKBDKeyboardTextBox.Text);
        }

        private void TaskKBDShortcutClearButton_Click(object sender, EventArgs e)
        {
            TaskKBDKeyboardTextBox.Text = "";
        }

        private void ProgramWindowClassButton_Click(object sender, EventArgs e)
        {
            SelectMainAppWindow(false);
        }

        private void SelectMainAppWindow(bool firstTime)
        {
            // If first time and loading a program that already exists, return
            if (firstTime && CurrentAppWindowClassName != null && CurrentAppWindowClassName.Length > 0) return;
            
            SelectWindow selectForm = new SelectWindow(CurrentAppPath, _CurrentAppName, firstTime);
            WindowState = FormWindowState.Minimized;
            DialogResult selectFormResult = selectForm.ShowDialog();

            if (selectFormResult != System.Windows.Forms.DialogResult.OK)
            {
                selectForm.Dispose();
                WindowState = FormWindowState.Normal;
                return;
            }

            WindowState = FormWindowState.Normal;

            // Before changing everything, check if the app already exists.
            if (File.Exists(SelectWindow.CapturedAppPath) && !CurrentAppPath.Equals(SelectWindow.CapturedAppPath, StringComparison.OrdinalIgnoreCase))
            {
                if (Directory.Exists(Path.Combine(Common.Path_AppData, Common.GetAppIdForProgramFile(SelectWindow.CapturedAppPath))))
                {
                    MessageBox.Show("The program settings for " + Path.GetFileNameWithoutExtension(SelectWindow.CapturedAppName) + " already exist.", "Program settings already exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // If not, ask for a dialog.
                if (Directory.Exists(Path.Combine(Common.Path_AppData, CurrentAppId)))
                {
                    if (MessageBox.Show("The settings for " + (CurrentAppName.Length > 0 ? CurrentAppName : CurrentAppProcessName)
                        + " must be saved before changing the program path. Do you want to save the settings now?"
                        , "Save"
                        , MessageBoxButtons.YesNo
                        , MessageBoxIcon.Information
                        , MessageBoxDefaultButton.Button1) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return;
                    } // else
                }
            }

            

            string newAppName = SelectWindow.CapturedAppName.Length > 0 ? SelectWindow.CapturedAppName : CurrentAppName;
            // Most app titles go like {Filename} - {AppTitle}
            // If the whole of CurrentAppName indexes in newAppName[1], make no change.
            // Else, change CurrentAppName to newAppName[1]
            string[] newAppNameSplitNeedle = new string[1];
            newAppNameSplitNeedle[0] = " - ";
            string[] newAppNameSplit = newAppName.Split(newAppNameSplitNeedle, StringSplitOptions.RemoveEmptyEntries);
            if (newAppNameSplit.Length >= 1)
            {
                if (newAppNameSplit.Last().IndexOf(CurrentAppName) < 0)
                    ChangeAppName(newAppNameSplit.Last());
            }

            // Change app class
            if (SelectWindow.CapturedAppClass.Length > 0)
                ChangeAppClass(SelectWindow.CapturedAppClass);

            // If app path is different, change it
            Programmatic = true;
            if (File.Exists(SelectWindow.CapturedAppPath) && !CurrentAppPath.Equals(SelectWindow.CapturedAppPath, StringComparison.OrdinalIgnoreCase))
                ChangeAppPath(SelectWindow.CapturedAppPath);
            Programmatic = false;

            selectForm.Dispose();
        }

        private void ChangeAppClass(string newAppClass)
        {
            string oldAppClass = CurrentAppWindowClassName;

            if (oldAppClass == newAppClass) return;

            // Replace AHK appname
            foreach (T7EJumplistItem jlio in JumplistListBox.Items)
            {
                jlio.TaskAHKScript = jlio.TaskAHKScript.Replace(
                    "JLE_AppWindowClassName := \"" + oldAppClass + "\"",
                    "JLE_AppWindowClassName := \"" + newAppClass + "\""
                    );

                if (CurrentJumplistItem.Equals(jlio)) // Update UI elements
                    TaskAHKTextBox.Text = jlio.TaskAHKScript;
            }

            CurrentAppWindowClassName = newAppClass;
            ProgramNameLabel.Text = "Program &Title:";
        }

        private void keyboardShortcutHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpDialog hd = new HelpDialog("Keyboard Shortcut Help", @"KEYBOARD SHORTCUTS make Jumplist Extender MUCH faster and easier to use! The shortcuts are simple:

* ""Ctrl"" shortcuts CHANGE user settings
* ""Alt"" shortcuts NAVIGATE the program fields

========================

JUMPLIST

Ctrl+Up -- MOVE UP the current jumplist item
Ctrl+Down -- MOVE DOWN the current jumplist item
Ctrl+Add -- ADD a jumplist item
Ctrl+Subtract -- DELETE a jumplist item
Delete -- Ditto.

NAVIGATION

Alt+Up -- Select the previous/upper jumplist item
Alt+Down -- Select the next/lower jumplist item
Alt+Left -- Focus on the jumplist box
Alt+Right -- Focus on the editing area

(The below shortcuts are underlined in the program.)
Alt+N -- Name
Alt+O -- Icon
Alt+T -- Item Type
Alt+C -- Task Action Type
Alt+D -- Task Action Properties
Alt+L -- File/Folder Shortcut Location

========================

FILE

Ctrl+N -- Start a new jumplist
Ctrl+O -- Open an existing jumplist
Ctrl+S -- Save the jumplist to disk
Ctrl+Alt+S -- Save the jumplist and APPLY TO TASKBAR

Ctrl+I -- Import a Jumplist Pack
Ctrl+P -- Export a Jumplist Pack");
            hd.Show();
        }

        private void importProgramSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportPack();
        }

        private void TaskKBDIgnoreCurrentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TaskKBDIgnoreCurrentCheckBox.Checked &&
                TaskKBDIgnoreAbsentCheckBox.Checked)
                TaskKBDIgnoreAbsentCheckBox.Checked =
                    _CurrentJumplistItem.TaskKBDIgnoreAbsent = false;

            _CurrentJumplistItem.TaskKBDIgnoreCurrent = TaskKBDIgnoreCurrentCheckBox.Checked;
        }

        private void TaskKBDIgnoreAbsentCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (TaskKBDIgnoreAbsentCheckBox.Checked &&
                TaskKBDIgnoreCurrentCheckBox.Checked)
                TaskKBDIgnoreCurrentCheckBox.Checked =
                    _CurrentJumplistItem.TaskKBDIgnoreCurrent = false;

            _CurrentJumplistItem.TaskKBDIgnoreAbsent = TaskKBDIgnoreAbsentCheckBox.Checked;
        }

        private void donateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "http://jumplist.gsdn-media.com/wiki/Site:Donate");
            this.WindowState = FormWindowState.Minimized;
            //ShowDonateDialog(true);
        }

        private void TaskKBDSwitchAHKButton_Click(object sender, EventArgs e)
        {
            TaskActionComboBox.SelectedIndex = 2;
            ActiveControl = TaskAHKTextBox;
            TaskAHKTextBox.SelectionLength = 0;
        }

        private string UpdatePath = "";

        private void CheckUpdateString()
        {
            // We aren't going to download UpdateCheck2.txt here, 'cuz StartForm already did.
            // Just check the stored UpdateCheck2.txt.

            if (!File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))) return;

            try
            {
                string[] versionCheckParts = File.ReadAllText(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")).Split('|');
                if (versionCheckParts == null || versionCheckParts.Length < 2) return;

                if (Assembly.GetExecutingAssembly().GetName().Version.ToString().Equals(versionCheckParts[0])) return;
                else
                {
                    // If UpdateCheck has 0.x.x.x|http://link|0.x-B, get the third slice
                    // else, substr from the first version string.
                    updateToVersion0ToolStripMenuItem.Text = String.Format(updateToVersion0ToolStripMenuItem.Text,
                        versionCheckParts.Length >= 3 ? versionCheckParts[2]
                        : versionCheckParts[0].Substring(0, 3));
                    updateToVersion0ToolStripMenuItem.Enabled = updateToVersion0ToolStripMenuItem.Visible = true;
                    updateToVersion0ToolStripMenuItem.Enabled = updateToVersion0ToolStripMenuItem.Visible
                        = true;
                    UpdatePath = versionCheckParts[1];
                }
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ /* Fail silently */ }
        }

        private void updateToVersion0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", UpdatePath);
        }

        private void visitTheOfficialWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "http://jumplist.gsdn-media.com");
        }
    }
}
