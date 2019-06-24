using System.Drawing;

namespace NoiseTest
{
    static public class Drawing
    {
        static public Bitmap Draw(Map map)
        {
            Bitmap bitmap = new Bitmap(500, 500);
            Color col = new Color();
            byte elevation; 
            byte moisture;
            bool tree;

            Biom OCEAN = new Biom(240, 100);
            Biom BEACH = new Biom(44, 51);
            Biom GRASS = new Biom(100, 90);
            Biom DESERT = new Biom(50, 75);
            Biom TUNDRA = new Biom(0, 0);
            Biom SWAMP = new Biom(125, 32);
            Biom MOUNTAIN = new Biom(0, 0);
            Biom TREE = new Biom(140, 80);
            Biom STEPPE = new Biom(40, 80);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    elevation = map.GetElevation(x, y);
                    moisture = map.GetMoisture(x, y);
                    tree = map.GetTrees(x, y);

                    if (elevation <= map.GetWaterlevel()) // OCEAN
                        col = ColorHSV.fromHSV(OCEAN.hue, OCEAN.saturation, (byte)(100 - (map.GetWaterlevel() - elevation) / 2.55));
                    else if (elevation <= map.GetWaterlevel() + 10) // BEACH a little above OCEAN
                        col = ColorHSV.fromHSV(BEACH.hue, BEACH.saturation, (byte)(50 + (elevation / 5.1)));
                    else if (elevation <= 130) // <- normal land level
                    {
                        if (moisture < 70)
                            col = ColorHSV.fromHSV(DESERT.hue, DESERT.saturation, (byte)((elevation / 5.1) + 50));
                        else if (moisture > 170)
                            col = ColorHSV.fromHSV(SWAMP.hue, SWAMP.saturation, (byte)(elevation / 2.55));
                        else
                            col = ColorHSV.fromHSV(GRASS.hue, GRASS.saturation, (byte)(elevation / 2.55));
                        if (tree && moisture >= 70)
                        {
                            col = ColorHSV.fromHSV(TREE.hue, TREE.saturation, 40);
                        }
                    }
                    else if(elevation <= 190)
                    {
                        if(moisture < 80)
                            col = ColorHSV.fromHSV(STEPPE.hue, STEPPE.saturation, (byte)(elevation / 2.2));
                        else if (moisture > 170)
                            col = ColorHSV.fromHSV(SWAMP.hue, SWAMP.saturation, (byte)(elevation / 2.55));
                        else
                            col = ColorHSV.fromHSV(GRASS.hue, GRASS.saturation, (byte)(elevation / 2.55));
                        if (tree)
                        {
                            col = ColorHSV.fromHSV(TREE.hue, TREE.saturation, 40);
                        }
                    }
                    else
                    {
                        if (moisture >= 127)
                            col = ColorHSV.fromHSV(TUNDRA.hue, TUNDRA.saturation, (byte)(elevation / 2.55));
                        else
                            col = ColorHSV.fromHSV(MOUNTAIN.hue, MOUNTAIN.saturation, (byte)(elevation / 3.0));
                    }

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