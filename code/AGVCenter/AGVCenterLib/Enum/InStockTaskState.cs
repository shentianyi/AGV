using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum InStockTaskState
    {
        Init = 0,
        WaitingStcok = 1,
        InStocking = 2,
        InStocked = 3,
        Canceled = 4
    }
}
