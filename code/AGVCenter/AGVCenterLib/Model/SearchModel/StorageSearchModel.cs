using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class StorageSearchModel:SearchModelBase
    {
        [DataMember]
        public string Nr { get; set; }
        [DataMember]
        public string KNr { get; set; }
        [DataMember]
        public string PositionNr { get; set; }
        [DataMember]
        public int? BoxTypeId { get; set; }
        [DataMember]
        public int? RoadMachineIndex { get; set; }

        [DataMember]
        public DateTime? FIFOStart { get; set; }

        [DataMember]
        public DateTime? FIFOEnd { get; set; }
    }
}