using AGVCenterLib.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class StockMovementSearchModel:SearchModelBase
    {
        [DataMember]
        public string UniqItemNr { get; set; }
        [DataMember]
        public string PositionNr { get; set; }

        [DataMember]
        public StockMovementType? StockMovementType
        {
            get;set;
        }
    }
}
