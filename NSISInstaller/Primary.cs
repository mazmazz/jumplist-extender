using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;
using System.Xml;
using T7ECommon;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Runtime.InteropServices;

using Microsoft.Win32;

namespace NSISInstaller
{
    public partial class Primary : Form
    {
        public Primary()
        {
            InitializeComponent();
        }

        public void SetAppIdBackAfterJumplist()
        {
            JumpList ownList = JumpList.CreateJumpListForAppId("T7E.Meta");
            ownList.AddUserTasks(new JumpListLink
            {
                Title = "Visit the official website",
                Path = "%windir%\\explorer.exe",
                Arguments = "\""+Common.WebPath_OfficialSite+"\"",
                IconReference = new IconReference(Path.Combine(Common.EnvPath_SystemRoot, "system32\\shell32.dll"), 135)
            });
            ownList.Refresh();
            Common.TaskbarManagerInstance.SetCurrentProcessAppId("T7E.Meta");
        }

        private void EraseJumplist(string appId)
        {
            try
            {
                JumpList dummyList = JumpList.CreateJumpListForAppId(appId);
                dummyList.Refresh();

                // Set appid back
                SetAppIdBackAfterJumplist();
            }
            catch (Exception e) { Console.Write("Erase Jumplist Error: " + e.Message); }
        }

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
                        if (!File.Exists(appIdPath) || new FileInfo(appIdPath).Length <= 0)
                        {
                            continue;
                        }

                        Common.AppIds.Add(reader.GetAttribute("id"));
                        Common.AppProcessNames.Add(reader.GetAttribute("processName"));
                        Common.AppCount++;

                        Common.Log("App added to list! | "
                            + Common.AppCount + " | "
                            + Common.AppIds.Last() + " | "
                            + Common.AppProcessNames.Last() + " | "
                        );
                    }
                }
            }
            ///////////
        }

        private void InstallInitialJLE()
        {
            // We already killed the processes in Main(). Exit
            Environment.Exit(0);
        }

        private void WinForm_Shown(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            //MessageBox.Show(Common.EnvPath_UserProfile);
            if (args.Length > 1)
            {
                if (args[1].Equals("/uninst:{f3ea12fa-c9a5-4c3d-990a-ba01be930e3e}")) UninstallJLE();
                else if (args[1].Equals("/inst:{90fd0530-b663-4fe7-9291-189031b47993}")) InstallJLE();
                else if (args[1].Equals("/instInitial:{8692ad85-0c47-4bf5-9f56-cbf25f1ac82d}")) InstallInitialJLE();
                else MessageBox.Show("JLE Installer Script: Only for the installer.");
            }
            else MessageBox.Show("JLE Installer Script: Only for the installer.");

            Environment.Exit(-1);
        }

        public string CurrentAppPath = "";
        public string CurrentAppId = "";
        public string CurrentAppName = "";
        public string CurrentAppProcessName = "";
        public string CurrentAppWindowClassName = "";

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target, params string[] ignoreFilenames)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into its new directory.
            bool goAhead = true;
            foreach (FileInfo fi in source.GetFiles())
            {
                goAhead = true;
                foreach (string ignoreFilename in ignoreFilenames) { 
                    if(fi.Name.Equals(ignoreFilename, StringComparison.OrdinalIgnoreCase)
                        && File.Exists(Path.Combine(target.FullName, ignoreFilename)))
                    {
                        goAhead = false;
                        break;
                    }
                }
                if(goAhead) fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true); 
            }

            // Copy each subdirectory using recursion.
            goAhead = true;
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                goAhead = true;
                foreach (string ignoreFilename in ignoreFilenames) { 
                    if(diSourceSubDir.Name.Equals(ignoreFilename, StringComparison.OrdinalIgnoreCase)
                        && Directory.Exists(Path.Combine(target.FullName, ignoreFilename)))
                    {
                        goAhead = false;
                        break;
                    }
                }
                if (goAhead)
                {
                    DirectoryInfo nextTargetSubDir =
                        target.CreateSubdirectory(diSourceSubDir.Name);
                    CopyAll(diSourceSubDir, nextTargetSubDir);
                }
            }
        }

        private void InstallJLE()
        {
            // Delete outdated files
            if (File.Exists(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Windows\\Start Menu\\Programs\\Startup\\Jumplist Extender.lnk")))
                File.Delete(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Windows\\Start Menu\\Programs\\Startup\\Jumplist Extender.lnk"));

            if (File.Exists(Path.Combine(Common.Path_AppData, "Icons\\[00] shell32.dll")))
                File.Delete(Path.Combine(Common.Path_AppData, "Icons\\[00] shell32.dll"));

            if (File.Exists(Path.Combine(Common.Path_ProgramFiles, "PinShortcut.vbs")))
                File.Delete(Path.Combine(Common.Path_ProgramFiles, "PinShortcut.vbs"));

            if (File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")))
                File.Delete(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"));

            if (File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck.txt")))
                File.Delete(Path.Combine(Common.Path_AppData, "UpdateCheck.txt"));

            // Copy files to AppData, if needed
            string appDataDir = Common.Path_AppData;
            string programFilesDir = Common.Path_ProgramFiles;
            //MessageBox.Show(appDataDir);
            if (!Directory.Exists(appDataDir))
            {
                try { Directory.CreateDirectory(appDataDir); }
                catch (Exception e) { Common.Fail("Data directory could not be created.", 1); }
                // Copy each file in programFiles\Defaults into appDataDir
            }

                // Copy each file into it’s new directory.
                DirectoryInfo source = new DirectoryInfo(Path.Combine(programFilesDir, "Defaults"));
                DirectoryInfo target = new DirectoryInfo(appDataDir);
                CopyAll(source, target, "AppList.xml", "Preferences.xml", "OrigProperties", "Icons");
            

            ReadAppList();

            if (Common.AppIds != null && Common.AppIds.Count > 0)
            {
                foreach (string appId in Common.AppIds)
                {
                    ReadAppSettings(appId); // Also reads jumplist into JumplistListBox
                    ApplyJumplistToTaskbar();

                    CurrentAppPath = "";
                    CurrentAppId = "";
                    CurrentAppName = "";
                    CurrentAppProcessName = "";
                    CurrentAppWindowClassName = "";

                    JumplistListBox.Items.Clear();
                }
            }

            // Shortcuts: Likely, we're gonna run T7EBackground anyways, so
            // let T7EBackground handle it.

            if (!Common.PrefExists("InstallDate"))
            {
                if (Common.AppCount > 0) Common.WritePref("InstallDate", DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString()
                     , "InstallUpgrade", false.ToString());
            }
            else
            {
                Common.WritePref("InstallUpgrade", true.ToString()
                    , "DonateDialogDisable", false.ToString());
            }

            // Write library
            if (!File.Exists(Path.Combine(Common.Path_AppData, "Programs.library-ms")))
            {
                ShellLibrary programsLibrary = new ShellLibrary("Programs", Common.Path_AppData, true);

                if (Directory.Exists(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar")))
                    programsLibrary.Add(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar"));

                if (Directory.Exists(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\StartMenu")))
                    programsLibrary.Add(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned\\StartMenu"));

                if (Directory.Exists(Path.Combine(Common.EnvPath_AllUsersProfile, "Microsoft\\Windows\\Start Menu\\Programs")))
                    programsLibrary.Add(Path.Combine(Common.EnvPath_AllUsersProfile, "Microsoft\\Windows\\Start Menu\\Programs"));

                if (Directory.Exists(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Windows\\Start Menu\\Programs")))
                    programsLibrary.Add(Path.Combine(Common.EnvPath_AppData, "Microsoft\\Windows\\Start Menu\\Programs"));

                programsLibrary.Dispose(); // Library is written
            }

            // Add to startup
            /*RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue("JumplistExtender") == null)
                rkApp.SetValue("JumplistExtender", Path.Combine(Common.Path_ProgramFiles, "T7EBackground.exe"));
            rkApp.Close();*/
            
        }

        public bool ApplyJumplistToTaskbar()
        {
            bool result = false;

            try
            {
                JumpList newList = JumpList.CreateJumpListForAppId(CurrentAppId);

                ListBox.ObjectCollection jumplistItems = JumplistListBox.Items;
                for (int i = 0; i < jumplistItems.Count; i++)
                {
                    // Look for a category
                    JumpListItemObject.ItemTypeVar jumplistItemType = ((JumpListItemObject)jumplistItems[i]).ItemType;
                    if (jumplistItemType == JumpListItemObject.ItemTypeVar.Category
                        || jumplistItemType == JumpListItemObject.ItemTypeVar.CategoryTasks)
                    {
                        JumpListCustomCategory category = new JumpListCustomCategory(((JumpListItemObject)jumplistItems[i]).ItemName);

                        // Look for a task
                        for (int j = i + 1; j < jumplistItems.Count; j++)
                        {
                            i = j - 1; // When J loop is exited, i has to be less.
                            JumpListItemObject jumplistItem = (JumpListItemObject)jumplistItems[j];
                            switch (jumplistItem.ItemType)
                            {
                                case JumpListItemObject.ItemTypeVar.Category:
                                case JumpListItemObject.ItemTypeVar.CategoryTasks:
                                    j = jumplistItems.Count; // Exit the for loop
                                    break;

                                case JumpListItemObject.ItemTypeVar.Task:
                                    if (jumplistItemType == JumpListItemObject.ItemTypeVar.Category)
                                        category.AddJumpListItems(
                                            ParseApplyJumpListTask(jumplistItem, j));
                                    else
                                        newList.AddUserTasks(
                                            ParseApplyJumpListTask(jumplistItem, j));
                                    break;

                                case JumpListItemObject.ItemTypeVar.FileFolder:
                                    JumpListLink link = new JumpListLink
                                    {
                                        Title = jumplistItem.ItemName,
                                        Path = "C:\\Windows\\explorer.exe",
                                        Arguments = jumplistItem.FilePath
                                    };
                                    if (jumplistItem.FileRunWithApp) link.Path = CurrentAppPath;
                                    // Format icon
                                    if (jumplistItem.ItemIconToString().Equals("Don't use an icon") != true)
                                    {
                                        // Icon processing
                                        string localIconPath = IconPathToLocal(jumplistItem.ItemIconPath, jumplistItem.ItemIconIndex, j, CurrentAppId, false);
                                        if (File.Exists(localIconPath))
                                            link.IconReference = new IconReference(localIconPath, 0);
                                    }

                                    if (jumplistItemType == JumpListItemObject.ItemTypeVar.Category)
                                        category.AddJumpListItems(link);
                                    else
                                        newList.AddUserTasks(link);
                                    break;

                                case JumpListItemObject.ItemTypeVar.Separator:
                                    if (jumplistItemType == JumpListItemObject.ItemTypeVar.CategoryTasks)
                                        newList.AddUserTasks(new JumpListSeparator());
                                    break;
                            }
                        }

                        if (jumplistItemType == JumpListItemObject.ItemTypeVar.Category)
                            newList.AddCustomCategories(category);
                    }
                }

                // ////////
                JumpList dummyList = JumpList.CreateJumpListForAppId(CurrentAppId);
                dummyList.AddUserTasks(new JumpListLink
                {
                    Title = "Dummy",
                    Path = "%windir%\\explorer.exe"
                });
                dummyList.Refresh();
                newList.Refresh();
                // Remove appid from own window!
                SetAppIdBackAfterJumplist();
                result = true;
            }
            catch (Exception e)
            {
                MessageBox.Show("JumpList applying not successful." + "\r\n"
                    + e.ToString());
            }

            return result;
        }

        private JumpListLink ParseApplyJumpListTask(JumpListItemObject jumplistItem, int itemIndex)
        {
            JumpListLink task = new JumpListLink
            {
                Title = jumplistItem.ItemName
            };

            if (jumplistItem.ItemIconToString().Equals("Don't use an icon") != true)
            {
                // Icon processing
                string localIconPath = IconPathToLocal(jumplistItem.ItemIconPath, jumplistItem.ItemIconIndex, itemIndex, CurrentAppId, false);
                if (File.Exists(localIconPath))
                    task.IconReference = new IconReference(localIconPath, 0);
            }

            switch (jumplistItem.TaskAction)
            {
                case JumpListItemObject.ActionType.Keyboard:
                    string kbdScriptName = GetKeyboardScriptFilename(jumplistItem, itemIndex);
                    if (File.Exists(kbdScriptName))
                    {// It should already have been made
                        task.Path = Common.Path_ProgramFiles + "\\AutoHotKey.exe";
                        task.Arguments = "\"" + kbdScriptName + "\"";
                    }
                    else return null;
                    break;

                case JumpListItemObject.ActionType.CommandLine:
                    if (jumplistItem.TaskCMDShowWindow)
                    {
                        task.Path = "cmd.exe";
                        task.Arguments = "/k \"" + jumplistItem.ItemCmdToString().Replace("\"", "\"\"") + "\"";
                    }
                    else
                    {
                        task.Path = jumplistItem.TaskCMDPath;
                        task.Arguments = jumplistItem.TaskCMDArgs;
                    }
                    break;

                case JumpListItemObject.ActionType.AutoHotKey:
                    string ahkFilename = GetAhkScriptFilename(false, jumplistItem, itemIndex);
                    if (File.Exists(ahkFilename)) // It should have already been made.
                    {
                        task.Path = Common.Path_ProgramFiles + "\\AutoHotKey.exe";
                        task.Arguments = "\"" + ahkFilename + "\"";
                    }
                    else return null;
                    break;
            }
            return task;
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
            return true;
        }

        public void ReadJumpList(string jumplistPath)
        {
            Common.Log("Reading jumplist at: " + jumplistPath, 1);
            if (!File.Exists(jumplistPath)) return;
            JumpListItemObject jumplistItem = new JumpListItemObject(this);

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
                if (((JumpListItemObject)JumplistListBox.Items[i]).ItemType == JumpListItemObject.ItemTypeVar.CategoryTasks)
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
                        jumplistItem = new JumpListItemObject(this);
                        jumplistItem.ItemName = reader["name"];
                        jumplistItem.ItemType = JumpListItemObject.ItemTypeVar.Category;
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
                                jumplistItem = new JumpListItemObject(this);
                                jumplistItem.ItemType = JumpListItemObject.ItemTypeVar.Separator;
                                JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                                currentItemPosition++;
                            }
                        }
                        else
                        {
                            jumplistItem = new JumpListItemObject(this);
                            jumplistItem.ItemName = "Tasks";
                            jumplistItem.ItemType = JumpListItemObject.ItemTypeVar.CategoryTasks;
                            JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                            currentItemPosition++;
                        }
                    }

                    else if (reader.IsStartElement("task"))
                    {
                        jumplistItem = new JumpListItemObject(this);
                        jumplistItem.ItemType = JumpListItemObject.ItemTypeVar.Task;
                        jumplistItem.ItemName = reader["name"];
                        // Now read for <icon> and <action>
                        while (reader.Read())
                        {
                            if (reader.IsStartElement("icon"))
                            {
                                // Consider translating this to norm. values
                                reader.Read();
                                string iconString = reader.Value;
                                jumplistItem.StringToItemIcon(iconString);
                            }

                            else if (reader.IsStartElement("action"))
                            {
                                switch (reader["type"])
                                {
                                    case "T7E_TYPE_KBD":
                                        jumplistItem.TaskAction = JumpListItemObject.ActionType.Keyboard;
                                        bool.TryParse(reader["delay"], out jumplistItem.TaskKBDDelay);
                                        if (jumplistItem.TaskKBDDelay)
                                            int.TryParse(reader["delayLength"], out jumplistItem.TaskKBDDelayLength);
                                        bool.TryParse(reader["newWindow"], out jumplistItem.TaskKBDNew);
                                        bool.TryParse(reader["isShortcut"], out jumplistItem.TaskKBDShortcutMode);
                                        reader.Read(); // Read to text node
                                        jumplistItem.TaskKBDString = reader.Value;
                                        break;

                                    case "T7E_TYPE_CMD":
                                        jumplistItem.TaskAction = JumpListItemObject.ActionType.CommandLine;
                                        bool.TryParse(reader["showWindow"], out jumplistItem.TaskCMDShowWindow);
                                        reader.Read(); // Read to text node
                                        string cmdString = "";
                                        cmdString = Common.ReplaceEnvVarToExpandedPath(reader.Value);

                                        jumplistItem.StringToItemCmd(cmdString);
                                        break;

                                    case "T7E_TYPE_AHK":
                                        jumplistItem.TaskAction = JumpListItemObject.ActionType.AutoHotKey;
                                        reader.Read(); // Read to text node
                                        string ahkPath;

                                        // If PackLoading, just load the AHK from the temp path
                                            ahkPath = Path.Combine(Common.Path_AppData, CurrentAppId + "\\" + reader.Value);

                                        if (!File.Exists(ahkPath)) { MessageBox.Show(ahkPath); break; }
                                        using (StreamReader ahkReader = new StreamReader(ahkPath))
                                        {
                                            jumplistItem.TaskAHKScript = ahkReader.ReadToEnd();
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
                        jumplistItem = new JumpListItemObject(this);
                        jumplistItem.ItemName = reader["name"];
                        bool.TryParse(reader["runWithApp"], out jumplistItem.FileRunWithApp);

                        // Now read for <icon> and <action>
                        while (reader.Read())
                        {
                            if (reader.IsStartElement("icon"))
                            {
                                reader.Read();
                                string iconString = reader.Value;
                                jumplistItem.StringToItemIcon(iconString);
                            }

                            else if (reader.IsStartElement("location"))
                            {
                                reader.Read(); // Get text node
                                string locationPath = "";
                                locationPath = Common.ReplaceEnvVarToExpandedPath(reader.Value);

                                if (!File.Exists(locationPath) && !Directory.Exists(locationPath)) break;
                                else jumplistItem.FilePath = locationPath;

                                // Check if directory
                                //FileAttributes attr = File.GetAttributes(locationPath);
                                //if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                                jumplistItem.ItemType = JumpListItemObject.ItemTypeVar.FileFolder;
                            }

                            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "link")
                                break;
                        }

                        JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                        currentItemPosition++;
                    }

                    else if (reader.IsStartElement("separator"))
                    {
                        jumplistItem = new JumpListItemObject(this);
                        jumplistItem.ItemType = JumpListItemObject.ItemTypeVar.Separator;
                        JumplistListBox.Items.Insert(currentItemPosition, jumplistItem);
                        currentItemPosition++;
                    }

                    else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "jumpList")
                        break;
                }
            }

            catch (Exception e)
            {
                Common.Log("ReadJumpList error!\r\n"
                    + e.ToString() + "\r\nJumpList was not read.");
            }

            reader.Close();

            Common.Log("Finished reading jumplist.", -1);
        }

        private string GetAhkScriptFilename(bool isPack, JumpListItemObject jumplistItem, int itemIndex)
        {
            // ahk_1.ahk
            
                return Path.Combine(Common.Path_AppData, CurrentAppId + "\\ahk_" + itemIndex + ".ahk");
        }
        static private string IconPathToLocal(string iconPath, int iconIndex, string appId)
        {
            return IconPathToLocal(iconPath, iconIndex, 0, appId, false);
        }
        private string GetKeyboardScriptFilename(JumpListItemObject jumplistItem, int itemIndex)
        {
            // keyboard_1.ahk
            return Path.Combine(Common.Path_AppData, CurrentAppId + "\\keyboard_" + itemIndex + ".ahk");
        }
        static private string IconPathToLocal(string iconPath, int iconIndex, int itemIndex, string appId, bool isPack)
        {
            
                return Path.Combine(Common.Path_AppData, appId + "\\"
                 + Path.GetFileNameWithoutExtension(iconPath)
                 + "_" + iconIndex
                 + "_" + itemIndex
                 + ".ico");
        }

        Dictionary<string, string> DeletedList;

        private void UninstallJLE()
        {
            // Remove from startup
            // Good luck: You can't access the current user's start menu from here!
            /*RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rkApp.GetValue("JumplistExtender") != null)
                rkApp.DeleteValue("JumplistExtender");
            rkApp.Close();*/

            ReadAppList();

            if (Common.AppIds != null && Common.AppIds.Count > 0)
            {
                foreach (string appId in Common.AppIds)
                {
                    EraseJumplist(appId);
                }

                // Reset the shortcuts, while you're at it
                // First, populate shortcutList with all shortcuts we want to check
                List<string> shortcutList = new List<string>();
                // Check for all shortcuts under %appdata%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar and StartMenu
                string userPinnedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned");
                DirectoryInfo di = new DirectoryInfo(userPinnedPath);
                FileInfo[] lnkInfo = di.GetFiles("*.lnk", SearchOption.AllDirectories);
                foreach (FileInfo lnk in lnkInfo)
                    shortcutList.Add(lnk.FullName);

                // For each shortcut link we want to check...
                foreach (string lnkPath in shortcutList)
                {
                    Common.Log("Checking shortcut " + lnkPath, 1);

                    string targetName = Path.GetFileNameWithoutExtension(Common.ResolveLnkPath(lnkPath));

                    Common.Log("Shortcut's target name: " + targetName);

                    // And check shortcut link object against every appId we want to
                    DeletedList = new Dictionary<string, string>();
                    for (int i = 0; i < Common.AppCount; i++)
                    {
                        DeletedList.Add(Common.AppProcessNames[i], Common.AppIds[i]);
                    }
                    foreach (KeyValuePair<string, string> deletedAppPair in DeletedList)
                    {
                        string appProcessName = deletedAppPair.Key;
                        string appId = deletedAppPair.Value;

                        Common.Log("Checking shortcut against " + Common.GetLogAppString(appProcessName, appId), 1);

                        if (targetName.Equals(appProcessName, StringComparison.OrdinalIgnoreCase)) // We found match!
                        {
                            Common.Log("Shortcut matched!");

                            string lnkFilename = Path.GetFileName(lnkPath);
                            string origLnkPath = Path.Combine(Common.Path_AppData, "OrigProperties\\" + lnkFilename);

                            // Replace the shortcut with the original, if exists
                            if (!System.IO.File.Exists(origLnkPath))
                            {
                                Common.Log("Original shortcut backup does not exist, creating blank AppId shortcut");
                                // If it doesn't exist, just remove T7E from lnkPath
                                string tmpLnkPath = Path.Combine(Path.GetTempPath(), lnkFilename);
                                File.Copy(lnkPath, tmpLnkPath, true);
                                Common.TaskbarManagerInstance.SetApplicationIdForShortcut(tmpLnkPath, "");
                                origLnkPath = tmpLnkPath;
                            }

                            if (System.IO.File.Exists(origLnkPath))
                            {
                                Common.Log("Pinning/Unpinning original shortcut");

                                // Pin original shortcut
                                UnpinShortcut(lnkPath);
                                if (lnkPath.LastIndexOf("StartMenu") > 0)
                                    PinShortcut(origLnkPath, true);
                                else
                                    PinShortcut(origLnkPath, false);
                                //System.IO.File.Copy(origLnkPath, lnkPath, true);
                                //System.IO.File.Delete(origLnkPath);
                            }

                            Common.Log(-1);
                            break; // Since a shortcut belongs to only one appId
                        }

                        Common.Log(-1);
                    }

                    Common.Log("Finished checking shortcut.", -1);

                }

                Common.Log("Assigning AppIds to all windows.", 1);
                EnumWindows(AssignAppIdsToWindow_EnumWindowsCallback, IntPtr.Zero);
                Common.Log("Done assigning AppIds to windows.", -1);
            }

            // Delete the appdata folder, if asked
            DialogResult deleteAppdata = MessageBox.Show("Do you want to delete your jumplist settings permanently?",
                "Delete Jumplist Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            try
            {
                if (deleteAppdata == System.Windows.Forms.DialogResult.Yes)
                    Directory.Delete(Common.Path_AppData, true);
            }
            catch (Exception e) { }
        }
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string Message);
        static uint WM_ShellHook = 0;

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32")]
        public static extern uint
            GetWindowThreadProcessId(IntPtr hwnd, out int lpdwProcessId);
        public bool AssignAppIdsToWindow_EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            // Is hWnd an actual window? Otherwise, you're getting 400 results
            if (!IsWindowVisible(hWnd)) return true;
            else if (GetWindowTextLength(hWnd) == 0) return true;

            AssignAppIdsToWindow(hWnd);
            return true;
        }
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
        int PROCESS_QUERY_INFORMATION = 0x0400;
        int PROCESS_VM_READ = 0x0010;
        [DllImport("psapi.dll")]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In] [MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
        public void AssignAppIdsToWindow(IntPtr hWnd)
        {
            //return; // 3/22: Until shortcuts are sorted out
            // Shortcuts are subject to follow errors:
            // 1. On T7EBackground startup: AppId is applied, but jumplist doesn't show
            // Shortcuts are definitely written; the button just isn't updated
            // Does jumplist cache have to be invalidated?
            // 2. On pinning to taskbar sometimes; pinning to start menu all the time:
            // Shortcut is not readable. Is this a race condition with the dir handler?

            // Get process ID from window
            int hwndPid;
            GetWindowThreadProcessId(hWnd, out hwndPid);

            // Get process handle from pID; get process path from handle
            IntPtr hwndPidHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, false, hwndPid);
            StringBuilder hwndProcessNameString = new StringBuilder(260);
            GetModuleFileNameEx(hwndPidHandle, IntPtr.Zero, hwndProcessNameString, 260);
            CloseHandle(hwndPidHandle);

            // Get process name from process path
            string hwndProcessName;
            try { hwndProcessName = Path.GetFileNameWithoutExtension(hwndProcessNameString.ToString()).ToLower(); }
            catch (Exception e) { return; }

            Common.Log("Assigning AppIds to window " + hwndProcessName, 1);

            //////////////////
            // Check for newly deleted appIds
            // Populate deletedList with every appId we want to check
            // NOTE: If we're deleting, the appid will be deleted, BUT the taskbar
            // will still show the jumplist. Find way to refresh taskbar...
            // I could remove the taskbar button and put it back, by setting the
            // window's owner to a hidden window...
            // But that wouldn't solve the shortcut issue. I NEED to refresh!
                Common.Log("Checking window for " + DeletedList.Count.ToString() + " deleted AppIds.", 1);

                foreach (KeyValuePair<string, string> deletedAppPair in DeletedList)
                {
                    string appProcessName = deletedAppPair.Key;
                    string appId = deletedAppPair.Value;

                    Common.Log("Checking window against " + Common.GetLogAppString(appProcessName, appId), 1);
                    if (hwndProcessName.Equals(appProcessName, StringComparison.OrdinalIgnoreCase)) // We found match!
                    {
                        Common.Log("Window matched!");

                        string origAppIdPath = Path.Combine(Common.Path_AppData, "OrigProperties\\" + appId + ".txt");
                        string origAppId;

                        // Get original appid, if it exists
                        if (System.IO.File.Exists(origAppIdPath))
                        {
                            StreamReader origAppIdStream = new System.IO.StreamReader(origAppIdPath);
                            origAppId = origAppIdStream.ReadToEnd();
                            origAppIdStream.Close();

                            System.IO.File.Delete(origAppIdPath);

                            Common.Log("Original AppId exists: " + origAppIdPath);
                        }
                        else
                        {
                            // If it doesn't exist, just remove appid
                            origAppId = "";
                            Common.Log("Original AppId does not exist; making blank.");
                        }

                        Common.Log("Setting original AppId to window.");
                        Common.TaskbarManagerInstance.SetApplicationIdForSpecificWindow(hWnd, origAppId);
                        ShowWindow(hWnd, 0);
                        //Thread.Sleep(250);
                        ShowWindow(hWnd, 5);

                        Common.Log("Finished checking AppId.", -1);

                        break; // Since a window belongs to only one appId
                    }
                    else
                    {
                        Common.Log(-1);
                    }
                }

                Common.Log("Finished checking for deleted AppIds.", -1);
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        public string GetStringResource(IntPtr hModuleInstance, uint uiStringID)
        {
            StringBuilder sb = new StringBuilder(255);
            Interop.LoadString(hModuleInstance, uiStringID, sb, sb.Capacity + 1);
            return sb.ToString();
        }

        private void UnpinShortcut(string lnkPath)
        {
            // Unpinning shortcuts is under UnpinShortcut()
            IntPtr shell32Module = Interop.GetModuleHandle("shell32.dll");

            string command;
            if (lnkPath.Contains("StartMenu"))
                command = GetStringResource(shell32Module, 5382); // Unpin from Start Men&u
            else
                command = GetStringResource(shell32Module, 5387); // Unpin from Tas&kbar

            int iRetVal;
            iRetVal = (int)Interop.ShellExecute(
                this.Handle,
                command,
                lnkPath,
                "",
                "",
                Interop.ShowCommands.SW_HIDE);
        }

        private void PinShortcut(string lnkPath, bool pinStartMenu)
        {
            // Unpinning shortcuts is under UnpinShortcut()
            IntPtr shell32Module = Interop.GetModuleHandle("shell32.dll");
            
            string command;
            if (pinStartMenu)
                command = GetStringResource(shell32Module, 5381); // Pin to Start Men&u
            else
                command = GetStringResource(shell32Module, 5386); // Pin to Tas&kbar

            int iRetVal;
            iRetVal = (int)Interop.ShellExecute(
                this.Handle,
                command,
                lnkPath,
                "",
                "",
                Interop.ShowCommands.SW_HIDE);
        }
    }
}
