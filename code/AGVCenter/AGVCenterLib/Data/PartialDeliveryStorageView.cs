using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
   public partial class  DeliveryStorageView
    {
        public bool IsInStock
        {
            get { return this.StorageId != null; }
        }


    }
}
