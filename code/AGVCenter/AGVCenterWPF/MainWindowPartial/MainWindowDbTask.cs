using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterLib.Service;
using AGVCenterWPF.Config;
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
                StockTaskItem item = this.GetDbTask(StockTaskItem.InPickRobotGetDbStates);
                if (item!=null && item.Barcode == taskItem.Barcode && taskItem.AgvPassFlag == (byte)AgvPassFlag.Pass)
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

               // AddOrUpdateItemToTaskDisplay(taskItem);


            }
        }

        private object getDbTaskLocker = new object();
        private StockTaskItem GetDbTask(List<StockTaskState> states, int? roadMachineIndex=null)
        {
            lock (this.getDbTaskLocker)
            {
                StockTask task = new StockTaskService(Settings.Default.dbString)
                    .GetTaskByStatesAndRoadMachine(states, roadMachineIndex);
                if (task != null)
                {
                    StockTaskItem taskItem = new StockTaskItem()
                    {
                        RoadMachineIndex = task.RoadMachineIndex.HasValue ? task.RoadMachineIndex.Value : 0,
                        BoxType = task.BoxType.HasValue ? (byte)task.BoxType.Value : (byte)0,
                        PositionNr = task.PositionNr,
                        PositionFloor = task.PositionRow.HasValue ? task.PositionFloor.Value : 0,
                        PositionColumn = task.PositionColumn.HasValue ? task.PositionColumn.Value : 0,
                        PositionRow = task.PositionRow.HasValue ? task.PositionRow.Value : 0,
                        AgvPassFlag = task.AgvPassFlag.HasValue ? (byte)task.AgvPassFlag.Value : (byte)0,
                        RestPositionFlag = task.RestPositionFlag.HasValue ? (byte)task.RestPositionFlag.Value : (byte)0,
                        Barcode = task.BarCode,
                        State = task.State.HasValue ? (StockTaskState)task.State.Value : StockTaskState.Init,
                        StockTaskType = task.Type.HasValue ? (StockTaskType)task.Type.Value : StockTaskType.NONE,
                        TrayReverseNo = task.TrayReverseNo.HasValue ? task.TrayReverseNo.Value : 0,
                        TrayNum = task.TrayNum.HasValue ? task.TrayNum.Value : 0,
                        DeliveryItemNum = task.DeliveryItemNum.HasValue ? task.DeliveryItemNum.Value : 0,
                        DbId = task.Id,
                        CreatedAt = task.CreatedAt.Value,
                        IsInProcessing = true
                    };

                   // AddOrUpdateItemToTaskDisplay(taskItem);
                    //  taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);
                    return taskItem;
                }
                else
                {
                    return null;
                }
            }
        } 



        //private void RefreshTaskList()
        //{
        //    List<StockTask> tasks = new StockTaskService(Settings.Default.dbString).GetLastTasks(BaseConfig.KeepMonitorTaskNum);
        //    foreach(var task in tasks)
        //    {

        //    }
        //}
    }
}
