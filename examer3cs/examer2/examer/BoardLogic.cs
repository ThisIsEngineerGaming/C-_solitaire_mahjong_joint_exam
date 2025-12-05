using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace examer
{
    public class BoardLogic
    {
        private static BoardLogic _instance;

        public static BoardLogic Instance
        {
            get
            {
                return _instance ??= new BoardLogic();
            }
        }

        public event EventHandler<TilesMatchedEventArgs>? TilesMatched;
        public List<TileObject> TilesOnBoard = new List<TileObject>();

        public bool IsTileFree(TileObject tile)
        {
            if (tile == null) return false;

            bool hasTileOnTop = false;
            bool leftBlocked = false;
            bool rightBlocked = false;

            foreach (var t in TilesOnBoard)
            {
                if (t == tile || !t.Visual.Visible) continue;

                if (t.Z > tile.Z &&
                    t.X == tile.X &&
                    t.Y == tile.Y)
                {
                    hasTileOnTop = true;
                }

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


        public bool TryMatch(TileObject a, TileObject b)
        {
            if (a == null || b == null) return false;
            if (a == b) return false;

            if (!IsTileFree(a) || !IsTileFree(b))
                return false;

            if (!TilesMatch(a.TileData, b.TileData))
                return false;

            TilesOnBoard.Remove(a);
            TilesOnBoard.Remove(b);

            if (a.Visual != null) a.Visual.Visible = false;
            if (b.Visual != null) b.Visual.Visible = false;

            TilesMatched?.Invoke(this, new TilesMatchedEventArgs(a, b));
            return true;
        }

        private static bool TilesMatch(Tile x, Tile y)
        {
            if (x == null || y == null) return false;
            return x.IsMatching(y);
        }

        public TileObject FindByVisual(Control visual)
        {
            foreach (var t in TilesOnBoard)
                if (t.Visual == visual)
                    return t;

            return null;
        }
    }

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