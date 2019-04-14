using TTMulti.Controls;

namespace TTMulti.Forms
{
    partial class OptionsDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDlg));
            this.okBtn = new System.Windows.Forms.Button();
            this.cancelBtn = new System.Windows.Forms.Button();
            this.aboutBtn = new System.Windows.Forms.Button();
            this.checkUpdateBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.controlsPicker = new TTMulti.Controls.ControlsPicker();
            this.addBindingBtn = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.keyPicker1 = new TTMulti.Controls.KeyPicker();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.keyPicker2 = new TTMulti.Controls.KeyPicker();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.controlAllGroupsChk = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(443, 395);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(80, 22);
            this.okBtn.TabIndex = 0;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(532, 395);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(80, 22);
            this.cancelBtn.TabIndex = 1;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aboutBtn.Location = new System.Drawing.Point(12, 395);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(80, 22);
            this.aboutBtn.TabIndex = 12;
            this.aboutBtn.Text = "About...";
            this.aboutBtn.UseVisualStyleBackColor = true;
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // checkUpdateBtn
            // 
            this.checkUpdateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkUpdateBtn.Location = new System.Drawing.Point(98, 395);
            this.checkUpdateBtn.Name = "checkUpdateBtn";
            this.checkUpdateBtn.Size = new System.Drawing.Size(111, 22);
            this.checkUpdateBtn.TabIndex = 13;
            this.checkUpdateBtn.Text = "Check for Updates";
            this.checkUpdateBtn.UseVisualStyleBackColor = true;
            this.checkUpdateBtn.Click += new System.EventHandler(this.checkUpdateBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(600, 378);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.tableLayoutPanel3);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage6.Size = new System.Drawing.Size(592, 352);
            this.tabPage6.TabIndex = 6;
            this.tabPage6.Text = "Multi-Mode Key Bindings";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(588, 348);
            this.tableLayoutPanel3.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(3, 289);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(582, 59);
            this.label6.TabIndex = 24;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(582, 30);
            this.label3.TabIndex = 21;
            this.label3.Text = "These are the keys that the multicontroller will send to Toontown while in multi-" +
    "mode. Make sure the Toontown keys match your key bindings in your game options.";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.controlsPicker, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.addBindingBtn, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 253);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // controlsPicker
            // 
            this.controlsPicker.AutoScroll = true;
            this.controlsPicker.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controlsPicker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlsPicker.Location = new System.Drawing.Point(2, 2);
            this.controlsPicker.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.controlsPicker.Name = "controlsPicker";
            this.controlsPicker.Size = new System.Drawing.Size(578, 220);
            this.controlsPicker.TabIndex = 20;
            // 
            // addBindingBtn
            // 
            this.addBindingBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addBindingBtn.Location = new System.Drawing.Point(3, 227);
            this.addBindingBtn.Name = "addBindingBtn";
            this.addBindingBtn.Size = new System.Drawing.Size(576, 23);
            this.addBindingBtn.TabIndex = 22;
            this.addBindingBtn.Text = "+ Add Custom Key Binding";
            this.toolTip1.SetToolTip(this.addBindingBtn, "Add a custom binding for any extra controls you want in multi-mode");
            this.addBindingBtn.UseVisualStyleBackColor = true;
            this.addBindingBtn.Click += new System.EventHandler(this.addBindingBtn_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage3.Size = new System.Drawing.Size(592, 352);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Hotkeys";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.keyPicker1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(586, 75);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode/Activate Hotkey:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(574, 28);
            this.label4.TabIndex = 13;
            this.label4.Text = "This key is used to change from multi-mode to mirror-mode and back. It also activ" +
    "ates the multicontroller when you have a Toontown window active. ";
            // 
            // keyPicker1
            // 
            this.keyPicker1.ChosenKey = System.Windows.Forms.Keys.Oemtilde;
            this.keyPicker1.ChosenKeyCode = global::TTMulti.Properties.Settings.Default.modeKeyCode;
            this.keyPicker1.DataBindings.Add(new System.Windows.Forms.Binding("ChosenKeyCode", global::TTMulti.Properties.Settings.Default, "modeKeyCode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.keyPicker1.Location = new System.Drawing.Point(7, 48);
            this.keyPicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.keyPicker1.MinimumSize = new System.Drawing.Size(40, 20);
            this.keyPicker1.Name = "keyPicker1";
            this.keyPicker1.Size = new System.Drawing.Size(150, 20);
            this.keyPicker1.TabIndex = 12;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.checkBox4);
            this.tabPage5.Controls.Add(this.keyPicker2);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage5.Size = new System.Drawing.Size(592, 352);
            this.tabPage5.TabIndex = 5;
            this.tabPage5.Text = "Keep-Alive";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 30);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Keep-Alive Key:";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = global::TTMulti.Properties.Settings.Default.disableKeepAlive;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TTMulti.Properties.Settings.Default, "disableKeepAlive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4.Location = new System.Drawing.Point(6, 6);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(115, 17);
            this.checkBox4.TabIndex = 3;
            this.checkBox4.Text = "Disable Keep-Alive";
            this.toolTip1.SetToolTip(this.checkBox4, "If checked, your toons will no longer be kept awake automatically.");
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // keyPicker2
            // 
            this.keyPicker2.ChosenKey = System.Windows.Forms.Keys.Home;
            this.keyPicker2.ChosenKeyCode = global::TTMulti.Properties.Settings.Default.keepAliveKeyCode;
            this.keyPicker2.DataBindings.Add(new System.Windows.Forms.Binding("ChosenKeyCode", global::TTMulti.Properties.Settings.Default, "keepAliveKeyCode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.keyPicker2.Location = new System.Drawing.Point(99, 29);
            this.keyPicker2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.keyPicker2.MinimumSize = new System.Drawing.Size(40, 20);
            this.keyPicker2.Name = "keyPicker2";
            this.keyPicker2.Size = new System.Drawing.Size(150, 20);
            this.keyPicker2.TabIndex = 13;
            this.toolTip1.SetToolTip(this.keyPicker2, "This is the key that will be pressed periodically to keep your toons awake.");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.controlAllGroupsChk);
            this.tabPage2.Controls.Add(this.checkBox3);
            this.tabPage2.Controls.Add(this.checkBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPage2.Size = new System.Drawing.Size(592, 352);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Other";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // controlAllGroupsChk
            // 
            this.controlAllGroupsChk.AutoSize = true;
            this.controlAllGroupsChk.Checked = global::TTMulti.Properties.Settings.Default.controlAllGroupsAtOnce;
            this.controlAllGroupsChk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TTMulti.Properties.Settings.Default, "controlAllGroupsAtOnce", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.controlAllGroupsChk.Location = new System.Drawing.Point(6, 54);
            this.controlAllGroupsChk.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.controlAllGroupsChk.Name = "controlAllGroupsChk";
            this.controlAllGroupsChk.Size = new System.Drawing.Size(210, 17);
            this.controlAllGroupsChk.TabIndex = 2;
            this.controlAllGroupsChk.Text = "Control all groups at once in multi-mode";
            this.toolTip1.SetToolTip(this.controlAllGroupsChk, "If checked, every left toon will move at once, and every right toon will move at " +
        "once while in multi-mode. Otherwise, you can only control one group at a time.");
            this.controlAllGroupsChk.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = global::TTMulti.Properties.Settings.Default.compactUI;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TTMulti.Properties.Settings.Default, "compactUI", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3.Location = new System.Drawing.Point(6, 30);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(112, 17);
            this.checkBox3.TabIndex = 1;
            this.checkBox3.Text = "Compact interface";
            this.toolTip1.SetToolTip(this.checkBox3, "If checked, the size of the multicontroller window will be smaller.");
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = global::TTMulti.Properties.Settings.Default.onTopWhenInactive;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::TTMulti.Properties.Settings.Default, "onTopWhenInactive", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(6, 6);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(153, 17);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.Text = "Keep on top when inactive";
            this.toolTip1.SetToolTip(this.checkBox2, "If checked, the multicontroller window will always stay visible over everything e" +
        "lse on your screen.");
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // OptionsDlg
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cancelBtn;
            this.ClientSize = new System.Drawing.Size(625, 430);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.checkUpdateBtn);
            this.Controls.Add(this.aboutBtn);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.okBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsDlg_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button aboutBtn;
        private System.Windows.Forms.Button checkUpdateBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private TTMulti.Controls.KeyPicker keyPicker1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label label2;
        private Controls.KeyPicker keyPicker2;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Label label3;
        private Controls.ControlsPicker controlsPicker;
        private System.Windows.Forms.Button addBindingBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox controlAllGroupsChk;
    }
}