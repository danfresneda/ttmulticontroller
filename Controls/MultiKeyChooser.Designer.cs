namespace TTMulti.Controls
{
    partial class MultiKeyChooser
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.addCustomControlBtn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.keyChooser2 = new TTMulti.Controls.KeyChooser();
            this.keyChooser1 = new TTMulti.Controls.KeyChooser();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Controls.Add(this.keyChooser2, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.keyChooser1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.addCustomControlBtn, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.button1, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(372, 79);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Left ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(181, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 30);
            this.label8.TabIndex = 23;
            this.label8.Text = "→";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addCustomControlBtn
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.addCustomControlBtn, 3);
            this.addCustomControlBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addCustomControlBtn.Location = new System.Drawing.Point(45, 53);
            this.addCustomControlBtn.Name = "addCustomControlBtn";
            this.addCustomControlBtn.Size = new System.Drawing.Size(294, 23);
            this.addCustomControlBtn.TabIndex = 22;
            this.addCustomControlBtn.Text = "+ Add Custom Control";
            this.addCustomControlBtn.UseVisualStyleBackColor = true;
            this.addCustomControlBtn.Click += new System.EventHandler(this.addCustomControlBtn_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(345, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "-";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // keyChooser2
            // 
            this.keyChooser2.ChosenKey = System.Windows.Forms.Keys.None;
            this.keyChooser2.ChosenKeyCode = 0;
            this.keyChooser2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyChooser2.Location = new System.Drawing.Point(210, 24);
            this.keyChooser2.Margin = new System.Windows.Forms.Padding(4);
            this.keyChooser2.MinimumSize = new System.Drawing.Size(50, 22);
            this.keyChooser2.Name = "keyChooser2";
            this.keyChooser2.Size = new System.Drawing.Size(128, 22);
            this.keyChooser2.TabIndex = 3;
            // 
            // keyChooser1
            // 
            this.keyChooser1.ChosenKey = System.Windows.Forms.Keys.None;
            this.keyChooser1.ChosenKeyCode = 0;
            this.keyChooser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.keyChooser1.Location = new System.Drawing.Point(46, 24);
            this.keyChooser1.Margin = new System.Windows.Forms.Padding(4);
            this.keyChooser1.MinimumSize = new System.Drawing.Size(50, 22);
            this.keyChooser1.Name = "keyChooser1";
            this.keyChooser1.Size = new System.Drawing.Size(128, 22);
            this.keyChooser1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 17);
            this.label2.TabIndex = 25;
            this.label2.Text = "You Press:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(209, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 17);
            this.label3.TabIndex = 26;
            this.label3.Text = "Toontown Expects:";
            // 
            // MultiKeyChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MultiKeyChooser";
            this.Size = new System.Drawing.Size(373, 239);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private KeyChooser keyChooser2;
        private System.Windows.Forms.Label label1;
        private KeyChooser keyChooser1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button addCustomControlBtn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
