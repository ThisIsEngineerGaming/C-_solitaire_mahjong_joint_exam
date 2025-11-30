namespace examer
{
    partial class Form1
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
            tile1 = new CheckBox();
            tile2 = new CheckBox();
            tile3 = new CheckBox();
            tile4 = new CheckBox();
            SuspendLayout();
            // 
            // tile1
            // 
            tile1.AutoSize = true;
            tile1.Location = new Point(524, 207);
            tile1.Name = "tile1";
            tile1.Size = new Size(54, 17);
            tile1.TabIndex = 0;
            tile1.Text = "tile";
            tile1.UseVisualStyleBackColor = true;
            tile1.CheckedChanged += tiles_CheckedChanged;
            // 
            // tile2
            // 
            tile2.AutoSize = true;
            tile2.Location = new Point(404, 207);
            tile2.Name = "tile2";
            tile2.Size = new Size(54, 17);
            tile2.TabIndex = 1;
            tile2.Text = "tile";
            tile2.UseVisualStyleBackColor = true;
            tile2.CheckedChanged += tiles_CheckedChanged;
            // 
            // tile3
            // 
            tile3.AutoSize = true;
            tile3.Location = new Point(524, 97);
            tile3.Name = "tile3";
            tile3.Size = new Size(61, 17);
            tile3.TabIndex = 2;
            tile3.Text = "tile1";
            tile3.UseVisualStyleBackColor = true;
            tile3.CheckedChanged += tiles_CheckedChanged;
            // 
            // tile4
            // 
            tile4.AutoSize = true;
            tile4.Location = new Point(404, 97);
            tile4.Name = "tile4";
            tile4.Size = new Size(61, 17);
            tile4.TabIndex = 3;
            tile4.Text = "tile1";
            tile4.UseVisualStyleBackColor = true;
            tile4.CheckedChanged += tiles_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tile4);
            Controls.Add(tile3);
            Controls.Add(tile2);
            Controls.Add(tile1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox tile1;
        private CheckBox tile2;
        private CheckBox tile3;
        private CheckBox tile4;
    }
}
