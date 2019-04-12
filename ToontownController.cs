using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing;
using TTMulti.Forms;

namespace TTMulti
{
    delegate void TTWindowActivatedHandler(object sender, IntPtr hWnd);
    delegate void TTWindowClosedHandler(object sender);

    class ToontownController : IMessageFilter
    {
        public event TTWindowActivatedHandler TTWindowActivated;
        public event TTWindowActivatedHandler TTWindowDeactivated;
        public event TTWindowClosedHandler TTWindowClosed;

        IntPtr _ttWindowHandle;
        public IntPtr TTWindowHandle
        {
            get
            {
                return _ttWindowHandle;
            }
            set
            {
                _ttWindowHandle = value;
            }
        }

        public Color BorderColor { get; set; }

        bool _showBorder = true;
        public bool ShowBorder
        {
            get
            {
                return _showBorder;
            }
            set
            {
                _showBorder = value;
            }
        }

        private bool ttWindowActive = false;
        public bool TTWindowActive
        {
            get => ttWindowActive;
            private set
            { 
                if (ttWindowActive != value)
                {
                    ttWindowActive = value;

                    if (ttWindowActive) { Console.WriteLine("{0} activate", _ttWindowHandle); TTWindowActivated?.Invoke(this, _ttWindowHandle); }
                    else { Console.WriteLine("{0} deactivate", _ttWindowHandle); TTWindowDeactivated?.Invoke(this, _ttWindowHandle); }
                }
            }
        }

        BorderWnd _borderWnd;

        Thread bgThread;
        
        public ToontownController()
        {
            bgThread = new Thread(() =>
            {
                _borderWnd = new BorderWnd();
                DateTime lastWake = DateTime.MinValue;

                Application.AddMessageFilter(this);

                while (true)
                {
                    if (_borderWnd.BorderColor != BorderColor)
                    {
                        _borderWnd.BorderColor = BorderColor;
                    }

                    try
                    {
                        if (TTWindowHandle != IntPtr.Zero && !Win32.IsWindow(TTWindowHandle))
                        {
                            TTWindowHandle = IntPtr.Zero;
                            TTWindowClosed?.Invoke(this);
                        }

                        if (TTWindowHandle == IntPtr.Zero && _borderWnd.Visible)
                        {
                            _borderWnd.Hide();
                        }
                        else if (TTWindowHandle != IntPtr.Zero)
                        {
                            TTWindowActive = (Win32.GetForegroundWindow() == TTWindowHandle);
                            
                            Win32.RECT lpRect;
                            Win32.GetClientRect(TTWindowHandle, out lpRect);

                            Win32.WINDOWPLACEMENT wndPlacement = new Win32.WINDOWPLACEMENT();
                            wndPlacement.Length = Marshal.SizeOf(wndPlacement);

                            Win32.GetWindowPlacement(TTWindowHandle, ref wndPlacement);

                            Point clientPoint = new Point(0, 0);
                            Win32.ClientToScreen(TTWindowHandle, ref clientPoint);

                            _borderWnd.Location = clientPoint;

                            switch (wndPlacement.ShowCmd)
                            {
                                case Win32.ShowWindowCommands.ShowMinimized:
                                    _borderWnd.Hide();
                                    break;
                                default:
                                    _borderWnd.Size = new Size(lpRect.Right - lpRect.Left, lpRect.Bottom - lpRect.Top);

                                    if (!_borderWnd.Visible && ShowBorder)
                                    {
                                        _borderWnd.Show();
                                    }
                                    else if (_borderWnd.Visible && !ShowBorder)
                                    {
                                        _borderWnd.Hide();
                                    }

                                    _borderWnd.WindowState = FormWindowState.Normal;

                                    bool borderWndIsAboveTT = false;
                                    IntPtr hWndAbove = TTWindowHandle;

                                    do
                                    {
                                        hWndAbove = Win32.GetWindow(hWndAbove, Win32.GetWindow_Cmd.GW_HWNDPREV);

                                        if (hWndAbove == _borderWnd.Handle)
                                        {
                                            borderWndIsAboveTT = true;
                                            break;
                                        }

                                    } while (hWndAbove != IntPtr.Zero);

                                    if (!borderWndIsAboveTT)
                                    {
                                        Win32.SetWindowPos(_borderWnd.Handle, TTWindowHandle, 0, 0, 0, 0, Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize);
                                        Win32.SetWindowPos(TTWindowHandle, _borderWnd.Handle, 0, 0, 0, 0, Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize);
                                    }

                                    break;
                            }
                        }

                        if (!Properties.Settings.Default.disableKeepAlive 
                        && (DateTime.Now - lastWake).TotalMinutes >= 1 
                        && TTWindowHandle != IntPtr.Zero
                        && Properties.Settings.Default.keepAliveKeyCode != (int)Keys.None)
                        {
                            PostMessage((uint)Win32.WM.KEYDOWN, (IntPtr)Properties.Settings.Default.keepAliveKeyCode, IntPtr.Zero);
                            Thread.Sleep(50);
                            PostMessage((uint)Win32.WM.KEYUP, (IntPtr)Properties.Settings.Default.keepAliveKeyCode, IntPtr.Zero);

                            lastWake = DateTime.Now;
                        }

                        Application.DoEvents();

                        Thread.Sleep(5);
                    }
                    catch (ThreadInterruptedException)
                    {
                        return;
                    }
                }
            }) { IsBackground = true };

            bgThread.Start();
        }

        public void PostMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (TTWindowHandle != IntPtr.Zero)
            {
                Win32.PostMessage(TTWindowHandle, msg, wParam, lParam);
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.HWnd != _borderWnd.Handle) return false;

            switch ((Win32.WM)m.Msg)
            {
                case Win32.WM.LBUTTONDOWN:
                case Win32.WM.LBUTTONUP:
                case Win32.WM.RBUTTONDOWN:
                case Win32.WM.RBUTTONUP:
                case Win32.WM.MOUSEMOVE:
                case Win32.WM.KEYDOWN:
                case Win32.WM.KEYUP:
                    PostMessage((uint)m.Msg, m.WParam, m.LParam);
                    break;
                case Win32.WM.SETCURSOR:
                    PostMessage((uint)m.Msg, TTWindowHandle, m.LParam);
                    break;
                default:
                    return false;
            }

            return false;
        }

        public void Shutdown()
        {
            _borderWnd.InvokeIfRequired(() => _borderWnd.Close());

            bgThread.Interrupt();
            while (bgThread.IsAlive) Thread.Sleep(1);

            if (TTWindowHandle != IntPtr.Zero)
            {

            }
        }
    }
}
