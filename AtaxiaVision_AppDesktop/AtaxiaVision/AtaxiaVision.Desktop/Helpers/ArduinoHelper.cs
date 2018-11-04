using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AtaxiaVision.Helpers
{
    class ArduinoHelper
    {
        public static Brush CalcularColor(int c)
        {
            if (c < 100)
                // Azul
                return new SolidColorBrush(Color.FromRgb(29, 22, 182));
            if (c < 200)
                // Verde
                return new SolidColorBrush(Color.FromRgb(22, 182, 51));
            if (c < 500)
                // Naranja
                return new SolidColorBrush(Color.FromRgb(226, 107, 25));
            // Rojo
            return new SolidColorBrush(Color.FromRgb(255, 0, 0));
        }

        
    }
}
