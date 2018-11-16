using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class AngulosServos
    {
        public int CodoArribaAbajo { get; set; }
        public int CodoDerechaIzquierda { get; set; }
        public int HomroArribaAbajo { get; set; }
        public int HomroAdelanteAtras { get; set; }

        public AngulosServos() { }

        public AngulosServos(string angulos)
        {
            if(angulos != null && angulos.Length == 13)
            {
                try
                {
                    // "*110 090 040 040"
                    CodoArribaAbajo = Convert.ToInt32(angulos.Substring(1, 3));
                    CodoDerechaIzquierda = Convert.ToInt32(angulos.Substring(4, 3));
                    HomroArribaAbajo = Convert.ToInt32(angulos.Substring(7, 3));
                    HomroAdelanteAtras = Convert.ToInt32(angulos.Substring(10, 3));
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public override string ToString()
        {
            return "*"
                + CodoArribaAbajo.ToString("D3")
                + CodoDerechaIzquierda.ToString("D3")
                + HomroArribaAbajo.ToString("D3")
                + HomroAdelanteAtras.ToString("D3");
        }
    }
}
