using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TTMulti.Events
{
    class WindowClientAreaLocationChangedEventArgs : EventArgs
    {
        public IntPtr WindowHandle { get; }

        public Point PreviousClientAreaLocation { get; }

        public Point ClientAreaLocation { get; }

        public WindowClientAreaLocationChangedEventArgs(IntPtr windowHandle, Point previousClientAreaLocation, Point clientAreaLocation)
        {
            WindowHandle = windowHandle;
            PreviousClientAreaLocation = previousClientAreaLocation;
            ClientAreaLocation = clientAreaLocation;
        }
    }
}
