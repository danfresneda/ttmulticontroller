
namespace TTMulti.Controls
{
    partial class ControllerGroupView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pairsLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.windowPair1 = new TTMulti.Controls.ControllerPairView();
            this.pairsLayoutPanel.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pairsLayoutPanel
            // 
            this.pairsLayoutPanel.AutoSize = true;
            this.pairsLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pairsLayoutPanel.Controls.Add(this.windowPair1);
            this.pairsLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pairsLayoutPanel.Location = new System.Drawing.Point(6, 26);
            this.pairsLayoutPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.pairsLayoutPanel.Name = "pairsLayoutPanel";
            this.pairsLayoutPanel.Size = new System.Drawing.Size(174, 46);
            this.pairsLayoutPanel.TabIndex = 0;
            // 
            // groupBox
            // 
            this.groupBox.AutoSize = true;
            this.groupBox.Controls.Add(this.pairsLayoutPanel);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Padding = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.groupBox.Size = new System.Drawing.Size(186, 87);
            this.groupBox.TabIndex = 5;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Toon Group #";
            this.toolTip1.SetToolTip(this.groupBox, "A group of toons that can be controlled together, which can have multiple pairs o" +
        "f toons");
            // 
            // windowPair1
            // 
            this.windowPair1.AutoSize = true;
            this.windowPair1.Location = new System.Drawing.Point(3, 3);
            this.windowPair1.Name = "windowPair1";
            this.windowPair1.Size = new System.Drawing.Size(168, 40);
            this.windowPair1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.windowPair1, "A pair of toons further divides a group into left and right toons");
            // 
            // WindowGroupView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.groupBox);
            this.Name = "WindowGroupView";
            this.Size = new System.Drawing.Size(189, 90);
            this.pairsLayoutPanel.ResumeLayout(false);
            this.pairsLayoutPanel.PerformLayout();
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pairsLayoutPanel;
        private ControllerPairView windowPair1;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
