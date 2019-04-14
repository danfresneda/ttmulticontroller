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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rightToonLbl = new System.Windows.Forms.Label();
            this.leftToonLbl = new System.Windows.Forms.Label();
            this.rightToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.leftToonCrosshair = new TTMulti.Controls.SelectWindowCrosshair();
            this.addGroupBtn = new System.Windows.Forms.Button();
            this.removeGroupBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(178, 78);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rightToonLbl);
            this.groupBox1.Controls.Add(this.leftToonLbl);
            this.groupBox1.Controls.Add(this.rightToonCrosshair);
            this.groupBox1.Controls.Add(this.leftToonCrosshair);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(172, 72);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Group 1";
            // 
            // rightToonLbl
            // 
            this.rightToonLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonLbl.AutoSize = true;
            this.rightToonLbl.Location = new System.Drawing.Point(106, 16);
            this.rightToonLbl.Name = "rightToonLbl";
            this.rightToonLbl.Size = new System.Drawing.Size(60, 13);
            this.rightToonLbl.TabIndex = 3;
            this.rightToonLbl.Text = "Right Toon";
            // 
            // leftToonLbl
            // 
            this.leftToonLbl.AutoSize = true;
            this.leftToonLbl.Location = new System.Drawing.Point(6, 16);
            this.leftToonLbl.Name = "leftToonLbl";
            this.leftToonLbl.Size = new System.Drawing.Size(53, 13);
            this.leftToonLbl.TabIndex = 2;
            this.leftToonLbl.Text = "Left Toon";
            // 
            // rightToonCrosshair
            // 
            this.rightToonCrosshair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.rightToonCrosshair.Location = new System.Drawing.Point(118, 32);
            this.rightToonCrosshair.Name = "rightToonCrosshair";
            this.rightToonCrosshair.SelectedBorderColor = System.Drawing.Color.DarkGreen;
            this.rightToonCrosshair.Size = new System.Drawing.Size(36, 36);
            this.rightToonCrosshair.TabIndex = 1;
            this.rightToonCrosshair.Tag = "right";
            this.rightToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.crosshair_WindowSelected);
            // 
            // leftToonCrosshair
            // 
            this.leftToonCrosshair.BackColor = System.Drawing.SystemColors.Control;
            this.leftToonCrosshair.Location = new System.Drawing.Point(14, 32);
            this.leftToonCrosshair.Name = "leftToonCrosshair";
            this.leftToonCrosshair.Padding = new System.Windows.Forms.Padding(2);
            this.leftToonCrosshair.SelectedBorderColor = System.Drawing.Color.LimeGreen;
            this.leftToonCrosshair.Size = new System.Drawing.Size(36, 36);
            this.leftToonCrosshair.TabIndex = 0;
            this.leftToonCrosshair.Tag = "left";
            this.leftToonCrosshair.WindowSelected += new TTMulti.Controls.WindowSelectedHandler(this.crosshair_WindowSelected);
            // 
            // addGroupBtn
            // 
            this.addGroupBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addGroupBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addGroupBtn.Location = new System.Drawing.Point(188, 12);
            this.addGroupBtn.Name = "addGroupBtn";
            this.addGroupBtn.Size = new System.Drawing.Size(32, 32);
            this.addGroupBtn.TabIndex = 1;
            this.addGroupBtn.Text = "+";
            this.addGroupBtn.UseVisualStyleBackColor = true;
            this.addGroupBtn.Click += new System.EventHandler(this.addGroupBtn_Click);
            // 
            // removeGroupBtn
            // 
            this.removeGroupBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.removeGroupBtn.Enabled = false;
            this.removeGroupBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeGroupBtn.Location = new System.Drawing.Point(188, 50);
            this.removeGroupBtn.Name = "removeGroupBtn";
            this.removeGroupBtn.Size = new System.Drawing.Size(32, 32);
            this.removeGroupBtn.TabIndex = 2;
            this.removeGroupBtn.Text = "-";
            this.removeGroupBtn.UseVisualStyleBackColor = true;
            this.removeGroupBtn.Click += new System.EventHandler(this.removeGroupBtn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(7, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(213, 44);
            this.label1.TabIndex = 3;
            this.label1.Text = "Use the number keys to switch between groups in Multi-Mode. Mirror Mode will cont" +
    "rol all groups at the same time.";
            // 
            // okBtn
            // 
            this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.okBtn.Location = new System.Drawing.Point(63, 145);
            this.okBtn.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(100, 27);
            this.okBtn.TabIndex = 4;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = true;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // WindowGroupsForm
            // 
            this.AcceptButton = this.okBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(226, 180);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.removeGroupBtn);
            this.Controls.Add(this.addGroupBtn);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WindowGroupsForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 85);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Window Groups";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowGroupsForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private TTMulti.Controls.SelectWindowCrosshair rightToonCrosshair;
        private TTMulti.Controls.SelectWindowCrosshair leftToonCrosshair;
        private System.Windows.Forms.Label rightToonLbl;
        private System.Windows.Forms.Label leftToonLbl;
        private System.Windows.Forms.Button addGroupBtn;
        private System.Windows.Forms.Button removeGroupBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button okBtn;
    }
}