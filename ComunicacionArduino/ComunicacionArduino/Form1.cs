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
    public partial class ComunicacionArduinoForm : Form
    {
        private SerialPort _serialPort = new SerialPort();
        private int _baudRate = 9600;
        private int _dataBits = 8;
        private Handshake _handshake = Handshake.None;
        private Parity _parity = Parity.None;
        private string _portName = "COM5";
        private StopBits _stopBits = StopBits.One;
        public ComunicacionArduinoForm()
        {
            InitializeComponent();
        }

        private void SincronizarButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ComLabel.Text))
            {
                AlertaLabel.Text = "Debe ingresar el COM del arduino";
                return;
            }
            if (string.IsNullOrEmpty(BaudioLabel.Text))
            {
                AlertaLabel.Text = "Debe ingresar los baudios del arduino";
                return;
            }

            try
            {
                _baudRate = int.Parse(BaudiosTextBox.Text);
                _portName = ComTextBox.Text;

                _serialPort.BaudRate = _baudRate;
                _serialPort.DataBits = _dataBits;
                _serialPort.Handshake = _handshake;
                _serialPort.Parity = _parity;
                _serialPort.PortName = _portName;
                _serialPort.StopBits = _stopBits;
                EnvioRecepcion envioRecepcion = new EnvioRecepcion(_serialPort);
                envioRecepcion.Show();
            }
            catch (Exception ex)
            {
                AlertaLabel.Text = "Error al sincronizar con Arduino";
                Console.WriteLine(ex.Message);
                return;
            }
        }

    }
}
