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
    public partial class WindowGroupsForm : Form
    {
        //List<SelectWindowCrosshair> crosshairs = new List<SelectWindowCrosshair>();

        public WindowGroupsForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;

            //crosshairs.Add(leftToonCrosshair);
            //crosshairs.Add(rightToonCrosshair);

            groupBox1.Tag = Multicontroller.Instance.ControllerGroups[0];

            for (int i = 1; i < Properties.Settings.Default.numberOfGroups; i++)
            {
                addGroup();
            }

            for (int i = 0; i < Multicontroller.Instance.ControllerGroups.Count; i++)
            {
                var controllerGroup = Multicontroller.Instance.ControllerGroups[i];

                var crosshairs = tableLayoutPanel1.Controls[i].Controls.OfType<SelectWindowCrosshair>().ToList();

                crosshairs.First(c => (string)c.Tag == "left").SelectedWindowHandle = controllerGroup.LeftController.TTWindowHandle;
                crosshairs.First(c => (string)c.Tag == "right").SelectedWindowHandle = controllerGroup.RightController.TTWindowHandle;
            }
        }

        private void SelectWindowsForm_Load(object sender, EventArgs e)
        {
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(
            //    this.Owner.Location.X - this.Width,
            //    this.Owner.Location.Y
            //    );
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
                controllerGroup = Multicontroller.Instance.CreateControllerGroup();
                Multicontroller.Instance.ControllerGroups.Add(controllerGroup);
            }

            GroupBox groupBox = new GroupBox()
            {
                Width = groupBox1.Width,
                Height = groupBox1.Height,
                Text = "Group " + (tableLayoutPanel1.Controls.Count + 1),
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

            //crosshairs.AddRange(new[] { crosshair1, crosshair2 });

            groupBox.Controls.AddRange(new Control[] { label1, label2, crosshair1, crosshair2 });

            return groupBox;
        }

        void crosshair_WindowSelected(object sender, IntPtr handle)
        {
            SelectWindowCrosshair crosshair = (SelectWindowCrosshair)sender;
            ControllerGroup controllerGroup = (ControllerGroup)crosshair.Parent.Tag;

            ToontownController controller = (crosshair.Tag == leftToonCrosshair.Tag) ? controllerGroup.LeftController : controllerGroup.RightController;

            controller.TTWindowHandle = handle;
        }

        private void addGroup()
        {
            GroupBox groupBox = createGroupBox();

            tableLayoutPanel1.Controls.Add(groupBox);

            addGroupBtn.Enabled = tableLayoutPanel1.Controls.Count < 10;
            removeGroupBtn.Enabled = true;
        }

        private void addSetBtn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Controls.Count < 10)
            {
                addGroup();

                Properties.Settings.Default.numberOfGroups++;
            }
        }

        private void removeSetBtn_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Controls.Count > 1)
            {
                tableLayoutPanel1.Controls.Remove(tableLayoutPanel1.Controls[tableLayoutPanel1.Controls.Count - 1]);

                removeGroupBtn.Enabled = tableLayoutPanel1.Controls.Count > 1;

                ControllerGroup controllerGroup = Multicontroller.Instance.ControllerGroups.Last();
                controllerGroup.LeftController.Shutdown();
                controllerGroup.RightController.Shutdown();
                Multicontroller.Instance.ControllerGroups.Remove(controllerGroup);

                Properties.Settings.Default.numberOfGroups--;

                addGroupBtn.Enabled = tableLayoutPanel1.Controls.Count < 10;
                //crosshairs.RemoveRange(crosshairs.Count - 3, 2);
            }
        }

        private void SelectWindowsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
