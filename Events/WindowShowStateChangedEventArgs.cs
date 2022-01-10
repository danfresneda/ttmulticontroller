using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTMulti.Events
{
    class WindowShowStateChangedEventArgs : EventArgs
    {
        public IntPtr WindowHandle { get; }

        public Win32.ShowWindowCommands PreviousShowState { get; }

        public Win32.ShowWindowCommands ShowState { get; }

        public WindowShowStateChangedEventArgs(IntPtr windowHandle, Win32.ShowWindowCommands previousShowState, Win32.ShowWindowCommands showState)
        {
            WindowHandle = windowHandle;
            PreviousShowState = previousShowState;
            ShowState = showState;
        }
    }
}
