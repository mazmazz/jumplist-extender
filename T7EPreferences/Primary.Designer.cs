using System.Drawing;

namespace T7EPreferences
{
    partial class Primary
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ImageComboBox.ImageComboBoxItem imageComboBoxItem1 = new ImageComboBox.ImageComboBoxItem();
            ImageComboBox.ImageComboBoxItem imageComboBoxItem2 = new ImageComboBox.ImageComboBoxItem();
            ImageComboBox.ImageComboBoxItem imageComboBoxItem3 = new ImageComboBox.ImageComboBoxItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Primary));
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuFileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAndApplyToTaskbarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importProgramSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFileSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuToolsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuToolsPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.keyboardShortcutHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuToolsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuToolsAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToVersion0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusBar = new System.Windows.Forms.StatusStrip();
            this.ToolBarStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.AddButtonContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCategoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSeparatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TabJumplist = new System.Windows.Forms.TabPage();
            this.ItemTypePanel = new System.Windows.Forms.Panel();
            this.TaskGroupBox = new System.Windows.Forms.GroupBox();
            this.TaskActionLabel = new System.Windows.Forms.Label();
            this.TaskActionComboBox = new System.Windows.Forms.ComboBox();
            this.TaskKBDPanel = new System.Windows.Forms.Panel();
            this.TaskKBDTextPanel = new System.Windows.Forms.Panel();
            this.TaskKBDSwitchAHKButton = new System.Windows.Forms.Button();
            this.TaskKBDHelpButton2 = new System.Windows.Forms.Button();
            this.TaskKBDTextBox = new System.Windows.Forms.TextBox();
            this.TaskKBDLabel = new System.Windows.Forms.Label();
            this.TaskKBDSettingsPanel = new System.Windows.Forms.Panel();
            this.TaskKBDIgnoreCurrentCheckBox = new System.Windows.Forms.CheckBox();
            this.TaskKBDIgnoreAbsentCheckBox = new System.Windows.Forms.CheckBox();
            this.TaskKBDNewCheckBox = new System.Windows.Forms.CheckBox();
            this.TaskKBDSwitchTextButton = new System.Windows.Forms.Button();
            this.TaskKBDSwitchShortcutButton = new System.Windows.Forms.Button();
            this.TaskKBDShortcutPanel = new System.Windows.Forms.Panel();
            this.TaskKBDShortcutClearButton = new System.Windows.Forms.Button();
            this.TaskKBDKeyboardTextBox = new T7ECommon.KeyboardTextBox();
            this.TaskKBDShortcutLabel = new System.Windows.Forms.Label();
            this.TaskKBDHelpButton = new System.Windows.Forms.Button();
            this.TaskCMDPanel = new System.Windows.Forms.Panel();
            this.TaskCMDHelpButton = new System.Windows.Forms.Button();
            this.TaskCMDBrowseButton = new System.Windows.Forms.Button();
            this.TaskCMDShowWindowCheckbox = new System.Windows.Forms.CheckBox();
            this.TaskCMDTextBox = new System.Windows.Forms.TextBox();
            this.TaskCMDLabel = new System.Windows.Forms.Label();
            this.TaskAHKPanel = new System.Windows.Forms.Panel();
            this.TaskAHKHelpButton = new System.Windows.Forms.Button();
            this.TaskAHKSaveCopyButton = new System.Windows.Forms.Button();
            this.TaskAHKOpenExternalButton = new System.Windows.Forms.Button();
            this.TaskAHKTextBox = new System.Windows.Forms.TextBox();
            this.TaskAHKLabel = new System.Windows.Forms.Label();
            this.FileGroupBox = new System.Windows.Forms.GroupBox();
            this.FileOpenApp = new System.Windows.Forms.RadioButton();
            this.FileOpenDefault = new System.Windows.Forms.RadioButton();
            this.FileBrowseButton = new System.Windows.Forms.Button();
            this.FolderBrowseButton = new System.Windows.Forms.Button();
            this.FileTextBox = new System.Windows.Forms.TextBox();
            this.FileLabel = new System.Windows.Forms.Label();
            this.ItemPanel = new System.Windows.Forms.Panel();
            this.ItemIconPictureBox = new System.Windows.Forms.PictureBox();
            this.ItemIconComboBox = new ImageComboBox.ImageComboBox();
            this.ItemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.ItemTypeLabel = new System.Windows.Forms.Label();
            this.IconButtonBrowse = new System.Windows.Forms.Button();
            this.ItemIconLabel = new System.Windows.Forms.Label();
            this.ItemNameTextBox = new System.Windows.Forms.TextBox();
            this.ItemNameLabel = new System.Windows.Forms.Label();
            this.JumplistPanel = new System.Windows.Forms.Panel();
            this.JumplistAddButton = new T7ECommon.MenuButton();
            this.JumplistDownButton = new System.Windows.Forms.Button();
            this.JumplistUpButton = new System.Windows.Forms.Button();
            this.JumplistDeleteButton = new T7ECommon.MenuButton();
            this.DeleteButtonContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.JumplistListBox = new System.Windows.Forms.ListBox();
            this.JumplistEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.JumplistHelpText = new System.Windows.Forms.Label();
            this.TabProgramSettings = new System.Windows.Forms.TabPage();
            this.ProgramSettingsPanel = new System.Windows.Forms.Panel();
            this.ProgramNewOKButton = new System.Windows.Forms.Button();
            this.ProgramWindowClassButton = new System.Windows.Forms.Button();
            this.ProgramWindowClassLabel = new System.Windows.Forms.Label();
            this.ProgramSettingsLabel = new System.Windows.Forms.Label();
            this.ProgramPathTextBox = new System.Windows.Forms.TextBox();
            this.ProgramPathLabel = new System.Windows.Forms.Label();
            this.ProgramNameTextBox = new System.Windows.Forms.TextBox();
            this.ProgramNameLabel = new System.Windows.Forms.Label();
            this.TabControl = new System.Windows.Forms.TabControl();
            this.visitTheOfficialWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuStrip.SuspendLayout();
            this.StatusBar.SuspendLayout();
            this.AddButtonContextMenuStrip.SuspendLayout();
            this.TabJumplist.SuspendLayout();
            this.ItemTypePanel.SuspendLayout();
            this.TaskGroupBox.SuspendLayout();
            this.TaskKBDPanel.SuspendLayout();
            this.TaskKBDTextPanel.SuspendLayout();
            this.TaskKBDSettingsPanel.SuspendLayout();
            this.TaskKBDShortcutPanel.SuspendLayout();
            this.TaskCMDPanel.SuspendLayout();
            this.TaskAHKPanel.SuspendLayout();
            this.FileGroupBox.SuspendLayout();
            this.ItemPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ItemIconPictureBox)).BeginInit();
            this.JumplistPanel.SuspendLayout();
            this.DeleteButtonContextMenuStrip.SuspendLayout();
            this.TabProgramSettings.SuspendLayout();
            this.ProgramSettingsPanel.SuspendLayout();
            this.TabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFileMenu,
            this.MenuToolsMenu,
            this.updateToVersion0ToolStripMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(820, 26);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // MenuFileMenu
            // 
            this.MenuFileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.MenuFileOpen,
            this.MenuFileSave,
            this.saveAndApplyToTaskbarToolStripMenuItem,
            this.MenuFileSeparator1,
            this.importProgramSettingsToolStripMenuItem,
            this.MenuFileExport,
            this.MenuFileSeparator2,
            this.MenuFileExit});
            this.MenuFileMenu.Name = "MenuFileMenu";
            this.MenuFileMenu.Size = new System.Drawing.Size(40, 22);
            this.MenuFileMenu.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(301, 22);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // MenuFileOpen
            // 
            this.MenuFileOpen.Name = "MenuFileOpen";
            this.MenuFileOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.MenuFileOpen.Size = new System.Drawing.Size(301, 22);
            this.MenuFileOpen.Text = "&Open";
            this.MenuFileOpen.Click += new System.EventHandler(this.MenuFileOpen_Click);
            // 
            // MenuFileSave
            // 
            this.MenuFileSave.Enabled = false;
            this.MenuFileSave.Name = "MenuFileSave";
            this.MenuFileSave.Size = new System.Drawing.Size(301, 22);
            this.MenuFileSave.Text = "&Save";
            this.MenuFileSave.Visible = false;
            this.MenuFileSave.Click += new System.EventHandler(this.MenuFileSave_Click);
            // 
            // saveAndApplyToTaskbarToolStripMenuItem
            // 
            this.saveAndApplyToTaskbarToolStripMenuItem.Name = "saveAndApplyToTaskbarToolStripMenuItem";
            this.saveAndApplyToTaskbarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveAndApplyToTaskbarToolStripMenuItem.Size = new System.Drawing.Size(301, 22);
            this.saveAndApplyToTaskbarToolStripMenuItem.Text = "Save and &Apply to Taskbar";
            this.saveAndApplyToTaskbarToolStripMenuItem.Click += new System.EventHandler(this.saveAndApplyToTaskbarToolStripMenuItem_Click);
            // 
            // MenuFileSeparator1
            // 
            this.MenuFileSeparator1.Name = "MenuFileSeparator1";
            this.MenuFileSeparator1.Size = new System.Drawing.Size(298, 6);
            // 
            // importProgramSettingsToolStripMenuItem
            // 
            this.importProgramSettingsToolStripMenuItem.Name = "importProgramSettingsToolStripMenuItem";
            this.importProgramSettingsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.importProgramSettingsToolStripMenuItem.Size = new System.Drawing.Size(301, 22);
            this.importProgramSettingsToolStripMenuItem.Text = "I&mport Jumplist Pack";
            this.importProgramSettingsToolStripMenuItem.Click += new System.EventHandler(this.importProgramSettingsToolStripMenuItem_Click);
            // 
            // MenuFileExport
            // 
            this.MenuFileExport.Name = "MenuFileExport";
            this.MenuFileExport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.MenuFileExport.Size = new System.Drawing.Size(301, 22);
            this.MenuFileExport.Text = "Ex&port Jumplist Pack";
            this.MenuFileExport.Click += new System.EventHandler(this.MenuFileExport_Click);
            // 
            // MenuFileSeparator2
            // 
            this.MenuFileSeparator2.Name = "MenuFileSeparator2";
            this.MenuFileSeparator2.Size = new System.Drawing.Size(298, 6);
            // 
            // MenuFileExit
            // 
            this.MenuFileExit.Name = "MenuFileExit";
            this.MenuFileExit.Size = new System.Drawing.Size(301, 22);
            this.MenuFileExit.Text = "E&xit";
            this.MenuFileExit.Click += new System.EventHandler(this.MenuFileExit_Click);
            // 
            // MenuToolsMenu
            // 
            this.MenuToolsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuToolsPreferences,
            this.keyboardShortcutHelpToolStripMenuItem,
            this.MenuToolsSeparator1,
            this.donateToolStripMenuItem,
            this.visitTheOfficialWebsiteToolStripMenuItem,
            this.toolStripSeparator1,
            this.MenuToolsAbout});
            this.MenuToolsMenu.Name = "MenuToolsMenu";
            this.MenuToolsMenu.Size = new System.Drawing.Size(55, 22);
            this.MenuToolsMenu.Text = "Tool&s";
            // 
            // MenuToolsPreferences
            // 
            this.MenuToolsPreferences.Enabled = false;
            this.MenuToolsPreferences.Name = "MenuToolsPreferences";
            this.MenuToolsPreferences.Size = new System.Drawing.Size(264, 22);
            this.MenuToolsPreferences.Text = "&Preferences";
            this.MenuToolsPreferences.Visible = false;
            // 
            // keyboardShortcutHelpToolStripMenuItem
            // 
            this.keyboardShortcutHelpToolStripMenuItem.Name = "keyboardShortcutHelpToolStripMenuItem";
            this.keyboardShortcutHelpToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.keyboardShortcutHelpToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.keyboardShortcutHelpToolStripMenuItem.Text = "&Key Shortcut Help";
            this.keyboardShortcutHelpToolStripMenuItem.Click += new System.EventHandler(this.keyboardShortcutHelpToolStripMenuItem_Click);
            // 
            // MenuToolsSeparator1
            // 
            this.MenuToolsSeparator1.Name = "MenuToolsSeparator1";
            this.MenuToolsSeparator1.Size = new System.Drawing.Size(261, 6);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.donateToolStripMenuItem.Text = "&Donate to Jumplist Extender!";
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.donateToolStripMenuItem_Click);
            // 
            // MenuToolsAbout
            // 
            this.MenuToolsAbout.Name = "MenuToolsAbout";
            this.MenuToolsAbout.Size = new System.Drawing.Size(264, 22);
            this.MenuToolsAbout.Text = "&About";
            this.MenuToolsAbout.Click += new System.EventHandler(this.MenuToolsAbout_Click);
            // 
            // updateToVersion0ToolStripMenuItem
            // 
            this.updateToVersion0ToolStripMenuItem.Enabled = false;
            this.updateToVersion0ToolStripMenuItem.Name = "updateToVersion0ToolStripMenuItem";
            this.updateToVersion0ToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.updateToVersion0ToolStripMenuItem.Text = "&Update to Version {0}!";
            this.updateToVersion0ToolStripMenuItem.Visible = false;
            this.updateToVersion0ToolStripMenuItem.Click += new System.EventHandler(this.updateToVersion0ToolStripMenuItem_Click);
            // 
            // StatusBar
            // 
            this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolBarStatusLabel});
            this.StatusBar.Location = new System.Drawing.Point(0, 567);
            this.StatusBar.Name = "StatusBar";
            this.StatusBar.Size = new System.Drawing.Size(820, 23);
            this.StatusBar.TabIndex = 2;
            // 
            // ToolBarStatusLabel
            // 
            this.ToolBarStatusLabel.Name = "ToolBarStatusLabel";
            this.ToolBarStatusLabel.Size = new System.Drawing.Size(49, 18);
            this.ToolBarStatusLabel.Text = "Ready";
            // 
            // AddButtonContextMenuStrip
            // 
            this.AddButtonContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTaskToolStripMenuItem,
            this.newFileFolderToolStripMenuItem,
            this.newCategoryToolStripMenuItem,
            this.newSeparatorToolStripMenuItem});
            this.AddButtonContextMenuStrip.Name = "AddButtonContextMenuStrip";
            this.AddButtonContextMenuStrip.Size = new System.Drawing.Size(234, 92);
            // 
            // newTaskToolStripMenuItem
            // 
            this.newTaskToolStripMenuItem.Name = "newTaskToolStripMenuItem";
            this.newTaskToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.newTaskToolStripMenuItem.Text = "New Task";
            this.newTaskToolStripMenuItem.Click += new System.EventHandler(this.newTaskToolStripMenuItem_Click);
            // 
            // newFileFolderToolStripMenuItem
            // 
            this.newFileFolderToolStripMenuItem.Name = "newFileFolderToolStripMenuItem";
            this.newFileFolderToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.newFileFolderToolStripMenuItem.Text = "New File/Folder Shortcut";
            this.newFileFolderToolStripMenuItem.Click += new System.EventHandler(this.newFileFolderToolStripMenuItem_Click);
            // 
            // newCategoryToolStripMenuItem
            // 
            this.newCategoryToolStripMenuItem.Name = "newCategoryToolStripMenuItem";
            this.newCategoryToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.newCategoryToolStripMenuItem.Text = "New Category";
            this.newCategoryToolStripMenuItem.Click += new System.EventHandler(this.newCategoryToolStripMenuItem_Click);
            // 
            // newSeparatorToolStripMenuItem
            // 
            this.newSeparatorToolStripMenuItem.Name = "newSeparatorToolStripMenuItem";
            this.newSeparatorToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.newSeparatorToolStripMenuItem.Text = "New Separator";
            this.newSeparatorToolStripMenuItem.Click += new System.EventHandler(this.newSeparatorToolStripMenuItem_Click);
            // 
            // TabJumplist
            // 
            this.TabJumplist.Controls.Add(this.ItemTypePanel);
            this.TabJumplist.Controls.Add(this.ItemPanel);
            this.TabJumplist.Controls.Add(this.JumplistPanel);
            this.TabJumplist.Controls.Add(this.JumplistEnableCheckBox);
            this.TabJumplist.Controls.Add(this.JumplistHelpText);
            this.TabJumplist.Location = new System.Drawing.Point(4, 25);
            this.TabJumplist.Margin = new System.Windows.Forms.Padding(2);
            this.TabJumplist.Name = "TabJumplist";
            this.TabJumplist.Padding = new System.Windows.Forms.Padding(2);
            this.TabJumplist.Size = new System.Drawing.Size(812, 507);
            this.TabJumplist.TabIndex = 0;
            this.TabJumplist.Text = "Jumplist";
            this.TabJumplist.UseVisualStyleBackColor = true;
            // 
            // ItemTypePanel
            // 
            this.ItemTypePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemTypePanel.Controls.Add(this.TaskGroupBox);
            this.ItemTypePanel.Controls.Add(this.FileGroupBox);
            this.ItemTypePanel.Enabled = false;
            this.ItemTypePanel.Location = new System.Drawing.Point(309, 112);
            this.ItemTypePanel.Margin = new System.Windows.Forms.Padding(2);
            this.ItemTypePanel.Name = "ItemTypePanel";
            this.ItemTypePanel.Size = new System.Drawing.Size(500, 408);
            this.ItemTypePanel.TabIndex = 16;
            // 
            // TaskGroupBox
            // 
            this.TaskGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskGroupBox.Controls.Add(this.TaskActionLabel);
            this.TaskGroupBox.Controls.Add(this.TaskActionComboBox);
            this.TaskGroupBox.Controls.Add(this.TaskKBDPanel);
            this.TaskGroupBox.Controls.Add(this.TaskCMDPanel);
            this.TaskGroupBox.Controls.Add(this.TaskAHKPanel);
            this.TaskGroupBox.Enabled = false;
            this.TaskGroupBox.Location = new System.Drawing.Point(0, 0);
            this.TaskGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskGroupBox.Name = "TaskGroupBox";
            this.TaskGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.TaskGroupBox.Size = new System.Drawing.Size(500, 386);
            this.TaskGroupBox.TabIndex = 3;
            this.TaskGroupBox.TabStop = false;
            this.TaskGroupBox.Text = "Task Properties";
            this.TaskGroupBox.Visible = false;
            // 
            // TaskActionLabel
            // 
            this.TaskActionLabel.AutoSize = true;
            this.TaskActionLabel.Location = new System.Drawing.Point(6, 22);
            this.TaskActionLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TaskActionLabel.Name = "TaskActionLabel";
            this.TaskActionLabel.Size = new System.Drawing.Size(51, 17);
            this.TaskActionLabel.TabIndex = 0;
            this.TaskActionLabel.Text = "A&ction:";
            // 
            // TaskActionComboBox
            // 
            this.TaskActionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TaskActionComboBox.FormattingEnabled = true;
            this.TaskActionComboBox.Items.AddRange(new object[] {
            "Send keystrokes to window",
            "Run command line or program",
            "Run AutoHotKey script"});
            this.TaskActionComboBox.Location = new System.Drawing.Point(59, 19);
            this.TaskActionComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskActionComboBox.Name = "TaskActionComboBox";
            this.TaskActionComboBox.Size = new System.Drawing.Size(428, 24);
            this.TaskActionComboBox.TabIndex = 1;
            this.TaskActionComboBox.SelectedIndexChanged += new System.EventHandler(this.TaskActionComboBox_SelectedIndexChanged);
            // 
            // TaskKBDPanel
            // 
            this.TaskKBDPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDPanel.Controls.Add(this.TaskKBDTextPanel);
            this.TaskKBDPanel.Controls.Add(this.TaskKBDSettingsPanel);
            this.TaskKBDPanel.Controls.Add(this.TaskKBDShortcutPanel);
            this.TaskKBDPanel.Enabled = false;
            this.TaskKBDPanel.Location = new System.Drawing.Point(0, 50);
            this.TaskKBDPanel.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDPanel.Name = "TaskKBDPanel";
            this.TaskKBDPanel.Size = new System.Drawing.Size(499, 336);
            this.TaskKBDPanel.TabIndex = 3;
            this.TaskKBDPanel.Visible = false;
            // 
            // TaskKBDTextPanel
            // 
            this.TaskKBDTextPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDTextPanel.Controls.Add(this.TaskKBDSwitchAHKButton);
            this.TaskKBDTextPanel.Controls.Add(this.TaskKBDHelpButton2);
            this.TaskKBDTextPanel.Controls.Add(this.TaskKBDTextBox);
            this.TaskKBDTextPanel.Controls.Add(this.TaskKBDLabel);
            this.TaskKBDTextPanel.Location = new System.Drawing.Point(0, 0);
            this.TaskKBDTextPanel.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDTextPanel.Name = "TaskKBDTextPanel";
            this.TaskKBDTextPanel.Size = new System.Drawing.Size(499, 276);
            this.TaskKBDTextPanel.TabIndex = 1;
            // 
            // TaskKBDSwitchAHKButton
            // 
            this.TaskKBDSwitchAHKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TaskKBDSwitchAHKButton.Location = new System.Drawing.Point(10, 241);
            this.TaskKBDSwitchAHKButton.Margin = new System.Windows.Forms.Padding(4);
            this.TaskKBDSwitchAHKButton.Name = "TaskKBDSwitchAHKButton";
            this.TaskKBDSwitchAHKButton.Size = new System.Drawing.Size(219, 29);
            this.TaskKBDSwitchAHKButton.TabIndex = 2;
            this.TaskKBDSwitchAHKButton.Text = "Write AutoHotKey Script";
            this.TaskKBDSwitchAHKButton.UseVisualStyleBackColor = true;
            this.TaskKBDSwitchAHKButton.Click += new System.EventHandler(this.TaskKBDSwitchAHKButton_Click);
            // 
            // TaskKBDHelpButton2
            // 
            this.TaskKBDHelpButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDHelpButton2.Location = new System.Drawing.Point(394, 241);
            this.TaskKBDHelpButton2.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDHelpButton2.Name = "TaskKBDHelpButton2";
            this.TaskKBDHelpButton2.Size = new System.Drawing.Size(94, 29);
            this.TaskKBDHelpButton2.TabIndex = 3;
            this.TaskKBDHelpButton2.Text = "Help";
            this.TaskKBDHelpButton2.UseVisualStyleBackColor = true;
            this.TaskKBDHelpButton2.Click += new System.EventHandler(this.TaskKBDHelpButton_Click);
            // 
            // TaskKBDTextBox
            // 
            this.TaskKBDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDTextBox.Location = new System.Drawing.Point(10, 28);
            this.TaskKBDTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDTextBox.Multiline = true;
            this.TaskKBDTextBox.Name = "TaskKBDTextBox";
            this.TaskKBDTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TaskKBDTextBox.Size = new System.Drawing.Size(476, 208);
            this.TaskKBDTextBox.TabIndex = 1;
            this.TaskKBDTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_SelectAll);
            this.TaskKBDTextBox.Leave += new System.EventHandler(this.TaskKBDTextBox_Leave);
            // 
            // TaskKBDLabel
            // 
            this.TaskKBDLabel.AutoSize = true;
            this.TaskKBDLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskKBDLabel.Location = new System.Drawing.Point(8, 5);
            this.TaskKBDLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TaskKBDLabel.Name = "TaskKBDLabel";
            this.TaskKBDLabel.Size = new System.Drawing.Size(311, 17);
            this.TaskKBDLabel.TabIndex = 0;
            this.TaskKBDLabel.Text = "Type plain keyboar&d text as it will appear:";
            // 
            // TaskKBDSettingsPanel
            // 
            this.TaskKBDSettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDSettingsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.TaskKBDSettingsPanel.BackColor = System.Drawing.Color.Transparent;
            this.TaskKBDSettingsPanel.Controls.Add(this.TaskKBDIgnoreCurrentCheckBox);
            this.TaskKBDSettingsPanel.Controls.Add(this.TaskKBDIgnoreAbsentCheckBox);
            this.TaskKBDSettingsPanel.Controls.Add(this.TaskKBDNewCheckBox);
            this.TaskKBDSettingsPanel.Controls.Add(this.TaskKBDSwitchTextButton);
            this.TaskKBDSettingsPanel.Controls.Add(this.TaskKBDSwitchShortcutButton);
            this.TaskKBDSettingsPanel.Location = new System.Drawing.Point(0, 279);
            this.TaskKBDSettingsPanel.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDSettingsPanel.Name = "TaskKBDSettingsPanel";
            this.TaskKBDSettingsPanel.Size = new System.Drawing.Size(502, 78);
            this.TaskKBDSettingsPanel.TabIndex = 9;
            // 
            // TaskKBDIgnoreCurrentCheckBox
            // 
            this.TaskKBDIgnoreCurrentCheckBox.AutoSize = true;
            this.TaskKBDIgnoreCurrentCheckBox.Location = new System.Drawing.Point(237, 30);
            this.TaskKBDIgnoreCurrentCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDIgnoreCurrentCheckBox.Name = "TaskKBDIgnoreCurrentCheckBox";
            this.TaskKBDIgnoreCurrentCheckBox.Size = new System.Drawing.Size(263, 21);
            this.TaskKBDIgnoreCurrentCheckBox.TabIndex = 6;
            this.TaskKBDIgnoreCurrentCheckBox.Text = "Ignore if program is currently running";
            this.TaskKBDIgnoreCurrentCheckBox.UseVisualStyleBackColor = true;
            this.TaskKBDIgnoreCurrentCheckBox.CheckedChanged += new System.EventHandler(this.TaskKBDIgnoreCurrentCheckBox_CheckedChanged);
            // 
            // TaskKBDIgnoreAbsentCheckBox
            // 
            this.TaskKBDIgnoreAbsentCheckBox.AutoSize = true;
            this.TaskKBDIgnoreAbsentCheckBox.Location = new System.Drawing.Point(10, 30);
            this.TaskKBDIgnoreAbsentCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDIgnoreAbsentCheckBox.Name = "TaskKBDIgnoreAbsentCheckBox";
            this.TaskKBDIgnoreAbsentCheckBox.Size = new System.Drawing.Size(228, 21);
            this.TaskKBDIgnoreAbsentCheckBox.TabIndex = 5;
            this.TaskKBDIgnoreAbsentCheckBox.Text = "Ignore if program is not running";
            this.TaskKBDIgnoreAbsentCheckBox.UseVisualStyleBackColor = true;
            this.TaskKBDIgnoreAbsentCheckBox.CheckedChanged += new System.EventHandler(this.TaskKBDIgnoreAbsentCheckBox_CheckedChanged);
            // 
            // TaskKBDNewCheckBox
            // 
            this.TaskKBDNewCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDNewCheckBox.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.TaskKBDNewCheckBox.Location = new System.Drawing.Point(10, 4);
            this.TaskKBDNewCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDNewCheckBox.Name = "TaskKBDNewCheckBox";
            this.TaskKBDNewCheckBox.Size = new System.Drawing.Size(285, 21);
            this.TaskKBDNewCheckBox.TabIndex = 0;
            this.TaskKBDNewCheckBox.Text = "Open new window if program is running";
            this.TaskKBDNewCheckBox.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.TaskKBDNewCheckBox.UseVisualStyleBackColor = true;
            this.TaskKBDNewCheckBox.CheckedChanged += new System.EventHandler(this.TaskKBDNewCheckBox_CheckedChanged);
            // 
            // TaskKBDSwitchTextButton
            // 
            this.TaskKBDSwitchTextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDSwitchTextButton.BackColor = System.Drawing.SystemColors.Control;
            this.TaskKBDSwitchTextButton.Location = new System.Drawing.Point(312, 0);
            this.TaskKBDSwitchTextButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDSwitchTextButton.Name = "TaskKBDSwitchTextButton";
            this.TaskKBDSwitchTextButton.Size = new System.Drawing.Size(179, 29);
            this.TaskKBDSwitchTextButton.TabIndex = 4;
            this.TaskKBDSwitchTextButton.Text = "Switch to Text Mode";
            this.TaskKBDSwitchTextButton.UseVisualStyleBackColor = true;
            this.TaskKBDSwitchTextButton.Click += new System.EventHandler(this.TaskKBDSwitchTextButton_Click);
            // 
            // TaskKBDSwitchShortcutButton
            // 
            this.TaskKBDSwitchShortcutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDSwitchShortcutButton.BackColor = System.Drawing.SystemColors.Control;
            this.TaskKBDSwitchShortcutButton.Location = new System.Drawing.Point(301, 0);
            this.TaskKBDSwitchShortcutButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDSwitchShortcutButton.Name = "TaskKBDSwitchShortcutButton";
            this.TaskKBDSwitchShortcutButton.Size = new System.Drawing.Size(190, 29);
            this.TaskKBDSwitchShortcutButton.TabIndex = 3;
            this.TaskKBDSwitchShortcutButton.Text = "Switch to Shortcut Mode";
            this.TaskKBDSwitchShortcutButton.UseVisualStyleBackColor = true;
            this.TaskKBDSwitchShortcutButton.Click += new System.EventHandler(this.TaskKBDSwitchShortcutButton_Click);
            // 
            // TaskKBDShortcutPanel
            // 
            this.TaskKBDShortcutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDShortcutPanel.Controls.Add(this.TaskKBDShortcutClearButton);
            this.TaskKBDShortcutPanel.Controls.Add(this.TaskKBDKeyboardTextBox);
            this.TaskKBDShortcutPanel.Controls.Add(this.TaskKBDShortcutLabel);
            this.TaskKBDShortcutPanel.Controls.Add(this.TaskKBDHelpButton);
            this.TaskKBDShortcutPanel.Location = new System.Drawing.Point(0, 0);
            this.TaskKBDShortcutPanel.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDShortcutPanel.Name = "TaskKBDShortcutPanel";
            this.TaskKBDShortcutPanel.Size = new System.Drawing.Size(499, 90);
            this.TaskKBDShortcutPanel.TabIndex = 0;
            // 
            // TaskKBDShortcutClearButton
            // 
            this.TaskKBDShortcutClearButton.Location = new System.Drawing.Point(9, 60);
            this.TaskKBDShortcutClearButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDShortcutClearButton.Name = "TaskKBDShortcutClearButton";
            this.TaskKBDShortcutClearButton.Size = new System.Drawing.Size(80, 29);
            this.TaskKBDShortcutClearButton.TabIndex = 2;
            this.TaskKBDShortcutClearButton.Text = "Clear";
            this.TaskKBDShortcutClearButton.UseVisualStyleBackColor = true;
            this.TaskKBDShortcutClearButton.Click += new System.EventHandler(this.TaskKBDShortcutClearButton_Click);
            // 
            // TaskKBDKeyboardTextBox
            // 
            this.TaskKBDKeyboardTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDKeyboardTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskKBDKeyboardTextBox.Location = new System.Drawing.Point(10, 28);
            this.TaskKBDKeyboardTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDKeyboardTextBox.Name = "TaskKBDKeyboardTextBox";
            this.TaskKBDKeyboardTextBox.Size = new System.Drawing.Size(476, 27);
            this.TaskKBDKeyboardTextBox.TabIndex = 1;
            this.TaskKBDKeyboardTextBox.Leave += new System.EventHandler(this.TaskKBDKeyboardTextBox_Leave);
            // 
            // TaskKBDShortcutLabel
            // 
            this.TaskKBDShortcutLabel.AutoSize = true;
            this.TaskKBDShortcutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskKBDShortcutLabel.Location = new System.Drawing.Point(6, 5);
            this.TaskKBDShortcutLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TaskKBDShortcutLabel.Name = "TaskKBDShortcutLabel";
            this.TaskKBDShortcutLabel.Size = new System.Drawing.Size(388, 17);
            this.TaskKBDShortcutLabel.TabIndex = 0;
            this.TaskKBDShortcutLabel.Text = "Press keyboar&d shortcuts to be sent to the program:";
            // 
            // TaskKBDHelpButton
            // 
            this.TaskKBDHelpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskKBDHelpButton.Location = new System.Drawing.Point(394, 60);
            this.TaskKBDHelpButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskKBDHelpButton.Name = "TaskKBDHelpButton";
            this.TaskKBDHelpButton.Size = new System.Drawing.Size(94, 29);
            this.TaskKBDHelpButton.TabIndex = 3;
            this.TaskKBDHelpButton.Text = "Help";
            this.TaskKBDHelpButton.UseVisualStyleBackColor = true;
            this.TaskKBDHelpButton.Click += new System.EventHandler(this.TaskKBDHelpButton_Click);
            // 
            // TaskCMDPanel
            // 
            this.TaskCMDPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskCMDPanel.Controls.Add(this.TaskCMDHelpButton);
            this.TaskCMDPanel.Controls.Add(this.TaskCMDBrowseButton);
            this.TaskCMDPanel.Controls.Add(this.TaskCMDShowWindowCheckbox);
            this.TaskCMDPanel.Controls.Add(this.TaskCMDTextBox);
            this.TaskCMDPanel.Controls.Add(this.TaskCMDLabel);
            this.TaskCMDPanel.Enabled = false;
            this.TaskCMDPanel.Location = new System.Drawing.Point(0, 50);
            this.TaskCMDPanel.Margin = new System.Windows.Forms.Padding(2);
            this.TaskCMDPanel.Name = "TaskCMDPanel";
            this.TaskCMDPanel.Size = new System.Drawing.Size(499, 336);
            this.TaskCMDPanel.TabIndex = 1;
            this.TaskCMDPanel.Visible = false;
            // 
            // TaskCMDHelpButton
            // 
            this.TaskCMDHelpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskCMDHelpButton.Location = new System.Drawing.Point(394, 59);
            this.TaskCMDHelpButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskCMDHelpButton.Name = "TaskCMDHelpButton";
            this.TaskCMDHelpButton.Size = new System.Drawing.Size(94, 29);
            this.TaskCMDHelpButton.TabIndex = 4;
            this.TaskCMDHelpButton.Text = "Help";
            this.TaskCMDHelpButton.UseVisualStyleBackColor = true;
            this.TaskCMDHelpButton.Click += new System.EventHandler(this.TaskCMDHelpButton_Click);
            // 
            // TaskCMDBrowseButton
            // 
            this.TaskCMDBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskCMDBrowseButton.Location = new System.Drawing.Point(394, 25);
            this.TaskCMDBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskCMDBrowseButton.Name = "TaskCMDBrowseButton";
            this.TaskCMDBrowseButton.Size = new System.Drawing.Size(94, 29);
            this.TaskCMDBrowseButton.TabIndex = 2;
            this.TaskCMDBrowseButton.Text = "Browse";
            this.TaskCMDBrowseButton.UseVisualStyleBackColor = true;
            this.TaskCMDBrowseButton.Click += new System.EventHandler(this.TaskCMDBrowseButton_Click);
            // 
            // TaskCMDShowWindowCheckbox
            // 
            this.TaskCMDShowWindowCheckbox.AutoSize = true;
            this.TaskCMDShowWindowCheckbox.Location = new System.Drawing.Point(38, 62);
            this.TaskCMDShowWindowCheckbox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskCMDShowWindowCheckbox.Name = "TaskCMDShowWindowCheckbox";
            this.TaskCMDShowWindowCheckbox.Size = new System.Drawing.Size(222, 21);
            this.TaskCMDShowWindowCheckbox.TabIndex = 3;
            this.TaskCMDShowWindowCheckbox.Text = "Run in Command Line Window";
            this.TaskCMDShowWindowCheckbox.UseVisualStyleBackColor = true;
            this.TaskCMDShowWindowCheckbox.CheckedChanged += new System.EventHandler(this.TaskCMDShowWindowCheckbox_CheckedChanged);
            // 
            // TaskCMDTextBox
            // 
            this.TaskCMDTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskCMDTextBox.Location = new System.Drawing.Point(9, 26);
            this.TaskCMDTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskCMDTextBox.Name = "TaskCMDTextBox";
            this.TaskCMDTextBox.Size = new System.Drawing.Size(379, 22);
            this.TaskCMDTextBox.TabIndex = 1;
            this.TaskCMDTextBox.Leave += new System.EventHandler(this.TaskCMDTextBox_Leave);
            // 
            // TaskCMDLabel
            // 
            this.TaskCMDLabel.AutoSize = true;
            this.TaskCMDLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskCMDLabel.Location = new System.Drawing.Point(6, 4);
            this.TaskCMDLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TaskCMDLabel.Name = "TaskCMDLabel";
            this.TaskCMDLabel.Size = new System.Drawing.Size(119, 17);
            this.TaskCMDLabel.TabIndex = 0;
            this.TaskCMDLabel.Text = "Comman&d Line:";
            // 
            // TaskAHKPanel
            // 
            this.TaskAHKPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskAHKPanel.BackColor = System.Drawing.Color.Transparent;
            this.TaskAHKPanel.Controls.Add(this.TaskAHKHelpButton);
            this.TaskAHKPanel.Controls.Add(this.TaskAHKSaveCopyButton);
            this.TaskAHKPanel.Controls.Add(this.TaskAHKOpenExternalButton);
            this.TaskAHKPanel.Controls.Add(this.TaskAHKTextBox);
            this.TaskAHKPanel.Controls.Add(this.TaskAHKLabel);
            this.TaskAHKPanel.Enabled = false;
            this.TaskAHKPanel.Location = new System.Drawing.Point(0, 50);
            this.TaskAHKPanel.Margin = new System.Windows.Forms.Padding(2);
            this.TaskAHKPanel.Name = "TaskAHKPanel";
            this.TaskAHKPanel.Size = new System.Drawing.Size(499, 336);
            this.TaskAHKPanel.TabIndex = 2;
            this.TaskAHKPanel.Visible = false;
            // 
            // TaskAHKHelpButton
            // 
            this.TaskAHKHelpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TaskAHKHelpButton.Location = new System.Drawing.Point(9, 299);
            this.TaskAHKHelpButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskAHKHelpButton.Name = "TaskAHKHelpButton";
            this.TaskAHKHelpButton.Size = new System.Drawing.Size(94, 29);
            this.TaskAHKHelpButton.TabIndex = 2;
            this.TaskAHKHelpButton.Text = "Help";
            this.TaskAHKHelpButton.UseVisualStyleBackColor = true;
            this.TaskAHKHelpButton.Click += new System.EventHandler(this.TaskAHKHelpButton_Click);
            // 
            // TaskAHKSaveCopyButton
            // 
            this.TaskAHKSaveCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskAHKSaveCopyButton.Location = new System.Drawing.Point(170, 299);
            this.TaskAHKSaveCopyButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskAHKSaveCopyButton.Name = "TaskAHKSaveCopyButton";
            this.TaskAHKSaveCopyButton.Size = new System.Drawing.Size(150, 29);
            this.TaskAHKSaveCopyButton.TabIndex = 3;
            this.TaskAHKSaveCopyButton.Text = "Save Copy As...";
            this.TaskAHKSaveCopyButton.UseVisualStyleBackColor = true;
            this.TaskAHKSaveCopyButton.Click += new System.EventHandler(this.TaskAHKSaveCopyButton_Click);
            // 
            // TaskAHKOpenExternalButton
            // 
            this.TaskAHKOpenExternalButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskAHKOpenExternalButton.Location = new System.Drawing.Point(325, 299);
            this.TaskAHKOpenExternalButton.Margin = new System.Windows.Forms.Padding(2);
            this.TaskAHKOpenExternalButton.Name = "TaskAHKOpenExternalButton";
            this.TaskAHKOpenExternalButton.Size = new System.Drawing.Size(162, 29);
            this.TaskAHKOpenExternalButton.TabIndex = 4;
            this.TaskAHKOpenExternalButton.Text = "Copy to Clipboard";
            this.TaskAHKOpenExternalButton.UseVisualStyleBackColor = true;
            this.TaskAHKOpenExternalButton.Click += new System.EventHandler(this.TaskAHKOpenExternalButton_Click);
            // 
            // TaskAHKTextBox
            // 
            this.TaskAHKTextBox.AcceptsReturn = true;
            this.TaskAHKTextBox.AcceptsTab = true;
            this.TaskAHKTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TaskAHKTextBox.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskAHKTextBox.Location = new System.Drawing.Point(6, 28);
            this.TaskAHKTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.TaskAHKTextBox.Multiline = true;
            this.TaskAHKTextBox.Name = "TaskAHKTextBox";
            this.TaskAHKTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TaskAHKTextBox.Size = new System.Drawing.Size(480, 265);
            this.TaskAHKTextBox.TabIndex = 1;
            this.TaskAHKTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Text_SelectAll);
            this.TaskAHKTextBox.Leave += new System.EventHandler(this.TaskAHKTextBox_Leave);
            // 
            // TaskAHKLabel
            // 
            this.TaskAHKLabel.AutoSize = true;
            this.TaskAHKLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskAHKLabel.Location = new System.Drawing.Point(8, 5);
            this.TaskAHKLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TaskAHKLabel.Name = "TaskAHKLabel";
            this.TaskAHKLabel.Size = new System.Drawing.Size(220, 17);
            this.TaskAHKLabel.TabIndex = 0;
            this.TaskAHKLabel.Text = "AutoHotKey Comman&d Script:";
            // 
            // FileGroupBox
            // 
            this.FileGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FileGroupBox.Controls.Add(this.FileOpenApp);
            this.FileGroupBox.Controls.Add(this.FileOpenDefault);
            this.FileGroupBox.Controls.Add(this.FileBrowseButton);
            this.FileGroupBox.Controls.Add(this.FolderBrowseButton);
            this.FileGroupBox.Controls.Add(this.FileTextBox);
            this.FileGroupBox.Controls.Add(this.FileLabel);
            this.FileGroupBox.Enabled = false;
            this.FileGroupBox.Location = new System.Drawing.Point(0, 0);
            this.FileGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.FileGroupBox.Name = "FileGroupBox";
            this.FileGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.FileGroupBox.Size = new System.Drawing.Size(500, 408);
            this.FileGroupBox.TabIndex = 4;
            this.FileGroupBox.TabStop = false;
            this.FileGroupBox.Text = "File/Folder Shortcut Settings";
            this.FileGroupBox.Visible = false;
            // 
            // FileOpenApp
            // 
            this.FileOpenApp.AutoSize = true;
            this.FileOpenApp.Location = new System.Drawing.Point(38, 140);
            this.FileOpenApp.Margin = new System.Windows.Forms.Padding(2);
            this.FileOpenApp.Name = "FileOpenApp";
            this.FileOpenApp.Size = new System.Drawing.Size(193, 21);
            this.FileOpenApp.TabIndex = 5;
            this.FileOpenApp.Text = "Open the shortcut with {0}";
            this.FileOpenApp.UseVisualStyleBackColor = true;
            this.FileOpenApp.CheckedChanged += new System.EventHandler(this.FileOpenApp_CheckedChanged);
            // 
            // FileOpenDefault
            // 
            this.FileOpenDefault.AutoSize = true;
            this.FileOpenDefault.Checked = true;
            this.FileOpenDefault.Location = new System.Drawing.Point(38, 112);
            this.FileOpenDefault.Margin = new System.Windows.Forms.Padding(2);
            this.FileOpenDefault.Name = "FileOpenDefault";
            this.FileOpenDefault.Size = new System.Drawing.Size(293, 21);
            this.FileOpenDefault.TabIndex = 4;
            this.FileOpenDefault.TabStop = true;
            this.FileOpenDefault.Text = "Open the shortcut with its default program";
            this.FileOpenDefault.UseVisualStyleBackColor = true;
            // 
            // FileBrowseButton
            // 
            this.FileBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FileBrowseButton.Location = new System.Drawing.Point(361, 75);
            this.FileBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.FileBrowseButton.Name = "FileBrowseButton";
            this.FileBrowseButton.Size = new System.Drawing.Size(128, 29);
            this.FileBrowseButton.TabIndex = 3;
            this.FileBrowseButton.Text = "Browse File";
            this.FileBrowseButton.UseVisualStyleBackColor = true;
            this.FileBrowseButton.Click += new System.EventHandler(this.FileBrowseButton_Click);
            // 
            // FolderBrowseButton
            // 
            this.FolderBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FolderBrowseButton.Location = new System.Drawing.Point(188, 75);
            this.FolderBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.FolderBrowseButton.Name = "FolderBrowseButton";
            this.FolderBrowseButton.Size = new System.Drawing.Size(169, 29);
            this.FolderBrowseButton.TabIndex = 2;
            this.FolderBrowseButton.Text = "Browse Folder";
            this.FolderBrowseButton.UseVisualStyleBackColor = true;
            this.FolderBrowseButton.Click += new System.EventHandler(this.FolderBrowseButton_Click);
            // 
            // FileTextBox
            // 
            this.FileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.FileTextBox.Location = new System.Drawing.Point(9, 44);
            this.FileTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.FileTextBox.Name = "FileTextBox";
            this.FileTextBox.Size = new System.Drawing.Size(478, 22);
            this.FileTextBox.TabIndex = 1;
            this.FileTextBox.Leave += new System.EventHandler(this.FileTextBox_Leave);
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(6, 22);
            this.FileLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(66, 17);
            this.FileLabel.TabIndex = 0;
            this.FileLabel.Text = "&Location:";
            // 
            // ItemPanel
            // 
            this.ItemPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemPanel.Controls.Add(this.ItemIconPictureBox);
            this.ItemPanel.Controls.Add(this.ItemIconComboBox);
            this.ItemPanel.Controls.Add(this.ItemTypeComboBox);
            this.ItemPanel.Controls.Add(this.ItemTypeLabel);
            this.ItemPanel.Controls.Add(this.IconButtonBrowse);
            this.ItemPanel.Controls.Add(this.ItemIconLabel);
            this.ItemPanel.Controls.Add(this.ItemNameTextBox);
            this.ItemPanel.Controls.Add(this.ItemNameLabel);
            this.ItemPanel.Enabled = false;
            this.ItemPanel.Location = new System.Drawing.Point(309, 8);
            this.ItemPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ItemPanel.Name = "ItemPanel";
            this.ItemPanel.Size = new System.Drawing.Size(500, 100);
            this.ItemPanel.TabIndex = 2;
            // 
            // ItemIconPictureBox
            // 
            this.ItemIconPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.ItemIconPictureBox.Location = new System.Drawing.Point(59, 39);
            this.ItemIconPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.ItemIconPictureBox.Name = "ItemIconPictureBox";
            this.ItemIconPictureBox.Size = new System.Drawing.Size(20, 20);
            this.ItemIconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ItemIconPictureBox.TabIndex = 20;
            this.ItemIconPictureBox.TabStop = false;
            // 
            // ItemIconComboBox
            // 
            this.ItemIconComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemIconComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ItemIconComboBox.DropDownHeight = 212;
            this.ItemIconComboBox.ImageList = null;
            this.ItemIconComboBox.Indent = 0;
            this.ItemIconComboBox.IntegralHeight = false;
            this.ItemIconComboBox.ItemHeight = 15;
            imageComboBoxItem1.Font = null;
            imageComboBoxItem1.Image = "(none)";
            imageComboBoxItem1.ImageIndex = -1;
            imageComboBoxItem1.IndentLevel = 0;
            imageComboBoxItem1.Item = null;
            imageComboBoxItem1.Text = "Use program icon";
            imageComboBoxItem2.Font = null;
            imageComboBoxItem2.Image = "(none)";
            imageComboBoxItem2.ImageIndex = -1;
            imageComboBoxItem2.IndentLevel = 0;
            imageComboBoxItem2.Item = null;
            imageComboBoxItem2.Text = "Don\'t use an icon";
            imageComboBoxItem3.Font = null;
            imageComboBoxItem3.Image = "(none)";
            imageComboBoxItem3.ImageIndex = -1;
            imageComboBoxItem3.IndentLevel = 0;
            imageComboBoxItem3.Item = null;
            imageComboBoxItem3.Text = "Select icon from file (or type in path to file...)";
            this.ItemIconComboBox.Items.AddRange(new ImageComboBox.ImageComboBoxItem[] {
            imageComboBoxItem1,
            imageComboBoxItem2,
            imageComboBoxItem3});
            this.ItemIconComboBox.Location = new System.Drawing.Point(81, 36);
            this.ItemIconComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ItemIconComboBox.Name = "ItemIconComboBox";
            this.ItemIconComboBox.Size = new System.Drawing.Size(308, 21);
            this.ItemIconComboBox.TabIndex = 3;
            this.ItemIconComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIconComboBox_SelectedIndexChanged);
            this.ItemIconComboBox.Leave += new System.EventHandler(this.ItemIconComboBox_Leave);
            // 
            // ItemTypeComboBox
            // 
            this.ItemTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ItemTypeComboBox.FormattingEnabled = true;
            this.ItemTypeComboBox.Items.AddRange(new object[] {
            "Task",
            "File/Folder Shortcut",
            "Category",
            "Separator"});
            this.ItemTypeComboBox.Location = new System.Drawing.Point(59, 68);
            this.ItemTypeComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.ItemTypeComboBox.Name = "ItemTypeComboBox";
            this.ItemTypeComboBox.Size = new System.Drawing.Size(429, 24);
            this.ItemTypeComboBox.TabIndex = 6;
            this.ItemTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemTypeComboBox_SelectedIndexChanged);
            // 
            // ItemTypeLabel
            // 
            this.ItemTypeLabel.AutoSize = true;
            this.ItemTypeLabel.Location = new System.Drawing.Point(8, 71);
            this.ItemTypeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ItemTypeLabel.Name = "ItemTypeLabel";
            this.ItemTypeLabel.Size = new System.Drawing.Size(44, 17);
            this.ItemTypeLabel.TabIndex = 5;
            this.ItemTypeLabel.Text = "&Type:";
            // 
            // IconButtonBrowse
            // 
            this.IconButtonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IconButtonBrowse.Location = new System.Drawing.Point(395, 34);
            this.IconButtonBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.IconButtonBrowse.Name = "IconButtonBrowse";
            this.IconButtonBrowse.Size = new System.Drawing.Size(94, 29);
            this.IconButtonBrowse.TabIndex = 4;
            this.IconButtonBrowse.Text = "Browse";
            this.IconButtonBrowse.UseVisualStyleBackColor = true;
            this.IconButtonBrowse.Click += new System.EventHandler(this.IconButtonBrowse_Click);
            // 
            // ItemIconLabel
            // 
            this.ItemIconLabel.AutoSize = true;
            this.ItemIconLabel.Location = new System.Drawing.Point(8, 40);
            this.ItemIconLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ItemIconLabel.Name = "ItemIconLabel";
            this.ItemIconLabel.Size = new System.Drawing.Size(38, 17);
            this.ItemIconLabel.TabIndex = 2;
            this.ItemIconLabel.Text = "Ic&on:";
            // 
            // ItemNameTextBox
            // 
            this.ItemNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ItemNameTextBox.Location = new System.Drawing.Point(59, 4);
            this.ItemNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ItemNameTextBox.Name = "ItemNameTextBox";
            this.ItemNameTextBox.Size = new System.Drawing.Size(429, 22);
            this.ItemNameTextBox.TabIndex = 1;
            this.ItemNameTextBox.Enter += new System.EventHandler(this.ItemNameTextBox_Enter);
            this.ItemNameTextBox.Leave += new System.EventHandler(this.ItemNameTextBox_Leave);
            // 
            // ItemNameLabel
            // 
            this.ItemNameLabel.AutoSize = true;
            this.ItemNameLabel.Location = new System.Drawing.Point(8, 8);
            this.ItemNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ItemNameLabel.Name = "ItemNameLabel";
            this.ItemNameLabel.Size = new System.Drawing.Size(49, 17);
            this.ItemNameLabel.TabIndex = 0;
            this.ItemNameLabel.Text = "&Name:";
            // 
            // JumplistPanel
            // 
            this.JumplistPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.JumplistPanel.Controls.Add(this.JumplistAddButton);
            this.JumplistPanel.Controls.Add(this.JumplistDownButton);
            this.JumplistPanel.Controls.Add(this.JumplistUpButton);
            this.JumplistPanel.Controls.Add(this.JumplistDeleteButton);
            this.JumplistPanel.Controls.Add(this.JumplistListBox);
            this.JumplistPanel.Enabled = false;
            this.JumplistPanel.Location = new System.Drawing.Point(6, 32);
            this.JumplistPanel.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistPanel.Name = "JumplistPanel";
            this.JumplistPanel.Size = new System.Drawing.Size(298, 470);
            this.JumplistPanel.TabIndex = 1;
            // 
            // JumplistAddButton
            // 
            this.JumplistAddButton.ContextMenuStrip = this.AddButtonContextMenuStrip;
            this.JumplistAddButton.Image = ((System.Drawing.Image)(resources.GetObject("JumplistAddButton.Image")));
            this.JumplistAddButton.Location = new System.Drawing.Point(254, 35);
            this.JumplistAddButton.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistAddButton.Name = "JumplistAddButton";
            this.JumplistAddButton.Size = new System.Drawing.Size(40, 40);
            this.JumplistAddButton.TabIndex = 1;
            this.JumplistAddButton.UseVisualStyleBackColor = true;
            // 
            // JumplistDownButton
            // 
            this.JumplistDownButton.Image = ((System.Drawing.Image)(resources.GetObject("JumplistDownButton.Image")));
            this.JumplistDownButton.Location = new System.Drawing.Point(254, 228);
            this.JumplistDownButton.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistDownButton.Name = "JumplistDownButton";
            this.JumplistDownButton.Size = new System.Drawing.Size(40, 40);
            this.JumplistDownButton.TabIndex = 4;
            this.JumplistDownButton.UseVisualStyleBackColor = true;
            this.JumplistDownButton.Click += new System.EventHandler(this.JumplistDownButton_Click);
            // 
            // JumplistUpButton
            // 
            this.JumplistUpButton.Image = ((System.Drawing.Image)(resources.GetObject("JumplistUpButton.Image")));
            this.JumplistUpButton.Location = new System.Drawing.Point(254, 180);
            this.JumplistUpButton.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistUpButton.Name = "JumplistUpButton";
            this.JumplistUpButton.Size = new System.Drawing.Size(40, 40);
            this.JumplistUpButton.TabIndex = 3;
            this.JumplistUpButton.UseVisualStyleBackColor = true;
            this.JumplistUpButton.Click += new System.EventHandler(this.JumplistUpButton_Click);
            // 
            // JumplistDeleteButton
            // 
            this.JumplistDeleteButton.ContextMenuStrip = this.DeleteButtonContextMenuStrip;
            this.JumplistDeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("JumplistDeleteButton.Image")));
            this.JumplistDeleteButton.Location = new System.Drawing.Point(254, 82);
            this.JumplistDeleteButton.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistDeleteButton.Name = "JumplistDeleteButton";
            this.JumplistDeleteButton.Size = new System.Drawing.Size(40, 40);
            this.JumplistDeleteButton.TabIndex = 2;
            this.JumplistDeleteButton.UseVisualStyleBackColor = true;
            // 
            // DeleteButtonContextMenuStrip
            // 
            this.DeleteButtonContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.DeleteButtonContextMenuStrip.Name = "DeleteButtonContextMenuStrip";
            this.DeleteButtonContextMenuStrip.Size = new System.Drawing.Size(118, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.JumplistDeleteButton_Click);
            // 
            // JumplistListBox
            // 
            this.JumplistListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.JumplistListBox.FormattingEnabled = true;
            this.JumplistListBox.ItemHeight = 16;
            this.JumplistListBox.Location = new System.Drawing.Point(0, 6);
            this.JumplistListBox.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistListBox.Name = "JumplistListBox";
            this.JumplistListBox.Size = new System.Drawing.Size(250, 404);
            this.JumplistListBox.TabIndex = 0;
            this.JumplistListBox.SelectedIndexChanged += new System.EventHandler(this.JumplistListBox_SelectedIndexChanged);
            this.JumplistListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.JumplistListBox_KeyDown);
            // 
            // JumplistEnableCheckBox
            // 
            this.JumplistEnableCheckBox.AutoSize = true;
            this.JumplistEnableCheckBox.Location = new System.Drawing.Point(9, 8);
            this.JumplistEnableCheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.JumplistEnableCheckBox.Name = "JumplistEnableCheckBox";
            this.JumplistEnableCheckBox.Size = new System.Drawing.Size(180, 21);
            this.JumplistEnableCheckBox.TabIndex = 0;
            this.JumplistEnableCheckBox.Text = "Enable Custom Jumplist";
            this.JumplistEnableCheckBox.UseVisualStyleBackColor = true;
            this.JumplistEnableCheckBox.Visible = false;
            this.JumplistEnableCheckBox.CheckedChanged += new System.EventHandler(this.JumplistEnableCheckBox_CheckedChanged);
            // 
            // JumplistHelpText
            // 
            this.JumplistHelpText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.JumplistHelpText.Location = new System.Drawing.Point(309, 115);
            this.JumplistHelpText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.JumplistHelpText.Name = "JumplistHelpText";
            this.JumplistHelpText.Size = new System.Drawing.Size(499, 388);
            this.JumplistHelpText.TabIndex = 0;
            this.JumplistHelpText.Text = "Add or select jumplist items to the left.\r\nOptions will appear that allow you to " +
                "customize the jumplist.";
            this.JumplistHelpText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TabProgramSettings
            // 
            this.TabProgramSettings.Controls.Add(this.ProgramSettingsPanel);
            this.TabProgramSettings.Location = new System.Drawing.Point(4, 25);
            this.TabProgramSettings.Margin = new System.Windows.Forms.Padding(2);
            this.TabProgramSettings.Name = "TabProgramSettings";
            this.TabProgramSettings.Padding = new System.Windows.Forms.Padding(2);
            this.TabProgramSettings.Size = new System.Drawing.Size(812, 507);
            this.TabProgramSettings.TabIndex = 1;
            this.TabProgramSettings.Text = "Program Settings";
            this.TabProgramSettings.UseVisualStyleBackColor = true;
            // 
            // ProgramSettingsPanel
            // 
            this.ProgramSettingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramSettingsPanel.Controls.Add(this.ProgramNewOKButton);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramWindowClassButton);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramWindowClassLabel);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramSettingsLabel);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramPathTextBox);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramPathLabel);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramNameTextBox);
            this.ProgramSettingsPanel.Controls.Add(this.ProgramNameLabel);
            this.ProgramSettingsPanel.Location = new System.Drawing.Point(0, 0);
            this.ProgramSettingsPanel.Margin = new System.Windows.Forms.Padding(2);
            this.ProgramSettingsPanel.Name = "ProgramSettingsPanel";
            this.ProgramSettingsPanel.Size = new System.Drawing.Size(801, 202);
            this.ProgramSettingsPanel.TabIndex = 0;
            // 
            // ProgramNewOKButton
            // 
            this.ProgramNewOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramNewOKButton.Location = new System.Drawing.Point(704, 144);
            this.ProgramNewOKButton.Margin = new System.Windows.Forms.Padding(2);
            this.ProgramNewOKButton.Name = "ProgramNewOKButton";
            this.ProgramNewOKButton.Size = new System.Drawing.Size(94, 29);
            this.ProgramNewOKButton.TabIndex = 7;
            this.ProgramNewOKButton.Text = "&OK";
            this.ProgramNewOKButton.UseVisualStyleBackColor = true;
            this.ProgramNewOKButton.Click += new System.EventHandler(this.ProgramNewOKButton_Click);
            // 
            // ProgramWindowClassButton
            // 
            this.ProgramWindowClassButton.Location = new System.Drawing.Point(8, 144);
            this.ProgramWindowClassButton.Margin = new System.Windows.Forms.Padding(2);
            this.ProgramWindowClassButton.Name = "ProgramWindowClassButton";
            this.ProgramWindowClassButton.Size = new System.Drawing.Size(190, 29);
            this.ProgramWindowClassButton.TabIndex = 6;
            this.ProgramWindowClassButton.Text = "&Select Main Window";
            this.ProgramWindowClassButton.UseVisualStyleBackColor = true;
            this.ProgramWindowClassButton.Click += new System.EventHandler(this.ProgramWindowClassButton_Click);
            // 
            // ProgramWindowClassLabel
            // 
            this.ProgramWindowClassLabel.AutoSize = true;
            this.ProgramWindowClassLabel.Location = new System.Drawing.Point(8, 124);
            this.ProgramWindowClassLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProgramWindowClassLabel.Name = "ProgramWindowClassLabel";
            this.ProgramWindowClassLabel.Size = new System.Drawing.Size(401, 17);
            this.ProgramWindowClassLabel.TabIndex = 5;
            this.ProgramWindowClassLabel.Text = "Re-select the program window, if settings need to be changed:";
            // 
            // ProgramSettingsLabel
            // 
            this.ProgramSettingsLabel.AutoSize = true;
            this.ProgramSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgramSettingsLabel.Location = new System.Drawing.Point(2, 2);
            this.ProgramSettingsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProgramSettingsLabel.Name = "ProgramSettingsLabel";
            this.ProgramSettingsLabel.Size = new System.Drawing.Size(398, 20);
            this.ProgramSettingsLabel.TabIndex = 0;
            this.ProgramSettingsLabel.Text = "Verify the settings below for the new program:";
            // 
            // ProgramPathTextBox
            // 
            this.ProgramPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramPathTextBox.Location = new System.Drawing.Point(8, 99);
            this.ProgramPathTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ProgramPathTextBox.Name = "ProgramPathTextBox";
            this.ProgramPathTextBox.Size = new System.Drawing.Size(790, 22);
            this.ProgramPathTextBox.TabIndex = 4;
            this.ProgramPathTextBox.Validated += new System.EventHandler(this.ProgramPathTextBox_Leave);
            // 
            // ProgramPathLabel
            // 
            this.ProgramPathLabel.AutoSize = true;
            this.ProgramPathLabel.Location = new System.Drawing.Point(8, 78);
            this.ProgramPathLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProgramPathLabel.Name = "ProgramPathLabel";
            this.ProgramPathLabel.Size = new System.Drawing.Size(124, 17);
            this.ProgramPathLabel.TabIndex = 3;
            this.ProgramPathLabel.Text = "Program &Location:";
            // 
            // ProgramNameTextBox
            // 
            this.ProgramNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgramNameTextBox.Location = new System.Drawing.Point(8, 52);
            this.ProgramNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.ProgramNameTextBox.Name = "ProgramNameTextBox";
            this.ProgramNameTextBox.Size = new System.Drawing.Size(790, 22);
            this.ProgramNameTextBox.TabIndex = 2;
            this.ProgramNameTextBox.Leave += new System.EventHandler(this.ProgramNameTextBox_Leave);
            // 
            // ProgramNameLabel
            // 
            this.ProgramNameLabel.AutoSize = true;
            this.ProgramNameLabel.Location = new System.Drawing.Point(8, 32);
            this.ProgramNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ProgramNameLabel.Name = "ProgramNameLabel";
            this.ProgramNameLabel.Size = new System.Drawing.Size(479, 17);
            this.ProgramNameLabel.TabIndex = 1;
            this.ProgramNameLabel.Text = "Program &Title: (Change if this text does NOT appear on program\'s title bar)";
            // 
            // TabControl
            // 
            this.TabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl.Controls.Add(this.TabProgramSettings);
            this.TabControl.Controls.Add(this.TabJumplist);
            this.TabControl.Location = new System.Drawing.Point(0, 29);
            this.TabControl.Margin = new System.Windows.Forms.Padding(2);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedIndex = 0;
            this.TabControl.Size = new System.Drawing.Size(820, 536);
            this.TabControl.TabIndex = 1;
            this.TabControl.SelectedIndexChanged += new System.EventHandler(this.TabControl_SelectedIndexChanged);
            this.TabControl.SizeChanged += new System.EventHandler(this.TabControl_SizeChanged);
            // 
            // visitTheOfficialWebsiteToolStripMenuItem
            // 
            this.visitTheOfficialWebsiteToolStripMenuItem.Name = "visitTheOfficialWebsiteToolStripMenuItem";
            this.visitTheOfficialWebsiteToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.visitTheOfficialWebsiteToolStripMenuItem.Text = "&Visit the Official Website";
            this.visitTheOfficialWebsiteToolStripMenuItem.Click += new System.EventHandler(this.visitTheOfficialWebsiteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(261, 6);
            // 
            // Primary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(820, 590);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.StatusBar);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Primary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Jumplist Extender";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Primary_FormClosing);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.StatusBar.ResumeLayout(false);
            this.StatusBar.PerformLayout();
            this.AddButtonContextMenuStrip.ResumeLayout(false);
            this.TabJumplist.ResumeLayout(false);
            this.TabJumplist.PerformLayout();
            this.ItemTypePanel.ResumeLayout(false);
            this.TaskGroupBox.ResumeLayout(false);
            this.TaskGroupBox.PerformLayout();
            this.TaskKBDPanel.ResumeLayout(false);
            this.TaskKBDTextPanel.ResumeLayout(false);
            this.TaskKBDTextPanel.PerformLayout();
            this.TaskKBDSettingsPanel.ResumeLayout(false);
            this.TaskKBDSettingsPanel.PerformLayout();
            this.TaskKBDShortcutPanel.ResumeLayout(false);
            this.TaskKBDShortcutPanel.PerformLayout();
            this.TaskCMDPanel.ResumeLayout(false);
            this.TaskCMDPanel.PerformLayout();
            this.TaskAHKPanel.ResumeLayout(false);
            this.TaskAHKPanel.PerformLayout();
            this.FileGroupBox.ResumeLayout(false);
            this.FileGroupBox.PerformLayout();
            this.ItemPanel.ResumeLayout(false);
            this.ItemPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ItemIconPictureBox)).EndInit();
            this.JumplistPanel.ResumeLayout(false);
            this.DeleteButtonContextMenuStrip.ResumeLayout(false);
            this.TabProgramSettings.ResumeLayout(false);
            this.ProgramSettingsPanel.ResumeLayout(false);
            this.ProgramSettingsPanel.PerformLayout();
            this.TabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFileMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem MenuToolsMenu;
        private System.Windows.Forms.ToolStripMenuItem MenuToolsPreferences;
        private System.Windows.Forms.ToolStripMenuItem MenuToolsAbout;
        private System.Windows.Forms.ToolStripMenuItem MenuFileExit;
        private System.Windows.Forms.ToolStripMenuItem MenuFileSave;
        private System.Windows.Forms.ToolStripMenuItem MenuFileExport;
        private System.Windows.Forms.ToolStripSeparator MenuFileSeparator1;
        private System.Windows.Forms.ToolStripSeparator MenuFileSeparator2;
        private System.Windows.Forms.ToolStripSeparator MenuToolsSeparator1;
        private System.Windows.Forms.StatusStrip StatusBar;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAndApplyToTaskbarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importProgramSettingsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip AddButtonContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem newTaskToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newSeparatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newCategoryToolStripMenuItem;
        private System.Windows.Forms.TabPage TabJumplist;
        private System.Windows.Forms.Panel ItemTypePanel;
        private System.Windows.Forms.GroupBox FileGroupBox;
        private System.Windows.Forms.Button FileBrowseButton;
        private System.Windows.Forms.Button FolderBrowseButton;
        private System.Windows.Forms.TextBox FileTextBox;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.GroupBox TaskGroupBox;
        private System.Windows.Forms.Label TaskActionLabel;
        private System.Windows.Forms.ComboBox TaskActionComboBox;
        private System.Windows.Forms.Panel TaskAHKPanel;
        private System.Windows.Forms.Button TaskAHKHelpButton;
        private System.Windows.Forms.Button TaskAHKSaveCopyButton;
        private System.Windows.Forms.Button TaskAHKOpenExternalButton;
        private System.Windows.Forms.TextBox TaskAHKTextBox;
        private System.Windows.Forms.Label TaskAHKLabel;
        private System.Windows.Forms.Panel TaskKBDPanel;
        private System.Windows.Forms.Button TaskKBDHelpButton;
        private System.Windows.Forms.CheckBox TaskKBDNewCheckBox;
        private System.Windows.Forms.TextBox TaskKBDTextBox;
        private System.Windows.Forms.Label TaskKBDLabel;
        private System.Windows.Forms.Panel TaskCMDPanel;
        private System.Windows.Forms.Button TaskCMDBrowseButton;
        private System.Windows.Forms.CheckBox TaskCMDShowWindowCheckbox;
        private System.Windows.Forms.TextBox TaskCMDTextBox;
        private System.Windows.Forms.Label TaskCMDLabel;
        private System.Windows.Forms.Panel ItemPanel;
        private System.Windows.Forms.ComboBox ItemTypeComboBox;
        private System.Windows.Forms.Label ItemTypeLabel;
        private System.Windows.Forms.Button IconButtonBrowse;
        private System.Windows.Forms.Label ItemIconLabel;
        private System.Windows.Forms.TextBox ItemNameTextBox;
        private System.Windows.Forms.Label ItemNameLabel;
        private System.Windows.Forms.Panel JumplistPanel;
        private T7ECommon.MenuButton JumplistAddButton;
        private System.Windows.Forms.Button JumplistDownButton;
        private System.Windows.Forms.Button JumplistUpButton;
        private T7ECommon.MenuButton JumplistDeleteButton;
        public System.Windows.Forms.ListBox JumplistListBox;
        private System.Windows.Forms.CheckBox JumplistEnableCheckBox;
        private System.Windows.Forms.Label JumplistHelpText;
        private System.Windows.Forms.TabPage TabProgramSettings;
        private System.Windows.Forms.Button ProgramNewOKButton;
        private System.Windows.Forms.Panel ProgramSettingsPanel;
        private System.Windows.Forms.Button ProgramWindowClassButton;
        private System.Windows.Forms.Label ProgramSettingsLabel;
        private System.Windows.Forms.TextBox ProgramPathTextBox;
        private System.Windows.Forms.Label ProgramPathLabel;
        private System.Windows.Forms.TextBox ProgramNameTextBox;
        private System.Windows.Forms.Label ProgramNameLabel;
        private System.Windows.Forms.TabControl TabControl;
        private ImageComboBox.ImageComboBox ItemIconComboBox;
        private System.Windows.Forms.ContextMenuStrip DeleteButtonContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.Button TaskCMDHelpButton;
        private System.Windows.Forms.PictureBox ItemIconPictureBox;
        private System.Windows.Forms.ToolStripStatusLabel ToolBarStatusLabel;
        private System.Windows.Forms.RadioButton FileOpenApp;
        private System.Windows.Forms.RadioButton FileOpenDefault;
        private System.Windows.Forms.Label TaskKBDShortcutLabel;
        private System.Windows.Forms.Button TaskKBDSwitchTextButton;
        private System.Windows.Forms.Button TaskKBDSwitchShortcutButton;
        private T7ECommon.KeyboardTextBox TaskKBDKeyboardTextBox;
        private System.Windows.Forms.Panel TaskKBDTextPanel;
        private System.Windows.Forms.Panel TaskKBDShortcutPanel;
        private System.Windows.Forms.Button TaskKBDHelpButton2;
        private System.Windows.Forms.Button TaskKBDShortcutClearButton;
        private System.Windows.Forms.Panel TaskKBDSettingsPanel;
        private System.Windows.Forms.Label ProgramWindowClassLabel;
        private System.Windows.Forms.ToolStripMenuItem keyboardShortcutHelpToolStripMenuItem;
        private System.Windows.Forms.CheckBox TaskKBDIgnoreCurrentCheckBox;
        private System.Windows.Forms.CheckBox TaskKBDIgnoreAbsentCheckBox;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.Button TaskKBDSwitchAHKButton;
        private System.Windows.Forms.ToolStripMenuItem updateToVersion0ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visitTheOfficialWebsiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

