using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using MaterialDesignThemes.Wpf;
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
    /// Lógica de interacción para ListaEjercicios.xaml
    /// </summary>
    public partial class ListaEjercicios : Window
    {
        public ListaEjercicios()
        {
            InitializeComponent();
            ObtenerEjercicios();
            
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new Inicio();
            win.Show();
            Close();
        }

        private void EditarBtn_Click(object sender, RoutedEventArgs e)
        {
            var filaSeleccionada = ((FrameworkElement)sender).DataContext as ExerciseID;
            var win = new NuevoEjercicio(filaSeleccionada);
            win.Show();
            Close();
        }

        private void ObtenerEjercicios()
        {
            var ejercicios = ServerHelper.ObtenerEjercicios();
            if (ejercicios != null)
                EjerciciosDatGrid.ItemsSource = ejercicios.Ejercicios;

        }

        private void CrearEjercicioBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new NuevoEjercicio();
            win.Show();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CerrarBtn_Click(sender, e);
            }
        }

        private void EliminarBtn_Click(object sender, RoutedEventArgs e)
        {
            var filaSeleccionada = ((FrameworkElement)sender).DataContext as ExerciseID;
            var win = new EliminarEjercicio(filaSeleccionada);
            win.Show();
            Close();
        }
    }
}
