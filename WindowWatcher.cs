using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<IntPtr> watchedWindowHandles = new List<IntPtr>();

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

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
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
                if (!Win32.IsWindow(windowHandle))
                {
                    WindowClosed?.Invoke(this, new Events.WindowClosedEventArgs(windowHandle));

                    watchedWindowHandles.Remove(windowHandle);
                }
            }

            watchTimer.Start();
        }
    }
}
