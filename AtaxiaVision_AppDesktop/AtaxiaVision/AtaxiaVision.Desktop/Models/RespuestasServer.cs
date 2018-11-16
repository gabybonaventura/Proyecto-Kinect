using AtaxiaVision.Helpers;
using Newtonsoft.Json;
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
            Paciente = new Patient();
            Ejercicio = new Exercise();
        }
    }
   
    public class RespuestaListaEjercicios
    {
        public List<Exercise> Ejercicios { get; set; }

        public RespuestaListaEjercicios()
        {
            Ejercicios = new List<Exercise>();
        }
    }

    public class RespuestaListaPacientes
    {
        public List<Patient> Pacientes { get; set; }

        public RespuestaListaPacientes()
        {
            Pacientes = new List<Patient>();
        }
    }

    #region Modelos Server
    public class Patient
    {
        public int Edad { get; set; }
        public string FechaInicio { get; set; }
        public int PacienteId { get; set; }
        public string Nombre { get; set; }

        public Patient() { }

        public Patient(dynamic patient)
        {  
            Edad = Convert.ToInt32(patient.age);
            FechaInicio = patient.beginDate;
            PacienteId = Convert.ToInt32(patient.idPatient);
            Nombre = patient.name;
        }
    }
    public class Exercise
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }
        public int Dificultad { get; set; }
        public string EstadoInicial { get; set; }
        public string EstadoFinal { get; set; }
        public string Nombre { get; set; }

        public Exercise() { }
        public Exercise(string id, dynamic exercise)
        {
            ID = id;
            Descripcion = exercise.description;
            Dificultad = Convert.ToInt32(exercise.difficulty);
            EstadoInicial = exercise.initialState;
            EstadoFinal = exercise.endingState;
            Nombre = exercise.name;
        }

        public ExerciseModelServer ConvertToModelServer()
        {
            return new ExerciseModelServer
            {
                description = Descripcion,
                difficulty = Dificultad,
                endingState = EstadoFinal,
                initialState = EstadoInicial,
                name = Nombre
            };
        }
    }

    public class ExerciseModelServer
    {
        public string description;
        public int difficulty;
        public string endingState;
        public string initialState;
        public string name;
    }

    public class ExerciseCustomModelServer
    {
        public string endingState;
        public string intialState;
    }

    public class Comments
    {
        public string comment;
    }
    #endregion
    
  
}
