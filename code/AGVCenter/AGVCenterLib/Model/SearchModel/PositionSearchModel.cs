using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class PositionSearchModel:SearchModelBase
    {
        [DataMember]
        public string Nr { get; set; }
        [DataMember]
        public string NrAct { get; set; }
        [DataMember]
        public bool? IsLocked { get; set; }
        [DataMember]
        public int? InStorePriority { get; set; }
    }
}
