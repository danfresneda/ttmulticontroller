using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTMulti
{
    /// <summary>
    /// A pair of controllers. One or more of these can exist in a group.
    /// </summary>
    class ControllerPair
    {
        public int PairNumber { get; }

        internal ToontownController LeftController { get; }
        internal ToontownController RightController { get; }

        public ControllerPair(int groupNumber, int pairNumber)
        {
            PairNumber = pairNumber;

            LeftController = new ToontownController(groupNumber, pairNumber);
            RightController = new ToontownController(groupNumber, pairNumber);
        }
        
        /// <summary>
        /// Shut down both controllers in this pair. Must be called when the controllers are no longer needed.
        /// </summary>
        public void Shutdown()
        {
            LeftController.Shutdown();
            RightController.Shutdown();
        }
    }
}
