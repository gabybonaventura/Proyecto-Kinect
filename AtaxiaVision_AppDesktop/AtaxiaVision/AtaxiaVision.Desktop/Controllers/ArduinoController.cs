using AtaxiaVision.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Controllers
{
    public class ArduinoController
    {
        private SerialPort _serialPort;
        int _intentos;
        public List<TensionServos> Tensiones;
        public TensionServos UltimaTension;
        public const string BRAZO_GB = "*110090040040";
        public const string BRAZO_CC = "*140090050030";
        public const string BRAZO_MATIVEGA = "*140090040050";

        //Seteo con valores por default el constructor
        public ArduinoController(int baudRate = 9600, 
            int dataBits = 8, 
            Handshake handshake = Handshake.None,
            Parity parity = Parity.None,
            string portName = "COM5",
            StopBits stopBits = StopBits.One)
        {
            Tensiones = new List<TensionServos>();
            _serialPort = new SerialPort()
            {
                BaudRate = baudRate,
                DataBits = dataBits,
                Handshake = handshake,
                Parity = parity,
                PortName = portName,
                StopBits = stopBits
            };
            _intentos = 0;
        }

        public void Inicializar(string inicio = BRAZO_GB)
        {
            try
            {
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                _serialPort.Open();
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write(inicio);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Console.WriteLine(indata);
            if (!string.IsNullOrEmpty(indata))
            {
                UltimaTension = new TensionServos(indata);
                Tensiones.Add(UltimaTension);
            }
        }

        public void CerrarPuerto()
        {
            if(_serialPort.IsOpen)
                _serialPort.Close();
        }

        public bool EnviarAngulosFromAngulosServos(AngulosServos angulos)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write(angulos.ToString());
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool EscribirAngulosArduino(Angulos angulos)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    string rtaAngulos = "*";

                    rtaAngulos += angulos.CodoArribaAbajo.ToString("000");
                    rtaAngulos += angulos.CodoIzquierdaDerecha.ToString("000");
                    rtaAngulos += angulos.HombroArribaAbajo.ToString("000");
                    rtaAngulos += angulos.HombroAdelanteAtras.ToString("000");

                    _serialPort.Write(rtaAngulos);
                    //    Console.WriteLine("Angulo Codo Arriba Abajo : " + angulos[0]);
                    //    Console.WriteLine("Angulo Codo Izq Der: " + angulos[1]);
                    //    Console.WriteLine("Angulo Hombro Arriba Abajo: " + angulos[2]);
                    //    Console.WriteLine("Angulo Hombro Adelante Atras: " + angulos[3]);
                    //
                }

                _intentos = 0;
                return true;
            }
            catch (Exception)
            {
                _intentos++;
                if (_intentos < 10)
                    EscribirAngulosArduino(angulos);
                else
                    return false;
            }
            return false;

        }

        
    }
}
