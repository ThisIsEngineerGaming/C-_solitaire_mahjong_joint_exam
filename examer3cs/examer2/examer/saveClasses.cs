using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examer
{
    public class HighScoreManager
    {
        private string fileName;

        public HighScoreManager(string file = "score.txt")
        {
            fileName = file;
        }

        public int LoadHighScore()
        {
            if (!File.Exists(fileName))
                return 0;

            try
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    if (line.StartsWith("highscore:", StringComparison.OrdinalIgnoreCase))
                    {
                        string num = line.Substring(10).Trim();
                        if (int.TryParse(num, out int hs))
                            return hs;
                    }
                }
            }
            catch { }

            return 0;
        }

        public void SaveScore(int lastScore, int highScore)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(fileName, false))
                {
                    sw.WriteLine($"highscore: {highScore}");
                    sw.WriteLine("last_game:");
                    sw.WriteLine($"score: {lastScore}");
                }
            }
            catch { }
        }
    }

}
