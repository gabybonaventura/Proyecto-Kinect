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
        public double HombroAuxArribaAbajo { get; set; }
        public double HombroObjArribaAbajo { get; set; }
        public double HombroAuxAtrasAdelante { get; set; }
        public double HombroObjAtrasAdelante { get; set; }

        public Angulos()
        {
            CodoArribaAbajo = -1;
            CodoIzquierdaDerecha = -1;
            HombroArribaAbajo = -1;
            HombroAdelanteAtras = -1;
            HombroAuxArribaAbajo = -1;
            HombroObjArribaAbajo = -1;
        }


    }
}
