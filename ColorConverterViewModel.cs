using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;

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
        public Model3DGroup ConeModel { get; private set; }
        public ColorConverterViewModel()
        {
            _color = new ColorModel();
            _color.PropertyChanged += ColorModel_PropertyChanged;
            GradientMaterial = CreateGradientMaterialForRactangle();
            ConeModel = CreateCone(1.5, 0.5, 20);
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

        public DiffuseMaterial CreateGradientMaterialForRactangle()
        {
            // Your algorithm to create a System.Drawing.Bitmap
            var bitmap = Algorithm.RGBGradient();

         /*   Bitmap hsvBitmap = Algorithm.HSVGradient((x, y) =>
            {
                // Map x and y to the ranges you need for H, S, and V
                double hue = x / 256.0 * 360; // Example: Map x to [0, 360] for Hue
                double saturation = y / 256.0; // Example: Map y to [0, 1] for Saturation
                double value = 1.0; // Value is constant at 1 for full brightness

                return Algorithm.ColorFromHSV(hue, saturation, value);
            });*/


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

        public DiffuseMaterial CreateGradientMaterialForCone()
        {
            // Your algorithm to create an HSV gradient as a System.Drawing.Bitmap
            // Define the HSV callbacks
            ColorCallback hueCallback = (x, y) => (x % 360, 100, 100); // Example: Hue varies with x
            ColorCallback saturationCallback = (x, y) => (100, 100, 100); // Example: Full saturation
            ColorCallback valueCallback = (x, y) => (100, 100, 100); // Example: Full value

            // Use the defined callbacks to create the HSV gradient bitmap
            var hsvBitmap = Algorithm.HsvGradient(hueCallback, saturationCallback, valueCallback);

            // Convert the HSV System.Drawing.Bitmap to a BitmapSource
            var bitmapSource = ConvertBitmapToBitmapSource(hsvBitmap);

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

        public Model3DGroup CreateCone(double height, double radius, int divisions)
        {
            Model3DGroup group = new Model3DGroup();
            Point3DCollection points = new Point3DCollection();
            Int32Collection triangleIndices = new Int32Collection();

  

            // Dodaj punkty bazowe
            points.Add(new Point3D(0, -height / 2, 0)); // Środek dolnej podstawy
            for (int i = 0; i < divisions; i++)
            {
                double angle = i * 2.0 * Math.PI / divisions;
                double x = radius * Math.Cos(angle);
                double z = radius * Math.Sin(angle);
                points.Add(new Point3D(x, -height / 2, z));
            }

            // Dodaj punkt wierzchołkowy stożka
            points.Add(new Point3D(0, height / 2, 0));

            // Tworzenie trójkątów dla podstawy
            for (int i = 1; i <= divisions; i++)
            {
                int next = (i % divisions) + 1;
                triangleIndices.Add(0);
                triangleIndices.Add(next);
                triangleIndices.Add(i);
            }

            // Tworzenie trójkątów dla boków stożka
            for (int i = 1; i <= divisions; i++)
            {
                int next = (i % divisions) + 1;
                triangleIndices.Add(points.Count - 1);
                triangleIndices.Add(i);
                triangleIndices.Add(next);
            }

            // Tworzenie geometrii i modelu
            MeshGeometry3D mesh = new MeshGeometry3D()
            {
                Positions = points,
                TriangleIndices = triangleIndices
            };


            PointCollection textureCoordinates = new PointCollection();

            // Bottom cap texture coordinates (circular pattern)
            textureCoordinates.Add(new Point(0.5, 0.5)); // Center of the bottom cap
            for (int i = 0; i < divisions; i++)
            {
                double angle = i * 2.0 * Math.PI / divisions;
                double u = 0.5 + 0.5 * Math.Cos(angle);
                double v = 0.5 + 0.5 * Math.Sin(angle);
                textureCoordinates.Add(new Point(u, v));
            }

            // Top point texture coordinates (all mapped to top texture point)
            textureCoordinates.Add(new Point(0.5, 0)); // Top of the texture

            // Assign the texture coordinates to the mesh
            mesh.TextureCoordinates = textureCoordinates;

            GeometryModel3D model = new GeometryModel3D(mesh, CreateGradientMaterialForCone());
            group.Children.Add(model);

            return group;
        }


    }




}
