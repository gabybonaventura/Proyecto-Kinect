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
            //_serialPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
            _serialPort.Open();
        }

        private void EnviarButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(EnviarTextBox.Text))
                _serialPort.Write(EnviarTextBox.Text);
            else
                AlertaLabel.Text = "Debe ingresar texto a enviar";
        }
        
        
    }
}
