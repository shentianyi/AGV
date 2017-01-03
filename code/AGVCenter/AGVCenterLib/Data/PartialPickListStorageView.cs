using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterLib.Data
{
   public partial class PickListStorageView
    {
        public bool IsInStock
        {
            get { return this.StorageId != null; }
        }
    }
}
