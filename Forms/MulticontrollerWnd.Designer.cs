namespace TTMulti.Forms
{
    partial class MulticontrollerWnd
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
            this.mirrorModeRadio = new System.Windows.Forms.RadioButton();
            this.wndGroup = new System.Windows.Forms.GroupBox();
            this.multiModeRadio = new System.Windows.Forms.RadioButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.optionsBtn = new System.Windows.Forms.Button();
            this.windowGroupsBtn = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.leftStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.rightStatusLbl = new System.Windows.Forms.ToolStripStatusLabel();
            this.rightToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.leftToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.wndGroup.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mirrorModeRadio
            // 
            this.mirrorModeRadio.Appearance = System.Windows.Forms.Appearance.Button;
            this.mirrorModeRadio.FlatAppearance.BorderColor = System.Drawing.Color.Violet;
            this.mirrorModeRadio.FlatAppearance.BorderSize = 3;
            this.mirrorModeRadio.FlatAppearance.CheckedBackColor = System.Drawing.Color.Violet;
            this.mirrorModeRadio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mirrorModeRadio.Location = new System.Drawing.Point(111, 18);
            this.mirrorModeRadio.Name = "mirrorModeRadio";
            this.mirrorModeRadio.Size = new System.Drawing.Size(98, 27);
            this.mirrorModeRadio.TabIndex = 3;
            this.mirrorModeRadio.Text = "Mirror Mode";
            this.mirrorModeRadio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.mirrorModeRadio.UseVisualStyleBackColor = true;
            this.mirrorModeRadio.Click += new System.EventHandler(this.mirrorModeRadio_Clicked);
            // 
            // wndGroup
            // 
            this.wndGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wndGroup.Controls.Add(this.mirrorModeRadio);
            this.wndGroup.Controls.Add(this.multiModeRadio);
            this.wndGroup.Location = new System.Drawing.Point(6, 54);
            this.wndGroup.Name = "wndGroup";
            this.wndGroup.Padding = new System.Windows.Forms.Padding(0);
            this.wndGroup.Size = new System.Drawing.Size(215, 53);
            this.wndGroup.TabIndex = 7;
            this.wndGroup.TabStop = false;
            this.wndGroup.Text = "Mode";
            // 
            // multiModeRadio
            // 
            this.multiModeRadio.Appearance = System.Windows.Forms.Appearance.Button;
            this.multiModeRadio.Checked = true;
            this.multiModeRadio.FlatAppearance.BorderColor = System.Drawing.Color.LimeGreen;
            this.multiModeRadio.FlatAppearance.BorderSize = 3;
            this.multiModeRadio.FlatAppearance.CheckedBackColor = System.Drawing.Color.LimeGreen;
            this.multiModeRadio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.multiModeRadio.Location = new System.Drawing.Point(6, 18);
            this.multiModeRadio.Name = "multiModeRadio";
            this.multiModeRadio.Size = new System.Drawing.Size(98, 27);
            this.multiModeRadio.TabIndex = 6;
            this.multiModeRadio.TabStop = true;
            this.multiModeRadio.Text = "Multi-Mode";
            this.multiModeRadio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.multiModeRadio.UseVisualStyleBackColor = true;
            this.multiModeRadio.Click += new System.EventHandler(this.multiModeRadio_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Key Sequence|*.keyseq";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Key Sequence|*.keyseq";
            // 
            // optionsBtn
            // 
            this.optionsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.optionsBtn.Location = new System.Drawing.Point(54, 30);
            this.optionsBtn.Name = "optionsBtn";
            this.optionsBtn.Size = new System.Drawing.Size(122, 20);
            this.optionsBtn.TabIndex = 8;
            this.optionsBtn.Text = "Options";
            this.optionsBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.optionsBtn.UseVisualStyleBackColor = true;
            this.optionsBtn.Click += new System.EventHandler(this.optionsBtn_Click);
            // 
            // windowGroupsBtn
            // 
            this.windowGroupsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowGroupsBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.windowGroupsBtn.Location = new System.Drawing.Point(54, 6);
            this.windowGroupsBtn.Name = "windowGroupsBtn";
            this.windowGroupsBtn.Size = new System.Drawing.Size(122, 20);
            this.windowGroupsBtn.TabIndex = 12;
            this.windowGroupsBtn.Text = "Window Groups";
            this.windowGroupsBtn.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.windowGroupsBtn.UseVisualStyleBackColor = true;
            this.windowGroupsBtn.Click += new System.EventHandler(this.windowGroupsBtn_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leftStatusLbl,
            this.toolStripStatusLabel2,
            this.rightStatusLbl});
            this.statusStrip1.Location = new System.Drawing.Point(0, 116);
            this.statusStrip1.Margin = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.statusStrip1.Size = new System.Drawing.Size(230, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // leftStatusLbl
            // 
            this.leftStatusLbl.Name = "leftStatusLbl";
            this.leftStatusLbl.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(214, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rightStatusLbl
            // 
            this.rightStatusLbl.Name = "rightStatusLbl";
            this.rightStatusLbl.Size = new System.Drawing.Size(0, 17);
            // 
            // rightToonCrosshair
            // 
            this.rightToonCrosshair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.rightToonCrosshair.Location = new System.Drawing.Point(183, 10);
            this.rightToonCrosshair.Name = "rightToonCrosshair";
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            this.rightToonCrosshair.Size = new System.Drawing.Size(38, 35);
            this.rightToonCrosshair.TabIndex = 14;
            this.rightToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.rightToonCrosshair_WindowSelected);
            // 
            // leftToonCrosshair
            // 
            this.leftToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.leftToonCrosshair.Location = new System.Drawing.Point(6, 10);
            this.leftToonCrosshair.Name = "leftToonCrosshair";
// TODO: Code generation for '' failed because of Exception 'Invalid Primitive Type: System.IntPtr. Consider using CodeObjectCreateExpression.'.
            this.leftToonCrosshair.Size = new System.Drawing.Size(38, 35);
            this.leftToonCrosshair.TabIndex = 13;
            this.leftToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.leftToonCrosshair_WindowSelected);
            // 
            // MulticontrollerWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(228, 138);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.rightToonCrosshair);
            this.Controls.Add(this.leftToonCrosshair);
            this.Controls.Add(this.windowGroupsBtn);
            this.Controls.Add(this.optionsBtn);
            this.Controls.Add(this.wndGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MulticontrollerWnd";
            this.Padding = new System.Windows.Forms.Padding(6, 6, 3, 26);
            this.Text = "Toontown Multicontroller";
            this.Activated += new System.EventHandler(this.MulticontrollerWnd_Activated);
            this.Deactivate += new System.EventHandler(this.MulticontrollerWnd_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWnd_FormClosing);
            this.Load += new System.EventHandler(this.MulticontrollerWnd_Load);
            this.wndGroup.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox wndGroup;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button optionsBtn;
        private System.Windows.Forms.Button windowGroupsBtn;
        public TTMulti.Controls.SelectWindowCrosshair leftToonCrosshair;
        public TTMulti.Controls.SelectWindowCrosshair rightToonCrosshair;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel leftStatusLbl;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel rightStatusLbl;
        private System.Windows.Forms.RadioButton mirrorModeRadio;
        private System.Windows.Forms.RadioButton multiModeRadio;
    }
}

