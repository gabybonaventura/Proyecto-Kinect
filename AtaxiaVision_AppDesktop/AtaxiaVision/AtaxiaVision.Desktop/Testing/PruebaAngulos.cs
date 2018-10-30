using AtaxiaVision.Helpers;
using AtaxiaVision.Models;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Testing
{
    class PruebaAngulos
    {
        private Joint mano, codo, hombro;
        private SkeletonPoint objeto;

        public PruebaAngulos()
        {
            hombro = new Joint();
            hombro.Position = SetPosition(-2.82, 2.92, 1);

            codo = new Joint();
            codo.Position = SetPosition(-3.38, 2.04, 2.3);

            mano = new Joint();
            mano.Position = SetPosition(-2.9, 1.22, 3.83);
            
            objeto = new SkeletonPoint();

            objeto = SetPosition(-2.3, 0.94, 2.47);
            AngleHelper a = new AngleHelper();

            a.SetValorAngulos(new Puntos(hombro,codo,mano,objeto));
        }

        public SkeletonPoint SetPosition(double x, double y, double z)
        {
            SkeletonPoint puntoaux = new SkeletonPoint();
            puntoaux.X = (float)x;
            puntoaux.Y = (float)y;
            puntoaux.Z = (float)z;
            return puntoaux;
        }
    }
}
