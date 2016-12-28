using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum StockTaskType
    {
        /// <summary>
        /// 无动作
        /// </summary>
        [Description("初始")]
        NONE =0,
        /// <summary>
        /// OUT
        /// </summary>
        [Description("出库")]
        OUT =1,
        /// <summary>
        /// IN
        /// </summary>
        [Description("入库")]
        IN =2,
        /// <summary>
        /// CHECK
        /// </summary>
        [Description("盘点")]
        CHECK =3
    }

    public enum StockTaskState
    {
        [Description("未知")]
        NONE = -1,
        /// <summary>
        /// 初始
        /// </summary>
        [Description("初始")]
        Init = 0,

        [Description("AGV放行失败")]
        AgvPassFail = 10,

        [Description("AGV等待放行")]
        AgvWaitPassing = 20,
        /// <summary>
        /// AGV 入库中
        /// </summary>
        [Description("AGV入库中")]
        AgvInStcoking = 30,
        /// <summary>
        /// 机械手入库中
        /// </summary>
        [Description("机械手入库中")]
        RobootInStocking = 40,

        /// <summary>
        /// 巷道机入库缓冲区中
        /// </summary>
        [Description("巷道机缓存中")]
        RoadMachineStockBuffing = 50,

        /// <summary>
        /// 巷道机入库中
        /// </summary>
        [Description("巷道机入库中")]
        RoadMachineInStocking = 60,
        /// <summary>
        /// 入库成功
        /// </summary>
        [Description("入库成功")]
        InStocked = 70,

        [Description("手动入库成功")]
        ManInStocked = 71,

        //[Description("入库取消")]
        //IsCanceled = 80,

        [Description("巷道机出库初始")]
        RoadMachineOutStockInit = 90,

        [Description("巷道机出库分发")]
        RoadMachineWaitOutStockDispatch = 95,

        [Description("巷道机出库等待")]
        RoadMachineWaitOutStock = 100,

        [Description("巷道机出库中")]
        RoadMachineOutStocking = 110,

        [Description("出库成功")]
        OutStocked = 120,

        [Description("手动出库成功")]
        ManOutStocked=121,

        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Canceled =800,

        // Error
        [Description("无库位错误")]
        ErrorNoPositoin = 930,
        [Description("唯一码不存在")]
        ErrorUniqNotExsits = 940,
        [Description("唯一码无法入库")]
        ErrorUniqCannotInStock = 950,
        [Description("创建数据库任务失败")]
        ErrorCreateDbTask = 960,
        [Description("重复扫描")]
        ErrorBarcodeReScan = 970,

        [Description("入库失败")]
        ErrorInStock = 980,
        [Description("出库失败")]
        ErrorOutStock = 990
    }


}
