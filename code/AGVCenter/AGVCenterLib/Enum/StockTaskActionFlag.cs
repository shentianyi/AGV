using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Enum
{
    public enum StockTaskActionFlag
    {
        //运行中
      //  Excuting = 0,
        //入库成功
        InSuccess = 1,
        //入库失败，目标库位存在货物
        InFailPositionWasStored = 2,

        ///入库失败，库位不存在
        InFailPositionNotExists = 3,

        //入库失败，其他错误
        InFailUnKnown = 4,

        //出库成功
        OutSuccess = 5,

        //出库失败，库位无货
        OutFailStoreNotFound = 6,

        //出库失败，条码不匹配
        OutFailBarNotMatch = 7,

        //出库失败，库位不存在
        OutFailPositionNotExists = 8,

        //出库失败，其他错误
        OutFailUnKnown = 9,

        //盘点匹配成功
        InventorySuccess = 10,
        //盘点匹配失败
        InventoryFail = 11,

        /// <summary>
        /// 工作成功完成
        /// </summary>
        Excuting = 101,
        Success = 104,
        Fail=105
    }
}
