using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class EjercicioViewModel
    {
        public string Nombre { get; set; }
        public int Dificultad { get; set; }
        public string Descripcion { get; set; }
        public string EstadoInicial { get; set; }
        public string EstadoFinal { get; set; }
    }
}
