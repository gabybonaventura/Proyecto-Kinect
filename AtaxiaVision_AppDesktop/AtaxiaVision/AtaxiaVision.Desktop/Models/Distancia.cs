using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class Distancia
    {
        public float DistanciaManoCodo { get; set; }
        public float DistanciaManoHombro { get; set; }
        public float DistanciaHombroCodo { get; set; }
        public float DistanciaHombroObjeto { get; set; }
        public float DistanciaHombroAux { get; set; }
        public float DistanciaObjetoAux { get; set; }


        public Distancia()
        {

        }
    }
}
