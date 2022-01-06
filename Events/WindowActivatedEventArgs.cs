using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTMulti.Events
{
    class WindowActivatedEventArgs : EventArgs
    {
        public IntPtr PreviousActiveWindowHandle { get; }

        public IntPtr ActiveWindowHandle { get; }

        public WindowActivatedEventArgs(IntPtr previouslyActiveWindowHandle, IntPtr activeWindowHandle)
        {
            PreviousActiveWindowHandle = previouslyActiveWindowHandle;
            ActiveWindowHandle = activeWindowHandle;
        }
    }
}
