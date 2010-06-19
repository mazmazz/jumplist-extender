using System;
using System.Windows .Forms;
using System.Drawing ;
using System.Drawing.Drawing2D;
using System.ComponentModel;


namespace ImageComboBox
{
	/// <summary>
	/// Summary description for ImageListBox.
	/// </summary>
	[ToolboxItem(false)]
	public sealed class ImageListBox: System.Windows .Forms.ListBox 
	{
		private ImageList imageList = null;
		private Icon oIcon = null;

		public ImageListBox()
		{
			int dimension=16;
			Bitmap bm = new Bitmap(dimension,dimension);
			Graphics g = Graphics.FromImage((Image)bm); 
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle imgRect = new Rectangle(0,0,bm.Width-1,bm.Height-1);
			g.DrawRectangle (new Pen (new SolidBrush (Color.Black)),imgRect);
			g.FillRectangle(new SolidBrush (Color.Transparent),imgRect);
			oIcon = Icon.FromHandle(bm.GetHicon()); 
			g.Dispose();
			bm.Dispose();
			ListBoxIcon = oIcon;
			this.ItemHeight = 25;
			this.DrawMode = DrawMode.OwnerDrawFixed ;
		}

		[BrowsableAttribute(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public Icon ListBoxIcon
		{
			get
			{
				return oIcon;
			}
			set
			{
				oIcon = value;
			}
		}
		[BrowsableAttribute(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public ImageList ImageList
		{
			get
			{
				return imageList;
			}
			set
			{
				imageList = value;
				if(imageList == null || imageList.Images.Count == 0)
				{
					this.Items .Clear();
					this.Items.Add ("(none)");
				}
				if(imageList != null && imageList.Images.Count > 0)
				{
					this.Items .Clear();
					this.ItemHeight = imageList.ImageSize.Height + 10;
					for(int i=0; i<imageList.Images .Count; i++)
						this.Items.Add(i);

					this.Items.Add ("(none)");
				}
			}
		
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem (e);
			
			if (e.Index >= 0 && Items.Count > 0 && e.Index < Items.Count )
			{
				e.DrawBackground();
				e.DrawFocusRectangle ();
				
				if(this.ImageList == null || this.ImageList .Images .Count == 0 || e.Index == Items.Count - 1)
				{
					e.Graphics.DrawIcon (this.oIcon ,new Rectangle (e.Bounds.X+2,e.Bounds.Y+5,this.oIcon.Width ,this.oIcon.Height));
					e.Graphics.DrawString(this.Items [e.Index ].ToString (),e.Font ,new SolidBrush(e.ForeColor),(float)(e.Bounds .X+ 16 + 3),(float)(e.Bounds.Y+5));
				}
				else
				{
					if(e.Index < Items.Count - 1)
						e.Graphics.DrawImage (this.ImageList.Images [e.Index ],new Rectangle (e.Bounds.X+2,e.Bounds.Y+5,this.ImageList.ImageSize.Width ,this.ImageList.ImageSize.Height));
					else
						e.Graphics.DrawIcon (this.oIcon ,new Rectangle (e.Bounds.X+2,e.Bounds.Y+5,this.oIcon.Width ,this.oIcon.Height));
					e.Graphics.DrawString(this.Items [e.Index ].ToString (),e.Font ,new SolidBrush(e.ForeColor),(float)(e.Bounds.X+this.ImageList.Images[e.Index].Width + 3),(float)(e.Bounds.Y+5));
				}
			}
		}
	}
}
