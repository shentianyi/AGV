using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Model
{
    public class RecordSearchModel
    {
        public int MovementId { get; }
        public string SourcePosation { get; set; }

        public string AimedPosation { get; set; }

        public string Operation { get; set; }

        public string Operator { get; set; }

        public string Time { get; set; }
    }
}
