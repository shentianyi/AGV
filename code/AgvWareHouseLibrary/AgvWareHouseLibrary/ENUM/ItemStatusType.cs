using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AgvWareHouseLibrary.ENUM
{
    public enum ItemStatusType
    {


        [Description("已下线")]
        Offline = 0,
        [Description("已入库")]
        Incoming = 1,
        [Description("已出库")]
        Outcoming = 2,
        [Description("已发运")]
        Dlivery = 3



    }
}
