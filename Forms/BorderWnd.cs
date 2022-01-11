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
    /// Displays the group number and an icon to indicate that the multicontroller is active.
    /// Displays a fake cursor to signify that mouse events will be replicated as well.
    /// </summary>
    internal partial class BorderWnd : Form, IWin32Window
    {
        private Color _borderColor = Color.Black;

        /// <summary>
        /// The color of the border displayed over a Toontown window.
        /// </summary>
        internal Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                this.Invalidate();
            }
        }

        private int _borderWidth = 5;

        /// <summary>
        /// The width of the border displayed over a Toontown window.
        /// </summary>
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                this.Invalidate();
            }
        }

        private int groupNumber;

        /// <summary>
        /// The window group number displayed on the top left of the window.
        /// </summary>
        public int GroupNumber
        {
            get => groupNumber;
            set
            {
                if (groupNumber != value)
                {
                    groupNumber = value;
                    this.Invalidate();
                }
            }
        }

        private bool showGroupNumber = false;

        /// <summary>
        /// Whether to show the window group number or not.
        /// </summary>
        public bool ShowGroupNumber
        {
            get => showGroupNumber;
            set
            {
                if (showGroupNumber != value)
                {
                    showGroupNumber = value;
                    this.Invalidate();
                }
            }
        }

        private bool _showFakeCursor;

        /// <summary>
        /// Enable showing a fake cursor to signify that mouse events are being replicated.
        /// </summary>
        internal bool ShowFakeCursor
        {
            get => _showFakeCursor;
            set
            {
                if (_showFakeCursor != value)
                {
                    _showFakeCursor = value;
                    Invalidate(fakeCursorRect);
                }
            }
        }

        private Point _fakeCursorPosition;

        /// <summary>
        /// The position of the fake cursor.
        /// </summary>
        internal Point FakeCursorPosition
        {
            get => _fakeCursorPosition;
            set
            {
                if (_fakeCursorPosition != value)
                {
                    _fakeCursorPosition = value;
                    Invalidate(fakeCursorRect);
                }
            }
        }

        private bool _fakeCursorIsInvalid;

        /// <summary>
        /// Whether to display an invalid fake cursor, signifying that the size of the 
        /// window doesn't match the source of the mouse events.
        /// </summary>
        internal bool FakeCursorIsInvalid
        {
            get => _fakeCursorIsInvalid;
            set
            {
                if (_fakeCursorIsInvalid != value)
                {
                    _fakeCursorIsInvalid = value;
                    Invalidate(fakeCursorRect);
                }
            }
        }

        /// <summary>
        /// Overrides the default style so that the window is transparent to clicks and borderless.
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
        protected override bool ShowWithoutActivation => true;

        private Bitmap fakeCursorImage = Properties.Resources.dupcursor,
            fakeCursorImageInvalid = Properties.Resources.dupcursorx;

        private Font textFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Regular);

        // Keep track of the last region where the cursor was draw so it can be invalidated quicker
        Rectangle fakeCursorRect;

        public BorderWnd()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid,
                BorderColor, BorderWidth, ButtonBorderStyle.Solid);

            if (ShowGroupNumber)
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                e.Graphics.DrawString(GroupNumber.ToString(), textFont, Brushes.White, 12, 160);
            }

            if (ShowFakeCursor)
            {
                fakeCursorRect = new Rectangle(FakeCursorPosition.X, FakeCursorPosition.Y, 32, 32);

                if (FakeCursorIsInvalid)
                {
                    e.Graphics.DrawImage(fakeCursorImageInvalid, fakeCursorRect);
                }
                else
                {
                    e.Graphics.DrawImage(fakeCursorImage, fakeCursorRect);
                }

                // Increase size to account for the next movement. Otherwise, clipping occurs.
                fakeCursorRect.Inflate(10, 10);
            }
        }
    }
}
