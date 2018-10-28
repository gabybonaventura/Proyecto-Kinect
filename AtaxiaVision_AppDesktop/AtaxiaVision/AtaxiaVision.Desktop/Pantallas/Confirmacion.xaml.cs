using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using AtaxiaVision.Testing;
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
        public string Pregunta { get; set; }
        public Confirmacion()
        {
            InitializeComponent();
        }

        public Confirmacion(EjercicioViewModel ejercicio, List<TensionServos> tensiones)
        {
            
            InitializeComponent();
            Ejercicio = ejercicio;
            var status = ServerHelper.EnviarEjercicio(Ejercicio);
            switch (status)
            {
                case ServerHelper.SERVER_ERROR:
                    ServerHelper.AgregarEjercicioDatosOffile(Ejercicio);
                    PreguntaLabel.Content = "Ejercicio finalizado con exito. Desea repetir el ejercicio?";
                    break;
                case ServerHelper.SERVER_OK:
                    PreguntaLabel.Content = "Ejercicio no finalizado. Desea repetir el ejercicio?";
                    break;
                default:
                    break;
            }
            Ejercicio.Ejercicio++;
        }

        public Confirmacion(bool flagToken, List<TensionServos> desvios, bool resultado, string token, int nroEj)
        {
           
        }


        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            Close();
        }
    }
}
