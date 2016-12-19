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

        AgvPassFail = 10,

        AgvWaitPassing = 20,
        /// <summary>
        /// AGV 入库中
        /// </summary>
        AgvInStcoking = 30,
        /// <summary>
        /// 机械手入库中
        /// </summary>
        RobootInStocking = 40,

        /// <summary>
        /// 巷道机入库缓冲区中
        /// </summary>
        RoadMachineStockBuffing = 50,

        /// <summary>
        /// 巷道机入库中
        /// </summary>
        RoadMachineInStocking = 60,
        /// <summary>
        /// 入库成功
        /// </summary>
        InStocked = 70,
        InCanceled = 80,
        RoadMachineOutStockInit = 90,
        RoadMachineWaitOutStock = 100,
        RoadMachineOutStocking = 110,
        OutStocked = 120,

        /// <summary>
        /// 取消
        /// </summary>
        Canceled=800,

        // Error
        ErrorNoPositoin = 930,
        ErrorUniqNotExsits = 940,
        ErrorUniqCannotInStock = 950,
        ErrorCreateDbTask = 960,
        ErrorBarcodeReScan = 970,

        ErrorInStock = 980,
        ErrorOutStock = 990
    }


}
