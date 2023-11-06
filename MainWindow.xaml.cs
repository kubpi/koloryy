using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kolory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DiffuseMaterial GradientMaterial { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            // Inicjowanie ViewModeli

            // Set the DataContext for the window to your ViewModel
            var viewModel = new ColorConverterViewModel();
            DataContext = viewModel;

            // Assign the gradient material to the cube
            GradientMaterial = viewModel.GradientMaterial;

        }

        
    }
}
