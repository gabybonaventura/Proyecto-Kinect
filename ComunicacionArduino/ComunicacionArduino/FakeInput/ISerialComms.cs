using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComunicacionArduino.FakeInput
{
    interface ISerialComms
    {
        void SendMessage(string message);
    }
}
