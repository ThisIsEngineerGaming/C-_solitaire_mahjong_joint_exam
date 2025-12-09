using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examer
{
    public partial class Pattern1
    {
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
        // shuffles the tiles around
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
