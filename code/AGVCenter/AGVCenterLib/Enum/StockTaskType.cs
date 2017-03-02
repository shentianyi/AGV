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
        NONE = 0,
        
        /// <summary>
        /// OUT
        /// </summary>
        [Description("出库")]
        OUT = 1,

        /// <summary>
        /// IN
        /// </summary>
        [Description("入库")]
        IN = 2,


        /// <summary>
        /// MOVE
        /// </summary>
        [Description("自动移库")]
        AUTO_MOVE = 3,

        ///// <summary>
        ///// AUTO_MOVE
        ///// </summary>
        //[Description("自动移库")]
        //AUTO_MOVE=3,


        /// <summary>
        /// CHECK
        /// </summary>
        [Description("盘点")]
        CHECK = 5
    }
}
