using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum UniqueItemState
    {
        [Description("新建")]
        Created = 100,

        [Description("已入库")]
        InStocked = 200,

        [Description("已出库")]
        OutStocked = 300,

        [Description("已出库返工")]
        OutStockRework = 400,

        [Description("已发运")]
        Sent = 600,

        [Description("误出库")]
        MisOutStocked = 900

    }
}
