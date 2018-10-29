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
            return indiceUltimoDesvio + cantidadDesvioContinuo == i;
        }

        public static int CalcularDesvios(List<TensionServos> tensiones)
        {
            int desvios = 0;
            int indiceUltimoDesvio = -1;
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
    }
}
