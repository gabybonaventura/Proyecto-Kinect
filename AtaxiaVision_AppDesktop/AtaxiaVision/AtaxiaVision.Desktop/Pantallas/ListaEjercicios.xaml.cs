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
        public List<EjercicioViewModel> TestCollection
        {
            get
            {
                return new List<EjercicioViewModel>()
                {
                    new EjercicioViewModel(){ Nombre = "Ejer 1Lorem ipsum  ", Descripcion = "Desc 1 Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod" , Dificultad = 1, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 1", Descripcion = "Desc 1 Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod" , Dificultad = 1, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "EjLorem ipsum er 1", Descripcion = "Desc 1 Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod" , Dificultad = 1, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Lorem ipsum  1", Descripcion = "Desc 1 Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod" , Dificultad = 1, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 1", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 1" , Dificultad = 1, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 2", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 2" , Dificultad = 2, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Lorem ipsum Ejer 2", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 2" , Dificultad = 2, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 2", Descripcion = "DeLorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmodsc 2" , Dificultad = 2, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 2", Descripcion = "Desc 2" , Dificultad = 2, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "ELorem ipsum jer 2", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 2" , Dificultad = 2, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 3", Descripcion = "Desc 3" , Dificultad = 3, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Lorem ipsum Ejer 3", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 3" , Dificultad = 3, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "ELorem ipsum jer 3", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 3" , Dificultad = 3, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 3", Descripcion = "Desc 3" , Dificultad = 3, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 4", Descripcion = "Desc 4" , Dificultad = 4, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "ELorem ipsum jer 4", Descripcion = "Desc 4" , Dificultad = 4, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 4", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 4" , Dificultad = 4, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "EjLorem ipsum er 4", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 4" , Dificultad = 4, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "Ejer 5", Descripcion = "Desc 5" , Dificultad = 5, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "EjLorem ipsum er 5", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 5" , Dificultad = 5, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "EjLorem ipsum er 5", Descripcion = "Desc 5" , Dificultad = 5, EstadoInicial="*99999", EstadoFinal="*111111"},
                    new EjercicioViewModel(){ Nombre = "EjLorem ipsum er 5", Descripcion = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod 5" , Dificultad = 5, EstadoInicial="*99999", EstadoFinal="*111111"},
                };
            }
        }
        
        public ListaEjercicios()
        {
            InitializeComponent();
            ObtenerEjercicios();
            
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void EditarBtn_Click(object sender, RoutedEventArgs e)
        {
            var filaSeleccionada = ((FrameworkElement)sender).DataContext as EjercicioViewModel;
        }

        private void ObtenerEjercicios()
        {
            var ejercicios = ServerHelper.ObtenerEjercicios();
            if (ejercicios != null)
                EjerciciosDatGrid.ItemsSource = ejercicios.Ejercicios;

        }
    }
}
