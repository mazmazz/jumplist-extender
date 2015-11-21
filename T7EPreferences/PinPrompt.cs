using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using T7ECommon;

namespace T7EPreferences
{
    public partial class PinPrompt : Form
    {
        FileSystemWatcher LnkWatcher;

        public PinPrompt(bool unpinPrompt, string lnkPath, string appName, string appPath, string appId)
        {
            PinPromptInitialize(unpinPrompt, lnkPath, appName, appPath, appId, false);
        }        

        public PinPrompt(bool unpinPrompt, string lnkPath, string appName, string appPath, string appId, bool pinRequired)
        {
            PinPromptInitialize(unpinPrompt, lnkPath, appName, appPath, appId, pinRequired);
        }

        private void PinPromptInitialize(bool unpinPrompt, string lnkPath, string appName, string appPath, string appId, bool pinRequired)
        {
            InitializeComponent();
            this.Icon = Primary.PrimaryIcon;

            PromptUnpin = unpinPrompt;
            CurrentAppId = appId;
            CurrentAppPath = appPath;
            CurrentAppName = appName;
            WatchTargetName = Path.GetFileNameWithoutExtension(appPath).ToLower();

            WatchLnkPath = lnkPath;

            SwitchPrompt(unpinPrompt, pinRequired);

            ReallyCenterToScreen();

            LnkWatcher = new FileSystemWatcher();
            HookShortcut();
        }

        bool PromptUnpin = false;
        string CurrentAppId = "";
        string CurrentAppName = "";
        string CurrentAppPath = "";
        string WatchLnkPath = "";
        string WatchTargetName = "";


        private void SwitchPrompt(bool unpinPrompt, bool pinRequired)
        {
            if (unpinPrompt)
            {
                this.Text = "Unpin the app";
                TitleLabel.Text = "Unpin the app from taskbar to continue.";
                DescLabel.Text = "The app needs to be unpinned to prevent duplicate program icons from appearing on the taskbar.";
                RunButton.Enabled = false;
                RunButtonPanel.Visible = false;
                UnpinPictureBox.Visible = true;
                PinPictureBox.Visible = false;
                TopMost = false;
                CloseButton.Text = "Cancel";
                CloseButton.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.Text = "Pin the app";                
                RunButton.Enabled = true;
                RunButtonPanel.Visible = true;
                RunButton.Text = String.Format("Run {0}", CurrentAppName);
                UnpinPictureBox.Visible = false;
                PinPictureBox.Visible = true;
                TopMost = true;
                if (pinRequired)
                {
                    TitleLabel.Text = "Run the app and pin to taskbar to finish saving.";
                    DescLabel.Text = "The app needs to be run first, then pinned by right-clicking its icon, before the jump list commands will work correctly.";
                    CloseButton.Text = "Cancel";
                    CloseButton.DialogResult = DialogResult.Cancel; // makes status text warn "you may need to pin the app"
                } else
                {
                    TitleLabel.Text = "Run the app and pin to taskbar (optional)";
                    DescLabel.Text = "You should pin the app by right-clicking its icon only while it's running, or else duplicate icons may appear in the future.";

                    bool hidePinPromptOptional = false;
                    if(Common.PrefExists("HidePinPromptOptional")) bool.TryParse(Common.ReadPref("HidePinPromptOptional"), out hidePinPromptOptional);
                    HidePromptCheckBox.Visible = true;
                    HidePromptCheckBox.Enabled = true;
                    HidePromptCheckBox.Checked = hidePinPromptOptional;

                    CloseButton.Text = "Skip";
                    CloseButton.DialogResult = DialogResult.OK;
                }

            }
        }



        protected void ReallyCenterToScreen()
        {
            Screen screen = Screen.FromControl(this);

            Rectangle workingArea = screen.WorkingArea;
            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            if (Path.GetFileName(CurrentAppPath).Equals("explorer.exe", StringComparison.OrdinalIgnoreCase))
                Process.Start(CurrentAppPath, "/e");
            else
            {
                try
                {
                    Process[] currentProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(CurrentAppPath));
                    if (currentProcesses.Length > 0)
                        Interop.SetForegroundWindow(currentProcesses[0].MainWindowHandle);
                    else
                        Process.Start(CurrentAppPath);
                }
                catch (Exception ex)
                {
                    Process.Start(CurrentAppPath);
                }
            }
        }

        /// <summary>
        /// Hook AppData\Microsoft\Internet Explorer\Quick Launch\User Pinned to look for file creation. When a LNK is created, T7EBg assigns the appropriate AppId.
        /// </summary>
        private void HookShortcut()
        {
            string basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Internet Explorer\\Quick Launch\\User Pinned");

            //CommonLog("Hooking " + basePath + " for DirectoryName and FileName changes");

            LnkWatcher.Path = basePath;
            LnkWatcher.IncludeSubdirectories = true;
            LnkWatcher.Filter = "*.lnk";
            LnkWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName;
            LnkWatcher.Created += new FileSystemEventHandler(lnkWatcher_Created);
            LnkWatcher.Deleted += new FileSystemEventHandler(LnkWatcher_Deleted);
            LnkWatcher.EnableRaisingEvents = true;

            //CommonLog("Registered shortcut hook on " + basePath);
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

            //CommonLog("Detected new shortcut: " + e.FullPath, 1);

            // Assign appid to last created shortcut
            if (!PromptUnpin)
            {
                if (Path.GetFileNameWithoutExtension(Common.ResolveLnkPath(e.FullPath)).ToLower() == WatchTargetName)
                {
                    CloseFromThread(DialogResult.OK);
                    //Close();
                    //DialogResult = DialogResult.OK;
                }
            }

            //CommonLog("Finished handling new shortcut.", 0);
        }


        private void LnkWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (PromptUnpin) {
                if (Path.GetFullPath(e.FullPath).Equals(Path.GetFullPath(WatchLnkPath)))
                {
                    CloseFromThread(DialogResult.OK);
                    //Close();
                    //DialogResult = DialogResult.OK;
                }
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
            if (CloseButton.Text == "Skip")
                DialogResult = DialogResult.OK;
            else
                DialogResult = DialogResult.Cancel;
        }

        private void PinPrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            LnkWatcher.EnableRaisingEvents = false;
            LnkWatcher.Dispose();

            if(HidePromptCheckBox.Enabled)
            {
                Common.WritePref("HidePinPromptOptional", HidePromptCheckBox.Checked.ToString());
            }
        }

        private void CloseFromThread(DialogResult result)
        {
            if (this.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                this.BeginInvoke(new MethodInvoker(() => CloseFromThread(result)));
            }
            else
            {
                // It's on the same thread, no need for Invoke
                Close();
                DialogResult = result;
            }
        }
    }
}
