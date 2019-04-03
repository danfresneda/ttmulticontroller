using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTMulti.Controls
{
    public partial class ControlsPicker : UserControl
    {
        public delegate void KeyMappingAddedHandler(object sender, KeyMapping keyMapping);
        public delegate void KeyMappingRemovedHandler(object sender, int row);

        public event EventHandler KeyMappingsChanged;
        public event KeyMappingAddedHandler KeyMappingAdded;
        public event KeyMappingRemovedHandler KeyMappingRemoved;

        private List<KeyMapping> keyMappings = new List<KeyMapping>();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<KeyMapping> KeyMappings {
            get
            {
                return keyMappings;
            }
            set
            {
                this.InvokeIfRequired(() =>
                {
                    while (tableLayoutPanel1.RowCount > 1)
                    {
                        removeRow(1);
                    }

                    foreach (KeyMapping keyMapping in value)
                    {
                        AddMapping(keyMapping);
                    }
                });
            }
        }

        public ControlsPicker()
        {
            InitializeComponent();
        }

        public void AddMapping(KeyMapping keyMapping)
        {
            this.InvokeIfRequired(() => {
                int rowNum = addRow(keyMapping);
                keyMappings.Add(keyMapping);
                KeyMappingAdded?.Invoke(this, keyMapping);
                KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
                tableLayoutPanel1.ScrollControlIntoView(tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 1]);
            });
        }

        public void RemoveMapping(int index)
        {
            removeRow(index + 1);
            keyMappings.RemoveAt(index);
            KeyMappingRemoved?.Invoke(this, index);
            KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
        }
        
        int addRow(KeyMapping keyMapping)
        {
            Point scrollPosition = this.AutoScrollPosition;
            Control label;

            if (keyMapping.ReadOnly)
            {
                label = new Label() { Text = keyMapping.Title.TrimEnd(':') + ":", Dock = DockStyle.Fill, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };
            }
            else
            {
                TextBox textbox = new TextBox() { Text = keyMapping.Title.TrimEnd(':') + ":", Dock = DockStyle.Fill };
                textbox.TextChanged += BindingTitle_TextChanged;
                textbox.Focus();
                textbox.SelectAll();
                label = textbox;
            }

            KeyPicker bindingKeyPicker = new KeyPicker() { Dock = DockStyle.Top, ChosenKey = keyMapping.Key },
                leftToonKeyPicker = new KeyPicker() { Dock = DockStyle.Top, ChosenKey = keyMapping.LeftToonKey },
                rightToonKeyPicker = new KeyPicker() { Dock = DockStyle.Top, ChosenKey = keyMapping.RightToonKey };
            Button removeBtn = new Button() { Text = "Remove", AutoSize = true,  Enabled = !keyMapping.ReadOnly };

            bindingKeyPicker.KeyChosen += KeyChooser_KeyChosen;
            removeBtn.Click += RemoveBtn_Click;
            
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel1.RowCount++;

            int rowNum = tableLayoutPanel1.RowCount - 1;

            tableLayoutPanel1.Controls.Add(label, 0, rowNum);
            tableLayoutPanel1.Controls.Add(bindingKeyPicker, 1, rowNum);
            tableLayoutPanel1.Controls.Add(leftToonKeyPicker, 2, rowNum);
            tableLayoutPanel1.Controls.Add(rightToonKeyPicker, 3, rowNum);
            tableLayoutPanel1.Controls.Add(removeBtn, 4, rowNum);
            tableLayoutPanel1.ResumeLayout();
            this.AutoScrollPosition = new Point(Math.Abs(scrollPosition.X), Math.Abs(scrollPosition.Y));

            return rowNum;
        }

        private void removeRow(int rowNum)
        {
            Point scrollPosition = this.AutoScrollPosition;
            tableLayoutPanel1.SuspendLayout();

            for (int c = 0; c < tableLayoutPanel1.ColumnCount; c++)
            {
                Control control = tableLayoutPanel1.GetControlFromPosition(c, rowNum);

                if (control != null)
                {
                    tableLayoutPanel1.Controls.Remove(control);
                }
            }

            for (int r = rowNum + 1; r < tableLayoutPanel1.RowCount; r++)
            {
                for (int c = 0; c < tableLayoutPanel1.ColumnCount; c++)
                {
                    Control control = tableLayoutPanel1.GetControlFromPosition(c, r);

                    if (control != null)
                    {
                        tableLayoutPanel1.SetRow(control, r - 1);
                    }
                }
            }

            tableLayoutPanel1.RowStyles.RemoveAt(rowNum);
            tableLayoutPanel1.RowCount--;
            tableLayoutPanel1.ResumeLayout();
            this.AutoScrollPosition = new Point(Math.Abs(scrollPosition.X), Math.Abs(scrollPosition.Y));
        }

        private void ControlsPicker_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                KeyMappings = new List<KeyMapping>()
                {
                    new KeyMapping("Test", Keys.None, Keys.None, Keys.None, true),
                    new KeyMapping("Test", Keys.None, Keys.None, Keys.None, true),
                    new KeyMapping("Test", Keys.A, Keys.None, Keys.D, true),
                    new KeyMapping("Test", Keys.None, Keys.None, Keys.None, true),
                    new KeyMapping("Test", Keys.None, Keys.B, Keys.None, true),
                    new KeyMapping("Test", Keys.None, Keys.None, Keys.None, true),
                    new KeyMapping("Test", Keys.None, Keys.None, Keys.C, true),
                    new KeyMapping("Test", Keys.None, Keys.None, Keys.None, true)
                };
            }
        }

        private void KeyChooser_KeyChosen(KeyPicker keyChooser, Keys keyChosen)
        {
            int rowNum = tableLayoutPanel1.GetRow(keyChooser);
            keyMappings[rowNum - 1].Key = keyChosen;
            KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BindingTitle_TextChanged(object sender, EventArgs e)
        {
            int rowNum = tableLayoutPanel1.GetRow((Control)sender);
            keyMappings[rowNum - 1].Title = (sender as Control).Text.Trim(':');
            KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            var position = tableLayoutPanel1.GetPositionFromControl((Control)sender);

            RemoveMapping(position.Row - 1);
        }
    }

    [Serializable]
    public class KeyMapping
    {
        public string Title { get; set; }
        public Keys Key { get; set; }
        public Keys LeftToonKey { get; set; }
        public Keys RightToonKey { get; set; }
        public bool ReadOnly { get; set; }

        public KeyMapping() { }

        public KeyMapping(string title, Keys key, Keys leftToonKey, Keys rightToonKey, bool readOnly)
        {
            Title = title;
            Key = key;
            LeftToonKey = leftToonKey;
            RightToonKey = rightToonKey;
            ReadOnly = readOnly;
        }
    }
}
