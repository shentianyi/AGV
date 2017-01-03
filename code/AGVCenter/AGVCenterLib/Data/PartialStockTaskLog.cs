using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterLib.Data
{
   public partial class StockTaskLog
    {
        public string FromStateStr
        {
            get
            {
                return EnumUtil.GetDescription((StockTaskState)this.FromState);
            }
        }
    }
}
