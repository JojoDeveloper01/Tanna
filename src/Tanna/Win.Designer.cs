﻿namespace Tanna
{
    partial class Win
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
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 70F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(125, 157);
            label1.Name = "label1";
            label1.Size = new Size(513, 125);
            label1.TabIndex = 0;
            label1.Text = "You Win!!!";
            // 
            // Win
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Yellow;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            ForeColor = SystemColors.ActiveCaptionText;
            Name = "Win";
            Text = "Win";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
    }
}