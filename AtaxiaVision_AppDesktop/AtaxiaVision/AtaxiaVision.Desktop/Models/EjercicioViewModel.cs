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

        public Exercise ToExercise()
        {
            return new Exercise()
            {
                name = Nombre,
                description = Descripcion,
                difficulty = Dificultad,
                endingState = EstadoFinal,
                initialState = EstadoInicial
            };
        }
    }

    public class Exercise
    {
        public string description;
        public int difficulty;
        public string endingState;
        public string initialState;
        public string name;

        public Exercise() { }

        public EjercicioViewModel ToEjercicioViewModel()
        {
            return new EjercicioViewModel()
            {
                Nombre = name,
                Descripcion = description,
                Dificultad = difficulty,
                EstadoInicial = initialState,
                EstadoFinal = endingState
            };
        }

    }
}
