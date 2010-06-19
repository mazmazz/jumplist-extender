using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T7ECommon;
using System.IO;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
using System.Xml;
using System.Windows.Forms;

namespace T7EPreferences
{
    static partial class Preferences
    {
        #region Path Replacement
        static public string ReplacePathsToVars(string input)
        {
            string output = input;

            // 1. Replace special paths with {T7E_Tags}, in this order:
            // ** {T7E_AppId}
            // ** {T7E_AppName}
            // ** {T7E_AppPath}
            // ** {T7E_AppProcessName}

            // ** {T7E_AppSettings} - AppData\Roaming\Jumplist Extender\T7E.2382ssidu_notepad
            // ** {T7E_AppData} - AppData\Roaming\Jumplist Extender
            // ** {T7E_ProgramFiles} - C:\Program Files\Jumplist Extener
            // ** {T7E_System32} - Usually C:\Windows\System32 -- can be from C:\Windows\Sysnative or C:\Windows\SysWOW64

            output = ReplaceT7EPathTags(output);
            output = ReplaceT7EAppTags(output);
            output = Common.ReplaceExpandedPathToEnvVar(output);

            return output;
        }

        static public string ReplaceVarsToPaths(string input)
        {
            string output = input;

            output = ReplaceT7EPathTagsToStrings(output);
            output = ReplaceT7EAppTagsToStrings(output);
            output = Common.ReplaceEnvVarToExpandedPath(output);

            return output;
        }

        static public string ReplaceT7EPathTagsToStrings(string input)
        {
            string output = input;

            // technically app tags, but we want these replaced in all cases
            output = Common.ReplaceEx(output, "{JLE_AppPath}", PrimaryParent.CurrentAppPath);
            output = Common.ReplaceEx(output, "{JLE_AppProcessName}", Path.GetFileName(PrimaryParent.CurrentAppPath));

            // ** {T7E_AppSettings} - AppData\Roaming\Jumplist Extender\T7E.2382ssidu_notepad
            // ** {T7E_AppData} - AppData\Roaming\Jumplist Extender
            // ** {T7E_ProgramFiles} - C:\Program Files\Jumplist Extener
            // ** {T7E_System32} - Usually C:\Windows\System32 -- can be from C:\Windows\Sysnative or C:\Windows\SysWOW64

            output = Common.ReplaceEx(output, "{JLE_AppSettings}", Path.Combine(Common.Path_AppData, PrimaryParent.CurrentAppId));
            output = Common.ReplaceEx(output, "{JLE_AppData}", Common.Path_AppData);
            output = Common.ReplaceEx(output, "{JLE_ProgramFiles}", "%programfiles%"); // Let T7ECommon deal with it.

            // TODO: Might needs guidance from Common.Env64
            
            output = Common.ReplaceEx(output, "{JLE_System32}", Path.Combine(Common.EnvPath_SystemRoot, "system32")); // Too easy.

            return output;
        }

        static public string ReplaceT7EPathTags(string input)
        {
            string output = input;

            // technically app tags, but we want these replaced in all cases
            output = Common.ReplaceEx(output, PrimaryParent.CurrentAppPath, "{JLE_AppPath}");
            output = Common.ReplaceEx(output, Path.GetFileName(PrimaryParent.CurrentAppPath), "{JLE_AppProcessName}");

            // ** {T7E_AppSettings} - AppData\Roaming\Jumplist Extender\T7E.2382ssidu_notepad
            // ** {T7E_AppData} - AppData\Roaming\Jumplist Extender
            // ** {T7E_ProgramFiles} - C:\Program Files\Jumplist Extener
            // ** {T7E_System32} - Usually C:\Windows\System32 -- can be from C:\Windows\Sysnative or C:\Windows\SysWOW64

            output = Common.ReplaceEx(output, Path.Combine(Common.Path_AppData, PrimaryParent.CurrentAppId), "{JLE_AppSettings}");
            output = Common.ReplaceEx(output, Common.Path_AppData, "{JLE_AppData}");
            output = Common.ReplaceEx(output, Common.Path_ProgramFiles, "{JLE_ProgramFiles}");
            output = Common.ReplaceEx(output, Path.Combine(Common.EnvPath_SystemRoot, "system32"), "{JLE_System32}");
            output = Common.ReplaceEx(output, Path.Combine(Common.EnvPath_SystemRoot, "syswow64"), "{JLE_System32}");
            output = Common.ReplaceEx(output, Path.Combine(Common.EnvPath_SystemRoot, "sysnative"), "{JLE_System32}");

            return output;
        }

        static public string ReplaceT7EAppTagsToStrings(string input)
        {
            // ** {T7E_AppId}
            // ** {T7E_AppName}
            // ** {T7E_AppPath}
            // ** {T7E_AppProcessName}

            string output = input;

            output = Common.ReplaceEx(output, "{JLE_AppId}", PrimaryParent.CurrentAppId);
            output = Common.ReplaceEx(output, "{JLE_AppName}", PrimaryParent.CurrentAppName);
            output = Common.ReplaceEx(output, "{JLE_AppWindowClassName}", PrimaryParent.CurrentAppWindowClassName);

            return output;
        }

        static public string ReplaceT7EAppTags(string input)
        {
            // ** {T7E_AppId}
            // ** {T7E_AppName}
            // ** {T7E_AppPath}
            // ** {T7E_AppProcessName}

            string output = input;

            output = Common.ReplaceEx(output, PrimaryParent.CurrentAppId, "{JLE_AppId}");
            output = Common.ReplaceEx(output, PrimaryParent.CurrentAppName, "{JLE_AppName}");
            output = Common.ReplaceEx(output, PrimaryParent.CurrentAppWindowClassName, "{JLE_AppWindowClassName}");

            return output;
        }
        #endregion

        #region Pack Exporting
        // MASSIVE AMOUNTS of code-reuse. Could use some code sharing later.
        // TODO
        static string PackTempDir;
        static bool PackProgramSpecific;

        static string PackName;
        static string PackPath;
        static string PackClassName;
        static bool PackFromX64;

        static public string[] LoadJumplistPackInfo(string packFilename)
        {
            string[] output = new string[3];

            string packTempDir = Path.Combine(Path.GetTempPath(), "T7E_" + new Random().Next(1000, 9999).ToString());
            if (Directory.Exists(packTempDir)) Directory.Delete(packTempDir, true);
            Directory.CreateDirectory(packTempDir);

            FastZip fz = new FastZip();
            try { fz.ExtractZip(packFilename, packTempDir, ""); }
            catch (Exception e) {
                output[0] = "Fail:{21d2ea6f-857c-4616-a0ba-e0d5a7475c3a}";
                output[1] = "";
                output[2] = "";
                return output; 
            }

            if (!File.Exists(Path.Combine(packTempDir, "AppSettings.xml"))) { Directory.Delete(packTempDir, true); return output; }

            using (XmlReader reader = XmlReader.Create(Path.Combine(packTempDir, "AppSettings.xml")))
            {
                while (reader.Read())
                    if (reader.IsStartElement("packSettings"))
                        break;

                // Take in attributes
                output[0] = reader["name"] != null && reader["name"].Length > 0 ? reader["name"] : "";
                output[1] = reader["path"] != null && reader["path"].Length > 0 ? reader["path"] : "";
                output[2] = reader["className"] != null && reader["className"].Length > 0 ? reader["className"] : "";
            }

            Directory.Delete(packTempDir, true);
            return output;
        }

        static public bool LoadJumplistPack(string packFilename)
        {
            // 1. Make a temp dir for the pack
            string packTempDir = Path.Combine(Path.GetTempPath(), "T7E_" + new Random().Next(1000, 9999).ToString());
            if (Directory.Exists(packTempDir)) Directory.Delete(packTempDir, true);
            Directory.CreateDirectory(packTempDir);
            PackTempDir = packTempDir;
            PackProgramSpecific = false;

            try
            {
                FastZip fz = new FastZip();
                fz.ExtractZip(packFilename, PackTempDir, "");
            }
            catch (Exception e) { return false; }

            bool result = false;

            using (XmlReader reader = XmlReader.Create(Path.Combine(packTempDir, "AppSettings.xml")))
            {
                while (reader.Read())
                    if (reader.IsStartElement("packSettings"))
                        break;

                // Take in attributes
                PackName = reader["name"];
                PackPath = reader["path"] != null && reader["path"].Length > 0 ? reader["path"] : "";
                PackClassName = reader["className"];
                bool.TryParse(reader["fromX64"], out PackFromX64);

                // Look for <jumpList>
                while (reader.Read())
                {
                    if (reader.IsStartElement("jumpList"))
                    {
                        reader.Read(); // Read to text node
                        string jumplistPath = Path.Combine(packTempDir, reader.Value);
                        PrimaryParent.PackLoading = true;
                        PrimaryParent.PackTempPath = packTempDir;
                        PrimaryParent.ReadJumpList(jumplistPath); // todo: PUT IN PREFS
                        PrimaryParent.PackTempPath = "";
                        PrimaryParent.PackLoading = false;
                        break; // Since <jumpList> is all we're looking for right now
                    }
                }

                result = true;
            }

            EndRead:
            PackTempDir = "";
            PackProgramSpecific = false;
            PackName = "";
            PackPath = "";
            PackClassName = "";
            PackFromX64 = false;
            Directory.Delete(packTempDir, true);
            return result;
        }

        static public void CreateJumplistPack(string packFilename, bool isProgramSpecific)
        {
            if (PrimaryParent.ActiveControl != null) PrimaryParent.ActiveControl = null;

            // 1. Make a temp dir for the pack
            string packTempDir = Path.Combine(Path.GetTempPath(), "T7E_" + new Random().Next(1000, 9999).ToString());
            if (Directory.Exists(packTempDir)) Directory.Delete(packTempDir, true);
            Directory.CreateDirectory(packTempDir);
            PackTempDir = packTempDir;
            PackProgramSpecific = isProgramSpecific;

            // 2. Write PackSettings.xml
            _WritePackSettingsXml();

            // 3. Zip up the directory
            if (File.Exists(packFilename)) File.Delete(packFilename);
            FastZip fzip = new FastZip();
            fzip.CreateZip(packFilename, packTempDir, true, "");

            PackTempDir = "";
            PackProgramSpecific = false;
            Directory.Delete(packTempDir, true);
        }

        static public void UploadJumplistPack(string packFilename) {
            System.Windows.Forms.MessageBox.Show("lolol");
        }

        static private void _WritePackSettingsXml()
        {
            if (PrimaryParent.ActiveControl != null) PrimaryParent.ActiveControl = null;

            string xmlSource = _CreateAppSettingsXml(true);

            //
            string xmlDestName = Path.Combine(PackTempDir, "AppSettings.xml");
            File.WriteAllText(xmlDestName, xmlSource);
        }
        #endregion
    }
}
