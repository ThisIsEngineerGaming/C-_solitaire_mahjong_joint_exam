using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace examer
{
    public partial class Pattern1 : Form
    {
        private HighScoreManager scoreManager = new HighScoreManager("score.txt");
        private int highScore = 0;
        private BoardLogic board;
        private List<TileObject> selectedTiles = new List<TileObject>();
        private bool suppressCheckedChanged;
        private static readonly Random rng = new Random();

        public Pattern1()
        {
            InitializeComponent();
            board = new BoardLogic();
        }

        private string scoreFile = "score.txt";
        private int score = 0;

        private void Form1_Load(object sender, EventArgs e)
        {

            highScore = scoreManager.LoadHighScore();
            score = 0;
            UpdateScoreLabel();
            int tileW = 40;
            int tileH = 60;
            Point center = new Point(350, 220);

            PictureBox CreateTile(string name, int bx, int by, int bz)
            {
                var pb = new PictureBox
                {
                    Width = tileW,
                    Height = tileH,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    Tag = name // store tile name for reference
                };

                // Load image from tileAssets folder
                string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tileAssets", $"{name}.png");
                if (File.Exists(imgPath))
                {
                    using (var temp = Image.FromFile(imgPath))
                    {
                        pb.Image = new Bitmap(temp);
                    }
                }
                else
                {
                    pb.BackColor = Color.Gray;
                }

                pb.Click += Tile_Click;

                int offsetX = tileW / 5;
                int offsetY = tileH / 2;

                pb.Location = new Point(
                    center.X + bx * tileW + bz * offsetX,
                    center.Y + by * tileH - bz * offsetY
                );

                gamePanel.Controls.Add(pb);
                pb.BringToFront();

                return pb;
            }

            // Add a tile to the board
            void AddTile(string suit, int value, int bx, int by, int bz)
            {
                string tileName = $"{suit}{value}";
                var pb = CreateTile(tileName, bx, by, bz);

                board.TilesOnBoard.Add(new TileObject(
                    new Tile { Suit = suit, Value = value },
                    bx, by, bz,
                    pb
                ));
            }



            // Generating layers
            for (int x = -6; x <= 6; x++)
            {
                for (int y = -3; y <= 3; y++)
                {
                    AddTile("B", 1, x, y, 0);
                }
            }

            for (int x = -5; x <= 5; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    AddTile("CR", 2, x, y, 1);
                }
            }


            for (int x = -4; x <= 4; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    AddTile("CHR", 3, x, y, 2);
                }
            }


            for (int x = -2; x <= 2; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    AddTile("WND", 1, x, y, 3);
                }
            }

            //AddTile("DR", 1, 0, 0, 4);

            // Wings (left side protrusions)
            AddTile("SSN", 1, -7, -2, 0);
            AddTile("SSN", 2, -7, -1, 0);
            AddTile("SSN", 3, -7, 0, 0);
            AddTile("SSN", 4, -7, 1, 0);
            AddTile("SSN", 1, -7, 2, 0);

            // Wings (right side protrusions)
            AddTile("FLW", 1, 7, -2, 0);
            AddTile("FLW", 2, 7, -1, 0);
            AddTile("FLW", 3, 7, 0, 0);
            AddTile("FLW", 4, 7, 1, 0);
            AddTile("FLW", 1, 7, 2, 0);



            // Assign solvable random names
            AssignRandomNamesInPairsSolvable(board.TilesOnBoard);

            // Update availability (blocked tiles disabled)
            UpdateTileAvailability();
        }
    }
}
