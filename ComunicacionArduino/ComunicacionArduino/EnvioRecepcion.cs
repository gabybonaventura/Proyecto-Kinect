using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComunicacionArduino
{
    public partial class EnvioRecepcion : Form
    {
        private SerialPort _serialPort;
        static string CadenaRecibida = "Mediciones:\n";
        static int Lineas = 1;
        private int defasaje = 10;

        public int AnguloHombroArribaAbajo { get; set; }
        public int AnguloHombroAdelanteAtras { get; set; }
        public int AnguloCodoArribaAbajo { get; set; }
        public int AnguloCodoDerechaIzquierda { get; set; }

        // Delegates para los consumos
        public delegate void DelegateHistorialConsumos(int[] consumos);
        public DelegateHistorialConsumos delegateHistorialConsumo;

        public delegate void DelegateConsumoHombroArribaAbajo(int consumo);
        public DelegateConsumoHombroArribaAbajo delegateConsumoHombroArribaAbajo;

        public delegate void DelegateConsumoHombroAdelanteAtras(int consumo);
        public DelegateConsumoHombroAdelanteAtras delegateConsumoHombroAdelanteAtras;

        public delegate void DelegateConsumoCodoArribaAbajo(int consumo);
        public DelegateConsumoCodoArribaAbajo delegateConsumoCodoArribaAbajo;

        public delegate void DelegateConsumoCodoDerechaIzquierda(int consumo);
        public DelegateConsumoCodoDerechaIzquierda delegateConsumoCodoDerechaIzquierda;

        public EnvioRecepcion()
        {
            InitializeComponent();
        }

        public EnvioRecepcion(SerialPort serialPort)
        {
            InitializeComponent();
            _serialPort = serialPort;
        }

        private void EnvioRecepcion_Load(object sender, EventArgs e)
        {
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _serialPort.Open();
            delegateHistorialConsumo = new DelegateHistorialConsumos(AgregarConsumosAHistorial);
            delegateConsumoHombroArribaAbajo = new DelegateConsumoHombroArribaAbajo(SetConsumoHombroArribaAbajo);
            delegateConsumoHombroAdelanteAtras = new DelegateConsumoHombroAdelanteAtras(SetConsumoHombroAdelanteAtras);
            delegateConsumoCodoArribaAbajo = new DelegateConsumoCodoArribaAbajo(SetConsumoCodoArribaAbajo);
            delegateConsumoCodoDerechaIzquierda = new DelegateConsumoCodoDerechaIzquierda(SetConsumoCodoDerechaIzquierda);
        }

        public void AgregarConsumosAHistorial(int[] consumos)
        {
            string c2 = consumos[2].ToString("000");
            string c3 = consumos[3].ToString("000");
            string c0 = consumos[0].ToString("000");
            string c1 = consumos[1].ToString("000");
            string cadena = "H.ArAb:   " + c2
                + "   |   H.AdAt:   " + c3
                + "   |   C.ArAb:   " + c0
                + "   |   C.DI:   " + c1
                + "\n";
            HistorialConsumo_txtArea.AppendText(cadena);
        }
        
        private void EnviarButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EnviarTextBox.Text))
                _serialPort.Write(EnviarTextBox.Text);
            else
                AlertaLabel.Text = "Debe ingresar texto a enviar";
            
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if(!string.IsNullOrEmpty(indata))
            {
                Console.WriteLine(indata);
                MostrarConsumos(indata);
            }
        }

        private void MostrarConsumos(string consumoString)
        {
            /*
            Servo CodoArribaAbajoServo;
            Servo CodoIzquierdaDerechaServo;
            Servo HombroArribaAbajoServo;
            Servo HombroAdelanteAtrasServo;
             */
            int[] consumos = CortarString(consumoString);
            try
            {
                ConsumoCodoArribaAbajo_lbl.Invoke(delegateConsumoCodoArribaAbajo, consumos[0]);
                ConsumoCodoDerechaIzquierda_lbl.Invoke(delegateConsumoCodoDerechaIzquierda, consumos[1]);
                ConsumoHombroArribaAbajo_lbl.Invoke(delegateConsumoHombroArribaAbajo, consumos[2]);
                ConsumoHombroAdelanteAtras_lbl.Invoke(delegateConsumoHombroAdelanteAtras, consumos[3]);
                HistorialConsumo_txtArea.Invoke(delegateHistorialConsumo, consumos);
            }
            catch (Exception)
            {
            }
        }

        private Brus CalcularColor(int c)
        {
            if (c < 100)
                return Color.Blue;
            if (c < 200)
                return Color.Green;
            if (c < 500)
                return Color.Orange;
            return Color.Red;
        }

        private void SetConsumoCodoArribaAbajo(int consumo)
        {
            ConsumoCodoArribaAbajo_lbl.ForeColor = CalcularColor(consumo);
            ConsumoCodoArribaAbajo_lbl.Text = "Consumo: " + consumo.ToString("000") + " mA";
        }

        private void SetConsumoCodoDerechaIzquierda(int consumo)
        {
            ConsumoCodoDerechaIzquierda_lbl.ForeColor = CalcularColor(consumo);
            ConsumoCodoDerechaIzquierda_lbl.Text = "Consumo: " + consumo.ToString("000") + " mA";
        }

        private void SetConsumoHombroAdelanteAtras(int consumo)
        {
            ConsumoHombroAdelanteAtras_lbl.ForeColor = CalcularColor(consumo);
            ConsumoHombroAdelanteAtras_lbl.Text = "Consumo: " + consumo.ToString("000") + " mA";
        }

        private void SetConsumoHombroArribaAbajo(int consumo)
        {
            ConsumoHombroArribaAbajo_lbl.ForeColor = CalcularColor(consumo);
            ConsumoHombroArribaAbajo_lbl.Text = "Consumo: " + consumo.ToString("000") + " mA";
        }

        private static int[] CortarString(string cadena)
        {
            int[] result = new int[4];

            string regexA = @"A\d+";
            string matchA = Regex.Match(cadena, regexA).Value;

            string regexB = @"B\d+";
            string matchB = Regex.Match(cadena, regexB).Value;

            string regexC = @"C\d+";
            string matchC = Regex.Match(cadena, regexC).Value;

            string regexD = @"D\d+";
            string matchD = Regex.Match(cadena, regexD).Value;

            string r0 = matchA == "" ? "" : matchA.Substring(1);
            string r1 = matchB == "" ? "" : matchB.Substring(1);
            string r2 = matchC == "" ? "" : matchC.Substring(1);
            string r3 = matchD == "" ? "" : matchD.Substring(1);

            result[0] = TryConvert(r0);
            result[1] = TryConvert(r1);
            result[2] = TryConvert(r2);
            result[3] = TryConvert(r3);
            return result;
        }

        private static int TryConvert(string number)
        {
            int n = 0;
            try
            {
                n = Convert.ToInt32(number);
                if (n > 999)
                    n = 999;
            }
            catch (Exception)
            {
                n = 0;
            }
            return n;
        }

        private void AngulosDefault_btn_Click(object sender, EventArgs e)
        {
            SetAnguloCodoArribaAbajo(110,false);
            SetAnguloCodoDerechaIzquierda(90, false);
            SetAnguloHombroArribaAbajo(40, false);
            SetAnguloHombroAdelanteAtras(40);
            HabilitarBotones();
        }

        private void ImprimirAngulosAEnviar()
        {
            /*
            Servo CodoArribaAbajoServo;
            Servo CodoIzquierdaDerechaServo;
            Servo HombroArribaAbajoServo;
            Servo HombroAdelanteAtrasServo;
             */
            string cadena =  "*" 
                + AnguloCodoArribaAbajo.ToString("D3")
                + AnguloCodoDerechaIzquierda.ToString("D3")
                + AnguloHombroArribaAbajo.ToString("D3")
                + AnguloHombroAdelanteAtras.ToString("D3");
            AngulosAEnviar_lbl.Text = "Enviando: "
                + cadena;
            EscribirSerial(cadena);
        }

        private void EscribirSerial(string cadena)
        {
            try
            {
                _serialPort.Write(cadena);
                SerialOk(cadena);
            }
            catch (Exception ex)
            {
                SerialError(cadena, ex.Message);
            }
            //MostrarConsumos("A11B222C333D4444");
        }

        private void SerialOk(string cadena)
        {
            EstadoEnvio_lbl.Text = "Estado envio: [" + cadena + "] Ok.";
            EstadoEnvio_lbl.ForeColor = Color.Green;
        }

        private void SerialError(string cadena, string error)
        {
            EstadoEnvio_lbl.Text = "Estado envio: [" + cadena + "] " + error;
            EstadoEnvio_lbl.ForeColor = Color.Red;
        }

        private void SetAnguloCodoArribaAbajo(int angulo, bool imprime = true)
        {
            int anguloValidado = ValidarAngulo(angulo);
            AnguloCodoArribaAbajo = anguloValidado;
            AnguloCodoArribaAbajo_txt.Text = anguloValidado.ToString();
            if(imprime)
                ImprimirAngulosAEnviar();
        }

        private void SetAnguloCodoDerechaIzquierda(int angulo, bool imprime = true)
        {
            int anguloValidado = ValidarAngulo(angulo);
            AnguloCodoDerechaIzquierda = anguloValidado;
            AnguloCodoDerechaIzquierda_txt.Text = anguloValidado.ToString();
            if (imprime)
                ImprimirAngulosAEnviar();
        }

        private void SetAnguloHombroArribaAbajo(int angulo, bool imprime = true)
        {
            int anguloValidado = ValidarAngulo(angulo);
            AnguloHombroArribaAbajo = anguloValidado;
            AnguloHombroArribaAbajo_txt.Text = anguloValidado.ToString();
            if (imprime)
                ImprimirAngulosAEnviar();
        }

        private void SetAnguloHombroAdelanteAtras(int angulo, bool imprime = true)
        {
            int anguloValidado = ValidarAngulo(angulo);
            AnguloHombroAdelanteAtras = anguloValidado;
            AnguloHombroAdelanteAtras_txt.Text = anguloValidado.ToString();
            if (imprime)
                ImprimirAngulosAEnviar();
        }

        private int ValidarAngulo(int angulo)
        {
            if (angulo < 0)
                return 0;
            if (angulo > 180)
                return 180;
            return angulo;
        }

        private void AnguloHombroArribaAbajo_txt_TextChanged(object sender, EventArgs e)
        {
            //int angulo = Convert.ToInt32(AnguloCodoArribaAbajo_txt.Text);
            //SetAnguloCodoArribaAbajo(angulo);
        }

        private void AnguloHombroAdelanteAtras_txt_TextChanged(object sender, EventArgs e)
        {
            //int angulo = Convert.ToInt32(AnguloHombroAdelanteAtras_txt.Text);
            //SetAnguloHombroAdelanteAtras(angulo);
        }

        private void AnguloCodoArribaAbajo_txt_TextChanged(object sender, EventArgs e)
        {
            //int angulo = Convert.ToInt32(AnguloCodoArribaAbajo_txt.Text);
            //SetAnguloCodoArribaAbajo(angulo);
        }

        private void AnguloCodoDerechaIzquierda_txt_TextChanged(object sender, EventArgs e)
        {
            //int angulo = Convert.ToInt32(AnguloCodoDerechaIzquierda_txt.Text);
            //SetAnguloCodoDerechaIzquierda(angulo);
        }

        private void HombroArribaAbajoMenos_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloHombroArribaAbajo_txt.Text);
            SetAnguloHombroArribaAbajo(angulo - defasaje);
        }

        private void HombroArribaAbajoMas_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloHombroArribaAbajo_txt.Text);
            SetAnguloHombroArribaAbajo(angulo + defasaje);
        }

        private void HombroAdelanteAtrasMas_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloHombroAdelanteAtras_txt.Text);
            SetAnguloHombroAdelanteAtras(angulo + defasaje);
        }

        private void HombroAdelanteAtrasMenos_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloHombroAdelanteAtras_txt.Text);
            SetAnguloHombroAdelanteAtras(angulo - defasaje);
        }

        private void CodoArribaAbajoMenos_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloCodoArribaAbajo_txt.Text);
            SetAnguloCodoArribaAbajo(angulo - defasaje);
        }

        private void CodoArribaAbajoMas_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloCodoArribaAbajo_txt.Text);
            SetAnguloCodoArribaAbajo(angulo + defasaje);
        }

        private void CodoDerechaIzquierdaMenos_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloCodoDerechaIzquierda_txt.Text);
            SetAnguloCodoDerechaIzquierda(angulo - defasaje);
        }

        private void CodoDerechaIzquierdaMas_btn_Click(object sender, EventArgs e)
        {
            int angulo = Convert.ToInt32(AnguloCodoDerechaIzquierda_txt.Text);
            SetAnguloCodoDerechaIzquierda(angulo + defasaje);
        }

        private void AnguloHombroArribaAbajo_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int angulo = Convert.ToInt32(AnguloHombroArribaAbajo_txt.Text);
                    SetAnguloHombroArribaAbajo(angulo);
                }
                catch (Exception)
                {
                    SetAnguloHombroArribaAbajo(0);
                }
            }
        }

        private void AnguloHombroAdelanteAtras_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int angulo = Convert.ToInt32(AnguloHombroAdelanteAtras_txt.Text);
                    SetAnguloHombroAdelanteAtras(angulo);
                }
                catch (Exception)
                {
                    SetAnguloHombroAdelanteAtras(0);
                }
                
            }
        }

        private void AnguloCodoArribaAbajo_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int angulo = Convert.ToInt32(AnguloCodoArribaAbajo_txt.Text);
                    SetAnguloCodoArribaAbajo(angulo);
                }
                catch (Exception)
                {
                    SetAnguloCodoArribaAbajo(0);
                }
                
            }
        }

        private void AnguloCodoDerechaIzquierda_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    int angulo = Convert.ToInt32(AnguloCodoDerechaIzquierda_txt.Text);
                    SetAnguloCodoDerechaIzquierda(angulo);
                }
                catch (Exception)
                {
                    SetAnguloCodoDerechaIzquierda(0);
                }
                
            }
        }
        

        private void HabilitarBotones()
        {
            AnguloHombroArribaAbajo_txt.Enabled = true;
            AnguloHombroAdelanteAtras_txt.Enabled = true;
            AnguloCodoArribaAbajo_txt.Enabled = true;
            AnguloCodoDerechaIzquierda_txt.Enabled = true;

            HombroArribaAbajoMenos_btn.Enabled = true;
            HombroArribaAbajoMas_btn.Enabled = true;
            HombroAdelanteAtrasMenos_btn.Enabled = true;
            HombroAdelanteAtrasMas_btn.Enabled = true;
            CodoDerechaIzquierdaMas_btn.Enabled = true;
            CodoDerechaIzquierdaMenos_btn.Enabled = true;
            CodoArribaAbajoMenos_btn.Enabled = true;
            CodoArribaAbajoMas_btn.Enabled = true;

            AngulosDefault_btn.Text = "Angulos default";
        }
    }
}
