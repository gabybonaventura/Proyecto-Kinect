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
    /// Lógica de interacción para EliminarEjercicio.xaml
    /// </summary>
    public partial class EliminarEjercicio : Window
    {
        public Exercise Ejercicio { get; set; }
        public delegate void SnackBarDelegate(string msj);
        public SnackBarDelegate snackBarDelegate;

        public EliminarEjercicio(Exercise exercise)
        {
            InitializeComponent();
            Ejercicio = exercise;
            NombreEjercicioLabel.Content += Ejercicio.Nombre;
            DficultadEjercicioLabel.Content += Ejercicio.Dificultad + "";
            DescripcionLabel.Content += Ejercicio.Descripcion;
        }

        private void EstadoSnackBar(string mensaje)
        {
            Snackbar.IsActive = false;
            Snackbar.Message = new SnackbarMessage() { Content = mensaje };
            Snackbar.IsActive = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            NoBtn_Click(sender, e);
        }

        private void SiBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = ServerHelper.EliminarEjercicio(Ejercicio.ID);
            if (result == ServerHelper.SERVER_OK)
                NoBtn_Click(sender, e);
            else
                EstadoSnackBar("No hay conexión para guardar el ejercicio. Intente nuevamente en unos minutos.");
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new ListaEjercicios();
            win.Show();
            Close();
        }
    }
}
