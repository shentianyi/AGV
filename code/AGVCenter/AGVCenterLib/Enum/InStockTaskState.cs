using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum StockTaskType
    {
        In=0,
        Out=1
    }

    public enum InStockTaskState
    {
        Init = 0,
        WaitingStcok = 1,
        InStocking = 2,
        InStocked = 3,
        Canceled = 4,
        
        // Error
        ErrorNoPositoin = 5,
        ErrorUniqNotExsits = 6,
        ErrorUniqCannotInStock=7
    }
}
