using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace KeyboardTextBox
{
    public partial class Form2 : Form
    {
        #region P/Invokes
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        const int VK_LCONTROL = 0xA2; const int VK_LCTRL = 0xA2;
        const int VK_RCONTROL = 0xA3; const int VK_RCTRL = 0xA3;
        const int VK_LMENU = 0xA4; const int VK_LALT = 0xA4;
        const int VK_RMENU = 0xA5; const int VK_RALT = 0xA5;
        const int VK_LSHIFT = 0xA0;
        const int VK_RSHIFT = 0xA1;
        const int VK_LWIN = 0x5B;
        const int VK_RWIN = 0x5C;


        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public UInt32 vkCode;
            public UInt32 scanCode;
            public KBDLLHOOKSTRUCTFlags flags;
            public UInt32 time;
            public IntPtr dwExtraInfo;
        }

        [Flags()]
        public enum KBDLLHOOKSTRUCTFlags : int
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }
        #endregion

        public Form2()
        {
            InitializeComponent();
            //_hookID = SetHook(HookCallback);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt) e.Handled = true;
            
            if (e.KeyCode == Keys.ControlKey) StateLCtrl = true;
            if (e.KeyCode == Keys.Menu) StateLAlt = true;
            if (e.KeyCode == Keys.ShiftKey) StateLShift = true;
            // Keys.Win
            UpdateShortcutString();

            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu || e.KeyCode == Keys.ShiftKey)
            {
                e.Handled = true;
                return;
            }

            if (ShortcutStringState
                && (StateLCtrl || StateLAlt || StateLWin))
            {
                // Shortcut string is active, with ctrl/alt/win
                textBox1.Text = textBox1.Text.Insert(ShortcutStringPos + ShortcutStringLength,
                    e.KeyCode.ToString() + "}");
                textBox1.SelectionStart = ShortcutStringPos + ShortcutStringLength + e.KeyCode.ToString().Length + 1;
                ShortcutStringState = false;
            }

            if (StateLCtrl || StateLAlt || StateLWin) e.SuppressKeyPress = true;

            textBox2.Text = StateLCtrl.ToString() + "|"
                + StateLAlt.ToString() + "|"
                + StateLShift.ToString() + "|"
                + StateLWin.ToString() + "|";
        }


        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt) e.Handled = true;
            if (e.KeyCode == Keys.ControlKey) StateLCtrl = false;
            if (e.KeyCode == Keys.Menu) StateLAlt = false;
            if (e.KeyCode == Keys.ShiftKey) StateLShift = false;
            // Keys.Win
            UpdateShortcutString();
        }

        private static void UpdateShortcutString()
        {
            //UpdateShortcutString() to preview {Ctrl+ {Shift+
            //and if ctrlaltshiftwin is pressed, return 1

            //Since the ctrl/alt/win keys won't reach the textbox, USS() puts the
            //shortcut string there for them. BUT if shift is the only key, DON'T.
            // if shortcutstringstatus == true, append any pressed keys to the
            // shortcut sting and make ssstatus false.
            // if shortcutstringstatus == false, just type the thing as normal.

            if (StateLShift && !StateLCtrl && !StateLAlt && !StateLWin) return;

            if (ShortcutStringState) // Any completed shortcut strings should've set
                // this false.
            {
                textBox1.Text = textBox1.Text.Remove(ShortcutStringPos, ShortcutStringLength);
                textBox1.SelectionStart = ShortcutStringPos;
            }
            else
                ShortcutStringPos = textBox1.SelectionStart;

            string shortcutString = "{";

            if (StateLCtrl) shortcutString += "Ctrl+";
            if (StateLAlt) shortcutString += "Alt+";
            if (StateLWin) shortcutString += "Win+";
            // Only add Shift+ if one of the other keys are present
            if (StateLShift && (StateLCtrl || StateLAlt || StateLWin))
            {
                shortcutString += "Shift+";
            }

            if (shortcutString.Length <= 1) shortcutString = "";
            ShortcutStringLength = shortcutString.Length;
            textBox1.Text = textBox1.Text.Insert(ShortcutStringPos, shortcutString);
            textBox1.SelectionStart = ShortcutStringPos;

            // If any of the keys are present, set shortcutstringstate = true;
            if (StateLCtrl || StateLAlt || StateLWin || StateLShift) ShortcutStringState = true;
            else ShortcutStringState = false;
        }

        // How do I use bitwise operators for this?
        private static bool StateLCtrl = false;
        private static bool StateLAlt = false;
        private static bool StateLShift = false;
        private static bool StateLWin = false;

        #region Keyboard Hook
        static IntPtr _hookID = IntPtr.Zero;

        private void textBox1_Enter(object sender, EventArgs e)
        {
            //_hookID = SetHook(HookCallback);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            //UnhookWindowsHookEx(_hookID);
        }



        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static bool ShortcutStringState = false;
        // This affects up/down/left/right, home/end/insert/delete, and shift
        private static int ShortcutStringLength = 0;
        private static int ShortcutStringPos = 0;

        

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode < 0) return CallNextHookEx(_hookID, nCode, wParam, lParam);
            int vkCode = Marshal.ReadInt32(lParam);
            //uint vkCode = kbdStruct.vkCode;
            if (vkCode == VK_LWIN) {
                if(wParam.ToInt32() == WM_KEYDOWN)
                    StateLWin = true;
                else
                    StateLWin = false;
                //UpdateShortcutString();
                return (IntPtr)1;
            }
            else return CallNextHookEx(_hookID, nCode, wParam, lParam);

            // Basically, if we hit a blacklisted key, report it as handled. Else, pass it
            // to the usual stream.
            // There's the problem of hitting a blacklisted key we NEED, e.g. Win

            if (wParam.ToInt32() == WM_KEYDOWN)
            {
                if (vkCode == VK_LCTRL) StateLCtrl = true;
                if (vkCode == VK_LALT) StateLAlt = true;
                if (vkCode == VK_LSHIFT) StateLShift = true;
                if (vkCode == VK_LWIN) StateLWin = true;
                UpdateShortcutString();// to preview {Ctrl+ {Shift+
                //and if ctrlaltshiftwin is pressed, return 1

                //Since the ctrl/alt/win keys won't reach the textbox, USS() puts the
                //shortcut string there for them. BUT if shift is the only key, DON'T.
                // if shortcutstringstatus == true, append any pressed keys to the
                // shortcut sting and make ssstatus false.
                // if shortcutstringstatus == false, just type the thing as normal.

                if (vkCode == VK_LCTRL || vkCode == VK_LALT || vkCode == VK_LWIN)
                    return (IntPtr)1;
            }
            else if (wParam.ToInt32() == WM_KEYUP)
            {
                if (vkCode == VK_LCTRL) StateLCtrl = false;
                if (vkCode == VK_LALT) StateLAlt = false;
                if (vkCode == VK_LSHIFT) StateLShift = false;
                if (vkCode == VK_LWIN) StateLWin = false;
                UpdateShortcutString();// to remove {Ctrl+ {Shift+ as needed
                //If any keys pressed since then, remove the shortcut string

                if (vkCode == VK_LCTRL || vkCode == VK_LALT || vkCode == VK_LWIN)
                    return (IntPtr)1;
            }

            bool Alt = (System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != 0;
            bool Control = (System.Windows.Forms.Control.ModifierKeys & Keys.Control) != 0;

            //Prevent ALT-TAB and CTRL-ESC by eating TAB and ESC. Also kill Windows Keys.
            //int vkCode = Marshal.ReadInt32(lParam);
            Keys key = (Keys)vkCode;
            if (key == Keys.LWin || key == Keys.RWin) return (IntPtr)1; //handled
            if (Alt && key == Keys.Tab) return (IntPtr)1; 
            if (Alt && key == Keys.Space) return (IntPtr)1; 
            if (Control && key == Keys.Escape) return (IntPtr)1;

            //if (key == Keys.None) return (IntPtr)1; 
            //if (key <= Keys.Back) return (IntPtr)1; 
            if (key == Keys.Menu) return (IntPtr)1; 
            if (key == Keys.Pause) return (IntPtr)1; 
            if (key == Keys.Help) return (IntPtr)1; 

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
        #endregion

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
        }
    }
}
