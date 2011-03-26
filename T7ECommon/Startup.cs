using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace T7ECommon
{
    public partial class Common
    {
        // Can't null this out.
        private static DateTime TimeLockExpiration = DateTime.Today;

        public static void CheckTimeLock()
        {
            // As of Version 0.2, TimeLock is disabled.
            return;
            //if(TimeLockExpiration == null) return;

            DateTime currentTime = DateTime.Now;

            if (currentTime.CompareTo(TimeLockExpiration) > 0)
            {
                System.Windows.Forms.MessageBox.Show("Jumplist Extender v" + Assembly.GetExecutingAssembly().GetName().Version.ToString()
                    + " expired on " + TimeLockExpiration.ToLongDateString() + "." + Environment.NewLine + Environment.NewLine
                    + "Please check on the internet for a new release of Jumplist Extender.",
                    "Time Lock",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Asterisk);

                Environment.Exit(-1);
            }
        }

        public static bool EnvInstalled = true;
        public static void CheckEnvInstalled()
        {
            // A local file, INSTALLED, determines whether JLE is installed.
            if (File.Exists(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "INSTALLED")))
                EnvInstalled = true;
            else
                EnvInstalled = false;
        }

        public static void CheckWindowsVersion()
        {
            System.OperatingSystem osInfo = System.Environment.OSVersion;
            if (osInfo.Version.Major < 6 || (osInfo.Version.Major == 6 && osInfo.Version.Minor < 1))
                Fail("Jumplist Extender cannot run on systems prior to Windows 7.", 1);
        }

        public static void CheckFiles()
        {
            string programFilesDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string appDataDir;
            if (EnvInstalled)
                appDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JumplistExtender");
            else
                appDataDir = Path.Combine(programFilesDir, "LocalData");

            // Check program files, first
            if (!File.Exists(Path.Combine(programFilesDir, "Defaults\\AppList.xml")))
                Fail("AppList.xml does not exist in program directory.", 1);
            else if (!File.Exists(Path.Combine(programFilesDir, "AutoHotKey.exe")))
                Fail("AutoHotKey.exe does not exist in program directory.", 1);
            /*else if (!File.Exists(Path.Combine(programFilesDir, "PinShortcut.vbs")))
                Fail("PinShortcut.vbs does not exist in program directory.", 1);*/

            #region Directories
            // Check appdata directories
            //System.Windows.Forms.MessageBox.Show(appDataDir);
            if (!Directory.Exists(appDataDir))
            {
                try { Directory.CreateDirectory(appDataDir); }
                catch (Exception e) { Fail("Data directory could not be created.", 1); }
                // Copy each file in programFiles\Defaults into appDataDir

                // Copy each file into it’s new directory.
                DirectoryInfo source = new DirectoryInfo(Path.Combine(programFilesDir, "Defaults"));
                DirectoryInfo target = new DirectoryInfo(appDataDir);
                CopyAll(source, target);
            }

            string appDataOrigPropertiesDir = Path.Combine(appDataDir, "OrigProperties");
            if (!Directory.Exists(appDataOrigPropertiesDir))
                Directory.CreateDirectory(appDataOrigPropertiesDir);
            #endregion

            // Check essential files
            // AppList.xml
            #region Essential Files
            string appListPath = Path.Combine(appDataDir, "AppList.xml");
            if (!File.Exists(appListPath))
                File.Copy(Path.Combine(programFilesDir, "AppList.xml"), appListPath);

            string commonAhkPath = Path.Combine(appDataDir, "AHK\\JLE_Common.ahk");
            if (!File.Exists(commonAhkPath))
                File.Copy(Path.Combine(programFilesDir, "AHK\\JLE_Common.ahk"), commonAhkPath);

            string keyboardTemplatePath = Path.Combine(appDataDir, "Templates\\Keyboard.ahk");
            if (!File.Exists(keyboardTemplatePath))
                File.Copy(Path.Combine(programFilesDir, "Templates\\Keyboard.ahk"), keyboardTemplatePath);

            string ahkTemplatePath = Path.Combine(appDataDir, "Templates\\TaskAHKScript.ahk");
            if (!File.Exists(ahkTemplatePath))
                File.Copy(Path.Combine(programFilesDir, "Templates\\TaskAHKScript.ahk"), ahkTemplatePath);
            #endregion

            // We're set!
            Path_AppData = appDataDir;
            Path_ProgramFiles = programFilesDir;

#if DEBUG
            WriteDebug(programFilesDir);
            WriteDebug(appDataDir);
#endif
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into its new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static bool SanitizeUpdateResponse(string updateResponse)
        {
            // UpdateCheck2.txt format:
            // New assembly version|Update page URL|New version display
            // 0.2.3802.42522|http://jumplist.gsdn-media.com/site/Website:Update|0.2-C
            // TODO: Best thing to do is to see if string splits properly, but too lazy
            // to figure out how.
            // Example failure cases: 404 HTML page, domain name registrar page,
            // plaintext "file not found"

            if (updateResponse.Length <= 2) return false; // Must have the two delineating |s
            else if (updateResponse.Length > 2083) return false; // Internet Explorer's URL character limit is 2083. 
            else if (!updateResponse.Contains('|')) return false; // Must have |s
            else if (updateResponse.Contains('<')) return false; // If there's a < in the string, then it's likely HTML.

            else return true;
        }
    }
}
