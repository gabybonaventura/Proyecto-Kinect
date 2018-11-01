using AtaxiaVision.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class RespuestaServer
    {
        public int RespuestaCode { get; set; }
        public bool PropiedadIsValid { get; set; }
        public Patient patient { get; set; }
        public int CodigoTokenValid { get; set; }

        public RespuestaServer()
        {
            RespuestaCode = ServerHelper.SERVER_ERROR;
        }
    }
}
