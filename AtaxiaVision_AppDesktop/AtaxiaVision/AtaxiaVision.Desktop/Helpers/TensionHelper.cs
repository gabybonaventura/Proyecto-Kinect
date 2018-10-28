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

        public static int CalcularDesvios(List<TensionServos> tensiones)
        {
            int desvios = 0;
            int indiceUltimoDesvio = -1;
            for (int i = 0; i < tensiones.Count; i++)
            {
                var tension = tensiones.ElementAt(i);
                if (HayDesvio(tension))
                {

                }
            }
            return 0;
        }
    }
}
