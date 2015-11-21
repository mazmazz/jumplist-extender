using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ShellLinkTester
{
	/// <summary>
	/// Summary description for frmAbout.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.TextBox txtInfo;
		private System.Windows.Forms.LinkLabel lnkVbaccelerator;
		private System.Windows.Forms.LinkLabel lnkNetProjectZip;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblInfo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmAbout));
			this.btnOK = new System.Windows.Forms.Button();
			this.txtInfo = new System.Windows.Forms.TextBox();
			this.lnkVbaccelerator = new System.Windows.Forms.LinkLabel();
			this.lnkNetProjectZip = new System.Windows.Forms.LinkLabel();
			this.lblName = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(276, 324);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(92, 28);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			// 
			// txtInfo
			// 
			this.txtInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtInfo.Location = new System.Drawing.Point(84, 148);
			this.txtInfo.Multiline = true;
			this.txtInfo.Name = "txtInfo";
			this.txtInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtInfo.Size = new System.Drawing.Size(284, 112);
			this.txtInfo.TabIndex = 1;
			this.txtInfo.Text = "";
			// 
			// lnkVbaccelerator
			// 
			this.lnkVbaccelerator.Location = new System.Drawing.Point(84, 284);
			this.lnkVbaccelerator.Name = "lnkVbaccelerator";
			this.lnkVbaccelerator.Size = new System.Drawing.Size(284, 16);
			this.lnkVbaccelerator.TabIndex = 2;
			this.lnkVbaccelerator.TabStop = true;
			this.lnkVbaccelerator.Text = "vbAccelerator.com Home Page";
			// 
			// lnkNetProjectZip
			// 
			this.lnkNetProjectZip.Location = new System.Drawing.Point(84, 268);
			this.lnkNetProjectZip.Name = "lnkNetProjectZip";
			this.lnkNetProjectZip.Size = new System.Drawing.Size(284, 16);
			this.lnkNetProjectZip.TabIndex = 3;
			this.lnkNetProjectZip.TabStop = true;
			this.lnkNetProjectZip.Text = "ShellLink Home Page";
			this.lnkNetProjectZip.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNetProjectZip_LinkClicked);
			// 
			// lblName
			// 
			this.lblName.Font = new System.Drawing.Font("Trebuchet MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblName.Location = new System.Drawing.Point(92, 32);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(276, 56);
			this.lblName.TabIndex = 4;
			this.lblName.Text = "Shell Link Tester";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.RosyBrown;
			this.label1.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(92, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(276, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "vbAccelerator.com";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Bitmap)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(80, 80);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.DarkGray;
			this.label2.Location = new System.Drawing.Point(8, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(360, 1);
			this.label2.TabIndex = 7;
			this.label2.Text = "label2";
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.DarkGray;
			this.label3.Location = new System.Drawing.Point(4, 316);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(360, 1);
			this.label3.TabIndex = 8;
			this.label3.Text = "label3";
			// 
			// lblInfo
			// 
			this.lblInfo.Location = new System.Drawing.Point(88, 96);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(280, 48);
			this.lblInfo.TabIndex = 9;
			this.lblInfo.Text = "Demonstrates reading and writing Shortcuts through the ShellLink object.";
			// 
			// frmAbout
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(378, 360);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.lblInfo,
																		  this.label3,
																		  this.label2,
																		  this.pictureBox1,
																		  this.label1,
																		  this.lblName,
																		  this.lnkNetProjectZip,
																		  this.lnkVbaccelerator,
																		  this.txtInfo,
																		  this.btnOK});
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About ShellLink Tester";
			this.Load += new System.EventHandler(this.frmAbout_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmAbout_Load(object sender, System.EventArgs e)
		{
		
		}

		private void lnkNetProjectZip_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
		
		}
	}
}
