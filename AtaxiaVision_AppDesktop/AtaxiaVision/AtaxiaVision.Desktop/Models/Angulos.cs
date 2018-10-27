using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class Angulos
    {
        public double CodoArribaAbajo { get; set; }
        public double CodoIzquierdaDerecha { get; set; }
        public double HombroArribaAbajo { get; set; }
        public double HombroAdelanteAtras { get; set; }

        public Angulos()
        {
            CodoArribaAbajo = -1;
            CodoIzquierdaDerecha = -1;
            HombroArribaAbajo = -1;
            HombroAdelanteAtras = -1;
        }


    }
}
