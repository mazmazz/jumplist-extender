using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Taskbar;
using System.Xml;

namespace T7ECommon
{
    public partial class Common
    {
        #region Meta
        static public string T7E_Brand = "Jumplist Extender";
        static public string Path_AppData;
        static public string Path_ProgramFiles;
        static public bool Env64 = (IntPtr.Size == 8);

        static public TaskbarManager TaskbarManagerInstance = TaskbarManager.Instance;
        static public IntPtr WindowHandle_Primary;

        static public string Template_KBD;
        static public string Template_AHK;

        static public void ReadTemplates()
        {
            // Read keyboard template
            try
            {
                using (TextReader kbdReader = new StreamReader(Path.Combine(Path_AppData, "Templates\\Keyboard.ahk")))
                {
                    Template_KBD = kbdReader.ReadToEnd();
                }
            }
            catch (Exception e) { }

            // Read AHK template
            try
            {
                using (TextReader ahkReader = new StreamReader(Path.Combine(Path_AppData, "Templates\\TaskAHKScript.ahk")))
                {
                    Template_AHK = ahkReader.ReadToEnd();
                }
            }
            catch (Exception e) { }
        }
#endregion

        static public int AppCount = 0;
        static public List<string> AppIds = new List<string>();
        static public List<string> AppProcessNames = new List<string>();
        //List<bool> AppSingleModes = new List<bool>();
        //List<bool> AppSpecialFeatures = new List<bool>();

        static public bool PrefExists(string prefName)
        {
            if (!File.Exists(Path.Combine(Common.Path_AppData, "Preferences.xml"))) {
                File.Copy(Path.Combine(Common.Path_ProgramFiles, "Defaults\\Preferences.xml"), Path.Combine(Common.Path_AppData, "Preferences.xml"));
                return false; 
            }
            bool result = false;

            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(Path.Combine(Common.Path_AppData, "Preferences.xml"));
                while (reader.Read())
                    if (reader.Name.Equals("preferences")) break;

                while (reader.Read())
                {
                    if (reader.Name.Equals("pref") && reader["name"].Equals(prefName))
                    {
                        result = true; break;
                    }
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return result;
        }

        static public bool WritePref(string prefName, string value)
        {
            return WritePref(false, prefName, value);
        }

        static public bool WritePref(params string[] prefPairs)
        {
            return WritePref(false, prefPairs);
        }

        static public bool WritePref(bool isCdata, params string[] prefPairs)
        {
            bool result = false;
            string prefsPath = Path.Combine(Common.Path_AppData, "Preferences.xml");

            XmlDocument document = null;
            XmlWriter writer = null;
            try
            {
                document = new XmlDocument();
                if (!File.Exists(prefsPath))
                    File.Copy(Path.Combine(Common.Path_ProgramFiles, "Defaults\\Preferences.xml"), prefsPath);
                document.Load(prefsPath);

                for (int i = 0; i < prefPairs.Length; i++)
                {
                    if (prefPairs[i + 1] == null) break; // Next pair doesn't exist

                    string prefName = prefPairs[i];
                    string value = prefPairs[i + 1];
                    XmlNode prefNode = document.SelectSingleNode("//pref[@name='" + prefName + "']");
                    if (prefNode != null)
                        if (prefNode.Attributes["value"] != null)
                            prefNode.Attributes["value"].Value = value;
                        else
                        {
                            XmlAttribute attributeNode = document.CreateAttribute("value");
                            attributeNode.Value = value;
                            prefNode.Attributes.Append(attributeNode);
                        }
                    else
                    {
                        XmlNode node = document.CreateElement("pref");
                        XmlAttribute nameAttributeNode = document.CreateAttribute("name");
                        nameAttributeNode.Value = prefName;
                        node.Attributes.Append(nameAttributeNode);
                        XmlAttribute attributeNode = document.CreateAttribute("value");
                        attributeNode.Value = value;
                        node.Attributes.Append(attributeNode);

                        // Get preferences node
                        if (document.GetElementsByTagName("preferences")[0] != null)
                            document.GetElementsByTagName("preferences")[0].AppendChild(node);
                    }
                    i++; // Doing for again will skip by 2
                }

                writer = XmlWriter.Create(Path.Combine(Common.Path_AppData, "Preferences.xml"));
                document.WriteTo(writer);
                result = true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Preferences could not be written." + Environment.NewLine + Environment.NewLine
                    + e.ToString());
            }
            finally
            {
                if (writer != null) writer.Close();
            }

            return result;
        }

        static public string ReadPref(string prefName)
        {
            if (!File.Exists(Path.Combine(Common.Path_AppData, "Preferences.xml")))
            {
                File.Copy(Path.Combine(Common.Path_ProgramFiles, "Defaults\\Preferences.xml"), Path.Combine(Common.Path_AppData, "Preferences.xml"));
                return "";
            }
            string result = "";

            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(Path.Combine(Common.Path_AppData, "Preferences.xml"));
                while (reader.Read())
                    if (reader.Name.Equals("preferences")) break;

                while (reader.Read())
                {
                    if (reader.Name.Equals("pref") && reader["name"].Equals(prefName))
                    {
                        if (reader["value"] != null && reader["value"].Length > 0) result = reader["value"];
                        // else, read into the CDATA[] node. TODO
                        break;
                    }
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return result;
        }

        static public string GetScriptPathForTitle(string appId, string taskTitle)
        {
            foreach (char c in Path.GetInvalidPathChars())
            {
                taskTitle = taskTitle.Replace(c.ToString(), "");
            }
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                taskTitle = taskTitle.Replace(c.ToString(), "");
            }
            taskTitle = taskTitle.Replace(" ", "");

            string returnPath = Path.Combine(Path_AppData, appId + "\\" + taskTitle + ".ahk");
            return returnPath;
        }

        static public string GetAppIdForProgramFile(string programFilePath)
        {
            // AppId is, e.g. T7E.DFC771C2_foobar2000
            // 1. Starts with "T7E."
            // 2. Hash is CRC-32-IEEE 802.3 of a long put-together string consisting of
            // EXE product name, EXE description, and EXE filename, stripped of all
            // special characters so only a-z A-Z 0-9 _. remain.
            // 3. Ends with _filenamewithoutextension, lowercase
            FileVersionInfo programFileInfo = FileVersionInfo.GetVersionInfo(programFilePath);
            string appIdHashSource = Regex.Replace(
                programFileInfo.ProductName
                + programFileInfo.FileDescription
                + Path.GetFileName(programFilePath)
            , "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);

            // Hash appIdHasSource into Crc32
            Crc32 appIdHash = new Crc32();
            appIdHash.ComputeHash(new ASCIIEncoding().GetBytes(appIdHashSource));

            // Build final appId
            string appId = "JLE." + String.Format("{0:X}", appIdHash.CrcValue) + "_" + Path.GetFileNameWithoutExtension(programFilePath).ToLower();

            if (appId.Length > 1)
                return appId;
            else
                return "";
        }

        static public string ResolveLnkPath(string lnkPath)
        {
            // Is it an MSI shortcut? Resolve THAT!
            if (!Path.GetExtension(lnkPath).Equals(".lnk", StringComparison.OrdinalIgnoreCase))
                return "";

            try
            {
                string msiOutput = MsiShortcutParser.ParseShortcut(lnkPath);
                if (msiOutput.Length > 0) return msiOutput;

                ShellLink link = new ShellLink();
                link.Open(lnkPath);
                string output = link.Target;
                link.Dispose();

                if (File.Exists(output) || Directory.Exists(output)) return output;
                else return "";
            }
            catch (Exception e) { return ""; }
        }
    }
}
