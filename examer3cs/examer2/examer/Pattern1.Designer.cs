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
        // stuff for the basic form design
        private void InitializeComponent()
        {
            //buttonExit
            exitButton = new Button();
            SuspendLayout();
            exitButton.Location = new Point(12, 502);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(75, 23);
            exitButton.TabIndex = 0;
            exitButton.Text = "Back";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += ExitButton_Click;
            AutoScaleDimensions = new SizeF(7F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(704, 537);
            Controls.Add(exitButton);
            //scoreLabel
            scoreLabel = new Label();
            scoreLabel.Location = new Point(200, 10);
            scoreLabel.Font = new Font("Arial", 14, FontStyle.Bold);
            scoreLabel.ForeColor = Color.White;
            scoreLabel.BackColor = Color.Transparent;
            scoreLabel.AutoSize = true;
            scoreLabel.Text = "Score: 0";
            Controls.Add(scoreLabel);
            // Name, title and sprites for the form
            Name = "Pattern1";
            Text = "Mahjong Pattern 1";
            // so it doesnt error when it cant load in
            try
            {
                this.Icon = new Icon("mahjong.ico");
                this.BackgroundImage = Image.FromFile("mahjongbg.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch {}
            gamePanel = new Panel();
            gamePanel.BackColor = Color.Transparent;
            gamePanel.Size = new Size(704, 537);
            gamePanel.Location = new Point(0, 0);

            gamePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Controls.Add(gamePanel);
            gamePanel.SendToBack();


            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Panel gamePanel;
    }
}
