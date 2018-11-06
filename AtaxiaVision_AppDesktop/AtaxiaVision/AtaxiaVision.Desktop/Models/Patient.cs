using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class Patient
    {
        public int Edad { get; set; }
        public DateTime FechaInicio { get; set; }
        public int PacienteId { get; set; }
        public string Nombre { get; set; }

        public Patient(dynamic patient)
        {
            Edad = Convert.ToInt32(patient.age);
            FechaInicio = DateTime.Now;
            PacienteId = Convert.ToInt32(patient.idPatient);
            Nombre = patient.name;
        }
    }

    
}
