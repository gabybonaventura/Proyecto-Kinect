using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
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

        public EliminarEjercicio(Exercise exercise)
        {
            InitializeComponent();
            Ejercicio = exercise;
            NombreEjercicioLabel.Content += Ejercicio.Nombre;
            DficultadEjercicioLabel.Content += Ejercicio.Dificultad + "";
            DescripcionLabel.Content += Ejercicio.Descripcion;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SiBtn_Click(object sender, RoutedEventArgs e)
        {
            ServerHelper.EliminarEjercicio(Ejercicio.ID);
            var win = new ListaEjercicios();
            win.Show();
            Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new ListaEjercicios();
            win.Show();
            Close();
        }
    }
}
