using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Model
{
   public class PosationSearchModel
    {
        public string PosationNr { get; set; }
        public string WHNr { get; set; }
        public int Floor { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
