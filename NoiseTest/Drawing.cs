using System;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;

// Klasse nur mit Funktionen ? Ansosnten: Objekt von Klasse Drawing insatnziieren in MainWindow und dann Methode aufrufen

namespace NoiseTest
{
    static public class Drawing
    {
        static public async Task<ImageSource> Draw(Map map)
        {
            Random rdm = new Random();
            Bitmap bitmap = new Bitmap(500, 500);
            System.Drawing.Color col = new System.Drawing.Color();

            int seed = rdm.Next();

            map.setScale(5f);
            map.setSeed(seed);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    byte elevation = map.getElevation(x, y);
                    if (map.getElevation(x, y) <= map.getWaterlevel()) //blau
                    {
                        col = System.Drawing.Color.FromArgb(255, 0, 0, 100);
                        //col = System.Drawing.Color.
                    }
                    else if (map.getElevation(x, y) <= map.getWeedlevel()) //grün
                    {
                        col = System.Drawing.Color.FromArgb(255, 0, elevation, 0);
                    }
                    else //Graustufe (ungefärbt)
                    {
                        col = System.Drawing.Color.FromArgb(255, (byte)(32 + elevation / 1.14), (byte)(32 + elevation / 1.14), (byte)(32 + elevation / 1.14));
                    }
                    bitmap.SetPixel(x, y, col);
                }
            }

            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return imageSource;
        }
    }
}