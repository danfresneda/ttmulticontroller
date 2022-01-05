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
    public partial class ControllerGroupView : UserControl
    {
        internal ControllerGroup ControllerGroup { get; }

        public ControllerGroupView()
        {
            InitializeComponent();
        }

        internal ControllerGroupView(ControllerGroup controllerGroup) : this()
        {
            ControllerGroup = controllerGroup;

            groupBox.Text += (Multicontroller.Instance.ControllerGroups.IndexOf(controllerGroup) + 1);

            pairsLayoutPanel.Controls.Clear();

            foreach (ControllerPair pair in controllerGroup.ControllerPairs)
            {
                AddPair(pair);
            }

            AdjustNumberOfPairs();
        }

        /// <summary>
        /// Add a pair to make sure there is always an extra one or remove extra pairs at the end if they are not being used
        /// </summary>
        private void AdjustNumberOfPairs()
        {
            var lastEmptyPairViews = pairsLayoutPanel.Controls.Cast<ControllerPairView>().Reverse()
                .TakeWhile(v => !v.ControllerPair.LeftController.HasWindow && !v.ControllerPair.RightController.HasWindow).ToArray();

            if (lastEmptyPairViews.Length > 0)
            {
                // Remove any extra pairs at the end that are empty
                for (int i = 0; i < lastEmptyPairViews.Length - 1; i++)
                {
                    RemoveLastPair();
                }
            }
            else
            {
                // Make sure there is always at least one empty pair at the end
                AddPair();
            }
        }

        private void AddPair()
        {
            if (ControllerGroup.ControllerPairs.Count < 10)
            {
                AddPair(ControllerGroup.AddPair());
            }
        }
        
        private void AddPair(ControllerPair pair)
        {
            ControllerPairView windowPairView = new ControllerPairView(pair);
            pairsLayoutPanel.Controls.Add(windowPairView);

            windowPairView.WindowSelected += WindowPairView_WindowSelected;
        }

        private void WindowPairView_WindowSelected(object sender, EventArgs e)
        {
            AdjustNumberOfPairs();
        }

        private void RemoveLastPair()
        {
            if (ControllerGroup.ControllerPairs.Count > 1)
            {
                ControllerGroup.RemoveLastPair();

                pairsLayoutPanel.Controls.RemoveAt(pairsLayoutPanel.Controls.Count - 1);
            }
        }
    }
}
