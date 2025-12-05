
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace examer
{
    /// <summary>
    /// Represents a collection of Tile objects with indexer, operator overloading, and save/load functionality.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Saves the object to a file.
        /// </summary>
        void Save(string filePath);

        /// <summary>
        /// Loads the object from a file.
        /// </summary>
        void Load(string filePath);
    }

    public class TileCollection : IEnumerable<Tile>, ISaveable
    {
        private readonly List<Tile> _tiles = new List<Tile>();

        /// <summary>
        /// Gets the tile at the specified index.
        /// </summary>
        public Tile this[int index]
        {
            get => _tiles[index];
            set => _tiles[index] = value;
        }

        /// <summary>
        /// Adds a tile to the collection.
        /// </summary>
        public void Add(Tile tile)
        {
            _tiles.Add(tile);
        }

        /// <summary>
        /// Operator overload to add a tile to the collection.
        /// </summary>
        public static TileCollection operator +(TileCollection collection, Tile tile)
        {
            collection.Add(tile);
            return collection;
        }

        /// <summary>
        /// Saves the collection to a file as JSON.
        /// </summary>
        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_tiles);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads the collection from a file as JSON.
        /// </summary>
        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var json = File.ReadAllText(filePath);
            var tiles = JsonSerializer.Deserialize<List<Tile>>(json);
            _tiles.Clear();
            if (tiles != null) _tiles.AddRange(tiles);
        }

        /// <summary>
        /// Returns an enumerator for the collection.
        /// </summary>
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

