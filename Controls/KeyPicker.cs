using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTMulti.Controls
{
    public partial class KeyPicker : UserControl
    {
        private const string DISABLED_FOCUSED_TEXT = "Disabled - press a key";
        private const string DISABLED_UNFOCUSED_TEXT = "Disabled - click here";

        public delegate void KeyChosenHandler(KeyPicker chooser, Keys keyChosen);

        public event KeyChosenHandler KeyChosen;

        Keys _key = Keys.None;

        bool isActive = false;

        static Dictionary<Keys, string> alternateKeyTexts = new Dictionary<Keys, string>()
        {
            {Keys.Oemcomma, "Comma"},
            {Keys.OemPeriod, "Period"},
            {Keys.OemOpenBrackets, "Open Brackets"},
            {Keys.OemCloseBrackets, "Close Brackets"},
            {Keys.OemBackslash, "Backslash"},
            {Keys.OemQuotes, "Quote"},
            {Keys.OemSemicolon, "Semicolon"},
            {Keys.OemQuestion, "Forward Slash"},
            {Keys.OemMinus, "Minus"},
            {Keys.Oemplus, "Equals"},
            {Keys.Oemtilde, "Tilde"},
            {Keys.Menu, "Alt"},
            {Keys.ShiftKey, "Shift"},
            {Keys.ControlKey, "Control"},
            {Keys.D1, "1"},
            {Keys.D2, "2"},
            {Keys.D3, "3"},
            {Keys.D4, "4"},
            {Keys.D5, "5"},
            {Keys.D6, "6"},
            {Keys.D7, "7"},
            {Keys.D8, "8"},
            {Keys.D9, "9"},
            {Keys.D0, "0"},
            {Keys.Left, "LeftArrow"},
            {Keys.Right, "RightArrow"},
            {Keys.Down, "DownArrow"},
            {Keys.Up, "UpArrow"},
            {Keys.Back, "Backspace"},
            {Keys.Capital, "CapsLock"},
            {Keys.Next, "PageDown"}
        };

        static KeyPicker()
        {
            alternateKeyTexts[Keys.Oem5] = alternateKeyTexts[Keys.OemBackslash];
            alternateKeyTexts[Keys.Oem6] = alternateKeyTexts[Keys.OemCloseBrackets];
            alternateKeyTexts[Keys.Oem1] = alternateKeyTexts[Keys.OemSemicolon];
            alternateKeyTexts[Keys.Oem7] = alternateKeyTexts[Keys.OemQuotes];
        }

        [Browsable(true)]
        public Keys ChosenKey
        {
            get { return _key; }
            set
            {
                _key = value;

                this.InvokeIfRequired(() =>
                {
                    string text = _key.ToString();

                    if (alternateKeyTexts.ContainsKey(_key))
                    {
                        text = alternateKeyTexts[_key];
                    }
                    else if (_key == Keys.None)
                    {
                        text = isActive ? DISABLED_FOCUSED_TEXT : DISABLED_UNFOCUSED_TEXT;
                    }

                    textBox1.Text = text;

                    if (_key == Keys.None)
                    {
                        textBox1.Font = new Font(textBox1.Font, FontStyle.Italic);
                    }
                    else
                    {
                        textBox1.Font = new Font(textBox1.Font, FontStyle.Regular);
                    }
                });
            }
        }

        [Browsable(true)]
        public int ChosenKeyCode
        {
            get
            {
                return (int)_key;
            }
            set
            {
                ChosenKey = (Keys)value;
            }
        }

        public KeyPicker()
        {
            InitializeComponent();
            textBox1.Text = _key.ToString();
            textBox1.Enter += TextBox1_Enter;
            textBox1.Leave += TextBox1_Leave;
        }

        private void TextBox1_Leave(object sender, EventArgs e)
        {
            isActive = false;

            if (ChosenKey == Keys.None)
            {
                textBox1.Text = DISABLED_UNFOCUSED_TEXT;
            }
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            isActive = true;

            if (ChosenKey == Keys.None)
            {
                textBox1.Text = DISABLED_FOCUSED_TEXT;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Tab:
                case Keys.Escape:
                case Keys.Enter:
                    keyDown(keyData);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            keyDown(e.KeyCode);
            e.SuppressKeyPress = true;
        }

        private void keyDown(Keys key)
        {
            ChosenKey = key;
            KeyChosen?.Invoke(this, ChosenKey);
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            keyDown(Keys.None);
        }
    }
}
