using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;

using vbAccelerator.Components.Shell;
using vbAccelerator.Controls.TextBox;
using vbAccelerator.Components.Menu;
using vbAccelerator.Forms.IconPicker;

namespace ShellLinkTester
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmShellLinkTester : System.Windows.Forms.Form
	{
		private ShellLink link = new ShellLink();
		private IconMainMenu mnuMain;
		private IconMenuItem mnuFileTop;
		private IconMenuItem mnuNew;
		private IconMenuItem mnuOpen;
		private IconMenuItem mnuClose;
		private IconMenuItem mnuFileSep1;
		private IconMenuItem mnuSave;
		private IconMenuItem mnuSaveAs;
		private IconMenuItem mnuFileSep2;
		private IconMenuItem mnuExit;
		private IconMenuItem mnuHelpTop;
		private IconMenuItem mnuAbout;
		private System.Windows.Forms.Label lblShortCut;
		private System.Windows.Forms.Label lblTarget;
		private AutoCompleteTextBox txtTarget;
		private System.Windows.Forms.TextBox txtArguments;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblArguments;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Button btnPick;
		private System.Windows.Forms.GroupBox fraIcon;
		private System.Windows.Forms.Label lblIconIndex;
		private System.Windows.Forms.Label lblIconFile;
		private System.Windows.Forms.TextBox txtIconFile;
		private System.Windows.Forms.TextBox txtIconIndex;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.PictureBox picSmallIcon;
		private System.Windows.Forms.Button btnChooseIcon;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmShellLinkTester()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set up the form:
			txtTarget.AutoCompleteFlags = AutoCompleteTextBox.SHAutoCompleteFlags.SHACF_FILESYS_ONLY;
			mnuNew.Click += new System.EventHandler(this.mnu_Click);
			mnuOpen.Click += new System.EventHandler(this.mnu_Click);
			mnuClose.Click += new System.EventHandler(this.mnu_Click);
			mnuSave.Click += new System.EventHandler(this.mnu_Click);
			mnuSaveAs.Click += new System.EventHandler(this.mnu_Click);
			mnuExit.Click += new System.EventHandler(this.mnu_Click);
			mnuAbout.Click += new System.EventHandler(this.mnu_Click);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmShellLinkTester));
			this.mnuMain = new IconMainMenu();
			this.mnuFileTop = new IconMenuItem();
			this.mnuNew = new IconMenuItem();
			this.mnuOpen = new IconMenuItem();
			this.mnuClose = new IconMenuItem();
			this.mnuFileSep1 = new IconMenuItem();
			this.mnuSave = new IconMenuItem();
			this.mnuSaveAs = new IconMenuItem();
			this.mnuFileSep2 = new IconMenuItem();
			this.mnuExit = new IconMenuItem();
			this.mnuHelpTop = new IconMenuItem();
			this.mnuAbout = new IconMenuItem();
			this.lblShortCut = new System.Windows.Forms.Label();
			this.lblTarget = new System.Windows.Forms.Label();
			this.txtTarget = new vbAccelerator.Controls.TextBox.AutoCompleteTextBox();
			this.txtArguments = new System.Windows.Forms.TextBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.lblArguments = new System.Windows.Forms.Label();
			this.lblDescription = new System.Windows.Forms.Label();
			this.btnPick = new System.Windows.Forms.Button();
			this.fraIcon = new System.Windows.Forms.GroupBox();
			this.btnChooseIcon = new System.Windows.Forms.Button();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.picSmallIcon = new System.Windows.Forms.PictureBox();
			this.txtIconIndex = new System.Windows.Forms.TextBox();
			this.txtIconFile = new System.Windows.Forms.TextBox();
			this.lblIconIndex = new System.Windows.Forms.Label();
			this.lblIconFile = new System.Windows.Forms.Label();
			this.fraIcon.SuspendLayout();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuFileTop,
																					this.mnuHelpTop});
			// 
			// mnuFileTop
			// 
			this.mnuFileTop.Index = 0;
			this.mnuFileTop.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuNew,
																					   this.mnuOpen,
																					   this.mnuClose,
																					   this.mnuFileSep1,
																					   this.mnuSave,
																					   this.mnuSaveAs,
																					   this.mnuFileSep2,
																					   this.mnuExit});
			this.mnuFileTop.Text = "&File";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mnuNew.Text = "&New";
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 1;
			this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuOpen.Text = "&Open...";
			// 
			// mnuClose
			// 
			this.mnuClose.Index = 2;
			this.mnuClose.Text = "&Close";
			// 
			// mnuFileSep1
			// 
			this.mnuFileSep1.Index = 3;
			this.mnuFileSep1.Text = "-";
			// 
			// mnuSave
			// 
			this.mnuSave.Index = 4;
			this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuSave.Text = "&Save";
			// 
			// mnuSaveAs
			// 
			this.mnuSaveAs.Index = 5;
			this.mnuSaveAs.Text = "Save &As...";
			// 
			// mnuFileSep2
			// 
			this.mnuFileSep2.Index = 6;
			this.mnuFileSep2.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 7;
			this.mnuExit.Text = "E&xit";
			// 
			// mnuHelpTop
			// 
			this.mnuHelpTop.Index = 1;
			this.mnuHelpTop.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mnuAbout});
			this.mnuHelpTop.Text = "&Help";
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 0;
			this.mnuAbout.Text = "&About...";
			// 
			// lblShortCut
			// 
			this.lblShortCut.BackColor = System.Drawing.SystemColors.ControlDark;
			this.lblShortCut.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblShortCut.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.lblShortCut.Location = new System.Drawing.Point(4, 4);
			this.lblShortCut.Name = "lblShortCut";
			this.lblShortCut.Size = new System.Drawing.Size(372, 24);
			this.lblShortCut.TabIndex = 0;
			this.lblShortCut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTarget
			// 
			this.lblTarget.Location = new System.Drawing.Point(4, 40);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(68, 20);
			this.lblTarget.TabIndex = 1;
			this.lblTarget.Text = "&Target:";
			// 
			// txtTarget
			// 
			this.txtTarget.AutoCompleteFlags = vbAccelerator.Controls.TextBox.AutoCompleteTextBox.SHAutoCompleteFlags.SHACF_FILESYS_ONLY;
			this.txtTarget.Location = new System.Drawing.Point(72, 36);
			this.txtTarget.Name = "txtTarget";
			this.txtTarget.Size = new System.Drawing.Size(280, 21);
			this.txtTarget.TabIndex = 2;
			this.txtTarget.Text = "";
			// 
			// txtArguments
			// 
			this.txtArguments.Location = new System.Drawing.Point(72, 60);
			this.txtArguments.Name = "txtArguments";
			this.txtArguments.Size = new System.Drawing.Size(280, 21);
			this.txtArguments.TabIndex = 3;
			this.txtArguments.Text = "";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(72, 112);
			this.txtDescription.MaxLength = 1024;
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtDescription.Size = new System.Drawing.Size(280, 80);
			this.txtDescription.TabIndex = 4;
			this.txtDescription.Text = "";
			// 
			// lblArguments
			// 
			this.lblArguments.Location = new System.Drawing.Point(4, 64);
			this.lblArguments.Name = "lblArguments";
			this.lblArguments.Size = new System.Drawing.Size(68, 23);
			this.lblArguments.TabIndex = 9;
			this.lblArguments.Text = "Arguments:";
			// 
			// lblDescription
			// 
			this.lblDescription.Location = new System.Drawing.Point(4, 112);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(68, 23);
			this.lblDescription.TabIndex = 10;
			this.lblDescription.Text = "Description:";
			// 
			// btnPick
			// 
			this.btnPick.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnPick.Location = new System.Drawing.Point(356, 36);
			this.btnPick.Name = "btnPick";
			this.btnPick.Size = new System.Drawing.Size(20, 20);
			this.btnPick.TabIndex = 13;
			this.btnPick.Text = "...";
			this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
			// 
			// fraIcon
			// 
			this.fraIcon.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.fraIcon.Controls.AddRange(new System.Windows.Forms.Control[] {
																				  this.btnChooseIcon,
																				  this.picIcon,
																				  this.picSmallIcon,
																				  this.txtIconIndex,
																				  this.txtIconFile,
																				  this.lblIconIndex,
																				  this.lblIconFile});
			this.fraIcon.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.fraIcon.Location = new System.Drawing.Point(8, 200);
			this.fraIcon.Name = "fraIcon";
			this.fraIcon.Size = new System.Drawing.Size(344, 156);
			this.fraIcon.TabIndex = 14;
			this.fraIcon.TabStop = false;
			this.fraIcon.Text = "Icon:";
			// 
			// btnChooseIcon
			// 
			this.btnChooseIcon.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChooseIcon.Location = new System.Drawing.Point(64, 116);
			this.btnChooseIcon.Name = "btnChooseIcon";
			this.btnChooseIcon.Size = new System.Drawing.Size(76, 32);
			this.btnChooseIcon.TabIndex = 19;
			this.btnChooseIcon.Text = "Pick...";
			this.btnChooseIcon.Click += new System.EventHandler(this.btnChooseIcon_Click);
			// 
			// picIcon
			// 
			this.picIcon.Location = new System.Drawing.Point(68, 80);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(32, 32);
			this.picIcon.TabIndex = 18;
			this.picIcon.TabStop = false;
			// 
			// picSmallIcon
			// 
			this.picSmallIcon.Location = new System.Drawing.Point(104, 80);
			this.picSmallIcon.Name = "picSmallIcon";
			this.picSmallIcon.Size = new System.Drawing.Size(32, 32);
			this.picSmallIcon.TabIndex = 17;
			this.picSmallIcon.TabStop = false;
			// 
			// txtIconIndex
			// 
			this.txtIconIndex.BackColor = System.Drawing.SystemColors.Control;
			this.txtIconIndex.Location = new System.Drawing.Point(64, 52);
			this.txtIconIndex.Name = "txtIconIndex";
			this.txtIconIndex.ReadOnly = true;
			this.txtIconIndex.Size = new System.Drawing.Size(276, 21);
			this.txtIconIndex.TabIndex = 16;
			this.txtIconIndex.Text = "";
			// 
			// txtIconFile
			// 
			this.txtIconFile.BackColor = System.Drawing.SystemColors.Control;
			this.txtIconFile.Location = new System.Drawing.Point(64, 24);
			this.txtIconFile.Name = "txtIconFile";
			this.txtIconFile.ReadOnly = true;
			this.txtIconFile.Size = new System.Drawing.Size(276, 21);
			this.txtIconFile.TabIndex = 15;
			this.txtIconFile.Text = "";
			// 
			// lblIconIndex
			// 
			this.lblIconIndex.Location = new System.Drawing.Point(4, 52);
			this.lblIconIndex.Name = "lblIconIndex";
			this.lblIconIndex.TabIndex = 14;
			this.lblIconIndex.Text = "Icon Index:";
			// 
			// lblIconFile
			// 
			this.lblIconFile.Location = new System.Drawing.Point(4, 24);
			this.lblIconFile.Name = "lblIconFile";
			this.lblIconFile.Size = new System.Drawing.Size(60, 23);
			this.lblIconFile.TabIndex = 13;
			this.lblIconFile.Text = "Icon File:";
			// 
			// frmShellLinkTester
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(380, 365);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.fraIcon,
																		  this.btnPick,
																		  this.lblDescription,
																		  this.lblArguments,
																		  this.txtDescription,
																		  this.txtArguments,
																		  this.txtTarget,
																		  this.lblTarget,
																		  this.lblShortCut});
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mnuMain;
			this.Name = "frmShellLinkTester";
			this.Text = "Shell Link Tester";
			this.Load += new System.EventHandler(this.frmShellLinkTester_Load);
			this.fraIcon.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmShellLinkTester());
		}

		private void openShortcut()
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "Shortcuts (*.lnk)|*.LNK|All Files (*.*)|*.*";
			o.DefaultExt = "LNK";
			o.CheckFileExists = true;
			o.CheckPathExists = true;
			if (o.ShowDialog() == DialogResult.OK)
			{
				link.Open(o.FileName);
				showDetails();
			}
		}

		private void showDetails()
		{
			// show the link's details:
			lblShortCut.Text = System.IO.Path.GetFileName(link.ShortCutFile);
				
			txtTarget.Text = link.Target;
			txtArguments.Text = link.Arguments;
			txtDescription.Text = link.Description;
			txtIconFile.Text = link.IconPath;
			txtIconIndex.Text = (link.IconPath.Length > 0 ? link.IconIndex.ToString() : "");
			System.Drawing.Icon linkIcon = link.LargeIcon;
			if (linkIcon != null)
			{
				picIcon.Image = linkIcon.ToBitmap();
				linkIcon.Dispose();
			}
			else
			{
				picIcon.Image = null;
			}
			linkIcon = link.SmallIcon;
			if (linkIcon != null)
			{
				picSmallIcon.Image = linkIcon.ToBitmap();
				linkIcon.Dispose();
			}
			else
			{
				picSmallIcon.Image = null;
			}

		}

		private void saveShortcutAs()
		{
			SaveFileDialog s = new SaveFileDialog();
			s.Filter = "Shortcut Files (*.lnk)|*.lnk|All Files (*.*)|*.*";
			s.FilterIndex = 1;
			s.DefaultExt = "LNK";
			s.CheckPathExists = true;
			if (s.ShowDialog(this) == DialogResult.OK)
			{
				// set path to save to
				link.ShortCutFile = s.FileName;

				// set the details of the link:
				link.Target = txtTarget.Text;
				link.Arguments = txtArguments.Text;
				link.Description = txtDescription.Text;
				link.IconPath = txtIconFile.Text;
				link.IconIndex = (txtIconIndex.Text.Length > 0 ? 
					System.Int32.Parse(txtIconIndex.Text) : 0);

				// save the link:
				link.Save();

				// refresh details:
				showDetails();
			}
		}

		private void frmShellLinkTester_Load(object sender, System.EventArgs e)
		{
		
		}

		private void mnu_Click(object sender, System.EventArgs e)
		{
			if ((sender == mnuNew) || (sender == mnuClose))
			{
				link = new ShellLink();
				showDetails();
			}
			else if (sender == mnuOpen)
			{
				openShortcut();
			}
			else if (sender == mnuSave)
			{
				if (link.ShortCutFile.Length == 0)
				{
					saveShortcutAs();
				}
				else
				{
					link.Save();
				}
			}
			else if (sender == mnuSaveAs)
			{
				saveShortcutAs();
			}
			else if (sender == mnuExit)
			{
				this.Close();
			}
			else if (sender == mnuAbout)
			{
				frmAbout a = new frmAbout();
				a.ShowDialog(this);
				a.Dispose();
			}
		}

		private void btnChooseIcon_Click(object sender, System.EventArgs e)
		{
			frmIconPicker fi = null;
			if (link.IconPath.Length > 0)
			{
				fi = new frmIconPicker(link.IconPath, link.IconIndex);
			}
			else
			{
				fi = new frmIconPicker(link.Target, 0);
			}
			if (fi.ShowDialog(this) == DialogResult.OK)
			{
				txtIconFile.Text = fi.IconFile;
				txtIconIndex.Text = fi.IconIndex.ToString();
				picIcon.Image = fi.LargeIcon.ToBitmap();
				picSmallIcon.Image = fi.SmallIcon.ToBitmap();
			}
			fi.Dispose();
		}

		private void btnPick_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "All Files (*.*)|*.*";
			o.CheckFileExists = true;
			o.CheckPathExists = true;
			if (o.ShowDialog() == DialogResult.OK)
			{
				txtTarget.Text = o.FileName;				
			}
		}

	}
}
