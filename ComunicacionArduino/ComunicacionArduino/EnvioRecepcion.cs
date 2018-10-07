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
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            if(!string.IsNullOrEmpty(indata))
            {
                Console.WriteLine(indata);
                /*
                if(EnvioRecepcion.Lineas != 10)
                {
                    CadenaRecibida += indata;
                    
                }
                */
            }
        }

    }
}
