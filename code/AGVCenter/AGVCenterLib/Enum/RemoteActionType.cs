using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum RemoteActionType
    {
        /// <summary>
        /// 重置之前的扫描条码
        /// </summary>
        RestPreScanBarcode,
        
        /// <summary>
        /// 取消任务
        /// </summary>
        CancelTask,

        /// <summary>
        /// 手动入库
        /// </summary>
        ManIn,

        /// <summary>
        /// 手动出库
        /// </summary>
        ManOut
    }
}
