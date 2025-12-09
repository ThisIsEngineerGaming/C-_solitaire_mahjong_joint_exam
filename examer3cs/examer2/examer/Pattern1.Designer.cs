namespace examer
{
    public partial class Pattern1
    {
        private System.ComponentModel.IContainer components = null;
        private Button exitButton;

        // 👉 Add this field so your C# class compiles
        private Label scoreLabel;

        private Panel gamePanel;

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
            // Exit button
            exitButton = new Button();
            SuspendLayout();
            exitButton.Location = new Point(12, 502);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(75, 23);
            exitButton.TabIndex = 0;
            exitButton.Text = "Back";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;

            // Score label
            scoreLabel = new Label();
            scoreLabel.Location = new Point(200, 10);
            scoreLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            scoreLabel.ForeColor = Color.White;
            scoreLabel.BackColor = Color.Transparent;
            scoreLabel.AutoSize = true;
            scoreLabel.Text = "Score: 0";

            // Form setup
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 537);
            Controls.Add(exitButton);
            Controls.Add(scoreLabel);

            Name = "Pattern1";
            Text = "Mahjong Pattern 1";

            // Background image & icon
            try
            {
                this.Icon = new Icon("mahjong.ico");
                this.BackgroundImage = Image.FromFile("mahjongbg.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }

            // Game panel
            gamePanel = new Panel();
            gamePanel.BackColor = Color.Transparent;
            gamePanel.Size = new Size(704, 537);
            gamePanel.Location = new Point(0, 0);
            gamePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

            Controls.Add(gamePanel);
            gamePanel.SendToBack();

            // Form Load
            Load += Form1_Load;

            ResumeLayout(false);
        }
        #endregion
    }
}
