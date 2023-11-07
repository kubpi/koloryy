using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace kolory
{
    public delegate int RGBColorCallback(int x, int y);
    public delegate (int h, int s, int v) ColorCallback(int x, int y);
    public static class Algorithm
    {
        public static Bitmap RGBGradient()
        {
            (RGBColorCallback r, RGBColorCallback g, RGBColorCallback b) = _values[_RGBindex++ % _values.Length];
            return RGBGradient(r, g, b);
        }

        public unsafe static Bitmap RGBGradient(
                RGBColorCallback red,
                RGBColorCallback green,
                RGBColorCallback blue
            )
        {
            var bmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb
            );

            byte* bytes = (byte*)data.Scan0.ToPointer();

            int len = data.Stride * data.Height;

            for (int y = 0; y < data.Height; y++)
            {
                int o = y * data.Stride;

                for (int x = 0; x < data.Width; x++)
                {
                    bytes[o + x * 3 + 0] = (byte)red(x, y);
                    bytes[o + x * 3 + 1] = (byte)green(x, y);
                    bytes[o + x * 3 + 2] = (byte)blue(x, y);
                }
            }

            bmp.UnlockBits(data);
            return bmp;
        }

        private static int _RGBindex = 0;
        private static readonly (RGBColorCallback r, RGBColorCallback g, RGBColorCallback b)[] _values = new (RGBColorCallback r, RGBColorCallback g, RGBColorCallback b)[]
        {
        ((x, y) => x, (x, y) => y, (x, y) => 0),
        ((x, y) => 0, (x, y) => x, (x, y) => y),
        ((x, y) => y, (x, y) => 0, (x, y) => x),
        ((x, y) => y, (x, y) => x, (x, y) => 0),
        ((x, y) => x, (x, y) => 0, (x, y) => y),
        ((x, y) => 0, (x, y) => y, (x, y) => x),
        };


        public static Bitmap HsvGradient()
        {
            (ColorCallback h, ColorCallback s, ColorCallback v) = _hsvValues[_HSVindex++ % _hsvValues.Length];
            return HsvGradient(h, s, v);
        }

        public unsafe static Bitmap HsvGradient(
                ColorCallback hue,
                ColorCallback saturation,
                ColorCallback value
            )
        {
            var bmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);
            var data = bmp.LockBits(
                new Rectangle(Point.Empty, bmp.Size),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb
            );

            byte* bytes = (byte*)data.Scan0.ToPointer();

            for (int y = 0; y < data.Height; y++)
            {
                int o = y * data.Stride;

                for (int x = 0; x < data.Width; x++)
                {
                    (int h, int s, int v) = hue(x, y);
                    Color rgb = HsvToRgb(h, s, v);
                    bytes[o + x * 3 + 0] = rgb.R;
                    bytes[o + x * 3 + 1] = rgb.G;
                    bytes[o + x * 3 + 2] = rgb.B;
                }
            }

            bmp.UnlockBits(data);
            return bmp;
        }

        private static int _HSVindex = 0;
        private static readonly (ColorCallback h, ColorCallback s, ColorCallback v)[] _hsvValues = new (ColorCallback h, ColorCallback s, ColorCallback v)[]
        {
        // You'll need to define your own HSV callbacks here
        // Here's an example:
        ((x, y) => (x % 360, 100, 100), // Hue varies with x
         (x, y) => (100, 100, 100),      // Full saturation
         (x, y) => (100, 100, 100))      // Full value
                                         // Add more as per your requirement
        };

        private static Color HsvToRgb(int h, int s, int v)
        {
            // Convert HSV to RGB here
            // This method should be implemented to convert an HSV color to an RGB color
            // Here's a simple example:
            double hue = h / 360.0;
            double saturation = s / 100.0;
            double value = v / 100.0;

            int i = (int)Math.Floor(hue * 6);
            double f = hue * 6 - i;
            double p = value * (1 - saturation);
            double q = value * (1 - f * saturation);
            double t = value * (1 - (1 - f) * saturation);

            double r = 0, g = 0, b = 0;
            switch (i % 6)
            {
                case 0: r = value; g = t; b = p; break;
                case 1: r = q; g = value; b = p; break;
                case 2: r = p; g = value; b = t; break;
                case 3: r = p; g = q; b = value; break;
                case 4: r = t; g = p; b = value; break;
                case 5: r = value; g = p; b = q; break;
            }

            return Color.FromArgb(
                (int)(r * 255),
                (int)(g * 255),
                (int)(b * 255)
            );
        }

    }
}
