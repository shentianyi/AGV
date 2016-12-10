using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum StockMovementType
    {
        [Description("入库")]
        In = 100,
        [Description("出库")]
        Out = 200,
        [Description("移库")]
        Move = 300
    }
}
