using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace StartAHKElevated
{
    class Program
    {
        static void Main(string[] args)
        {
            string programFilesDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string scriptPath = args[0];
            StartElevated(Path.Combine(programFilesDir, "AutoHotKey.exe"), programFilesDir, "\""+scriptPath+"\"");
        }

        public static void StartElevated(string filename, string workingDirectory, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.Verb = "runas";
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.FileName = filename;
            startInfo.Arguments = arguments;

            try { Process.Start(startInfo); }
            catch (Exception e) { /* Fail silently */ }
        }

    }
}
