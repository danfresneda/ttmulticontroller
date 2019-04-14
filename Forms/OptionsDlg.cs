using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment;
using System.Deployment.Application;
using TTMulti.Controls;

namespace TTMulti.Forms
{
    public partial class OptionsDlg : Form
    {
        private bool loaded = false;

        public OptionsDlg()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;

            toolTip1.SetToolTip(checkBox2, "If checked, the Multicontroller window will stay on top of everything else. Otherwise, it will go to the background when it's deactivated by clicking on another window.");
            toolTip1.SetToolTip(checkBox3, "If checked, some of the UI elements will be hidden to make the Multicontroller window smaller.");

            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                checkUpdateBtn.Visible = false;
            }
        }

        private void OptionsDlg_Load(object sender, EventArgs e)
        {
            controlsPicker.KeyMappings = Properties.SerializedSettings.Default.Bindings;

            loaded = true;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            Properties.SerializedSettings.Default.Bindings = controlsPicker.KeyMappings;
            
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            DialogResult = DialogResult.Cancel;
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
            new AboutWnd().ShowDialog(this);
        }

        // https://docs.microsoft.com/en-us/visualstudio/deployment/how-to-check-for-application-updates-programmatically-using-the-clickonce-deployment-api?view=vs-2015
        private void checkUpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("There are no updates available at the moment.", "No Update Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("This application can't be updated. Try re-installing it from the homepage.", "Cannot Be Updated", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void addBindingBtn_Click(object sender, EventArgs e)
        {
            AddKeyMappingDlg addKeyMappingDlg = new AddKeyMappingDlg();

            while (addKeyMappingDlg.ShowDialog() == DialogResult.OK)
            {
                var keyBindings = controlsPicker.KeyMappings;

                if (string.IsNullOrEmpty(addKeyMappingDlg.BindingName.Trim()))
                {
                    MessageBox.Show("Please enter a name for the binding.");
                }
                /*else if (addKeyMappingDlg.LeftToonKey != Keys.None && keyBindings.Any(t => t.LeftToonKey == addKeyMappingDlg.LeftToonKey))
                {
                    MessageBox.Show("Sorry, the key you picked for the left toon is already being used for another binding on the left toon.");
                }
                else if (addKeyMappingDlg.RightToonKey != Keys.None && keyBindings.Any(t => t.RightToonKey == addKeyMappingDlg.RightToonKey))
                {
                    MessageBox.Show("Sorry, the key you picked for the right toon is already being used for another binding on the right toon.");
                }*/
                else
                {
                    if (addKeyMappingDlg.LeftToonKey >= Keys.D0 && addKeyMappingDlg.LeftToonKey <= Keys.D9
                        || addKeyMappingDlg.LeftToonKey >= Keys.NumPad0 && addKeyMappingDlg.LeftToonKey <= Keys.NumPad9
                        || addKeyMappingDlg.RightToonKey >= Keys.D0 && addKeyMappingDlg.RightToonKey <= Keys.D9
                        || addKeyMappingDlg.RightToonKey >= Keys.NumPad0 && addKeyMappingDlg.RightToonKey <= Keys.NumPad9)
                    {
                        MessageBox.Show("Note: the number keys (0-9) and number pad keys are reserved for switching groups if there is more than 1 group.");
                    }

                    controlsPicker.AddMapping(new KeyMapping(addKeyMappingDlg.BindingName, addKeyMappingDlg.BindingKey, addKeyMappingDlg.LeftToonKey, addKeyMappingDlg.RightToonKey, false));
                    break;
                }
            }
        }
    }
}
