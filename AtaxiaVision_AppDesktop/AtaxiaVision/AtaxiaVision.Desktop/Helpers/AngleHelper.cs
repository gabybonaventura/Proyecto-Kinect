using System;
using AtaxiaVision.Models;
using Microsoft.Kinect;

namespace AtaxiaVision.Helpers
{
    class AngleHelper
    {
        private Angulos angulos;
        private Distancia distancia;
        private Puntos puntos;

        public Angulos SetValorAngulos(Puntos puntos)
        {
            this.puntos = puntos;
            angulos = new Angulos();

            //calculo distancias para obtener el ANGULO del CODO ARRIBA ABAJO
            CalcularAnguloCodoArribaAbajo();


            
            if (distancia.DistanciaHombroObjeto > distancia.DistanciaHombroCodo
                + distancia.DistanciaManoCodo)
                return angulos;

            //calculo del angulo hombro ATRAS ADELANTE:

            //Calculo del angulo hombro ARRIBA ABAJO:
            CalcularAnguloHombroArribaAbajo();
            
            return angulos;
            /*
            double[] array = new double[4];
            array[0] = -1;
            array[1] = -1;
            array[2] = -1;
            array[3] = -1;
            //punto auxiliar en el torso para calcular el ángulo del hombro 
            //en el movimiento arriba - abajo.

            SkeletonPoint puntoTorso = new SkeletonPoint();
            puntoTorso.X = hombro.Position.X;
            puntoTorso.Y = codo.Position.Y;
            puntoTorso.Z = hombro.Position.Z;

            //calculamos la distancia entre el objeto y el hombro.
            float distHombroObj = DistanceHelper.ObtenerDistancia(hombro, skelObjeto);

            //calculamos la distancia entre las articulaciones:
            //distancia entre hombro y mano
            float distHombroMano = DistanceHelper.ObtenerDistancia(mano, hombro);
            //distancia entre mano y codo
            float distCodoMano = DistanceHelper.ObtenerDistancia(mano, codo);
            //distancia entre codo y hombro
            float distHombroCodo = DistanceHelper.ObtenerDistancia(codo, hombro);
            //distancia entre hombro y punto auxiliar del torso
            float distTorsoHombro = DistanceHelper.ObtenerDistancia(hombro, puntoTorso);

            //distancia entre mano y objeto
            float distManoObj = DistanceHelper.ObtenerDistancia(mano, skelObjeto);


            Console.WriteLine($"Distancia Codo mano {distCodoMano}");
            Console.WriteLine($"Distancia Hombro Codo {distHombroCodo}");
            Console.WriteLine($"Distancia Hombro Objeto {distHombroObj}");



            if (distHombroObj > (distCodoMano + distHombroCodo))
            {
                Console.WriteLine("no se puede alcanzar el objeto!");
            }
            else
            {
                //el primer argumento es el segmento opuesto al angulo que queremos obtener.

                double anguloHombroAdelanteAtrasFut = DistanceHelper.CalcularAngulo(distManoObj,
                        distHombroObj, distHombroMano);

                double anguloCodoFut = DistanceHelper.CalcularAngulo(distHombroObj,
                    distHombroCodo, distCodoMano);

                float distAuxManoObjeto = (mano.Position.Y - skelObjeto.Y);
                puntoTorso.Y = codo.Position.Y + distAuxManoObjeto;
                double anguloHombroArribaAbajoFut = DistanceHelper.AnguloRectang(distHombroCodo, distTorsoHombro);

                //si el punto del codo está más arriba que el hombro, el angulo se calculará para el otro lado
                //entonces hay que sacar el suplementario para que corresponda al servo.
                if (puntoTorso.Y > hombro.Position.Y)
                {
                    anguloHombroArribaAbajoFut = 180 - anguloHombroArribaAbajoFut;
                }
                if (anguloHombroAdelanteAtrasFut < 90 && anguloHombroAdelanteAtrasFut > 0 &&
                    anguloCodoFut < 90 && anguloCodoFut > 0)
                {
                    array[0] = anguloCodoFut;
                    array[1] = 090;
                    array[2] = anguloHombroAdelanteAtrasFut;
                    array[3] = anguloHombroArribaAbajoFut;
                }
            }
            return array;*/
        }

        private void CalcularAnguloCodoArribaAbajo()
        {
            distancia.DistanciaHombroCodo =
                DistanceHelper.ObtenerDistancia(puntos.Hombro, puntos.Codo);
            distancia.DistanciaManoCodo =
                DistanceHelper.ObtenerDistancia(puntos.Mano, puntos.Codo);
            /*distancia.DistanciaManoHombro =
                DistanceHelper.ObtenerDistancia(puntos.Mano, puntos.Hombro);*/
            distancia.DistanciaHombroObjeto =
                DistanceHelper.ObtenerDistancia(puntos.Hombro, puntos.Objeto);

            angulos.CodoArribaAbajo = CalcularAngulo(distancia.DistanciaHombroObjeto,
                distancia.DistanciaManoCodo, distancia.DistanciaHombroCodo);
        }

        private void CalcularAnguloHombroArribaAbajo()
        {
            angulos.HombroAuxArribaAbajo = CalcularAngulo(distancia.DistanciaManoCodo,
                distancia.DistanciaHombroObjeto, distancia.DistanciaHombroCodo);

            //creo un punto auxiliar:
            SkeletonPoint puntoAux = new SkeletonPoint()
            { 
                X = puntos.Objeto.X,
                Y = puntos.Hombro.Position.Y,
                Z = puntos.Hombro.Position.Z
            };

            distancia.DistanciaHombroAux = DistanceHelper.ObtenerDistancia(
                puntos.Hombro, puntos.PuntoAuxHombroArribaAbajo);
            distancia.DistanciaObjetoAux = DistanceHelper.ObtenerDistancia(
                puntos.Objeto, puntos.PuntoAuxHombroArribaAbajo);

            angulos.HombroObjArribaAbajo = AnguloRectang(
                distancia.DistanciaObjetoAux, distancia.DistanciaHombroAux);

            angulos.HombroArribaAbajo = angulos.HombroObjArribaAbajo -
                angulos.HombroAuxArribaAbajo;

        }

        public double CalcularAngulo(float segmentoA, float segmentoB, float segmentoC)
        {
            //se devuelve el ángulo del primer punto, es decir que si el hombro es "a",
            //devuelve el angulo del hombro en relación al codo y mano.

            float segA2 = (float)Math.Pow(segmentoA, 2);
            float segB2 = (float)Math.Pow(segmentoB, 2);
            float segC2 = (float)Math.Pow(segmentoC, 2);

            double rad = Math.Acos((segA2 - segB2 - segC2) / ((-2) * segmentoB * segmentoC));

            double deg = DistanceHelper.RadToDeg(rad);
            return deg;
        }

        public static double AnguloRectang(float segH, float segA)
        {
            double rad = Math.Acos(segA / segH);
            double deg = DistanceHelper.RadToDeg(rad);
            return deg;
        }
    }
}
