using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    public class Angulos
    {
        private double codoAA;
        private double codoID;
        private double hombroID;
        private double hombroAA;

        public Angulos(string datoArduino)
        {
            //debo dividir en partes:
            //primero llega un *.
            if (datoArduino[0].Equals("*"))
            {
                int dato1 = datoArduino.IndexOf("A");
                int dato2 = datoArduino.IndexOf("B");
                int dato3 = datoArduino.IndexOf("C");
                int dato4 = datoArduino.IndexOf("D");

                codoAA = double.Parse(datoArduino.Substring(dato1, dato2));
                codoID = double.Parse(datoArduino.Substring(dato2, dato3));
                hombroID = double.Parse(datoArduino.Substring(dato3, dato4));
                hombroAA = double.Parse(datoArduino.Substring(dato4));
            }
            
        }

        public double HombroAA { get => hombroAA; set => hombroAA = value; }
        public double HombroID { get => hombroID; set => hombroID = value; }
        public double CodoID { get => codoID; set => codoID = value; }
        public double CodoAA { get => codoAA; set => codoAA = value; }
    }
}
