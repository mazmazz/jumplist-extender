using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using T7ECommon;
using System.Threading;
using System.Diagnostics;

namespace T7EBackground
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{7124FFA4-A77D-415A-A102-B019BA4756F7}");
        public static bool CleaningUp = false;

        static void Main()
        {
            // Do file/directory checks
            // Are we running on Win7 or later? If we're not, exit.
            Common.CheckWindowsVersion();

            // Check install dir
            Common.CheckEnvInstalled();

            // Do file/directory checks
            Common.CheckFiles();

            // Check if another process of the same name exists


            ////////////////////////

            // Start running the app. If DEBUG, run without exception logging.
            // If RELEASE, then yes, put it in a try{}catch{} block.
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                try {
                    Application.EnableVisualStyles();

                    //#if RELEASE
                    Application.Run(new Primary());
                    mutex.ReleaseMutex();
                } catch(Exception e)
                {
                    mutex.ReleaseMutex();

                    if (!CleaningUp)
                    {
                        Process.Start(Application.ExecutablePath);
                        Environment.Exit(-1);
                    }
                }
            }
            else
            {
                return;
            }
//#endif
/*
            try { throw new NotImplementedException(); Application.Run(new Primary()); }
            catch (Exception e)
            {
                Common.SendExceptionLog(e, "T7EBackground");
                Environment.Exit(-1);
            }
            */
        }

    }
}
