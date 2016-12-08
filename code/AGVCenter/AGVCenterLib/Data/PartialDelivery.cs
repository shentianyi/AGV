using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
    public partial class Delivery
    {
        public static List<DeliveryState> CanSendStates = new List<DeliveryState>()
        {
            DeliveryState.Init
        };


        public bool CanSend
        {
            get
            {
                return CanSendStates.Contains((DeliveryState)this.State);
            }
        }

        public string StateStr
        {
            get
            {
                return EnumUtil.GetDescription((DeliveryState)this.State);
            }
        }

    }
}
