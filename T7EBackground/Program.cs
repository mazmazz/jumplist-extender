using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using T7ECommon;

namespace T7EBackground
{
    static class Program
    {
        static void Main()
        {
            // Do file/directory checks
            // Are we running on Win7 or later? If we're not, exit.
            Common.CheckWindowsVersion();

            // Check install dir
            Common.CheckEnvInstalled();

            // Do file/directory checks
            Common.CheckFiles();

            ////////////////////////

            // Start running the app. If DEBUG, run without exception logging.
            // If RELEASE, then yes, put it in a try{}catch{} block.
            Application.EnableVisualStyles();

#if DEBUG
            Application.Run(new Primary());
            return;
#endif

            try { Application.Run(new Primary()); }
            catch (Exception e)
            {
                Common.SendExceptionLog(e, "T7EBackground");
                Environment.Exit(-1);
            }
        }

    }
}
