﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class SesionViewModel
    {
        public string Token { get; set; }
        // Indica si el token es valido o invalido, una vez que se verifico con el servidor.
        public bool TokenValido { get; set; }
        // Indica si el token fue verificado contra el servidor.
        public bool TokenVerificado { get; set; }
    }
}
