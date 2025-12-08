
using System;
using System.Drawing;
using System.Windows.Forms;

namespace examer
{

    public partial class MainMenuForm : Form
    {
        /// Initializes the main menu form, sets title and icon.
        public MainMenuForm()
        {
            InitializeComponent();
            this.Text = "Mahjong Game";
            // so it doesnt error when it cant load in
            try
            {
                this.Icon = new Icon("mahjong.ico");
                this.BackgroundImage = Image.FromFile("mahjongbg.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch {}
        }


        /// Handles Play button click to start the game.
        private void PlayButton_Click(object sender, EventArgs e)
        {
            Pattern1 newForm1 = new Pattern1();
            newForm1.Show();
            this.Hide();
        }

        /// Handles Exit button click to exit the application.
        private void ExitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
