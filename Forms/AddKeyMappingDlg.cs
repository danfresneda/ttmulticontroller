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
    internal partial class AddKeyMappingDlg : Form
    {
        internal string BindingName { get { return nameTxt.Text; } }
        internal Keys BindingKey { get { return bindingKeyPicker.ChosenKey; } }
        internal Keys LeftToonKey { get { return leftToonKeyPicker.ChosenKey; } }
        internal Keys RightToonKey { get { return rightToonKeyPicker.ChosenKey; } }

        public AddKeyMappingDlg()
        {
            InitializeComponent();
        }

        private void AddKeyMappingDlg_Load(object sender, EventArgs e)
        {

        }
    }
}
