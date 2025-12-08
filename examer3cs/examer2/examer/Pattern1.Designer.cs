namespace examer
{
    partial class Pattern1
    {
        private System.ComponentModel.IContainer components = null;
        private Button exitButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            exitButton = new Button();
            SuspendLayout();
            exitButton.Location = new Point(12, 502);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(75, 23);
            exitButton.TabIndex = 0;
            exitButton.Text = "Exit";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 537);
            Controls.Add(exitButton);
            Name = "Pattern1";
            Text = "Mahjong Pattern 1";
            try
            {
                this.Icon = new Icon("mahjong.ico");
                this.BackgroundImage = Image.FromFile("mahjongbg.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch {}

            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}
