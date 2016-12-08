using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum DeliveryState
    {
        [Description("初始")]
        Init=100,

        [Description("已发运")]
        Sent=600
    }
}
