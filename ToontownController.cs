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
    class ToontownController
    {
        /// <summary>
        /// The controlled window was activated
        /// </summary>
        public event EventHandler WindowActivated;

        /// <summary>
        /// The controlled window was deactivated
        /// </summary>
        public event EventHandler WindowDeactivated;

        /// <summary>
        /// The controlled window was closed
        /// </summary>
        public event EventHandler WindowClosed;

        internal event OverlayMouseEventHandler MouseEvent;

        IntPtr _windowHandle;
        public IntPtr WindowHandle
        {
            get => _windowHandle;
            set
            {
                if (_windowHandle != value)
                {
                    _windowHandle = value;

                    WindowWatcher.Instance.WatchWindow(_windowHandle);
                }
            }
        }

        public bool HasWindow { get => WindowHandle != IntPtr.Zero; }

        public Size WindowSize { get; private set; }

        public bool ShowFakeCursor { get; set; }

        public Point FakeCursorPosition { get; set; }

        /// <summary>
        /// Whether the controlled window's size is mismatched 
        /// </summary>
        public bool IsWindowSizeMismatched { get; set; }

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

        public int GroupNumber { get; }

        public bool ShowGroupNumber { get; set; } = false;

        public bool CaptureMouseEvents { get; set; } = false;

        private bool _isWindowActive = false;
        public bool IsWindowActive
        {
            get => _isWindowActive;
            private set
            {
                if (_isWindowActive != value)
                {
                    _isWindowActive = value;

                    if (_isWindowActive)
                    {
                        WindowActivated?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        WindowDeactivated?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool ErrorOccurredPostingMessage { get; private set; } = false;

        BorderWnd _borderWnd;
        MouseEventOverlay _overlayWnd;

        Thread bgThread;
        
        public ToontownController(int groupNumber)
        {
            GroupNumber = groupNumber;

            WindowWatcher.Instance.ActiveWindowChanged += WindowWatcher_ActiveWindowChanged;
            WindowWatcher.Instance.WindowClosed += WindowWatcher_WindowClosed;

            bgThread = new Thread(() =>
            {
                _borderWnd = new BorderWnd();
                _overlayWnd = new MouseEventOverlay();

                _overlayWnd.MouseEvent += _overlayWnd_MouseEvent;

                DateTime lastWake = DateTime.MinValue;

                while (true)
                {
                    if (_borderWnd.BorderColor != BorderColor)
                    {
                        _borderWnd.BorderColor = BorderColor;
                    }

                    if (_borderWnd.GroupNumber != GroupNumber)
                    {
                        _borderWnd.GroupNumber = GroupNumber;
                    }

                    if (_borderWnd.ShowGroupNumber != ShowGroupNumber)
                    {
                        _borderWnd.ShowGroupNumber = ShowGroupNumber;
                    }

                    if (_borderWnd.FakeCursorPosition != FakeCursorPosition)
                    {
                        _borderWnd.FakeCursorPosition = FakeCursorPosition;
                    }

                    if (_borderWnd.ShowFakeCursor != ShowFakeCursor)
                    {
                        _borderWnd.ShowFakeCursor = ShowFakeCursor;
                    }

                    if (_borderWnd.FakeCursorIsInvalid != IsWindowSizeMismatched)
                    {
                        _borderWnd.FakeCursorIsInvalid = IsWindowSizeMismatched;
                    }

                    try
                    {
                        if (!HasWindow && _borderWnd.Visible)
                        {
                            _borderWnd.Hide();
                            _overlayWnd.Hide();
                        }
                        else if (HasWindow)
                        {
                            Win32.RECT lpRect;
                            Win32.GetClientRect(WindowHandle, out lpRect);

                            Win32.WINDOWPLACEMENT wndPlacement = new Win32.WINDOWPLACEMENT();
                            wndPlacement.Length = Marshal.SizeOf(wndPlacement);

                            Win32.GetWindowPlacement(WindowHandle, ref wndPlacement);

                            Point clientPoint = new Point(0, 0);
                            Win32.ClientToScreen(WindowHandle, ref clientPoint);

                            _borderWnd.Location = clientPoint;
                            _overlayWnd.Location = clientPoint;

                            switch (wndPlacement.ShowCmd)
                            {
                                case Win32.ShowWindowCommands.ShowMinimized:
                                    _borderWnd.Hide();
                                    _overlayWnd.Hide();
                                    break;
                                default:
                                    _borderWnd.Size = _overlayWnd.Size = WindowSize = 
                                        new Size(lpRect.Right - lpRect.Left, lpRect.Bottom - lpRect.Top);
                                    
                                    if (ShowBorder)
                                    {
                                        if (!_borderWnd.Visible)
                                        {
                                            _borderWnd.Show();
                                        }

                                        if (CaptureMouseEvents && !_overlayWnd.Visible)
                                        {
                                            _overlayWnd.Show();
                                        } 
                                        else if (!CaptureMouseEvents && _overlayWnd.Visible)
                                        {
                                            _overlayWnd.Hide();
                                        }
                                    }
                                    else if (!ShowBorder && (_borderWnd.Visible || _overlayWnd.Visible))
                                    {
                                        _borderWnd.Hide();
                                        _overlayWnd.Hide();
                                    }

                                    _borderWnd.WindowState = FormWindowState.Normal;
                                    _overlayWnd.WindowState = FormWindowState.Normal;

                                    /*
                                     * Order the windows in the following z-order:
                                     * 1 - overlay window
                                     * 2 - border window
                                     * 3 - Toontown window
                                     */

                                    bool borderWndIsAboveTT = false,
                                        overlayWndIsAboveTT = false;

                                    IntPtr hWndAbove = WindowHandle;

                                    do
                                    {
                                        hWndAbove = Win32.GetWindow(hWndAbove, Win32.GetWindow_Cmd.GW_HWNDPREV);
                                        
                                        if (hWndAbove == _borderWnd.Handle)
                                        {
                                            borderWndIsAboveTT = true;
                                        }
                                        else if (hWndAbove == _overlayWnd.Handle)
                                        {
                                            overlayWndIsAboveTT = true;
                                        }

                                        if (overlayWndIsAboveTT && borderWndIsAboveTT)
                                        {
                                            break;
                                        }

                                    } while (hWndAbove != IntPtr.Zero);

                                    if (!borderWndIsAboveTT || !overlayWndIsAboveTT)
                                    {
                                        // TODO: Check this logic
                                        Win32.SetWindowPos(_overlayWnd.Handle, _borderWnd.Handle, 0, 0, 0, 0, Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize);
                                        Win32.SetWindowPos(_borderWnd.Handle, WindowHandle, 0, 0, 0, 0, Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize);
                                        Win32.SetWindowPos(WindowHandle, _borderWnd.Handle, 0, 0, 0, 0, Win32.SetWindowPosFlags.DoNotActivate | Win32.SetWindowPosFlags.IgnoreMove | Win32.SetWindowPosFlags.IgnoreResize);
                                    }

                                    break;
                            }
                        }

                        if (!Properties.Settings.Default.disableKeepAlive 
                        && (DateTime.Now - lastWake).TotalMinutes >= 1 
                        && WindowHandle != IntPtr.Zero
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

        private void WindowWatcher_WindowClosed(object sender, Events.WindowClosedEventArgs e)
        {
            if (e.ClosedWindowHandle == WindowHandle)
            {
                WindowClosed?.Invoke(this, EventArgs.Empty);

                WindowHandle = IntPtr.Zero;
            }
        }

        private void WindowWatcher_ActiveWindowChanged(object sender, Events.WindowActivatedEventArgs e)
        {
            if (!HasWindow)
            {
                return;
            }

            if (e.ActiveWindowHandle == WindowHandle)
            {
                IsWindowActive = true;
            }
            else if (e.PreviousActiveWindowHandle == WindowHandle)
            {
                IsWindowActive = false;
            }
        }

        private void _overlayWnd_MouseEvent(object sender, Message m)
        {
            MouseEvent?.Invoke(this, m);
        }

        /// <summary>
        /// Post a message asynchronously to the Toontown window
        /// </summary>
        public void PostMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (WindowHandle != IntPtr.Zero)
            {
                if (!Win32.PostMessage(WindowHandle, msg, wParam, lParam))
                {
                    ErrorOccurredPostingMessage = true;
                }
            }
        }

        /// <summary>
        /// Alternative to PostMessage to try to maintain the order of mouse events.
        /// Returns immediately unlike SendMessage as long as the target window is in a different
        /// thread.
        /// </summary>
        public void SendMessage(uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (WindowHandle != IntPtr.Zero)
            {
                Win32.SendNotifyMessage(WindowHandle, msg, wParam, lParam);
            }
        }

        public void Shutdown()
        {
            bgThread.Interrupt();
            while (bgThread.IsAlive) Thread.Sleep(1);

            _borderWnd.InvokeIfRequired(() =>
            {
                _borderWnd.Close();
                _overlayWnd.Close();
                Application.DoEvents();
            });

            WindowWatcher.Instance.ActiveWindowChanged -= WindowWatcher_ActiveWindowChanged;
            WindowWatcher.Instance.WindowClosed -= WindowWatcher_WindowClosed;
        }
    }
}
