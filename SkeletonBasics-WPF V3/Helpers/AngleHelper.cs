using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Helpers
{
    class AngleHelper
    {
        static public int CotaDistancia = 20;
       
        public static double[] SetValorAngulos(Joint hombro, Joint mano, Joint codo, Joint cadera, SkeletonPoint skelObjeto)
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
                //distancia entre cadera y hombro
            float distCadHombro = DistanceHelper.ObtenerDistancia(cadera, hombro);
                //distancia entre cadera y mano
            float distCadMano = DistanceHelper.ObtenerDistancia(mano, cadera);
                //distancia entre hombro y punto auxiliar del torso
            float distTorsoHombro = DistanceHelper.ObtenerDistancia(hombro, puntoTorso);

            //distancia entre mano y objeto
            float distManoObj = DistanceHelper.ObtenerDistancia(mano, skelObjeto);

            if (distHombroObj > (distCodoMano + distHombroCodo + AngleHelper.CotaDistancia))
            {
                Console.WriteLine("no se puede alcanzar el objeto!");
            }
            else
            {
                //calculamos el ángulo ACTUAL de las articulaciones
                //el primer argumento es el segmento opuesto al angulo que queremos obtener.

                
                double anguloCodoAct = DistanceHelper.CalcularAngulo(distHombroMano,
                    distHombroCodo, distCodoMano);

                double anguloHombroCad = DistanceHelper.CalcularAngulo(distCadMano,
                        distHombroMano, distCadHombro);

                double anguloHombro_aaFut = DistanceHelper.CalcularAngulo(distManoObj,
                        distHombroObj, distHombroMano);

                double angHombro_id = DistanceHelper.AnguloRectang(distHombroCodo, distTorsoHombro);

                double anguloCodoFut = DistanceHelper.CalcularAngulo(distHombroObj,
                    distHombroCodo, distCodoMano);

                float distAuxMO = (mano.Position.Y - skelObjeto.Y);
                puntoTorso.Y = codo.Position.Y + distAuxMO;
                double angHombro_idFut = DistanceHelper.AnguloRectang(distHombroCodo, distTorsoHombro);

                //si el punto del codo está más arriba que el hombro, el angulo se calculará para el otro lado
                //entonces hay que sacar el suplementario para que corresponda al servo.
                if (puntoTorso.Y > hombro.Position.Y)
                {
                    angHombro_idFut = 180 - angHombro_idFut;
                }
                if(anguloHombro_aaFut < 90 && anguloHombro_aaFut > 0 &&
                    anguloCodoFut < 90 && anguloCodoFut > 0)
                {
                    array[0] = anguloCodoFut;
                    array[1] = 090;
                    array[2] = anguloHombro_aaFut;
                    array[3] = angHombro_idFut;
                }
                Console.WriteLine($"Angulo Codo Adelante Atras: {array[0]}");
                Console.WriteLine($"Angulo Codo Izquieda Derecha: {array[1]}");
                Console.WriteLine($"Hombro Adelante Atras: {array[2]}");
                Console.WriteLine($"Hombro Izquieda Derecha: {array[3]}");
            }
            return array;
        }
    }
}
