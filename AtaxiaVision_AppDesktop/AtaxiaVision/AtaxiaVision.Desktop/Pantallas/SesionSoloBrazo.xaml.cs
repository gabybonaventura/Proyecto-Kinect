using AtaxiaVision.Controllers;
using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using MaterialDesignThemes.Wpf;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace AtaxiaVision.Pantallas
{
    /// <summary>
    /// Lógica de interacción para SesionSoloBrazo.xaml
    /// </summary>
    public partial class SesionSoloBrazo : Window
    {
        #region Properties
        private const float RenderWidth = 640.0f;

        private const float RenderHeight = 480.0f;

        private KinectSensor sensor;

        private DrawingGroup drawingGroup;

        private DrawingImage imageSource;

        private WriteableBitmap colorBitmap;

        private byte[] colorPixels;

        VideoController videoController;


        public List<int> Delays
        {
            get
            {
                return new List<int>() { 3, 4, 5, 6, 7, 8, 9, 10 };
            }
        }
        public List<string> ModoRepeticion
        { get
            {
                return new List<string>() { "Automático", "Manual" };
            }
        }
        public RespuestaToken Token { get; set; }
        public bool RepeticionAutomatica { get; set; }
        public int DelaySeg { get; set; }
        public int Repeticiones { get; set; }
        private RepeticionViewModel Ejercicio { get; set; }
        private SesionViewModel Sesion { get; set; }
        ArduinoController arduinoController;

        public delegate void EstadoLabelDelegate(string nombre);
        public EstadoLabelDelegate estadoLabelDelegate;
        public delegate void RepetecionesLabelDelegate(int r);
        public RepetecionesLabelDelegate repetecionesLabelDelegate;
        public delegate void RepeticionButtonDelegate(bool isEnable);
        public RepeticionButtonDelegate repeticionButtonDelegate;

        private BackgroundWorker ejercicioAutomaticoBG = new BackgroundWorker();
        private BackgroundWorker ejercicioManualBG = new BackgroundWorker();
        #endregion Properties

        #region Delegates
        public delegate void SnackBarDelegate(string msg);
        public SnackBarDelegate snackBarDelegate;
        public delegate void ConsumoHombroArribaAbajoDelegate(int consumo);
        public ConsumoHombroArribaAbajoDelegate consumoHombroArribaAbajoDelegate;
        public delegate void ConsumoHombroAdelanteAtras(int consumo);
        public ConsumoHombroAdelanteAtras consumoHombroAdelanteAtrasDelegate;
        public delegate void ConsumoCodoArribaAbajo(int consumo);
        public ConsumoCodoArribaAbajo consumoCodoArribaAbajoDelegate;
        public delegate void ConsumoCodoDerechaIzquierda(int consumo);
        public ConsumoCodoDerechaIzquierda consumoCodoDerechaIzquierdaDelegate;
        public delegate void ReportesButtonDelegate(Visibility visibility);
        public ReportesButtonDelegate reportesButtonDelegate;
        public delegate void SeCortoExoesqueletoDelegate();
        public SeCortoExoesqueletoDelegate seCortoExoesqueletoDelegate;

        private BackgroundWorker TensionesServosBackgroundWorker = new BackgroundWorker();
        private BackgroundWorker ArduinoActivoBackgroundWorker = new BackgroundWorker();
        private bool _grabando = false;
        #endregion

        #region Metodos Delegates
        private void EstadoSnackBar(string mensaje)
        {
            Snackbar.IsActive = false;
            Snackbar.Message = new SnackbarMessage() { Content = mensaje };
            Snackbar.IsActive = true;
        }
        private void SetConsumoHombroArribaAbajo(int consumo)
        {
            ConsumoHombroArribaAbajoLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            if (consumo > 0)
                ConsumoHombroArribaAbajoLabel.Content = consumo + " mA";
            else
                ConsumoHombroArribaAbajoLabel.Content = "-";
        }
        private void SetConsumoHombroAdelanteAtras(int consumo)
        {
            ConsumoHombroAdelanteAtrasLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            if (consumo > 0)
                ConsumoHombroAdelanteAtrasLabel.Content = consumo + " mA";
            else
                ConsumoHombroAdelanteAtrasLabel.Content = "-";
        }
        private void SetConsumoCodoArribaAbajo(int consumo)
        {
            ConsumoCodoArribaAbajoLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            if (consumo > 0)
                ConsumoCodoArribaAbajoLabel.Content = consumo + " mA";
            else
                ConsumoCodoArribaAbajoLabel.Content = "-";
        }
        private void SetConsumoCodoDerechaIzquierda(int consumo)
        {
            ConsumoCodoDerechaIzquierdaLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            if (consumo > 0)
                ConsumoCodoDerechaIzquierdaLabel.Content = consumo + " mA";
            else
                ConsumoCodoDerechaIzquierdaLabel.Content = "-";
        }
        private void MostrarConsumosDelegates()
        {
            TensionServos tensiones;
            if (arduinoController == null || arduinoController.UltimaTension == null)
            {
                tensiones = new TensionServos()
                {
                    CodoArribaAbajo = -1,
                    CodoIzquierdaDerecha = -1,
                    HombroAdelanteAtras = -1,
                    HombroArribaAbajo = -1
                };
            }
            else
                tensiones = arduinoController.UltimaTension;

            ConsumoHombroArribaAbajoLabel.Dispatcher.Invoke(consumoHombroArribaAbajoDelegate, tensiones.HombroArribaAbajo);
            ConsumoHombroAdelanteAtrasLabel.Dispatcher.Invoke(consumoHombroAdelanteAtrasDelegate, tensiones.HombroAdelanteAtras);
            ConsumoCodoArribaAbajoLabel.Dispatcher.Invoke(consumoCodoArribaAbajoDelegate, tensiones.CodoArribaAbajo);
            ConsumoCodoDerechaIzquierdaLabel.Dispatcher.Invoke(consumoCodoDerechaIzquierdaDelegate, tensiones.CodoIzquierdaDerecha);
        }
        private void TensionesServosBG()
        {
            var bg = TensionesServosBackgroundWorker;
            bg.DoWork += (s, e) =>
            {
                while (true)
                {
                    MostrarConsumosDelegates();
                    Thread.Sleep(1000);
                }
            };

            if (!bg.IsBusy)
                bg.RunWorkerAsync();
        }
        private void ArduinoActivoBG()
        {
            var bg = ArduinoActivoBackgroundWorker;
            bg.DoWork += (s, e) =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (arduinoController == null || arduinoController.UltimaTension == null)
                    {
                        Snackbar.Dispatcher.Invoke(snackBarDelegate, "Exoesqueleto no detectado.");
                        Snackbar.Dispatcher.Invoke(seCortoExoesqueletoDelegate, null);
                    }
                }
            };
            if (!bg.IsBusy)
                bg.RunWorkerAsync();
        }

        private void DehabilitarBotones()
        {
            DelayComboBox.IsEnabled = false;
            ModoRepeticionComboBox.IsEnabled = false;
            IniciarButton.Visibility = Visibility.Hidden;
            SinExoesqueletoButton.Visibility = Visibility.Visible;
            EstadoLabel.Content = "Se detecto la falta de exoesqueleto. Por favor finalizar el ejercicio.";
        }

        private void VisibilidadReportesButton(Visibility visibility)
        {
            ReportesButton.Visibility = visibility;
            if(visibility == Visibility.Visible)
                ReportesButton.Focus();
        }
        #endregion

        public SesionSoloBrazo(RespuestaToken token, SesionViewModel sesionVM, RepeticionViewModel ejercicioVM)
        {
            InitializeComponent();
            Sesion = sesionVM;
            Ejercicio = ejercicioVM;
            Ejercicio.Duracion = new TimeSpan(0, 0, 0);
            Token = token;
            arduinoController = new ArduinoController();
            videoController = new VideoController();
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            IrAConfirmacion();
        }

        private void LlenarComboBoxs()
        {
            DelayComboBox.ItemsSource = Delays;
            DelayComboBox.SelectedValue = Delays.FirstOrDefault();
            ModoRepeticionComboBox.ItemsSource = ModoRepeticion;
            ModoRepeticionComboBox.SelectedValue = ModoRepeticion.FirstOrDefault();
        }

        private void MostrarRepeticiones(int r)
        {
            RepeticionesLabel.Content = r +"/" + Token.Repeticiones;
        }

        private void EstadoRepetecionesButton(bool isEnable)
        {
            RepeticionButton.IsEnabled = isEnable;
            if (isEnable)
                RepeticionButton.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.drawingGroup = new DrawingGroup();
            this.imageSource = new DrawingImage(this.drawingGroup);
            Image.Source = this.imageSource;

            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                this.sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                this.colorPixels = new byte[this.sensor.ColorStream.FramePixelDataLength];
                this.colorBitmap = new WriteableBitmap(this.sensor.ColorStream.FrameWidth, this.sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                sensor.ColorFrameReady += Sensor_ColorFrameReady;
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }
            }
            if (null == this.sensor)
            {
                Image.Source = new BitmapImage(
                    new Uri("pack://application:,,,/AtaxiaVision;component/Imagenes/KinectAV.png"));
                EstadoSnackBar("Kinect no lista.");
            }
            seCortoExoesqueletoDelegate = new SeCortoExoesqueletoDelegate(DehabilitarBotones);
            estadoLabelDelegate = new EstadoLabelDelegate(EstadoCard);
            repetecionesLabelDelegate = new RepetecionesLabelDelegate(MostrarRepeticiones);
            repeticionButtonDelegate = new RepeticionButtonDelegate(EstadoRepetecionesButton);
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            consumoHombroArribaAbajoDelegate = new ConsumoHombroArribaAbajoDelegate(SetConsumoHombroArribaAbajo);
            consumoHombroAdelanteAtrasDelegate = new ConsumoHombroAdelanteAtras(SetConsumoHombroAdelanteAtras);
            consumoCodoArribaAbajoDelegate = new ConsumoCodoArribaAbajo(SetConsumoCodoArribaAbajo);
            consumoCodoDerechaIzquierdaDelegate = new ConsumoCodoDerechaIzquierda(SetConsumoCodoDerechaIzquierda);
            reportesButtonDelegate = new ReportesButtonDelegate(VisibilidadReportesButton);
            TensionesServosBG();
            ArduinoActivoBG();
            EjecutarEjercicioManualBG();   // Preparo ejercicio manual

            LlenarComboBoxs();          // Lleno los combo boxs
            MostrarRepeticiones(0);      // Lleno el campo de repeticiones
            //Si no se logra iniciar el arduino se vuelve a inicio
            //arduinoController.Inicializar(ArduinoController.BRAZO_GB);
            if (!arduinoController.Inicializar(ArduinoController.BRAZO_GB))
            {
                Cerrar();
                var win = new Inicio("Exoesqueleto no detectado. No se puede ejecutar el ejecicio sin el mismo.");
                win.Show();
                Close();
            }
            IniciarButton.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                IrAConfirmacion();
            }
        }

        private void DehabilitarComponentes()
        {
            DelayComboBox.IsEnabled = false;
            ModoRepeticionComboBox.IsEnabled = false;
            IniciarButton.IsEnabled = false;
            // Si es manual, oculto el boton y muestro el otro
            if (!RepeticionAutomatica)
            {
                RepeticionButton.Visibility = Visibility.Visible;
                IniciarButton.Visibility = Visibility.Hidden;
            }
        }

        private void EsRepeticionAutomatica()
        {
            if (ModoRepeticionComboBox.SelectedValue.ToString() == "Automático")
                RepeticionAutomatica = true;
        }

        private void GuardarDelay()
        {
            DelaySeg = Convert.ToInt32(DelayComboBox.SelectedValue);
        }

        private void IniciarButton_Click(object sender, RoutedEventArgs e)
        {
            _grabando = true;
            EsRepeticionAutomatica();
            GuardarDelay();
            DehabilitarComponentes();
            EjecutarEjercicio();
        }

        private void EjecutarEjercicio()
        {
            if (RepeticionAutomatica)
                EjecutarEjercicioAutomatico();
            else
                EjecutarEjercicioManual();
        }

        private void RepeticionButton_Click(object sender, RoutedEventArgs e)
        {
            EjecutarEjercicioManual();
        }

        public void EstadoCard(string estado)
        {
            EstadoLabel.Content = estado;
        }

        public void EjecutarEjercicioAutomatico()
        {
            ejercicioAutomaticoBG.DoWork += (s, e) =>
            {
                for (int i = 0; i < Token.Repeticiones; i++)
                {
                    EjecutarRepeticion(i + 1);
                }
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Fin de repeticiones.");
                if (arduinoController != null && arduinoController.UltimaTension != null)
                    ReportesButton.Dispatcher.Invoke(reportesButtonDelegate, Visibility.Visible);
                videoController.FinGrabacion = DateTime.Now.Ticks;
                _grabando = false;
            };

            if (!ejercicioAutomaticoBG.IsBusy)
                ejercicioAutomaticoBG.RunWorkerAsync();
        }

        private void EjecutarEjercicioManual()
        {
            if (!ejercicioManualBG.IsBusy)
                ejercicioManualBG.RunWorkerAsync();
        }

        public void EjecutarEjercicioManualBG()
        {
            ejercicioManualBG.DoWork += (s, e) =>
            {
                RepeticionButton.Dispatcher.Invoke(repeticionButtonDelegate, false);
                EjecutarRepeticion(Repeticiones + 1);
                Repeticiones++;
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Fin Repetición");
                if (Repeticiones < Token.Repeticiones)
                    RepeticionButton.Dispatcher.Invoke(repeticionButtonDelegate, true);
                else
                {
                    RepeticionButton.Dispatcher.Invoke(repeticionButtonDelegate, false);
                    if (arduinoController != null && arduinoController.UltimaTension != null)
                        ReportesButton.Dispatcher.Invoke(reportesButtonDelegate, Visibility.Visible);
                    videoController.FinGrabacion = DateTime.Now.Ticks;
                    _grabando = false;
                }
            };
        }

        private void EjecutarRepeticion(int i)
        {
            if (arduinoController != null && arduinoController.UltimaTension != null)
            {
                TimeSpan inicio = new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
                RepeticionesLabel.Dispatcher.Invoke(repetecionesLabelDelegate, (i));
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Inicio repetición: " + (i));
                Thread.Sleep(2 * 1000);
                arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(Token.Ejercicio.EstadoInicial));
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Posicionando Estado Inicial");
                Thread.Sleep(DelaySeg * 1000);
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Posicionando Estado Final");
                arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(Token.Ejercicio.EstadoFinal));
                Thread.Sleep(DelaySeg * 1000);
                TimeSpan fin = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                TimeSpan duracion = fin - inicio;
                Ejercicio.Duracion += duracion;
            }
        }

        private void IrAConfirmacion(bool finConExito = false)
        {
            Cerrar();

            Ejercicio.FinalizoConExito = finConExito;
            
            Confirmacion win = new Confirmacion(Token, Sesion, Ejercicio, arduinoController.Tensiones, videoController);
            win.Show();
            Close();
        }

        private void IrAConfirmacion_Click(object sender, RoutedEventArgs e)
        {
            IrAConfirmacion(true);
        }

        private void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {

                if (colorFrame != null)
                {

                    colorFrame.CopyPixelDataTo(this.colorPixels);

                    System.Drawing.Bitmap bmp = EmguCVHelper.ImageToBitmap(colorFrame);

                    if(_grabando)
                    {
                        videoController.InicioGrabacion = DateTime.Now.Ticks;

                        videoController.framesBmp.Add(bmp);
                    }
                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }

                using (DrawingContext dc = this.drawingGroup.Open())
                {
                    dc.DrawImage(this.colorBitmap, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                    this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
                }
            }
        }

        private void Cerrar()
        {
            arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(ArduinoController.BRAZO_GB));
            //videoController.FinGrabacion = DateTime.Now.Ticks;
            arduinoController.CerrarPuerto();
            if (null != this.sensor)
            {
                this.sensor.Stop();
                this.sensor.Dispose();
            }
        }

        private void SinExoesqueletoButton_Click(object sender, RoutedEventArgs e)
        {
            IrAConfirmacion();
        }
    }
}
