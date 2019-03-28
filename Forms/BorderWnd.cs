using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace TTMulti.Forms
{
    public partial class BorderWnd : Form, IWin32Window
    {
        Color _borderColor = Color.Black;
        public Color BorderColor
        {
            get
            {
                return _borderColor;
            }
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }
        public int BorderWidth { get; set; }

        public BorderWnd()
        {
            InitializeComponent();

            this.BorderWidth = 5;
            this.Cursor = new Cursor(new MemoryStream(Properties.Resources.toonmono));
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= Win32.WS_EX_LAYERED | Win32.WS_EX_TRANSPARENT | Win32.WS_EX_TOOLWINDOW;

                return createParams;
            }
        }

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid);
        }
    }
}
