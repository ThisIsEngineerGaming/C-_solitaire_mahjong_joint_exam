using System;
using System.Collections.Generic;
// The actual logic for matching tiles
namespace examer
{
    public class TileComparers
    {
        public class SuitComparer : IComparer<Tile>
        {
            public int Compare(Tile x, Tile y)
            {
                if (x == null || y == null) throw new ArgumentNullException();
                int suitCompare = string.Compare(x.Suit, y.Suit, StringComparison.OrdinalIgnoreCase);
                if (suitCompare != 0) return suitCompare;
                return x.Value.CompareTo(y.Value);
            }
        }

        public class ValueComparer : IComparer<Tile>
        {
            public int Compare(Tile x, Tile y)
            {
                if (x == null || y == null) throw new ArgumentNullException();
                return x.Value.CompareTo(y.Value);
            }
        }
    }
}
