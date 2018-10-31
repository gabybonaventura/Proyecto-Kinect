using AtaxiaVision.Models;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Controllers
{
    class ArduinoController
    {
        private SerialPort _serialPort;
        int _intentos;
        public List<TensionServos> Tensiones;


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

        public void Inicializar()
        {
            try
            {
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                _serialPort.Open();
                if (_serialPort.IsOpen)
                {
                    _serialPort.Write("*110090040040");
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
                Tensiones.Add(new TensionServos(indata));
            }
        }

        public void CerrarPuerto()
        {
            if(_serialPort.IsOpen)
                _serialPort.Close();
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
