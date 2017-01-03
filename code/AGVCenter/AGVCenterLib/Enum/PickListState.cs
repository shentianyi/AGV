using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum PickListState
    {
        [Description("新建")]
        Init =100,

        [Description("已创建出库任务")]
        PickTaskCreated =200
    }
}
