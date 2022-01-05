
namespace TTMulti.Controls
{
    partial class ControllerPairView
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
            this.rightToonLbl = new System.Windows.Forms.Label();
            this.leftToonLbl = new System.Windows.Forms.Label();
            this.rightToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.leftToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.SuspendLayout();
            // 
            // rightToonLbl
            // 
            this.rightToonLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonLbl.AutoSize = true;
            this.rightToonLbl.Location = new System.Drawing.Point(87, 0);
            this.rightToonLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rightToonLbl.Name = "rightToonLbl";
            this.rightToonLbl.Size = new System.Drawing.Size(41, 34);
            this.rightToonLbl.TabIndex = 7;
            this.rightToonLbl.Text = "Right\r\nToon";
            this.rightToonLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // leftToonLbl
            // 
            this.leftToonLbl.AutoSize = true;
            this.leftToonLbl.Location = new System.Drawing.Point(-3, 0);
            this.leftToonLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.leftToonLbl.Name = "leftToonLbl";
            this.leftToonLbl.Size = new System.Drawing.Size(41, 34);
            this.leftToonLbl.TabIndex = 6;
            this.leftToonLbl.Text = "Left\r\nToon";
            this.leftToonLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // rightToonCrosshair
            // 
            this.rightToonCrosshair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.rightToonCrosshair.Location = new System.Drawing.Point(132, 0);
            this.rightToonCrosshair.Margin = new System.Windows.Forms.Padding(4);
            this.rightToonCrosshair.Name = "rightToonCrosshair";
            this.rightToonCrosshair.Padding = new System.Windows.Forms.Padding(2);
            this.rightToonCrosshair.SelectedBorderColor = System.Drawing.Color.DarkGreen;
            this.rightToonCrosshair.Size = new System.Drawing.Size(36, 36);
            this.rightToonCrosshair.TabIndex = 5;
            this.rightToonCrosshair.Tag = "right";
            this.rightToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.rightToonCrosshair_WindowSelected);
            // 
            // leftToonCrosshair
            // 
            this.leftToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.leftToonCrosshair.Location = new System.Drawing.Point(40, 0);
            this.leftToonCrosshair.Margin = new System.Windows.Forms.Padding(4);
            this.leftToonCrosshair.Name = "leftToonCrosshair";
            this.leftToonCrosshair.Padding = new System.Windows.Forms.Padding(2);
            this.leftToonCrosshair.SelectedBorderColor = System.Drawing.Color.LimeGreen;
            this.leftToonCrosshair.Size = new System.Drawing.Size(36, 36);
            this.leftToonCrosshair.TabIndex = 4;
            this.leftToonCrosshair.Tag = "left";
            this.leftToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.leftToonCrosshair_WindowSelected);
            // 
            // WindowPairView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.leftToonLbl);
            this.Controls.Add(this.rightToonLbl);
            this.Controls.Add(this.rightToonCrosshair);
            this.Controls.Add(this.leftToonCrosshair);
            this.Name = "WindowPairView";
            this.Size = new System.Drawing.Size(168, 37);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label rightToonLbl;
        private System.Windows.Forms.Label leftToonLbl;
        private SelectWindowCrosshair rightToonCrosshair;
        private SelectWindowCrosshair leftToonCrosshair;
    }
}
