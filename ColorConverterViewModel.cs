using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Brush = System.Windows.Media.Brush;

namespace kolory
{
    public class ColorConverterViewModel : INotifyPropertyChanged
    {
        private ColorModel _color;
        private ICommand _convertCommand;
        private DiffuseMaterial _gradientMaterial;
        public DiffuseMaterial GradientMaterial
        {
            get { return _gradientMaterial; }
            private set
            {
                _gradientMaterial = value;
                OnPropertyChanged(nameof(GradientMaterial));
            }
        }

        public ColorModel Color
        {
            get { return _color ??= new ColorModel(); }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                    OnPropertyChanged(nameof(ColorDisplay));
                    OnPropertyChanged(nameof(ColorDisplayHex));
                }
            }
        }

        public ColorConverterViewModel()
        {
            _color = new ColorModel();
            _color.PropertyChanged += ColorModel_PropertyChanged;
            GradientMaterial = CreateGradientMaterial();
        }

        private void ColorModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // This will handle any property change within ColorModel
            OnPropertyChanged(nameof(ColorDisplay));
            OnPropertyChanged(nameof(ColorDisplayHex));
        }
        public string ColorDisplayHex => $"#{Color.R:X2}{Color.G:X2}{Color.B:X2}";

        public Brush ColorDisplay => new SolidColorBrush(System.Windows.Media.Color.FromRgb(Color.R, Color.G, Color.B));




        // Implementacja interfejsu INotifyPropertyChanged...
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DiffuseMaterial CreateGradientMaterial()
        {
            // Your algorithm to create a System.Drawing.Bitmap
            var bitmap = Algorithm.Gradient();

            // Convert the System.Drawing.Bitmap to a BitmapSource
            var bitmapSource = ConvertBitmapToBitmapSource(bitmap);

            // Create a brush with the BitmapSource
            var brush = new ImageBrush(bitmapSource)
            {
                ViewportUnits = BrushMappingMode.Absolute,
                TileMode = TileMode.Tile,
                Stretch = Stretch.Fill
            };

            // Create and return the material with the brush
            return new DiffuseMaterial(brush);
        }

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bitmap)
        {
            using (var memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    }




}
