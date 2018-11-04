using AtaxiaVision.Desktop.Pantallas;
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
using System.Windows.Shapes;

namespace AtaxiaVision.Pantallas
{
    /// <summary>
    /// Interaction logic for NuevoEjercicio.xaml
    /// </summary>
    public partial class NuevoEjercicio : Window
    {
        public List<int> AngulosDisponibles {
            get
            {
                return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180 };
            }
        }

        private void AsignarListasAngulos()
        {
            AnguloHombroArribaAbajoComboBox.ItemsSource = AngulosDisponibles;
            AnguloHombroAdelanteAtrasComboBox.ItemsSource = AngulosDisponibles;
            AnguloCodoArribaAbajoComboBox.ItemsSource = AngulosDisponibles;
            AnguloCodoDerechaIzquierdaComboBox.ItemsSource = AngulosDisponibles;
        }

        public NuevoEjercicio()
        {
            InitializeComponent();
            AsignarListasAngulos();
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CerrarBtn_Click(sender,e);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NombreEjercicioTextBox.Focus();
        }
    }
}
