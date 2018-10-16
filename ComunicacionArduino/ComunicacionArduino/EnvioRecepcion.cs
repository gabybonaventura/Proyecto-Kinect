using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
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
                /*
                if(EnvioRecepcion.Lineas != 10)
                {
                    CadenaRecibida += indata;
                    
                }
                */
            }
        }

        private void MostrarConsumos(string consumos)
        {
            string[] consumo = CortarString(consumos);
            /*
            Servo CodoArribaAbajoServo;
            Servo CodoIzquierdaDerechaServo;
            Servo HombroArribaAbajoServo;
            Servo HombroAdelanteAtrasServo;
             */
            SetConsumoCodoArribaAbajo(Convert.ToInt32(consumo[0]));
            SetConsumoCodoDerechaIzquierda(Convert.ToInt32(consumo[1]));
            SetConsumoHombroArribaAbajo(Convert.ToInt32(consumo[2]));
            SetConsumoHombroAdelanteAtras(Convert.ToInt32(consumo[3]));

            HistorialConsumo(consumo);
        }

        private void HistorialConsumo(string[] consumo)
        {
            HistorialConsumo_txtArea.AppendText(
                "H.ArAb:   " + Convert.ToInt32(consumo[2]).ToString("D3")
                + "   |   H.AdAt:   " + Convert.ToInt32(consumo[3]).ToString("D3")
                + "   |   C.ArAb:   " + Convert.ToInt32(consumo[0]).ToString("D3")
                + "   |   C.DI:   " + Convert.ToInt32(consumo[1]).ToString("D3")
                + "\n"
                ); 
        }

        private Color CalcularColor(int c)
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
            ConsumoCodoArribaAbajo_lbl.Text = "Consumo: " + consumo.ToString("D3") + " mA";
        }

        private void SetConsumoCodoDerechaIzquierda(int consumo)
        {
            ConsumoCodoDerechaIzquierda_lbl.ForeColor = CalcularColor(consumo);
            ConsumoCodoDerechaIzquierda_lbl.Text = "Consumo: " + consumo.ToString("D3") + " mA";
        }

        private void SetConsumoHombroAdelanteAtras(int consumo)
        {
            ConsumoHombroAdelanteAtras_lbl.ForeColor = CalcularColor(consumo);
            ConsumoHombroAdelanteAtras_lbl.Text = "Consumo: " + consumo.ToString("D3") + " mA";
        }

        private void SetConsumoHombroArribaAbajo(int consumo)
        {
            ConsumoHombroArribaAbajo_lbl.ForeColor = CalcularColor(consumo);
            ConsumoHombroArribaAbajo_lbl.Text = "Consumo: " + consumo.ToString("D3") + " mA";
        }

        private static string[] CortarString(string cadena)
        {
            string[] result = new string[4];
            result[0] = cadena.Split('A').Last().Split('B').FirstOrDefault();
            result[1] = cadena.Split('B').Last().Split('C').FirstOrDefault();
            result[2] = cadena.Split('C').Last().Split('D').FirstOrDefault();
            result[3] = cadena.Split('D').Last();
            return result;
        }

        private void AngulosDefault_btn_Click(object sender, EventArgs e)
        {
            SetAnguloCodoArribaAbajo(20,false);
            SetAnguloCodoDerechaIzquierda(90, false);
            SetAnguloHombroArribaAbajo(40, false);
            SetAnguloHombroAdelanteAtras(40);
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
            AnguloCodoArribaAbajo = angulo;
            AnguloCodoArribaAbajo_txt.Text = angulo.ToString();
            if(imprime)
                ImprimirAngulosAEnviar();
        }

        private void SetAnguloCodoDerechaIzquierda(int angulo, bool imprime = true)
        {
            AnguloCodoDerechaIzquierda = angulo;
            AnguloCodoDerechaIzquierda_txt.Text = angulo.ToString();
            if (imprime)
                ImprimirAngulosAEnviar();
        }

        private void SetAnguloHombroArribaAbajo(int angulo, bool imprime = true)
        {
            AnguloHombroArribaAbajo = angulo;
            AnguloHombroArribaAbajo_txt.Text = angulo.ToString();
            if (imprime)
                ImprimirAngulosAEnviar();
        }

        private void SetAnguloHombroAdelanteAtras(int angulo, bool imprime = true)
        {
            AnguloHombroAdelanteAtras = angulo;
            AnguloHombroAdelanteAtras_txt.Text = angulo.ToString();
            if (imprime)
                ImprimirAngulosAEnviar();
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
                int angulo = Convert.ToInt32(AnguloHombroArribaAbajo_txt.Text);
                SetAnguloHombroArribaAbajo(angulo);
            }
        }

        private void AnguloHombroAdelanteAtras_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int angulo = Convert.ToInt32(AnguloHombroAdelanteAtras_txt.Text);
                SetAnguloHombroAdelanteAtras(angulo);
            }
        }

        private void AnguloCodoArribaAbajo_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int angulo = Convert.ToInt32(AnguloCodoArribaAbajo_txt.Text);
                SetAnguloCodoArribaAbajo(angulo);
            }
        }

        private void AnguloCodoDerechaIzquierda_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int angulo = Convert.ToInt32(AnguloCodoDerechaIzquierda_txt.Text);
                SetAnguloCodoDerechaIzquierda(angulo);
            }
        }
    }
}
