﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
    [DataContract]
    public class StockTaskLogSearchModel:StockTaskSearchModel
    {
        [DataMember]
        public int? StockTaskId { get; set; }
    }
}