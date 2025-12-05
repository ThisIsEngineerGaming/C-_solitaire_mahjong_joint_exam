
using System;
using System.Drawing;
using System.Windows.Forms;

namespace examer
{
    /// <summary>
    /// Main menu form for the Mahjong game.
    /// </summary>
    public partial class MainMenuForm : Form
    {
        /// <summary>
        /// Initializes the main menu form, sets title and icon.
        /// </summary>
        public MainMenuForm()
        {
            InitializeComponent();
            this.Text = "Mahjong Game";
            try
            {
                this.Icon = new Icon("mahjong.ico"); // Place mahjong.ico in output directory
            }
            catch { /* Ignore if icon not found */ }
        }

        /// <summary>
        /// Handles Play button click to start the game.
        /// </summary>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            Pattern1 newForm1 = new Pattern1();
            newForm1.Show();
            this.Hide();
        }

        /// <summary>
        /// Handles Exit button click to exit the application.
        /// </summary>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
