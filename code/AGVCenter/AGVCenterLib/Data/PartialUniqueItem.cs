using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;

namespace AGVCenterLib.Data
{
    public partial class UniqueItem
    {
        public static List<UniqueItemState>
            CanInStockStates = new List<UniqueItemState>()
            {
                UniqueItemState.Created,
                UniqueItemState.OutStocked
            };

        public bool IsCanInStockState
        {
            get
            {
             return CanInStockStates.Contains((UniqueItemState)this.State);
            }
        }

        public string BoxTypeStr
        {
            get
            {
                return BoxType.GetStr(this.BoxTypeId);
            }
        }

    }
}
