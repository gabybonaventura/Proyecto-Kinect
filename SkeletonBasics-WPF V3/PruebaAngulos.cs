using Microsoft.Kinect;
using Microsoft.Samples.Kinect.SkeletonBasics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Samples.Kinect.SkeletonBasics
{
    class PruebaAngulos
    {
        private Joint mano, codo, hombro;
        private SkeletonPoint objeto;

        public PruebaAngulos()
        {
            mano = new Joint();
            mano.Position = SetPosition(-0.87,4.29,1.89);

            codo = new Joint();
            codo.Position = SetPosition(-0.87, 2.31, 1.8);

            hombro = new Joint();
            hombro.Position = SetPosition(-0.87, 0.52, 3.8);

            objeto = new SkeletonPoint();

            objeto = SetPosition(-0.87, 4.74, 4.1);

            AngleHelper.SetValorAngulos(hombro, mano, codo, objeto);

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
