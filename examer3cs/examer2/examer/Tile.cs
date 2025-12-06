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

        /// <summary>
        /// Seasons (SSN) and Flowers (FLW) match each other regardless of number.
        /// All other tiles match only if Suit AND Value match exactly.
        /// </summary>
        public bool IsMatching(Tile other)
        {
            if (other == null) return false;

            string s1 = Suit?.ToUpperInvariant();
            string s2 = other.Suit?.ToUpperInvariant();

            bool isSF1 = (s1 == "SSN" || s1 == "FLW");
            bool isSF2 = (s2 == "SSN" || s2 == "FLW");

            if (isSF1 && isSF2)
                return true;

            return s1 == s2 && Value == other.Value;
        }

        public override string ToString() => $"{Suit}{Value}";
    }
}
