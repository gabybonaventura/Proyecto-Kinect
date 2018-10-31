using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using AtaxiaVision.Testing;
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
    /// Interaction logic for Confirmacion.xaml
    /// </summary>
    public partial class Confirmacion : Window
    {
        public EjercicioViewModel Ejercicio { get; set; }
        public SesionViewModel Sesion { get; set; }
        public List<TensionServos> Tensiones { get; set; }
        public string Pregunta { get; set; }
        
        public delegate void SnackBarDelegate(string msg);
        public SnackBarDelegate snackBarDelegate;
        public delegate void ProgressBarDelegate(Visibility visibility);
        public ProgressBarDelegate progressBarDelegate;
        public delegate void NoBtnDelegate(bool state);
        public NoBtnDelegate noBtnDelegate;
        public delegate void SiBtnDelegate(bool state);
        public SiBtnDelegate siBtnDelegate;
        public delegate void ResultadoLabelDelegate(string resultado);
        public ResultadoLabelDelegate resultadoLabelDelegate;
        public delegate void PreguntaLabelDelegate(string resultado);
        public PreguntaLabelDelegate preguntaLabelDelegate;
        public delegate void EjercicioCardDelegate(Visibility visibility);
        public EjercicioCardDelegate ejercicioCardDelegate;
        public delegate void DesviosLabelDelegate(string resultado);
        public DesviosLabelDelegate desviosLabelDelegate;
        public delegate void FechaLabelDelegate(DateTime date);
        public FechaLabelDelegate fechaLabelDelegate;

        // Backgruond Worker
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public Confirmacion(SesionViewModel sesionVM, EjercicioViewModel ejercicioVM, List<TensionServos> tensiones)
        {
            InitializeComponent();
            Sesion = sesionVM;
            Ejercicio = ejercicioVM;
            Ejercicio.Duracion = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) - Ejercicio.Duracion;
            DuracionLabel.Content = "Duracion: " + Ejercicio.Duracion;
            Tensiones = tensiones;
            ContentTokenLabel(Ejercicio.Token);
            ContentEjercicioLabel(Ejercicio.Ejercicio + "");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TareasBackGruondWorker();
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            progressBarDelegate = new ProgressBarDelegate(EstadoProgressBar);
            siBtnDelegate = new SiBtnDelegate(EstadoSi);
            noBtnDelegate = new NoBtnDelegate(EstadoNo);
            preguntaLabelDelegate = new PreguntaLabelDelegate(ContentPreguntaLabel);
            resultadoLabelDelegate = new ResultadoLabelDelegate(ContentResultadoLabel);
            ejercicioCardDelegate = new EjercicioCardDelegate(EstadoEjercicioCard);
            desviosLabelDelegate = new DesviosLabelDelegate(ContentDesviosLabel);
            fechaLabelDelegate = new FechaLabelDelegate(FechaDesviosLabel);
            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }

        private void TareasBackGruondWorker()
        {
            backgroundWorker.DoWork += (s, e) =>
            {
                // Muestro la barra de cargando
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Visible);
                
                Tensiones = TensionHelper.TestListaTension(4000);
                //Tensiones = TensionHelper.TestListaTensionManual();
                Ejercicio.Desvios = TensionHelper.CalcularDesvios(Tensiones);
                DesviosLabel.Dispatcher.Invoke(desviosLabelDelegate, Ejercicio.Desvios + "");
                var status = ServerHelper.EnviarEjercicio(Ejercicio);
                if (status == ServerHelper.SERVER_ERROR)
                {
                    Snackbar.Dispatcher.Invoke(snackBarDelegate, "No se pudo sincornizar los datos. Vamos a volver a intentar luego.");
                }
                if (Ejercicio.FinalizoConExito)
                {
                    ResultadoLabel.Dispatcher.Invoke(resultadoLabelDelegate, "¡Ejercicio finalizado con éxito!");
                    PreguntaLabel.Dispatcher.Invoke(preguntaLabelDelegate, "¿Desea repetir el ejercicio?");
                }
                else
                {
                    ResultadoLabel.Dispatcher.Invoke(resultadoLabelDelegate, "Ejercicio no finalizado");
                    PreguntaLabel.Dispatcher.Invoke(preguntaLabelDelegate, "¿Desea repetir el ejercicio?");
                }
                EjercicioCard.Dispatcher.Invoke(ejercicioCardDelegate, Visibility.Visible);
                SiBtn.Dispatcher.Invoke(siBtnDelegate, true);
                NoBtn.Dispatcher.Invoke(noBtnDelegate, true);
                FechaLabel.Dispatcher.Invoke(fechaLabelDelegate, DateTime.Now);
                ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Hidden);
            };
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

        private void EstadoSi(bool state)
        {
            SiBtn.IsEnabled = state;
        }

        private void EstadoNo(bool state)
        {
            NoBtn.IsEnabled = state;
        }

        private void ContentResultadoLabel(string text)
        {
            ResultadoLabel.Content = text;
        }

        private void ContentPreguntaLabel(string text)
        {
            PreguntaLabel.Content = text;
        }

        private void EstadoEjercicioCard(Visibility visibility)
        {
            EjercicioCard.Visibility = Visibility;
        }

        private void ContentTokenLabel(string text)
        {
            // Necesito reemplazar con dos __ porque con uno lo subraya
            TokenLabel.Content = "Token: " + text.Replace("_","__");
        }

        private void ContentEjercicioLabel(string text)
        {
            EjercicioLabel.Content = "Ejercicio: " + text;
        }

        private void ContentDesviosLabel(string text)
        {
            DesviosLabel.Content = "Desvios: " + text;
        }

        private void FechaDesviosLabel(DateTime date)
        {
            FechaLabel.Content = "Fecha: " + date.ToString("dd/MM/yyyy hh:mm:ss");
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            Close();
        }

        private void SiBtn_Click(object sender, RoutedEventArgs e)
        {
            Ejercicio.Ejercicio++;
            Principal win = new Principal(Sesion, Ejercicio);
            win.Show();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                NoBtn_Click(sender,e);
            }
            if (e.Key == Key.N)
            {
                NoBtn_Click(sender, e);
            }
            if (e.Key == Key.S)
            {
                SiBtn_Click(sender, e);
            }
        }
    }
}
