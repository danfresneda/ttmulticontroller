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
    /// <summary>
    /// This window is used to display a border around Toontown windows that are controlled 
    /// by the multicontroller. The border is drawn manually.
    /// </summary>
    internal partial class BorderWnd : Form, IWin32Window
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

        // TODO: Does this need to be settable? If yes, call Invalidate when setting it.
        public int BorderWidth { get; set; } = 5;

        /// <summary>
        /// Overrides the default style so that the window is transparent and borderless.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= Win32.WS_EX_LAYERED | Win32.WS_EX_TRANSPARENT | Win32.WS_EX_TOOLWINDOW;

                return createParams;
            }
        }

        /// <summary>
        /// Allows the window to be shown without activating it. By default, the window is activated
        /// when show which would disrupt operation of the multicontroller.
        /// </summary>
        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }

        public BorderWnd()
        {
            InitializeComponent();
            
            // TODO: Remove this? I don't think it's needed.
            this.Cursor = new Cursor(new MemoryStream(Properties.Resources.toonmono));
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
