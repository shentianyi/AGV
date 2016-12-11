using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum StockTaskType
    {
        /// <summary>
        /// 无动作
        /// </summary>
        NONE =0,
        /// <summary>
        /// OUT
        /// </summary>
        OUT=1,
        /// <summary>
        /// IN
        /// </summary>
        IN=2,
        /// <summary>
        /// CHECK
        /// </summary>
        CHECK=3
    }

    public enum StockTaskState
    {
        NONE = -1,
        /// <summary>
        /// 初始
        /// </summary>
        Init = 0,
        /// <summary>
        /// AGV 入库中
        /// </summary>
        AgvInStcoking = 1,
        /// <summary>
        /// 机械手入库中
        /// </summary>
        RobootInStocking = 2,

        /// <summary>
        /// 巷道机入库缓冲区中
        /// </summary>
        RoadMachineStockBuffing = 3,

        /// <summary>
        /// 巷道机入库中
        /// </summary>
        RoadMachineInStocking = 4,
        /// <summary>
        /// 入库成功
        /// </summary>
        InStocked = 5,
        InCanceled = 6,
        RoadMachineOutStockInit=7,
        RoadMachineWaitOutStock = 8,
        RoadMachineOutStocking = 9,
        OutStocked = 10,

        // Error
        ErrorNoPositoin = 20,
        ErrorUniqNotExsits = 21,
        ErrorUniqCannotInStock = 22,
        ErrorCreateDbTask = 23,
        ErrorBarcodeReScan=24
    }


}
