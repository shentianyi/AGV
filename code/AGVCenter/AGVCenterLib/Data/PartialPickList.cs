using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
    public partial class PickList
    {
        

        public string StateStr
        {
            get
            {
                return EnumUtil.GetDescription((PickListState)this.State);
            }
        }

    }
}
