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
    enum ControllerType
    {
        Left,
        Right
    }

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

        /// <summary>
        /// The handle of the window being controlled.
        /// TODO: make sure the handle does not belong to a utility window of a controller.
        /// </summary>
        public IntPtr WindowHandle
        {
            get => _windowHandle;
            set
            {
                if (_windowHandle != value)
                {
                    if (_windowHandle != IntPtr.Zero)
                    {
                        WindowWatcher.Instance.StopWatchingWindow(_windowHandle);
                    }
                    
                    _windowHandle = value;

                    if (_windowHandle == IntPtr.Zero)
                    {
                        WindowClosed?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        WindowWatcher.Instance.WatchWindow(_windowHandle);
                    }

                    RefreshOptions();
                    RefreshUtilityWindows();
                }
            }
        }

        public bool HasWindow { get => WindowHandle != IntPtr.Zero; }

        public Size WindowSize { get; private set; }

        public bool ShowFakeCursor
        {
            get => _borderWnd.ShowFakeCursor;
            set
            {
                if (_borderWnd.ShowFakeCursor != value)
                {
                    _borderWnd.ShowFakeCursor = value;
                }
            }
        }

        public Point FakeCursorPosition
        {
            get => _borderWnd.FakeCursorPosition;
            set => _borderWnd.FakeCursorPosition = value;
        }

        /// <summary>
        /// Whether the controlled window's size is mismatched 
        /// </summary>
        public bool IsWindowSizeMismatched
        {
            get => _borderWnd.FakeCursorIsInvalid;
            set => _borderWnd.FakeCursorIsInvalid = value;
        }

        public Color BorderColor
        {
            get => _borderWnd.BorderColor;
            private set => _borderWnd.BorderColor = value;
        }

        public bool ShowBorder { get; private set; }

        public int GroupNumber
        {
            get => _borderWnd.GroupNumber;
            set => _borderWnd.GroupNumber = value;
        }

        public int PairNumber { get; set; }

        public bool ShowGroupNumber
        {
            get => _borderWnd.ShowGroupNumber;
            private set => _borderWnd.ShowGroupNumber = value;
        }

        public bool CaptureMouseEvents { get; private set; }

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

        public ControllerType Type { get; }

        public bool ErrorOccurredPostingMessage { get; private set; } = false;

        BorderWnd _borderWnd = new BorderWnd();
        MouseEventOverlay _overlayWnd = new MouseEventOverlay();

        // Timer to send keep-alive key presses
        System.Timers.Timer keepAliveTimer = new System.Timers.Timer()
        {
            AutoReset = false,
            Interval = 60000
        };

        Multicontroller multicontroller = Multicontroller.Instance;

        public ToontownController(int groupNumber, int pairNumber, ControllerType type)
        {
            GroupNumber = groupNumber;
            PairNumber = pairNumber;
            Type = type;

            WindowWatcher.Instance.ActiveWindowChanged += WindowWatcher_ActiveWindowChanged;
            WindowWatcher.Instance.WindowClosed += WindowWatcher_WindowClosed;
            WindowWatcher.Instance.WindowClientAreaLocationChanged += WindowWatcher_WindowClientAreaLocationChanged;
            WindowWatcher.Instance.WindowClientAreaSizeChanged += WindowWatcher_WindowClientAreaSizeChanged;
            WindowWatcher.Instance.WindowShowStateChanged += WindowWatcher_WindowShowStateChanged;

            multicontroller.ModeChanged += Multicontroller_ModeChanged;
            multicontroller.ActiveControllersChanged += Multicontroller_ActiveControllersChanged;
            multicontroller.ActiveChanged += Multicontroller_ActiveChanged;
            multicontroller.SettingChanged += Multicontroller_SettingChanged;

            Properties.Settings.Default.PropertyChanged += Settings_PropertyChanged;

            _overlayWnd.MouseEvent += _overlayWnd_MouseEvent;

            keepAliveTimer.Elapsed += KeepAliveTimer_Elapsed;
        }

        private void Multicontroller_SettingChanged(object sender, EventArgs e)
        {
            RefreshOptions();
        }

        private void Multicontroller_ActiveChanged(object sender, EventArgs e)
        {
            RefreshOptions();
        }

        private void Multicontroller_ActiveControllersChanged(object sender, EventArgs e)
        {
            RefreshOptions();
        }

        private void Multicontroller_ModeChanged(object sender, EventArgs e)
        {
            RefreshOptions();
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RefreshOptions();
        }

        private void KeepAliveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!Properties.Settings.Default.disableKeepAlive
                && Properties.Settings.Default.keepAliveKeyCode != (int)Keys.None
                && HasWindow)
            {
                PostMessage(Win32.WM.KEYDOWN, (IntPtr)Properties.Settings.Default.keepAliveKeyCode, IntPtr.Zero);
                Thread.Sleep(50);
                PostMessage(Win32.WM.KEYUP, (IntPtr)Properties.Settings.Default.keepAliveKeyCode, IntPtr.Zero);

                keepAliveTimer.Start();
            }
        }

        private void WindowWatcher_WindowShowStateChanged(object sender, Events.WindowShowStateChangedEventArgs e)
        {
            if (HasWindow && WindowHandle == e.WindowHandle)
            {
                switch (e.ShowState)
                {
                    case Win32.ShowWindowCommands.ShowMinimized:
                        _borderWnd.Hide();
                        _overlayWnd.Hide();
                        break;
                    default:
                        // TODO: why is this needed?
                        _borderWnd.WindowState = FormWindowState.Normal;
                        _overlayWnd.WindowState = FormWindowState.Normal;
                        break;
                }

                RefreshUtilityWindows();
            }
        }

        private void WindowWatcher_WindowClientAreaSizeChanged(object sender, Events.WindowClientAreaSizeChangedEventArgs e)
        {
            if (HasWindow && WindowHandle == e.WindowHandle)
            {
                _borderWnd.Size = _overlayWnd.Size = WindowSize = e.ClientAreaSize;
            }
        }

        private void WindowWatcher_WindowClientAreaLocationChanged(object sender, Events.WindowClientAreaLocationChangedEventArgs e)
        {
            if (HasWindow && WindowHandle == e.WindowHandle)
            {
                _borderWnd.Location = _overlayWnd.Location = e.ClientAreaLocation;
            }
        }

        private void WindowWatcher_WindowClosed(object sender, Events.WindowClosedEventArgs e)
        {
            if (e.ClosedWindowHandle == WindowHandle)
            {
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

        private void RefreshUtilityWindows()
        {
            if (ShowBorder && HasWindow)
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

                // TODO: why is this needed?
                _borderWnd.WindowState = FormWindowState.Normal;
                _overlayWnd.WindowState = FormWindowState.Normal;

                // Running twice seems to get the mouse overlay to properly show up after activating using the hotkey
                ReorderUtilityWindows();
                ReorderUtilityWindows();
            }
            else if ((!ShowBorder || !HasWindow) && (_borderWnd.Visible || _overlayWnd.Visible))
            {
                _borderWnd.Hide();
                _overlayWnd.Hide();
            }
        }

        private void ReorderUtilityWindows()
        {
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
        }

        private void RefreshOptions()
        {
            if (multicontroller.ShowAllBorders && multicontroller.IsActive)
            {
                ShowBorder = true;
                ShowGroupNumber = true;
                CaptureMouseEvents = false;

                BorderColor = Type == ControllerType.Left ? Color.LimeGreen : Color.Green;
            }
            else if (multicontroller.IsActive)
            {
                switch (multicontroller.CurrentMode)
                {
                    case MulticontrollerMode.Group:
                    case MulticontrollerMode.Pair:
                    case MulticontrollerMode.AllGroup:
                        BorderColor = Type == ControllerType.Left ? Color.LimeGreen : Color.Green;
                        break;
                    case MulticontrollerMode.MirrorAll:
                    case MulticontrollerMode.MirrorGroup:
                        BorderColor = Color.Violet;
                        break;
                    case MulticontrollerMode.MirrorIndividual:
                        BorderColor = Color.Turquoise;
                        break;
                }

                ShowBorder = multicontroller.ActiveControllers.Contains(this);

                if (multicontroller.CurrentMode == MulticontrollerMode.Pair
                    || multicontroller.CurrentMode == MulticontrollerMode.MirrorIndividual)
                {
                    ShowGroupNumber = false;
                }
                else
                {
                    ShowGroupNumber = multicontroller.ControllerGroups.Count > 1;
                }

                CaptureMouseEvents = Properties.Settings.Default.replicateMouse;
            }
            else
            {
                ShowBorder = false;
            }

            RefreshUtilityWindows();

            if ((Properties.Settings.Default.disableKeepAlive || !HasWindow) && keepAliveTimer.Enabled)
            {
                keepAliveTimer.Stop();
            }
            else if (!Properties.Settings.Default.disableKeepAlive && HasWindow && !keepAliveTimer.Enabled)
            {
                keepAliveTimer.Start();
            }
        }

        private void _overlayWnd_MouseEvent(object sender, Message m)
        {
            MouseEvent?.Invoke(this, m);
        }

        /// <summary>
        /// Post a message asynchronously to the Toontown window
        /// </summary>
        public void PostMessage(Win32.WM msg, IntPtr wParam, IntPtr lParam)
        {
            if (WindowHandle != IntPtr.Zero)
            {
                if (!Win32.PostMessage(WindowHandle, (uint)msg, wParam, lParam))
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
        public void SendMessage(Win32.WM msg, IntPtr wParam, IntPtr lParam)
        {
            if (WindowHandle != IntPtr.Zero)
            {
                Win32.SendNotifyMessage(WindowHandle, (uint)msg, wParam, lParam);
            }
        }

        public void Shutdown()
        {
            _borderWnd.Close();
            _overlayWnd.Close();

            WindowWatcher.Instance.ActiveWindowChanged -= WindowWatcher_ActiveWindowChanged;
            WindowWatcher.Instance.WindowClosed -= WindowWatcher_WindowClosed;
            WindowWatcher.Instance.WindowClientAreaLocationChanged -= WindowWatcher_WindowClientAreaLocationChanged;
            WindowWatcher.Instance.WindowClientAreaSizeChanged -= WindowWatcher_WindowClientAreaSizeChanged;
            WindowWatcher.Instance.WindowShowStateChanged -= WindowWatcher_WindowShowStateChanged;

            multicontroller.ModeChanged -= Multicontroller_ModeChanged;
            multicontroller.ActiveControllersChanged -= Multicontroller_ActiveControllersChanged;
            multicontroller.ActiveChanged -= Multicontroller_ActiveChanged;
            multicontroller.SettingChanged -= Multicontroller_SettingChanged;

            Properties.Settings.Default.PropertyChanged -= Settings_PropertyChanged;
        }
    }
}
