using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTMulti.Events
{
    class WindowClosedEventArgs : EventArgs
    {
        public IntPtr ClosedWindowHandle { get; }

        public WindowClosedEventArgs(IntPtr closedWindowHandle)
        {
            ClosedWindowHandle = closedWindowHandle;
        }
    }
}
