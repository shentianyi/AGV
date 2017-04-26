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

        public string CreatedAtStr
        {
            get {
                return this.CreatedAt.HasValue ? this.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            }
        }
    }
}
