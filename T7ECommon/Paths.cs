using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace T7ECommon
{
    public partial class Common
    {
        #region Environment Paths

        static public string EnvPath_SystemRoot = Environment.GetEnvironmentVariable("systemroot");
        static public string EnvPath_AppData = Environment.GetEnvironmentVariable("appdata");
        // On 64-bit, this is gonna be C:\Program Files
        static public string EnvPath_ProgramFiles = Environment.GetEnvironmentVariable("programfiles");
        static public string EnvPath_ProgramFilesX86 = Environment.GetEnvironmentVariable("programfiles(x86)");
        static public string EnvPath_ProgramFilesW6432 = Environment.GetEnvironmentVariable("programfilesw6432");
        static public string EnvPath_UserProfile = Environment.GetEnvironmentVariable("userprofile");
        static public string EnvPath_Public = Environment.GetEnvironmentVariable("public");
        static public string EnvPath_AllUsersProfile = Environment.GetEnvironmentVariable("allusersprofile");
        static public string EnvPath = Environment.GetEnvironmentVariable("Path");

        #endregion

        #region Utilities
        public static string ReplaceEx(string original,
                    string pattern, string replacement)
        {
            if (pattern == null || pattern.Trim().Length <= 0) return original;
            if (replacement == null) replacement = "";

            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            
            return new string(chars, 0, count);
        }
        #endregion

        static public string ReplaceEnvVarToExpandedPath(string pathString)
        {
            return ReplaceEnvVarToExpandedPath(pathString, true);
        }

        static public string ReplaceEnvVarToExpandedPath(string pathString, bool fromX64)
        {
            if (pathString == null || pathString.Length <= 0) return "";

            // If we cared about case-sensitivity for XML files, we could do a regex
            pathString = ReplaceEx(pathString, "%userprofile%", EnvPath_UserProfile);
            pathString = ReplaceEx(pathString, "%appdata%", EnvPath_AppData);

            pathString = _EnvVarResolveProgramFiles(pathString, fromX64);

            pathString = ReplaceEx(pathString, "%systemroot%", EnvPath_SystemRoot);
            pathString = ReplaceEx(pathString, "%public%", EnvPath_Public);
            pathString = ReplaceEx(pathString, "%allusersprofile%", EnvPath_AllUsersProfile);

            return pathString;
        }

        static private string _EnvVarResolveProgramFiles(string pathString, bool fromX64)
        {
            // Depending on which environment we're operating on, we need to resolve %programfiles%
            if (Common.Env64)
            {
                if (fromX64) // For Jumplist Packs
                {
                    // x64 systems know where everything's at. Replace as-is
                    pathString = ReplaceEx(pathString, "%programfiles%", EnvPath_ProgramFiles);
                    pathString = ReplaceEx(pathString, "%programfilesx86%", EnvPath_ProgramFilesX86);
                    pathString = ReplaceEx(pathString, "%programfilesw6432%", EnvPath_ProgramFilesW6432);
                }
                else
                {
                    // x64 needs to determine where the x86-originated program might be.
                    // If we're operating on just one path, that's easy:
                    // Determine the existence of C:\Program Files (x86)\path.ext. If it exists, replace with C:\Program Files.
                    // If it doesn't, or we're not opearting on a path, then default to C:\Program Files
                    string filePath = ReplaceEx(pathString, "%programfiles%", EnvPath_ProgramFilesX86);
                    string filePathx64 = ReplaceEx(pathString, "%programfiles%", EnvPath_ProgramFiles);
                    if (File.Exists(filePath) || Directory.Exists(filePath))
                        pathString = filePath;
                    else if (File.Exists(filePathx64) || Directory.Exists(filePathx64))
                        pathString = filePathx64;
                    else
                        pathString = filePath;
                }
            }
            else // we're running on x86
            {
                // Everything becomes C:\Program Files, in x86
                pathString = ReplaceEx(pathString, "%programfiles%", EnvPath_ProgramFiles);
                pathString = ReplaceEx(pathString, "%programfilesx86%", EnvPath_ProgramFiles);
                pathString = ReplaceEx(pathString, "%programfilesw6432%", EnvPath_ProgramFiles);
            }

            return pathString;
        }

        static public string ReplaceExpandedPathToEnvVar(string input)
        {
            string output = input;

            output = ReplaceEx(output, EnvPath_UserProfile, "%userprofile%");
            output = ReplaceEx(output, EnvPath_AppData, "%appdata%");
            output = ReplaceEx(output, EnvPath_ProgramFiles, "%programfiles%");
            output = ReplaceEx(output, EnvPath_SystemRoot, "%systemroot%");
            output = ReplaceEx(output, EnvPath_ProgramFilesX86, "%programfilesx86%");
            // I understand replacing the %programfilesw6432% incoming envvar, but outgoing is redundant
            // since %programfiles% was already operated
            //output = ReplaceEx(output, EnvPath_ProgramFilesW6432, "%programfilesw6432%");
            output = ReplaceEx(output, EnvPath_Public, "%public%");
            output = ReplaceEx(output, EnvPath_AllUsersProfile, "%allusersprofile%");

            return output;
        }
    }
}
