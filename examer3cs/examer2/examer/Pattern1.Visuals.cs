using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace examer
{
    public partial class Pattern1
    {
        // puts the images on tiles
        private void UpdateTileImage(TileObject tile)
        {
            if (tile.Visual is not PictureBox pb) return;

            string filename = GetFileName(tile.TileData);
            string imgPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "tileAssets", filename);

            if (!File.Exists(imgPath))
            {
                pb.Image = null;
                return;
            }

            using (var temp = Image.FromFile(imgPath))
            {
                pb.Image = new Bitmap(temp);
            }

            if (pb.Image != null)
                pb.Image.Tag = null;
        }
        // Stuff for making the blocked tile images grayed out and a bit transparent
        private Image MakeBlockedImage(Image original)
        {
            if (original == null)
                return null;

            if (original.Tag as string == "blocked")
                return original;

            float opacity = 0.7f;
            float grayL = 0.33f;

            Bitmap output = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(output))
            {
                var colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                    new float[][]
                    {
                new float[] { grayL, grayL, grayL, 0, 0 },
                new float[] { grayL, grayL, grayL, 0, 0 },
                new float[] { grayL, grayL, grayL, 0, 0 },

                new float[] { 0, 0, 0, opacity, 0 },

                new float[] { 0, 0, 0, 0, 1 }
                    });

                var attributes = new System.Drawing.Imaging.ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                g.DrawImage(
                    original,
                    new Rectangle(0, 0, output.Width, output.Height),
                    0, 0, original.Width, original.Height,
                    GraphicsUnit.Pixel,
                    attributes
                );
            }
            // What allows to do the check 4 it
            output.Tag = "blocked";

            return output;
        }
        // asset name getter for tiles
        private string GetFileName(Tile tile)
        {
            return tile.Suit switch
            {
                "B" => $"B{tile.Value}.png",
                "CR" => $"CR{tile.Value}.png",
                "CHR" => $"CHR{tile.Value}.png",

                "WND" => tile.Value switch
                {
                    1 => "WNDS.png",
                    2 => "WNDN.png",
                    3 => "WNDE.png",
                    4 => "WNDW.png",
                    _ => "WND?.png"
                },

                "DR" => tile.Value switch
                {
                    1 => "DRR.png",
                    2 => "DRG.png",
                    3 => "DRW.png",
                    _ => "DR?.png"
                },

                "SSN" => $"SSN{tile.Value}.png",
                "FLW" => $"FLW{tile.Value}.png",
                _ => $"{tile.Suit}{tile.Value}.png"
            };
        }
    }
}