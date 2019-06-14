using System;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;
using NoiseTest.Rendering;

// Klasse nur mit Funktionen ? Ansosnten: Objekt von Klasse Drawing insatnziieren in MainWindow und dann Methode aufrufen

namespace NoiseTest
{
    static public class Drawing
    {
        static public Bitmap Draw(Map map)
        {
            Bitmap bitmap = new Bitmap(500, 500);
            System.Drawing.Color col = new System.Drawing.Color();
            byte elevation; // <- the compressed Elevation 
            byte moisture;


            Biom OCEAN = new Biom(240, 100);
            Biom BEACH = new Biom(40, 75);
            Biom GRASS = new Biom(100, 80);
            Biom DESERT = new Biom(60, 75);
            //Biom Tundra = new Biom();
            //Biom SWAMP = new Biom();


            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    elevation = map.getCompressedElevation(x, y);
                    moisture = map.getMoisture(x, y);
                    if (elevation <= map.getWaterlevel()) // OCEAN
                    {
                        col = ColorHSV.fromHSV(OCEAN.hue, OCEAN.saturation, (byte)(100 - (map.getWaterlevel() - elevation) / 2.55));
                    }
                    else if (elevation <= map.getWaterlevel() + 15) // BEACH a little above OCEAN
                    {
                        col = ColorHSV.fromHSV(BEACH.hue, BEACH.saturation, (byte)(elevation / 2.55));
                    }
                    else
                    {
                        if (moisture >= 200)
                            col = ColorHSV.fromHSV(DESERT.hue, DESERT.saturation, (byte)(elevation / 2.55));
                        else
                            col = ColorHSV.fromHSV(GRASS.hue, GRASS.saturation, (byte)(elevation / 2.55));
                    }
                    //Console.Write(col.ToString() + "\n");
                    bitmap.SetPixel(x, y, col);
                }
            }

            //ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            //TaskCompletionSource<ImageSource> tcs = new TaskCompletionSource<ImageSource>();
            //tcs.SetResult(imageSource);
            return bitmap;
        }
    }
}