using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace examer
{
    public partial class Pattern1
    {
        // The bulk of the game logic. Checks if they can be matched, tries to perform it, updates tile states, checks for win/loss
        private void HandleSelection(TileObject a, TileObject b)
        {
            if (a == null || b == null) return;

            if (!a.TileData.IsMatching(b.TileData)) return;
            if (!board.IsTileFree(a) || !board.IsTileFree(b)) return;

            if (board.TryMatch(a, b))
            {
                score += 100;
                UpdateScoreLabel();
                UpdateTileAvailability();
                CheckForWin();
                CheckForLoss();
            }
        }
        // Updates the enabled/disabled state of tiles based on whether they are free or blocked
        private void UpdateTileAvailability()
        {
            foreach (var t in board.TilesOnBoard)
            {
                if (t.Visual is PictureBox pb)
                {
                    bool free = board.IsTileFree(t);

                    pb.Enabled = pb.Visible && free;

                    if (free)
                    {
                        UpdateTileImage(t);
                    }
                    else
                    {
                        if (pb.Image != null)
                            pb.Image = MakeBlockedImage(pb.Image);
                    }
                }
            }
        }
        // If there are no tiles, then epic win
        private void CheckForWin()
        {
            if (board.TilesOnBoard.Count == 0)
            {
                DialogResult result = MessageBox.Show(
                    "Congrats, you matched everything.\nPress OK to go back to the menu...",
                    "You Win!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                if (result == DialogResult.OK)
                {
                    this.Hide();
                    SaveScore(score);
                    MainMenuForm menu = new MainMenuForm();
                    menu.Show();
                    this.Close();
                }
            }
        }
        // if there are no matching pairs left BUT there are still tiles remaining, then you lose(its really difficult to lose though)
        private void CheckForLoss()
        {
            if (!HasAnyAvailableMoves() && board.TilesOnBoard.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "You cant match anything else now.\nClick OK to go back to the main menu.",
                    "Game Over",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.OK)
                {
                    this.Hide();
                    SaveScore(score);
                    MainMenuForm menu = new MainMenuForm();
                    menu.Show();
                    this.Close();
                }
            }
        }
        // This is what actually checks for available moves for the loss condition
        private bool HasAnyAvailableMoves()
        {
            var freeTiles = board.TilesOnBoard
                .Where(t => board.IsTileFree(t))
                .ToList();

            for (int i = 0; i < freeTiles.Count; i++)
            {
                for (int j = i + 1; j < freeTiles.Count; j++)
                {
                    if (freeTiles[i].TileData.IsMatching(freeTiles[j].TileData))
                        return true;
                }
            }

            return false;
        }
        // Updates the score.
        private void UpdateScoreLabel()
        {
            scoreLabel.Text = $"Score: {score}   Highscore: {highScore}";
        }
        // saves the score if last one is lower than current
        private void SaveScore(int lastScore)
        {
            if (lastScore > highScore)
                highScore = lastScore;

            scoreManager.SaveScore(lastScore, highScore);
        }
    }
}