using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public delegate void ProgressBarDelegate(Visibility visibility);
        public ProgressBarDelegate progressBarDelegate;
        public delegate void EjerciciosDatGridDelegate(List<Exercise> exercises);
        public EjerciciosDatGridDelegate ejerciciosDatGridDelegate;
        public delegate void SnackBarDelegate(string msj);
        public SnackBarDelegate snackBarDelegate;

        private BackgroundWorker ejerciciosBG = new BackgroundWorker();


        private void EstadoProgressBar(Visibility visibility)
        {
            ProgressBar.Visibility = visibility;
        }

        private void LlenarTabla(List<Exercise> exercises)
        {
            EjerciciosDatGrid.ItemsSource = exercises;
        }
        private void EstadoSnackBar(string mensaje)
        {
            Snackbar.IsActive = false;
            Snackbar.Message = new SnackbarMessage() { Content = mensaje };
            Snackbar.IsActive = true;
        }

        public void EjercicioTabla()
        {
            ejerciciosBG.DoWork += (s, e) =>
            {
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Visible);
                var ejercicios = ServerHelper.ObtenerEjercicios();
                if (ejercicios != null)
                    EjerciciosDatGrid.Dispatcher.Invoke(ejerciciosDatGridDelegate, ejercicios.Ejercicios);
                else
                    EjerciciosDatGrid.Dispatcher.Invoke(snackBarDelegate, "No hay conexión a internet para obtener los ejercicios.");
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Hidden);
            };
        }

        public ListaEjercicios()
        {
            InitializeComponent();
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new Inicio();
            win.Show();
            Close();
        }

        private void EditarBtn_Click(object sender, RoutedEventArgs e)
        {
            var filaSeleccionada = ((FrameworkElement)sender).DataContext as Exercise;
            var win = new NuevoEjercicio(filaSeleccionada);
            win.Show();
            Close();
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
            var filaSeleccionada = ((FrameworkElement)sender).DataContext as Exercise;
            var win = new EliminarEjercicio(filaSeleccionada);
            win.Show();
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EjercicioTabla();
            progressBarDelegate = new ProgressBarDelegate(EstadoProgressBar);
            ejerciciosDatGridDelegate = new EjerciciosDatGridDelegate(LlenarTabla);
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            if (!ejerciciosBG.IsBusy)
                ejerciciosBG.RunWorkerAsync();
        }
    }
}
