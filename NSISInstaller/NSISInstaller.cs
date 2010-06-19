using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Xml;
using T7ECommon;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NSISInstaller
{
    class NSISInstaller
    {
        static public void Main(string[] args)
        {
            Common.CheckWindowsVersion();

            // Check install dir
            Common.EnvInstalled = true;

            // Do file/directory checks
            Common.Path_ProgramFiles = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Common.Path_AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JumplistExtender");

            foreach (Process T7EPreferencesProcess in Process.GetProcessesByName("T7EPreferences"))
            {
                T7EPreferencesProcess.Kill();
            }

            foreach(Process T7EBackgroundProcess in Process.GetProcessesByName("T7EBackground")) {
                T7EBackgroundProcess.Kill();
            }

            // Read the applist
            Application.Run(new Primary());
        }

        


    }
}
