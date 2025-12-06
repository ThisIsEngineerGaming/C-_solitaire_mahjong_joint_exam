
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace examer
{
    public interface ISaveable
    {
        void Save(string filePath);

        void Load(string filePath);
    }

    public class TileCollection : IEnumerable<Tile>, ISaveable
    {
        private readonly List<Tile> _tiles = new List<Tile>();

        public Tile this[int index]
        {
            get => _tiles[index];
            set => _tiles[index] = value;
        }

        public void Add(Tile tile)
        {
            _tiles.Add(tile);
        }

        public static TileCollection operator +(TileCollection collection, Tile tile)
        {
            collection.Add(tile);
            return collection;
        }

        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_tiles);
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var json = File.ReadAllText(filePath);
            var tiles = JsonSerializer.Deserialize<List<Tile>>(json);
            _tiles.Clear();
            if (tiles != null) _tiles.AddRange(tiles);
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

