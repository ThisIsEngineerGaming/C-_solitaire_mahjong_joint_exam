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
        // Basic tile selection and matching logic
        private void tiles_CheckedChanged(object? sender, EventArgs e)
        {
            if (suppressCheckedChanged) return;
            if (sender is not CheckBox cb) return;

            var tileObj = board.FindByVisual(cb);
            if (tileObj == null) return;

            if (!board.IsTileFree(tileObj))
            {
                suppressCheckedChanged = true;
                cb.Checked = false;
                suppressCheckedChanged = false;
                return;
            }

            if (cb.Checked)
            {
                if (!selectedTiles.Contains(tileObj))
                    selectedTiles.Add(tileObj);
            }
            else
            {
                selectedTiles.Remove(tileObj);
            }

            if (selectedTiles.Count == 2)
            {
                HandleSelection(selectedTiles[0], selectedTiles[1]);
                ClearSelectionVisuals();
            }
        }
        // The bulk of the game logic. Checks if they can be matched, tries to perform it, updates tile states, checks for win/loss
        private void HandleSelection(TileObject a, TileObject b)
        {
            if (a == null || b == null) return;

            if (!a.TileData.IsMatching(b.TileData)) return;
            if (!board.IsTileFree(a) || !board.IsTileFree(b)) return;

            if (board.TryMatch(a, b))
            {
                score += 100;
                UpdateScoreLabel();
                UpdateTileAvailability();
                CheckForWin();
                CheckForLoss();
            }
        }
        // Updates the enabled/disabled state of tiles based on whether they are free or blocked
        private void UpdateTileAvailability()
        {
            foreach (var t in board.TilesOnBoard)
            {
                if (t.Visual is PictureBox pb)
                {
                    bool free = board.IsTileFree(t);

                    pb.Enabled = pb.Visible && free;

                    if (free)
                    {
                        UpdateTileImage(t);
                    }
                    else
                    {
                        if (pb.Image != null)
                            pb.Image = MakeBlockedImage(pb.Image);
                    }
                }
            }
        }
        // If there are no tiles, then epic win
        private void CheckForWin()
        {
            if (board.TilesOnBoard.Count == 0)
            {
                DialogResult result = MessageBox.Show(
                    "Congrats, you matched everything.\nPress OK to go back to the menu...",
                    "You Win!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (result == DialogResult.OK)
                {
                    this.Hide();
                    SaveScore(score);
                    MainMenuForm menu = new MainMenuForm();
                    menu.Show();
                    this.Close();
                }
            }
        }
        // if there are no matching pairs left BUT there are still tiles remaining, then you lose(its really difficult to lose though)
        private void CheckForLoss()
        {
            if (!HasAnyAvailableMoves() && board.TilesOnBoard.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "You cant match anything else now.\nClick OK to go back to the main menu.",
                    "Game Over",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.OK)
                {
                    this.Hide();
                    SaveScore(score);
                    MainMenuForm menu = new MainMenuForm();
                    menu.Show();
                    this.Close();
                }
            }
        }
        // This is what actually checks for available moves for the loss condition
        private bool HasAnyAvailableMoves()
        {
            var freeTiles = board.TilesOnBoard
                .Where(t => board.IsTileFree(t))
                .ToList();

            for (int i = 0; i < freeTiles.Count; i++)
            {
                for (int j = i + 1; j < freeTiles.Count; j++)
                {
                    if (freeTiles[i].TileData.IsMatching(freeTiles[j].TileData))
                        return true; 
                }
            }

            return false; 
        }

        // Logic for selecting a tile(for visuals and matching)
        private void Tile_Click(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pb) return;

            var tileObj = board.FindByVisual(pb);
            if (tileObj == null) return;

            if (!board.IsTileFree(tileObj))
                return; // blocked tile

            if (!selectedTiles.Contains(tileObj))
                selectedTiles.Add(tileObj);

            // highlight selection visually
            pb.BorderStyle = BorderStyle.Fixed3D;

            if (selectedTiles.Count == 2)
            {
                HandleSelection(selectedTiles[0], selectedTiles[1]);
                ClearSelectionVisuals();
            }
        }
        // Does what it says, clears selection highlights
        private void ClearSelectionVisuals()
        {
            foreach (var t in selectedTiles)
                if (t.Visual is PictureBox pb)
                    pb.BorderStyle = BorderStyle.None;

            selectedTiles.Clear();
        }

        // Very simple, press button, go to menu, wow
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Hide();

            MainMenuForm menu = new MainMenuForm();
            menu.Show();

            this.Close();
        }


        // Most miserable logic that made us go insane. Creates a random but solvable tile set with way too many checks
        private void AssignRandomNamesInPairsSolvable(List<TileObject> tiles)
        {
            if (tiles == null || tiles.Count == 0) return;

            int count = tiles.Count;
            int toAssignPairs = count / 2;

            const int maxAttempts = 500;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                bool[] present = Enumerable.Repeat(true, count).ToArray();
                (string suit, int val)[] assigned = new (string, int)[count];
                bool[] isAssigned = new bool[count];
                bool failure = false;
                int assignedPairs = 0;

                while (assignedPairs < toAssignPairs)
                {
                    var freeIndices = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        if (!present[i] || isAssigned[i]) continue;
                        if (IsSimulatedFree(i, tiles, present))
                            freeIndices.Add(i);
                    }

                    if (freeIndices.Count < 2)
                    {
                        failure = true;
                        break;
                    }

                    int idxA = freeIndices[rng.Next(freeIndices.Count)];
                    int idxB;
                    do { idxB = freeIndices[rng.Next(freeIndices.Count)]; } while (idxB == idxA);

                    var tileType = GenerateRandomTile();

                    assigned[idxA] = tileType;
                    assigned[idxB] = tileType;
                    isAssigned[idxA] = isAssigned[idxB] = true;

                    present[idxA] = present[idxB] = false;
                    assignedPairs++;
                }

                if (!failure)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (isAssigned[i])
                        {
                            tiles[i].TileData.Suit = assigned[i].suit;
                            tiles[i].TileData.Value = assigned[i].val;
                        }

                        UpdateTileImage(tiles[i]);
                    }

                    return;
                }
            }

            AssignRandomNamesInPairsSimple(tiles);
        }
        // asset name getter for tiles
        private string GetFileName(Tile tile)
        {
            return tile.Suit switch
            {
                "B" => $"B{tile.Value}.png",
                "CR" => $"CR{tile.Value}.png",
                "CHR" => $"CHR{tile.Value}.png",

                "WND" => tile.Value switch
                {
                    1 => "WNDS.png",
                    2 => "WNDN.png",
                    3 => "WNDE.png",
                    4 => "WNDW.png",
                    _ => "WND?.png"
                },

                "DR" => tile.Value switch
                {
                    1 => "DRR.png",
                    2 => "DRG.png",
                    3 => "DRW.png",
                    _ => "DR?.png"
                },

                "SSN" => $"SSN{tile.Value}.png",
                "FLW" => $"FLW{tile.Value}.png",
                _ => $"{tile.Suit}{tile.Value}.png"
            };
        }

        private void UpdateScoreLabel()
        {
            scoreLabel.Text = $"Score: {score}   Highscore: {highScore}";
        }
        
        private void UpdateTileImage(TileObject tile)
        {
            if (tile.Visual is not PictureBox pb) return;

            string filename = GetFileName(tile.TileData);
            string imgPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tileAssets", filename);

            if (!File.Exists(imgPath))
            {
                pb.Image = null;
                return;
            }

            using (var temp = Image.FromFile(imgPath))
            {
                pb.Image = new Bitmap(temp);
            }

            if (pb.Image != null)
                pb.Image.Tag = null;
        }

        // part of the logic for generating solvable tile sets, 
        private void AssignRandomNamesInPairsSimple(List<TileObject> tiles)
        {
            int count = tiles.Count;
            int pairs = count / 2;
            var list = new List<(string suit, int value)>();
            for (int i = 0; i < pairs; i++)
            {
                var (suit, value) = GenerateRandomTile();
                list.Add((suit, value));
                list.Add((suit, value));
            }
            Shuffle(list);

            for (int i = 0; i < list.Count; i++)
            {
                tiles[i].TileData.Suit = list[i].suit;
                tiles[i].TileData.Value = list[i].value;
                if (tiles[i].Visual is CheckBox cb)
                    cb.Text = ShortName(tiles[i].TileData.Suit, tiles[i].TileData.Value);
            }

            if (count % 2 == 1)
            {
                var last = tiles[list.Count];
                if (last.Visual is CheckBox cb)
                    cb.Text = ShortName(last.TileData.Suit, last.TileData.Value);
            }

            for (int i = 0; i < list.Count; i++)
            {
                tiles[i].TileData.Suit = list[i].suit;
                tiles[i].TileData.Value = list[i].value;
                UpdateTileImage(tiles[i]);
            }

            if (count % 2 == 1)
                UpdateTileImage(tiles[list.Count]);
        }
        // Still for the generation logic
        private bool IsSimulatedFree(int idx, List<TileObject> tiles, bool[] present)
        {
            var tile = tiles[idx];
            if (tile == null) return false;

            bool hasTileOnTop = false;
            bool leftBlocked = false;
            bool rightBlocked = false;

            for (int j = 0; j < tiles.Count; j++)
            {
                if (j == idx) continue;
                if (!present[j]) continue;

                var t = tiles[j];

                // tile on top (higher Z, same X/Y)
                if (t.Z > tile.Z && t.X == tile.X && t.Y == tile.Y)
                    hasTileOnTop = true;

                // same layer blocking neighbors
                if (t.Z == tile.Z)
                {
                    if (t.X == tile.X - 1 && t.Y == tile.Y)
                        leftBlocked = true;
                    if (t.X == tile.X + 1 && t.Y == tile.Y)
                        rightBlocked = true;
                }
            }

            if (hasTileOnTop) return false;
            if (leftBlocked && rightBlocked) return false;
            return true;
        }
        // Stuff for making the blocked tile images grayed out and a bit transparent
        private Image MakeBlockedImage(Image original)
        {
            if (original == null)
                return null;

            if (original.Tag as string == "blocked")
                return original;

            float opacity = 0.7f;
            float grayL = 0.33f;

            Bitmap output = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(output))
            {
                var colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                    new float[][]
                    {
                new float[] { grayL, grayL, grayL, 0, 0 },
                new float[] { grayL, grayL, grayL, 0, 0 },
                new float[] { grayL, grayL, grayL, 0, 0 },

                new float[] { 0, 0, 0, opacity, 0 },

                new float[] { 0, 0, 0, 0, 1 }
                    });

                var attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(
                    original,
                    new Rectangle(0, 0, output.Width, output.Height),
                    0, 0, original.Width, original.Height,
                    GraphicsUnit.Pixel,
                    attributes
                );
            }
            // What allows to do the check 4 it
            output.Tag = "blocked";

            return output;
        }
        // Gambling for tiles and all their variations, lets go
        private (string suit, int value) GenerateRandomTile()
        {
            string[] suits = { "B", "CR", "CHR", "WND", "DR", "SSN", "FLW" };
            string suit = suits[rng.Next(suits.Length)];
            int value = 1;

            switch (suit)
            {
                case "B":
                case "CR":
                case "CHR":
                    value = rng.Next(1, 10); // 1..9
                    break;
                case "WND":
                    value = rng.Next(1, 5);  // 1..4 
                    break;
                case "DR":
                    value = rng.Next(1, 4);  // 1..3
                    break;
                case "SSN":
                case "FLW":
                    value = rng.Next(1, 5);  // 1..4
                    break;
            }

            return (suit, value);
        }
        //transforming the numbers into proper names (Used for logic and old display for when they did not have images)
        private string ShortName(string suit, int value)
        {
            if (string.IsNullOrEmpty(suit)) return "?";

            suit = suit.ToUpperInvariant();
            return suit switch
            {
                "B" => $"B{value}",
                "CR" => $"CR{value}",
                "CHR" => $"CHR{value}",
                "WND" => value switch
                {
                    1 => "WNDS",
                    2 => "WNDN",
                    3 => "WNDE",
                    4 => "WNDW",
                    _ => "WND?"
                },
                "DRG" => value switch
                {
                    1 => "DRR",
                    2 => "DRG",
                    3 => "DRW",
                    _ => "DR?"
                },
                "SSN" => $"SSN{value}",
                "FLW" => $"FLW{value}",
                _ => $"{suit}{value}"
            };
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        private void SaveScore(int lastScore)
        {
            if (lastScore > highScore)
                highScore = lastScore;

            scoreManager.SaveScore(lastScore, highScore);
        }
    }
}
