using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using AtaxiaVision.Testing;
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
    /// Interaction logic for Confirmacion.xaml
    /// </summary>
    public partial class Confirmacion : Window
    {
        public EjercicioViewModel Ejercicio { get; set; }
        public SesionViewModel Sesion { get; set; }
        public string Pregunta { get; set; }

        private void EstadoSnackBar(string mensaje)
        {
            Snackbar.IsActive = false;
            Snackbar.Message = new SnackbarMessage() { Content = mensaje };
            Snackbar.IsActive = true;
        }

        public Confirmacion(SesionViewModel sesionVM, EjercicioViewModel ejercicioVM, List<TensionServos> tensiones)
        {
            InitializeComponent();
            Sesion = sesionVM;
            Ejercicio = ejercicioVM;
            Ejercicio.Desvios = TensionHelper.CalcularDesvios(tensiones);
            var status = ServerHelper.EnviarEjercicio(Ejercicio);
            if(status == ServerHelper.SERVER_ERROR)
                ServerHelper.AgregarEjercicioDatosOffile(Ejercicio);
            if (Ejercicio.FinalizoConExito)
                PreguntaLabel.Content = "Ejercicio finalizado con éxito. ¿Desea repetir el ejercicio?";
            else
                PreguntaLabel.Content = "Ejercicio no finalizado. ¿Desea repetir el ejercicio?";
            EstadoSnackBar("Fin ejercicio " + Ejercicio.Ejercicio);
            Ejercicio.Ejercicio++;
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            Close();
        }

        private void SiBtn_Click(object sender, RoutedEventArgs e)
        {
            Principal win = new Principal(Sesion, Ejercicio);
            win.Show();
            Close();
        }
    }
}
