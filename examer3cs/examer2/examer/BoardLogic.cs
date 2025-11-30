using System.Collections.Generic;
using System.Windows.Forms;

namespace examer
{
    public class BoardLogic
    {
        public List<TileObject> TilesOnBoard = new List<TileObject>();

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

            return true;
        }

        private static bool TilesMatch(Tile x, Tile y)
        {
            if (x == null || y == null) return false;
            return x.IsMatching(y);
        }

        public TileObject FindByVisual(Control visual)
        {
            foreach (TileObject t in TilesOnBoard)
                if (t.Visual == visual) return t;

            return null;
        }
    }
}