namespace TTMulti.Forms
{
    partial class WindowGroupsForm
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
            this.okBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.addGroupBtn = new System.Windows.Forms.Button();
            this.removeGroupBtn = new System.Windows.Forms.Button();
            this.groupsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.windowPairGroup1 = new TTMulti.Controls.ControllerGroupView();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupsFlowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(352, 473);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(75, 25);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "Close";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 53);
            this.label1.TabIndex = 7;
            this.label1.Text = "Use the number keys to switch between groups in Multi-Mode. Mirror Mode will cont" +
    "rol all groups at the same time.";
            // 
            // addGroupBtn
            // 
            this.addGroupBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addGroupBtn.Location = new System.Drawing.Point(12, 440);
            this.addGroupBtn.Margin = new System.Windows.Forms.Padding(4);
            this.addGroupBtn.Name = "addGroupBtn";
            this.addGroupBtn.Size = new System.Drawing.Size(115, 25);
            this.addGroupBtn.TabIndex = 1;
            this.addGroupBtn.Text = "Add Group";
            this.toolTip1.SetToolTip(this.addGroupBtn, "Add a new group");
            this.addGroupBtn.UseVisualStyleBackColor = true;
            this.addGroupBtn.Click += new System.EventHandler(this.addGroupBtn_Click);
            // 
            // removeGroupBtn
            // 
            this.removeGroupBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeGroupBtn.Enabled = false;
            this.removeGroupBtn.Location = new System.Drawing.Point(12, 473);
            this.removeGroupBtn.Margin = new System.Windows.Forms.Padding(4);
            this.removeGroupBtn.Name = "removeGroupBtn";
            this.removeGroupBtn.Size = new System.Drawing.Size(115, 25);
            this.removeGroupBtn.TabIndex = 2;
            this.removeGroupBtn.Text = "Remove Group";
            this.toolTip1.SetToolTip(this.removeGroupBtn, "Remove the last group");
            this.removeGroupBtn.UseVisualStyleBackColor = true;
            this.removeGroupBtn.Click += new System.EventHandler(this.removeGroupBtn_Click);
            // 
            // groupsFlowPanel
            // 
            this.groupsFlowPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupsFlowPanel.AutoScroll = true;
            this.groupsFlowPanel.Controls.Add(this.windowPairGroup1);
            this.groupsFlowPanel.Location = new System.Drawing.Point(12, 65);
            this.groupsFlowPanel.Name = "groupsFlowPanel";
            this.groupsFlowPanel.Size = new System.Drawing.Size(415, 368);
            this.groupsFlowPanel.TabIndex = 5;
            // 
            // windowPairGroup1
            // 
            this.windowPairGroup1.AutoSize = true;
            this.windowPairGroup1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.windowPairGroup1.Location = new System.Drawing.Point(3, 3);
            this.windowPairGroup1.Name = "windowPairGroup1";
            this.windowPairGroup1.Size = new System.Drawing.Size(189, 90);
            this.windowPairGroup1.TabIndex = 0;
            // 
            // WindowGroupsForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(439, 510);
            this.Controls.Add(this.removeGroupBtn);
            this.Controls.Add(this.groupsFlowPanel);
            this.Controls.Add(this.addGroupBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.okBtn);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(457, 450);
            this.Name = "WindowGroupsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Window Groups";
            this.Activated += new System.EventHandler(this.WindowGroupsForm_Activated);
            this.Deactivate += new System.EventHandler(this.WindowGroupsForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowGroupsForm_FormClosing);
            this.groupsFlowPanel.ResumeLayout(false);
            this.groupsFlowPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addGroupBtn;
        private System.Windows.Forms.Button removeGroupBtn;
        private System.Windows.Forms.FlowLayoutPanel groupsFlowPanel;
        private Controls.ControllerGroupView windowPairGroup1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}