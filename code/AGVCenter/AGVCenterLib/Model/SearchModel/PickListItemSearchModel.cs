using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class PickListItemSearchModel:SearchModelBase
    {
        [DataMember]
        public string Nr { get; set; }

        [DataMember]
        public string KNr { get; set; }

        [DataMember]
        public string PositionNr { get; set; }


        [DataMember]
        public string PickListNr { get; set; }


        [DataMember]
        public string PickListNrAct { get; set; }
    }
}
