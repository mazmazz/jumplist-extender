using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using T7ECommon;
using System.Diagnostics;

namespace T7EPreferences
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            //Loading loadingForm = new Loading("Starting...", true, true);
            //loadingForm.Show();

            // Are we running on Win7 or later? If we're not, exit.
            Common.CheckWindowsVersion();

            // Check install dir
            Common.CheckEnvInstalled();

            // Do file/directory checks
            Common.CheckFiles();

            // Check time-lock
            Common.CheckTimeLock();

            // Check registration codes
            Common.CheckRegistrationCodes();

            // Run T7EBackground, if not running. And not installed.
            if (Process.GetProcessesByName("T7EBackground").Length <= 0)
            {
                try
                {
                    Process.Start(Path.Combine(Common.Path_ProgramFiles, "T7EBackground.exe"));
                }
                catch (Exception e) { }
            }

#if DEBUG
            Application.EnableVisualStyles();
            Application.Run(new Primary());
            return;
#endif
//IF RELEASE
            try
            {
                Application.EnableVisualStyles();
                Application.Run(new Primary());
            }
            catch (Exception e)
            {
                Common.SendExceptionLog(e, Primary._CurrentAppName, Primary._CurrentAppPath);

                Environment.Exit(-1);
            }
//ENDIF
        }

        
    }
}
