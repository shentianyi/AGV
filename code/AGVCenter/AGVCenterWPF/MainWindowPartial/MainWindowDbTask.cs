using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterLib.Service;
using AGVCenterWPF.Properties;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        private object createDbTaskLocker = new object();

        private bool CreateDbTask(StockTaskItem taskItem)
        {
            lock (this.createDbTaskLocker)
            {
                StockTaskItem item = this.GetDbTask(StockTaskState.AgvInStcoking);
                if (item.Barcode == taskItem.Barcode && taskItem.AgvPassFlag == (byte)AgvPassFlag.Pass)
                {
                    // 不重复创建放行任务
                    return true;
                }
                else
                {
                    StockTaskService ts = new StockTaskService(OPCConfig.DbString);

                    return ts.CreateInStockTask(taskItem);
                }
            }
        }


        private object updateDbTaskLocker = new object();
        private void UpdateDbTask(StockTaskItem taskItem)
        {
            lock (updateDbTaskLocker)
            {
                if (taskItem.DbId > 0)
                {
                    LogUtil.Logger.Info("***********************************************************");
                    LogUtil.Logger.InfoFormat("任务状态变化:bar:{0}-dbid: {1}-position: {2}:{3}------->{4}",
                        taskItem.Barcode,
                        taskItem.DbId,
                        taskItem.PositionNr,
                        taskItem.StateWas,
                        taskItem.State);
                    LogUtil.Logger.Info("***********************************************************");

                    StockTask updatedStockTask = new StockTask();

                    updatedStockTask.Id = taskItem.DbId;
                    updatedStockTask.State = (int)taskItem.State;

                    updatedStockTask.RoadMachineIndex = taskItem.RoadMachineIndex;
                    updatedStockTask.PositionNr = taskItem.PositionNr;
                    updatedStockTask.PositionFloor = taskItem.PositionFloor;
                    updatedStockTask.PositionColumn = taskItem.PositionColumn;
                    updatedStockTask.PositionRow = taskItem.PositionRow;

//                    receiveMessageQueue.Enqueue(updatedStockTask);
  //                  receivedEvent.Set();
                    StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                    sts.UpdateTaskState(updatedStockTask);
                }

            }
        }

        private object getDbTaskLocker = new object();
        private StockTaskItem GetDbTask(StockTaskState state, int? roadMachineIndex=null)
        {
            lock (this.getDbTaskLocker)
            {
                StockTask st = new StockTaskService(Settings.Default.dbString).GetTaskByStateAndRoadMachine(state, roadMachineIndex);
                if (st != null)
                {
                    StockTaskItem taskItem = new StockTaskItem()
                    {
                        StockTaskType = StockTaskType.OUT,
                        Barcode = st.BarCode,
                        BoxType = (byte)st.BoxType,
                        PositionNr = st.PositionNr,
                        PositionFloor = (byte)st.PositionFloor,
                        PositionColumn = (byte)st.PositionColumn,
                        PositionRow = (byte)st.PositionRow,
                        RoadMachineIndex = st.RoadMachineIndex.Value,

                        TrayReverseNo = st.TrayReverseNo.HasValue ? st.TrayReverseNo.Value : 0,
                        TrayNum = st.TrayNum.HasValue ? st.TrayNum.Value : 0,
                        DeliveryItemNum = st.DeliveryItemNum.HasValue ? st.DeliveryItemNum.Value : 0,
                        State = (StockTaskState)st.State,
                        DbId = st.Id,
                        IsInProcessing = true
                    };
                  //  taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);
                    return taskItem;
                }
                else
                {
                    return null;
                }
            }
        } 

    }
}
