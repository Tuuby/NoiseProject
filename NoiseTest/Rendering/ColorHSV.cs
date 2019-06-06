using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoiseTest
{
    static class ColorHSV
    {
        static public Color fromHSV(int hue, byte saturation, byte value)
        {
            double h, s, v;
            h = hue;
            s = saturation / 100.0;
            v = value / 100.0;

            double chroma = v * s;
            double minimum = v - chroma;
            double x = chroma * (1 - Math.Abs((h / 60 % 2) - 1));
            chroma *= 255;
            minimum *= 255;
            x *= 255;

            if (h >= 0 && h < 60)
                return Color.FromArgb(255, (int)(chroma + minimum), (int)(x + minimum), (int)minimum);
            else if (h >= 60 && h < 120)
                return Color.FromArgb(255, (int)(x + minimum), (int)(chroma + minimum), (int)minimum);
            else if (h >= 120 && h < 180)
                return Color.FromArgb(255, (int)minimum, (int)(chroma + minimum), (int)(x + minimum));
            else if (h >= 180 && h < 240)
                return Color.FromArgb(255, (int)minimum, (int)(x + minimum), (int)(chroma + minimum));
            else if (h >= 240 && h < 300)
                return Color.FromArgb(255, (int)(x + minimum), (int)minimum, (int)(chroma + minimum));
            else if (h >= 300 && h < 360)
                return Color.FromArgb(255, (int)(chroma + minimum), (int)minimum, (int)(x + minimum));
            else
                return Color.FromArgb(255, (int)minimum, (int)minimum, (int)minimum);
        }
    }
}
