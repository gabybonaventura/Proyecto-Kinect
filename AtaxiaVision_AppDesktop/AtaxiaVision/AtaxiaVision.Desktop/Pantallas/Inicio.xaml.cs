using AtaxiaVision.Helpers;
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
        public bool EjercicioPersonalizado { get; set; }
        public RespuestaToken RespuestaToken { get; set; }

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

        public delegate void NombreEjercicioDelegate(string nombre);
        public NombreEjercicioDelegate nombreEjercicioDelegate;

        public delegate void DificultadEjercicioDelegate(int n);
        public DificultadEjercicioDelegate dificultadEjercicioDelegate;

        public delegate void RepeticionesEjercicioDelegate(int n);
        public RepeticionesEjercicioDelegate repeticionesEjercicioDelegate;

        public delegate void IconoTokenDelegate(Visibility visibility);
        public IconoTokenDelegate iconoTokenDelegate;

        public delegate void DNIPacienteDelegate(Visibility visibility);
        public DNIPacienteDelegate dNIPacienteDelegate;

        public delegate void SesionPacienteDelegate(Visibility visibility);
        public SesionPacienteDelegate sesionPacienteDelegate;

        public delegate void ValidarTokenButtonDelegate(Visibility visibility);
        public ValidarTokenButtonDelegate tokenButtonDelegate;

        public delegate void RatingBarDelegate(Visibility visibility);
        public RatingBarDelegate ratingBarDelegate;

        public delegate void EstadoGeneralDelegate(string msj);
        public EstadoGeneralDelegate estadoGeneralDelegate;

        public delegate void AtrasBtnDelegate(Visibility visibility);
        public AtrasBtnDelegate atrasBtnDelegate;

        public delegate void SinConexionCardDelegate(Visibility visibility);
        public SinConexionCardDelegate sinConexionCardDelegate;

        public delegate void SinConexionTextBlockDelegate(Visibility visibility);
        public SinConexionTextBlockDelegate sinConexionTextBlockDelegate;

        public delegate void TokenInvalidoTextBlockDelegate(Visibility visibility);
        public TokenInvalidoTextBlockDelegate tokenInvalidoTextBlockDelegate;

        // Backgruond Worker
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        
        public Inicio()
        {
            //PruebaAngulos p = new PruebaAngulos();
            InitializeComponent();
            //ServerHelper.TestInicializarArchivo();
            SincronizarDatos();
            // Test de grabacion json
            //ServerHelper.TestLeerArchivo();
        }
        public Inicio(string mensaje)
        {
            InitializeComponent();
            EstadoSnackBar(mensaje);
            SincronizarDatos();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ValidarTokenBackGruondWorker();
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            progressBarDelegate = new ProgressBarDelegate(EstadoProgressBar);
            iniRehabBtnDelegate = new IniRehabBtnDelegate(EstadoIniciarRehabilitacion);
            labelNombreDelegate = new LabelNombreDelegate(LlenarNombrePaciente);
            labelIdDelegate = new LabelIdDelegate(LlenarId);
            pacienteCardDelegate = new PacienteCardDelegate(EstadoPacienteCard);
            nombreEjercicioDelegate = new NombreEjercicioDelegate(LlenarNombreEjercicio);
            dificultadEjercicioDelegate = new DificultadEjercicioDelegate(LlenarDificultadEjercicio);
            repeticionesEjercicioDelegate = new RepeticionesEjercicioDelegate(LlenarRepeticiones);
            iconoTokenDelegate = new IconoTokenDelegate(EstadoIconoToken);
            dNIPacienteDelegate = new DNIPacienteDelegate(EstadoDNIPaciente);
            sesionPacienteDelegate = new SesionPacienteDelegate(EstadoSesionId);
            tokenButtonDelegate = new ValidarTokenButtonDelegate(EstadoValidarTokenBtn);
            ratingBarDelegate = new RatingBarDelegate(EstadoRatingBar);
            estadoGeneralDelegate = new EstadoGeneralDelegate(EstadoGeneral);
            atrasBtnDelegate = new AtrasBtnDelegate(EstadoAtrasBtn);
            sinConexionCardDelegate = new SinConexionCardDelegate(EstadoSinConexionCard);
            sinConexionTextBlockDelegate = new SinConexionTextBlockDelegate(EstadoSinConexiónTextBlock);
            tokenInvalidoTextBlockDelegate = new TokenInvalidoTextBlockDelegate(EstadoTokenInvalidoTextBlock);


            Sesion = new SesionViewModel();
            Ejercicio = new RepeticionViewModel();
            DNITextBox.Focus();
        }

        private void EstadoGeneral(string msj)
        {
            EstadoVentanaLabel.Text = msj;
        }

        private void LlenarId(int id)
        {
            IdLabel.Content = "DNI: " + id.ToString();
        }

        private void LlenarNombrePaciente(string nombre)
        {
            NombreLabel.Content = "Nombre: " + nombre;
        }

        private void LlenarNombreEjercicio(string nombre)
        {
            NombreEjercicioLabel.Content = "Nombre: " + nombre;
        }

        private void LlenarDificultadEjercicio(int n)
        {
            DificultadRatingBar.Value = n;
        }

        private void LlenarRepeticiones(int n)
        {
            RepeticionesEjercicioLabel.Content = "Repeticiones: " + n;
        }
        
        private void EstadoPacienteCard(Visibility visibility)
        {
            PacienteCard.Visibility = visibility;
        }

        private void EstadoSinConexionCard(Visibility visibility)
        {
            SinConexionCard.Visibility = visibility;
        }

        public void EstadoTokenInvalidoTextBlock(Visibility visibility)
        {
            TokenInvalidoTextBlock.Visibility = visibility;
            SegundoTokenInvalidoTextBlock.Visibility = visibility;
            if(visibility == Visibility.Visible)
            {
                SinConexiónTextBlock.Visibility = Visibility.Hidden;
                SegundoSinConexionTextBlock.Visibility = Visibility.Hidden;
                IniRehabBtn.Visibility = Visibility.Hidden;
            }
        }

        public void EstadoSinConexiónTextBlock(Visibility visibility)
        {
            SinConexiónTextBlock.Visibility = visibility;
            SegundoSinConexionTextBlock.Visibility = visibility;
            IniRehabBtn.Visibility = visibility;
            if (visibility == Visibility.Visible)
            {
                TokenInvalidoTextBlock.Visibility = Visibility.Hidden; 
                SegundoTokenInvalidoTextBlock.Visibility = Visibility.Hidden;
            }
        }

        private void EstadoAtrasBtn(Visibility visibility)
        {
            AtrasBtn.Visibility = visibility;
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

        private void EstadoIconoToken(Visibility visibility)
        {
            IconoValidarToken.Visibility = visibility;
        }

        private void EstadoDNIPaciente(Visibility visibility)
        {
            DNITextBox.Visibility = visibility;
        }

        private void EstadoSesionId(Visibility visibility)
        {
            SesionTextBox.Visibility = visibility;
        }

        private void EstadoValidarTokenBtn(Visibility visibility)
        {
            ValidarTokenBtn.Visibility = visibility;
        }

        private void EstadoRatingBar(Visibility visibility)
        {
            DificultadRatingBar.Visibility = visibility;
        }

        private void EstadoIniciarRehabilitacion(bool state)
        {
            if (state)
            {
                IniRehabBtn.Visibility = Visibility.Visible;
                IniRehabBtn.IsEnabled = state;
                IniRehabBtn.Focus();
            }
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
            if (EjercicioPersonalizado)
            {
                var ej = new SesionSoloBrazo(RespuestaToken, Sesion, Ejercicio);
                ej.Show();
            }
            else
            {
                Principal win = new Principal(RespuestaToken, Sesion, Ejercicio);
                win.Show();
            }
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

            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }

        // Se agrega esta funcion para asignar el trabajo una sola vez, y despues dentro de "ValidarToken()" solo se llama a la misma.
        private void ValidarTokenBackGruondWorker()
        {
            backgroundWorker.DoWork += (s, e) =>
            {
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Visible);
                //IconoValidarToken.Dispatcher.Invoke(iconoTokenDelegate,Visibility.Hidden);
                //DNITextBox.Dispatcher.Invoke(dNIPacienteDelegate,Visibility.Hidden);
                //SesionTextBox.Dispatcher.Invoke(sesionPacienteDelegate,Visibility.Hidden);
                //ValidarTokenBtn.Dispatcher.Invoke(tokenButtonDelegate,Visibility.Hidden);
                // Thread.Sleep(5000); // Es solo para ver como queda la animacion este sleep.
                // Valido el token DNI + Sesion
                RespuestaToken = ServerHelper.ValidarToken(Sesion.Token);
                var result = RespuestaToken;
                Sesion.TokenVerificado = true;
                // Muestro el Snackbar
                switch (result.CodigoTokenValid)
                {
                    case ServerHelper.TOKEN_SINCONEXION:
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "No hay conexión a internet para validar el token.");
                        EstadoVentanaLabel.Dispatcher.Invoke(estadoGeneralDelegate, "Sin conexión");
                        DificultadRatingBar.Dispatcher.Invoke(ratingBarDelegate, Visibility.Hidden);
                        SinConexiónTextBlock.Dispatcher.Invoke(sinConexionTextBlockDelegate, Visibility.Visible);
                        SinConexionCard.Dispatcher.Invoke(sinConexionCardDelegate, Visibility.Visible);
                        Sesion.TokenValido = false;
                        break;
                    case ServerHelper.TOKEN_INVALIDO:
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "Token inválido. Revisa los datos.");
                        DificultadRatingBar.Dispatcher.Invoke(ratingBarDelegate, Visibility.Hidden);
                        TokenInvalidoTextBlock.Dispatcher.Invoke(tokenInvalidoTextBlockDelegate, Visibility.Visible);
                        SinConexionCard.Dispatcher.Invoke(sinConexionCardDelegate, Visibility.Visible);
                        EstadoVentanaLabel.Dispatcher.Invoke(estadoGeneralDelegate, "Token rechazado");
                        Sesion.TokenValido = false;
                        break;
                    case ServerHelper.TOKEN_VALIDO:
                        if (result.Ejercicio.Nombre != "Reach")
                            EjercicioPersonalizado = true;
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "Token Válido.");
                        EstadoVentanaLabel.Dispatcher.Invoke(estadoGeneralDelegate, "Token válido");
                        Sesion.TokenValido = true;
                        IdLabel.Dispatcher.Invoke(labelIdDelegate, result.Paciente.PacienteId);
                        NombreLabel.Dispatcher.Invoke(labelNombreDelegate, result.Paciente.Nombre);
                        PacienteCard.Dispatcher.Invoke(pacienteCardDelegate, Visibility.Visible);
                        SinConexionCard.Dispatcher.Invoke(sinConexionCardDelegate, Visibility.Hidden);
                        NombreEjercicioLabel.Dispatcher.Invoke(nombreEjercicioDelegate, result.Ejercicio.Nombre);
                        DificultadRatingBar.Dispatcher.Invoke(dificultadEjercicioDelegate, result.Ejercicio.Dificultad);
                        DificultadRatingBar.Dispatcher.Invoke(ratingBarDelegate, Visibility.Visible);
                        RepeticionesEjercicioLabel.Dispatcher.Invoke(repeticionesEjercicioDelegate, result.Repeticiones);
                        IniRehabBtn.Dispatcher.Invoke(iniRehabBtnDelegate, true);
                        break;
                    default:
                        break;
                }                
                AtrasBtn.Dispatcher.Invoke(atrasBtnDelegate, Visibility.Visible);
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
            if (e.Key == Key.Enter && AtrasBtn.Visibility == Visibility.Hidden)
            {
                ValidarToken();
            }
        }

        private void SesionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AtrasBtn.Visibility == Visibility.Hidden)
            {
                ValidarToken();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (AtrasBtn.Visibility == Visibility.Hidden)
                    Close();
                else
                    AtrasBtn_Click(sender, e);
            }   
        }

        private void ListarEjercicioBtn_Click(object sender, RoutedEventArgs e)
        {
            //NuevoEjercicio win = new NuevoEjercicio();
            ListaEjercicios win = new ListaEjercicios();
            win.Show();
            Close();
        }

        private void AtrasBtn_Click(object sender, RoutedEventArgs e)
        {
            EstadoPacienteCard(Visibility.Hidden);
            IniRehabBtn.Visibility = Visibility.Hidden;
            AtrasBtn.Visibility = Visibility.Hidden;
            SinConexionCard.Visibility = Visibility.Hidden;
            EstadoRatingBar(Visibility.Hidden);
            EstadoGeneral("Ingrese los datos de la sesión");
            DNITextBox.Focus();
        }
    }
}
