using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Helpers
{
    class DatosSesion
    {
        private string token_id;
        private int nro_ejercicio;
        private bool resultado;
        private int desvios;
        private float promedio;

        public string Token_id { get => token_id; set => token_id = value; }
        public int Nro_ejercicio { get => nro_ejercicio; set => nro_ejercicio = value; }
        public bool Resultado { get => resultado; set => resultado = value; }
        public int Desvios { get => desvios; set => desvios = value; }
        public float Promedio { get => promedio; set => promedio = value; }

        public DatosSesion(string token, int nroEj, bool result, int desv, float prom)
        {
            this.token_id = token;
            this.nro_ejercicio = nroEj;
            this.resultado = result;
            this.desvios = desv;
            this.promedio = prom;
        }

        public DatosSesion() { }
        

    }
}
