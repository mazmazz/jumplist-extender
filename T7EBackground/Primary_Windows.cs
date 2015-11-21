using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T7ECommon;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace T7EBackground
{
    public partial class Primary
    {
        #region Window Handling
        public bool Window_EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            // Is hWnd an actual window? Otherwise, you're getting 400 results
            if (!Interop.IsWindowVisible(hWnd)) return true;
            else if (Interop.GetWindowTextLength(hWnd) == 0) return true;

            Dictionary<string, string>[] appPairsList = new Dictionary<string, string>[2];

            if (Window_AppPairsChangesList[0].Count > 0 || Window_AppPairsChangesList[1].Count > 0)
                appPairsList = Window_AppPairsChangesList;
            else
            {
                Dictionary<string, string> appPairs = new Dictionary<string, string>();
                for (int i = 0; i < Common.AppCount; i++)
                    appPairs.Add(Common.AppProcessNames[i], Common.AppIds[i]);
                appPairsList[0] = appPairs;
                appPairsList[1] = new Dictionary<string, string>();
            }

            AssignAppIdsToWindow(hWnd, appPairsList);
            return true;
        }

        /// <summary>
        /// Checks target window against new AppIds
        /// </summary>
        /// <param name="hWnd">Handle to target window</param>
        /// <param name="changesList">App changes list</param>
        /// <param name="hwndProcessName">Process name of target window</param>
        private void CheckWindowAddedAppIds(IntPtr hWnd, Dictionary<string, string>[] changesList, string hwndProcessName)
        {
            // Check for newly added appIds, or all appIds
            // Populate compareList with every appId we want to check
            Dictionary<string, string> compareList;
            if (changesList[0].Count > 0)
                compareList = changesList[0];
            else
            {
                CommonLog("I have nothing to compare for your open windows.", -1);
                return;
            }

            CommonLog("Checking window for " + compareList.Count.ToString() + " AppIds.", 1);

            // For every AppEntry, check if the AppId matches the window.
            foreach (KeyValuePair<string, string> appPair in compareList)
            {
                string appProcessName = appPair.Key;
                string appId = appPair.Value;

                CommonLog("Checking window against " + Common.GetLogAppString(appProcessName, appId), 1);

                // Does target window match the AppId we're comparing?
                if (hwndProcessName.Equals(appProcessName, StringComparison.OrdinalIgnoreCase)) // We found match!
                {
                    CommonLog("Window matched!");

                    // Get window's original AppId. If it's original, store a backup.
                    string hwndAppId = Common.TaskbarManagerInstance.GetApplicationIdForWindow(hWnd);

                    if (hwndAppId.Length > 3 && hwndAppId.Substring(0, 3).Equals("T7E") == false)
                    {
                        CommonLog("Window has original AppId: " + hwndAppId);
                        CommonLog("Storing original AppId.");

                        // Original appid exists. Store it
                        string origHwndPath = Path.Combine(Common.Path_AppData, "OrigProperties\\" + appId + ".txt");
                        TextWriter tw = new StreamWriter(origHwndPath, false);
                        tw.Write(hwndAppId);
                        tw.Close();
                    }

                    // Apply T7E AppId to target window!
                    CommonLog("Applying AppId");
                    Common.TaskbarManagerInstance.SetApplicationIdForSpecificWindow(hWnd, appId);

                    // Win10 RTM: Pin shortcut here if no shortcut exists
                    // Doesn't work
                    //if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build < 10586)
                    //    PinTempShortcutFromHwnd(hWnd, appId);

                    CommonLog("Done checking window!", -1);

                    break; // Since a window belongs to only one appId
                }
                else
                    CommonLog(-1);
            }
        }

        /// <summary>
        /// Checks target window against deleted AppIds
        /// </summary>
        /// <param name="hWnd">Handle to target window</param>
        /// <param name="changesList">AppChangesList</param>
        /// <param name="hwndProcessName">Process name of target window</param>
        private void CheckWindowDeletedAppIds(IntPtr hWnd, Dictionary<string,string>[] changesList, string hwndProcessName) 
        {
            if (changesList[1].Count > 0)
            {
                // Get deletedList from changesList
                Dictionary<string, string> deletedList = changesList[1];

                CommonLog("Checking window for " + deletedList.Count.ToString() + " deleted AppIds.", 1);

                foreach (KeyValuePair<string, string> deletedAppPair in deletedList)
                {
                    string appProcessName = deletedAppPair.Key;
                    string appId = deletedAppPair.Value;

                    // Does target window have the deleted appId? Remove it.
                    CommonLog("Checking window against " + Common.GetLogAppString(appProcessName, appId), 1);
                    if (hwndProcessName.Equals(appProcessName, StringComparison.OrdinalIgnoreCase)) // We found match!
                    {
                        CommonLog("Window matched!");

                        string origAppIdPath = Path.Combine(Common.Path_AppData, "OrigProperties\\" + appId + ".txt");
                        string origAppId;

                        // Get original appid, if it exists
                        if (File.Exists(origAppIdPath))
                        {
                            StreamReader origAppIdStream = new System.IO.StreamReader(origAppIdPath);
                            origAppId = origAppIdStream.ReadToEnd();
                            origAppIdStream.Close();

                            File.Delete(origAppIdPath);

                            CommonLog("Original AppId exists: " + origAppIdPath);
                        }
                        else
                        {
                            // If it doesn't exist, just remove appid
                            origAppId = "";
                            CommonLog("Original AppId does not exist; making blank.");
                        }

                        CommonLog("Setting original AppId to window.");
                        Common.TaskbarManagerInstance.SetApplicationIdForSpecificWindow(hWnd, origAppId);
                        Interop.ShowWindow(hWnd, 0);
                        //Thread.Sleep(250);
                        Interop.ShowWindow(hWnd, 5);

                        // Win10 RTM: Unpin shortcut here
                        // doesn't work
                        //if(Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build < 10586)
                        //    UnpinTempShortcut(hWnd, origAppId);

                        CommonLog("Finished checking AppId.", -1);

                        break; // Since a window belongs to only one appId
                    }
                    else
                    {
                        CommonLog(-1);
                    }
                }

                CommonLog("Finished checking for deleted AppIds.", -1);
            }
        }

        #region AssignAppIdsToWindows()
        /// <summary>
        /// Assign AppIds to all windows, checking against master AppList
        /// </summary>
        private void AssignAppIdsToWindows()
        {
            // Enumerate all windows
            CommonLog("Assigning AppIds to all windows.", 1);
            Interop.EnumWindows(Window_EnumWindowsCallback, IntPtr.Zero);
            CommonLog("Done assigning AppIds to windows.", -1);
        }

        /// <summary>
        /// Checks single window against the master AppList
        /// </summary>
        /// <param name="hwnd">Handle of target window</param>
        private void AssignAppIdsToWindows(IntPtr hwnd)
        {
            Dictionary<string, string>[] appPairsList = new Dictionary<string, string>[2];
            appPairsList[0] = new Dictionary<string, string>();
            appPairsList[1] = new Dictionary<string, string>();
            AssignAppIdsToWindows(hwnd, appPairsList);
        }

        /// <summary>
        /// Checks single window against a defined AppChangesList
        /// </summary>
        /// <param name="hwnd">Handle of target window</param>
        /// <param name="changesList">A two-part dictionary array consisting of added and deleted AppIds</param>
        private void AssignAppIdsToWindows(IntPtr hwnd, Dictionary<string, string>[] changesList)
        {
            Dictionary<string, string>[] appPairsList = new Dictionary<string, string>[2];

            if (changesList[0].Count > 0 || changesList[1].Count > 0)
                appPairsList = changesList;
            else
            {
                Dictionary<string, string> appPairs = new Dictionary<string, string>();
                for (int i = 0; i < Common.AppCount; i++)
                    appPairs.Add(Common.AppProcessNames[i], Common.AppIds[i]);
                appPairsList[0] = appPairs;
                appPairsList[1] = new Dictionary<string, string>();
            }

            if (hwnd.ToInt32() > 0)
                AssignAppIdsToWindow(hwnd, appPairsList);
            else
            {
                Window_AppPairsChangesList = changesList;
                CommonLog("Assigning AppIds to all windows.", 1);
                Interop.EnumWindows(Window_EnumWindowsCallback, IntPtr.Zero);
                CommonLog("Done assigning AppIds to windows.", -1);
                Window_AppPairsChangesList = new Dictionary<string, string>[2];
                Window_AppPairsChangesList[0] = new Dictionary<string, string>();
                Window_AppPairsChangesList[1] = new Dictionary<string, string>();
            }
        }

        public string GetWindowProcessPath(IntPtr hWnd)
        {
            // Get process ID from window
            int hwndPid;
            Interop.GetWindowThreadProcessId(hWnd, out hwndPid);

            StringBuilder hwndProcessNameString = new StringBuilder(Interop.MAX_PATH);
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                processHandle = Interop.OpenProcess(Interop.ProcessAccess.QueryInformationLimited, false, hwndPid);
                uint pathLength = 260;
                Interop.QueryFullProcessImageName(processHandle, 0, hwndProcessNameString, ref pathLength);
                Interop.CloseHandle(processHandle);

                return hwndProcessNameString.ToString();
            }
            catch (Exception e) { Interop.CloseHandle(processHandle); return ""; }
        }

        /// <summary>
        /// The main procedure for assigning AppId to window. Takes one hWnd and checks it against changesList
        /// </summary>
        /// <param name="hWnd">Handle to target window</param>
        /// <param name="changesList">A two-part dictionary array consisting of added and deleted AppIds</param>
        public void AssignAppIdsToWindow(IntPtr hWnd, Dictionary<string, string>[] changesList)
        {
            // Get process ID from window
            int hwndPid;
            Interop.GetWindowThreadProcessId(hWnd, out hwndPid);

            // Get process handle from process ID, and gets its program path.
            StringBuilder hwndProcessNameString = new StringBuilder(Interop.MAX_PATH);
            try
            {
                IntPtr processHandle = Interop.OpenProcess(Interop.ProcessAccess.QueryInformationLimited, false, hwndPid);
                uint pathLength = 260;
                Interop.QueryFullProcessImageName(processHandle, 0, hwndProcessNameString, ref pathLength);
                Interop.CloseHandle(processHandle);
            }
            catch (Exception e) { return; }

            // Get process name from the path, without .EXE.
            // T7E stores process names without the extension, for some reason I forget.
            string hwndProcessName;
            try { hwndProcessName = Path.GetFileNameWithoutExtension(hwndProcessNameString.ToString()).ToLower(); }
            catch (Exception e) { return; }

            CommonLog("Assigning AppIds to window " + hwndProcessName, 1);

            // Check window against deleted AppIds
            CheckWindowDeletedAppIds(hWnd, changesList, hwndProcessName);

            // Then check it against new AppIds
            CheckWindowAddedAppIds(hWnd, changesList, hwndProcessName);

            CommonLog("Finished assigning AppIds to window.", -2);
        }
        #endregion

        /// <summary>
        /// Detects WM_ShellHook when a window is created and assigns the appropriate AppId.
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_ShellHook)
            {
                if ((int)m.WParam == 1) // HSHELL_WINDOWCREATED
                {
                    CommonLog("Detected new window.", 1);

                    // Do I want this to be multithreaded? Possible limitation: AssignAppIdsToWindows
                    // creates a new AppWindow. Can I cancel the AssignAppIdsToWindows thread???
                    //Thread matchWindowThread = new Thread(new ParameterizedThreadStart(AssignAppIdsToWindows));
                    if (System.Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Minor < 10586)
                    {
                        //if (IsAppPinnedByProcessName(Path.GetFileNameWithoutExtension(GetWindowProcessPath(m.LParam))))
                            AssignAppIdsToWindows(m.LParam);
                    }
                    else
                    {
                        AssignAppIdsToWindows(m.LParam);
                    }

                    // Win10 RTM: If window does not have a pinned shortcut, then pin it. Monitor for window closing then delete the shortcut
                    // The temp pinning is a workaround as jump lists do not work for unpinned shortcuts (the list is displayed, but actions don't do anything.
                    // This workaround is unnecessary in TH2 (build 10586) as the OS bug is fixed.

                    CommonLog("Finished handling created window.", 0);
                }
                else if((int)m.WParam == 2) // HSHELL_WINDOWDESTROYED
                {
                    // Win10 RTM: Unpin shortcut when windows is closed
                    // doesn't work
                    //System.OperatingSystem osInfo = System.Environment.OSVersion;
                    //if (osInfo.Version.Major == 10 && (osInfo.Version.Build < 10586))
                    //    UnpinTempShortcut(m.LParam, "");
                }
            }

            base.WndProc(ref m);
        }
        #endregion
    }
}
