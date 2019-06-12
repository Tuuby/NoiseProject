using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private float scale = 0.01f;
        private byte waterlevel = 0;
        //private byte weedlevel = 0;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            elevation = createArray();
            moisture = createArray();
        }

        public Map(int seed)
        {
            elevation = createArray();
            moisture = createArray();
            elevationSeed = seed;
        }

        public Map()
        {
            elevation = createArray();
            moisture = createArray();
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
        public byte getMoisture(int x, int y)
        {
            return moisture[x, y];
        }

        // gibt das gesamte Array elevation zurück
        public byte[,] getMoisture()
        {
            return moisture;
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

        //public void setWeedlevel(byte weedlevel)
        //{
        //    this.weedlevel = weedlevel;
        //}

        //public byte getWeedlevel()
        //{
        //    return weedlevel;
        //}

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
    }
}
