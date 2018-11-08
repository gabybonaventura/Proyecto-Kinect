using AtaxiaVision.Controllers;
using AtaxiaVision.Desktop.Pantallas;
using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        ArduinoController arduinoController = new ArduinoController();

        public delegate void EstadoLabelDelegate(string nombre);
        public EstadoLabelDelegate estadoLabelDelegate;
        public delegate void RepetecionesLabelDelegate(int r);
        public RepetecionesLabelDelegate repetecionesLabelDelegate;
        public delegate void RepeticionButtonDelegate(bool isEnable);
        public RepeticionButtonDelegate repeticionButtonDelegate;

        private BackgroundWorker ejercicioAutomaticoBG = new BackgroundWorker();
        private BackgroundWorker ejercicioManualBG = new BackgroundWorker();

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

        private BackgroundWorker TensionesServosBackgroundWorker = new BackgroundWorker();
        private BackgroundWorker ArduinoActivoBackgroundWorker = new BackgroundWorker();
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
            ConsumoHombroArribaAbajoLabel.Content = consumo + " mA";
        }
        private void SetConsumoHombroAdelanteAtras(int consumo)
        {
            ConsumoHombroAdelanteAtrasLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            ConsumoHombroAdelanteAtrasLabel.Content = consumo + " mA";
        }
        private void SetConsumoCodoArribaAbajo(int consumo)
        {
            ConsumoCodoArribaAbajoLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            ConsumoCodoArribaAbajoLabel.Content = consumo + " mA";
        }
        private void SetConsumoCodoDerechaIzquierda(int consumo)
        {
            ConsumoCodoDerechaIzquierdaLabel.Foreground = ArduinoHelper.CalcularColor(consumo);
            ConsumoCodoDerechaIzquierdaLabel.Content = consumo + " mA";
        }
        private void MostrarConsumosDelegates()
        {
            TensionServos tensiones;
            if (arduinoController == null || arduinoController.UltimaTension == null)
            {
                tensiones = new TensionServos()
                {
                    CodoArribaAbajo = 20,
                    CodoIzquierdaDerecha = 105,
                    HombroAdelanteAtras = 215,
                    HombroArribaAbajo = 545
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
                Thread.Sleep(1000);
                if (arduinoController == null || arduinoController.UltimaTension == null)
                    Snackbar.Dispatcher.Invoke(snackBarDelegate, "Exoesqueleto no detectado.");
            };
            if (!bg.IsBusy)
                bg.RunWorkerAsync();
        }
        #endregion

        public SesionSoloBrazo(RespuestaToken token = null)
        {
            InitializeComponent();
            if (token == null)
                Token = new RespuestaToken();
            else
                Token = token;
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new Inicio();
            win.Show();
            Close();
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            estadoLabelDelegate = new EstadoLabelDelegate(EstadoCard);
            repetecionesLabelDelegate = new RepetecionesLabelDelegate(MostrarRepeticiones);
            repeticionButtonDelegate = new RepeticionButtonDelegate(EstadoRepetecionesButton);
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            consumoHombroArribaAbajoDelegate = new ConsumoHombroArribaAbajoDelegate(SetConsumoHombroArribaAbajo);
            consumoHombroAdelanteAtrasDelegate = new ConsumoHombroAdelanteAtras(SetConsumoHombroAdelanteAtras);
            consumoCodoArribaAbajoDelegate = new ConsumoCodoArribaAbajo(SetConsumoCodoArribaAbajo);
            consumoCodoDerechaIzquierdaDelegate = new ConsumoCodoDerechaIzquierda(SetConsumoCodoDerechaIzquierda);
            TensionesServosBG();
            ArduinoActivoBG();
            EjecutarEjercicioManualBG();   // Preparo ejercicio manual

            LlenarComboBoxs();          // Lleno los combo boxs
            MostrarRepeticiones(0);      // Lleno el campo de repeticiones
            arduinoController.Inicializar(ArduinoController.BRAZO_GB);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                var win = new Inicio();
                win.Show();
                Close();
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
                    RepeticionesLabel.Dispatcher.Invoke(repetecionesLabelDelegate, (i + 1));
                    EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Inicio repetición: " + (i + 1));
                    Thread.Sleep(2 * 1000);
                    arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(Token.Ejercicio.EstadoInicial));
                    EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Posicionando Estado Inicial [" + Token.Ejercicio.EstadoInicial + "]");
                    Thread.Sleep(DelaySeg * 1000);
                    EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Posicionando Estado Final [" + Token.Ejercicio.EstadoFinal + "]");
                    arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(Token.Ejercicio.EstadoFinal));
                    Thread.Sleep(DelaySeg * 1000);
                }
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Fin de repeticiones.");
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
                RepeticionesLabel.Dispatcher.Invoke(repetecionesLabelDelegate, (Repeticiones + 1));
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Inicio repetición: " + (Repeticiones + 1));
                Thread.Sleep(2 * 1000);
                arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(Token.Ejercicio.EstadoInicial));
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Posicionando Estado Inicial [" + Token.Ejercicio.EstadoInicial + "]");
                Thread.Sleep(DelaySeg * 1000);
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Posicionando Estado Final [" + Token.Ejercicio.EstadoFinal + "]");
                arduinoController.EnviarAngulosFromAngulosServos(new AngulosServos(Token.Ejercicio.EstadoFinal));
                Thread.Sleep(DelaySeg * 1000);
                Repeticiones++;
                EstadoLabel.Dispatcher.Invoke(estadoLabelDelegate, "Fin Repetición");
                if (Repeticiones < Token.Repeticiones)
                    RepeticionButton.Dispatcher.Invoke(repeticionButtonDelegate, true);
                else
                    RepeticionButton.Dispatcher.Invoke(repeticionButtonDelegate, false);
            };
        }
    }
}
