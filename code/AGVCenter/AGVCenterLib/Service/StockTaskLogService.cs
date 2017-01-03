using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model.SearchModel;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterLib.Service
{
    public class StockTaskLogService : ServiceBase
    {

        public StockTaskLogService(string dbString) : base(dbString)
        {

        }

        public void CreateByStockTask(StockTask stockTask)
        {
            try
            {
                StockTaskLog stl = initByStockTask(stockTask);
                IStockTaskLogRepository taskRep = new StockTaskLogRepository(this.Context);
                taskRep.Create(stl);
                this.Context.SaveAll();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        public void CreateByStockTasks(List<StockTask> stockTasks)
        {
            try
            {
                List<StockTaskLog> logs = new List<StockTaskLog>();
                foreach (var stockTask in stockTasks)
                {
                    StockTaskLog stl = initByStockTask(stockTask);
                    logs.Add(stl);
                }
                IStockTaskLogRepository taskRep = new StockTaskLogRepository(this.Context);
                taskRep.Creates(logs);
                this.Context.SaveAll();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        public IQueryable<StockTaskLog> Search(StockTaskLogSearchModel searchModel)
        { 
             return new StockTaskLogRepository(this.Context).Search(searchModel);
        }

        private StockTaskLog initByStockTask(StockTask task)
        {
            StockTaskLog stl = new StockTaskLog()
            { StockTaskId = task.Id,
                BoxType = task.BoxType,
                RoadMachineIndex = task.RoadMachineIndex,
                PositionNr = task.PositionNr,
                PositionFloor = task.PositionFloor,
                PositionColumn = task.PositionColumn,
                PositionRow = task.PositionRow,
                AgvPassFlag = task.AgvPassFlag,
                RestPositionFlag = task.RestPositionFlag,
                BarCode = task.BarCode,
                FromState = task.State,
                Type = task.Type,
                TrayReverseNo = task.TrayReverseNo,
                TrayNum = task.TrayNum,
                PickItemNum = task.PickItemNum,
                PickBatchId = task.PickBatchId,
                TrayBatchId = task.TrayBatchId,
                PickListItemId=task.PickListItemId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            return stl;
        }
    }
}
