﻿using System;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;

namespace NoiseTest
{
    static public class Drawing
    {
        static public Bitmap Draw(Map map)
        {
            Bitmap bitmap = new Bitmap(500, 500);
            System.Drawing.Color col = new System.Drawing.Color();
            byte elevation; 
            byte moisture;
            bool tree;

            Biom OCEAN = new Biom(240, 100);
            Biom BEACH = new Biom(40, 75);
            Biom GRASS = new Biom(100, 90);
            Biom DESERT = new Biom(60, 75);
            //Biom Tundra = new Biom();
            Biom SWAMP = new Biom(85, 80);
            Biom MOUNTAIN = new Biom(0, 0);
            Biom TREE = new Biom(30, 100);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    elevation = map.getElevation(x, y);
                    moisture = map.getMoisture(x, y);
                    tree = map.getTrees(x, y);

                    if (elevation <= map.getWaterlevel()) // OCEAN
                        col = ColorHSV.fromHSV(OCEAN, (byte)(100 - (map.getWaterlevel() - elevation) / 2.55));
                    else if (elevation <= map.getWaterlevel() + 15) // BEACH a little above OCEAN
                        col = ColorHSV.fromHSV(BEACH, (byte)(elevation / 2.55));
                    else if (elevation <= map.getWeedlevel())
                    {
                        if (moisture < 30)
                            col = ColorHSV.fromHSV(DESERT, (byte)(elevation / 2.55));
                        else if (moisture > 170)
                            col = ColorHSV.fromHSV(SWAMP, (byte)(elevation / 2.55));
                        else
                            col = ColorHSV.fromHSV(GRASS, (byte)(elevation / 2.55));
                        if (tree)
                        {
                            col = ColorHSV.fromHSV(TREE, 40);
                        }
                    }
                    else
                        col = ColorHSV.fromHSV(MOUNTAIN, (byte)(elevation / 2.55));

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