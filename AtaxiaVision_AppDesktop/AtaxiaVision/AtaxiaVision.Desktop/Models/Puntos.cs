using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtaxiaVision.Models
{
    public class Puntos
    {
        public Joint Hombro { get; set; }
        public Joint Codo { get; set; }
        public Joint Mano { get; set; }
        public SkeletonPoint Objeto { get; set; }
        public Point Hombro2d { get; set; }
        public Point Codo2d { get; set; }
        public Point Mano2d { get; set; }
        public Point Objeto2d { get; set; }
        public Point PuntoAuxHombroArribaAbajo { get; set; }
        public Point PuntoAuxHombroAtrasAdelante { get; set; }

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
