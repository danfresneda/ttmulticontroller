using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TTMulti
{
    /// <summary>
    /// A group of controllers that contains one or more pairs of left and right controllers.
    /// </summary>
    sealed class ControllerGroup : IDisposable
    {
        /// <summary>
        /// The Toontown window of a controller in this group was activated
        /// </summary>
        public event TTWindowActivatedHandler TTWindowActivated;

        /// <summary>
        /// The Toontown window of a controller in this group was deactivated
        /// </summary>
        public event TTWindowActivatedHandler TTWindowDeactivated;

        /// <summary>
        /// The Toontown window of a controller in this group was closed
        /// </summary>
        public event TTWindowClosedHandler TTWindowClosed;

        /// <summary>
        /// A mouse event was captured from a controller in this group
        /// </summary>
        internal event Forms.OverlayMouseEventHandler MouseEvent;

        /// <summary>
        /// A pair of controllers was added or removed
        /// </summary>
        public event EventHandler PairAddedRemoved;

        internal List<ControllerPair> ControllerPairs { get; } = new List<ControllerPair>();

        internal IEnumerable<ToontownController> AllControllers { get => ControllerPairs.SelectMany(p => new[] { p.LeftController, p.RightController }); }
        internal IEnumerable<ToontownController> LeftControllers { get => ControllerPairs.Select(p => p.LeftController); }
        internal IEnumerable<ToontownController> RightControllers { get => ControllerPairs.Select(p => p.RightController); }

        internal int GroupNumber { get; }

        public ControllerGroup(int groupNumber)
        {
            GroupNumber = groupNumber;

            AddPair();
        }

        ~ControllerGroup()
        {
            Dispose();
        }

        /// <summary>
        /// Add a new ControllerPair at the end of the list
        /// </summary>
        /// <returns></returns>
        public ControllerPair AddPair()
        {
            var pair = new ControllerPair(GroupNumber);

            pair.LeftController.TTWindowActivated += Controller_TTWindowActivated;
            pair.RightController.TTWindowActivated += Controller_TTWindowActivated;
            pair.LeftController.TTWindowDeactivated += Controller_TTWindowDeactivated;
            pair.RightController.TTWindowDeactivated += Controller_TTWindowDeactivated;
            pair.LeftController.TTWindowClosed += Controller_TTWindowClosed; ;
            pair.RightController.TTWindowClosed += Controller_TTWindowClosed;
            pair.LeftController.MouseEvent += Controller_MouseEvent;
            pair.RightController.MouseEvent += Controller_MouseEvent;

            ControllerPairs.Add(pair);

            PairAddedRemoved?.Invoke(this, EventArgs.Empty);

            return pair;
        }

        /// <summary>
        /// Remove the last ControllerPair from the list
        /// </summary>
        public void RemoveLastPair()
        {
            if (ControllerPairs.Count > 1)
            {
                ControllerPair pair = ControllerPairs.Last();

                pair.Shutdown();

                ControllerPairs.Remove(pair);

                PairAddedRemoved?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Controller_TTWindowClosed(object sender)
        {
            TTWindowClosed?.Invoke(sender);
        }

        private void Controller_TTWindowDeactivated(object sender, IntPtr hWnd)
        {
            TTWindowDeactivated?.Invoke(sender, hWnd);
        }

        private void Controller_TTWindowActivated(object sender, IntPtr hWnd)
        {
            TTWindowActivated?.Invoke(sender, hWnd);
        }

        private void Controller_MouseEvent(object sender, System.Windows.Forms.Message m)
        {
            MouseEvent?.Invoke(sender, m);
        }

        public void Dispose()
        {
            foreach (var pair in ControllerPairs)
            {
                pair.Shutdown();
            }

            GC.SuppressFinalize(this);
        }
    }
}
