using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Model
{
    public class MoveStockModel
    {
        public StorageUniqueItemView FromStorage { get; set; }
        public Position FromPosition{get;set;}
        public Position ToPosition { get; set; }

    }
}
