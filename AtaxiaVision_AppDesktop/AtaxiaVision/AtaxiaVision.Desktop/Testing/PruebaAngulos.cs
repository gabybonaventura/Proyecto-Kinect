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
            mano = new Joint();
            mano.Position = SetPosition(1.26, 2.2, 2.23);

            codo = new Joint();
            codo.Position = SetPosition(1.26, 0.98, 1.52);

            hombro = new Joint();
            hombro.Position = SetPosition(1.26, -0.46, 2.77);

            objeto = new SkeletonPoint();

            objeto = SetPosition(0.43, 2.64, 3);
            AngleHelper a = new AngleHelper();

            a.SetValorAngulos(new Puntos(hombro, mano, codo, objeto));

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
