using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class SearchModelBase
    {
        [DataMember]
        public DateTime? CreatedAtStart { get; set; }

        [DataMember]
        public DateTime? CreatedAtEnd { get; set; }
    }
}
