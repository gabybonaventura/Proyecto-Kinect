using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class Puntos
    {
        public Joint Hombro { get; set; }
        public Joint Codo { get; set; }
        public Joint Mano { get; set; }
        public SkeletonPoint Objeto { get; set; }
        public SkeletonPoint PuntoAuxHombroArribaAbajo { get; set; }

        public Puntos() { }
        
        public Puntos(Joint hombro, Joint codo, Joint mano, SkeletonPoint objeto)
        {
            Hombro = hombro;
            Codo = codo;
            Mano = mano;
            Objeto = objeto;
        }
    }
}
