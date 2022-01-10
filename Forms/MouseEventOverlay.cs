using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTMulti.Forms
{
    internal delegate void OverlayMouseEventHandler(object sender, Message m);

    /// <summary>
    /// This overlay window is displayed over the "border window" in order to capture mouse events
    /// while on top of a Toontown window. Without switching to WPF, the border window cannot
    /// be set to capture clicks in its entire area while still using a transparency key.
    /// </summary>
    public partial class MouseEventOverlay : Form
    {
        internal event OverlayMouseEventHandler MouseEvent;

        private short lastX = 0, lastY = 0;

        public MouseEventOverlay()
        {
            InitializeComponent();
            Cursor = new Cursor(new MemoryStream(Properties.Resources.toonmonocur));
        }

        protected override bool ShowWithoutActivation => true;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                
                // Make the window not activate when clicked
                createParams.ExStyle |= Win32.WS_EX_NOACTIVATE;

                // Make the window not count double clicks
                createParams.ClassStyle &= ~Win32.CS_DBLCLKS;

                return createParams;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch ((Win32.WM)m.Msg)
            {
                case Win32.WM.MOUSEACTIVATE:
                    // Make the window not activate when clicked (return MA_NOACTIVATE)
                    m.Result = (IntPtr)3;

                    // Returning immediately prevents activation when another window on the same thread is active
                    return;
                case Win32.WM.LBUTTONDOWN:
                case Win32.WM.LBUTTONUP:
                case Win32.WM.RBUTTONDOWN:
                case Win32.WM.RBUTTONUP:
                case Win32.WM.MBUTTONDOWN:
                case Win32.WM.MBUTTONUP:
                case Win32.WM.MOUSEWHEEL:
                case Win32.WM.MOUSELEAVE:
                    MouseEvent?.Invoke(this, m);
                    break;
                case Win32.WM.MOUSEMOVE:
                    // Optimization: check that the mouse has actually moved before forwarding event
                    short x = (short)m.LParam,
                        y = (short)(m.LParam.ToInt32() >> 16);

                    if (x != lastX || y != lastY)
                    {
                        lastX = x;
                        lastY = y;

                        MouseEvent?.Invoke(this, m);
                    }

                    break;
            }

            base.WndProc(ref m);
        }
    }
}
