using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class TensionServos
    {
        public int CodoArribaAbajo { get; set; }
        public int CodoIzquierdaDerecha { get; set; }
        public int HombroArribaAbajo { get; set; }
        public int HombroAdelanteAtras { get; set; }

        public TensionServos() { }

        public TensionServos(string datoArduino)
        {
            //el string llega en el formato:
            //A: codoArribaAbajo
            //B: codoIzquierdaDerecha
            //C: hombroArribaAbajo
            //D: hombroAdelanteAtras

            string regexA = @"A\d+";
            string matchA = Regex.Match(datoArduino, regexA).Value;

            string regexB = @"B\d+";
            string matchB = Regex.Match(datoArduino, regexB).Value;

            string regexC = @"C\d+";
            string matchC = Regex.Match(datoArduino, regexC).Value;

            string regexD = @"D\d+";
            string matchD = Regex.Match(datoArduino, regexD).Value;

            string r0 = matchA == "" ? "" : matchA.Substring(1);
            string r1 = matchB == "" ? "" : matchB.Substring(1);
            string r2 = matchC == "" ? "" : matchC.Substring(1);
            string r3 = matchD == "" ? "" : matchD.Substring(1);

            CodoArribaAbajo = TryConvert(r0);
            CodoIzquierdaDerecha = TryConvert(r1);
            HombroArribaAbajo = TryConvert(r2);
            HombroAdelanteAtras = TryConvert(r3);
        }
        
        private int TryConvert(string number)
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

        public override string ToString()
        {
            return "*"
               + CodoArribaAbajo.ToString("D3")
               + CodoIzquierdaDerecha.ToString("D3")
               + HombroArribaAbajo.ToString("D3")
               + HombroAdelanteAtras.ToString("D3");
        }
    }
}
