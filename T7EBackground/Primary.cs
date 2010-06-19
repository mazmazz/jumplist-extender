using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Taskbar;
using Microsoft.WindowsAPICodePack.Shell;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Threading;
using T7ECommon;
using System.Reflection;
using System.Net;

namespace T7EBackground
{
    public partial class Primary : Form
    {
        FileSystemWatcher LnkWatcher;
        FileSystemWatcher AppListWatcher;
        FileSystemWatcher UpdateWatcher;
        static uint WM_ShellHook;

        string UpdatePath = "";

        Icon PrimaryIcon;

        Dictionary<string, string>[] Window_AppPairsChangesList = new Dictionary<string, string>[2];

        static TextWriter LogTxtFile;

        #region Start-Up
        /// <summary>
        /// Hook Path_AppData\AppList.xml to look for LastWrite changes
        /// </summary>
        private void HookAppList()
        {
            CommonLog("Hooking " + Common.Path_AppData + "\\AppList.xml" + " for LastWrite");
            AppListWatcher.Path = Common.Path_AppData;
            AppListWatcher.Filter = "AppList.xml";
            AppListWatcher.NotifyFilter = NotifyFilters.LastWrite;
            AppListWatcher.Changed += new FileSystemEventHandler(AppListWatcher_Changed);
            AppListWatcher.EnableRaisingEvents = true;
            CommonLog("Done hooking AppList.");
        }

        /// <summary>
        /// Hook AppData\Microsoft\Internet Explorer\Quick Launch\User Pinned to look for file creation. When a LNK is created, T7EBg assigns the appropriate AppId.
        /// </summary>
        private void HookShortcut()
        {
            
            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned");

            CommonLog("Hooking " + basePath + " for DirectoryName and FileName changes");

            LnkWatcher.Path = basePath;
            LnkWatcher.IncludeSubdirectories = true;
            LnkWatcher.Filter = "*.lnk";
            LnkWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName;
            LnkWatcher.Created += new FileSystemEventHandler(lnkWatcher_Created);
            LnkWatcher.EnableRaisingEvents = true;

            CommonLog("Registered shortcut hook on " + basePath);
        }

        /// <summary>
        /// Hooks window creation to listen for appropriate windows that need AppIds.
        /// </summary>
        private static void HookWindows()
        {
            CommonLog("Attempting to install window hook");

            if (Interop.RegisterShellHookWindow(Common.WindowHandle_Primary))
            {
                WM_ShellHook = Interop.RegisterWindowMessage("SHELLHOOK");

                Common.Debug_ShowMessageBox = false;
                CommonLog("Registered window hook.");
            }
            else CommonLog("Failed to register window hook.");
        }

        /// <summary>
        /// Hook Path_AppData\UpdateCheck2.txt to look for LastWrite changes
        /// </summary>
        private void HookUpdate()
        {
            CommonLog("Hooking " + Common.Path_AppData + "\\UpdateCheck2.txt for LastWrite changes");

            UpdateWatcher.Path = Common.Path_AppData;
            UpdateWatcher.Filter = "UpdateCheck2.txt";
            UpdateWatcher.NotifyFilter = NotifyFilters.LastWrite;
            UpdateWatcher.Changed += new FileSystemEventHandler(UpdateWatcher_Changed);
            UpdateWatcher.EnableRaisingEvents = true;

            CommonLog("Hook successful");
        }

        private void StartUpdateTimer()
        {
            CommonLog("Starting update timer");
            UpdateTimer.Start();
        }

        /// <summary>
        /// Initializes T7EBackground.
        /// </summary>
        public Primary()
        {
            InitializeComponent();
            PrimaryIcon = new Icon(Resource1.PrimaryIcon, 256, 256);
            this.Icon = PrimaryIcon;
            TrayIcon.Icon = PrimaryIcon;
            MessageBox.Show("fucik");
            LogTxtFile = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "T7EBackgroundLog_"+DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString()+DateTime.Now.Second.ToString()+".txt"));
            MessageBox.Show("fidld");
            CommonLog("Starting T7EBackground Primary Form", 1);
            
            // Initialize variables
            Common.WindowHandle_Primary = this.Handle;

            // Sets handler for Ctrl+C on console
            Interop.SetConsoleCtrlHandler(new Interop.ConsoleCtrlDelegate(ConsoleCtrlCheck), true);

            // Initializes common AppPairsChangesList, for Window handling to refer from
            // since, because I use Interop.EnumWindows() to cycle through each open
            // window, I need to store the changesList commonly, so EnumWindows can refer to it.
            Window_AppPairsChangesList = new Dictionary<string, string>[2];
            Window_AppPairsChangesList[0] = new Dictionary<string, string>(); // AppPairs[0] is an "added appIds" list
            Window_AppPairsChangesList[1] = new Dictionary<string, string>(); // AppPairs[1] is a "deleted appIds" list

            // Set up AppList and LNK watchers
            LnkWatcher = new FileSystemWatcher();
            AppListWatcher = new FileSystemWatcher();

            // Read AppList.xml and do initial assigning of appids
            CommonLog("Reading AppList.xml, and assigning AppIds and Windows");
            ReadAppList();
            AssignAppIdsToShortcuts(); // Shortcuts _BEFORE_ windows
            AssignAppIdsToWindows();
            CommonLog("Done assigning AppIds and Windows");

            // Hook shortcuts, windows, and AppList.xml to detect further changes.
            CommonLog("Starting hooks");
            HookShortcut();
            HookWindows();
            HookAppList();
            CommonLog("Finished starting hooks");

            // Hook the update timer -- every 24 hours
            CommonLog("Starting update check sequence");
            CheckUpdateString();
            StartUpdateTimer();
            CommonLog("Finished update check sequence");

            CommonLog("Finished Primary Form start-up.", 0);
        }

        /// <summary>
        /// When Primary form loads, hide the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Primary_Load(object sender, EventArgs e)
        {
            // Hide the window when finished loading.
            Hide();

            // And set the process priority to "Low"
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Idle;
        }

        /// <summary>
        /// When Primary form is minimized, hide the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Primary_Resize(object sender, System.EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                Hide();
        }
        #endregion

        #region Tray Icon
        /// <summary>
        /// When double-clicking the tray icon, run T7EPreferences.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Common.Path_ProgramFiles, "T7EPreferences.exe"));
        }

        /// <summary>
        /// Call the Close(); method to initiate cleanup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrayIconContextMenuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// When clicking "Open Settings," run T7EPreferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrayIconContextMenuOpenSettings_Click(object sender, EventArgs e)
        {
            Process.Start(Path.Combine(Common.Path_ProgramFiles, "T7EPreferences.exe"));
        }

        #region CheckUpdateString()
        /// <summary>
        /// Check Path_AppData\UpdateCheck2.txt for new updates and download if needed. If any are found, set a tray balloon.
        /// </summary>
        private void CheckUpdateString()
        {
            CheckUpdateString(true);
        }

        /// <summary>
        /// Check Path_AppData\UpdateCheck2.txt for new updates. If any are found, set a tray balloon.
        /// </summary>
        /// <param name="download">Download UpdateCheck2.txt from the web server</param>
        private void CheckUpdateString(bool download)
        {
            // Download the file, first. This is on a new thread; it won't affect nothing(?)
            if (download)
            {
                if ((File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))
                    && File.GetCreationTime(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")).Day - DateTime.Now.Day <= -1)
                    || !File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")))
                {
                    try { new WebClient().DownloadFile("http://jumplist.gsdn-media.com/UpdateCheck2.txt", Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")); }
                    catch (Exception ee) { /* Fail silently. */ }
                }
            }

            if (!File.Exists(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt"))) return;

            try
            {
                string[] versionCheckParts = File.ReadAllText(Path.Combine(Common.Path_AppData, "UpdateCheck2.txt")).Split('|');
                if (versionCheckParts == null || versionCheckParts.Length < 2) return;

                if (Assembly.LoadFile(
                    Path.Combine(Common.Path_ProgramFiles, "T7EPreferences.exe")
                    ).GetName().Version.ToString().Equals(versionCheckParts[0])) return;
                else
                {
                    // If UpdateCheck has 0.x.x.x|http://link|0.x-B, get the third slice
                    // else, substr from the first version string.
                    updateToVersion0ToolStripMenuItem.Text = String.Format(updateToVersion0ToolStripMenuItem.Text,
                        versionCheckParts.Length >= 3 ? versionCheckParts[2]
                        : versionCheckParts[0].Substring(0, 3));
                    updateToVersion0ToolStripMenuItem.Enabled = updateToVersion0ToolStripMenuItem.Visible = true;

                    TrayIcon.BalloonTipIcon = ToolTipIcon.Info;
                    TrayIcon.BalloonTipTitle = String.Format("Jumplist Extender Version {0} is now available", 
                        versionCheckParts.Length >= 3 ? versionCheckParts[2]
                        : versionCheckParts[0].Substring(0, 3));
                    TrayIcon.BalloonTipText = "Click to download.";
                    UpdatePath = versionCheckParts[1];
                    TrayIcon.ShowBalloonTip(1000); // Show for 10 seconds; msdn default
                }
            }
            catch (Exception e) { /*MessageBox.Show(e.ToString());*/ /* Fail silently */ }
        }
        #endregion

        private void TrayIcon_BalloonTipClicked_Update(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", UpdatePath);
        }

        private void updateToVersion0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", UpdatePath);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            // Check UpdateCheck2.txt
            CheckUpdateString();
        }

        private void UpdateWatcher_Changed(object sender, EventArgs e)
        {
            // Check UpdateCheck2.txt, newly written
            CheckUpdateString(false);
        }
        #endregion

        #region Cleanup
        /// <summary>
        /// Unhook the window shell hook created previously
        /// </summary>
        static public void UnhookWindows()
        {
            CommonLog("1. Unhook the window shell hook");
            Interop.RegisterShellHook(Common.WindowHandle_Primary, 0);
            CommonLog("1B. Done unhooking shell hook");
        }

        /// <summary>
        /// Dispose the AppList.xml watcher
        /// </summary>
        private void UnhookAppList()
        {
            CommonLog("2. Unhook the AppList.xml watcher.");
            AppListWatcher.EnableRaisingEvents = false;
            AppListWatcher.Dispose();
            CommonLog("2B. Done unhooking AppList.xml watcher.");
        }

        /// <summary>
        /// Dispose the LNK watcher
        /// </summary>
        private void UnhookShortcut()
        {
            CommonLog("3. Unhook LNK watcher");
            LnkWatcher.EnableRaisingEvents = false;
            LnkWatcher.Dispose();
            CommonLog("3B. Done unhooking LNK watcher");
        }

        /// <summary>
        /// Handle clean-up when pressing Ctrl+C
        /// </summary>
        private static void CleanUp_Ctrl()
        {
            //CommonLog("Closing app via cancel; cleaning up...", 1);
            UnhookWindows();
            //CommonLog("Finished cleaning up; exiting app.", 0);
        }

        /// <summary>
        /// Main clean-up procedure; unhooks windows, applist, shortcut. Optionally removes jumplists if pref is specified.
        /// </summary>
        private void CleanUp()
        {
            CommonLog("Closing app; cleaning up...", 1);

            UnhookWindows();
            UnhookAppList();

            bool cleanAppIds = false;
            if (Common.PrefExists("CleanAppIdsOnExit"))
                bool.TryParse(Common.ReadPref("CleanAppIdsOnExit"), out cleanAppIds);

            CommonLog("Checked cleanAppIds = " + cleanAppIds.ToString());
            if (cleanAppIds)
            {
                CommonLog("4. Cleaning AppIds from Shortcuts and Windows");
                // Remove jumplist from windows and shortcuts. Just pass the applist
                // as the deletedlist
                Dictionary<string, string>[] appPairsList = new Dictionary<string, string>[2];
                Dictionary<string, string> appPairs = new Dictionary<string, string>();
                // Key: Processname; Value: AppId
                for (int i = 0; i < Common.AppCount; i++)
                    appPairs.Add(Common.AppProcessNames[i], Common.AppIds[i]);
                appPairsList[0] = new Dictionary<string, string>();
                appPairsList[1] = appPairs;

                // Pass deleted appPairs to shortcuts and windows
                AssignAppIdsToShortcuts("", appPairsList);
                AssignAppIdsToWindows(IntPtr.Zero, appPairsList);
                CommonLog("4B. Done cleaning AppIds from Shortcuts and Windows");
            }

            UnhookShortcut();

            CommonLog("Finished cleaning up; exiting app.", 0);
        }

        /// <summary>
        /// Form closing initiates clean-up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Primary_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommonLog("Received cleanup signal from Primary_FormClosing: Starting cleanup");
            CleanUp();
            CommonLog("Finished cleanup; passing to FormClosing to close app");
            LogTxtFile.Close();

        }

        /// <summary>
        /// Ctrl+C initiates simplified clean-up -- only unhooks shell hook.
        /// </summary>
        /// <param name="ctrlType"></param>
        /// <returns></returns>
        private static bool ConsoleCtrlCheck(Interop.CtrlTypes ctrlType)
        {
            CommonLog("Cleanup initiated through ConsoleCtrlCheck");
            CleanUp_Ctrl();
            CommonLog("Done with ConsoleCtrlCheck cleanup.");
            
            return false;
        }
        #endregion

        private void visitTheOfficialWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "http://jumplist.gsdn-media.com");
        }

        #region Logging

        public string GetLogAppString(string processName, string appId)
        {
            return processName + " | " + appId;
        }

        static public int LogIndentLevel = 0;

        static public void CommonLog(string messageString)
        {
            CommonLog(messageString, -32767, false);
        }

        static public void CommonLog(int logChange)
        {
            CommonLog("", logChange, false);
        }

        static public void CommonLog(string messageString, int logChange)
        {
            CommonLog(messageString, logChange, false);
        }

        static public void CommonLog(string messageString, int logChange, bool followingLine)
        {
#if RELEASE
            return;
#endif
            if (messageString.Length > 0)
            {
                for (int i = 0; i < LogIndentLevel * 4; i++) Console.Write(" ");
                LogTxtFile.Write(messageString + Environment.NewLine);
                if (followingLine) Console.Write(Environment.NewLine);
            }

            if (logChange != 0 && logChange != -32767)
            {
                LogIndentLevel = LogIndentLevel + logChange;
                if (LogIndentLevel < 0) LogIndentLevel = 0;
            }
            if (logChange == 0)
            {
                LogIndentLevel = 0;
                LogTxtFile.Write(Environment.NewLine);
            }
        }
        #endregion
    }
}
