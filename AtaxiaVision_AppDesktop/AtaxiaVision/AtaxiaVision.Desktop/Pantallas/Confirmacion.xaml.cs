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
using Emgu.CV;
using System.Threading;
using System.Windows.Interop;
using AtaxiaVision.Controllers;
using System.Drawing;
using System.Diagnostics;

namespace AtaxiaVision.Pantallas
{
    /// <summary>
    /// Interaction logic for Confirmacion.xaml
    /// </summary>
    public partial class Confirmacion : Window
    {
        private VideoController _videoController;

        VideoCapture videocapture;
        int TotalFrames;
        int FrameInicioBoomerang;
        int FrameFinBoomerang;
        int CurrentFrameNo;
        Mat CurrentFrame;
        int FPS;
        bool Adelante = true;

        public RepeticionViewModel Ejercicio { get; set; }
        public SesionViewModel Sesion { get; set; }
        public List<TensionServos> Tensiones { get; set; }
        public RespuestaToken RespuestaToken { get; set; }
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
        public string nombreArchivo;
        // Backgruond Worker
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public Confirmacion(RespuestaToken token,
            SesionViewModel sesionVM, 
            RepeticionViewModel ejercicioVM, 
            List<TensionServos> tensiones, 
            VideoController videoController)
        {
            InitializeComponent();
            this._videoController = videoController;

            this.nombreArchivo = nombreArchivo;
            Sesion = sesionVM;
            Ejercicio = ejercicioVM;
            RespuestaToken = token;
            DuracionLabel.Content = "Duracion: " + Ejercicio.Duracion;
            Tensiones = tensiones;
            ContentTokenLabel(Ejercicio.Token);
            ContentEjercicioLabel(Ejercicio.Ejercicio + "");

            videocapture = new VideoCapture($"C://Users//Public/Videos//{nombreArchivo}.avi");
            TotalFrames = Convert.ToInt32(videocapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount));
            FrameInicioBoomerang = TotalFrames - 51;
            FrameFinBoomerang = TotalFrames - 1;
            //FPS = Convert.ToInt32(videocapture.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps));
            int Duracion = (int)(new TimeSpan(_videoController.FinGrabacion - _videoController.InicioGrabacion)).TotalSeconds;
            Duracion = Duracion == 0 ? 1 : Duracion;
            FPS = _videoController.framesBmp.Count / Duracion;
            CurrentFrame = new Mat();
            CurrentFrameNo = 0;
            //PlayVideo();
            PlayVideoBitMap();
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
                
                //Tensiones = TensionHelper.TestListaTension(4000);
                //Tensiones = TensionHelper.TestListaTensionManual();
                Ejercicio.Desvios = TensionHelper.CalcularDesvios(Tensiones);
                DesviosLabel.Dispatcher.Invoke(desviosLabelDelegate, Ejercicio.Desvios + "");
                var status = ServerHelper.EnviarRepeticion(Ejercicio);
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
            Principal win = new Principal(RespuestaToken, Sesion, Ejercicio);
            win.Show();
            Close();
        }
        private async void PlayVideoBitMap()
        {
            if (videocapture == null)
            {
                return;
            }
            FrameFinBoomerang = _videoController.framesBmp.Count - 1;
            FrameInicioBoomerang = FrameFinBoomerang - 50;
            while (FrameInicioBoomerang < 0)
                FrameInicioBoomerang += 5;
            if (FrameInicioBoomerang >= FrameFinBoomerang)
                return;
            CurrentFrameNo = FrameInicioBoomerang;

            try
            {
                while (true)
                {
                    Bitmap bmpFrameActual = _videoController.framesBmp[CurrentFrameNo];

                    IntPtr handle = bmpFrameActual.GetHbitmap();
                    try
                    {
                        ImageSource imageSource = Imaging
                                    .CreateBitmapSourceFromHBitmap(handle,
                                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        this.Image.Source = imageSource;
                        if (CurrentFrameNo == FrameInicioBoomerang)
                            Adelante = true;
                        if (CurrentFrameNo == FrameFinBoomerang)
                            Adelante = false;

                        if (Adelante)
                            CurrentFrameNo += 5;
                        else
                            CurrentFrameNo -= 5;
                        await Task.Delay(1000 / FPS);
                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                    finally
                    {
                        //Tengo que marcar el objeto como borrado, para que el garbage collector lo borre
                        //Sino colapsa la memoria
                        DeleteObject(handle);
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private async void PlayVideo()
        {
            if (videocapture == null)
            {
                return;
            }
            try
            {               
                CurrentFrameNo = TotalFrames - 51;
                while (CurrentFrameNo < TotalFrames)
                {
                    videocapture.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, CurrentFrameNo);
                    videocapture.Read(CurrentFrame);
                    
                    //Tener cuidado con este puntero porque sobre carga la memoria
                    IntPtr handle = CurrentFrame.Bitmap.GetHbitmap();

                    try
                    {

                        ImageSource imageSource = Imaging
                            .CreateBitmapSourceFromHBitmap(handle,
                            IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                        this.Image.Source = imageSource;
                        if (CurrentFrameNo == FrameInicioBoomerang)
                            Adelante = true;
                        if (CurrentFrameNo == FrameFinBoomerang)
                            Adelante = false;

                        if (Adelante)
                            CurrentFrameNo += 5;
                        else
                            CurrentFrameNo -= 5;

                        await Task.Delay(1000 / FPS);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        //Tengo que marcar el objeto como borrado, para que el garbage collector lo borre
                        //Sino colapsa la memoria
                        DeleteObject(handle);
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.C || e.Key == Key.Enter)
            {
                //NoBtn_Click(sender,e);
                Close();
            }
            //if (e.Key == Key.N)
            //{
            //    NoBtn_Click(sender, e);
            //}
            //if (e.Key == Key.S)
            //{
            //    SiBtn_Click(sender, e);
            //}
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GuardarVideoClick(object sender, RoutedEventArgs e)
        {

            string nombreArchivo = $"Paciente{Sesion.Token} {DateTime.Now.ToString("ddMMyyyy")}";

            _videoController.GuardarVideo(nombreArchivo);

            GuardarVideoButton.IsEnabled = false;

            Process.Start($"C:\\Users\\Public\\Videos\\{nombreArchivo}.avi");
        }
    }
}
