using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace examer
{
    public partial class Pattern1 : Form
    {
        private BoardLogic board;
        private List<TileObject> selectedTiles = new List<TileObject>();
        private bool suppressCheckedChanged;

        public Pattern1()
        {
            InitializeComponent();
            board = new BoardLogic();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int tileW = 60;
            int tileH = 30;
            int zOffsetX = 6;
            int zOffsetY = 10;
            Point center = new Point(350, 220);

            CheckBox CreateTile(string name, int bx, int by, int bz)
            {
                var cb = new CheckBox
                {
                    AutoSize = false,
                    Width = tileW,
                    Height = tileH,
                    Text = name,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                cb.CheckedChanged += tiles_CheckedChanged;

                cb.Location = new Point(
                    center.X + bx * tileW + bz * zOffsetX,
                    center.Y + by * tileH - bz * zOffsetY
                );

                Controls.Add(cb);
                cb.BringToFront();
                return cb;
            }

            void AddTile(string suit, int value, int bx, int by, int bz)
            {
                var cb = CreateTile($"{suit}{value}", bx, by, bz);
                board.TilesOnBoard.Add(new TileObject(
                    new Tile { Suit = suit, Value = value },
                    bx, by, bz,
                    cb
                ));
            }

            for (int x = -3; x <= 3; x++)
            {
                AddTile("BMB", 1, x, -1, 0);
                AddTile("CRC", 7, x, 0, 0);
            }

            for (int x = -2; x <= 2; x++)
            {
                AddTile("CH", 5, x, -1, 1);
                AddTile("CH", 8, x, 0, 1);
            }

            AddTile("WND", 1, 0, -1, 2);
            AddTile("WND", 1, 0, 0, 2);
            AddTile("DRG", 1, 0, 0, 3);

            AddTile("FLW", 1, -3, -1, 0);
            AddTile("FLW", 2, -3, 0, 0);
            AddTile("FLW", 3, -4, 0, 1);

            AddTile("SSN", 1, 3, -1, 0);
            AddTile("SSN", 2, 3, 0, 0);
            AddTile("SSN", 3, 4, 0, 1);


            TileRandom.ShuffleTiles(board.TilesOnBoard);

            UpdateTileAvailability();
        }

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
                if (!selectedTiles.Contains(tileObj)) selectedTiles.Add(tileObj);
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

        private void HandleSelection(TileObject a, TileObject b)
        {
            if (a == null || b == null) return;

            if (!a.TileData.IsMatching(b.TileData))
            {
                return;
            }

            if (!board.IsTileFree(a) || !board.IsTileFree(b))
            {
                return;
            }

            if (board.TryMatch(a, b))
            {
                UpdateTileAvailability();
            }
            else
            {

            }
        }

        private void UpdateTileAvailability()
        {
            foreach (var t in board.TilesOnBoard)
            {
                if (t.Visual is Control c)
                {
                    c.Enabled = c.Visible && board.IsTileFree(t);
                }
            }
        }

        private void ClearSelectionVisuals()
        {
            suppressCheckedChanged = true;

            foreach (var t in selectedTiles)
                if (t.Visual is CheckBox cb) cb.Checked = false;

            selectedTiles.Clear();
            suppressCheckedChanged = false;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
