using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace examer
{
    public partial class Pattern1 : Form
    {
        private BoardLogic board;
        private List<TileObject> selectedTiles = new List<TileObject>();
        private bool suppressCheckedChanged;
        private static readonly Random rng = new Random();

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

            // ------------------------------
            // ORIGINAL PATTERN (unchanged)
            // ------------------------------

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

            // IMPORTANT: NO LOCATION SHUFFLE (keeps pattern positions unchanged)

            // Assign solvable random names in pairs
            AssignRandomNamesInPairsSolvable(board.TilesOnBoard);

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

        private void HandleSelection(TileObject a, TileObject b)
        {
            if (a == null || b == null) return;

            if (!a.TileData.IsMatching(b.TileData)) return;
            if (!board.IsTileFree(a) || !board.IsTileFree(b)) return;

            if (board.TryMatch(a, b))
            {
                UpdateTileAvailability();
            }
        }

        private void UpdateTileAvailability()
        {
            foreach (var t in board.TilesOnBoard)
                if (t.Visual is Control c)
                    c.Enabled = c.Visible && board.IsTileFree(t);
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

        // ------------------------------------------------
        // SOLVABLE PAIR ASSIGNER
        // - Assigns names in pairs
        // - Simulates removals so assignment is guaranteed solvable (except possibly one leftover if tile count is odd)
        // ------------------------------------------------

        private void AssignRandomNamesInPairsSolvable(List<TileObject> tiles)
        {
            if (tiles == null || tiles.Count == 0) return;

            int count = tiles.Count;
            int toAssignPairs = count / 2; // we will assign pairs for floor(count/2) pairs; if odd one tile may be left unchanged

            // We'll attempt multiple times if random choices cause a dead-end
            const int maxAttempts = 500;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // simulation state
                bool[] present = Enumerable.Repeat(true, count).ToArray();    // which tiles are still present (in simulation)
                (string suit, int val)[] assigned = new (string, int)[count]; // assigned pair name per tile, default nulls as ("",0)
                bool[] isAssigned = new bool[count];

                bool failure = false;

                int assignedPairs = 0;

                while (assignedPairs < toAssignPairs)
                {
                    // Collect indices of tiles that are present and unassigned and are free in the simulated board
                    var freeIndices = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        if (!present[i] || isAssigned[i]) continue;
                        if (IsSimulatedFree(i, tiles, present))
                            freeIndices.Add(i);
                    }

                    if (freeIndices.Count < 2)
                    {
                        // dead end for this attempt — restart
                        failure = true;
                        break;
                    }

                    // choose two distinct free indices at random
                    int idxA = freeIndices[rng.Next(freeIndices.Count)];
                    int idxB;
                    do
                    {
                        idxB = freeIndices[rng.Next(freeIndices.Count)];
                    } while (idxB == idxA);

                    var tileType = GenerateRandomTile();

                    // assign them
                    assigned[idxA] = tileType;
                    assigned[idxB] = tileType;
                    isAssigned[idxA] = isAssigned[idxB] = true;

                    // simulate removing them (so other tiles can become free)
                    present[idxA] = false;
                    present[idxB] = false;

                    assignedPairs++;
                }

                if (!failure)
                {
                    // success — apply assignments to actual tiles and update texts
                    // For any tile not assigned (only possible if odd count) keep its original TileData but update text shortname
                    for (int i = 0; i < count; i++)
                    {
                        if (isAssigned[i])
                        {
                            tiles[i].TileData.Suit = assigned[i].suit;
                            tiles[i].TileData.Value = assigned[i].val;
                        }
                        // update visual text in short format
                        if (tiles[i].Visual is CheckBox cb)
                            cb.Text = ShortName(tiles[i].TileData.Suit, tiles[i].TileData.Value);
                    }

                    return; // done
                }

                // else loop to try another random attempt
            }

            // If we reach here, attempts failed — fallback to safe simple pair assignment (non-guaranteed)
            // (This is unlikely; fallback kept so the game still runs)
            AssignRandomNamesInPairsSimple(tiles);
        }

        /// <summary>
        /// Simple random pairs (fallback). Assigns pairs for floor(count/2) pairs starting at the beginning; does NOT simulate removals.
        /// </summary>
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
        }

        /// <summary>
        /// Returns true if tile at index idx is free under the *simulated* present[] state.
        /// The logic mirrors BoardLogic.IsTileFree but uses 'present' mask instead of Visual.Visible.
        /// </summary>
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

        private (string suit, int value) GenerateRandomTile()
        {
            string[] suits = { "BMB", "CRC", "CH", "WND", "DRG", "SSN", "FLW" };
            string suit = suits[rng.Next(suits.Length)];
            int value = 1;

            switch (suit)
            {
                case "BMB":
                case "CRC":
                case "CH":
                    value = rng.Next(1, 10); // 1..9
                    break;
                case "WND":
                    value = rng.Next(1, 5);  // 1..4 => mapped to directions
                    break;
                case "DRG":
                    value = rng.Next(1, 4);  // 1..3
                    break;
                case "SSN":
                case "FLW":
                    value = rng.Next(1, 5);  // 1..4
                    break;
            }

            return (suit, value);
        }

        /// <summary>
        /// Short readable text format for tiles requested:
        /// B1..B9, CR1..CR9, CHR1..CHR9, WNDNORTH / WNDSOUTH / WNDEAST / WNDWEST,
        /// DRR / DRG / DRW, SSN1..SSN4, FLW1..FLW4
        /// </summary>
        private string ShortName(string suit, int value)
        {
            if (string.IsNullOrEmpty(suit)) return "?";

            suit = suit.ToUpperInvariant();
            return suit switch
            {
                "BMB" => $"B{value}",
                "CRC" => $"CR{value}",
                "CH" => $"CHR{value}",
                "WND" => value switch
                {
                    1 => "WNDSOUTH",
                    2 => "WNDNORTH",
                    3 => "WNDEAST",
                    4 => "WNDWEST",
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
    }
}
