namespace examer
{
    partial class Pattern1
    {
        private System.ComponentModel.IContainer components = null;
        private Button exitButton;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        /// Required method for Designer support — do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            exitButton = new Button();
            SuspendLayout();
            // 
            // exitButton
            // 
            exitButton.Location = new Point(12, 502);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(75, 23);
            exitButton.TabIndex = 0;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;
            // 
            // Pattern1
            // 
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 537);
            Controls.Add(exitButton);
            Name = "Pattern1";
            Text = "Mahjong Tiles";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}
