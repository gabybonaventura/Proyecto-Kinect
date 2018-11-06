using AtaxiaVision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Helpers
{
    class TensionHelper
    {
        private const int TensionLimite = 200;

        private static bool HayDesvio(TensionServos tension)
        {
            return (tension.CodoArribaAbajo > TensionLimite ||
                    tension.CodoIzquierdaDerecha > TensionLimite ||
                    tension.HombroAdelanteAtras > TensionLimite ||
                    tension.HombroArribaAbajo > TensionLimite);
        }

        private static bool EsNuevoDesvio(int i, int indiceUltimoDesvio, int cantidadDesvioContinuo)
        {
            return indiceUltimoDesvio + cantidadDesvioContinuo != i;
        }

        public static int CalcularDesvios(List<TensionServos> tensiones)
        {
            int desvios = 0;
            int indiceUltimoDesvio = -5;
            int cantidadDesviosContinuos = 1;
            for (int i = 0; i < tensiones.Count; i++)
            {
                var tension = tensiones.ElementAt(i);
                if (HayDesvio(tension))
                {
                    if (EsNuevoDesvio(i, indiceUltimoDesvio, cantidadDesviosContinuos))
                    {
                        indiceUltimoDesvio = i;
                        cantidadDesviosContinuos = 1;
                        desvios++;
                    }
                    else
                        cantidadDesviosContinuos++;
                }
            }
            return desvios;
        }

        public static List<TensionServos> TestListaTensionManual()
        {
            List<TensionServos> list = new List<TensionServos>();
            list.Add(new TensionServos() { CodoArribaAbajo = 10 });
            list.Add(new TensionServos() { CodoArribaAbajo = 20 });
            list.Add(new TensionServos() { CodoArribaAbajo = 100 });
            list.Add(new TensionServos() { CodoArribaAbajo = 250 });
            list.Add(new TensionServos() { CodoArribaAbajo = 300 });
            list.Add(new TensionServos() { CodoArribaAbajo = 250 });
            list.Add(new TensionServos() { CodoArribaAbajo = 300 });
            list.Add(new TensionServos() { CodoArribaAbajo = 250 });
            list.Add(new TensionServos() { CodoArribaAbajo = 10 });
            list.Add(new TensionServos() { CodoArribaAbajo = 50 });
            list.Add(new TensionServos() { CodoArribaAbajo = 60 });
            list.Add(new TensionServos() { CodoArribaAbajo = 40 });
            list.Add(new TensionServos() { CodoArribaAbajo = 250 });
            list.Add(new TensionServos() { CodoArribaAbajo = 330 });
            list.Add(new TensionServos() { CodoArribaAbajo = 250 });
            list.Add(new TensionServos() { CodoArribaAbajo = 254 });
            list.Add(new TensionServos() { CodoArribaAbajo = 253 });
            list.Add(new TensionServos() { CodoArribaAbajo = 320 });
            list.Add(new TensionServos() { CodoArribaAbajo = 15 });
            list.Add(new TensionServos() { CodoArribaAbajo = 14 });
            list.Add(new TensionServos() { CodoArribaAbajo = 16 });
            list.Add(new TensionServos() { CodoArribaAbajo = 18 });
            list.Add(new TensionServos() { CodoArribaAbajo = 250 });
            list.Add(new TensionServos() { CodoArribaAbajo = 233 });
            list.Add(new TensionServos() { CodoArribaAbajo = 332 });
            list.Add(new TensionServos() { CodoArribaAbajo = 222 });
            list.Add(new TensionServos() { CodoArribaAbajo = 11 });
            list.Add(new TensionServos() { CodoArribaAbajo = 20 });
            return list;
        }

        public static List<TensionServos> TestListaTension(int cantDesvios)
        {
            List<TensionServos> list = new List<TensionServos>();
            int count = new Random().Next(300,600);
            int contadorDesvios = cantDesvios;
            for (int i = 0; i < count; i++)
            {
                double probabilidad = new Random().NextDouble();
                if(probabilidad < 0.5 && contadorDesvios > 0)
                {
                    contadorDesvios--;
                    list.AddRange(ListaDesvios(new Random().Next(5, 200)));
                }
                else
                {
                    list.AddRange(ListaNoDesvios(new Random().Next(5, 200)));
                }
            }
            return list;
        }

        private static List<TensionServos> ListaDesvios(int count)
        {
            List<TensionServos> list = new List<TensionServos>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(new TensionServos
                {
                    CodoArribaAbajo = new Random().Next(TensionLimite,int.MaxValue),
                    CodoIzquierdaDerecha= new Random().Next(TensionLimite, int.MaxValue),
                    HombroAdelanteAtras = new Random().Next(TensionLimite, int.MaxValue),
                    HombroArribaAbajo = new Random().Next(TensionLimite, int.MaxValue)
                });
            }
            return list;
        }

        private static List<TensionServos> ListaNoDesvios(int count)
        {
            List<TensionServos> list = new List<TensionServos>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(new TensionServos
                {
                    CodoArribaAbajo = new Random().Next(0, TensionLimite),
                    CodoIzquierdaDerecha = new Random().Next(0, TensionLimite),
                    HombroAdelanteAtras = new Random().Next(0, TensionLimite),
                    HombroArribaAbajo = new Random().Next(0, TensionLimite)
                });
            }
            return list;
        }
        
    }
}
