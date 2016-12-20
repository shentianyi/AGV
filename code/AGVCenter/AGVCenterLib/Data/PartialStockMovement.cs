using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterLib.Data
{
   public partial class  StockMovement
    {
        

        public string TypeStr
        {
            get
            {
                return EnumUtil.GetDescription((StockMovementType)this.Type);
            }
        }
    }
}
