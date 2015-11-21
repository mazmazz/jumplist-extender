using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace vbAccelerator.Controls.ListBox
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
	/// Summary description for IconPickerListBox.
	/// </summary>
	public class IconPickerListBox : System.Windows.Forms.ListBox
	{
		public enum IconPickerIconSize
		{
			large,
			small		// not implemented
		}

		private IconPickerIconSize iconSize = IconPickerIconSize.large;
		private System.Windows.Forms.ImageList ilsIcons = null;

		private void drawItem(System.Windows.Forms.DrawItemEventArgs e)
		{
			if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
			{
				e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
				if (this.Focused)
				{
					e.DrawFocusRectangle();
				}
			}
			else
			{
				e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
			}
			ilsIcons.Draw(e.Graphics, 
				e.Bounds.Left + 2, 
				e.Bounds.Top + 2, 
				ilsIcons.ImageSize.Width, 
				ilsIcons.ImageSize.Height, 
				(int)this.Items[e.Index]);			
		}

		protected override void OnDrawItem ( System.Windows.Forms.DrawItemEventArgs e )
		{
			if (base.DesignMode)
			{
				base.OnDrawItem(e);
			}
			else
			{
				if (e.Index>-1)
				{
					drawItem(e);
				}
				else
				{
					base.OnDrawItem(e);
				}
			}
		}

		public IconPickerIconSize IconSize
		{
			get
			{
				return iconSize;
			}
			set
			{
				iconSize = value;
			}
		}

		public void LoadIcons(string file)
		{
			this.Items.Clear();
			ilsIcons.Images.Clear();

			// Get number of icons:
			int iconCount = UnManagedMethods.ExtractIconEx(
				file, -1, null, null, 0);
			if (iconCount > 0)
			{
				IntPtr[] hIcon = new IntPtr[iconCount];
				if (iconSize == IconPickerIconSize.large)
				{
					iconCount = UnManagedMethods.ExtractIconEx(
						file, 0, hIcon, null, iconCount);
				}
				else
				{
					iconCount = UnManagedMethods.ExtractIconEx(
						file, 0, null, hIcon, iconCount);
				}
				if (iconCount > 0)
				{
					for (int i = 0; i < iconCount; i++)
					{
						Icon icon = Icon.FromHandle(hIcon[i]);
						ilsIcons.Images.Add(icon);
						icon.Dispose();
						this.Items.Add(i);
					}
				}
			}
		}

		public IconPickerListBox() : base()
		{
			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.MultiColumn = true;
			this.ColumnWidth = 36;
			this.ItemHeight = 36;
			this.HorizontalScrollbar = true;

			this.ilsIcons = new System.Windows.Forms.ImageList();
			ilsIcons.ColorDepth = ColorDepth.Depth32Bit;
			ilsIcons.ImageSize = new System.Drawing.Size(32, 32);
		}
	}
}
