using System;
using System.ComponentModel;

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
        private double _c;
        private double _m;
        private double _y;
        private double _k;

        private bool _updating = false;

        public byte R
        {
            get { return _r; }
            set
            {
                if (_r != value)
                {
                    _r = value;
                    OnPropertyChanged(nameof(R));
                    ConvertToHSV();
                    ConvertToCMYK();
                    OnPropertyChanged(nameof(H));
                    OnPropertyChanged(nameof(S));
                    OnPropertyChanged(nameof(V));
                    OnPropertyChanged(nameof(C));
                    OnPropertyChanged(nameof(M));
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(K));
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
                    ConvertToHSV();
                    ConvertToCMYK();
                    OnPropertyChanged(nameof(H));
                    OnPropertyChanged(nameof(S));
                    OnPropertyChanged(nameof(V));
                    OnPropertyChanged(nameof(C));
                    OnPropertyChanged(nameof(M));
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(K));
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
                    ConvertToHSV();
                    ConvertToCMYK();
                    OnPropertyChanged(nameof(H));
                    OnPropertyChanged(nameof(S));
                    OnPropertyChanged(nameof(V));
                    OnPropertyChanged(nameof(C));
                    OnPropertyChanged(nameof(M));
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(K));
                }
            }
        }
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
                    OnPropertyChanged(nameof(R));
                    OnPropertyChanged(nameof(G));
                    OnPropertyChanged(nameof(B));
                    OnPropertyChanged(nameof(C));
                    OnPropertyChanged(nameof(M));
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(K));
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
                    OnPropertyChanged(nameof(R));
                    OnPropertyChanged(nameof(G));
                    OnPropertyChanged(nameof(B));
                    OnPropertyChanged(nameof(C));
                    OnPropertyChanged(nameof(M));
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(K));
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
                    OnPropertyChanged(nameof(R));
                    OnPropertyChanged(nameof(G));
                    OnPropertyChanged(nameof(B));
                    OnPropertyChanged(nameof(C));
                    OnPropertyChanged(nameof(M));
                    OnPropertyChanged(nameof(Y));
                    OnPropertyChanged(nameof(K));
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
                ConvertToHSV();
                OnPropertyChanged(nameof(R));
                OnPropertyChanged(nameof(G));
                OnPropertyChanged(nameof(B));
                OnPropertyChanged(nameof(H));
                OnPropertyChanged(nameof(S));
                OnPropertyChanged(nameof(V));
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
                ConvertToHSV();            
                OnPropertyChanged(nameof(R));
                OnPropertyChanged(nameof(G));
                OnPropertyChanged(nameof(B));
                OnPropertyChanged(nameof(H));
                OnPropertyChanged(nameof(S));
                OnPropertyChanged(nameof(V));
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
                ConvertToHSV();
                OnPropertyChanged(nameof(R));
                OnPropertyChanged(nameof(G));
                OnPropertyChanged(nameof(B));
                OnPropertyChanged(nameof(H));
                OnPropertyChanged(nameof(S));
                OnPropertyChanged(nameof(V));
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
                ConvertToHSV();
                OnPropertyChanged(nameof(R));
                OnPropertyChanged(nameof(G));
                OnPropertyChanged(nameof(B));
                OnPropertyChanged(nameof(H));
                OnPropertyChanged(nameof(S));
                OnPropertyChanged(nameof(V));
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

            _h = (float)h;
            _s = (float)s;
            _v = (float)v / 255;
            OnPropertyChanged(nameof(H));
            OnPropertyChanged(nameof(S));
            OnPropertyChanged(nameof(V));
        }

        public void ConvertToCMYK()
        {
            if (R == 0 && G == 0 && B == 0)
            {
                _c = 0;
                _m = 0;
                _y = 0;
                _k = 1;
                return;
            }

            double c = 1 - (_r / 255.0);
            double m = 1 - (_g / 255.0);
            double y = 1 - (_b / 255.0);
            double k = Math.Min(c, Math.Min(m, y));

            _c = (c - k) / (1 - k);
            _m = (m - k) / (1 - k);
            _y = (y - k) / (1 - k);
            _k = k;

            // Powiadamiaj o zmianach
            OnPropertyChanged(nameof(C));
            OnPropertyChanged(nameof(M));
            OnPropertyChanged(nameof(Y));
            OnPropertyChanged(nameof(K));
        }
        public void ConvertHSVToRGB()
        {
            if (S == 0)
            {
                _r = _g = _b = (byte)(V * 255);
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
                    _r = (byte)(V * 255);
                    _g = (byte)(t * 255);
                    _b = (byte)(p * 255);
                    break;
                case 1:
                    _r = (byte)(q * 255);
                    _g = (byte)(V * 255);
                    _b = (byte)(p * 255);
                    break;
                case 2:
                    _r = (byte)(p * 255);
                    _g = (byte)(V * 255);
                    B = (byte)(t * 255);
                    break;
                case 3:
                    _r = (byte)(p * 255);
                    _g = (byte)(q * 255);
                    _b = (byte)(V * 255);
                    break;
                case 4:
                    _r = (byte)(t * 255);
                    _g = (byte)(p * 255);
                    _b = (byte)(V * 255);
                    break;
                default:
                    _r = (byte)(V * 255);
                    _g = (byte)(p * 255);
                    _b = (byte)(q * 255);
                    break;
            }

            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(G));
            OnPropertyChanged(nameof(B));
            ConvertToCMYK();

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

            // Zaktualizuj prywatne pola bezpośrednio
            _r = tempR;
            _g = tempG;
            _b = tempB;

            // Powiadamiaj o zmianach
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(G));
            OnPropertyChanged(nameof(B));
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
                _r = 0;
                _g = 0;
                _b = 0;
                return;
            }


            // Zaktualizuj prywatne pola bezpośrednio
            _r = Convert.ToByte(255 * (1 - _c) * (1 - _k));
            _g = Convert.ToByte(255 * (1 - _m) * (1 - _k));
            _b = Convert.ToByte(255 * (1 - _y) * (1 - _k));

            // Powiadamiaj o zmianach
            OnPropertyChanged(nameof(R));
            OnPropertyChanged(nameof(G));
            OnPropertyChanged(nameof(B));
            ConvertToHSV();
        }


    }


}
