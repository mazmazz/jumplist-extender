using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace KillT7E
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (Process T7EPreferencesProcess in Process.GetProcessesByName("T7EPreferences"))
            {
                T7EPreferencesProcess.Kill();
            }

            foreach (Process T7EBackgroundProcess in Process.GetProcessesByName("T7EBackground"))
            {
                T7EBackgroundProcess.Kill();
            }
        }
    }
}
