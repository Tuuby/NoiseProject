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
            byte elevation;
            byte moisture;

            int seed = rdm.Next();

            map.setScale(5f);
            map.setElevationSeed(seed);
            seed = rdm.Next();
            map.setMoistureSeed(seed);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    elevation = map.getElevation(x, y);
                    moisture = map.getMoisture(x, y);
                    if (elevation <= map.getWaterlevel())
                    {
                        col = ColorHSV.fromHSV(240, 100, (byte)(100 - (map.getWaterlevel() - elevation) / 2.55));
                    }
                    else
                    {
                        col = ColorHSV.fromHSV(100, (byte)(moisture / 2.55), (byte)(elevation / 2.55));
                    }
                    //Console.Write(col.ToString() + "\n");
                    bitmap.SetPixel(x, y, col);
                }
            }

            ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return imageSource;
        }
    }
}