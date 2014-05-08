using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;

using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Drawing;

using System.Drawing.IconLib;
using System.Drawing.IconLib.ColorProcessing;

using System.Windows.Forms;

using T7ECommon;

namespace T7EPreferences
{
    static partial class Preferences
    {
        static public Primary PrimaryParent;
        static private string AppSettingsPath;

        #region XML Writing
        static public void WriteAppSettingsXml()
        {
            if (PrimaryParent.ActiveControl != null) PrimaryParent.ActiveControl = null;

            string appSettingsPath = Path.Combine(Common.Path_AppData, PrimaryParent.CurrentAppId);
            if (!Directory.Exists(appSettingsPath))
                Directory.CreateDirectory(appSettingsPath);
            AppSettingsPath = appSettingsPath;
            string xmlSource = _CreateAppSettingsXml(false);

            //
            string xmlDestName = Path.Combine(Common.Path_AppData, PrimaryParent.CurrentAppId + "\\AppSettings.xml");
            File.WriteAllText(xmlDestName, xmlSource);
            AppSettingsPath = "";
        }

        static private string _CreateAppSettingsXml(bool isPack)
        {
            string output = "";
            MemoryStream xmlMemoryStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(xmlMemoryStream, null);
            try
            {
                if(isPack)
                    xmlWriter.WriteStartElement("packSettings");
                else
                    xmlWriter.WriteStartElement("appSettings");
                if (!isPack || PackProgramSpecific) // Should be false except specified with isPack.
                {
                    xmlWriter.WriteAttributeString("id", PrimaryParent.CurrentAppId);
                    xmlWriter.WriteAttributeString("name", PrimaryParent.CurrentAppName);
                    xmlWriter.WriteAttributeString("path", PrimaryParent.CurrentAppPath);
                    xmlWriter.WriteAttributeString("className", PrimaryParent.CurrentAppWindowClassName);
                }
                xmlWriter.WriteAttributeString("fromX64", Common.Env64.ToString());
                //xmlWriter.WriteAttributeString("singleMode", singlemode);
                //xmlWriter.WriteAttributeString("specialFeatures", specialfeatures);

                // Evaluate the jumplist
                if (PrimaryParent.JumplistEnabled
                    && WriteJumplistXml(isPack))
                    xmlWriter.WriteElementString("jumpList", "JumpList.xml");

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show("AppSettings write not successful." + "\r\n"
                    + e.ToString());
            }
            finally
            {
                output = Encoding.UTF8.GetString(xmlMemoryStream.GetBuffer());
                xmlWriter.Close();
                xmlMemoryStream.Dispose();
            }

            return output; 
        }

        static public bool WriteJumplistXml(bool isPack)
        {
            // Delete ICO files; delete AHK files too???
            if (!isPack)
            {
                string[] iconFiles = Directory.GetFiles(AppSettingsPath, "*.ico");
                foreach (string iconName in iconFiles)
                    File.Delete(iconName);

                string[] kbdFiles = Directory.GetFiles(AppSettingsPath, "keyboard_*.ahk");
                foreach (string kbdName in kbdFiles)
                    File.Delete(kbdName);

                string[] ahkFiles = Directory.GetFiles(AppSettingsPath, "ahk_*.ahk");
                foreach (string ahkName in ahkFiles)
                {
                    if (File.Exists(ahkName + ".bak")) File.Delete(ahkName + ".bak");
                    File.Move(ahkName, ahkName + ".bak");
                }
            }

            //
            string xmlSource = _CreateJumplistXml(isPack);

            //
            string xmlDestName;
            if(isPack)
                xmlDestName = Path.Combine(PackTempDir, "JumpList.xml");
            else
                xmlDestName = Path.Combine(Common.Path_AppData, PrimaryParent.CurrentAppId + "\\JumpList.xml");
            File.WriteAllText(xmlDestName, xmlSource);
            
            return true;
        }

        static private string _CreateJumplistXml(bool isPack)
        {
            string output = "";
            MemoryStream xmlMemoryStream = new MemoryStream();
            XmlTextWriter xmlWriter = new XmlTextWriter(xmlMemoryStream, null);

            try
            {
                xmlWriter.WriteStartElement("jumpList");

                ListBox.ObjectCollection jumplistItems = PrimaryParent.JumplistListBox.Items;
                for (int i = 0; i < jumplistItems.Count; i++)
                {
                    // Look for a category
                    if (((T7EJumplistItem)jumplistItems[i]).ItemType == T7EJumplistItem.ItemTypeVar.Category
                        || ((T7EJumplistItem)jumplistItems[i]).ItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                    {
                        if(((T7EJumplistItem)jumplistItems[i]).ItemType == T7EJumplistItem.ItemTypeVar.Category)
                            xmlWriter.WriteStartElement("category");
                        else
                            xmlWriter.WriteStartElement("tasksCategory");
                        xmlWriter.WriteAttributeString("name", ((T7EJumplistItem)jumplistItems[i]).ItemName);

                        // Look for a task
                        for (int j = i+1; j < jumplistItems.Count; j++)
                        {
                            i = j-1; // When J loop is exited, i has to be less.
                            T7EJumplistItem jumplistItem = (T7EJumplistItem)jumplistItems[j];
                            switch (jumplistItem.ItemType)
                            {
                                case T7EJumplistItem.ItemTypeVar.Category:
                                case T7EJumplistItem.ItemTypeVar.CategoryTasks:
                                    j = jumplistItems.Count; // Exit the for loop
                                    break;

                                case T7EJumplistItem.ItemTypeVar.Task:
                                    ParseWriteJumpListTask(ref xmlWriter, isPack, jumplistItem, j);
                                    break;

                                case T7EJumplistItem.ItemTypeVar.FileFolder:
                                    xmlWriter.WriteStartElement("link");
                                    xmlWriter.WriteAttributeString("name", jumplistItem.ItemName);
                                    xmlWriter.WriteAttributeString("runWithApp", jumplistItem.FileRunWithApp.ToString());

                                    // Icon
                                    if (jumplistItem.ItemIconToString().Equals("Don't use an icon") != true)
                                    {
                                        // Icon processing
                                        // TODO: Jumplist Packs: If icon is "Use program icon," the icon is saved anyway, even though it shouldn't be
                                        // It's benign, but it wastes like 2kb of space per item.
                                        string localIconPath = IconPathToLocal(jumplistItem.ItemIconPath, jumplistItem.ItemIconIndex, j, PrimaryParent.CurrentAppId, isPack);
                                        if (SaveLocalIcon(localIconPath, jumplistItem.ItemIconBitmap))
                                        {
                                            if (isPack)
                                            {
                                                if(jumplistItem.ItemIconToString().Equals("Use program icon")) xmlWriter.WriteElementString("icon", "Use program icon");
                                                else xmlWriter.WriteElementString("icon", Path.GetFileName(localIconPath));
                                            }
                                            else
                                                xmlWriter.WriteElementString("icon", jumplistItem.ItemIconToString());
                                        }
                                    }

                                    if(isPack)
                                        xmlWriter.WriteElementString("location", ReplacePathsToVars(jumplistItem.FilePath));
                                    else
                                        xmlWriter.WriteElementString("location", jumplistItem.FilePath);
                                    xmlWriter.WriteEndElement();
                                    break;

                                case T7EJumplistItem.ItemTypeVar.Separator:
                                    xmlWriter.WriteElementString("separator", "");
                                    break;
                            }
                        }

                        xmlWriter.WriteEndElement();
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
            }
            catch (Exception e)
            {
                MessageBox.Show("JumpList write not successful." + "\r\n"
                    + e.ToString());
            }
            finally
            {
                output = Encoding.UTF8.GetString(xmlMemoryStream.GetBuffer());
                xmlWriter.Close();
                xmlMemoryStream.Dispose();
            }

            return output; 
        }

        static private void ParseWriteJumpListTask(ref XmlTextWriter xmlWriter, bool isPack, T7EJumplistItem jumplistItem, int itemIndex)
        {
            xmlWriter.WriteStartElement("task");
            xmlWriter.WriteAttributeString("name", jumplistItem.ItemName);

            if (jumplistItem.ItemIconToString().Equals("Don't use an icon") != true)
            {
                // Icon processing
                string localIconPath = IconPathToLocal(jumplistItem.ItemIconPath, jumplistItem.ItemIconIndex, itemIndex, PrimaryParent.CurrentAppId, isPack); 
                if(SaveLocalIcon(localIconPath, jumplistItem.ItemIconBitmap))
                {
                    if (isPack)
                    {
                        if (jumplistItem.ItemIconToString().Equals("Use program icon")) xmlWriter.WriteElementString("icon", "Use program icon");
                        else xmlWriter.WriteElementString("icon", Path.GetFileName(localIconPath));
                    }
                    else
                        xmlWriter.WriteElementString("icon", jumplistItem.ItemIconToString());
                }
            }

            xmlWriter.WriteStartElement("action");
            switch (jumplistItem.TaskAction)
            {
                case T7EJumplistItem.ActionType.Keyboard:
                    // Include AHK processing HERE in xml stage, or in jumplist stage?
                    // Include here.
                    if (!isPack)
                    { // We only need this if we're not writing a pack.
                        string kbdFilename = GetKeyboardScriptFilename(PrimaryParent, jumplistItem, itemIndex);
                        FormKeyboardScript(PrimaryParent, jumplistItem, kbdFilename);
                    }

                    xmlWriter.WriteAttributeString("type", "T7E_TYPE_KBD");
                    xmlWriter.WriteAttributeString("ignoreAbsent", jumplistItem.TaskKBDIgnoreAbsent.ToString());
                    xmlWriter.WriteAttributeString("ignoreCurrent", jumplistItem.TaskKBDIgnoreCurrent.ToString());
                    xmlWriter.WriteAttributeString("sendBackground", jumplistItem.TaskKBDSendInBackground.ToString());
                    xmlWriter.WriteAttributeString("minimizeAfterward", jumplistItem.TaskKBDMinimizeAfterward.ToString());
                    xmlWriter.WriteAttributeString("newWindow", jumplistItem.TaskKBDNew.ToString());
                    xmlWriter.WriteAttributeString("isShortcut", jumplistItem.TaskKBDShortcutMode.ToString());
                    xmlWriter.WriteCData(jumplistItem.TaskKBDString);
                    break;

                case T7EJumplistItem.ActionType.CommandLine:
                    xmlWriter.WriteAttributeString("type", "T7E_TYPE_CMD");
                    xmlWriter.WriteAttributeString("showWindow", jumplistItem.TaskCMDShowWindow.ToString());
                    xmlWriter.WriteAttributeString("workingDir", jumplistItem.ItemWorkDirToString());
                    if(isPack)
                        xmlWriter.WriteValue(ReplacePathsToVars(jumplistItem.ItemCmdToString()));
                    else
                        xmlWriter.WriteValue(jumplistItem.ItemCmdToString());
                    break;

                case T7EJumplistItem.ActionType.AutoHotKey:
                    // Include processing here.
                    string ahkFilename = GetAhkScriptFilename(isPack, jumplistItem, itemIndex);
                    FormAhkScript(isPack, jumplistItem, ahkFilename);

                    xmlWriter.WriteAttributeString("type", "T7E_TYPE_AHK");
                    xmlWriter.WriteValue(Path.GetFileName(ahkFilename));
                    break;
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
        }
        #endregion

        #region Jumplist Application
        static public void EraseJumplist(string appId)
        {
            try
            {
                JumpList dummyList = JumpList.CreateJumpListForAppId(appId);
                dummyList.ClearAllUserTasks();
                dummyList.Refresh();

                // Set appid back
                SetAppIdBackAfterJumplist();
            }
            catch (Exception e) { }
        }

        static public void SetAppIdBackAfterJumplist()
        {
            JumpList ownList = JumpList.CreateJumpListForAppId("T7E.Meta");
            // BUG: WE NEED TO FIGURE OUT HOW TO RETURN JUMP LIST TO NORMAL
            /*ownList.AddUserTasks(new JumpListLink
            {
                Title = "Visit the official website",
                Path = "%windir%\\explorer.exe",
                Arguments = "\"http://jumplist.gsdn-media.com\"",
                IconReference = new IconReference(Path.Combine(Common.EnvPath_SystemRoot, "system32\\shell32.dll"), 135)
            });*/
            ownList.Refresh();
            Common.TaskbarManagerInstance.SetCurrentProcessAppId("");
        }

        static public bool ApplyJumplistToTaskbar(Primary parent)
        {
            bool result = false;

            try
            {
                JumpList newList = JumpList.CreateJumpListForAppId(parent.CurrentAppId);
                //newList.KnownCategoryToDisplay = JumpListKnownCategoryType.Recent;
                //newList.KnownCategoryOrdinalPosition = 0;
                
                ListBox.ObjectCollection jumplistItems = parent.JumplistListBox.Items;
                for (int i = 0; i < jumplistItems.Count; i++)
                {
                    // Look for a category
                    T7EJumplistItem.ItemTypeVar jumplistItemType = ((T7EJumplistItem)jumplistItems[i]).ItemType;
                    if (jumplistItemType == T7EJumplistItem.ItemTypeVar.Category
                        || jumplistItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                    {
                        JumpListCustomCategory category = new JumpListCustomCategory(((T7EJumplistItem)jumplistItems[i]).ItemName);

                        // Look for a task
                        for (int j = i + 1; j < jumplistItems.Count; j++)
                        {
                            i = j - 1; // When J loop is exited, i has to be less.
                            T7EJumplistItem jumplistItem = (T7EJumplistItem)jumplistItems[j];
                            switch (jumplistItem.ItemType)
                            {
                                case T7EJumplistItem.ItemTypeVar.Category:
                                case T7EJumplistItem.ItemTypeVar.CategoryTasks:
                                    j = jumplistItems.Count; // Exit the for loop
                                    break;

                                case T7EJumplistItem.ItemTypeVar.Task:
                                    if (jumplistItemType == T7EJumplistItem.ItemTypeVar.Category)
                                        category.AddJumpListItems(
                                            ParseApplyJumpListTask(parent, jumplistItem, j));
                                    else
                                        newList.AddUserTasks(
                                            ParseApplyJumpListTask(parent, jumplistItem, j));
                                    break;

                                case T7EJumplistItem.ItemTypeVar.FileFolder:
                                    // If file is an EXE, just pass the path over.
                                    // TODO: Merge "Command line" and "File/Folder shortcut" into one dialog, based on "Command Line"

                                    JumpListLink link = new JumpListLink
                                    {
                                        Title = jumplistItem.ItemName,
                                        Path = "C:\\Windows\\explorer.exe",
                                        Arguments = jumplistItem.FilePath
                                        // working directory here? We don't know what the program is.
                                        // AHK doesn't detect the default program's working dir, either.
                                    };
                                    if (jumplistItem.FileRunWithApp)
                                    {
                                        link.Path = parent.CurrentAppPath;
                                        // we use working dir here because we KNOW the program to use
                                        link.WorkingDirectory = Path.GetDirectoryName(parent.CurrentAppPath);
                                    }

                                    
                                    if (Path.GetExtension(jumplistItem.FilePath).ToLower() == ".exe")
                                    {
                                        link.Path = jumplistItem.FilePath;
                                        link.Arguments = "";
                                        // we use working dir here because we KNOW the program to use
                                        link.WorkingDirectory = Path.GetDirectoryName(jumplistItem.FilePath);
                                    }
                                    

                                    // Format icon
                                    if (jumplistItem.ItemIconToString().Equals("Don't use an icon") != true)
                                    {
                                        // Icon processing
                                        string localIconPath = IconPathToLocal(jumplistItem.ItemIconPath, jumplistItem.ItemIconIndex, j, parent.CurrentAppId, false);
                                        if (File.Exists(localIconPath))
                                            link.IconReference = new IconReference(localIconPath, 0);
                                    }

                                    if (jumplistItemType == T7EJumplistItem.ItemTypeVar.Category)
                                        category.AddJumpListItems(link);
                                    else
                                        newList.AddUserTasks(link);
                                    break;

                                case T7EJumplistItem.ItemTypeVar.Separator:
                                    if (jumplistItemType == T7EJumplistItem.ItemTypeVar.CategoryTasks)
                                        newList.AddUserTasks(new JumpListSeparator());
                                    break;
                            }
                        }

                        if (jumplistItemType == T7EJumplistItem.ItemTypeVar.Category)
                            newList.AddCustomCategories(category);
                    }
                }

                // ////////
                JumpList dummyList = JumpList.CreateJumpListForAppId(parent.CurrentAppId);
                /*dummyList.AddUserTasks(new JumpListLink
                {
                    Title = "Dummy",
                    Path = "%windir%\\explorer.exe"
                });*/
                dummyList.ClearAllUserTasks();
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

        static public bool ApplyBlankJumplistToTaskbar(Primary parent)
        {
            bool result = false;

            try
            {
                // ////////
                JumpList dummyList = JumpList.CreateJumpListForAppId(parent.CurrentAppId);
                dummyList.ClearAllUserTasks();
                dummyList.Refresh();
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

        static private JumpListLink ParseApplyJumpListTask(Primary parent, T7EJumplistItem jumplistItem, int itemIndex)
        {
            JumpListLink task = new JumpListLink
                {
                    Title = jumplistItem.ItemName
                };

            if (jumplistItem.ItemIconToString().Equals("Don't use an icon") != true)
            {
                // Icon processing
                string localIconPath = IconPathToLocal(jumplistItem.ItemIconPath, jumplistItem.ItemIconIndex, itemIndex, parent.CurrentAppId, false);
                if (File.Exists(localIconPath))
                    task.IconReference = new IconReference(localIconPath, 0);
            }

            switch (jumplistItem.TaskAction)
            {
                case T7EJumplistItem.ActionType.Keyboard:
                    string kbdScriptName = GetKeyboardScriptFilename(parent, jumplistItem, itemIndex);
                    if (File.Exists(kbdScriptName))
                    {// It should already have been made
                        task.Path = Common.Path_ProgramFiles + "\\AutoHotKey.exe";
                        task.Arguments = "\"" + kbdScriptName + "\"";
                    }
                    else return null;
                    break;

                case T7EJumplistItem.ActionType.CommandLine:
                    if (jumplistItem.TaskCMDShowWindow)
                    {
                        task.Path = "cmd.exe";
                        task.Arguments = "/k \"" + jumplistItem.ItemCmdToString().Replace("\"", "\"\"") + "\"";
                    } else {
                        task.Path = jumplistItem.TaskCMDPath;
                        task.Arguments = jumplistItem.TaskCMDArgs;
                    }
                    if (string.IsNullOrEmpty(jumplistItem.TaskCMDWorkDir))
                        task.WorkingDirectory = Path.GetDirectoryName(jumplistItem.TaskCMDPath);
                    else
                        task.WorkingDirectory = jumplistItem.TaskCMDWorkDir;
                    break;

                case T7EJumplistItem.ActionType.AutoHotKey:
                    string ahkFilename = GetAhkScriptFilename(false, jumplistItem, itemIndex);
                    if (File.Exists(ahkFilename)) // It should have already been made.
                    {
                        // Working directory info for autohotkey? Probably not,
                        // since some scripts will use the predefined scripts in appdata.
                        task.Path = Common.Path_ProgramFiles + "\\AutoHotKey.exe";
                        task.Arguments = "\"" + ahkFilename + "\"";
                    }
                    else return null;
                    break;
            }
            return task;
        }
        #endregion

        #region Data Utilities
        static private string GetKeyboardScriptFilename(Primary parent, T7EJumplistItem jumplistItem, int itemIndex)
        {
            // keyboard_1.ahk
            return Path.Combine(Common.Path_AppData, parent.CurrentAppId + "\\keyboard_" + itemIndex + ".ahk");
        }

        static private string GetAhkScriptFilename(bool isPack, T7EJumplistItem jumplistItem, int itemIndex)
        {
            // ahk_1.ahk
            if (isPack)
                return Path.Combine(PackTempDir, "ahk_" + itemIndex + ".ahk");
            else
                return Path.Combine(Common.Path_AppData, PrimaryParent.CurrentAppId + "\\ahk_" + itemIndex + ".ahk");
        }

        static private bool FormAhkScript(bool isPack, T7EJumplistItem jumplistItem, string fileName)
        {
            if (jumplistItem.TaskAHKScript.Length < 1) return false;

            string ahkScript = jumplistItem.TaskAHKScript;

            if (isPack)
            {
                // Replace paths with vars, but gotta do this in special way
                ahkScript = ahkScript.Replace(
                    "JLE_AppId := \"" + PrimaryParent.CurrentAppId + "\"",
                    "JLE_AppId := \"{JLE_AppId}\""
                    );

                ahkScript = ahkScript.Replace(
                    "JLE_AppName := \"" + PrimaryParent.CurrentAppName + "\"",
                    "JLE_AppName := \"{JLE_AppName}\""
                    );
                ahkScript = ahkScript.Replace(
                    "; AutoHotKey Script - For \"" + PrimaryParent.CurrentAppName + "\"",
                    "; AutoHotKey Script - For \"{JLE_AppName}\""
                    );

                string tempAppPath = PrimaryParent.CurrentAppPath;

                ahkScript = ahkScript.Replace(
                    "JLE_AppPath := \"" + tempAppPath + "\"",
                    "JLE_AppPath := \"{JLE_AppPath}\"");

                ahkScript = ahkScript.Replace(
                    "JLE_AppProcessName := \"" + Path.GetFileName(PrimaryParent.CurrentAppPath) + "\"",
                    "JLE_AppProcessName := \"{JLE_AppProcessName}\"");

                ahkScript = ahkScript.Replace(
                    "JLE_AppWindowClassName := \"" + PrimaryParent.CurrentAppWindowClassName + "\"",
                    "JLE_AppWindowClassName := \"{JLE_AppWindowClassName}\"");

                ahkScript = ReplaceT7EPathTags(ahkScript);
                ahkScript = Common.ReplaceExpandedPathToEnvVar(ahkScript);
            }

            TextWriter scriptWriter = new StreamWriter(fileName);
            scriptWriter.Write(ahkScript);
            scriptWriter.Dispose();
            return true;
        }

        static private void FormKeyboardScript(Primary parent, T7EJumplistItem jumplistItem, string fileName)
        {
            string templateText = Common.Template_KBD;

            string tempAppPath = parent.CurrentAppPath;

            templateText = templateText.Replace("{Path_AppData}", Common.Path_AppData);
            templateText = templateText.Replace("{AppId}", parent.CurrentAppId);
            templateText = templateText.Replace("{AppName}", parent.CurrentAppName);
            templateText = templateText.Replace("{AppPath}", tempAppPath);
            templateText = templateText.Replace("{AppProcessName}", Path.GetFileName(parent.CurrentAppPath).ToLower());
            templateText = templateText.Replace("{AppWindowClassName}", parent.CurrentAppWindowClassName);
            templateText = templateText.Replace("{Keystroke}", jumplistItem.TaskKBDString);

            templateText = templateText.Replace("{KBDStartNewProcess}", Convert.ToInt32(jumplistItem.TaskKBDNew).ToString());
            templateText = templateText.Replace("{KBDIgnoreAbsent}", Convert.ToInt32(jumplistItem.TaskKBDIgnoreAbsent).ToString());
            templateText = templateText.Replace("{KBDIgnoreCurrent}", Convert.ToInt32(jumplistItem.TaskKBDIgnoreCurrent).ToString());
            if(jumplistItem.TaskKBDMinimizeAfterward)
                templateText = templateText.Replace("{KBDSendBackground}", 2.ToString());
            else
                templateText = templateText.Replace("{KBDSendBackground}", Convert.ToInt32(jumplistItem.TaskKBDSendInBackground).ToString());
            
            TextWriter scriptWriter = new StreamWriter(fileName);
            scriptWriter.Write(templateText);
            scriptWriter.Dispose();
        }

        static private string IconPathToLocal(string iconPath, int iconIndex, string appId)
        {
            return IconPathToLocal(iconPath, iconIndex, 0, appId, false);
        }

        static private string IconPathToLocal(string iconPath, int iconIndex, int itemIndex, string appId, bool isPack)
        {
            if(isPack)
                return Path.Combine(PackTempDir,
                 Path.GetFileNameWithoutExtension(iconPath)
                 + "_" + iconIndex
                 + "_" + itemIndex
                 + ".ico");
            else 
                return Path.Combine(Common.Path_AppData, appId + "\\"
                 + Path.GetFileNameWithoutExtension(iconPath)
                 + "_" + iconIndex
                 + "_" + itemIndex
                 + ".ico");
        }

        static private bool SaveLocalIcon(string localIconPath, Bitmap iconBitmap)
        {
            try
            {
                MultiIcon bmIcon = new MultiIcon();
                SingleIcon bsIcon = bmIcon.Add("Icon 1");

                IconOutputFormat outputFormat = IconOutputFormat.None; // Save icon as is

                bsIcon.CreateFrom(iconBitmap, outputFormat);
                bsIcon.Save(localIconPath);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
        #endregion

        public static void DisableApp(string appId, string appPath, string appName)
        {
            // The difference is that DisableApp() is removed from the internal jumplist
            // and AppList.xml, BUT the data stays in AppData.
            // Plus, it gets moved to DisableAppList.xml, so T7EPreferences can tell
            // the difference.

            // 1. Delete from internal jumplist
            Common.AppIds.Remove(appId);
            Common.AppProcessNames.Remove(Path.GetFileNameWithoutExtension(appPath));
            Common.AppCount--;

            // 2. Rewrite AppList.xml without the id
            WriteAppListXml();

            // T7EBackground should be replacing the window and shortcut appids here.
            // RACE CONDITION: Prevent OrigProperties/App Name.lnk from deleting before
            // T7EBackground gets around to it. For now, we just leave it.
            // Not a good practice?

            // 3. Add id to DisabledAppList.xml
            DisabledList_AddApp(appId, appPath, appName);
        }

        public static int DisabledList_AppCount()
        {
            string disabledAppListXmlPath = Path.Combine(Common.Path_AppData, "DisabledAppList.xml");
            XmlDocument disabledAppList = new XmlDocument();
            if (File.Exists(disabledAppListXmlPath))
                disabledAppList.Load(disabledAppListXmlPath);
            else
                return 0;
            
            return disabledAppList.DocumentElement.GetElementsByTagName("app") != null ?
                disabledAppList.DocumentElement.GetElementsByTagName("app").Count
                : 0;
        }

        static void DisabledList_AddApp(string appId, string appPath, string appName)
        {
            string disabledAppListXmlPath = Path.Combine(Common.Path_AppData, "DisabledAppList.xml");
            XmlDocument disabledAppList = new XmlDocument();
            if (File.Exists(disabledAppListXmlPath))
                disabledAppList.Load(disabledAppListXmlPath);
            else
                disabledAppList.AppendChild(disabledAppList.CreateElement("disabledAppList"));

            XmlElement disabledAppNode = disabledAppList.CreateElement("app");
            disabledAppNode.SetAttribute("id", appId);
            disabledAppNode.SetAttribute("name", appName);
            disabledAppNode.SetAttribute("processName", Path.GetFileNameWithoutExtension(appPath));
            disabledAppNode.SetAttribute("path", appPath);

            //existingNode = disabledAppList.GetElementById(appId);
            foreach (XmlElement appElement in disabledAppList.GetElementsByTagName("app"))
                if (appElement.GetAttribute("id") == appId)
                {
                    disabledAppList.DocumentElement.RemoveChild(appElement);
                    break;
                }

            
            disabledAppList.DocumentElement.AppendChild(disabledAppNode);

            disabledAppList.Save(disabledAppListXmlPath);
        }

        static void DisabledList_RemoveApp(string appId)
        {
            string disabledAppListXmlPath = Path.Combine(Common.Path_AppData, "DisabledAppList.xml");
            XmlDocument disabledAppList = new XmlDocument();
            if (File.Exists(disabledAppListXmlPath))
                disabledAppList.Load(disabledAppListXmlPath);
            else
                return;

            //existingNode = disabledAppList.GetElementById(appId);
            foreach (XmlElement appElement in disabledAppList.GetElementsByTagName("app"))
                if (appElement.GetAttribute("id") == appId)
                {
                    disabledAppList.DocumentElement.RemoveChild(appElement);
                    break;
                } 

            disabledAppList.Save(disabledAppListXmlPath);
        }

        public static void DeleteApp(string appId, string appPath, bool appIsDisabled)
        {
            if (appIsDisabled)
            {
                // Delete app from DisabledAppList.xml 
                DisabledList_RemoveApp(appId);
            }

            DeleteApp(appId, appPath);
        }

        public static void EnableApp(string appId, string appPath)
        {
            // 1. Add entry to internal AppList
            Common.AppIds.Add(appId);
            Common.AppProcessNames.Add(Path.GetFileNameWithoutExtension(appPath));
            Common.AppCount++;

            // 2. Add entry to applist.xml
            Preferences.WriteAppListXml();

            // 3. Remove entry from disabledapplist.xml
            DisabledList_RemoveApp(appId);
        }

        public static void DeleteApp(string appId, string appPath)
        {
            // 1. Delete from internal jumplist
            try
            {
                Common.AppIds.Remove(appId);
                Common.AppProcessNames.Remove(Path.GetFileNameWithoutExtension(appPath));
                Common.AppCount--;

                // 2. Rewrite AppList.xml without the appid.
                WriteAppListXml();
            }
            catch (Exception e) { /* Just means app was disabled. */ }

            // T7EBackground should be replacing the window and shortcut appids here.
            // RACE CONDITION: Prevent OrigProperties/App Name.lnk from deleting before
            // T7EBackground gets around to it. For now, we just leave it.
            // Not a good practice?

            // 3. Erase the corresponding jumplist
            try
            {
                JumpList dummyList = JumpList.CreateJumpListForAppId(appId);
                dummyList.Refresh();
            }
            catch (Exception e) { }
            // Reset this window's appid!

            // 4. Delete the app from AppData
            Directory.Delete(Path.Combine(Common.Path_AppData, appId), true);
            // Delete OrigProperties/App Name.lnk
        }

        public static void WriteAppListXml()
        {
            // Add entry to AppList.xml
            string appListXmlPath = Path.Combine(Common.Path_AppData, "AppList.xml");
            if (File.Exists(appListXmlPath))
                File.Copy(appListXmlPath, appListXmlPath + ".bak", true);

            XmlTextWriter xmlWriter;
            int retries = 10;
            while (true)
            {
                try
                {
                    xmlWriter = new XmlTextWriter(appListXmlPath, null);
                    break; // success!
                }
                catch
                {
                    if (--retries == 0) return;
                    else System.Threading.Thread.Sleep(500);
                }
            }

            xmlWriter.WriteStartElement("appList");
            xmlWriter.WriteAttributeString("cleanAppIdsOnExit", "false");
            for (int i = 0; i < Common.AppCount; i++)
            {
                xmlWriter.WriteStartElement("app");
                xmlWriter.WriteAttributeString("id", Common.AppIds[i]);
                xmlWriter.WriteAttributeString("processName", Common.AppProcessNames[i]);
                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        public static void SaveApp(Primary parent)
        {
            parent.ActiveControl = null;

            WriteAppListXml();
            WriteAppSettingsXml();
        }
    }
}
