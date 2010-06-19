using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyboardTextBox
{
    public partial class KeyboardTextBox : TextBox
    {
        #region Allowed Keys Fields
        private bool _allowAltTab = true; // Alt+Tab
        private bool _allowNavigation = true; // Up/Down/Left/Right/Home/End/Ctrl+All
        private bool _allowDelete = true; // Backspace/Delete/Ctrl+All
        private bool _allowTabKeys = true; // Tab/Shift+Tab
        private bool _chainAllowedKeys = true;
        
        [Description("Passes Alt+Tab to the system, instead of recording it."),
        Category("Allowed Keys"),
        DefaultValue(true),
        Browsable(true)]
        public bool AllowAltTab {
            get { return _allowAltTab; }
            set { _allowAltTab = value; }
        }

        [Description("Lets the user use the arrow keys, Home, and End, to scroll the text."),
        Category("Allowed Keys"),
        DefaultValue(true),
        Browsable(true)]
        public bool AllowNavigation
        {
            get { return _allowNavigation; }
            set { _allowNavigation = value; }
        }
        
        //public bool AllowClipboard = false; // Ctrl+X/C/V/Z/Y/Ctrl+Ins/Shift+Ins/Shift+Del

        [Description("Lets the user use Backspace and Delete, to remove text."),
        Category("Allowed Keys"),
        DefaultValue(true),
        Browsable(true)]
        public bool AllowDelete
        {
            get { return _allowAltTab; }
            set { _allowDelete = value; }
        }

        [Description("Lets the user use Tab to switch to the next control."),
        Category("Allowed Keys"),
        DefaultValue(true),
        Browsable(true)]
        public bool AllowTabKeys
        {
            get { return _allowAltTab; }
            set { _allowTabKeys = value; }
        }

        [Description("Records special keys in keyboard shortcut chains."),
        Category("Allowed Keys"),
        DefaultValue(true),
        Browsable(true)]
        public bool ChainAllowedKeys
        {
            get { return _allowAltTab; }
            set { _chainAllowedKeys = value; }
        }


        #endregion

        #region Chain Status Fields
        private KeyboardHook WinHook;
        private bool StatusCtrl = false;
        private bool StatusAlt = false;
        private bool StatusShift = false;
        private bool StatusWin = false;
        private bool StatusSingle = false;
        private bool StatusChain = false;
        private int ShortcutStringPos = 0;
        private int ShortcutStringLength = 0;
        private Form Parent;
        #endregion

        public KeyboardTextBox()
        {
            // Technically you don't NEED a parent form,
            // but it's best to have one so you can unhook
            // the keyboard LL hook when window is inactive.
        }

        public void SetParent(Form parentForm)
        {
            Parent = parentForm;
            Parent.FormClosing += new FormClosingEventHandler(Parent_FormClosing);
            Parent.Activated += new EventHandler(Parent_Activated);
            Parent.Deactivate += new EventHandler(Parent_Deactivate);
        }

        #region Text Input
        private void ProcessKeyPress(int vkCode)
        {
            Keys keyCode = (Keys)vkCode;

            switch (keyCode)
            {
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    if (!StatusCtrl && !StatusAlt && !StatusShift && !StatusWin)
                        StatusSingle = true;
                    else
                        StatusSingle = false;

                    StatusCtrl = true;
                    goto specialkey;
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                    if (!StatusCtrl && !StatusAlt && !StatusShift && !StatusWin)
                        StatusSingle = true;
                    else
                        StatusSingle = false;

                    StatusAlt = true;
                    goto specialkey;
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    if (!StatusCtrl && !StatusAlt && !StatusShift && !StatusWin)
                        StatusSingle = true;
                    else
                        StatusSingle = false;

                    StatusShift = true;
                    goto specialkey;
                case Keys.LWin:
                case Keys.RWin:
                    if (!StatusCtrl && !StatusAlt && !StatusShift && !StatusWin)
                        StatusSingle = true;
                    else
                        StatusSingle = false;

                    StatusWin = true;
                    goto specialkey;

                specialkey:
                    //StatusChain = true;
                    UpdateChain();
                    break;

                // If any other key is pressed, check for ctrl/alt/shift/win and complete the chain.
                default:
                    if (StatusChain) // If StatusChain, do things for special keys and complete chain
                    {
                        // No matter what key it is, add it on and suppress the key press
                        string completeChain = Text.Insert(ShortcutStringPos + ShortcutStringLength, ReturnKeyName(vkCode) + "]");
                        Text = completeChain;
                        SelectionStart = ShortcutStringPos + ShortcutStringLength + ReturnKeyName(vkCode).Length + 1;
                        StatusChain = false;
                        StatusSingle = false;
                    }
                    else
                    {
                        // If the key is alphabetical, capitalize it.
                        int vKey = vkCode;
                        string charString = "";

                        if (vKey == 0x14) break; // Ignore capslock by itself
                        else if (vKey >= 0x41 && vKey <= 0x5A)
                            charString = keyCode.ToString().ToUpper();
                        /*else if (vKey == 0x20) // Spacebar. Else, becomes {Space}
                            charString = " ";*/
                        else
                            charString = ReturnKeyName(vkCode);

                        string textboxtext = Text;
                        int textboxcaret = SelectionStart;
                        // TODO: Processing EVERY key through the hook means only allowing
                        // a subset of all keys available. MUST be fixed along with i18n!
                        textboxtext = Text.Insert(textboxcaret, charString);
                        Text = textboxtext;
                        SelectionStart = textboxcaret + charString.Length;
                        break;
                    }
                    break;
            }
        }

        private void ProcessKeyPressUp(int vkCode)
        {
            switch ((Keys)vkCode)
            {
                // If StatusSingle is true, keep the {Ctrl} String. Else, erase the incomplete string.
                // If a chain was complete, everything would already be false.
                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                    StatusCtrl = false;
                    goto specialkey;
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                    StatusAlt = false;
                    goto specialkey;
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:
                    StatusShift = false;
                    goto specialkey;
                case Keys.LWin:
                case Keys.RWin:
                    StatusWin = false;
                    goto specialkey;

                specialkey:
                    UpdateChain();
                    if (!StatusCtrl && !StatusAlt && !StatusShift && !StatusWin) StatusChain = false;
                    break;
            }
        }

        private void UpdateChain()
        {
            if (StatusChain && !StatusSingle)
            {
                Text = Text.Remove(ShortcutStringPos, ShortcutStringLength);
                SelectionStart = ShortcutStringPos;
            }
            else if (StatusChain && StatusSingle)
            {
                ShortcutStringPos = SelectionStart;
                string singleText = Text.Insert(ShortcutStringPos + ShortcutStringLength - 1, "]");
                singleText = singleText.Remove(ShortcutStringPos + ShortcutStringLength, 1);
                Text = singleText;
                SelectionStart = ShortcutStringPos + ShortcutStringLength;
            }

            // Start the shortcut string.
            string shortcutString = "[";
            if (StatusCtrl) shortcutString += "Ctrl+";
            if (StatusAlt) shortcutString += "Alt+";
            if (StatusWin) shortcutString += "Win+";
            if (StatusShift) shortcutString += "Shift+";

            // If shortcutString is effectively empty, make it so.
            // This will happen if no keys are pressed.
            if (shortcutString.Length <= 1) shortcutString = "";

            // Update ShortcutStringLength, insert shortcutString into TextBox,
            // and move caret pos to ShortcutStringPos.
            ShortcutStringLength = shortcutString.Length;
            ShortcutStringPos = SelectionStart;
            Text = Text.Insert(ShortcutStringPos, shortcutString);
            SelectionStart = ShortcutStringPos;

            StatusChain = true;
        }

        private string ReturnKeyName(int keyCode)
        {
            // Generally, most keys are OK as-is. Just watch for the OEM punctuations,
            // and certain special keys.
            switch (keyCode)
            {
                // Digits
                case 0x30: return "0";
                case 0x31: return "1";
                case 0x32: return "2";
                case 0x33: return "3";
                case 0x34: return "4";
                case 0x35: return "5";
                case 0x36: return "6";
                case 0x37: return "7";
                case 0x38: return "8";
                case 0x39: return "9";

                // Punctuation, OEM keys
                case 0xBA: return ";"; // Semicolon for US keyboards
                case 0xBB: return "{+}"; // AHK needs {}
                case 0xBC: return ",";
                case 0xBD: return "-";
                case 0xBE: return ".";
                case 0xBF: return "/"; // /? for US keyboards
                case 0xC0: return "`"; // `~ for US keyboards
                case 0xDB: return "{[}"; // [{ for US keyboards
                case 0xDC: return "\\"; // \| for US keyboards
                case 0xDD: return "{]}"; // ]} for US keyboards
                case 0xDE: return "'"; // '" for US keyboards

                // capital, return, back, next (pgdn)
                case 0x14: return "{CapsLock}";
                case 0x0D: return "{Enter}";
                case 0x08: return "{Backspace}";
                case 0x22: return "{PgDn}";

                // Other special keys that need to be bracketed
                //case 0x0D: return "{Enter}";//VK_ENTER
                case 0x1B: return "{Escape}";//VK_ESCAPE
                case 0x20: return "{Space}"; //VK_SPACE

                case 0x09: return "{Tab}";//VK_TAB
                case 0x2E: return "{Delete}";//VK_DELETE
                case 0x2D: return "{Insert}";//VK_INSERT
                case 0x24: return "{Home}";//VK_HOME
                case 0x23: return "{End}";//VK_END
                case 0x21: return "{PgUp}";//VK_PRIOR/PAGEUP

                //case 0x14: return "{CapsLock}";//VK_CAPITAL/CAPSLOCK
                case 0x91: return "{ScrollLock}";//VK_SCROLL
                case 0x90: return "{NumLock}";//VK_NUMLOCK
                case 0x2A: return "{PrintScreen}";//VK_PRINT/PRINTSCREEN

                case 0x13: return "{Pause}";//VK_PAUSE

                case 0x70: //VK_F1
                    return "{F1}";
                case 0x71: //VK_F2
                    return "{F2}";
                case 0x72: //VK_F1
                    return "{F3}";
                case 0x73: //VK_F1
                    return "{F4}";
                case 0x74: //VK_F1
                    return "{F5}";
                case 0x75: //VK_F1
                    return "{F6}";
                case 0x76: //VK_F1
                    return "{F7}";
                case 0x77: //VK_F1
                    return "{F8}";
                case 0x78: //VK_F1
                    return "{F9}";
                case 0x79: //VK_F1
                    return "{F10}";
                case 0x80: //VK_F1
                    return "{F11}";
                case 0x81: //VK_F1
                    return "{F12}";

                default: return ((Keys)keyCode).ToString();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Alt) e.SuppressKeyPress = true;

            // Handle deletion
            if (AllowDelete && !StatusChain
                && (e.KeyCode == Keys.Delete
                || e.KeyCode == Keys.Back))
            {
                // Are we between a {}? Move to the next }


                // Are we between a []? Move to the next ]

                // If behind a }, delete everything from } to {

                // If behind a ], delete everything from ] to [
            }

            // Key exceptions
            if (AllowDelete && StatusChain
                && (e.KeyCode == Keys.Delete
                || e.KeyCode == Keys.Back))
            {
                e.SuppressKeyPress = true;
                if (ChainAllowedKeys) { ProcessKeyPress((int)e.KeyCode); }
            }

            if (AllowNavigation && StatusChain
                && (((int)e.KeyCode) >= 0x21 && ((int)e.KeyCode) <= 0x29))
            {
                e.SuppressKeyPress = true;
                if (ChainAllowedKeys) { ProcessKeyPress((int)e.KeyCode); }
            }

            // Only works with Multiline and AcceptTab set to true
            if (AllowTabKeys && ((int)e.KeyCode) == 0x09)
            {
                e.SuppressKeyPress = true;
                if (StatusChain)
                {
                    Text = StatusShift.ToString();
                    if (StatusShift)
                    {
                        StatusCtrl = StatusShift = StatusWin = StatusAlt = false;
                        StatusSingle = false;
                        UpdateChain(); StatusChain = false;
                        if(Parent != null) Parent.SelectNextControl(this, false, true, true, true);
                    }
                    else if (ChainAllowedKeys) { ProcessKeyPress((int)e.KeyCode); }
                }
                else if(Parent != null) Parent.SelectNextControl(this, true, true, true, true);
            }
        }
        #endregion

        #region Hook Events
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            CreateWinHook();
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            DestroyWinHook();
        }

        // Form activated, deactivated, closing
        void Parent_Activated(object sender, EventArgs e)
        {
            if (Focused) CreateWinHook();
        }

        void Parent_Deactivate(object sender, EventArgs e)
        {
            DestroyWinHook();
        }

        void Parent_FormClosing(object sender, FormClosingEventArgs e)
        {
            DestroyWinHook();
        }
        #endregion

        #region Utilities
        private void CreateWinHook()
        {
            if (WinHook != null) return;
            if (AllowAltTab) WinHook = new KeyboardHook(KeyboardHook.Parameters.AllowAltTab);
            else WinHook = new KeyboardHook();
            WinHook.KeyIntercepted += new KeyboardHook.KeyboardHookEventHandler(WinHook_KeyIntercepted);

            WinHook.AllowNavigation = AllowNavigation;
            WinHook.AllowDelete = AllowDelete;
            WinHook.AllowTabKeys = AllowTabKeys;

            if (StatusChain)
            {
                StatusCtrl = StatusAlt = StatusShift = StatusWin = StatusSingle = false;
                UpdateChain();
                StatusChain = false;
            }
        }

        private void DestroyWinHook()
        {
            if (WinHook != null) { WinHook.Dispose(); WinHook = null; }
        }

        void WinHook_KeyIntercepted(KeyboardHook.KeyboardHookEventArgs e)
        {
            if (e.PassThrough)
            {
                StatusCtrl = StatusAlt = StatusShift = StatusWin = StatusSingle = false;
                //UpdateChain();
                return;
            }

            if (!e.IsKeyUp) ProcessKeyPress(e.KeyCode);
            else ProcessKeyPressUp(e.KeyCode);
        }
        #endregion
    }
}
