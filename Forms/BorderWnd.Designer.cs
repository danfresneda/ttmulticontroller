﻿namespace TTMulti.Forms
{
    partial class BorderWnd
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
            this.fakeCursorImg = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.fakeCursorImg)).BeginInit();
            this.SuspendLayout();
            // 
            // fakeCursorImg
            // 
            this.fakeCursorImg.Image = global::TTMulti.Properties.Resources.dupcursor;
            this.fakeCursorImg.Location = new System.Drawing.Point(157, 200);
            this.fakeCursorImg.Name = "fakeCursorImg";
            this.fakeCursorImg.Size = new System.Drawing.Size(32, 32);
            this.fakeCursorImg.TabIndex = 1;
            this.fakeCursorImg.TabStop = false;
            this.fakeCursorImg.Visible = false;
            // 
            // BorderWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Fuchsia;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.fakeCursorImg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "BorderWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "BorderWnd";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            ((System.ComponentModel.ISupportInitialize)(this.fakeCursorImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox fakeCursorImg;
    }
}