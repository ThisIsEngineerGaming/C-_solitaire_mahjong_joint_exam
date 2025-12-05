
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace examer
{
    /// <summary>
    /// Provides logic for managing the board and tile matching.
    /// Implements Singleton pattern and exposes events for tile matches.
    /// </summary>
    public class BoardLogic
    {
        private static BoardLogic? _instance;
        private static readonly object _lock = new();

        /// <summary>
        /// Gets the singleton instance of BoardLogic.
        /// </summary>
        public static BoardLogic Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new BoardLogic();
                }
            }
        }

        /// <summary>
        /// Event raised when tiles are matched and removed.
        /// </summary>
        public event EventHandler<TilesMatchedEventArgs>? TilesMatched;

        /// <summary>
        /// List of tiles currently on the board.
        /// </summary>
        public List<TileObject> TilesOnBoard = new List<TileObject>();

        /// <summary>
        /// Determines if a tile is free (not blocked).
        /// </summary>
        public bool IsTileFree(TileObject tile)
        {
            if (tile == null) return false;

            foreach (TileObject t in TilesOnBoard)
            {
                if (t == tile) continue;
                if (t.Visual is Control vc && !vc.Visible) continue;
                // stricter blocking: any visible tile above that overlaps (within +/-1 on X/Y) blocks
                if (t.Z > tile.Z && System.Math.Abs(t.X - tile.X) <= 1 && System.Math.Abs(t.Y - tile.Y) <= 1)
                    return false;
            }

            bool left = false;
            bool right = false;

            foreach (TileObject t in TilesOnBoard)
            {
                if (t == tile) continue;
                if (t.Visual is Control vc && !vc.Visible) continue;
                if (t.Z == tile.Z && t.Y == tile.Y)
                {
                    if (t.X == tile.X - 1) left = true;
                    if (t.X == tile.X + 1) right = true;
                }
            }

            if (left && right) return false;
            return true;
        }

        /// <summary>
        /// Attempts to match two tiles and remove them if successful.
        /// Raises TilesMatched event if successful.
        /// </summary>
        public bool TryMatch(TileObject a, TileObject b)
        {
            if (a == null || b == null) return false;
            if (a == b) return false;
            if (!IsTileFree(a) || !IsTileFree(b)) return false;
            if (!TilesMatch(a.TileData, b.TileData)) return false;

            TilesOnBoard.Remove(a);
            TilesOnBoard.Remove(b);

            if (a.Visual != null) a.Visual.Visible = false;
            if (b.Visual != null) b.Visual.Visible = false;

            TilesMatched?.Invoke(this, new TilesMatchedEventArgs(a, b));

            return true;
        }

        /// <summary>
        /// Checks if two tiles match.
        /// </summary>
        private static bool TilesMatch(Tile x, Tile y)
        {
            if (x == null || y == null) return false;
            return x.IsMatching(y);
        }

        /// <summary>
        /// Finds a TileObject by its visual control.
        /// </summary>
        public TileObject FindByVisual(Control visual)
        {
            foreach (TileObject t in TilesOnBoard)
                if (t.Visual == visual) return t;

            return null;
        }
    }

    /// <summary>
    /// Event arguments for matched tiles.
    /// </summary>
    public class TilesMatchedEventArgs : EventArgs
    {
        public TileObject TileA { get; }
        public TileObject TileB { get; }

        public TilesMatchedEventArgs(TileObject a, TileObject b)
        {
            TileA = a;
            TileB = b;
        }
    }
}

