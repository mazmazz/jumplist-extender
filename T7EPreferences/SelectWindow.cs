using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Runtime.InteropServices;
using T7ECommon;

namespace T7EPreferences
{
    public partial class SelectWindow : Form
    {
        private static string CurrentAppPath;
        private string CurrentAppName;

        public static string CapturedAppName;
        public static string CapturedAppPath;
        public static string CapturedAppClass;

        private static Interop.MouseHook.LowLevelMouseProc MouseHook = HookCallback;
        private static IntPtr HookId = IntPtr.Zero;

        static SelectWindow DialogInstance;

        public SelectWindow(string appPath, string appName, bool firstTime)
        {
            InitializeComponent();
            DialogInstance = this;
            this.Icon = Primary.PrimaryIcon;

            CurrentAppPath = appPath;
            CurrentAppName = appName;

            MainLabel.Text = String.Format(MainLabel.Text, appName.Length > 0 ? appName : Path.GetFileNameWithoutExtension(appName));
            RunButton.Text = String.Format(RunButton.Text, appName.Length > 0 ? appName : Path.GetFileNameWithoutExtension(appName));
            SelectedWindowLabel.Text = String.Format("If {0} is not running, click on the button above "
                + "to start it.", appName.Length > 0 ? appName : Path.GetFileNameWithoutExtension(appName));

            try
            {
                Process[] currentProcesses = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(CurrentAppPath));
                if (currentProcesses.Length > 0)
                    Interop.SetForegroundWindow(currentProcesses[0].MainWindowHandle);
            }
            catch (Exception e) { }

            if (firstTime)
                CancelButton.Text = "Skip";
            else
                CancelButton.Text = "Cancel";
        }

        private static IntPtr SetHook(Interop.MouseHook.LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return Interop.MouseHook.SetWindowsHookEx(Interop.MouseHook.WH_MOUSE_LL, proc,
                    Interop.GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(
        int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 &&
                Interop.MouseHook.MouseMessages.WM_LBUTTONDOWN == (Interop.MouseHook.MouseMessages)wParam)
            {
                Interop.MouseHook.MSLLHOOKSTRUCT hookStruct = (Interop.MouseHook.MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Interop.MouseHook.MSLLHOOKSTRUCT));
                
                // Get window under point
                IntPtr capturedWindow = Interop.WindowFromPoint(hookStruct.pt);
                // First, check if the process is ours.
                uint processId;
                Interop.GetWindowThreadProcessId(capturedWindow, out processId);
                if (processId == Process.GetCurrentProcess().Id) return Interop.CallNextHookEx(HookId, nCode, wParam, lParam);
                
                // Get top-level window
                IntPtr parentWindow = capturedWindow;
                while(parentWindow != IntPtr.Zero) {
                    capturedWindow = parentWindow;
                    parentWindow = Interop.GetParent(parentWindow);
                }

                // Get window's text, class, and path
                // class
                StringBuilder sbClass = new StringBuilder(100);
                try { Interop.GetClassName(capturedWindow, sbClass, 100); }
                catch (Exception e) { }

                // path
                string tempPath = "";
                StringBuilder sbPath = new StringBuilder(Interop.MAX_PATH);
                try
                {
                    IntPtr processHandle = Interop.OpenProcess(Interop.ProcessAccess.QueryInformationLimited, false, (int)processId);
                    uint pathLength = 260;
                    Interop.QueryFullProcessImageName(processHandle, 0, sbPath, ref pathLength);
                    tempPath = sbPath.ToString();
                    Interop.CloseHandle(processHandle);
                }
                catch (Exception e) { }
                /*try { tempPath = Process.GetProcessById((int)processId).MainModule.FileName; }
                catch (Exception e) { }*/

                // name
                int textLength = Interop.GetWindowTextLength(capturedWindow);
                StringBuilder sbText = new StringBuilder(textLength + 1);
                try { Interop.GetWindowText(capturedWindow, sbText, sbText.Capacity); }
                catch (Exception e) { }

                // Blacklist dialogs, the shell
                if (sbClass.ToString().Length <= 0 ||
                    sbClass.ToString()[0].Equals('#')) 
                    return Interop.CallNextHookEx(HookId, nCode, wParam, lParam);
                if ((Path.GetFileName(tempPath).Equals("explorer.exe", StringComparison.OrdinalIgnoreCase)
                        && !sbClass.ToString().Equals("CabinetWClass")) // file manager window
                    || Path.GetFileName(tempPath).Equals("rundll32.exe", StringComparison.OrdinalIgnoreCase))
                    return Interop.CallNextHookEx(HookId, nCode, wParam, lParam);

                CapturedAppClass = sbClass.ToString();
                CapturedAppPath = tempPath;
                CapturedAppName = sbText.ToString();
                DialogInstance.OkButton.Enabled = true;

                // If paths match, go ahead and just send the dialogresult
                if (CapturedAppPath.Equals(CurrentAppPath, StringComparison.OrdinalIgnoreCase))
                {
                    DialogInstance.OkButton.PerformClick();
                }
                else
                {
                    // Update label
                    DialogInstance.SelectedWindowLabel.Text =
                        "Selected Window: " + CapturedAppName;
                    DialogInstance.SelectedWindowLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
            return Interop.CallNextHookEx(HookId, nCode, wParam, lParam);
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

            if (SelectedWindowLabel.Text.Length <= 0) SelectedWindowLabel.Text = "Click on the main program window when it finishes loading.";
        }

        private void SelectWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Interop.UnhookWindowsHookEx(HookId);
        }

        private void SelectWindow_Shown(object sender, EventArgs e)
        {
            Interop.SetForegroundWindow(Handle);
            HookId = SetHook(MouseHook);
            this.Shown -= SelectWindow_Shown;
        }
    }
}
