using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace T7ECommon
{
    class Messages
    {
        // Prefixes:
        // S_ -- System
        // J_ -- Jumplist

        public static string S_OsNotSupported = "Jumplist Extender cannot run on systems prior to Windows 7.";

        static List<string> stringList = new List<string>();

        public static void LoadLanguage(string languageFilename)
        {
            // Load programfiles\Jumplist Extender\Languages\languageFilename.xml
            // <language name="Portuguesa" jleVersion="1.0">
            // <string name="S_OsNotSupported">Translation string</string>
            // </language>

            //stringList.

            //RefreshText();
        }

        public static void RefreshText()
        {
            // Refresh all labels here with their new lagnauges.
        }
    }
}
