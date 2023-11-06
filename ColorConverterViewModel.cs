using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace kolory
{
    public class ColorConverterViewModel : INotifyPropertyChanged
    {
        private ColorModel _color;
        private ICommand _convertCommand;

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




    }




}
