using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using vbAccelerator.Controls.ListBox;
using vbAccelerator.Controls.TextBox;

namespace vbAccelerator.Forms.IconPicker
{

	internal class UnManagedMethods
	{
		[DllImport("Shell32", CharSet=CharSet.Auto)]
		internal extern static int ExtractIconEx (
			[MarshalAs(UnmanagedType.LPTStr)] 
			string lpszFile,
			int nIconIndex,
			IntPtr[] phIconLarge, 
			IntPtr[] phIconSmall,
			int nIcons);
	}

	/// <summary>
	/// Summary description for frmIconPicker.
	/// </summary>
	public class frmIconPicker : System.Windows.Forms.Form
	{
		private string iconFile = "";
		private int iconIndex = -1;
		private bool loaded = false;

		private vbAccelerator.Controls.TextBox.AutoCompleteTextBox txtTarget;
		private System.Windows.Forms.Label lblTarget;
		private System.Windows.Forms.Button btnPick;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private vbAccelerator.Controls.ListBox.IconPickerListBox lstIcons;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Icon LargeIcon
		{
			get
			{
				IntPtr[] hIconEx = new IntPtr[1] {IntPtr.Zero};
				int iconCount = UnManagedMethods.ExtractIconEx(
					iconFile,
					iconIndex,
					hIconEx,
					null,
					1);
				// If success then return as a GDI+ object
				Icon icon = null;
				if (hIconEx[0] != IntPtr.Zero)
				{
					icon = Icon.FromHandle(hIconEx[0]);
					//UnManagedMethods.DestroyIcon(hIconEx[0]);
				}
				return icon;
			}
		}

		public Icon SmallIcon
		{
			get
			{
				IntPtr[] hIconEx = new IntPtr[1] {IntPtr.Zero};
				int iconCount = UnManagedMethods.ExtractIconEx(
					iconFile,
					iconIndex,
					null,
					hIconEx,
					1);
				// If success then return as a GDI+ object
				Icon icon = null;
				if (hIconEx[0] != IntPtr.Zero)
				{
					icon = Icon.FromHandle(hIconEx[0]);
					//UnManagedMethods.DestroyIcon(hIconEx[0]);
				}
				return icon;
			}
		}

		public string IconFile
		{
			get
			{
				return iconFile;
			}
			set
			{
				iconFile = value;
				if (loaded)
				{
					loadIcons();
					selectIconIndex();
				}
			}
		}
		public int IconIndex
		{
			get
			{
				return iconIndex;
			}
			set
			{
				iconIndex = value;
				if (loaded)
				{
					selectIconIndex();
				}
			}
		}

		public frmIconPicker()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Set up target:
			txtTarget.AutoCompleteFlags = AutoCompleteTextBox.SHAutoCompleteFlags.SHACF_FILESYS_ONLY;

		}
		public frmIconPicker(string iconFile) : this()
		{
			this.iconFile = iconFile;
		}
		public frmIconPicker(string iconFile, int iconIndex) : this()
		{
			this.iconFile = iconFile;
			this.iconIndex = iconIndex;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.lstIcons = new vbAccelerator.Controls.ListBox.IconPickerListBox();
			this.txtTarget = new vbAccelerator.Controls.TextBox.AutoCompleteTextBox();
			this.lblTarget = new System.Windows.Forms.Label();
			this.btnPick = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lstIcons
			// 
			this.lstIcons.ColumnWidth = 36;
			this.lstIcons.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lstIcons.HorizontalScrollbar = true;
			this.lstIcons.IconSize = vbAccelerator.Controls.ListBox.IconPickerListBox.IconPickerIconSize.large;
			this.lstIcons.ItemHeight = 36;
			this.lstIcons.Location = new System.Drawing.Point(8, 80);
			this.lstIcons.MultiColumn = true;
			this.lstIcons.Name = "lstIcons";
			this.lstIcons.Size = new System.Drawing.Size(304, 148);
			this.lstIcons.TabIndex = 0;
			this.lstIcons.SelectedIndexChanged += new System.EventHandler(this.lstIcons_SelectedIndexChanged);
			// 
			// txtTarget
			// 
			this.txtTarget.AutoCompleteFlags = vbAccelerator.Controls.TextBox.AutoCompleteTextBox.SHAutoCompleteFlags.SHACF_FILESYS_ONLY;
			this.txtTarget.Location = new System.Drawing.Point(8, 24);
			this.txtTarget.Name = "txtTarget";
			this.txtTarget.Size = new System.Drawing.Size(228, 21);
			this.txtTarget.TabIndex = 4;
			this.txtTarget.Text = "";
			// 
			// lblTarget
			// 
			this.lblTarget.Location = new System.Drawing.Point(8, 4);
			this.lblTarget.Name = "lblTarget";
			this.lblTarget.Size = new System.Drawing.Size(228, 20);
			this.lblTarget.TabIndex = 3;
			this.lblTarget.Text = "&Look for icons in this file:";
			// 
			// btnPick
			// 
			this.btnPick.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnPick.Location = new System.Drawing.Point(240, 24);
			this.btnPick.Name = "btnPick";
			this.btnPick.Size = new System.Drawing.Size(75, 24);
			this.btnPick.TabIndex = 5;
			this.btnPick.Text = "&Browse...";
			this.btnPick.Click += new System.EventHandler(this.btnPick_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 60);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(300, 16);
			this.label1.TabIndex = 6;
			this.label1.Text = "Select an icon from the list below:";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(152, 248);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 28);
			this.btnOK.TabIndex = 7;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(232, 248);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 28);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			// 
			// frmIconPicker
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(322, 284);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnCancel,
																		  this.btnOK,
																		  this.label1,
																		  this.btnPick,
																		  this.txtTarget,
																		  this.lblTarget,
																		  this.lstIcons});
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmIconPicker";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose Icon";
			this.Load += new System.EventHandler(this.frmIconPicker_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmIconPicker_Load(object sender, System.EventArgs e)
		{
			loadIcons();
			selectIconIndex();
		}

		private void loadIcons()
		{
			lstIcons.Items.Clear();
			if (iconFile.Length > 0)
			{
				lstIcons.LoadIcons(iconFile);
			}
			txtTarget.Text = iconFile;
		}

		private void selectIconIndex()
		{
			if ((iconIndex > -1) && (iconIndex < lstIcons.Items.Count))
			{
				lstIcons.SelectedIndex = iconIndex;
			}
			else
			{
				if (lstIcons.Items.Count > 0)
				{
					lstIcons.SelectedIndex = 0;
				}
			}
		}

		private void lstIcons_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			iconIndex = lstIcons.SelectedIndex;
		}

		private void btnPick_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog o = new OpenFileDialog();
			o.Filter = "Icon Files|*.EXE;*.DLL;*.ICO|Programs|*.EXE|Libraries|*.DLL|Icon Files|*.ICO|All Files (*.*)|*.*";
			o.FilterIndex = 1;
			o.CheckFileExists = true;
			o.CheckPathExists = true;
			if (o.ShowDialog(this) == DialogResult.OK)
			{
				iconFile = o.FileName;
				iconIndex = 0;
				loadIcons();
				selectIconIndex();
			}
		}

	}
}
