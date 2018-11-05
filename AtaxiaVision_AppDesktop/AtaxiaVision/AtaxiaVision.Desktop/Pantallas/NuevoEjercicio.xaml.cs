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
    /// Interaction logic for NuevoEjercicio.xaml
    /// </summary>
    public partial class NuevoEjercicio : Window
    {
        private ArduinoController Arduino { get; set; }
        private List<int> AngulosDisponibles {
            get
            {
                return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180 };
            }
        }
        
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
            if (Arduino == null || Arduino.UltimaTension == null)
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
                tensiones = Arduino.UltimaTension;

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
                if (Arduino == null || Arduino.UltimaTension == null)
                    Snackbar.Dispatcher.Invoke(snackBarDelegate, "Exoesqueleto no detectado.");
            };
            if (!bg.IsBusy)
                bg.RunWorkerAsync();
        }
        #endregion

        private void AsignarListasAngulos()
        {
            AnguloHombroArribaAbajoComboBox.ItemsSource = AngulosDisponibles;
            AnguloHombroAdelanteAtrasComboBox.ItemsSource = AngulosDisponibles;
            AnguloCodoArribaAbajoComboBox.ItemsSource = AngulosDisponibles;
            AnguloCodoDerechaIzquierdaComboBox.ItemsSource = AngulosDisponibles;
        }
        private void InicializarArduino()
        {
            try
            {
                Arduino = new ArduinoController();
                Arduino.Inicializar(ArduinoController.BRAZO_CC);
            }
            catch (Exception)
            {
                Arduino = null;
            }
        }

        public NuevoEjercicio()
        {
            InitializeComponent();
            AsignarListasAngulos();
            InicializarArduino();

            
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            Inicio inicio = new Inicio();
            inicio.Show();
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CerrarBtn_Click(sender,e);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NombreEjercicioTextBox.Focus();

            #region Delegates
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            consumoHombroArribaAbajoDelegate = new ConsumoHombroArribaAbajoDelegate(SetConsumoHombroArribaAbajo);
            consumoHombroAdelanteAtrasDelegate = new ConsumoHombroAdelanteAtras(SetConsumoHombroAdelanteAtras);
            consumoCodoArribaAbajoDelegate = new ConsumoCodoArribaAbajo(SetConsumoCodoArribaAbajo);
            consumoCodoDerechaIzquierdaDelegate = new ConsumoCodoDerechaIzquierda(SetConsumoCodoDerechaIzquierda);
            TensionesServosBG();
            ArduinoActivoBG();
            #endregion
        }

        private void EnviarAngulos()
        {

        }
    }
}
