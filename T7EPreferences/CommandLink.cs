using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.ComponentModel;

namespace T7EPreferences
{
    /*public class CommandLink : Button
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SendMessage(HandleRef hWnd,
           UInt32 Msg, ref int wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SendMessage(HandleRef hWnd,
           UInt32 Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SendMessage(HandleRef hWnd,
           UInt32 Msg, IntPtr wParam, bool lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern int SendMessage(HandleRef hWnd,
           UInt32 Msg, IntPtr wParam, IntPtr lParam);

        const int BS_COMMANDLINK = 0x0000000E;

        const uint BCM_SETNOTE = 0x00001609;
        const uint BCM_GETNOTE = 0x0000160A;
        const uint BCM_GETNOTELENGTH = 0x0000160B;

        const uint BCM_SETSHIELD = 0x0000160C;

        public CommandLink()
        {
            this.FlatStyle = FlatStyle.System;
        }

        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                return new Size(180, 60);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParams = base.CreateParams;
                cParams.Style |= BS_COMMANDLINK;
                return cParams;
            }
        }

        private bool _shield = false;

        [Category("Command Link"),
         Description("Gets or sets the shield icon visibility of the command link."),
         DefaultValue(false)]
        public bool Shield
        {
            get { return _shield; }
            set
            {
                _shield = value;
                SendMessage(new HandleRef(this, this.Handle), BCM_SETSHIELD, IntPtr.Zero,
                    _shield);
            }
        }

        [Category("Command Link"),
         Description("Gets or sets the note text of the command link."),
         DefaultValue("")]
        public string Note
        {
            get
            {
                return GetNoteText();
            }
            set
            {
                SetNoteText(value);
            }
        }

        private void SetNoteText(string value)
        {
            SendMessage(new HandleRef(this, this.Handle),
                BCM_SETNOTE,
                IntPtr.Zero, value);
        }

        private string GetNoteText()
        {
            int length = SendMessage(new HandleRef(this, this.Handle),
                BCM_GETNOTELENGTH,
                IntPtr.Zero, IntPtr.Zero) + 1;

            StringBuilder sb = new StringBuilder(length);

            SendMessage(new HandleRef(this, this.Handle),
                BCM_GETNOTE,
                ref length, sb);

            return sb.ToString();
        }

    }*/

    class CommandLinkButton : Button
    {
        // this version started at http://danielmoth.com/Blog
        public CommandLinkButton()
        {
            base.FlatStyle = FlatStyle.System;
        }

        private string helpText = "";
        public string HelpText
        {
            get
            {
                return this.helpText;
            }
            set
            {
                Win32Api.SendMessage(this.Handle,
                  0x00001609, // BCM_SETNOTE
                  IntPtr.Zero, value);

                this.helpText = value;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x0000000e; //BS_COMMANDLINK
                return cp;
            }
        }
    }

    public static class Win32Api
    {
        [DllImport("user32", CharSet = CharSet.Unicode)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);
    }

}
