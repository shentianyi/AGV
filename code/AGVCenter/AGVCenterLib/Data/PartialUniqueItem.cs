using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterLib.Data
{
    public partial class UniqueItem
    {
        public static List<UniqueItemState>
            CanInStockStates = new List<UniqueItemState>()
            {
                UniqueItemState.Created,
                UniqueItemState.MisOutStocked,
               UniqueItemState.OutStocked
            };

        public static List<UniqueItemState>
            CanMoveStockStates = new List<UniqueItemState>()
            {
                UniqueItemState.InStocked
            };

        public bool IsCanInStockState
        {
            get
            {
             return CanInStockStates.Contains((UniqueItemState)this.State);
            }
        }

        public bool IsCanMoveStockState
        {
            get
            {
                return CanMoveStockStates.Contains((UniqueItemState)this.State);
            }
        }

        public string BoxTypeStr
        {
            get
            {
                return BoxType.GetStr(this.BoxTypeId);
            }
        }
        public string StateStr
        {
            get
            {
                return EnumUtil.GetDescription((UniqueItemState)this.State);
            }
        }
    }
}
