using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.OPC;

namespace AGVCenterLib.Service
{
    public class StockTaskService : ServiceBase
    {

        public StockTaskService(string dbString) : base(dbString)
        {
        }
        /// <summary>
        /// 创建入库任务
        /// </summary>
        /// <returns></returns>
        public bool CreateInStockTask(OPCSetInStockTask task)
        {
            StockTask st = new StockTask()
            {
                BoxType = task.BoxType,
                PositionFloor = task.PositionFloor,
                PositionColumn = task.PositionColumn,
                PositionRow = task.PositionRow,
                AgvPassFlag = task.AgvPassFlag,
                RestPositionFlag = task.RestPositionFlag,
                BarCode = task.Barcode,
                State = (int)task.State,
                Type = (int)StockTaskType.In,
                CreatedAt = DateTime.Now
            };
            this.Context.StockTask.InsertOnSubmit(st);
            this.Context.SubmitChanges();
            task.DbId = st.id;
            return true;
        }
    }
}
