using AtaxiaVision.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class RespuestaToken
    {
        public int CodigoTokenValid { get; set; }
        public int Repeticiones { get; set; }
        public Patient Paciente { get; set; }
        public Exercise Ejercicio { get; set; }

        public RespuestaToken()
        {
            CodigoTokenValid = ServerHelper.TOKEN_SINCONEXION;
        }
    }


    #region Modelos base
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

    public class Exercise
    {
        public string Descripcion { get; set; }
        public int Dificultad { get; set; }
        public string EstadoInicial { get; set; }
        public string EstadoFinal { get; set; }
        public string Nombre { get; set; }

        public Exercise(dynamic exercise)
        {
            Descripcion = exercise.description;
            Dificultad = Convert.ToInt32(exercise.difficulty);
            EstadoInicial = exercise.initialState;
            EstadoFinal = exercise.endingState;
            Nombre = exercise.name;
        }
    }
    #endregion
}
