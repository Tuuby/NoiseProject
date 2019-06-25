using System;

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
        private bool[,] trees;
        private float scale = 0.01f;
        private byte waterlevel = 0;
        private byte weedlevel = 0;
        private int elevationDifferenz;
        private int moistureDifferenz;
        //private byte weedlevel = 0;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            elevation = CreateArray();
            moisture = CreateArray();
            trees = new bool[this.width, this.height];
        }

        public Map(int seed)
        {
            elevation = CreateArray();
            moisture = CreateArray();
            elevationSeed = seed;
            trees = new bool[width, height];
        }

        public Map()
        {
            elevation = CreateArray();
            moisture = CreateArray();
        }

        // erstellt ein zweidimensionales Array mit den Ausmaßen width*height
        private byte[,] CreateArray()
        {
            return new byte[width, height];
        }

        public void SetElevationSeed(int seed)
        {
            elevationSeed = seed;
        }

        public void SetMoistureSeed(int seed)
        {
            moistureSeed = seed;
        }

        // gibt den Höhenwert eines einzelnen Punktes im Array zurück
        public byte GetElevation(int x, int y)
        {
            return elevation[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public byte[,] GetElevation()
        {
            return elevation;
        }

        // gibt den Höhenwert eines einzelnen Punktes im Array zurück

        public byte GetMoisture(int x, int y)
        {
            return moisture[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public byte[,] GetMoisture()
        {
            return moisture;
        }

        public bool GetTrees(int x, int y)
        {
            return trees[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public bool[,] GetTrees()
        {
            return trees;
        }

        public void SetScale(float scale)
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

        public float GetScale()
        {
            return scale;
        }

        public void SetWaterlevel(byte waterlevel)
        {
            this.waterlevel = waterlevel;
        }

        public byte GetWaterlevel()
        {
            return waterlevel;
        }


        public double CalculateDistance(int x1, int y1, int x2, int y2)
        {
            int x3 = x2 - x1;
            int y3 = y2 - y1;
            int squareX = x3 * x3;
            int squareY = y3 * y3;
            int square = squareX + squareY;
            double distance = Math.Sqrt(square);
            return distance;
        }

        public void MakeIsland()
        {
            double maxDistance = CalculateDistance(width / 2, height / 2, 0, 0);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double distance = CalculateDistance(width / 2, height / 2, x, y) / maxDistance;
                    distance *= 255;
                    elevation[x, y] = (byte)((255 + elevation[x, y] - distance) / 2);
                }
            }
        }

        //public void setWeedlevel(byte weedlevel)
        //{
        //    this.weedlevel = weedlevel;
        //}

        public void SetWeedlevel(byte weedlevel)
        {
            this.weedlevel = weedlevel;
        }

        public byte GetWeedlevel()
        {
            return weedlevel;
        }

        public void SetElevationDifferenz(int value)
        {
            elevationDifferenz = value;
        }

        public int GetElevationDifferenz()
        {
            return elevationDifferenz;
        }

        public void SetMoistureDifferent(int value)
        {
            moistureDifferenz = value;
        }

        public int GetMoistureDifferent()
        {
            return moistureDifferenz;
        }

        // füllt das Array elevation mit Höhenwerten, welche durch Überlagerung mehrerer Simplex-Noises generiert werden
        public void GenerateElevation() // default value, when no paramter 
        {
            int multiplyFactor = GetElevationDifferenz();
            Noise.Seed = elevationSeed;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double el = (Noise.CalcPixel2D(x, y, 0.5f * scale)
                                    + 0.5 * Noise.CalcPixel2D(x, y, 2 * scale)
                                    + 0.25 * Noise.CalcPixel2D(x, y, 4 * scale)) / 1.75;    //1.75 ist wichtig um innerhalb der Grenzen eines Bytes zu bleiben
                    elevation[x, y] = (byte)(Math.Pow(el, 2) / 255);
                    if (GetElevationDifferenz() < 0)
                    {
                        // elevation + (-50) >= Byte.Min
                        // elevation >= Byte.Min - (-50)
                        if (elevation[x, y] >= Byte.MinValue - elevationDifferenz)
                            elevation[x, y] = (byte)(GetElevationDifferenz() + elevation[x, y]);
                        else
                            elevation[x, y] = Byte.MinValue;
                    }
                    else if (GetElevationDifferenz() > 0) {
                        if (elevation[x, y] <= Byte.MaxValue - GetElevationDifferenz())
                            elevation[x, y] = (byte)(GetElevationDifferenz() + elevation[x, y]);
                        else
                            elevation[x, y] = Byte.MaxValue;
                    }
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
                    double mo = (Noise.CalcPixel2D(x, y, 0.5f * scale)
                                    + 0.5 * Noise.CalcPixel2D(x, y, 2 * scale)
                                    + 0.25 * Noise.CalcPixel2D(x, y, 4 * scale)) / 1.75;    //1.75 ist wichtig um innerhalb der Grenzen eines Bytes zu bleiben
                                    //+ 0.43 * Noise.CalcPixel2D(x, y, 4 * scale)) / 1.75;
                    moisture[x, y] = (byte)mo;
                    if (moistureDifferenz < 0)
                    {
                        if (moisture[x, y] >= Byte.MinValue - moistureDifferenz)
                            moisture[x, y] = (byte)(moistureDifferenz + moisture[x, y]);
                        else
                            moisture[x, y] = Byte.MinValue;
                    }
                    else if (moistureDifferenz > 0)
                    {
                        if (moisture[x, y] <= Byte.MaxValue - moistureDifferenz)
                            moisture[x, y] = (byte)(moistureDifferenz + moisture[x, y]);
                        else
                            moisture[x, y] = Byte.MaxValue;
                    }
                }
            }
        }

        public void DistributeTrees()
        {
            trees = new bool[width, height];
            Random rnd = new Random();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    byte m = moisture[x, y];
                    int rdm = rnd.Next(255);
                    if (rdm < m / 8 * scale)
                    {
                        trees[x, y] = true;
                    }
                } 
            }
        }
    }
}
