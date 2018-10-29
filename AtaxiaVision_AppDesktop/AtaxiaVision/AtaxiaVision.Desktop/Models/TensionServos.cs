using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class TensionServos
    {
        public double CodoArribaAbajo { get; set; }
        public double CodoIzquierdaDerecha { get; set; }
        public double HombroArribaAbajo { get; set; }
        public double HombroAdelanteAtras { get; set; }

        public TensionServos() { }

        public TensionServos(string datoArduino)
        {
            //el string llega en el formato:
            //A: codoArribaAbajo
            //B: codoIzquierdaDerecha
            //C: hombroArribaAbajo
            //D: hombroAdelanteAtras

            //primero llega un *
            if (datoArduino[0].Equals("*"))
            {
                //obtengo el index de las letras:
                int indexA = datoArduino.IndexOf("A");
                int indexB = datoArduino.IndexOf("B");
                int indexC = datoArduino.IndexOf("C");
                int indexD = datoArduino.IndexOf("D");

                CodoArribaAbajo = double.Parse(datoArduino.Substring(indexA, indexB));
                CodoIzquierdaDerecha = double.Parse(datoArduino.Substring(indexB, indexC));
                HombroArribaAbajo = double.Parse(datoArduino.Substring(indexC, indexD));
                HombroAdelanteAtras = double.Parse(datoArduino.Substring(indexD));
            }
        }
    }
}
