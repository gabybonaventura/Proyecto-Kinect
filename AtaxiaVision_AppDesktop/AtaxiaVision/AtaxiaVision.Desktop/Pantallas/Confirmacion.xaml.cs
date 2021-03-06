﻿using AtaxiaVision.Desktop.Pantallas;
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

        int FrameInicioBoomerang;
        int FrameFinBoomerang;
        int CurrentFrameNo;
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
        // Backgruond Worker
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private bool _reproducirBoomerang = true;
        private BackgroundWorker _guardarVideoBackgroundWorker = new BackgroundWorker();

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
            
            Sesion = sesionVM;
            Ejercicio = ejercicioVM;
            RespuestaToken = token;
            DuracionLabel.Content = "Duracion: " + Ejercicio.Duracion;
            Tensiones = tensiones;
            ContentTokenLabel(Ejercicio.Token);
            ContentEjercicioLabel(Ejercicio.Ejercicio + "");
            
            int Duracion = (int)(new TimeSpan(_videoController.FinGrabacion - _videoController.InicioGrabacion)).TotalSeconds;
            Duracion = Duracion == 0 ? 1 : Duracion;
            FPS = _videoController.framesBmp.Count / Duracion;
            CurrentFrameNo = 0;
            if (_videoController != null && _videoController.framesBmp.Count > 0)
                PlayVideoBitMap();
            else
                GuardarVideoButton.IsEnabled = false;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            backgroundWorker.DoWork += TareasBackGruondWorker;
            _guardarVideoBackgroundWorker.DoWork += GuardarVideoAsync;
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

            ComentarioRepeticionTextBox.Focus();
        }

        private void GuardarVideoAsync(object sender, DoWorkEventArgs e)
        {
            SiBtn.Dispatcher.Invoke(siBtnDelegate, false);
            NoBtn.Dispatcher.Invoke(noBtnDelegate, false);

            ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Visible);

            string nombreArchivo = $"Paciente{Sesion.Token} {DateTime.Now.ToString("ddMMyyyy")}";

            _videoController.GuardarVideo(nombreArchivo);

            Process.Start($"C:\\Users\\Public\\Videos\\{nombreArchivo}.avi");

            ProgressBar.Dispatcher.Invoke(progressBarDelegate, Visibility.Hidden);

            SiBtn.Dispatcher.Invoke(siBtnDelegate, true);
            NoBtn.Dispatcher.Invoke(noBtnDelegate, true);

        }

        private void TareasBackGruondWorker(object sender, DoWorkEventArgs e)
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
            EnviarComentario();
            Close();
        }

        private void SiBtn_Click(object sender, RoutedEventArgs e)
        {
            Ejercicio.Ejercicio++;
            if(RespuestaToken.Ejercicio.Nombre == "Reach")
            {
                Principal win = new Principal(RespuestaToken, Sesion, Ejercicio);
                win.Show();
            }
            else
            {
                SesionSoloBrazo win = new SesionSoloBrazo(RespuestaToken, Sesion, Ejercicio);
                win.Show();
            }
            EnviarComentario();
            Close();
        }

        private async void PlayVideoBitMap()
        {
            FrameFinBoomerang = _videoController.framesBmp.Count - 1;
            FrameInicioBoomerang = FrameFinBoomerang - 50;
            while (FrameInicioBoomerang < 0)
                FrameInicioBoomerang += 5;
            if (FrameInicioBoomerang >= FrameFinBoomerang)
                return;
            CurrentFrameNo = FrameInicioBoomerang;
            if (FPS == 0)
                FPS = 30;

            try
            {
                while (_reproducirBoomerang)
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

            }
        }
        
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                NoBtn_Click(sender,e);
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
            GuardarVideoButton.IsEnabled = false;
            _guardarVideoBackgroundWorker.RunWorkerAsync();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _reproducirBoomerang = false;
            _videoController.framesBmp = null;
        }

        private void EnviarComentario()
        {
            if (!String.IsNullOrEmpty(ComentarioRepeticionTextBox.Text))
            {
                ServerHelper.EnviarComentarioPaciente(
                    RespuestaToken.Paciente.PacienteId,
                    new Comments
                    {
                        comment = ComentarioRepeticionTextBox.Text
                    });
            }
        }
    }
}
