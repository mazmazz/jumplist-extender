using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T7ECommon;
using System.IO;
using System.Xml;

namespace T7EBackground
{
    public partial class Primary
    {
        #region AppList.xml Reading
        #region ReadAppList()
        /// <summary>
        /// Reads AppList.xml through without checking for changes
        /// </summary>
        private void ReadAppList()
        {
            ReadAppList(false);
        }

        /// <summary>
        /// Reads AppList.xml, optionally checking for changes from the previously saved list.
        /// </summary>
        /// <param name="checkChanges">Check for AppList.xml changes</param>
        /// <returns>2-part dictionary array consisting of, 1. New AppIds, and 2. Deleted AppIds. Empty if checkChanges is false.</returns>
        private Dictionary<string, string>[] ReadAppList(bool checkChanges)
        {
            CommonLog("Reading applist for configured apps.", 1);

            int newAppCount = 0;
            List<string> newAppIds = new List<string>();
            List<string> newAppProcessNames = new List<string>();

            /////////// XML CODE
            XmlReader reader = null;
            try
            {
                reader = XmlReader.Create(Path.Combine(Common.Path_AppData, "AppList.xml"));
                
                CommonLog("Reading AppList.xml", 1);

                // Find <appList>. If found, break.
                // We just do this to find if there's appList at all, first.
                // If there's not, XmlReader seeks to the end, and it can't read anymore
                while (reader.Read())
                    if (reader.IsStartElement("appList"))
                        break;

                // Under <appList>, find all <app> tags. Register their attributes
                while (reader.Read())
                {
                    if (reader.IsStartElement("app"))
                    {
                        newAppIds.Add(reader.GetAttribute("id"));
                        newAppProcessNames.Add(reader.GetAttribute("processName"));
                        
                        newAppCount++;

                        // If appid settings directory doesn't exist in Path_AppData, create it.
                        if (!Directory.Exists(Path.Combine(Common.Path_AppData, newAppIds.Last())))
                            Directory.CreateDirectory(Path.Combine(Common.Path_AppData, newAppIds.Last()));

                        CommonLog("Read App " + newAppCount
                            + " | " + newAppProcessNames.Last()
                            + " | " + newAppIds.Last()
                        );
                    }
                }

                reader.Close();
                CommonLog("Finished reading AppList.xml", -1);
            }
            catch (Exception e)
            {
                CommonLog("Reading AppList.xml failed: " + e.ToString()
                            + Environment.NewLine + "Returning empty applist", -2);

                // Upon fail, return an empty changesList. This will be interpreted
                // by the calling function to stop actions.
                Dictionary<string, string>[] compareListEmpty = new Dictionary<string, string>[2];
                compareListEmpty[0] = new Dictionary<string, string>();
                compareListEmpty[1] = new Dictionary<string, string>();

                if (reader != null) reader.Close();

                return compareListEmpty;
            }

            ////////////////////////

            // If compareList is true, return a collection of string lists:
            // List 1: Added appids
            // List 2: Deleted appids
            Dictionary<string, string>[] compareList = new Dictionary<string, string>[2];

            if (checkChanges)
            {
                CommonLog("Checking additions to applist", 1);

                // Check for newly added elements
                Dictionary<string, string> addedList = new Dictionary<string, string>();
                int match = 0;
                for (int i = 0; i < newAppCount; i++)
                {
                    // If item does not exist in Common.AppProcessNames, then it's newly added.
                    CommonLog("Checking " + Common.GetLogAppString(newAppProcessNames[i], newAppIds[i]), 1);

                    foreach (string AppProcessName in Common.AppProcessNames)
                        if (newAppProcessNames[i].Equals(AppProcessName)) match++; // Since it exists

                    // After the loop, if it does not exist, match == 0.
                    // Add it to newAppList!
                    if (match == 0)
                    {
                        CommonLog("App " + newAppProcessNames[i] + " | " + newAppIds[i] + "was newly added.", -1);
                        addedList.Add(newAppProcessNames[i], newAppIds[i]);
                    }
                    else
                    {
                        CommonLog(-1);
                        // Set match = 0 for checking the deleted elements
                        match = 0;
                    }
                }

                compareList[0] = addedList;
                CommonLog(-1);

                // Check for deleted elements
                CommonLog("Checking deletions from applist", 1);
                Dictionary<string, string> deletedList = new Dictionary<string, string>();
                match = 0;
                foreach (string AppProcessName in Common.AppProcessNames)
                {
                    // If item exists in Common.AppProcessNames, but NOT in newAppProcessNames,
                    // then it was deleted.
                    CommonLog("Checking " + AppProcessName, 1);

                    foreach (string newAppProcessName in newAppProcessNames)
                    {
                        if (AppProcessName.Equals(newAppProcessName)) match++; // Since it exists
                    }

                    // BUT, if it does NOT exist, we go here. Add it to deletedList!
                    if (match == 0)
                    {
                        CommonLog("App " + AppProcessName + " was deleted", -1);
                        deletedList.Add(AppProcessName, ""); // We don't need the AppId index
                    }
                    // AppId folder WOULD be deleted, but not wise.
                    else
                    {
                        CommonLog(-1);
                        match = 0;
                    }
                }

                compareList[1] = deletedList;
                CommonLog(-1);

                CommonLog("Done checking applist modifications!", -1);
            }
            else
            {
                // Init added and deleted lists as empty
                compareList[0] = new Dictionary<string, string>();
                compareList[1] = new Dictionary<string, string>();
            }

            // Transfer new lists to the master memory list.
            Common.AppCount = newAppCount;
            Common.AppIds = newAppIds;
            Common.AppProcessNames = newAppProcessNames;

            CommonLog("Finished reading applist", -1);

            // This will be empty if we didn't check for changes.
            return compareList;
        }
        #endregion

        /// <summary>
        /// Listener method for LNK watcher: Detects changes on AppList.xml, and passes them to shortcuts and windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AppListWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            /* This fires twice on my system -- not sure on yours.
             * If file access fails, ReadAppList returns an empty appListChanegs
             * I can't think of a reliable, universal way to prevent this.
             * Anyways, this should fire twice, so ONE of the attempts should work?
             * For now, just refuse to process if appListChanges = 0
             */

            // Reload the app list, look for list differences
            Dictionary<string, string>[] appListChanges = ReadAppList(true);
            // Are there no differences? Return without change.
            if (appListChanges[0].Count == 0 && appListChanges[1].Count == 0) return;

            /* Shortcuts BEFORE windows! When deleting window appids, WinShell snaps
             * the window to the nearest matched shortcut. If you remove the shortcut
             * AFTER the window is matched, WinShell still thinks the window has
             * the deleted jumplist.
             */
            AssignAppIdsToShortcuts("", appListChanges);
            AssignAppIdsToWindows(IntPtr.Zero, appListChanges);
        }
        #endregion
    }
}
