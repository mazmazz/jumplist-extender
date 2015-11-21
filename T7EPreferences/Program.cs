using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using T7ECommon;
using System.Diagnostics;
using System.Threading;

namespace T7EPreferences
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{467D53F9-24D0-47BA-83F2-214A6791C451}");

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

            //#if DEBUG
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.Run(new Primary());
                mutex.ReleaseMutex();
            } else
            {
                Process currentProcess = Process.GetCurrentProcess();
                var runningProcess = (from process in Process.GetProcesses()
                                      where
                                        process.Id != currentProcess.Id &&
                                        process.ProcessName.Equals(
                                          currentProcess.ProcessName,
                                          StringComparison.Ordinal)
                                      select process).FirstOrDefault();
                if (runningProcess != null)
                {
                    Interop.SetForegroundWindow(runningProcess.MainWindowHandle);
                }
            }
                //#endif
                /*
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
                */
        }
    }
}
