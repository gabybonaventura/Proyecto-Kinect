using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Samples.Kinect.SkeletonBasics.Helpers
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
        //Evento para calcular angulos:
        public static double Angulos(Joint j1, Joint j2, Joint j3)
        {
            double Angulo = 0;
            double shrhX = j1.Position.X - j2.Position.X;
            double shrhY = j1.Position.Y - j2.Position.Y;
            double shrhZ = j1.Position.Z - j2.Position.Z;
            double hsl = vectorNorm(shrhX, shrhY, shrhZ);
            double unrhX = j3.Position.X - j2.Position.X;
            double unrhY = j3.Position.Y - j2.Position.Y;
            double unrhZ = j3.Position.Z - j2.Position.Z;
            double hul = vectorNorm(unrhX, unrhY, unrhZ);
            double mhshu = shrhX * unrhX + shrhY * unrhY + shrhZ * unrhZ;
            double x = mhshu / (hul * hsl);

            if (x != Double.NaN)
            {
                if (-1 <= x && x <= 1)
                {
                    double angleRAd = Math.Acos(x);
                    Angulo = angleRAd * (180.0 / Math.PI);
                }
                else
                    Angulo = 0;
            }
            else
                Angulo = 0;

            return Angulo;
        }
        //Muestra:
        public static double vectorNorm(double x, double y, double z)
        {

            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));

        }
    }
}
