namespace examer
{
    public class Tile
    {
        public string Suit { get; set; }
        public int Value { get; set; }

        public Tile() { }

        public Tile(string suit, int value)
        {
            Suit = suit;
            Value = value;
        }

        public bool IsMatching(Tile other)
        {
            if (other == null) return false;
            return string.Equals(Suit, other.Suit, System.StringComparison.OrdinalIgnoreCase)
                && Value == other.Value;
        }

        public override string ToString() => $"{Suit}:{Value}";
    }
}
