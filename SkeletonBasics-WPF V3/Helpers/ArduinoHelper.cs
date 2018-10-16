using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Helpers
{
    public static class ArduinoHelper
    {
        static int intentos = 0;
        public static bool EscribirAngulosArduino(SerialPort _serialPort, Double[] angulos)
        {

            try
            {
                if (_serialPort.IsOpen)
                {
                    string rtaAngulos = "*";
                    foreach (double ang in angulos)
                    {
                        string aux = null;
                        aux = ang.ToString("000");

                        rtaAngulos += aux;
                    }
                _serialPort.Write(rtaAngulos);
                Console.WriteLine("rta angulos: " + rtaAngulos);
                }

                intentos = 0;
                return true;
            }
            catch (Exception)
            {
                intentos++;
                if (intentos < 10)
                    EscribirAngulosArduino(_serialPort, angulos);
                else
                    return false;
            }
            return false;
            
        }
    }
}
