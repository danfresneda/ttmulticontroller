using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace TTMulti.Forms
{
    internal partial class AboutWnd : Form
    {
        public AboutWnd()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon;
        }
        
        private void AboutWnd_Load(object sender, EventArgs e)
        {
            label1.Text += Application.ProductVersion;
            linkLabel1.Text += Properties.Settings.Default.homepageUrl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text.Substring(e.Link.Start, e.Link.Length));
        }
    }
}
