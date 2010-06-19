using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

using Microsoft.WindowsAPICodePack.Taskbar;

namespace T7EPreferences
{
    static class Common
    {
        static public string CurrentAppId;
        static public string _CurrentAppName;
        static public string _CurrentAppPath;
        static public string CurrentAppProcessName;

        public enum StartDialogResults
        {
            None = 0,
            NewApp,
            OpenApp,
            ManageApp
        }
        static public StartDialogResults StartDialogResult;

        static public int AppCount = 0;
        static public List<string> AppIds = new List<string>();
        static public List<string> AppProcessNames = new List<string>();
        //List<bool> AppSingleModes = new List<bool>();
        //List<bool> AppSpecialFeatures = new List<bool>();
    }
}
