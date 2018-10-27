using System;
using Microsoft.Kinect;

namespace AtaxiaVision.Helpers
{
    class AngleHelper
    {
        public static double[] SetValorAngulos(Joint hombro, Joint mano, Joint codo, SkeletonPoint skelObjeto)
        {
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
            return array;
        }
    }
}
