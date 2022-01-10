using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TTMulti.Events
{
    class WindowClientAreaSizeChangedEventArgs : EventArgs
    {
        public IntPtr WindowHandle { get; }

        public Size PreviousClientAreaSize { get; }

        public Size ClientAreaSize { get; }

        public WindowClientAreaSizeChangedEventArgs(IntPtr windowHandle, Size previousClientAreaSize, Size clientAreaSize)
        {
            WindowHandle = windowHandle;
            PreviousClientAreaSize = previousClientAreaSize;
            ClientAreaSize = clientAreaSize;
        }
    }
}
