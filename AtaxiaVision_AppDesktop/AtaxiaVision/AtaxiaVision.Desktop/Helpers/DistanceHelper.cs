using System;
using Microsoft.Kinect;

namespace AtaxiaVision.Helpers
{
    public class DistanceHelper
    {
        public static float ObtenerDistancia(Joint firstJoint, Joint secondJoint)
        {
            float Distancia_X = firstJoint.Position.X - secondJoint.Position.X;
            float Distancia_Y = firstJoint.Position.Y - secondJoint.Position.Y;
            float Distancia_Z = firstJoint.Position.Z - secondJoint.Position.Z;

            return (float)Math.Sqrt(Math.Pow(Distancia_X, 2) + Math.Pow(Distancia_Y, 2) + Math.Pow(Distancia_Z, 2));                //Realiza el calculo de la distancia entre articulaciones
        }

        //Método calculo ángulo donde: 
        public static double CalcularAngulo(float segmentoA, float segmentoB, float segmentoC)
        {
            //se devuelve el ángulo del primer punto, es decir que si el hombro es "a",
            //devuelve el angulo del hombro en relación al codo y hombro.


            float segA2 = (float)Math.Pow(segmentoA, 2);
            float segB2 = (float)Math.Pow(segmentoB, 2);
            float segC2 = (float)Math.Pow(segmentoC, 2);

            double rad = Math.Acos((segA2 - segB2 - segC2) / ((-2) * segmentoB * segmentoC));

            double deg = DistanceHelper.RadToDeg(rad);
            return deg;

            //return rad;
        }

        public static double AnguloRectang(float segH, float segA)
        {
            double rad = Math.Acos(segA / segH);
            double deg = DistanceHelper.RadToDeg(rad);
            return deg;
        }

        public static float ObtenerDistancia(Joint articulacion, SkeletonPoint objeto)
        {
            float Distancia_X = articulacion.Position.X - (float)objeto.X;
            float Distancia_Y = articulacion.Position.Y - (float)objeto.Y;
            float Distancia_Z = articulacion.Position.Z - (float)objeto.Z;

            return (float)Math.Sqrt(Math.Pow(Distancia_X, 2) + Math.Pow(Distancia_Y, 2) + Math.Pow(Distancia_Z, 2));
        }

        public static double RadToDeg(double rad)
        {
            return rad * (180.0 / Math.PI);
        }

        //evento para convertir puntos en SkeletonPoint:
        public static SkeletonPoint ObtenerSkelPoint(int equis, int ygriega, int zeta, KinectSensor sensor)
        {
            DepthImagePoint dip = new DepthImagePoint();
            dip.X = equis;
            dip.Y = ygriega;
            dip.Depth = zeta;
            SkeletonPoint aux = sensor.CoordinateMapper.MapDepthPointToSkeletonPoint(DepthImageFormat.Resolution640x480Fps30, dip);
            return aux;
        }
    }
}
