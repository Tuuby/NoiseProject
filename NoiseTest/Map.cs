using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseTest
{
    public class Map
    {
        private int width = 10;
        private int height = 10;
        private byte[,] elevation;
        private float scale = 0.01f;
        private byte waterlevel = 0;
        private byte weedlevel = 0;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            elevation = createArray();
            //Noise.Seed = seed;
            
        }

       /* public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            elevation = createArray();
        }*/

        public Map(int seed)
        {
            elevation = createArray();
            Noise.Seed = seed;
        }

        public Map()
        {
            elevation = createArray();
        }

        private byte[,] createArray()
        {
            return new byte[width, height];
        }

        public void setSeed(int Seed)
        {
            Noise.Seed = Seed;
        }

        public byte getElevation(int x, int y)
        {
            return elevation[x, y];
        }

        public byte[,] getAll()
        {
            return elevation;
        }

         public void setScale(float scale)
        {
            if ((scale >= 1) && (scale <= 5)) {
                this.scale = scale / 500;
            } else
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

        public void setWeedlevel(byte weedlevel)
        {
            this.weedlevel = weedlevel;
        }

        public byte getWeedlevel()
        {
            return weedlevel;
        }

        public void Generate()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    double el = (Noise.CalcPixel2D(x, y, 0.5f * scale)
                                    + 0.5 * Noise.CalcPixel2D(x, y, 2 * scale)
                                    + 0.25 * Noise.CalcPixel2D(x, y, 4 * scale)) / 1.75;    //1.75 ist wichtig um innerhalb der Grenzen eines Bytes zu bleiben
                    elevation[x, y] = (byte) (Math.Pow(el, 2) / 255);
                }
            }
        }
    }
}
