using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtaxiaVision.Models
{
    public class Patient
    {
        public int Age { get; set; }
        public DateTime BeginDate { get; set; }
        public int IdPatient { get; set; }
        public string Name { get; set; }

        public Patient() { }
    }

    
}
