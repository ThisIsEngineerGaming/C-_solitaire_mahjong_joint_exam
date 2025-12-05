using System;
using System.Collections.Generic;

namespace examer
{
    public static class TileRandom
    {
        private static readonly Random rng = new Random();

        public static void ShuffleTiles(List<TileObject> tiles)
        {
            int n = tiles.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (tiles[n], tiles[k]) = (tiles[k], tiles[n]);
            }
        }
    }
}
