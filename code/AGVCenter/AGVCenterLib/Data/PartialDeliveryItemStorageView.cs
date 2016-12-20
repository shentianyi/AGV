using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
   public partial class DeliveryItemStorageView
    {
        public string BoxTypeStr
        {
            get
            {
                return BoxType.GetStr(this.UniqueItemBoxTypeId);
            }
        }
    }
}
