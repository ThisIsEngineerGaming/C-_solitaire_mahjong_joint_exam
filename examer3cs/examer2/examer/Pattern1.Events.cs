using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examer
{
    public partial class Pattern1
    {
        // Logic for selecting a tile(for visuals and matching)
        private void Tile_Click(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pb) return;

            var tileObj = board.FindByVisual(pb);
            if (tileObj == null) return;

            if (!board.IsTileFree(tileObj))
                return; // blocked tile

            if (!selectedTiles.Contains(tileObj))
                selectedTiles.Add(tileObj);

            // highlight selection visually
            pb.BorderStyle = BorderStyle.Fixed3D;

            if (selectedTiles.Count == 2)
            {
                HandleSelection(selectedTiles[0], selectedTiles[1]);
                ClearSelectionVisuals();
            }
        }
        // Basic tile selection and matching logic
        private void tiles_CheckedChanged(object? sender, EventArgs e)
        {
            if (suppressCheckedChanged) return;
            if (sender is not CheckBox cb) return;

            var tileObj = board.FindByVisual(cb);
            if (tileObj == null) return;

            if (!board.IsTileFree(tileObj))
            {
                suppressCheckedChanged = true;
                cb.Checked = false;
                suppressCheckedChanged = false;
                return;
            }

            if (cb.Checked)
            {
                if (!selectedTiles.Contains(tileObj))
                    selectedTiles.Add(tileObj);
            }
            else
            {
                selectedTiles.Remove(tileObj);
            }

            if (selectedTiles.Count == 2)
            {
                HandleSelection(selectedTiles[0], selectedTiles[1]);
                ClearSelectionVisuals();
            }
        }
        // Very simple, press button, go to menu, wow
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Hide();

            MainMenuForm menu = new MainMenuForm();
            menu.Show();

            this.Close();
        }
        // Does what it says, clears selection highlights
        private void ClearSelectionVisuals()
        {
            foreach (var t in selectedTiles)
                if (t.Visual is PictureBox pb)
                    pb.BorderStyle = BorderStyle.None;

            selectedTiles.Clear();
        }
    }
}