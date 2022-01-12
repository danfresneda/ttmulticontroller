using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TTMulti.Controls;

namespace TTMulti.Forms
{
    /// <summary>
    /// This dialog allows the user to define groups of Toontown windows.
    /// The number of groups possible is limited to 10 since there are only 10 number keys.
    /// </summary>
    public partial class WindowGroupsForm : Form
    {
        public WindowGroupsForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;

            if (Properties.Settings.Default.lastGroupsFormSize != Size.Empty)
            {
                Size = Properties.Settings.Default.lastGroupsFormSize;
            }

            groupsFlowPanel.Controls.Clear();

            for (int i = 0; i < Multicontroller.Instance.ControllerGroups.Count; i++)
            {
                var controllerGroup = Multicontroller.Instance.ControllerGroups[i];

                addGroup(controllerGroup);
            }
        }

        private void addGroup(ControllerGroup group)
        {
            ControllerGroupView groupView = new ControllerGroupView(group);

            groupsFlowPanel.Controls.Add(groupView);

            addGroupBtn.Enabled = groupsFlowPanel.Controls.Count < 10;
            removeGroupBtn.Enabled = true;
        }

        private void WindowGroupsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.lastGroupsFormSize = Size;
            Properties.Settings.Default.Save();
        }

        private void addGroupBtn_Click(object sender, EventArgs e)
        {
            if (groupsFlowPanel.Controls.Count < 10)
            {
                addGroup(Multicontroller.Instance.AddControllerGroup());
                Properties.Settings.Default.numberOfGroups++;
            }
        }

        private void removeGroupBtn_Click(object sender, EventArgs e)
        {
            if (groupsFlowPanel.Controls.Count > 1)
            {
                groupsFlowPanel.Controls.RemoveAt(groupsFlowPanel.Controls.Count - 1);

                addGroupBtn.Enabled = groupsFlowPanel.Controls.Count < 10;
                removeGroupBtn.Enabled = groupsFlowPanel.Controls.Count > 1;

                Multicontroller.Instance.RemoveControllerGroup(Multicontroller.Instance.ControllerGroups.Count - 1);
                Properties.Settings.Default.numberOfGroups--;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WindowGroupsForm_Activated(object sender, EventArgs e)
        {
            Multicontroller.Instance.IsActive = true;
        }

        private void WindowGroupsForm_Deactivate(object sender, EventArgs e)
        {
            Multicontroller.Instance.IsActive = false;
        }
    }
}
