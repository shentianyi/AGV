using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
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
        public bool CreateInStockTask(StockTaskItem task)
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
                Type = (int)StockTaskType.IN,
                CreatedAt = DateTime.Now
            };
            IStockTaskRepository stRep = new StockTaskRepository(this.Context);
            stRep.Create(st);
            this.Context.SaveAll();
            task.DbId = st.id;
            return true;
        }

        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskItem"></param>
        /// <returns></returns>
        public bool UpdateTaskState(StockTaskItem taskItem)
        {
            IStockTaskRepository stockTaskRep = new StockTaskRepository(this.Context);
            StockTask t = stockTaskRep.FindById(taskItem.DbId);
            if (t != null)
            {
                t.State = (int)taskItem.State;

                t.RoadMachineIndex = taskItem.RoadMachineIndex;
                t.PositionNr = taskItem.PositionNr;
                t.PositionFloor = taskItem.PositionFloor;
                t.PositionColumn = taskItem.PositionColumn;
                t.PositionRow = taskItem.PositionRow;
                
                this.Context.SaveAll();
            }
            
            return true;
        }
    }
}
