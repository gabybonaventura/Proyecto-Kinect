using System;
using System.Windows;
using AtaxiaVision.Models;
using Microsoft.Kinect;

namespace AtaxiaVision.Helpers
{
    class AngleHelper
    {
        private Angulos angulos;
        private Distancia distancia;
        private Puntos puntos = new Puntos();

        public Angulos SetValorAngulos(Puntos puntos)
        {
            this.puntos = puntos;
            distancia = new Distancia();
            angulos = new Angulos();

            //calculo distancias para obtener el ANGULO del CODO ARRIBA ABAJO
            CalcularAnguloCodoArribaAbajo();

            if (distancia.DistanciaHombroObjeto > distancia.DistanciaHombroCodo
                + distancia.DistanciaManoCodo)
                return angulos;

            //calculo del angulo hombro ATRAS ADELANTE:
            CalcularAnguloHombroAdelanteAtras();

            //Calculo del angulo hombro ARRIBA ABAJO:
            //            CalcularAnguloHombroArribaAbajo();
            angulos.HombroArribaAbajo = 90.0;
            angulos.CodoIzquierdaDerecha = 90.0;
            
            return angulos;
        }



        private void CalcularAnguloCodoArribaAbajo()
        {
            distancia.DistanciaHombroCodo =
                DistanceHelper.ObtenerDistancia(puntos.Hombro, puntos.Codo);

            distancia.DistanciaManoCodo =
                DistanceHelper.ObtenerDistancia(puntos.Mano, puntos.Codo);
            
            distancia.DistanciaHombroObjeto =
                DistanceHelper.ObtenerDistancia(puntos.Hombro, puntos.Objeto);

            angulos.CodoArribaAbajo = CalcularAngulo(distancia.DistanciaHombroObjeto,
                distancia.DistanciaManoCodo, distancia.DistanciaHombroCodo);
        }

        private void CalcularAnguloHombroAdelanteAtras()
        {
            puntos.Hombro2d = new Point(puntos.Hombro.Position.Y, puntos.Hombro.Position.Z);
        //    puntos.Codo2d = new Point(puntos.Codo.Position.Y, puntos.Codo.Position.Z);
        //    puntos.Mano2d = new Point(puntos.Mano.Position.Y, puntos.Mano.Position.Z);
            puntos.Objeto2d = new Point(puntos.Objeto.Y, puntos.Objeto.Z);

        //    distancia.DistanciaManoHombro = DistanceHelper.ObtenerDistancia(
        //        puntos.Hombro2d, puntos.Mano2d);

        //    distancia.DistanciaManoCodo = DistanceHelper.ObtenerDistancia(
               // puntos.Codo2d, puntos.Mano2d);

           // distancia.DistanciaHombroCodo = DistanceHelper.ObtenerDistancia(
           //     puntos.Hombro2d, puntos.Codo2d);

            angulos.HombroAuxAtrasAdelante = CalcularAngulo(distancia.DistanciaManoCodo,
                distancia.DistanciaHombroObjeto, distancia.DistanciaHombroCodo);

            //puntos.PuntoAuxHombroAtrasAdelante = new Point(puntos.Hombro2d.X, puntos.Objeto2d.Y);
            puntos.PuntoAuxHombroAtrasAdelante = new Point(puntos.Objeto2d.X, puntos.Hombro2d.Y);

            distancia.DistanciaObjetoAux = DistanceHelper.ObtenerDistancia(
                puntos.Objeto2d, puntos.PuntoAuxHombroAtrasAdelante);

            distancia.DistanciaHombroAux = DistanceHelper.ObtenerDistancia(
                puntos.Hombro2d, puntos.PuntoAuxHombroAtrasAdelante);

            angulos.HombroObjAtrasAdelante = CalcularAngulo(distancia.DistanciaManoCodo,
                distancia.DistanciaHombroObjeto, distancia.DistanciaHombroCodo);
                //AnguloRectangCos(
                //distancia.DistanciaObjetoAux, distancia.DistanciaHombroAux);

            angulos.HombroAdelanteAtras = angulos.HombroObjAtrasAdelante -
                angulos.HombroAuxAtrasAdelante;
        }

        /*private void CalcularAnguloHombroArribaAbajo()
        {
            puntos.Hombro2d = new Point(puntos.Hombro.Position.X, puntos.Hombro.Position.Z);
            //puntos.Codo2d = new Point(puntos.Codo.Position.X, puntos.Codo.Position.Z);
            //puntos.Mano2d = new Point(puntos.Mano.Position.X, puntos.Mano.Position.Z);
            puntos.Objeto2d = new Point(puntos.Objeto.X, puntos.Objeto.Z);

            //distancia.DistanciaManoHombro = DistanceHelper.ObtenerDistancia(
             //   puntos.Hombro2d, puntos.Mano2d);

            //distancia.DistanciaManoCodo = DistanceHelper.ObtenerDistancia(
              //  puntos.Codo2d, puntos.Mano2d);

            //distancia.DistanciaHombroCodo = DistanceHelper.ObtenerDistancia(
              //  puntos.Hombro2d, puntos.Codo2d);

            angulos.HombroAuxArribaAbajo= CalcularAngulo(distancia.DistanciaManoCodo,
                distancia.DistanciaManoHombro, distancia.DistanciaHombroCodo);

            puntos.PuntoAuxHombroArribaAbajo = new Point(puntos.Hombro2d.X, puntos.Objeto2d.Y);

            distancia.DistanciaObjetoAux = DistanceHelper.ObtenerDistancia(
                puntos.Objeto2d, puntos.PuntoAuxHombroAtrasAdelante);

            distancia.DistanciaHombroAux = DistanceHelper.ObtenerDistancia(
                puntos.Hombro2d, puntos.PuntoAuxHombroAtrasAdelante);

            angulos.HombroObjArribaAbajo = AnguloRectangCos(
                distancia.DistanciaObjetoAux, distancia.DistanciaHombroAux);

            if (puntos.Objeto2d.X < puntos.Hombro2d.X)
                angulos.HombroObjArribaAbajo = 180 - angulos.HombroObjArribaAbajo;

            angulos.HombroArribaAbajo = angulos.HombroObjArribaAbajo -
                angulos.HombroAuxArribaAbajo;
        }*/
/*
        private void CalcularAnguloHombroAdelanteAtras()
        {
            //contiene el angulo entre hombro codo y mano.
            angulos.HombroAuxAtrasAdelante = CalcularAngulo(distancia.DistanciaManoCodo,
                distancia.DistanciaHombroObjeto, distancia.DistanciaHombroCodo);

            //creo un punto auxiliar para calcular angulo que hay entre hombro y objeto.

            puntos.PuntoAuxHombroAtrasAdelante = new SkeletonPoint()
            {
                X = puntos.Hombro.Position.X,
                Y = puntos.Hombro.Position.Y,
                Z = puntos.Objeto.Z
            };

            //calculo distancia entre hombro y puntoaux
            distancia.DistanciaHombroAux = DistanceHelper.ObtenerDistancia(
                puntos.Hombro, puntos.PuntoAuxHombroAtrasAdelante);

            //calculo distancia entre objeto y puntoaux
            distancia.DistanciaObjetoAux = DistanceHelper.ObtenerDistancia(
                puntos.Objeto, puntos.PuntoAuxHombroAtrasAdelante);

            //calculo angulo entre hombro, puntoaux y objeto:
            angulos.HombroObjAtrasAdelante = CalcularAngulo(
                distancia.DistanciaObjetoAux, distancia.DistanciaHombroAux,
                distancia.DistanciaHombroObjeto);

            angulos.HombroAdelanteAtras = angulos.HombroObjAtrasAdelante +
                angulos.HombroAuxAtrasAdelante;

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

        }*/

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

        public static double AnguloRectangCos(float segO, float segA)
        {
            double rad = Math.Atan(segO / segA);
            double deg = DistanceHelper.RadToDeg(rad);
            return deg;
        }
    }
}
