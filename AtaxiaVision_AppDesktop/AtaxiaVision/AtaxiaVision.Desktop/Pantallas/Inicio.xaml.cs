﻿using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using AtaxiaVision.Pantallas;
using AtaxiaVision.Testing;
using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AtaxiaVision.Desktop.Pantallas
{
    /// <summary>
    /// Interaction logic for Inicio.xaml
    /// </summary>
    public partial class Inicio : Window
    {
        public SesionViewModel Sesion { get; set; }
        public RepeticionViewModel Ejercicio { get; set; }
        public bool FlagFocusSesionTextBox { get; set; }

        // Delegates (son como punteros de C, sirven para que entre 
        // hilos asincronicos puedan acceder a los componentes de la vista)
        public delegate void SnackBarDelegate(string msg);
        public SnackBarDelegate snackBarDelegate;

        public delegate void ProgressBarDelegate(Visibility visibility);
        public ProgressBarDelegate progressBarDelegate;

        public delegate void IniRehabBtnDelegate(bool state);
        public IniRehabBtnDelegate iniRehabBtnDelegate;

        public delegate void PacienteCardDelegate(Visibility visibility);
        public PacienteCardDelegate pacienteCardDelegate;

        public delegate void LabelEdadDelegate(int edad);
        public LabelEdadDelegate labelEdadDelegate;

        public delegate void LabelNombreDelegate(string nombre);
        public LabelNombreDelegate labelNombreDelegate;

        public delegate void LabelIdDelegate(int id);
        public LabelIdDelegate labelIdDelegate;

        // Backgruond Worker
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        
        public Inicio()
        {
            //PruebaAngulos p = new PruebaAngulos();
            InitializeComponent();
            ServerHelper.TestInicializarArchivo();
            SincronizarDatos();
            // Test de grabacion json
            //ServerHelper.TestLeerArchivo();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarTokenBackGruondWorker();
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            progressBarDelegate = new ProgressBarDelegate(EstadoProgressBar);
            iniRehabBtnDelegate = new IniRehabBtnDelegate(EstadoIniciarRehabilitacion);
            labelEdadDelegate = new LabelEdadDelegate(Edad);
            labelNombreDelegate = new LabelNombreDelegate(Nombre);
            labelIdDelegate = new LabelIdDelegate(Id);
            pacienteCardDelegate = new PacienteCardDelegate(EstadoPacienteCard);

            Sesion = new SesionViewModel();
            Ejercicio = new RepeticionViewModel();
            DNITextBox.Focus();
        }

        private void Id(int id)
        {
            IdLabel.Content += id.ToString();
        }

        private void Nombre(string nombre)
        {
            NombreLabel.Content += nombre;
        }

        private void Edad(int edad)
        {
            EdadLabel.Content += edad.ToString();
        }

        private void EstadoPacienteCard(Visibility visibility)
        {
            PacienteCard.Visibility = visibility;
        }

        private void EstadoSnackBar(string mensaje)
        {
            Snackbar.IsActive = false;
            Snackbar.Message = new SnackbarMessage() { Content = mensaje };
            Snackbar.IsActive = true;
        }

        private void EstadoProgressBar(Visibility visibility)
        {
            ProgressBar.Visibility = visibility;
        }

        private void EstadoIniciarRehabilitacion(bool state)
        {
            IniRehabBtn.IsEnabled = state;
            IniRehabBtn.Focus();
        }

        private void SincronizarDatos()
        {
            var result = ServerHelper.SincronizarDatosOffline();
            switch (result)
            {
                case ServerHelper.ARCHIVOOFFLINE_SINCRONIZADO:
                    EstadoSnackBar("Datos sincronizados.");
                    break;
                case ServerHelper.ARCHIVOOFFLINE_NOSINCRONIZADO:
                    EstadoSnackBar("Error al sincronizar.");
                    break;
                default:
                    break;
            }
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ValidarTokenBtn_Click(object sender, RoutedEventArgs e)
        {
            ValidarToken();
        }

        private void IniRehabBtn_Click(object sender, RoutedEventArgs e)
        {
            Principal win = new Principal(Sesion, Ejercicio);
            win.Show();
            Close();
        }

        private void ValidarToken()
        {
            if (String.IsNullOrEmpty(DNITextBox.Text))
            {
                EstadoSnackBar("Por favor ingrese un DNI.");
                return;
            }
            if (String.IsNullOrEmpty(SesionTextBox.Text))
            {
                EstadoSnackBar("Por favor ingrese un número de sesión.");
                return;
            }
            Sesion.Token = DNITextBox.Text + "_" + SesionTextBox.Text;
            Ejercicio.Token = Sesion.Token;
            Ejercicio.Ejercicio = 1;
            if(!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }

        // Se agrega esta funcion para asignar el trabajo una sola vez, y despues dentro de "ValidarToken()" solo se llama a la misma.
        private void ValidarTokenBackGruondWorker()
        {
            backgroundWorker.DoWork += (s, e) =>
            {
                // Muestro la barra de cargando
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Visible);
                // Thread.Sleep(5000); // Es solo para ver como queda la animacion este sleep.
                // Valido el token DNI + Sesion
                var result = ServerHelper.ValidarToken(Sesion.Token);
                Sesion.TokenVerificado = true;
                // Muestro el Snackbar
                switch (result.CodigoTokenValid)
                {
                    case ServerHelper.TOKEN_SINCONEXION:
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "No hay conexión a internet para validar el token.");
                        Sesion.TokenValido = false;
                        break;
                    case ServerHelper.TOKEN_INVALIDO:
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "Token inválido. Vas a poder utilizar la Aplicación sin problemas.");
                        Sesion.TokenValido = false;
                        break;
                    case ServerHelper.TOKEN_VALIDO:
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "Token Válido.");
                        Sesion.TokenValido = true;
                        IdLabel.Dispatcher.Invoke(labelIdDelegate, result.Patient.PacienteId);
                        NombreLabel.Dispatcher.Invoke(labelNombreDelegate, result.Patient.Nombre);
                        EdadLabel.Dispatcher.Invoke(labelEdadDelegate, result.Patient.Edad);
                        PacienteCard.Dispatcher.Invoke(pacienteCardDelegate, Visibility.Visible);
                        break;
                    default:
                        break;
                }                
                IniRehabBtn.Dispatcher.Invoke(iniRehabBtnDelegate, true);
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Hidden);
            };
        }
        
        private void DNITextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (FlagFocusSesionTextBox && DNITextBox.GetLineLength(0) == 8)
            {
                SesionTextBox.Focus();
                FlagFocusSesionTextBox = false;
            }
        }

        private void DNITextBox_KeyDown(object sender, KeyEventArgs e)
        {
            FlagFocusSesionTextBox = true;
            if (e.Key == Key.Enter)
            {
                ValidarToken();
            }
        }

        private void SesionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ValidarToken();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }   
            if(e.Key == Key.I)
            {
                IniRehabBtn_Click(sender, e);
            }
            if (e.Key == Key.Add)
            {
                NuevoEjercicioBtn_Click(sender, e);
            }
        }

        private void NuevoEjercicioBtn_Click(object sender, RoutedEventArgs e)
        {
            //NuevoEjercicio win = new NuevoEjercicio();
            ListaEjercicios win = new ListaEjercicios();
            win.Show();
            Close();
        }
    }
}
