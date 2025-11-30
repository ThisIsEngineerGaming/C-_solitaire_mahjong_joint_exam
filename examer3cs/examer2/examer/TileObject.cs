using System.Windows.Forms;

namespace examer
{
    public class TileObject
    {
        public Tile TileData { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public Control Visual { get; set; }

        public TileObject(Tile tile, int x, int y, int z, Control visual)
        {
            TileData = tile;
            X = x;
            Y = y;
            Z = z;
            Visual = visual;
        }

        public override string ToString()
        {
            return TileData?.ToString() + " @ (" + X + "," + Y + "," + Z + ")";
        }
    }
}
