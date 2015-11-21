using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T7ECommon;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace T7EBackground
{
    public partial class Primary
    {
        #region Shortcut Handling
        /// <summary>
        /// Checks list of shortcuts against newly added AppIds
        /// </summary>
        /// <param name="shortcutList">List of shortcuts to check</param>
        /// <param name="changesList">Compare list to compare to</param>
        private void CheckShortcutsAddedAppIds(List<string> shortcutList, Dictionary<string, string>[] changesList)
        {
            if (DisableShortcutChanging) return;

            Dictionary<string, string> compareList;
            if (changesList[0].Count > 0)
                compareList = changesList[0];
            else
            {
                CommonLog("Compare list is empty. Finished checking for AppIds.", -1);
                return;
            }

            CommonLog("Checking shortcuts for " + compareList.Count.ToString() + " AppIds.", 1);

            // Check each LNK file in shortcutList
            foreach (string lnkPath in shortcutList)
            {
                CommonLog("Checking shortcut " + lnkPath, 1);

                // Get the shortcut LNK path
                string targetName = Path.GetFileNameWithoutExtension(Common.ResolveLnkPath(lnkPath));
                CommonLog("Shortcut's target name: " + targetName);

                if (targetName.Length <= 0)
                {
                    CommonLog(-1);
                    continue;
                }

                // Check each AppId for matches against LNK file
                foreach (KeyValuePair<string, string> appPair in compareList)
                {
                    string appProcessName = appPair.Key;
                    string appId = appPair.Value;

                    CommonLog("Checking shortcut against " + Common.GetLogAppString(appProcessName, appId), 1);

                    // Does the LNK filename match the AppProcessName? If it does, manipulate its shortcut.
                    if (targetName.Equals(appProcessName, StringComparison.OrdinalIgnoreCase)) // We found match!
                    {
                        CommonLog("Shortcut matched!");

                        if (Environment.OSVersion.Version.Major >= 10)
                        {
                            CommonLog("Assigning app ID to pinned shortcut.");
                            Common.TaskbarManagerInstance.SetApplicationIdForShortcut(lnkPath, appId);
                        }
                        else
                        {
                            // Is shortcut original, or tampered? Check its appId for T7E
                            string lnkAppId = Common.TaskbarManagerInstance.GetApplicationIdForShortcut(lnkPath);
                            string lnkFilename = Path.GetFileName(lnkPath);

                            // Get the LNK paths
                            string origLnkPath = Path.Combine(Common.Path_AppData, "OrigProperties\\" + lnkFilename);
                            string modifiedLnkPath = Path.Combine(Common.Path_AppData, appId + "\\" + lnkFilename);

                            if (lnkAppId.Length > 3 && lnkAppId.Substring(0, 3).Equals("T7E"))
                            {
                                CommonLog("Shortcut has existing T7E AppId: " + lnkAppId);
                                CommonLog("Copying original if it exists");

                                // It's tampered. Copy over original to User Pinned
                                if (File.Exists(origLnkPath))
                                    File.Copy(origLnkPath, lnkPath, true);
                            }
                            else
                            {
                                CommonLog("Shortcut is original, unmodified.");
                                if (lnkAppId.Length > 3)
                                    CommonLog("Shortcut has its own AppId: " + lnkAppId);

                                // It's original. Copy the original to appdata
                                File.Copy(lnkPath, origLnkPath, true);
                            }


                            // Apply T7E appId to modifiedLnkPath
                            if (!File.Exists(modifiedLnkPath))
                            {
                                CommonLog("Modified shortcut backup does not exist -- creating.");
                                File.Copy(lnkPath, modifiedLnkPath, true);
                                Common.TaskbarManagerInstance.SetApplicationIdForShortcut(modifiedLnkPath, appId);
                            }

                            // Pin modifiedLnkPath shortcut
                            CommonLog("Copying modified shortcut to pinned.");
                            /*UnpinShortcut(lnkPath);
                            if (lnkPath.LastIndexOf("StartMenu") > 0)
                                PinShortcut(modifiedLnkPath, true);
                            else
                                PinShortcut(modifiedLnkPath, false);*/
                            // I can just copy over the modified LNK. The jumplist will show.
                            File.Copy(modifiedLnkPath, lnkPath, true);
                        }

                        CommonLog("Finished checking appid.", -1);

                        break; // Since a shortcut link belongs to only one appId
                    }
                    else
                        CommonLog(-1);
                }

                CommonLog("Finished checking shortcut.", -1);
            }
        }

        /// <summary>
        /// Checks list of shortcuts against deleted AppIds
        /// </summary>
        /// <param name="shortcutList">List of shortcuts to check</param>
        /// <param name="changesList">Compare list to compare to</param>
        private void CheckShortcutsDeletedAppIds(List<string> shortcutList, Dictionary<string,string>[] changesList)
        {
            if (DisableShortcutChanging) return;

            if (changesList[1].Count > 0)
            {
                Dictionary<string, string> deletedList = new Dictionary<string, string>();
                deletedList = changesList[1];

                CommonLog("Checking shortcuts for " + deletedList.Count.ToString() + " deleted AppIds.", 1);

                // For each shortcut link we want to check...
                foreach (string lnkPath in shortcutList)
                {
                    CommonLog("Checking shortcut " + lnkPath, 1);

                    string targetName = Path.GetFileNameWithoutExtension(Common.ResolveLnkPath(lnkPath));

                    CommonLog("Shortcut's target name: " + targetName);

                    // And check shortcut link object against every appId we want to
                    foreach (KeyValuePair<string, string> deletedAppPair in deletedList)
                    {
                        string appProcessName = deletedAppPair.Key;
                        string appId = deletedAppPair.Value;

                        CommonLog("Checking shortcut against " + Common.GetLogAppString(appProcessName, appId), 1);

                        if (targetName.Equals(appProcessName, StringComparison.OrdinalIgnoreCase)) // We found match!
                        {
                            CommonLog("Shortcut matched!");

                            string lnkFilename = Path.GetFileName(lnkPath);
                            string origLnkPath = Path.Combine(Common.Path_AppData, "OrigProperties\\" + lnkFilename);

                            // Replace the shortcut with the original, if exists
                            if (Environment.OSVersion.Version.Major >= 10)
                            {
                                CommonLog("Removing application ID from original shortcut");
                                Common.TaskbarManagerInstance.SetApplicationIdForShortcut(lnkPath, "");
                            }
                            else
                            {
                                if (File.Exists(origLnkPath))
                                {
                                    CommonLog("Pinning/Unpinning original shortcut");

                                    // Pin original shortcut
                                    UnpinShortcut(lnkPath);
                                    if (lnkPath.LastIndexOf("StartMenu") > 0)
                                        PinShortcut(origLnkPath, true);
                                    else
                                        PinShortcut(origLnkPath, false);
                                    //File.Copy(origLnkPath, lnkPath, true);
                                    //File.Delete(origLnkPath);
                                }
                                else
                                {
                                    CommonLog("Original shortcut backup does not exist, creating blank AppId shortcut");
                                    // If it doesn't exist, just remove T7E from lnkPath
                                    string tmpLnkPath = Path.Combine(Path.GetTempPath(), lnkFilename);
                                    File.Copy(lnkPath, tmpLnkPath, true);
                                    Common.TaskbarManagerInstance.SetApplicationIdForShortcut(tmpLnkPath, "");
                                    origLnkPath = tmpLnkPath;
                                }
                            }

                            CommonLog(-1);
                            break; // Since a shortcut belongs to only one appId
                        }

                        CommonLog(-1);
                    }

                    CommonLog("Finished checking shortcut.", -1);
                }

                CommonLog("Finished checking for deleted AppIds.", -1);
            }
        }

        public string GetStringResource(IntPtr hModuleInstance, uint uiStringID)
        {
            StringBuilder sb = new StringBuilder(255);
            Interop.LoadString(hModuleInstance, uiStringID, sb, sb.Capacity + 1);
            return sb.ToString();
        }

        private void UnpinShortcut(string lnkPath)
        {
            // If lnk is on a network dir, it needs to be copied to local temp
            // Windows remembers which shortcuts are pinned to taskbar
            if (!File.Exists(lnkPath)) return;

            if (DisableShortcutChanging) return;

            string tempLnkPath = "";
            if (Interop.PathIsNetworkPath(lnkPath))
            {
                tempLnkPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(lnkPath));
                File.Copy(lnkPath, tempLnkPath);
                lnkPath = tempLnkPath;

                if (!File.Exists(lnkPath)) return;
            }

            // Unpinning shortcuts is under UnpinShortcut()
            IntPtr shell32Module = Interop.GetModuleHandle("shell32.dll");

            string command;
            if (lnkPath.Contains("StartMenu"))
                command = GetStringResource(shell32Module, 5382); // Unpin from Start Men&u // win8+: 51394
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

            if (File.Exists(tempLnkPath)) File.Delete(tempLnkPath);
        }

        private void PinShortcut(string lnkPath, bool pinStartMenu)
        {
            // If lnk is on a network dir, it needs to be copied to local temp
            // Windows remembers which shortcuts are pinned to taskbar
            // Does T7EBackground need to be local too???
            if (!File.Exists(lnkPath)) return;

            if (DisableShortcutChanging) return;

            string tempLnkPath = "";
            if(Interop.PathIsNetworkPath(lnkPath))
            {
                tempLnkPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(lnkPath));
                File.Copy(lnkPath, tempLnkPath);
                lnkPath = tempLnkPath;

                if (!File.Exists(lnkPath)) return;
            }

            LnkWatcher.EnableRaisingEvents = false;

            IntPtr shell32Module = Interop.GetModuleHandle("shell32.dll");

            string command;
            if (pinStartMenu)
                command = GetStringResource(shell32Module, 5381); // Pin to Start Men&u // win8+: 51201
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

            if (LnkWatcher.Path.Length > 0)
                LnkWatcher.EnableRaisingEvents = true;

            if (File.Exists(tempLnkPath)) File.Delete(tempLnkPath);
        }

        private void UnpinTempShortcut(IntPtr hWnd, string appId)
        {
            if (DisableShortcutChanging) return;

            string hwndProcessPath = "";
            string hwndProcessName = "";
            string tempLnkPath = "";

            if(!IntPtr.Equals(hWnd, IntPtr.Zero))
            {
                // Get process ID from window
                int hwndPid;
                Interop.GetWindowThreadProcessId(hWnd, out hwndPid);

                // Get process handle from pID; get process path from handle
                IntPtr hwndPidHandle = Interop.OpenProcess(Interop.ProcessAccess.QueryInformation | Interop.ProcessAccess.VMRead, false, hwndPid);
                StringBuilder hwndProcessNameString = new StringBuilder(260);
                Interop.GetModuleFileNameEx(hwndPidHandle, IntPtr.Zero, hwndProcessNameString, 260);
                Interop.CloseHandle(hwndPidHandle);

                try { hwndProcessPath = hwndProcessNameString.ToString(); }
                catch (Exception e) { return; }

                if (!File.Exists(hwndProcessPath)) return;

                hwndProcessName = Path.GetFileNameWithoutExtension(hwndProcessPath).ToLower();

                tempLnkPath = Path.Combine(Common.Path_AppData, "TempShortcuts" + "\\" + hwndProcessName + ".lnk");
            }
            else
            {
                // Get lnk from appId
                foreach (KeyValuePair<string, string> lnkPair in TempShortcut_AppIdPairs)
                {
                    string lnkName = lnkPair.Key;
                    string lnkAppId = lnkPair.Value;

                    if (appId == lnkAppId)
                    {
                        hwndProcessName = Path.GetFileNameWithoutExtension(lnkName).ToLower();
                        tempLnkPath = lnkName;
                    }
                }
            }

            if (!IsAppPinnedByProcessName(hwndProcessName)) return;

            if (File.Exists(tempLnkPath))
            {
                if (TempShortcut_AppIdPairs.ContainsKey(tempLnkPath))
                    TempShortcut_AppIdPairs.Remove(tempLnkPath);

                UnpinShortcut(tempLnkPath);
                File.Delete(tempLnkPath);
            }
            
        }

        private void PinTempShortcutFromHwnd(IntPtr hWnd, string appId)
        {
            if (DisableShortcutChanging) return;

            // Get process ID from window
            int hwndPid;
            Interop.GetWindowThreadProcessId(hWnd, out hwndPid);

            // Get process handle from pID; get process path from handle
            IntPtr hwndPidHandle = Interop.OpenProcess(Interop.ProcessAccess.QueryInformation | Interop.ProcessAccess.VMRead, false, hwndPid);
            StringBuilder hwndProcessNameString = new StringBuilder(260);
            Interop.GetModuleFileNameEx(hwndPidHandle, IntPtr.Zero, hwndProcessNameString, 260);
            Interop.CloseHandle(hwndPidHandle);

            string hwndProcessPath;
            try { hwndProcessPath = hwndProcessNameString.ToString(); }
            catch (Exception e) { return; }

            if (!File.Exists(hwndProcessPath)) return;

            string hwndProcessName = Path.GetFileNameWithoutExtension(hwndProcessPath).ToLower();

            if(IsAppPinnedByProcessName(hwndProcessName)) return;

            string newLnkPath = Path.Combine(Common.Path_AppData, "TempShortcuts" + "\\" + hwndProcessName + ".lnk");

            using (ShellLink shortcut = new ShellLink())
            {
                shortcut.Target = hwndProcessPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(hwndProcessPath);
                shortcut.Description = hwndProcessName;
                shortcut.DisplayMode = ShellLink.LinkDisplayMode.edmNormal;
                shortcut.Save(newLnkPath);
            }

            Common.TaskbarManagerInstance.SetApplicationIdForShortcut(newLnkPath, appId);

            if (TempShortcut_AppIdPairs.ContainsKey(newLnkPath))
                TempShortcut_AppIdPairs.Remove(newLnkPath);
            TempShortcut_AppIdPairs.Add(newLnkPath, appId);

            PinShortcut(newLnkPath, false);
        }

        private bool IsAppPinnedByProcessName(string processName)
        {
            if (processName.Length < 1) return false;

            // Check for all shortcuts under %appdata%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar and StartMenu
            List<string> shortcutList = new List<string>();
            string userPinnedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned");
            DirectoryInfo di = new DirectoryInfo(userPinnedPath);
            FileInfo[] lnkInfo = di.GetFiles("*.lnk", SearchOption.AllDirectories);

            foreach (FileInfo lnk in lnkInfo)
            {
                string targetName = Path.GetFileNameWithoutExtension(Common.ResolveLnkPath(lnk.FullName)).ToLower();

                if (processName.ToLower() == targetName) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks all TaskBar and StartMenu shortcuts against the master AppId list.
        /// </summary>
        private void AssignAppIdsToShortcuts()
        {
            AssignAppIdsToShortcuts("");
        }

        /// <summary>
        /// Checks a single shortcut against the master AppId list.
        /// </summary>
        /// <param name="shortcutPath">Full path to LNK file</param>
        private void AssignAppIdsToShortcuts(string shortcutPath)
        {
            Dictionary<string, string>[] appPairsList = new Dictionary<string, string>[2];

            Dictionary<string, string> appPairs = new Dictionary<string, string>();

            // Make an AppChangesList by adding the master AppList to appPairsList[0], the "added AppIds" list.
            // Leave appPairsList[1], the "deleted AppIds" list, blank.
            for (int i = 0; i < Common.AppCount; i++)
                appPairs.Add(Common.AppProcessNames[i], Common.AppIds[i]);
            appPairsList[0] = appPairs;
            appPairsList[1] = new Dictionary<string, string>();

            AssignAppIdsToShortcuts(shortcutPath, appPairsList);
        }

        /// <summary>
        /// Main procedure for assigning AppIds to shortcuts. Takes single LNK and checks it against an AppChangesList
        /// </summary>
        /// <param name="shortcutPath">Full path to target LNK file. Leave blank to check for every TaskBar and StartMenu shortcut.</param>
        /// <param name="changesList">Two-part Dictionary array, consisting of added AppIds and deleted AppIds</param>
        private void AssignAppIdsToShortcuts(string shortcutPath, Dictionary<string, string>[] changesList)
        {
            if (DisableShortcutChanging) return;

            CommonLog("Assigning AppIds to User Pinned shortcuts", 1);

            // First, populate shortcutList with all shortcutrs we want to check
            List<string> shortcutList = new List<string>();
            if (File.Exists(shortcutPath))
                shortcutList.Add(shortcutPath); // Only check for this
            else
            {
                // Check for all shortcuts under %appdata%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar and StartMenu
                string userPinnedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned");
                DirectoryInfo di = new DirectoryInfo(userPinnedPath);
                FileInfo[] lnkInfo = di.GetFiles("*.lnk", SearchOption.AllDirectories);
                foreach (FileInfo lnk in lnkInfo)
                    shortcutList.Add(lnk.FullName);
            }

            CommonLog("Read shortcut list to check. Length: " + shortcutList.Count.ToString());

            /////////////

            // Check for newly deleted appIds
            CheckShortcutsDeletedAppIds(shortcutList, changesList);

            // Check for newly added appIds, or all appIds
            CheckShortcutsAddedAppIds(shortcutList, changesList);

            CommonLog("Done assigning AppIds to shortcuts.", -2);
        }

        private void lnkWatcher_Created(object sender, FileSystemEventArgs e)
        {
            var loopI = 0;
            LnkWatcher_Beginning:

            if ((new FileInfo(e.FullPath).Length) <= 0)
            {
                Thread.Sleep(20);
                loopI++;
                if (loopI <= 13) goto LnkWatcher_Beginning; // Try for 260ms
                else return;
            }

            //System.Windows.Forms.MessageBox.Show(Common.TaskbarManagerInstance.GetApplicationIdForShortcut(e.FullPath));

            CommonLog("Detected new shortcut: " + e.FullPath, 1);

            // Assign appid to last created shortcut
            if (!DisableShortcutChanging)
                AssignAppIdsToShortcuts(e.FullPath);

            CommonLog("Finished handling new shortcut.", 0);
        }


        private void LnkWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            // We need to assign a blank appid to any open windows
            // and, if not win10 rtm, then reassign the appid

            /*
            // Check if lnk file is in temp pinned list; if so, repin.
            // Jump list removals should result in removal from pin list before this event is called

            foreach (KeyValuePair<string, string> lnkPair in TempShortcut_AppIdPairs)
            {
                string lnkName = lnkPair.Key;
                string appId = lnkPair.Value;

                // We need to assign a blank appid to any open windows
                // and, if not win10 rtm, then reassign the appid

                if(Path.GetFileNameWithoutExtension(e.FullPath) == Path.GetFileNameWithoutExtension(lnkName))
                {
                    PinShortcut(lnkName, false); 
                }
            }
            */
        }
        
        #endregion
        }
    }
