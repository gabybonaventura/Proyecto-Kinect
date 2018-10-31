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
            hombro.Position = SetPosition(2.3, 1.21, -1.67);

            codo = new Joint();
            codo.Position = SetPosition(2.3, 0.98, -0.78);

            mano = new Joint();
            mano.Position = SetPosition(2.3, 1.42, 0.3);
            
            objeto = new SkeletonPoint();

            objeto = SetPosition(2.3, 0.25, -0.17);
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
