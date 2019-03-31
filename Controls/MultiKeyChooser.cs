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
        [Browsable(false)]
        public List<KeyMapping> Keys {
            get
            {
                List<KeyMapping> keys = new List<KeyMapping>();

                for (int r = 1; r < tableLayoutPanel1.RowCount - 1; r++)
                {
                    string title = tableLayoutPanel1.GetControlFromPosition(0, r).Text;
                    Keys key1 = (tableLayoutPanel1.GetControlFromPosition(1, r) as KeyChooser).ChosenKey;
                    Keys key2 = (tableLayoutPanel1.GetControlFromPosition(3, r) as KeyChooser).ChosenKey;
                    bool readOnly = !tableLayoutPanel1.GetControlFromPosition(4, r).Enabled;

                    keys.Add(new KeyMapping(title, key1, key2, readOnly));
                }

                return keys;
            }
        }

        public MultiKeyChooser()
        {
            InitializeComponent();
            removeRow(1);
        }

        public void AddKey(string title, Keys key1, Keys key2, bool readOnly)
        {
            this.InvokeIfRequired(() => {
                int rowNum = addLine();
                tableLayoutPanel1.GetControlFromPosition(0, rowNum).Text = title;
                (tableLayoutPanel1.GetControlFromPosition(1, rowNum) as KeyChooser).ChosenKey = key1;
                (tableLayoutPanel1.GetControlFromPosition(3, rowNum) as KeyChooser).ChosenKey = key2;
                tableLayoutPanel1.GetControlFromPosition(4, rowNum).Enabled = !readOnly;
            });
        }
        
        int addLine()
        {
            Label label = new Label() { Text = "Custom" , Dock = DockStyle.Fill, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };
            Label arrowLabel = new Label() { Text = "→" , Dock = DockStyle.Fill, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter };
            KeyChooser keyChooser1 = new KeyChooser() { Dock = DockStyle.Fill }, 
            keyChooser2 = new KeyChooser() { Dock = DockStyle.Fill };
            Button removeBtn = new Button() { Text = "-" , Dock = DockStyle.Fill };

            removeBtn.Click += RemoveBtn_Click;

            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tableLayoutPanel1.RowCount++;

            int rowNum = tableLayoutPanel1.RowCount - 2;
            tableLayoutPanel1.SetRow(addCustomControlBtn, rowNum + 1);

            tableLayoutPanel1.Controls.Add(label, 0, rowNum);
            tableLayoutPanel1.Controls.Add(keyChooser1, 1, rowNum);
            tableLayoutPanel1.Controls.Add(arrowLabel, 2, rowNum);
            tableLayoutPanel1.Controls.Add(keyChooser2, 3, rowNum);
            tableLayoutPanel1.Controls.Add(removeBtn, 4, rowNum);
            tableLayoutPanel1.ResumeLayout();

            return rowNum;
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            var position = tableLayoutPanel1.GetPositionFromControl((Control)sender);

            removeRow(position.Row);
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

        private void addCustomControlBtn_Click(object sender, EventArgs e)
        {
            addLine();
        }
    }

    public struct KeyMapping
    {
        public string Title;
        public Keys UserKey, SendKey;
        public bool ReadOnly;

        public KeyMapping(string title, Keys key1, Keys key2, bool readOnly)
        {
            Title = title;
            UserKey = key1;
            SendKey = key2;
            ReadOnly = readOnly;
        }
    }
}
