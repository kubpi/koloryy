using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kolory
{
    public class ColorModel: INotifyPropertyChanged
    {
     
        private byte _r;
        private byte _g;
        private byte _b;


        private float _h;
        private float _s;
        private float _v;

        public float H {
            get { return _h; }
            set
            {
                if (_h != value)
                {
                    _h = value;
                    OnPropertyChanged(nameof(H));
                    ConvertHSVToRGB();
                    ConvertHSVToCMYK();
                }
            }
        }

        public float S
        {
            get { return _s; }
            set
            {
                if (_s != value)
                {
                    _s = value;
                    OnPropertyChanged(nameof(S));
                    ConvertHSVToRGB();
                    ConvertHSVToCMYK();
                }
            }
        }

        public float V
        {
            get { return _v; }
            set
            {
                if (_v != value)
                {
                    _v = value;
                    OnPropertyChanged(nameof(V));
                    ConvertHSVToRGB();
                    ConvertHSVToCMYK();
                }
            }
        }





        private double _c;
        private double _m;
        private double _y;
        private double _k;


        public byte R
        {
            get { return _r; }
            set
            {
                if (_r != value)
                {
                    _r = value;
                    OnPropertyChanged(nameof(R));

                }
            }
        }

        public byte G
        {
            get { return _g; }
            set
            {
                if (_g != value)
                {
                    _g = value;
                    OnPropertyChanged(nameof(G));
                }
            }
        }

        public byte B
        {
            get { return _b; }
            set
            {
                if (_b != value)
                {
                    _b = value;
                    OnPropertyChanged(nameof(B));
                }
            }
        }

        public double C
        {
            get => _c;
            set
            {
                _c = value;
                OnPropertyChanged(nameof(C));
                ConvertCMYKToRGB();
            }
        }

        public double M
        {
            get => _m;
            set
            {
                _m = value;
                OnPropertyChanged(nameof(M));
                ConvertCMYKToRGB();
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
                ConvertCMYKToRGB();
            }
        }

        public double K
        {
            get => _k;
            set
            {
                _k = value;
                OnPropertyChanged(nameof(K));
                ConvertCMYKToRGB();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ConvertToHSV()
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(R, G), B);
            v = Math.Max(Math.Max(R, G), B);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (R == v)
                    h = (G - B) / delta;
                else if (G == v)
                    h = 2 + (B - R) / delta;
                else if (B == v)
                    h = 4 + (R - G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            H = (float)h;
            S = (float)s;
            V = (float)v / 255;
        }

        public void ConvertToCMYK()
        {
            if (R == 0 && G == 0 && B == 0)
            {
                C = 0;
                M = 0;
                Y = 0;
                K = 1;
                return;
            }

            C = 1 - (R / 255.0);
            M = 1 - (G / 255.0);
            Y = 1 - (B / 255.0);

            K = Math.Min(C, Math.Min(M, Y));

            C = (C - K) / (1 - K);
            M = (M - K) / (1 - K);
            Y = (Y - K) / (1 - K);
        }

        // Można teraz wywołać te metody po ustawieniu wartości RGB w modelu.

        public void ConvertHSVToRGB()
        {
            if (S == 0)
            {
                R = G = B = (byte)(V * 255);
                return;
            }

            float sector = H / 60;
            int i = (int)Math.Floor(sector);
            float f = sector - i;
            float p = V * (1 - S);
            float q = V * (1 - S * f);
            float t = V * (1 - S * (1 - f));

            switch (i)
            {
                case 0:
                    R = (byte)(V * 255);
                    G = (byte)(t * 255);
                    B = (byte)(p * 255);
                    break;
                case 1:
                    R = (byte)(q * 255);
                    G = (byte)(V * 255);
                    B = (byte)(p * 255);
                    break;
                case 2:
                    R = (byte)(p * 255);
                    G = (byte)(V * 255);
                    B = (byte)(t * 255);
                    break;
                case 3:
                    R = (byte)(p * 255);
                    G = (byte)(q * 255);
                    B = (byte)(V * 255);
                    break;
                case 4:
                    R = (byte)(t * 255);
                    G = (byte)(p * 255);
                    B = (byte)(V * 255);
                    break;
                default:
                    R = (byte)(V * 255);
                    G = (byte)(p * 255);
                    B = (byte)(q * 255);
                    break;
            }
        }
        public void ConvertHSVToCMYK()
        {
            // Najpierw przekształcamy HSV na tymczasowe wartości RGB
            (byte tempR, byte tempG, byte tempB) = HSVToRGB();

            // Następnie konwertujemy tymczasowe wartości RGB na CMYK
            RGBToCMYK(tempR, tempG, tempB);
        }

        private (byte, byte, byte) HSVToRGB()
        {
            byte tempR, tempG, tempB;

            if (S == 0)
            {
                tempR = tempG = tempB = (byte)(V * 255);
                return (tempR, tempG, tempB);
            }

            float sector = H / 60;
            int i = (int)Math.Floor(sector);
            float f = sector - i;
            float p = V * (1 - S);
            float q = V * (1 - S * f);
            float t = V * (1 - S * (1 - f));

            switch (i)
            {
                case 0:
                    tempR = (byte)(V * 255);
                    tempG = (byte)(t * 255);
                    tempB = (byte)(p * 255);
                    break;
                case 1:
                    tempR = (byte)(q * 255);
                    tempG = (byte)(V * 255);
                    tempB = (byte)(p * 255);
                    break;
                case 2:
                    tempR = (byte)(p * 255);
                    tempG = (byte)(V * 255);
                    tempB = (byte)(t * 255);
                    break;
                case 3:
                    tempR = (byte)(p * 255);
                    tempG = (byte)(q * 255);
                    tempB = (byte)(V * 255);
                    break;
                case 4:
                    tempR = (byte)(t * 255);
                    tempG = (byte)(p * 255);
                    tempB = (byte)(V * 255);
                    break;
                default:
                    tempR = (byte)(V * 255);
                    tempG = (byte)(p * 255);
                    tempB = (byte)(q * 255);
                    break;
            }

            return (tempR, tempG, tempB);
        }


        private void RGBToCMYK(byte r, byte g, byte b)
        {
            if (r == 0 && g == 0 && b == 0)
            {
                C = 0;
                M = 0;
                Y = 0;
                K = 1;
                return;
            }

            C = 1 - (r / 255.0);
            M = 1 - (g / 255.0);
            Y = 1 - (b / 255.0);

            K = Math.Min(C, Math.Min(M, Y));

            C = (C - K) / (1 - K);
            M = (M - K) / (1 - K);
            Y = (Y - K) / (1 - K);
        }

        public void ConvertCMYKToRGB()
        {
            if (K == 1)
            {
                R = 0;
                G = 0;
                B = 0;
                return;
            }

            R = Convert.ToByte(255 * (1 - C) * (1 - K));
            G = Convert.ToByte(255 * (1 - M) * (1 - K));
            B = Convert.ToByte(255 * (1 - Y) * (1 - K));
        }


    }


}
