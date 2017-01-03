using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class PickListSearchModel
    {
        [DataMember]
        public string Nr { get; set; }


        [DataMember]
        public string NrAct { get; set; }
    }
}
