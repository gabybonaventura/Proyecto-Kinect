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
        
        public static float ObtenerDistancia(Joint articulacion, SkeletonPoint objeto)
        {
            float Distancia_X = articulacion.Position.X - (float)objeto.X;
            float Distancia_Y = articulacion.Position.Y - (float)objeto.Y;
            float Distancia_Z = articulacion.Position.Z - (float)objeto.Z;

            return (float)Math.Sqrt(Math.Pow(Distancia_X, 2) + Math.Pow(Distancia_Y, 2) + Math.Pow(Distancia_Z, 2));
        }
        public static float ObtenerDistancia(SkeletonPoint punto1, SkeletonPoint punto2)
        {
            float Distancia_X = punto1.X - punto2.X;
            float Distancia_Y = punto1.Y - punto2.Y;
            float Distancia_Z = punto1.Z - punto2.Z;

            return (float)Math.Sqrt(Math.Pow(Distancia_X, 2) + Math.Pow(Distancia_Y, 2) + Math.Pow(Distancia_Z, 2));                //Realiza el calculo de la distancia entre articulaciones
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
