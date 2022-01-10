using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace TTMulti
{
    /// <summary>
    /// Global watcher to notify when windows are activated, deactivated, or closed.
    /// Windows must be added to the watch list to be notified.
    /// TODO: set the synchronizing object to a form
    /// </summary>
    class WindowWatcher
    {
        public static WindowWatcher Instance { get; } = new WindowWatcher();

        /// <summary>
        /// A watched window was activated or deactivated
        /// </summary>
        public event EventHandler<Events.WindowActivatedEventArgs> ActiveWindowChanged;

        /// <summary>
        /// A watched window was closed
        /// </summary>
        public event EventHandler<Events.WindowClosedEventArgs> WindowClosed;

        /// <summary>
        /// The client area size of a watched window was changed
        /// </summary>
        public event EventHandler<Events.WindowClientAreaSizeChangedEventArgs> WindowClientAreaSizeChanged;

        /// <summary>
        /// The client area location of a watched window was moved
        /// </summary>
        public event EventHandler<Events.WindowClientAreaLocationChangedEventArgs> WindowClientAreaLocationChanged;

        /// <summary>
        /// The show state (minimized, normal, maximized) of a watched window was changed
        /// </summary>
        public event EventHandler<Events.WindowShowStateChangedEventArgs> WindowShowStateChanged;

        /// <summary>
        /// Synchronizing object for event callbacks
        /// </summary>
        public ISynchronizeInvoke SynchronizingObject
        {
            get => watchTimer.SynchronizingObject;
            set => watchTimer.SynchronizingObject = value;
        }

        private struct WindowInfo
        {
            public Size ClientAreaSize { get; set; }
            public Point ClientAreaScreenLocation { get; set; }
            public Win32.ShowWindowCommands ShowState { get; set; }

            public WindowInfo(Size clientAreaSize, Point clientAreaScreenLocation, Win32.ShowWindowCommands showState)
            {
                ClientAreaSize = clientAreaSize;
                ClientAreaScreenLocation = clientAreaScreenLocation;
                ShowState = showState;
            }
        }

        private List<IntPtr> watchedWindowHandles = new List<IntPtr>();

        private Dictionary<IntPtr, WindowInfo> lastWindowInfos = new Dictionary<IntPtr, WindowInfo>();

        private IntPtr lastActiveWindowHandle = IntPtr.Zero;

        private Timer watchTimer = new Timer(15);

        private WindowWatcher()
        {
            watchTimer.Elapsed += Timer_Elapsed;
            watchTimer.AutoReset = false;
            watchTimer.Start();
        }

        /// <summary>
        /// Add a window handle to be notified when the window is closed or it becomes active
        /// </summary>
        public void WatchWindow(IntPtr windowHandle)
        {
            if (!watchedWindowHandles.Contains(windowHandle))
            {
                watchedWindowHandles.Add(windowHandle);
            }
        }

        /// <summary>
        /// Stop notifications for a window
        /// </summary>
        public void StopWatchingWindow(IntPtr windowHandle)
        {
            watchedWindowHandles.Remove(windowHandle);
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Check if active window has changed
            IntPtr activeWindowHandle = Win32.GetForegroundWindow();

            if (activeWindowHandle != lastActiveWindowHandle)
            {
                // Only notify on watched windows
                if (watchedWindowHandles.Contains(lastActiveWindowHandle) || watchedWindowHandles.Contains(activeWindowHandle))
                {
                    ActiveWindowChanged?.Invoke(this, new Events.WindowActivatedEventArgs(lastActiveWindowHandle, activeWindowHandle));
                }
                
                lastActiveWindowHandle = activeWindowHandle;
            }

            foreach (IntPtr windowHandle in watchedWindowHandles.ToArray())
            {
                // Check if window has been closed
                if (!Win32.IsWindow(windowHandle))
                {
                    WindowClosed?.Invoke(this, new Events.WindowClosedEventArgs(windowHandle));

                    watchedWindowHandles.Remove(windowHandle);
                    lastWindowInfos.Remove(windowHandle);

                    continue;
                }

                Size clientAreaSize = GetWindowClientAreaSize(windowHandle);
                Point clientAreaLocation = GetWindowClientAreaLocation(windowHandle);
                Win32.ShowWindowCommands showState = GetWindowShowState(windowHandle);
                
                if (lastWindowInfos.ContainsKey(windowHandle))
                {
                    WindowInfo lastInfo = lastWindowInfos[windowHandle];

                    if (lastInfo.ShowState != showState)
                    {
                        WindowShowStateChanged?.Invoke(this, new Events.WindowShowStateChangedEventArgs(windowHandle, lastInfo.ShowState, showState));
                        
                        lastInfo.ShowState = showState;
                    }

                    if (lastInfo.ClientAreaSize != clientAreaSize)
                    {
                        WindowClientAreaSizeChanged?.Invoke(this, new Events.WindowClientAreaSizeChangedEventArgs(windowHandle, lastInfo.ClientAreaSize, clientAreaSize));

                        lastInfo.ClientAreaSize = clientAreaSize;
                    }

                    if (lastInfo.ClientAreaScreenLocation != clientAreaLocation)
                    {
                        WindowClientAreaLocationChanged?.Invoke(this, new Events.WindowClientAreaLocationChangedEventArgs(windowHandle, lastInfo.ClientAreaScreenLocation, clientAreaLocation));

                        lastInfo.ClientAreaScreenLocation = clientAreaLocation;
                    }
                }
                else
                {
                    lastWindowInfos.Add(windowHandle, new WindowInfo(clientAreaSize, clientAreaLocation, showState));

                    WindowShowStateChanged?.Invoke(this, new Events.WindowShowStateChangedEventArgs(windowHandle, Win32.ShowWindowCommands.Hide, showState));

                    WindowClientAreaSizeChanged?.Invoke(this, new Events.WindowClientAreaSizeChangedEventArgs(windowHandle, Size.Empty, clientAreaSize));

                    WindowClientAreaLocationChanged?.Invoke(this, new Events.WindowClientAreaLocationChangedEventArgs(windowHandle, Point.Empty, clientAreaLocation));
                }
            }

            watchTimer.Start();
        }

        private Size GetWindowClientAreaSize(IntPtr windowHandle)
        {
            Win32.GetClientRect(windowHandle, out Win32.RECT lpRect);

            return new Size(lpRect.Right - lpRect.Left, lpRect.Bottom - lpRect.Top);
        }

        private Point GetWindowClientAreaLocation(IntPtr windowHandle)
        {
            Point screenLocation = new Point(0, 0);
            Win32.ClientToScreen(windowHandle, ref screenLocation);

            return screenLocation;
        }

        private Win32.ShowWindowCommands GetWindowShowState(IntPtr windowHandle)
        {
            Win32.WINDOWPLACEMENT wndPlacement = new Win32.WINDOWPLACEMENT();
            wndPlacement.Length = Marshal.SizeOf(wndPlacement);

            Win32.GetWindowPlacement(windowHandle, ref wndPlacement);

            return wndPlacement.ShowCmd;
        }
    }
}
