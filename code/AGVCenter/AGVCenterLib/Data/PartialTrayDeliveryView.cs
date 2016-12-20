using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterLib.Data
{
  public partial  class  TrayDeliveryView
    {
        public string StateStr
        {
            get
            {
                return EnumUtil.GetDescription((DeliveryState)this.State);
            }
        }
    }
}
