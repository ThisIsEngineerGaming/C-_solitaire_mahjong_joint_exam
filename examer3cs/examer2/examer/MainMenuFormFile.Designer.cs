namespace examer
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    partial class MainMenuForm : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PlayButton = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // PlayButton
            // 
            PlayButton.Location = new Point(358, 199);
            PlayButton.Name = "PlayButton";
            PlayButton.Size = new Size(75, 23);
            PlayButton.TabIndex = 0;
            PlayButton.Text = "Play";
            PlayButton.UseVisualStyleBackColor = true;
            PlayButton.Click += PlayButton_Click;
            // 
            // ExitButton
            // 
            button2.Location = new Point(358, 256);
            button2.Name = "ExitButton";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 1;
            button2.Text = "Exit";
            button2.UseVisualStyleBackColor = true;
            button2.Click += this.ExitButton_Click;
            // 
            // MainMenu
            // 
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button2);
            Controls.Add(PlayButton);
            Name = "MainMenu";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button PlayButton;
        private Button button2;
    }

}