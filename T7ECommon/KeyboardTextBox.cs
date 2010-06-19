using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace T7ECommon
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
                        string completeChain = Text.Insert(ShortcutStringPos + ShortcutStringLength, ReturnKeyName(vkCode) + "] ");
                        Text = completeChain;
                        SelectionStart = ShortcutStringPos + ShortcutStringLength + ReturnKeyName(vkCode).Length + 2;
                        StatusChain = false;
                        StatusSingle = false;
                    }
                    else
                    {
                        // If the key is alphabetical, capitalize it.
                        int vKey = vkCode;
                        string charString = "";

                        if (vKey == 0x14) break; // Ignore capslock by itself
                        else if (vKey == 0x90) break; // and numlock
                        else if (vKey >= 0x41 && vKey <= 0x5A)
                            charString = keyCode.ToString().ToUpper() + " ";
                        /*else if (vKey == 0x20) // Spacebar. Else, becomes {Space}
                            charString = " ";*/
                        else
                            charString = ReturnKeyName(vkCode) + " ";

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
                if(ShortcutStringPos + ShortcutStringLength <= Text.Length) Text = Text.Remove(ShortcutStringPos, ShortcutStringLength);
                SelectionStart = ShortcutStringPos <= Text.Length ? ShortcutStringPos : Text.Length;
            }
            else if (StatusChain && StatusSingle)
            {
                ShortcutStringPos = SelectionStart;
                string singleText = Text.Insert(ShortcutStringPos + ShortcutStringLength - 1, "] ");
                singleText = singleText.Remove(ShortcutStringPos + ShortcutStringLength + 1, 1);
                Text = singleText;
                SelectionStart = ShortcutStringPos + ShortcutStringLength + 1;
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
                case 0x03: return "{CtrlBreak}";

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

                case 0xAD:
                    return "{Volume_Mute}";
                case 0xAE:
                    return "{Volume_Down}";
                case 0xAF:
                    return "{Volume_Up}";

                case 0xB0:
                    return "{Media_Next}";
                case 0xB1:
                    return "{Media_Prev}";
                case 0xB2:
                    return "{Media_Stop}";
                case 0xB3:
                    return "{Media_Play_Pause}";

                case 0x5D:
                    return "{AppsKey}";

                // Numpad keys
                case 0x60:
                    return "{Numpad0}";
                case 0x61:
                    return "{Numpad1}";
                case 0x62:
                    return "{Numpad2}";
                case 0x63:
                    return "{Numpad3}";
                case 0x64:
                    return "{Numpad4}";
                case 0x65:
                    return "{Numpad5}";
                case 0x66:
                    return "{Numpad6}";
                case 0x67:
                    return "{Numpad7}";
                case 0x68:
                    return "{Numpad8}";
                case 0x69:
                    return "{Numpad9}";
                case 0x6A:
                    return "{NumpadMult}";
                case 0x6B:
                    return "{NumpadAdd}";
                case 0x6C:
                    return "{NumpadEnter}";
                case 0x6D:
                    return "{NumpadSub}";
                case 0x6E:
                    return "{NumpadDot}";
                case 0x6F:
                    return "{NumpadDiv}";

                default:
                    string keyString = ((Keys)keyCode).ToString();
                    // Wrap in {}s, precede capital letters with _
                    //keyString = Regex.Replace(keyString, @"(\b[a-z]|\B[A-Z])", new MatchEvaluator(CamelCasetoTitleCase));
                    if(keyString.Length <= 1) return keyString; else return "{"+keyString+"}";
            }
        }

        public string CamelCasetoTitleCase(Match m)
        {
            char c = m.Captures[0].Value[0];
            return ((c >= 'a') && (c <= 'z')) ? Char.ToUpper(c).ToString() : "_" + c;
        }

        private void DeleteKeyBlock(bool deleteKey)
        {
            // deleteKey determines

        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Alt) e.SuppressKeyPress = true;

            // Handle deletion
            if (AllowDelete && !StatusChain)
            {
                if (e.KeyCode == Keys.Back)
                    ProcessBackspace();
                else if (e.KeyCode == Keys.Delete)
                    ProcessDelete();

                e.SuppressKeyPress = true;
            }

            if (AllowNavigation && !StatusChain)
            {
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up)
                    SeekBeforeKeyBlock();
                else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Down)
                    SeekAfterKeyBlock();
                else if (e.KeyCode == Keys.Home) SelectionStart = 0;
                else if (e.KeyCode == Keys.End) SelectionStart = Text.Length;

                e.SuppressKeyPress = true;
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

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);

            if (IsNextBlockCloser()) SeekAfterKeyBlock();
            else SeekBeforeKeyBlock();
        }

        private void ProcessBackspace()
        {
            // Backspace: If you select between a keyblock,
            // Move to the NEXT space (or end of string)
            // and remove the PRECEDING keyblock behind the space
            // _lo|lcats_ ==> _lolcats_| ==> _|(nomorelolcats_)

            int caretPos = SelectionStart;
            
            if(caretPos <= 0) return; // Don't delete if beginning of string

            // Check if prior character is a space
            if (!Text[caretPos - 1 >= 0 ? caretPos - 1 : 0].Equals(' '))
            {
                int i = caretPos;
                // Move to the next found space character, or end of string
                do
                {
                    // lo|lcats_
                    i++;
                    caretPos = i;
                } while (i < Text.Length && !Text[i].Equals(' '));
                // Jump behind the space, if applicable
                caretPos = caretPos + 1 <= Text.Length ? caretPos + 1 : caretPos;
            }

            // Delete characters 'til the next preceding space, or beginning of string
            // lolcats_|
            if (Text[caretPos-1].Equals(' '))
            {
                Text = Text.Remove(caretPos-1, 1);
                caretPos = caretPos-2;
            } 
            for (int i = caretPos >= Text.Length ? Text.Length-1 : caretPos
                ; i >= 0 && !Text[i].Equals(' '); i--)
            {
                Text = Text.Remove(i, 1);
                caretPos = i;
            }

            // Set SelectionStart to caretPos
            SelectionStart = caretPos >= 0 ? caretPos : 0;
        }

        private void ProcessDelete()
        {
            // Delete: If you select between a keyblock,
            // Move to the PREVIOUS space (or beginning of string)
            // and remove the ANTECEDING keyblock ahead of the space
            // _lo|lcats_ ==> _|lolcats_ ==> _|(nomorelolcats_)\

            int caretPos = SelectionStart;

            if (caretPos >= Text.Length) return; // Don't delete if end of string

            // Check if prior character is a space
            if (caretPos > 0 && !Text[caretPos - 1 >= 0 ? caretPos - 1 : 0].Equals(' '))
            {
                // Move to the next found space character, or end of string
                for(int i = caretPos; i >= 0 && !Text[i].Equals(' '); i--)
                {
                    // _lo|lcats
                    caretPos = i;
                }
                // Jump behind the space, if applicable
                //caretPos = caretPos + 1 <= Text.Length ? caretPos + 1 : caretPos;
            }

            // Delete characters 'til the next preceding space, or beginning of string
            // lolcats_|
            caretPos = caretPos - 1 >= 0 ? caretPos-1 : 0;
            if (Text[caretPos].Equals(' '))
            {
                Text = Text.Remove(caretPos, 1);
            }
            int j = caretPos;
            do
            {
                Text = Text.Remove(j, 1);
            } while (j < Text.Length && !Text[j].Equals(' '));
            if (caretPos == 0 && Text[0].Equals(' ')) { 
                Text = Text.Remove(0, 1);
                SelectionStart = 0;
            }
            else // Set SelectionStart to caretPos
                SelectionStart = caretPos+1 < Text.Length ? caretPos+1 : Text.Length;
        }

        private bool IsNextBlockCloser()
        {
            int caretPos = SelectionStart;
            if (caretPos <= 0 || caretPos >= Text.Length) return true;

            // Determine distance to next code block
            int nextDist = 0;
            for (int i = caretPos; i <= Text.Length && !Text[i].Equals(' '); i++)
            {
                nextDist++;
            }

            // Determine distance to previous code block
            int prevDist = 0;
            for (int i = caretPos; i >= 0 && !Text[i].Equals(' '); i--)
            {
                prevDist++;
            }
            
            if (nextDist <= prevDist) return true;
            else return false;
        }

        private void SeekBeforeKeyBlock()
        {
            int caretPos = SelectionStart;
            //if (SelectionLength > 0) caretPos = SelectionStart + SelectionLength;
            if (caretPos <= 0) return; // Already at beginning of sting

            // Check if prior character is a space
            if (caretPos > 0 && Text[caretPos - 1 >= 0 ? caretPos - 1 : 0].Equals(' '))
            {
                caretPos = caretPos - 2 >= 0 ? caretPos - 2 : 0;
            }
            // Move to the next found space character, or end of string
            for (int i = caretPos; i >= 0 && !Text[i].Equals(' '); i--)
            {
                // _lo|lcats
                caretPos = i;
            }

            SelectionStart = caretPos;
            SelectionLength = 0; // TODO: Add selections?
        }

        private void SeekAfterKeyBlock()
        {
            int caretPos = SelectionStart;
            //if (SelectionLength > 0) caretPos = SelectionStart + SelectionLength;
            if (caretPos >= Text.Length) return; // Already at end of block

            // Check if prior character is a space
            //if (!Text[caretPos - 1 >= 0 ? caretPos - 1 : 0].Equals(' '))
            //{
                int i = caretPos;
                // Move to the next found space character, or end of string
                do
                {
                    // lo|lcats_
                    i++;
                    caretPos = i;
                } while (i < Text.Length && !Text[i].Equals(' '));
                // Jump behind the space, if applicable
                caretPos = caretPos + 1 <= Text.Length ? caretPos + 1 : caretPos;
            //}

            SelectionStart = caretPos;
            SelectionLength = 0; // TODO: Allow selections?
        }
        #endregion

        #region Hook Events
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            CreateWinHook();
            if (SelectionStart == Text.Length - 1) SelectionStart++; // Alt'ing into this causes caret to position wrong
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
