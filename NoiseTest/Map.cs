using System;
using System.Diagnostics;

namespace NoiseTest
{
    public class Map
    // Diese Klasse implementiert eine Karte mit den Ausmaßen width*height, einer Höhenkoordinate für jeden Punkt,
    // der Skalierung scale und mehreren Schwellenwerten für Wasser und Gras
    {
        private int width = 10;
        private int height = 10;
        private int elevationSeed;
        private int moistureSeed;
        private byte[,] elevation;
        private byte[,] moisture;
        private byte[,] compressedElevation;
        private bool[,] trees;
        private float scale = 0.01f;
        private byte waterlevel = 0;
        private byte weedlevel = 0;
        //private byte weedlevel = 0;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            elevation = createArray();
            moisture = createArray();
            compressedElevation = createArray();
            trees = new bool[this.width, this.height];
        }

        public Map(int seed)
        {
            elevation = createArray();
            moisture = createArray();
            compressedElevation = createArray();
            elevationSeed = seed;
            trees = new bool[width, height];
        }

        public Map()
        {
            elevation = createArray();
            moisture = createArray();
            compressedElevation = createArray();
        }

        // erstellt ein zweidimensionales Array mit den Ausmaßen width*height
        private byte[,] createArray()
        {
            return new byte[width, height];
        }

        public void setElevationSeed(int seed)
        {
            elevationSeed = seed;
        }

        public void setMoistureSeed(int seed)
        {
            moistureSeed = seed;
        }

        // gibt den Höhenwert eines einzelnen Punktes im Array zurück
        public byte getElevation(int x, int y)
        {
            return elevation[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public byte[,] getElevation()
        {
            return elevation;
        }

        // gibt den Höhenwert eines einzelnen Punktes im Array zurück

        public byte getCompressedElevation(int x, int y)
        {
            return compressedElevation[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public byte[,] getCOmpressedElevation()
        {
            return compressedElevation;
        }

        // gibt den Höhenwert eines einzelnen Punktes im Array zurück

        public byte getMoisture(int x, int y)
        {
            return moisture[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public byte[,] getMoisture()
        {
            return moisture;
        }

        public bool getTrees(int x, int y)
        {
            return trees[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public bool[,] getTrees()
        {
            return trees;
        }

        public void setScale(float scale)
        {
            if ((scale >= 1) && (scale <= 5))
            {
                this.scale = scale / 500;
            }
            else
            {
                throw new ArgumentException("Falscher Parameter du Affe");
            }
        }

        public float getScale()
        {
            return scale;
        }

        public void setWaterlevel(byte waterlevel)
        {
            this.waterlevel = waterlevel;
        }

        public byte getWaterlevel()
        {
            return waterlevel;
        }


        public double calculateDistance(int x1, int y1, int x2, int y2)
        {
            int x3 = x2 - x1;
            int y3 = y2 - y1;
            int squareX = x3 * x3;
            int squareY = y3 * y3;
            int square = squareX + squareY;
            double distance = Math.Sqrt(square);
            return distance;
        }

        public void makeIsland()
        {
            double maxDistance = calculateDistance(width / 2, height / 2, 0, 0);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double distance = calculateDistance(width / 2, height / 2, x, y) / maxDistance;
                    distance *= 255;
                    elevation[x, y] = (byte)((255 + elevation[x, y] - distance) / 2);
                }
            }
        }

        //public void setWeedlevel(byte weedlevel)
        //{
        //    this.weedlevel = weedlevel;
        //}

        public void setWeedlevel(byte weedlevel)
        {
            this.weedlevel = weedlevel;
        }

        public byte getWeedlevel()
        {
            return weedlevel;
        }

        // füllt das Array elevation mit Höhenwerten, welche durch Überlagerung mehrerer Simplex-Noises generiert werden
        public void GenerateElevation()
        {
            Noise.Seed = elevationSeed;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double el = (Noise.CalcPixel2D(x, y, 0.5f * scale)
                                    + 0.5 * Noise.CalcPixel2D(x, y, 2 * scale)
                                    + 0.25 * Noise.CalcPixel2D(x, y, 4 * scale)) / 1.75;    //1.75 ist wichtig um innerhalb der Grenzen eines Bytes zu bleiben
                    elevation[x, y] = (byte)(Math.Pow(el, 2) / 255);
                }
            }
        }

        // füllt das Array moisture mit Feuchtigkeitswerten, welche durch Überlagerung mehrerer Simplex-Noises generiert werden
        public void GenerateMoisture()
        {
            Noise.Seed = moistureSeed;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double mo = Noise.CalcPixel2D(x, y, 0.5f * scale)
                                   /* + 0.5 * Noise.CalcPixel2D(x, y, 2 * scale)
                                    + 0.25 * Noise.CalcPixel2D(x, y, 4 * scale)) / 1.75*/;    //1.75 ist wichtig um innerhalb der Grenzen eines Bytes zu bleiben
                    moisture[x, y] = (byte)mo;
                }
            }
        }

        public void compressingElevation (double factor)
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    if(elevation[x, y] < Byte.MaxValue / factor)
                        compressedElevation[x, y] = (byte) (elevation[x, y] * factor);
                }
            }
        }

        public void distributeTrees(int range)
        {
            int localMax = 0;
            int localMaxX = 0;
            int localMaxY = 0;

            for (int xBlock = 0; xBlock < width / range; xBlock++)
            {
                for (int yBlock = 0; yBlock < height / range; yBlock++)
                {
                    for (int x = 0; x < range && (xBlock + x) < width; x++)
                    {
                        for(int y = 0; y < range && (yBlock + y) < height; y++)
                        {
                            if (moisture[xBlock + x, yBlock + y] > localMax)
                            {
                                localMax = moisture[xBlock + x, yBlock + y];
                                localMaxX = xBlock + x;
                                localMaxY = yBlock + y;
                            }
                        }
                        trees[localMaxX, localMaxY] = true;
                    }
                }
            }
        //{
        //    int x = 0;
        //    int y = 0;
        //    while (!(x == width) && !(y == height))
        //    {
        //        byte localMax = 0;
        //        int localMaxX = 0;
        //        int localMaxY = 0;
        //        for (; x < x + range; x++)
        //        {
        //            for (; y < y + range; y++)
        //            {
        //                if (moisture[x,y] > localMax)
        //                {
        //                    localMax = moisture[x, y];
        //                    localMaxX = x;
        //                    localMaxY = y;
        //                }
        //            }
        //            trees[localMaxX, localMaxY] = true;
        //        }
        //    }
        }
    }
}
