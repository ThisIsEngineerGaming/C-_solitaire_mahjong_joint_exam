using System.Collections;
using System.Collections.Generic;

namespace examer
{
    public class TileCollection : IEnumerable<Tile>
    {
        private readonly List<Tile> _tiles = new List<Tile>();

        public void Add(Tile tile)
        {
            _tiles.Add(tile);
        }

        public IEnumerator<Tile> GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
