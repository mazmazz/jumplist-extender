using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace vbAccelerator.Components.Menu
{
	
	public class IconMenuItem : MenuItem
	{
		private string m_tag = "";
		private Font m_font = null;
		private int m_iconIndex = -1;

		private int iconWidth = 24;
		private int iconToTextSeparatorWidth  = 7;
		private int textToEdgeSeparatorWidth = 16;	

		public Font Font
		{
			get
			{
				return m_font;
			}
			set
			{
				m_font = value;
			}
		}

		public string Tag
		{
			get
			{
				return m_tag;
			}
			set
			{
				m_tag = value;
			}
		}

		public int IconIndex
		{
			get
			{
				return m_iconIndex;
			}
			set
			{
				m_iconIndex = value;
			}
		}

		private void DrawCaption(Graphics gfx, string text, Brush brush, 
			RectangleF layoutRect, bool showAccelerator, bool shortCut, bool isEnabled)
		{
			System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
			sf.FormatFlags = StringFormatFlags.NoWrap;
			sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
			sf.LineAlignment = StringAlignment.Center;				
			if (showAccelerator)
			{
				sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;				
			}
			if (shortCut)
			{
				SizeF size = gfx.MeasureString(text, m_font);
				layoutRect.X = layoutRect.Width - size.Width - textToEdgeSeparatorWidth;
				sf.Alignment = StringAlignment.Near;
			}
			else
			{
				sf.Alignment = StringAlignment.Near;
			}
			if (isEnabled)
			{
				gfx.DrawString(text, m_font, brush, layoutRect, sf);
			}
			else
			{
				Brush disabledBrush = SystemBrushes.ControlDark;
				gfx.DrawString(text, m_font, disabledBrush , layoutRect, sf);	
				//ControlPaint.DrawStringDisabled(gfx, text, m_font,
				//	Color.FromKnownColor(KnownColor.ControlDark), layoutRect, sf);
			}
		}

		private void DrawIcon(Graphics gfx, Rectangle rect, 
			bool isSelected, bool isChecked, bool isRadioChecked, bool isEnabled)
		{
			System.Windows.Forms.ImageList iconImageList = null;

			IconMainMenu m = (IconMainMenu)this.GetMainMenu();
			if (m != null)
			{
				iconImageList = m.IconImageList;
			}
			else
			{
				IconContextMenu c = (IconContextMenu)this.GetContextMenu();
				if (c != null)
				{
					iconImageList = c.IconImageList;
				}
			}

			bool hasIcon = false;
			if (iconImageList != null)
			{
				if ((m_iconIndex >= 0) && (m_iconIndex < iconImageList.Images.Count))
				{
					hasIcon = true;

					int x = rect.Left + (iconWidth- iconImageList.ImageSize.Width)/2;
					int y = rect.Top + (rect.Height - iconImageList.ImageSize.Height)/2;
					
					if ((isSelected) && (isEnabled))
					{
						// draw icon as a shadow here
						ControlPaint.DrawImageDisabled(gfx, 
							iconImageList.Images[m_iconIndex], 
							x, y, Color.FromKnownColor(KnownColor.Control));
						x-=1;
						y-=1;
					}
					if (isEnabled)
					{
						iconImageList.Draw(gfx, x, y, 
							iconImageList.ImageSize.Width, iconImageList.ImageSize.Height, m_iconIndex);
					}
					else
					{
						ControlPaint.DrawImageDisabled(gfx, 
							iconImageList.Images[m_iconIndex], 
							x, y, Color.FromKnownColor(KnownColor.Control));
					}
				}
			}

			if (isChecked)
			{
				Rectangle checkRect = rect;
				checkRect.Width = iconWidth;
				checkRect.Inflate(-1,-1);				
				checkRect.Height -= 1;
				Pen pen = SystemPens.Highlight;
				gfx.DrawRectangle(pen, checkRect);
				if (!hasIcon)
				{
					int x = rect.Left + (iconWidth- SystemInformation.MenuCheckSize.Width)/2;
					int y = rect.Top + (rect.Height - SystemInformation.MenuCheckSize.Width)/2;

					// Fill the background of the check item with an alpha-blended
					// highlight colour:
					int alpha = 24;
					if (isSelected)
					{
						alpha = 128;
					}
					Brush br = new SolidBrush(Color.FromArgb(alpha, Color.FromKnownColor(KnownColor.Highlight)));
					gfx.FillRectangle(br, checkRect);
					br.Dispose();

					// create a new bitmap to draw the menu glyph onto:
					Bitmap bm = new Bitmap(SystemInformation.MenuCheckSize.Width, SystemInformation.MenuCheckSize.Height);
					Graphics gr = Graphics.FromImage(bm);					
					if (isRadioChecked)
					{
						// draw radio option						
						ControlPaint.DrawMenuGlyph(gr, 0, 0, 
							bm.Width, bm.Height,
							MenuGlyph.Bullet);
					}
					else
					{
						// draw check mark
						ControlPaint.DrawMenuGlyph(gr, 0, 0, 
							bm.Width, bm.Height,
							MenuGlyph.Checkmark);
					}
					Rectangle outRect = new Rectangle(x, y, bm.Width, bm.Height);
					// Draw the menu glyph transparently:
					ImageAttributes imgAttr = new ImageAttributes();
					imgAttr.SetColorKey(bm.GetPixel(0,0), bm.GetPixel(0,0), ColorAdjustType.Default);
					gfx.DrawImage(bm, outRect, 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel, imgAttr);
					imgAttr.Dispose();
					bm.Dispose();
					gr.Dispose();
				}
			}			

		}

		protected override void OnDrawItem ( System.Windows.Forms.DrawItemEventArgs e )
		{			
			if (this.Parent == this.GetMainMenu())
			{
				Brush brush;
				brush = SystemBrushes.Control;
				e.Graphics.FillRectangle(brush, e.Bounds);
				if (
					((e.State & DrawItemState.Selected) == DrawItemState.Selected) || 
					((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
					)
				{					

					Rectangle highlightRect = e.Bounds;
					highlightRect.X += 1;
					highlightRect.Y += 1;
					highlightRect.Width -= 3;

					Pen pen = SystemPens.Highlight;
					if ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
					{						
						highlightRect.Height -= 2;
						brush = new SolidBrush(HighlightBrushColor());
						e.Graphics.FillRectangle(brush, highlightRect);
						brush.Dispose();

						e.Graphics.DrawRectangle(pen, highlightRect);
					}
					else
					{
						highlightRect.X -= 1;

						highlightRect.Height -= 1;
						brush = SystemBrushes.Control;
						e.Graphics.FillRectangle(brush, highlightRect);

						highlightRect.Height -= 1;
						highlightRect.Width -= 1;
						pen = SystemPens.ControlDark;
						e.Graphics.DrawLine(pen, highlightRect.X, highlightRect.Y + highlightRect.Height, highlightRect.X, highlightRect.Y);
						e.Graphics.DrawLine(pen, highlightRect.X, highlightRect.Y, highlightRect.X + highlightRect.Width, highlightRect.Y);
						e.Graphics.DrawLine(pen, highlightRect.X + highlightRect.Width, highlightRect.Y, highlightRect.X + highlightRect.Width, highlightRect.Y + highlightRect.Height);
						
						//pen = SystemPens.ControlDark;
						pen = new Pen(Color.FromArgb(128, Color.Black));
						e.Graphics.DrawLine(pen, 
							highlightRect.X + highlightRect.Width + 1, 
							highlightRect.Y + 4, 
							highlightRect.X + highlightRect.Width + 1, 
							highlightRect.Y + highlightRect.Height);
						pen.Dispose();
						//pen = SystemPens.ControlDarkDark;
						pen = new Pen(Color.FromArgb(64, Color.Black));
						e.Graphics.DrawLine(pen, 
							highlightRect.X + highlightRect.Width + 2, 
							highlightRect.Y + 4, 
							highlightRect.X + highlightRect.Width + 2, 
							highlightRect.Y + highlightRect.Height);						
						pen.Dispose();
						pen = new Pen(Color.FromArgb(32, Color.Black));
						e.Graphics.DrawLine(pen, 
							highlightRect.X + highlightRect.Width + 3, 
							highlightRect.Y + 5, 
							highlightRect.X + highlightRect.Width + 3, 
							highlightRect.Y + highlightRect.Height);						
						pen.Dispose();
					}

				}
				brush = SystemBrushes.WindowText;
				RectangleF layoutRect = new RectangleF(e.Bounds.X + 1, e.Bounds.Y + 1, e.Bounds.Width - 2, e.Bounds.Height);
				layoutRect.X += 4;
				DrawCaption(e.Graphics, this.Text, brush, layoutRect, 
					((e.State & DrawItemState.NoAccelerator) != DrawItemState.NoAccelerator ),
					false, this.Enabled);
			}
			else
			{
				Brush iconBarBrush = SystemBrushes.Control;
				e.Graphics.FillRectangle(iconBarBrush, e.Bounds.X, e.Bounds.Y, 
					iconWidth, e.Bounds.Height);

				Rectangle rectOut = e.Bounds;

				if (this.Text.Equals("-"))
				{
					rectOut.X += iconWidth;
					Brush brush;
					if (System.Environment.OSVersion.Version.Major > 4)
					{
						brush = SystemBrushes.Menu;
					}
					else
					{
						brush = SystemBrushes.Window;
					}
					e.Graphics.FillRectangle(brush, rectOut);

					Pen pen = SystemPens.ControlDark;
					e.Graphics.DrawLine(pen, 
						iconWidth + iconToTextSeparatorWidth, e.Bounds.Y + 1, 
						e.Bounds.Width, e.Bounds.Y + 1);
				}
				else
				{
					Rectangle rectIn = rectOut;
					Brush brush;
					rectIn.Height -= 1;
					if (
						((e.State & DrawItemState.Selected) == DrawItemState.Selected) && 
						(this.Enabled))

					{
						if (System.Environment.OSVersion.Version.Major > 4)
						{
							brush = SystemBrushes.Menu;
						}
						else
						{
							brush = SystemBrushes.Window;
						}
						e.Graphics.FillRectangle(brush, rectIn);
						brush = new SolidBrush(Color.FromArgb(64, Color.FromKnownColor(KnownColor.Highlight)));
						e.Graphics.FillRectangle(brush, rectIn);
						brush.Dispose();

						Pen pen = SystemPens.Highlight;
						Rectangle highlightRect = rectIn;
						highlightRect.Width -= 1;
						e.Graphics.DrawRectangle(pen, highlightRect);
					}
					else
					{
						rectOut.X += iconWidth;
						rectOut.Width -= iconWidth;
						if (System.Environment.OSVersion.Version.Major > 4)
						{
							brush = SystemBrushes.Menu;
						}
						else
						{
							brush = SystemBrushes.Window;
						}
						e.Graphics.FillRectangle(brush, rectOut);
					}

					DrawIcon(
						e.Graphics, e.Bounds, 
						((e.State & DrawItemState.Selected)==DrawItemState.Selected),
						this.Checked, this.RadioCheck, this.Enabled);

					brush = SystemBrushes.WindowText;
					RectangleF layoutRect = new RectangleF(e.Bounds.X + iconWidth + iconToTextSeparatorWidth, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
					DrawCaption(e.Graphics, this.Text, brush, layoutRect, 
						((e.State & DrawItemState.NoAccelerator) != DrawItemState.NoAccelerator),
						false, this.Enabled);

					if (this.ShowShortcut)
					{
						if (this.Shortcut != Shortcut.None)
						{
							string shortCut = ShortcutToString(this.Shortcut);
							DrawCaption(e.Graphics, shortCut, brush, layoutRect, false, true, this.Enabled);
						}
					}
				}
			}
		}

		protected override void OnMeasureItem ( System.Windows.Forms.MeasureItemEventArgs e )
		{			
			if (this.Parent == this.GetMainMenu())
			{
				SizeF fontSize = e.Graphics.MeasureString(this.Text.Replace("&",""), this.Font);
				e.ItemWidth = (int)fontSize.Width + 3;
				e.ItemHeight = (int)fontSize.Height;
			}
			else
			{
				Size size = new Size(iconWidth + iconToTextSeparatorWidth + textToEdgeSeparatorWidth, 3);
				if (!this.Text.Equals("-"))
				{
					string toMeasure = this.Text;
					if (this.ShowShortcut)
					{
						if (this.Shortcut != Shortcut.None)
						{
							toMeasure += " " + ShortcutToString(this.Shortcut);
						}
					}
					SizeF fontSize = e.Graphics.MeasureString(toMeasure, this.Font);
					size.Width += (int)fontSize.Width;
					size.Height = Math.Max(iconWidth, (int)fontSize.Height);
				}
				e.ItemHeight = size.Height;
				e.ItemWidth = size.Width;
			}
		}

		private Color HighlightBrushColor()
		{			
			//return Color.FromArgb(192, Color.FromKnownColor(KnownColor.Highlight));
			Color cH = Color.FromKnownColor(KnownColor.Highlight);
			Color cM;
			if (System.Environment.OSVersion.Version.Major > 4)
			{
				cM = Color.FromKnownColor(KnownColor.Menu);
			}
			else
			{
				cM = Color.FromKnownColor(KnownColor.Window);
			}
			return Color.FromArgb(
				Math.Max(0,Math.Min(255, cH.R + (cM.R - cH.R)/2)),
				Math.Max(0,Math.Min(255, cH.G + (cM.G - cH.G)/2)),
				Math.Max(0,Math.Min(255, cH.B + (cM.B - cH.B)/2)));
		}

		private string ShortcutToString(Shortcut s)
		{
			string ret = s.ToString();
			ret = ret.Replace("Ctrl", "Ctrl+");
			ret = ret.Replace("Alt", "Alt+");
			ret = ret.Replace("Shift", "Shift+");
			return ret;
		}

		public IconMenuItem() : base()
		{
			this.OwnerDraw = true;
			m_font = SystemInformation.MenuFont;
		}
	}

	public class IconContextMenu : ContextMenu
	{
		private System.Windows.Forms.ImageList imgList;

		public System.Windows.Forms.ImageList IconImageList
		{
			get
			{
				return imgList;
			}
			set
			{
				imgList = value;
			}
		}

		public IconContextMenu() : base()
		{
		}

		public IconContextMenu( IconMenuItem[] items ) : base(items)
		{
		}
	}

	public class IconMainMenu : MainMenu
	{
		private System.Windows.Forms.ImageList imgList;

		public System.Windows.Forms.ImageList IconImageList
		{
			get
			{
				return imgList;
			}
			set
			{
				imgList = value;
			}
		}

		public IconMainMenu() : base()
		{
		}

		public IconMainMenu ( IconMenuItem[] items ) : base(items)
		{
		}
	}

}
