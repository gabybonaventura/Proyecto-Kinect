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
        public ArduinoController Arduino { get; set; }
        public AngulosServos Angulos { get; set; }
        public AngulosServos AngulosDefault { get; set; }
        public Exercise Ejercicio { get; set; }
        public Exercise EjercicioGenerico { get; set; }
        public List<int> AngulosDisponibles {
            get
            {
                return new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 149, 150, 151, 152, 153, 154, 155, 156, 157, 158, 159, 160, 161, 162, 163, 164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 180 };
            }
        }
        public List<string> Destino { get; set; }
        public RespuestaListaPacientes Pacientes { get; set; }
        
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
        public void SetEjercicio(Exercise ejercicio)
        {
            if(ejercicio != null)
            {
                Ejercicio = ejercicio;
            }
            else
            {
                Ejercicio = new Exercise();
            }
        }
        public void SetEjercicioGenerico(Exercise ejercicio)
        {
            EjercicioGenerico = ejercicio;
        }
        private void InicializarArduino()
        {
            AngulosDefault = new AngulosServos(ArduinoController.BRAZO_GB);
            try
            {
                Arduino = new ArduinoController();
                if (String.IsNullOrEmpty(Ejercicio.EstadoInicial))
                    Angulos = new AngulosServos(AngulosDefault.ToString());
                else
                    Angulos = new AngulosServos(Ejercicio.EstadoInicial);
                Arduino.Inicializar(Angulos.ToString());
            }
            catch (Exception)
            {
                Arduino = null;
            }
        }
        public void LlenarCampos()
        {
            NombreEjercicioTextBox.Text = Ejercicio.Nombre;
            DescripcionEjercicioTextBox.Text = Ejercicio.Descripcion;
            DificultadRatingBar.Value = Ejercicio.Dificultad;
            if(!String.IsNullOrEmpty(Ejercicio.EstadoInicial) 
                && !String.IsNullOrEmpty(Ejercicio.EstadoFinal))
            {
                GuardarEjercicioBtn.IsEnabled = true;
                VerEstadoInicialBtn.IsEnabled = true;
                VerEstadoFinalBtn.IsEnabled = true;
            }
            else
            {
                if (!String.IsNullOrEmpty(Ejercicio.EstadoInicial))
                    VerEstadoInicialBtn.IsEnabled = true;
                if (!String.IsNullOrEmpty(Ejercicio.EstadoFinal))
                    VerEstadoFinalBtn.IsEnabled = true;
            }
        }

        public void SetAngulos()
        {
            AnguloHombroArribaAbajoComboBox.SelectedValue = Angulos.HomroArribaAbajo;
            AnguloHombroAdelanteAtrasComboBox.SelectedValue = Angulos.HomroAdelanteAtras;
            AnguloCodoArribaAbajoComboBox.SelectedValue = Angulos.CodoArribaAbajo;
            AnguloCodoDerechaIzquierdaComboBox.SelectedValue = Angulos.CodoDerechaIzquierda;
            Arduino.EnviarAngulosFromAngulosServos(Angulos);
        }

        public void LlenarDesplegable()
        {
            Destino = new List<string>() { "Genérico" };
            // Si es un ejercicio editado, listo personas
            if (!String.IsNullOrEmpty(Ejercicio.ID))
            {
                Pacientes = ServerHelper.ObtenerPacientes();
                Destino.AddRange(Pacientes.Pacientes.Select(x => x.Nombre));
            }
            DestinoComboBox.ItemsSource = Destino;
            // Siempre selecciono el "Generico"
            DestinoComboBox.SelectedItem = DestinoComboBox.Items[0];
        }

        public NuevoEjercicio(Exercise ejercicio = null)
        {
            InitializeComponent();      // Inicializar componentes
            Ejercicio = ejercicio;
        }

        private void CerrarBtn_Click(object sender, RoutedEventArgs e)
        {
            var win = new ListaEjercicios();
            win.Show();
            Arduino.CerrarPuerto();
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
            #region Delegates
            snackBarDelegate = new SnackBarDelegate(EstadoSnackBar);
            consumoHombroArribaAbajoDelegate = new ConsumoHombroArribaAbajoDelegate(SetConsumoHombroArribaAbajo);
            consumoHombroAdelanteAtrasDelegate = new ConsumoHombroAdelanteAtras(SetConsumoHombroAdelanteAtras);
            consumoCodoArribaAbajoDelegate = new ConsumoCodoArribaAbajo(SetConsumoCodoArribaAbajo);
            consumoCodoDerechaIzquierdaDelegate = new ConsumoCodoDerechaIzquierda(SetConsumoCodoDerechaIzquierda);
            TensionesServosBG();
            ArduinoActivoBG();
            #endregion
            
            AsignarListasAngulos();     // Asigna los 180 grados de las listas
            SetEjercicio(Ejercicio);    // Setea el EjercicioViewModel
            if (Ejercicio != null)
                SetEjercicioGenerico(Ejercicio);
            InicializarArduino();       // Envia los datos iniciales al exoesqueleto
            LlenarCampos();             // Llena campos en base al EjercicioViewModel
            SetAngulos();               // Setea los grados a la vista y al exoesqueleto
            LlenarDesplegable();        // Llena el desplegable dependiendo si es un nuevo ejercicio
            NombreEjercicioTextBox.Focus();
        }

        private void HabilitarBotonGuardar()
        {
            if (!String.IsNullOrEmpty(Ejercicio.EstadoFinal) &&
                !String.IsNullOrEmpty(Ejercicio.EstadoInicial))
                GuardarEjercicioBtn.IsEnabled = true;
        }

        private bool ValidarEjercicio()
        {
            if (String.IsNullOrEmpty(Ejercicio.Nombre))
            {
                EstadoSnackBar("Ingresé un Nombre para el ejercicio.");
                return false;
            }
            if (Ejercicio.Dificultad == 0)
            {
                EstadoSnackBar("Ingresé una Dificultad para el ejercicio.");
                return false;
            }
            if (String.IsNullOrEmpty(Ejercicio.Descripcion))
            {
                EstadoSnackBar("Ingresé una Descripción para el ejercicio.");
                return false;
            }
            if (String.IsNullOrEmpty(Ejercicio.EstadoInicial))
            {
                EstadoSnackBar("Ingresé un Estado Inicial para el ejercicio.");
                return false;
            }
            if (String.IsNullOrEmpty(Ejercicio.EstadoFinal))
            {
                EstadoSnackBar("Ingresé un Estado Final para el ejercicio.");
                return false;
            }
            if(Ejercicio.EstadoInicial == Ejercicio.EstadoFinal)
            {
                EstadoSnackBar("Estado inicial es el mismo que el Estado final.");
                return false;
            }
            return true;
        }

        #region Botones Angulos default
        private void AnguloHombroArribaAbajoDefaultBtn_Click(object sender, RoutedEventArgs e)
        {
            Angulos.HomroArribaAbajo = AngulosDefault.HomroArribaAbajo;
            SetAngulos();
        }

        private void AnguloCodoArribaAbajoDefaultBtn_Click(object sender, RoutedEventArgs e)
        {
            Angulos.CodoArribaAbajo = AngulosDefault.CodoArribaAbajo;
            SetAngulos();
        }

        private void AnguloHombroAdelanteAtrasDefaultBtn_Click(object sender, RoutedEventArgs e)
        {
            Angulos.HomroAdelanteAtras = AngulosDefault.HomroAdelanteAtras;
            SetAngulos();
        }

        private void AnguloCodoDerechaIzquierdaDefaultBtn_Click(object sender, RoutedEventArgs e)
        {
            Angulos.CodoDerechaIzquierda = AngulosDefault.CodoDerechaIzquierda;
            SetAngulos();
        }
        #endregion
        
        #region Metodos angulos combo box + btns
        private void AnguloHombroArribaAbajoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnguloHombroArribaAbajoComboBox.SelectedItem != null)
            {
                var a = AnguloHombroArribaAbajoComboBox.SelectedItem;
                Angulos.HomroArribaAbajo = Convert.ToInt32(a);
                SetAngulos();
            }
        }

        private void AnguloHombroAdelanteAtrasComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnguloHombroAdelanteAtrasComboBox.SelectedItem != null)
            {
                var a = AnguloHombroAdelanteAtrasComboBox.SelectedItem;
                Angulos.HomroAdelanteAtras = Convert.ToInt32(a);
                SetAngulos();
            }
        }

        private void AnguloCodoArribaAbajoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnguloCodoArribaAbajoComboBox.SelectedItem != null)
            {
                var a = AnguloCodoArribaAbajoComboBox.SelectedItem;
                Angulos.CodoArribaAbajo = Convert.ToInt32(a);
                SetAngulos();
            }
        }

        private void AnguloCodoDerechaIzquierdaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AnguloCodoDerechaIzquierdaComboBox.SelectedItem != null)
            {
                var a = AnguloCodoDerechaIzquierdaComboBox.SelectedItem;
                Angulos.CodoDerechaIzquierda = Convert.ToInt32(a);
                SetAngulos();
            }
        }

        private void GuardarEstadoInicialBtn_Click(object sender, RoutedEventArgs e)
        {
            Ejercicio.EstadoInicial = Angulos.ToString();
            VerEstadoInicialBtn.IsEnabled = true;
            HabilitarBotonGuardar();
        }

        private void GuardarEstadoFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            Ejercicio.EstadoFinal = Angulos.ToString();
            VerEstadoFinalBtn.IsEnabled = true;
            HabilitarBotonGuardar();
        }

        private void VerEstadoInicialBtn_Click(object sender, RoutedEventArgs e)
        {
            Angulos = new AngulosServos(Ejercicio.EstadoInicial);
            SetAngulos();
        }

        private void VerEstadoFinalBtn_Click(object sender, RoutedEventArgs e)
        {
            Angulos = new AngulosServos(Ejercicio.EstadoFinal);
            SetAngulos();
        }
        #endregion

        private void GuardarEjercicioBtn_Click(object sender, RoutedEventArgs e)
        {
            Ejercicio.Nombre = NombreEjercicioTextBox.Text;
            Ejercicio.Descripcion = DescripcionEjercicioTextBox.Text;
            Ejercicio.Dificultad = DificultadRatingBar.Value;
            if (ValidarEjercicio())
            {
                var selectedValue = DestinoComboBox.SelectedValue.ToString();
                if (selectedValue == Destino.FirstOrDefault())
                {
                    var result = ServerHelper.EnviarEjercicio(Ejercicio,null);
                    if (result == ServerHelper.SERVER_OK)
                        CerrarBtn_Click(sender, e);
                    else
                        EstadoSnackBar("No hay conexión para guardar el ejercicio. Intente nuevamente en unos minutos.");
                }
                else
                {
                    var PacienteSeleccionado =
                    Pacientes.Pacientes.FirstOrDefault(
                        x => x.Nombre == selectedValue);
                    var result = ServerHelper.EnviarEjercicio(Ejercicio, PacienteSeleccionado.PacienteId + "");
                    if (result == ServerHelper.SERVER_OK)
                        CerrarBtn_Click(sender, e);
                    else
                        EstadoSnackBar("No hay conexión para guardar el ejercicio. Intente nuevamente en unos minutos.");
                }
            }
        }

        private void DestinoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedValue = DestinoComboBox.SelectedValue.ToString();
            if (selectedValue == Destino.FirstOrDefault())
            {
                // Ejercicio generico
                NombreEjercicioTextBox.IsEnabled = true;
                DificultadRatingBar.IsEnabled = true;
                DescripcionEjercicioTextBox.IsEnabled = true;
                // Seteo de nuevo angulos ejercicio generico
                Ejercicio = EjercicioGenerico;
                Angulos = new AngulosServos(Ejercicio.EstadoInicial);
                SetAngulos();
            }
            else
            {
                // Ejercicio personalizado
                NombreEjercicioTextBox.IsEnabled = false;
                DificultadRatingBar.IsEnabled = false;
                DescripcionEjercicioTextBox.IsEnabled = false;
                // Busco ejercicio personalizado en el servidor
                var PacienteSeleccionado = Pacientes.Pacientes.FirstOrDefault(x => x.Nombre == selectedValue);
                var ejercicio = ServerHelper.ObtenerEjercicioPersonalizado(PacienteSeleccionado, Ejercicio);
                if(ejercicio != null)
                {
                    Ejercicio = ejercicio;
                    Angulos = new AngulosServos(ejercicio.EstadoInicial);
                    SetAngulos();
                }
            }
        }
    }
}
