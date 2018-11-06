using AtaxiaVision.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class RespuestaToken
    {
        public int CodigoTokenValid { get; set; }
        public Patient Patient { get; set; }

        public RespuestaToken()
        {
            CodigoTokenValid = ServerHelper.TOKEN_SINCONEXION;
        }
    }
    
}
