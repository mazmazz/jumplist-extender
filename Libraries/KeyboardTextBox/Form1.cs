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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Keyboard shortcut keys:
        // Ctrl Alt Win Shift RCtrl RAlt RShift
        // Start behavior when Ctrl, Alt, or Win is down
        // Stop behavior when a non-modifier key is pressed
        // Backspace 
        // {Ctrl+Alt+Shift+Win+D}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //private void textBox1_KeyDown(object sender, KeyEventArgs e)
        //{
        //    return;
        //    switch (e.KeyCode)
        //    {
        //        case Keys.ShiftKey:
        //            MessageBox.Show("SHIFT ME");
        //            break;
        //        case Keys.A:
        //            MessageBox.Show("I AM AN A");
        //            e.Handled = true;
        //            break;
        //        case Keys.LWin:
        //            MessageBox.Show("I am a shitty Operating System");
        //            e.Handled = true;
        //            break;
        //    }
        //}

        //// Keypress fires for any key that has a character -- space, letters.
        //// But not modifier keys or special keys. Yes!
        //private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    return;
        //    textBox1.Text += "Keypressed " + e.KeyChar;
        //    e.Handled = true;
        //    return;
        //    switch (e.KeyChar)
        //    {
        //        case 'a':
        //            e.Handled = true;
        //            break;
        //    }
        //}

        //private void textBox1_KeyUp(object sender, KeyEventArgs e)
        //{
        //    return;
        //    switch (e.KeyCode)
        //    {
        //        case Keys.LWin:
        //            e.Handled = true;
        //            break;
        //    }
        //}

        // //////

        static IntPtr _hookID = IntPtr.Zero;

        private void textBox1_Enter(object sender, EventArgs e)
        {
            _hookID = SetHook(HookCallback);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
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


        static bool ctrlKey = false;
        static bool altKey = false;
        static bool winKey = false;
        static bool shiftKey = false;
        static bool rCtrlKey = false;
        static bool rAltKey = false;
        static bool rShiftKey = false;
        static bool rWinKey = false;

        // In multi-line mode, we won't capture
        // Arrow keys, home, insert, end, delete, backspace, tab, enter
        // In single-line mode, we WILL capture those (except enter, which switches to multiline)
        // In ALL cases, we won't capture:
        // Alt+Tab, Alt+F4, Ctrl+Shift+Esc, Ctrl+Alt+Del

        static int ShortcutStringOffset = 0;
        static int ShortcutStringLength = 0;
        static bool ShortcutStringActive = false;

        private static void UpdateShortcutString(KBDLLHOOKSTRUCT keyStruct, bool cutOff)
        {
            if (cutOff && !ShortcutStringActive) return;

            // Ignore these keys:
            switch (keyStruct.vkCode)
            {
                // Ignore always
                case 0x08: //VK_BACKSPACE
                case 0x2E: //VK_DELETE
                    return;
                // Alt+Tab, Alt+F4, Ctrl+Shift+Esc, Ctrl+Alt+Del



                // Ignore when multiline is enabled
                case 0x24: //VK_HOME
                case 0x23: //VK_END
                case 0x2D: //VK_INSERT
                //case 0x2E: //VK_DELETE
                case 0x09: //VK_TAB
                case 0x0D: //VK_RETURN
                case 0x25: //VK_LEFT
                case 0x26: //VK_UP
                case 0x27: //VK_RIGHT
                case 0x28: //VK_DOWN
                    if (textBox1.Multiline) return;
                    else break;
            }
            
            string shortcutString = "{";
            if (ctrlKey) shortcutString += "Ctrl+";
            if(rCtrlKey) shortcutString += "RCtrl+";
            if (altKey) shortcutString += "Alt+";
            if (rAltKey) shortcutString += "RAlt+";
            if (shiftKey) shortcutString += "Shift+";
            if (rShiftKey) shortcutString += "RShift+";
            if (winKey) shortcutString += "Win+";
            if (rWinKey) shortcutString += "RWin+";

            // Now for the key value
            int vKey = (int)keyStruct.vkCode;

            switch (vKey)
            {
                case 0x0D: //VK_ENTER
                    shortcutString += "Enter";
                    break;
                case 0x1B: //VK_ESCAPE
                    shortcutString += "Esc";
                    break;
                case 0x20: //VK_SPACE
                    if(shortcutString.Length > 1) shortcutString += "Space";
                    break;
                case 0x09: //VK_TAB
                    shortcutString += "Tab";
                    break;
                //case 0x08: //VK_BACKSPACE
                //    shortcutString += "Backspace";
                //    break;
                //case 0x2E: //VK_DELETE
                //    shortcutString += "Delete";
                //    break;
                case 0x2D: //VK_INSERT
                    shortcutString += "Insert";
                    break;
                case 0x25: //VK_LEFT
                    shortcutString += "Left";
                    break;
                case 0x26: //VK_UP
                    shortcutString += "Up";
                    break;
                case 0x27: //VK_RIGHT
                    shortcutString += "Right";
                    break;
                case 0x28: //VK_DOWN
                    shortcutString += "Down";
                    break;
                case 0x24: //VK_HOME
                    shortcutString += "Home";
                    break;
                case 0x23: //VK_END
                    shortcutString += "End";
                    break;
                case 0x21: //VK_PRIOR/PAGEUP
                    shortcutString += "PgUp";
                    break;
                case 0x22: //VK_NEXT/PAGEDOWN
                    shortcutString += "PgDn";
                    break;
                case 0x14: //VK_CAPITAL/CAPSLOCK
                    shortcutString += "CapsLock";
                    break;
                case 0x91: //VK_SCROLL
                    shortcutString += "ScrollLock";
                    break;
                case 0x90: //VK_NUMLOCK
                    shortcutString += "NumLock";
                    break;
                case 0x2A: //VK_PRINT/PRINTSCREEN
                    shortcutString += "PrintScreen";
                    break;
                case 0x13: //VK_PAUSE
                    shortcutString += "Pause";
                    break;
                case 0x70: //VK_F1
                    shortcutString += "F1";
                    break;
                case 0x71: //VK_F2
                    shortcutString += "F2";
                    break;
                case 0x72: //VK_F1
                    shortcutString += "F3";
                    break;
                case 0x73: //VK_F1
                    shortcutString += "F4";
                    break;
                case 0x74: //VK_F1
                    shortcutString += "F5";
                    break;
                case 0x75: //VK_F1
                    shortcutString += "F6";
                    break;
                case 0x76: //VK_F1
                    shortcutString += "F7";
                    break;
                case 0x77: //VK_F1
                    shortcutString += "F8";
                    break;
                case 0x78: //VK_F1
                    shortcutString += "F9";
                    break;
                case 0x79: //VK_F1
                    shortcutString += "F10";
                    break;
                case 0x80: //VK_F1
                    shortcutString += "F11";
                    break;
                case 0x81: //VK_F1
                    shortcutString += "F12";
                    break;

                case VK_LCTRL:
                case VK_RCTRL:
                case VK_LMENU:
                case VK_RMENU:
                case VK_LSHIFT:
                case VK_RSHIFT:
                case VK_LWIN:
                case VK_RWIN:
                    break;

                default:
                    if ((vKey >= 0x30 && vKey <= 0x5A) // 0-9, A-Z
                        || (vKey >= 0x60 && vKey <= 0x6B) // NumPad 0-9, math keys
                        || (vKey >= 0xBA && vKey <= 0xDF) // Punctuation, culture keys
                        )
                        shortcutString += ((Keys)vKey).ToString();

                    else if (vKey >= 255)
                        shortcutString += "sc" + keyStruct.scanCode.ToString("{0:x3}");

                    else
                        shortcutString += "vk" + vKey.ToString("{0:x2}");
                    break;
            }

            shortcutString += "}";
            textBox1.Text += "||" + shortcutString;
            // Return single character
            if (shortcutString.Length == 3)
            {
                char[] shortcutChars = shortcutString.ToCharArray();
                textBox1.Text.Insert(textBox1.SelectionStart, shortcutChars[1].ToString());
                return;
            }

            //// /////

            ShortcutStringOffset = textBox1.SelectionStart;

            if (cutOff)
            {
                textBox1.Text = textBox1.Text.Remove(ShortcutStringOffset, ShortcutStringLength);
                ShortcutStringLength = 0;
                textBox1.Text = textBox1.Text.Insert(ShortcutStringOffset, shortcutString);
                ShortcutStringActive = false;
            }
            else
            {
                // The difference is if you're NOT cutting off, you switch the
                // caret to the beginning, so you can erase the old shortcut string later.
                ShortcutStringLength = shortcutString.Length;
                textBox1.Text = textBox1.Text.Remove(ShortcutStringOffset, ShortcutStringLength);
                textBox1.Text = textBox1.Text.Insert(ShortcutStringOffset, shortcutString);
                textBox1.SelectionStart = ShortcutStringOffset;
                ShortcutStringActive = true;
            }

            textBox1.Text += "||" + shortcutString;
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(nCode < 0) return CallNextHookEx(_hookID, nCode, wParam, lParam);
            KBDLLHOOKSTRUCT kbdStruct = new KBDLLHOOKSTRUCT();
            Marshal.PtrToStructure(lParam, kbdStruct);
            
            if (wParam == (IntPtr)WM_KEYDOWN)
            {
                switch (kbdStruct.vkCode)
                {
                    case VK_LCTRL:
                        ctrlKey = true;
                        break;
                    case VK_RCTRL:
                        rCtrlKey = true;
                        break;
                    case VK_LMENU:
                        altKey = true;
                        break;
                    case VK_RMENU:
                        rAltKey = true;
                        break;
                    case VK_LSHIFT:
                        shiftKey = true;
                        break;
                    case VK_RSHIFT:
                        rShiftKey = true;
                        break;
                    case VK_LWIN:
                        winKey = true;
                        break;
                    case VK_RWIN:
                        rWinKey = true;
                        break;
                }
                UpdateShortcutString(kbdStruct, false);
            }
            else if (wParam == (IntPtr)WM_KEYUP)
            {
                UpdateShortcutString(kbdStruct, true);
                switch (kbdStruct.vkCode)
                {
                    case VK_LCTRL:
                        ctrlKey = false;
                        break;
                    case VK_RCTRL:
                        rCtrlKey = false;
                        break;
                    case VK_LMENU:
                        altKey = false;
                        break;
                    case VK_RMENU:
                        rAltKey = false;
                        break;
                    case VK_LSHIFT:
                        shiftKey = false;
                        break;
                    case VK_RSHIFT:
                        rShiftKey = false;
                        break;
                    case VK_LWIN:
                        winKey = false;
                        break;
                    case VK_RWIN:
                        rWinKey = false;
                        break;
                }
            }

            // ////////

            return (IntPtr)1;

            bool Alt = (System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != 0;
            bool Control = (System.Windows.Forms.Control.ModifierKeys & Keys.Control) != 0;

            //Prevent ALT-TAB and CTRL-ESC by eating TAB and ESC. Also kill Windows Keys.
            int vkCode = Marshal.ReadInt32(lParam);
            Keys key = (Keys)vkCode;

            if (Alt && key == Keys.F4)
            {
                //Application.Current.Shutdown();
                return (IntPtr)1; //handled
            }
            if (key == Keys.LWin || key == Keys.RWin) return (IntPtr)1; //handled
            if (Alt && key == Keys.Tab) return (IntPtr)1; 
            if (Alt && key == Keys.Space) return (IntPtr)1; 
            if (Control && key == Keys.Escape) return (IntPtr)1;
            if (key == Keys.None) return (IntPtr)1; 
            if (key <= Keys.Back) return (IntPtr)1; 
            if (key == Keys.Menu) return (IntPtr)1; 
            if (key == Keys.Pause) return (IntPtr)1; 
            if (key == Keys.Help) return (IntPtr)1; 
            if (key == Keys.Sleep) return (IntPtr)1; 
            if (key == Keys.Apps) return (IntPtr)1; 
            if (key >= Keys.KanaMode && key <= Keys.HanjaMode) return (IntPtr)1; 
            if (key >= Keys.IMEConvert && key <= Keys.IMEModeChange) return (IntPtr)1; 
            if (key >= Keys.BrowserBack && key <= Keys.BrowserHome) return (IntPtr)1; 
            if (key >= Keys.MediaNextTrack && key <= Keys.OemClear) return (IntPtr)1; 

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

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
        static extern int GetAsyncKeyState(int vKey);

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
        public enum KBDLLHOOKSTRUCTFlags : int {
            LLKHF_EXTENDED = 0x01,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80,
        }
        #endregion
    }
}
