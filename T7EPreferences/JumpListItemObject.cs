using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using T7ECommon;

namespace T7EPreferences
{
    class T7EJumplistItem
    {
        // For each item, its values are stored when the textbox changes.
        // When item is recalled, so are its values.
        // When saving, the "master values" (item type index) decide which values get
        // stored in XML, and which get tossed.
        // If a state (e.g. separator vs task) calls for an invalid property (name, icon,)
        // keep the value in memory; just disable the field. And upon save, dont put in XML        
        private string CurrentAppId;
        private string CurrentAppPath;
        private Primary PrimaryParent;

        public enum ItemTypeVar
        {
            Task = 0,
            FileFolder,
            Category,
            Separator,
            CategoryTasks
        }

        public enum ActionType
        {
            Keyboard = 0,
            CommandLine,
            AutoHotKey
        }

        #region Fields
        string _ItemName = "";
        public string ItemName
        {
            set { _ItemName = value; }
            get
            {
                if (_ItemName.Length <= 0)
                {
                    switch (ItemType)
                    {
                        case ItemTypeVar.Task:
                            return "[Unnamed Task]";
                        case ItemTypeVar.FileFolder:
                            return "[Unnamed Shortcut]";
                        case ItemTypeVar.CategoryTasks: // Tasks Category
                            return "Tasks";
                        case ItemTypeVar.Category: // Category
                            return "[Unnamed Category]";
                        case ItemTypeVar.Separator: // Separator
                            return "------------------------------------";
                        default:
                            return "[Unnamed Item]";
                    }
                }
                else return _ItemName;
            }
        }
        public string ItemIconPath = "";
        int _ItemIconIndex = 0;
        public int ItemIconIndex
        {
            get { return _ItemIconIndex; }
            set
            {
                _ItemIconIndex = value;
                //GenerateItemIcon();
            }
        }
        //

        //
        Bitmap _ItemIconBitmap = null;
        public Bitmap ItemIconBitmap
        {
            get { return _ItemIconBitmap; }
            set
            {
                _ItemIconBitmap = value;
                // Set parent's icon view
            }
        }
        //
        public ItemTypeVar ItemType; // VIP
        
        public string FilePath = "";
        public bool FileRunWithApp = false;

        public ActionType TaskAction = 0; // VIP
        public string TaskAHKScript = ""; // This needs a default too, but do it only if
                                     // UI shows TaskAHKTextBox, and this is empty.
        public string TaskCMDPath = "";
        public string TaskCMDArgs = "";
        public string TaskCMDWorkDir = "";
        public bool TaskCMDShowWindow = false;

        public bool TaskKBDShortcutMode = true;
        public string TaskKBDString = "";
        public bool TaskKBDIgnoreAbsent = false;
        public bool TaskKBDIgnoreCurrent = false;
        public bool TaskKBDNew = false;
        // These two are saved and pulled up in the UI just like the rest
        // but they're codified in the AHK template differently:
        // if TaskKBDSendInBackground, then SendBackground = 1
        // if TaskKBDMinimizeAfterward, then SendBackground = 2
        // whether or not SIB is enabled.
        public bool TaskKBDSendInBackground = false;
        public bool TaskKBDMinimizeAfterward = false;
        #endregion

        #region P/Invokes
        [DllImport("shell32.dll")]
        static extern IntPtr ExtractIcon(IntPtr hInst, string lpszExeFileName, int nIconIndex);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern uint ExtractIconEx(string szFileName, int nIconIndex,
           ref IntPtr phiconLarge, ref IntPtr phiconSmall, uint nIcons);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyIcon(IntPtr hIcon);
        #endregion

        public T7EJumplistItem(Primary parent)
        {
            PrimaryParent = parent;

            TaskCMDPath = parent.CurrentAppPath; // Path will always start as this.
            TaskAHKScript = Common.Template_AHK;
            string tempAppPath = parent.CurrentAppPath;

            TaskAHKScript = TaskAHKScript.Replace("{Path_AppData}", Common.Path_AppData);
            TaskAHKScript = TaskAHKScript.Replace("{AppId}", parent.CurrentAppId);
            TaskAHKScript = TaskAHKScript.Replace("{AppName}", parent.CurrentAppName);
            TaskAHKScript = TaskAHKScript.Replace("{AppPath}", tempAppPath);
            TaskAHKScript = TaskAHKScript.Replace("{AppProcessName}", Path.GetFileName(parent.CurrentAppPath).ToLower());
            TaskAHKScript = TaskAHKScript.Replace("{AppWindowClassName}", parent.CurrentAppWindowClassName);
        }

        public string TaskKBDStringToTextString()
        {
            // Pretty much, replace {enter} with a newline.
            string textString = TaskKBDString.Replace("{Enter}", Environment.NewLine);
            return textString;
        }

        public void TextStringToTaskKBDString(string textString)
        {
            // Pretty much, replace a newline with {enter}.
            TaskKBDString = textString.Replace(Environment.NewLine, "{Enter}");
        }

        public string TaskKBDStringToShortcutString()
        {
            // For each text preceded by ^!+#, wrap it in brackets
            // Replace newlines with {Enter}
            // Separate each block with a space " "

            string shortcutString = "";
            // For parsing purposes, replace {{} with +[ and {}} with +]
            TaskKBDString = TaskKBDString.Replace("{{}", "+[");
            TaskKBDString = TaskKBDString.Replace("{}}", "+]");
            // Attempt to replace standalone brackets
            TaskKBDString = TaskKBDString.Replace("{ ", "+[ ");
            TaskKBDString = TaskKBDString.Replace(" }", " +]");
            /*// Replace all capital letters with +a +b
             * //Can't use this because capital letters may exist in {SpecialKeys}
            string tempString = "";
            for (int k = 0; k < TaskKBDString.Length; k++)
            {

                if (char.IsUpper(TaskKBDString[k])) tempString += "+" + char.ToLower(TaskKBDString[k]);
                else tempString += TaskKBDString[k];
            }
            TaskKBDString = tempString;*/

            for (int i = 0; i < TaskKBDString.Length; i++)
            {
                // Replace enter with {enter}
                if (TaskKBDString[i].Equals('\r'))
                {
                    shortcutString += "{Enter} ";
                    if (TaskKBDString[i + 1].Equals('\n')) i++;
                }

                // If run into a shortcut symbol, start wrapping it in []s
                else if (TaskKBDString[i].Equals('^')
                    || TaskKBDString[i].Equals('!')
                    || TaskKBDString[i].Equals('+')
                    || TaskKBDString[i].Equals('#'))
                {
                    shortcutString += "[";
                    bool shortcutSymbol = true;
                    bool brackets = false;
                    // Iterate until space is reached
                    for (int j = i; 
                        j < TaskKBDString.Length && !TaskKBDString[j].Equals(' ') && !TaskKBDString[j].Equals('\r'); 
                        j++)
                    {
                        if (TaskKBDString[j].Equals('^')) if (shortcutSymbol) shortcutString += "Ctrl+"; else break;
                        else if (TaskKBDString[j].Equals('!')) if (shortcutSymbol) shortcutString += "Alt+"; else break;
                        else if (TaskKBDString[j].Equals('+')) if (shortcutSymbol) shortcutString += "Shift+"; else break;
                        else if (TaskKBDString[j].Equals('#')) if (shortcutSymbol) shortcutString += "Win+"; else break;
                        else
                        {
                            shortcutSymbol = false;
                            if (TaskKBDString[j].Equals('{'))
                            {
                                brackets = true;
                                shortcutString += TaskKBDString[j];
                            }

                            else
                            {
                                if (brackets)
                                {
                                    shortcutString += TaskKBDString[j];
                                    if (TaskKBDString[j].Equals('}')) { i = j; break; }
                                }
                                else
                                {
                                    if (TaskKBDString[j].Equals('[')) shortcutString += "{[}";
                                    else if (TaskKBDString[j].Equals(']')) shortcutString += "{]}";
                                    else shortcutString += char.ToUpper(TaskKBDString[j]);
                                    i = j;
                                    break;
                                }
                            }
                        }

                        i = j;
                    }
                    shortcutString += "] ";
                }
                
                // { }s are special: Send them without trailing spaces
                else if (TaskKBDString[i].Equals('{'))
                {
                    if (i+6 <= TaskKBDString.Length && TaskKBDString.Substring(i, 6).Equals("{Ctrl}", StringComparison.OrdinalIgnoreCase))
                    { shortcutString += "[Ctrl] "; i = i + 5; }
                    else if (i + 5 <= TaskKBDString.Length && TaskKBDString.Substring(i, 5).Equals("{Alt}", StringComparison.OrdinalIgnoreCase))
                    { shortcutString += "[Alt] "; i = i + 4; }
                    else if (i + 7 <= TaskKBDString.Length && TaskKBDString.Substring(i, 7).Equals("{Shift}", StringComparison.OrdinalIgnoreCase))
                    { shortcutString += "[Shift] "; i = i + 6; }
                    else if (i + 5 <= TaskKBDString.Length && TaskKBDString.Substring(i, 5).Equals("{Win}", StringComparison.OrdinalIgnoreCase))
                    { shortcutString += "[Win] "; i = i + 4; }

                    else
                    {
                        for (int j = i;
                            j < TaskKBDString.Length && !TaskKBDString[j].Equals('}');
                            j++)
                        {
                            shortcutString += TaskKBDString[j];
                            i = j;
                        }
                    }
                }

                // " " becomes {Space}
                else if (TaskKBDString[i].Equals(' ')) shortcutString += "{Space} ";

                // If run into [], bracket those in {}s
                else if (TaskKBDString[i].Equals('[')) shortcutString += "{[} ";
                else if (TaskKBDString[i].Equals(']')) shortcutString += "{]} ";

                // else, send the character as-is
                else
                {
                    if(char.IsUpper(TaskKBDString[i]))
                        shortcutString += "[Shift+" + char.ToUpper(TaskKBDString[i]) + "] ";
                    else
                        shortcutString += char.ToUpper(TaskKBDString[i]) + " ";
                }

            }

            return shortcutString;
        }

        public void ShortcutStringToTaskKBDString(string shortcutString)
        {
            string[] shortcutStringArray = shortcutString.Split(' ');
            string taskKBDString = "";
            
            foreach(string keyBlock in shortcutStringArray) {
                if (keyBlock != null && keyBlock.Length <= 0) continue;

                string tempKeyBlock = keyBlock;

                // Change [Ctrl+{Shortcuts}] to ^ahkshortcuts
                if (tempKeyBlock[0].Equals('['))
                {
                    // Are we just dealing with shift+letter?
                    if (tempKeyBlock.IndexOf("[Shift+") == 0 && tempKeyBlock.Length == 9
                        && char.IsLetter(tempKeyBlock[7]))
                    {
                        taskKBDString += char.ToUpper(tempKeyBlock[7]);
                    }
                    else
                    {
                        tempKeyBlock = tempKeyBlock.Trim('[', ']');
                        tempKeyBlock = tempKeyBlock.Replace("Ctrl+", "^");
                        tempKeyBlock = tempKeyBlock.Replace("Alt+", "!");
                        tempKeyBlock = tempKeyBlock.Replace("Shift+", "+");
                        tempKeyBlock = tempKeyBlock.Replace("Win+", "#");

                        // Also cover the standalones
                        tempKeyBlock = tempKeyBlock.Replace("Ctrl", "{Ctrl}");
                        tempKeyBlock = tempKeyBlock.Replace("Alt", "{Alt}");
                        tempKeyBlock = tempKeyBlock.Replace("Shift", "{Shift}");
                        tempKeyBlock = tempKeyBlock.Replace("Win", "{Win}");

                        // Convert escaped {[}s to [s
                        tempKeyBlock = tempKeyBlock.Replace("{[}", "[");
                        tempKeyBlock = tempKeyBlock.Replace("{]}", "]");
                        taskKBDString += tempKeyBlock.ToLower(); // TODO: {Space} should be converted {Space}, not {space}
                    }
                }

                // {Enter} is already in its needed form.

                // Else, add the character as-is
                else {
                    // First, replace {[} {]}
                    tempKeyBlock = tempKeyBlock.Replace("{[}", "[");
                    tempKeyBlock = tempKeyBlock.Replace("{]}", "]");

                    // {Space} becomes " ", if used by itself
                    tempKeyBlock = tempKeyBlock.Replace("{Space}", " ");

                    if (tempKeyBlock.Length == 1) taskKBDString += tempKeyBlock.ToLower();
                    else taskKBDString += tempKeyBlock;
                }
            }

            TaskKBDString = taskKBDString;
        }

        public void StringToItemWorkDir(string cmdWorkDir)
        {
            TaskCMDWorkDir = cmdWorkDir;
        }

        public void StringToItemCmd(string cmdString)
        {
            string cmdLine = cmdString.Trim();
            string cmdArg1 = "";
            string cmdArgRest = "";
            bool quotedArg1 = false;

            if (cmdLine.Length <= 0)
            {
                TaskCMDPath = PrimaryParent.CurrentAppPath;
                TaskCMDArgs = "";
                return;
            }

            if (cmdLine[0].Equals('"'))
            {
                quotedArg1 = true;
                // cmdArg1 is the first quoted thing
                for (int i = 1; i < cmdLine.Length; i++)
                {
                    if (!cmdLine[i].Equals('"'))
                        cmdArg1 += cmdLine[i];
                    else
                    {
                        cmdArgRest = i + 1 >= cmdLine.Length ? 
                            "" 
                            : cmdLine[i+1] == ' ' && i+2 < cmdLine.Length ? 
                                cmdLine.Substring(i+2) 
                                : cmdLine.Substring(i+1);
                        break;
                    }
                }
            }
            else
            {
                // cmdArg1 is the first string until space
                for (int i = 0; i < cmdLine.Length; i++)
                {
                    if (!cmdLine[i].Equals(' '))
                        cmdArg1 += cmdLine[i];
                    else
                    {
                        cmdArgRest = i + 1 >= cmdLine.Length ? "" : cmdLine.Substring(i + 1);
                        break;
                    }
                }
            }

            // First, check if newTaskPath is actually a path. If it's not, it's likely
            // args for the program.
            TaskCMDPath = "";
            if (File.Exists(cmdArg1)
                || Directory.Exists(cmdArg1)
                && !Path.Equals(cmdArg1, PrimaryParent.CurrentAppPath))
            {
                TaskCMDPath = cmdArg1;
                TaskCMDArgs = cmdArgRest;
            }

            string[] envPaths = Common.EnvPath.Split(';');
            string cmdArg1Filename = Path.GetFileName(cmdArg1); 
            foreach (string envPath in envPaths)
            {
                if (File.Exists(Path.Combine(envPath, cmdArg1Filename))
                || Directory.Exists(Path.Combine(envPath, cmdArg1Filename))
                && !Path.Equals(cmdArg1, PrimaryParent.CurrentAppPath))
                {
                    TaskCMDPath = cmdArg1;
                    TaskCMDArgs = cmdArgRest;
                }
            }
            
            if(TaskCMDPath == "") // It's args for the progarm.
            {
                TaskCMDPath = PrimaryParent.CurrentAppPath;
                TaskCMDArgs = quotedArg1 ?
                    "\"" + cmdArg1 + "\" " + cmdArgRest
                    : cmdArg1 + " " + cmdArgRest;
            }
        }

        public string ItemWorkDirToString()
        {
            return TaskCMDWorkDir;
        }

        public string ItemCmdToString()
        {
            if (TaskCMDPath.Length <= 0)
                return "\"" + PrimaryParent.CurrentAppPath + "\" ";

            //if (Path.Equals(TaskCMDPath, PrimaryParent.CurrentAppPath))
            //    return TaskCMDArgs;
            //else
                return "\"" + TaskCMDPath + "\" " + TaskCMDArgs;
        }

        public string ItemIconToString()
        {
            if (ItemIconPath.Equals(PrimaryParent.CurrentAppPath, StringComparison.CurrentCultureIgnoreCase)
                && ItemIconIndex == 0)
                return "Use program icon";
            else if (ItemIconPath.Length <= 0)
                return "Don't use an icon";
            else
            {
                string pathExtension = Path.GetExtension(ItemIconPath).ToLower();
                switch(pathExtension) {
                    case ".png":
                    case ".jpg":
                    case ".gif":
                    case ".bmp":
                        return ItemIconPath;
                    default:
                        return ItemIconPath + "|" + ItemIconIndex.ToString();
                }
            }
        }

        public void StringToItemIcon(string iconString)
        {
            string itemIconPath = "";
            int itemIconIndex = 0;
            if (iconString.Equals("Use program icon", StringComparison.OrdinalIgnoreCase))
            {
                // Bitmap-ize
                itemIconPath = PrimaryParent.CurrentAppPath;
                itemIconIndex = 0;
            }
            else if (iconString.Equals("Don't use an icon", StringComparison.OrdinalIgnoreCase)
                || iconString.Trim().Length <= 0)
            {
                // Blank bitmap-ize
                itemIconPath = "";
                itemIconIndex = 0;
            }
            else
            {
                string[] iconStringSplit = iconString.Split('|');
                if (iconStringSplit.Length >= 1
                    && File.Exists(iconStringSplit[0])
                    || Directory.Exists(iconStringSplit[0]))
                {
                    itemIconPath = iconStringSplit[0];
                    if(iconStringSplit.Length >= 2)
                        Int32.TryParse(iconStringSplit[1], out itemIconIndex);
                }
                else
                    return;

                string iconPathExtension = Path.GetExtension(itemIconPath.ToLower());
                // Consider this where we Bitmap-ize our icon?
                switch (iconPathExtension)
                {
                    case ".png":
                    case ".jpg":
                    case ".gif":
                    case ".bmp":
                        // bitmapize
                        itemIconIndex = 0;
                        break;
                    case ".ico":
                    case ".dll":
                    case ".exe":
                        itemIconIndex = 0;
                        if (iconStringSplit.Length >= 2)
                            int.TryParse(iconStringSplit[1], out itemIconIndex);
                        // Check if index exists
                        IntPtr zeroPtr = IntPtr.Zero;
                        uint iconCount = ExtractIconEx(itemIconPath, -1, ref zeroPtr, ref zeroPtr, 1);

                        Common.WriteDebug(iconCount.ToString());
                        if (itemIconIndex < iconCount)
                            break;
                        else if (itemIconIndex >= iconCount
                            && iconCount >= 1)
                            itemIconIndex = 0;
                        else
                            return; // Our icon doesn't exist, chuck.

                        break;
                }
            }

            // Bitmapize
            if (!BitmapIzeItemIcon(itemIconPath, itemIconIndex))
                return;
            ItemIconPath = itemIconPath;
            ItemIconIndex = itemIconIndex;
        }

        private bool BitmapIzeItemIcon(string iconPath, int iconIndex)
        {
            Bitmap iconBitmap = new Bitmap(48,48);
            if (iconPath.Length <= 0)
            {
                ItemIconBitmap = iconBitmap;
                return true;
            }

            try
            {
                iconPath = Path.GetFullPath(iconPath.Trim('\"')); // Well-formed

                Common.Log("Evaluating icon. Path: " + iconPath, 1);

                if (!File.Exists(iconPath))
                    return false;

                switch (Path.GetExtension(iconPath).ToLower())
                {
                    case ".dll":
                    case ".exe":
                    case ".ico":
                        // Get iconResourceNum from iconpickdialog
                        //IntPtr iconPointer = ExtractIcon(IntPtr.Zero, iconPath, iconIndex);
                        IntPtr largeIconPointer = IntPtr.Zero;
                        IntPtr dummyPointer = IntPtr.Zero;
                        //uint icons = ExtractIconEx(iconPath, iconIndex, ref largeIconPointer, ref dummyPointer, 1);
                        largeIconPointer = ExtractIcon(IntPtr.Zero, iconPath, iconIndex);
                        Icon extractedIcon = Icon.FromHandle(largeIconPointer);

                        iconBitmap = extractedIcon.ToBitmap();

                        extractedIcon.Dispose();
                        DestroyIcon(dummyPointer);
                        DestroyIcon(largeIconPointer);
                        break;

                    case ".png":
                    case ".bmp":
                    case ".gif":
                    case ".jpg":
                        Image sourceImage = Image.FromFile(iconPath);

                        int sourceImageWidth;
                        int sourceImageHeight;
                        if (sourceImage.Width > sourceImage.Height)
                        {
                            if (sourceImage.Width > 48)
                            {
                                double resizePercent = (double)48 / (double)sourceImage.Width;
                                sourceImageWidth = 48;
                                sourceImageHeight = (int)(sourceImage.Height * resizePercent);
                                if (sourceImageHeight > 48) sourceImageHeight = 48;
                            }
                            else
                            {
                                sourceImageWidth = sourceImage.Width;
                                sourceImageHeight = sourceImage.Height;
                            }
                        }
                        else
                        {
                            if (sourceImage.Height > 48)
                            {
                                double resizePercent = (double)48 / (double)sourceImage.Height;
                                sourceImageWidth = (int)(sourceImage.Width * resizePercent);
                                if (sourceImageWidth > 48) sourceImageWidth = 48;
                                sourceImageHeight = 48;
                            }
                            else
                            {
                                sourceImageWidth = sourceImage.Width;
                                sourceImageHeight = sourceImage.Height;
                            }
                        }

                        int dimension = sourceImageWidth > sourceImageHeight ?
                            sourceImageWidth
                            : sourceImageHeight;

                        int dimensionDiff = Math.Abs(sourceImageWidth - sourceImageHeight);

                        Bitmap resultBitmap = new Bitmap(dimension, dimension);
                        Graphics workingGraphic = Graphics.FromImage((Image)resultBitmap);
                        workingGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                        // If image is not square, make it square
                        if (sourceImage.Width > sourceImage.Height)
                            workingGraphic.DrawImage(sourceImage, 0, dimensionDiff / 2, sourceImageWidth, sourceImageHeight);
                        else
                            workingGraphic.DrawImage(sourceImage, dimensionDiff / 2, 0, sourceImageWidth, sourceImageHeight); // If the image is square, dimensionDiff should == 0

                        workingGraphic.Dispose();
                        sourceImage.Dispose();

                        iconBitmap = new Bitmap((Image)resultBitmap);
                        resultBitmap.Dispose();
                        break;
                    default:
                        break;
                }

                Common.Log("Done evaluating icon.", -1);
            } catch(Exception e) { return false; }

            //iconBitmap.Save("icon.png", System.Drawing.Imaging.ImageFormat.Png);
            ItemIconBitmap = iconBitmap;
            return true;
        }

        public override string ToString()
        {
            // Return representation of this item, depending on its type:
            switch (ItemType)
            {
                case ItemTypeVar.Task: // Task
                    return ItemName != null && ItemName.Length > 0 ?
                        "        " + ItemName :
                        "        [Unnamed Task]";
                case ItemTypeVar.FileFolder: // FileFolder
                    return ItemName != null && ItemName.Length > 0 ? 
                        "        " + ItemName :
                        "        [Unnamed Shortcut]";
                case ItemTypeVar.CategoryTasks: // Tasks Category
                case ItemTypeVar.Category: // Category
                    return ItemName != null && ItemName.Length > 0 ? 
                        ItemName :
                        "[Unnamed Category]";
                case ItemTypeVar.Separator: // Separator
                    return "        ------------------------------------";
            }
            
            return base.ToString();
        }

        #region Data Utilities
        private Bitmap EvaluateIcon(string iconPath)
        {
            int iconIndex = 0;
            if (iconPath.ToLower() == "use program icon")
                iconPath = CurrentAppPath;
            else if (iconPath.ToLower() == "don't use an icon")
                iconPath = "";
            else
            {
                string[] iconPathSplit = iconPath.Split('|');
                iconPath = iconPathSplit[0] == null ? "" : iconPathSplit[0];
                if (iconPathSplit[1] != null)
                    iconIndex = Convert.ToInt32(iconPathSplit[1]);
            }

            return EvaluateIcon(iconPath, iconIndex);
        }
        private Bitmap EvaluateIcon(string iconPath, int iconResourceNum)
        {
            Bitmap iconBitmap = null;

            string iconLocalPath = Path.Combine(Common.Path_AppData, CurrentAppId + "\\" + Path.GetFileNameWithoutExtension(iconPath).ToLower() + "_" + iconResourceNum.ToString() + ".ico");

            if (!File.Exists(iconPath))
            {
                if (File.Exists(iconLocalPath))
                    iconPath = iconLocalPath;
                else
                    return null;
            }

            switch (Path.GetExtension(iconPath).ToLower())
            {
                case ".dll":
                case ".exe":
                    try
                    {
                        IntPtr iconPointer = ExtractIcon(IntPtr.Zero, iconPath, iconResourceNum);
                        Icon extractedIcon = Icon.FromHandle(iconPointer);
                        iconBitmap = extractedIcon.ToBitmap();
                        extractedIcon.Dispose();
                    }
                    catch (Exception e)
                    {
                        break;
                    }

                    break;

                case ".ico":
                    try
                    {
                        IntPtr iconPointer = ExtractIcon(IntPtr.Zero, iconPath, 0);
                        Icon extractedIcon = Icon.FromHandle(iconPointer);
                        iconBitmap = extractedIcon.ToBitmap();
                        extractedIcon.Dispose();
                    }
                    catch (Exception e)
                    {
                        break;
                    }

                    break;

                case ".png":
                case ".bmp":
                case ".gif":
                case ".jpg":
                    Image sourceImage = Image.FromFile(iconPath);

                    int sourceImageWidth;
                    int sourceImageHeight;
                    if (sourceImage.Width > sourceImage.Height)
                    {
                        if (sourceImage.Width > 256)
                        {
                            double resizePercent = (double)256 / (double)sourceImage.Width;
                            sourceImageWidth = 256;
                            sourceImageHeight = (int)(sourceImage.Height * resizePercent);
                            if (sourceImageHeight > 256) sourceImageHeight = 256;
                        }
                        else
                        {
                            sourceImageWidth = sourceImage.Width;
                            sourceImageHeight = sourceImage.Height;
                        }
                    }
                    else
                    {
                        if (sourceImage.Height > 256)
                        {
                            double resizePercent = (double)256 / (double)sourceImage.Height;
                            sourceImageWidth = (int)(sourceImage.Width * resizePercent);
                            if (sourceImageWidth > 256) sourceImageWidth = 256;
                            sourceImageHeight = 256;
                        }
                        else
                        {
                            sourceImageWidth = sourceImage.Width;
                            sourceImageHeight = sourceImage.Height;
                        }
                    }

                    int dimension = sourceImageWidth > sourceImageHeight ?
                        sourceImageWidth
                        : sourceImageHeight;

                    int dimensionDiff = Math.Abs(sourceImageWidth - sourceImageHeight);

                    Bitmap resultBitmap = new Bitmap(dimension, dimension);
                    Graphics workingGraphic = Graphics.FromImage((Image)resultBitmap);
                    workingGraphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                    // If image is not square, make it square
                    if (sourceImage.Width > sourceImage.Height)
                        workingGraphic.DrawImage(sourceImage, 0, dimensionDiff / 2, sourceImageWidth, sourceImageHeight);
                    else
                        workingGraphic.DrawImage(sourceImage, dimensionDiff / 2, 0, sourceImageWidth, sourceImageHeight); // If the image is square, dimensionDiff should == 0

                    workingGraphic.Dispose();
                    sourceImage.Dispose();

                    iconBitmap = new Bitmap((Image)resultBitmap);
                    resultBitmap.Dispose();
                    break;
                default:
                    break;
            }

            return iconBitmap;
        }
        #endregion
    }
}
