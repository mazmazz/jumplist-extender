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

        /// <summary>
        /// Programmatically unpins a LNK from either the taskbar or start menu, depending on the path.
        /// </summary>
        /// <param name="lnkPath">Full path to the LNK. Its location in StartMenu or TaskBar determines where to unpin it from.</param>
        private void UnpinShortcut(string lnkPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cscript.exe";
            startInfo.Arguments = "\"" + Path.Combine(Common.Path_ProgramFiles, "PinShortcut.vbs") + "\" /unpin \"" + Path.GetDirectoryName(lnkPath) + "\" \"" + Path.GetFileName(lnkPath) + "\"";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process pinProcess = new Process();
            pinProcess.StartInfo = startInfo;
            pinProcess.Start();
            pinProcess.WaitForExit();
        }

        /// <summary>
        /// Programmatically pins a shortcut to either the start menu or taskbar.
        /// </summary>
        /// <param name="lnkPath">Full path to LNK</param>
        /// <param name="pinStartMenu">Pin to start menu, not taskbar</param>
        private void PinShortcut(string lnkPath, bool pinStartMenu)
        {
            LnkWatcher.EnableRaisingEvents = false;

            string command;
            if (pinStartMenu)
                command = "/pinStart";
            else
                command = "/pinTaskbar";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cscript.exe";
            startInfo.Arguments = "\"" + Path.Combine(Common.Path_ProgramFiles, "PinShortcut.vbs") + "\" " + command + " \"" + Path.GetDirectoryName(lnkPath) + "\" \"" + Path.GetFileName(lnkPath) + "\"";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process pinProcess = new Process();
            pinProcess.StartInfo = startInfo;
            pinProcess.Start();
            pinProcess.WaitForExit();

            if (LnkWatcher.Path.Length > 0)
                LnkWatcher.EnableRaisingEvents = true;
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
            CommonLog("Assigning AppIds to User Pinned shortcuts", 1);

            // First, populate shortcutList with all shortcuts we want to check
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

            CommonLog("Detected new shortcut: " + e.FullPath, 1);

            // Assign appid to last created shortcut
            AssignAppIdsToShortcuts(e.FullPath);

            CommonLog("Finished handling new shortcut.", 0);
        }
        #endregion
    }
}
