using AGVCenterLib.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AgvServiceLib.DataModel
{
    [DataContract]
    public class UniqueItemModel
    {
        [DataMember]
        public string Nr { get; set; }

        [DataMember]
        public string PartNr { get; set; }

        [DataMember]
        public string KNr { get; set; }

        [DataMember]
        public string KNrWithYear { get; set; }

        [DataMember]
        public string CheckCode { get; set; }

        [DataMember]
        public string KskNr { get; set; }

        [DataMember]
        public string QR { get; set; }

        [DataMember]
        public int State { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public int BoxTypeId { get; set; }

    }
}
