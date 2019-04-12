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

            groupBox1.Tag = Multicontroller.Instance.ControllerGroups[0];

            for (int i = 1; i < Properties.Settings.Default.numberOfGroups; i++)
            {
                addGroup();
            }

            for (int i = 0; i < Multicontroller.Instance.ControllerGroups.Count; i++)
            {
                var controllerGroup = Multicontroller.Instance.ControllerGroups[i];

                // The TableLayoutPanel only holds GroupBoxes, each of which has two window pickers
                var crosshairs = tableLayoutPanel1.Controls[i].Controls.OfType<SelectWindowCrosshair>().ToList();

                crosshairs.First(c => (string)c.Tag == "left").SelectedWindowHandle = controllerGroup.LeftController.TTWindowHandle;
                crosshairs.First(c => (string)c.Tag == "right").SelectedWindowHandle = controllerGroup.RightController.TTWindowHandle;
            }
        }

        private void addGroup()
        {
            GroupBox groupBox = createGroupBox();

            tableLayoutPanel1.Controls.Add(groupBox);

            addGroupBtn.Enabled = tableLayoutPanel1.Controls.Count < 10;
            removeGroupBtn.Enabled = true;
        }
        
        private GroupBox createGroupBox()
        {
            int index = tableLayoutPanel1.Controls.Count;

            ControllerGroup controllerGroup;

            if (Multicontroller.Instance.ControllerGroups.Count > index)
            {
                controllerGroup = Multicontroller.Instance.ControllerGroups[index];
            }
            else
            {
                controllerGroup = Multicontroller.Instance.AddControllerGroup();
            }

            GroupBox groupBox = new GroupBox()
            {
                Width = groupBox1.Width,
                Height = groupBox1.Height,
                Text = "Group " + (index + 1),
                Tag = controllerGroup
            };

            Label label1 = new Label()
            {
                Location = leftToonLbl.Location,
                Text = leftToonLbl.Text,
                AutoSize = leftToonLbl.AutoSize
            };

            Label label2 = new Label()
            {
                Location = rightToonLbl.Location,
                Text = rightToonLbl.Text,
                AutoSize = rightToonLbl.AutoSize
            };

            SelectWindowCrosshair crosshair1 = new SelectWindowCrosshair() {
                Location = leftToonCrosshair.Location,
                Size = leftToonCrosshair.Size,
                Tag = leftToonCrosshair.Tag
            };

            crosshair1.WindowSelected += crosshair_WindowSelected;

            SelectWindowCrosshair crosshair2 = new SelectWindowCrosshair() {
                Location = rightToonCrosshair.Location,
                Size = rightToonCrosshair.Size,
                Tag = rightToonCrosshair.Tag
            };

            crosshair2.WindowSelected += crosshair_WindowSelected;
            
            groupBox.Controls.AddRange(new Control[] { label1, label2, crosshair1, crosshair2 });

            return groupBox;
        }

        private void WindowGroupsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void crosshair_WindowSelected(object sender, IntPtr handle)
        {
            SelectWindowCrosshair crosshair = (SelectWindowCrosshair)sender;
            ControllerGroup controllerGroup = (ControllerGroup)crosshair.Parent.Tag;

            ToontownController controller = (crosshair.Tag == leftToonCrosshair.Tag) ? controllerGroup.LeftController : controllerGroup.RightController;

            controller.TTWindowHandle = handle;
        }
        
        private void addGroupBtn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Controls.Count < 10)
            {
                addGroup();
                Properties.Settings.Default.numberOfGroups++;
            }
        }

        private void removeGroupBtn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Controls.Count > 1)
            {
                tableLayoutPanel1.Controls.RemoveAt(tableLayoutPanel1.Controls.Count - 1);

                addGroupBtn.Enabled = tableLayoutPanel1.Controls.Count < 10;
                removeGroupBtn.Enabled = tableLayoutPanel1.Controls.Count > 1;

                Multicontroller.Instance.RemoveControllerGroup(Multicontroller.Instance.ControllerGroups.Count - 1);
                Properties.Settings.Default.numberOfGroups--;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
