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
    public partial class MultiKeyChooser : UserControl
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
                return keyMappings.ToList();
            }
            set
            {
                this.InvokeIfRequired(() =>
                {
                    for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
                    {
                        removeRow(i);
                    }

                    for (int i = 0; i < value.Count; i++)
                    {
                        AddMapping(value[i].Title, value[i].Key, value[i].ReadOnly);
                    }

                    KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public IEnumerable<string> KeyMappingTitles
        {
            get
            {
                return keyMappings.Select(t => t.Title);
            }
            set
            {
                for (int r = 0; r < Math.Min(tableLayoutPanel1.RowCount, value.Count()); r++)
                {
                    keyMappings[r].Title = value.ElementAt(r);
                    tableLayoutPanel1.GetControlFromPosition(0, r).Text = value.ElementAt(r) + ":";
                }
            }
        }

        private bool showRemoveButtons = true;

        [Browsable(true)]
        public bool ShowRemoveButtons
        {
            get { return showRemoveButtons; }
            set
            {
                showRemoveButtons = value;

                this.InvokeIfRequired(() =>
                {
                    tableLayoutPanel1.ColumnStyles[tableLayoutPanel1.ColumnCount - 1].SizeType = value ? SizeType.Absolute : SizeType.AutoSize;

                    for (int r = 0; r < tableLayoutPanel1.RowCount; r++)
                    {
                        Control control = tableLayoutPanel1.GetControlFromPosition(2, r);

                        if (control != null)
                        {
                            control.Visible = showRemoveButtons;
                        }
                    }
                });
            }
        }

        public MultiKeyChooser()
        {
            InitializeComponent();
            removeRow(0);
        }

        public void AddMapping(string title, Keys key, bool readOnly)
        {
            this.InvokeIfRequired(() => {
                int rowNum = addLine(title, key, readOnly);
                KeyMapping keyMapping = new KeyMapping(title, key, readOnly);
                keyMappings.Add(keyMapping);
                KeyMappingAdded?.Invoke(this, keyMapping);
                KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
                tableLayoutPanel1.ScrollControlIntoView(tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 1]);
            });
        }

        public void RemoveMapping(int rowNum)
        {
            removeRow(rowNum);
            keyMappings.RemoveAt(rowNum);
            KeyMappingRemoved?.Invoke(this, rowNum);
            KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
        }
        
        int addLine(string title, Keys key, bool readOnly)
        {
            Point scrollPosition = this.AutoScrollPosition;
            Control label;

            if (readOnly)
            {
                label = new Label() { Text = title.TrimEnd(':') + ":", Dock = DockStyle.Fill, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };
            }
            else
            {
                TextBox textbox = new TextBox() { Text = title.TrimEnd(':') + ":", Dock = DockStyle.Fill };
                textbox.TextChanged += BindingTitle_TextChanged;
                textbox.Focus();
                textbox.SelectAll();
                label = textbox;
            }
            
            KeyChooser keyChooser1 = new KeyChooser() { Dock = DockStyle.Top, ChosenKey = key };
            Button removeBtn = new Button() { Text = "Remove", AutoSize = true,  Enabled = !readOnly, Visible = showRemoveButtons };

            keyChooser1.KeyChosen += KeyChooser_KeyChosen;
            removeBtn.Click += RemoveBtn_Click;
            
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel1.RowCount++;

            int rowNum = tableLayoutPanel1.RowCount - 1;

            tableLayoutPanel1.Controls.Add(label, 0, rowNum);
            tableLayoutPanel1.Controls.Add(keyChooser1, 1, rowNum);
            tableLayoutPanel1.Controls.Add(removeBtn, 2, rowNum);
            tableLayoutPanel1.ResumeLayout();
            this.AutoScrollPosition = new Point(Math.Abs(scrollPosition.X), Math.Abs(scrollPosition.Y));

            return rowNum;
        }

        private void KeyChooser_KeyChosen(KeyChooser keyChooser, Keys keyChosen)
        {
            int rowNum = tableLayoutPanel1.GetRow(keyChooser);
            keyMappings[rowNum].Key = keyChosen;
        }

        private void BindingTitle_TextChanged(object sender, EventArgs e)
        {
            int rowNum = tableLayoutPanel1.GetRow((Control)sender);
            keyMappings[rowNum].Title = (sender as Control).Text.Trim(':');
            KeyMappingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            var position = tableLayoutPanel1.GetPositionFromControl((Control)sender);

            RemoveMapping(position.Row);
        }

        private void removeRow(int rowNum)
        {
            //Point scrollPosition = this.AutoScrollPosition;
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
            //this.AutoScrollPosition = new Point(Math.Abs(scrollPosition.X), Math.Abs(scrollPosition.Y));
        }

        private void MultiKeyChooser_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                KeyMappings = new List<KeyMapping>()
                {
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true),
                    new KeyMapping("Test", Keys.None, true)
                };
            }
        }
    }

    [Serializable]
    public class KeyMapping
    {
        public string Title { get; set; }
        public Keys Key { get; set; }
        public bool ReadOnly { get; set; }

        public KeyMapping() { }

        public KeyMapping(string title, Keys key, bool readOnly)
        {
            Title = title;
            Key = key;
            ReadOnly = readOnly;
        }
    }
}
