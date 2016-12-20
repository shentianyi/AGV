using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    public class UniqueItemSearchModel
    {
        [DataMember]
        public string Nr { get; set; }
        [DataMember]
        public string KNr { get; set; }
    }
}
