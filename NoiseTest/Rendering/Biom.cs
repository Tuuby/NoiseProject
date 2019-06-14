using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseTest.Rendering
{
    public struct Biom
    {
        public int hue;
        public byte saturation;

        public Biom(int hue, byte saturation)
        {
            this.hue = hue;
            this.saturation = saturation;
        }
    }
}
