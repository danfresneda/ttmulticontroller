using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TTMulti.Controls
{
    public delegate void WindowSelectedHandler(object sender, IntPtr handle);

    public partial class SelectWindowCrosshair : UserControl
    {
        public event WindowSelectedHandler WindowSelected;

        IntPtr selectedWindowHandle = IntPtr.Zero;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr SelectedWindowHandle
        {
            get => selectedWindowHandle;
            set
            {
                if (selectedWindowHandle != value)
                {
                    selectedWindowHandle = value;
                    UpdateBorder();
                }
            }
        }

        private Color selectedBorderColor = Color.LimeGreen;
        public Color SelectedBorderColor
        {
            get => selectedBorderColor;
            set
            {
                if (selectedBorderColor != value)
                {
                    selectedBorderColor = value;
                    UpdateBorder();
                }
            }
        }

        bool isDragging = false;

        public SelectWindowCrosshair()
        {
            InitializeComponent();
        }

        private void UpdateBorder()
        {
            try
            {
                this.InvokeIfRequired(() =>
                {
                    this.BackColor = selectedWindowHandle != IntPtr.Zero ? SelectedBorderColor : SystemColors.Control;
                });
            }
            catch { }
        }

        private void SelectWindowCrosshair_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            pictureBox1.Image = Properties.Resources.findere;
            Cursor.Current = new Cursor(new MemoryStream(Properties.Resources.searchw));
        }

        private void SelectWindowCrosshair_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                pictureBox1.Image = Properties.Resources.finderf;

                IntPtr hWnd = Win32.WindowFromPoint(Cursor.Position);

                Cursor.Current = Cursors.Default;

                if (hWnd == this.TopLevelControl.Handle
                    || (this.TopLevelControl.Parent != null && hWnd == this.TopLevelControl.Parent.Handle)
                    || hWnd != Win32.GetAncestor(hWnd, Win32.GetAncestorFlags.GetRoot))
                {
                    hWnd = IntPtr.Zero;
                }

                SelectedWindowHandle = hWnd;

                var evt = WindowSelected;

                if (evt != null)
                {
                    evt(this, hWnd);
                }
            }
        }
    }
}
