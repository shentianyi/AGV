using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    public class StorageSearchModel
    {
        [DataMember]
        public string Nr { get; set; }
        [DataMember]
        public string KNr { get; set; }
        [DataMember]
        public string PositionNr { get; set; }
    }
}