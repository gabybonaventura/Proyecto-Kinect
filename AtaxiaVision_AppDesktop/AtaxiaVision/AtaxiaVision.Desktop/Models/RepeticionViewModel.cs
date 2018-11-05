using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class RepeticionViewModel
    {
        public string Token { get; set; }
        public int Ejercicio { get; set; }
        public bool FinalizoConExito { get; set; }
        public int Desvios { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Duracion { get; set; }
    }
}