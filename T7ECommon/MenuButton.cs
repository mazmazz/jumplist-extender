using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

namespace T7ECommon
{
    public class MenuButton : Button
    {
    
        public MenuButton()
        {
            
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!ContextMenuStrip.Visible)
                ContextMenuStrip.Show(this, this.Width, 0);
            else
                ContextMenuStrip.Hide();
            /*if (((MouseEventArgs)e).Button == MouseButtons.Left && ContextMenuStrip != null)
            {
                if (!ContextMenuStrip.Visible)
                    ContextMenuStrip.Show(this, this.Width, 0);
                else
                    ContextMenuStrip.Hide();
            }*/
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }
}