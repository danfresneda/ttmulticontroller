using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTMulti.Controls
{
    public partial class ControllerPairView : UserControl
    {
        internal ControllerPair ControllerPair { get; }

        public event EventHandler WindowSelected;

        public ControllerPairView()
        {
            InitializeComponent();
        }

        internal ControllerPairView(ControllerPair controllerPair) : this()
        {
            ControllerPair = controllerPair;

            leftToonCrosshair.SelectedWindowHandle = ControllerPair.LeftController.WindowHandle;
            rightToonCrosshair.SelectedWindowHandle = ControllerPair.RightController.WindowHandle;
        }

        private void leftToonCrosshair_WindowSelected(object sender, IntPtr handle)
        {
            ControllerPair.LeftController.WindowHandle = leftToonCrosshair.SelectedWindowHandle;
            WindowSelected?.Invoke(this, EventArgs.Empty);
        }

        private void rightToonCrosshair_WindowSelected(object sender, IntPtr handle)
        {
            ControllerPair.RightController.WindowHandle = rightToonCrosshair.SelectedWindowHandle;
            WindowSelected?.Invoke(this, EventArgs.Empty);
        }
    }
}
