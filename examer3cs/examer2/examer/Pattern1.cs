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
        private CheckBox? tile5;
        private CheckBox? tile6;

        public Pattern1()
        {
            InitializeComponent();
            board = new BoardLogic();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // create runtime checkboxes for the middle layer
            tile5 = new CheckBox { AutoSize = true, Name = "tile5", TabIndex = 4, UseVisualStyleBackColor = true };
            tile6 = new CheckBox { AutoSize = true, Name = "tile6", TabIndex = 5, UseVisualStyleBackColor = true };
            tile5.CheckedChanged += tiles_CheckedChanged;
            tile6.CheckedChanged += tiles_CheckedChanged;
            Controls.Add(tile5);
            Controls.Add(tile6);

            // labels
            tile1.Text = "Circle3";
            tile3.Text = "Circle3";
            tile5!.Text = "Bamboo2";
            tile6!.Text = "Bamboo2";
            tile2.Text = "EastWind";
            tile4.Text = "EastWind";

            // stack layout parameters
            var stackAX = 200;
            var stackBX = 320;
            var baseY = 200;
            var layerOffset = 14; // vertical offset between layers

            // Stack A positions (X=stackAX)
            tile2.Location = new Point(stackAX, baseY); // wind bottom (Z=0)
            tile5.Location = new Point(stackAX, baseY - layerOffset); // bamboo middle (Z=1)
            tile1.Location = new Point(stackAX + 6, baseY - 2 * layerOffset); // circle top (Z=2)

            // Stack B positions (X=stackBX)
            tile4.Location = new Point(stackBX, baseY); // wind bottom
            tile6!.Location = new Point(stackBX, baseY - layerOffset); // bamboo middle
            tile3.Location = new Point(stackBX + 6, baseY - 2 * layerOffset); // circle top

            // ensure circles visually on top
            tile1.BringToFront();
            tile3.BringToFront();
            tile5!.BringToFront();
            tile6!.BringToFront();
            tile2.BringToFront();
            tile4.BringToFront();

            // create TileObjects with Z ordering: top=2, middle=1, bottom=0
            var circleTopA = new TileObject(new Tile { Suit = "circle", Value = 3 }, 0, 0, 2, tile1);
            var bambooMidA = new TileObject(new Tile { Suit = "bamboo", Value = 2 }, 0, 0, 1, tile5!);
            var windBottomA = new TileObject(new Tile { Suit = "wind", Value = 1 }, 0, 0, 0, tile2);

            var circleTopB = new TileObject(new Tile { Suit = "circle", Value = 3 }, 1, 0, 2, tile3);
            var bambooMidB = new TileObject(new Tile { Suit = "bamboo", Value = 2 }, 1, 0, 1, tile6!);
            var windBottomB = new TileObject(new Tile { Suit = "wind", Value = 1 }, 1, 0, 0, tile4);

            board.TilesOnBoard.AddRange(new[] { circleTopA, bambooMidA, windBottomA, circleTopB, bambooMidB, windBottomB });

            UpdateTileAvailability();

            // clear any selections
            tile1.Checked = false;
            tile2.Checked = false;
            tile3.Checked = false;
            tile4.Checked = false;
            tile5!.Checked = false;
            tile6!.Checked = false;
        }

        private void tiles_CheckedChanged(object? sender, EventArgs e)
        {
            if (suppressCheckedChanged) return;
            if (sender is not CheckBox cb) return;
            var tileObj = board.FindByVisual(cb);
            if (tileObj == null) return;

            // Defensive: if the tile is not free, do not allow selecting it
            if (!board.IsTileFree(tileObj))
            {
                suppressCheckedChanged = true;
                try { cb.Checked = false; }
                finally { suppressCheckedChanged = false; }
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

            if (!TilesMatch(a.TileData, b.TileData))
            {
                MessageBox.Show("Tiles do not match.", "No match", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!board.IsTileFree(a) || !board.IsTileFree(b))
            {
                MessageBox.Show("One or both tiles are blocked and cannot be matched.", "Cannot match", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (board.TryMatch(a, b))
            {
                UpdateTileAvailability();
                MessageBox.Show("Tiles matched and removed.", "Matched", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Unable to match tiles (internal check failed).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void UpdateTileAvailability()
        {
            foreach (var t in board.TilesOnBoard)
            {
                if (t.Visual is Control c)
                {
                    // if visual is hidden, keep it disabled; otherwise enable only if tile is free
                    c.Enabled = c.Visible && board.IsTileFree(t);
                }
            }
        }

        private static bool TilesMatch(Tile x, Tile y)
        {
            if (x == null || y == null) return false;
            return x.IsMatching(y);
        }

        private void ClearSelectionVisuals()
        {
            suppressCheckedChanged = true;
            try
            {
                foreach (var t in selectedTiles)
                {
                    if (t.Visual is CheckBox cb) cb.Checked = false;
                }
            }
            finally
            {
                selectedTiles.Clear();
                suppressCheckedChanged = false;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}