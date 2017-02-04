using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterWPF.Config;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        #region 巷道机任务队列定义


      //  Queue RoadMachine1TaskQueue;

       // Queue RoadMachine2TaskQueue;

        // 巷道机1 任务队列
        /// <summary>
        /// 巷道机1 总任务队列
        /// </summary>
        Queue RoadMachine1CenterTaskQueue;
        /// <summary>
        /// 巷道机1 入库任务队列
        /// </summary>
        Queue RoadMachine1InTaskQueue;
        /// <summary>
        /// 巷道机1 出库任务队列
        /// </summary>
        Queue RoadMachine1OutTaskQueue;

        // 巷道机2 任务队列
        /// <summary>
        /// 巷道机2 总任务队列
        /// </summary>
        Queue RoadMachine2CenterTaskQueue;
        /// <summary>
        /// 巷道机2 入库任务队列
        /// </summary>
        Queue RoadMachine2InTaskQueue;
        /// <summary>
        /// 巷道机2 出库任务队列
        /// </summary>
        Queue RoadMachine2OutTaskQueue;
        #endregion

        private object roadMachineTaskQueueLocker = new object();
        /// <summary>
        /// 巷道机任务添加
        /// </summary>
        /// <param name="taskItem"></param>
        private void EnqueueRoadMachineTask(StockTaskItem taskItem)
        {
            lock (roadMachineTaskQueueLocker)
            {
                if (taskItem.RoadMachineIndex == 1)
                {
                    if (taskItem.StockTaskType == StockTaskType.IN)
                    {
                        this.RoadMachine1InTaskQueue.Enqueue(taskItem);
                    }
                    else if (taskItem.StockTaskType == StockTaskType.OUT)
                    {
                        this.RoadMachine1OutTaskQueue.Enqueue(taskItem);
                    }
                }
                else if (taskItem.RoadMachineIndex == 2)
                {
                    if (taskItem.StockTaskType == StockTaskType.IN)
                    {
                        this.RoadMachine2InTaskQueue.Enqueue(taskItem);
                    }
                    else if (taskItem.StockTaskType == StockTaskType.OUT)
                    {
                        this.RoadMachine2OutTaskQueue.Enqueue(taskItem);
                    }
                }


            }
        }

        /// <summary>
        /// 巷道机任务分发
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <returns></returns>
        private StockTaskItem DequeueRoadmMachineTaskToCenter(int roadMachineIndex)
        {
            lock (roadMachineTaskQueueLocker)
            {
                if (roadMachineIndex == 1)
                {
                    #region 巷道机1 运行模式
                    if (ModeConfig.RoadMachine1TaskMode == RoadMachineTaskModel.OutHigherThanIn)
                    {
                        if (RoadMachine1OutTaskQueue.Count > 0)
                        {
                            RoadMachine1CenterTaskQueue.Enqueue(RoadMachine1OutTaskQueue.Dequeue());

                        }
                        else if (RoadMachine1InTaskQueue.Count > 0)
                        {
                            RoadMachine1CenterTaskQueue.Enqueue(RoadMachine1InTaskQueue.Dequeue());

                        }
                        else
                        {

                        }
                    }
                    else if (ModeConfig.RoadMachine1TaskMode == RoadMachineTaskModel.InHigherThanOut)
                    {
                        if (RoadMachine1InTaskQueue.Count > 0)
                        {
                            RoadMachine1CenterTaskQueue.Enqueue(RoadMachine1InTaskQueue.Dequeue());
                        }
                        else if (RoadMachine1OutTaskQueue.Count > 0)
                        {
                            RoadMachine1CenterTaskQueue.Enqueue(RoadMachine1OutTaskQueue.Dequeue());

                        }
                    }
                    else if (ModeConfig.RoadMachine1TaskMode == RoadMachineTaskModel.OnlyOut)
                    {
                        if (RoadMachine1OutTaskQueue.Count > 0)
                        {
                            RoadMachine1CenterTaskQueue.Enqueue(RoadMachine1OutTaskQueue.Dequeue());
                        }
                    }
                    else if (ModeConfig.RoadMachine1TaskMode == RoadMachineTaskModel.OnlyIn)
                    {
                        if (RoadMachine1InTaskQueue.Count > 0)
                        {
                            RoadMachine1CenterTaskQueue.Enqueue(RoadMachine1InTaskQueue.Dequeue());
                        }
                    }
                    #endregion
                }
                else if (roadMachineIndex == 2)
                {
                    #region 巷道机2模式
                    if (ModeConfig.RoadMachine2TaskMode == RoadMachineTaskModel.OutHigherThanIn)
                    {
                        if (RoadMachine2OutTaskQueue.Count > 0)
                        {
                            RoadMachine2CenterTaskQueue.Enqueue(RoadMachine2OutTaskQueue.Dequeue());

                        }
                        else if (RoadMachine2InTaskQueue.Count > 0)
                        {
                            RoadMachine2CenterTaskQueue.Enqueue(RoadMachine2InTaskQueue.Dequeue());

                        }
                        else
                        {

                        }
                    }
                    else if (ModeConfig.RoadMachine2TaskMode == RoadMachineTaskModel.InHigherThanOut)
                    {
                        if (RoadMachine2InTaskQueue.Count > 0)
                        {
                            RoadMachine2CenterTaskQueue.Enqueue(RoadMachine2InTaskQueue.Dequeue());

                        }
                        else if (RoadMachine2OutTaskQueue.Count > 0)
                        {
                            RoadMachine2CenterTaskQueue.Enqueue(RoadMachine2OutTaskQueue.Dequeue());

                        }
                        else
                        {

                        }
                    }
                    else if (ModeConfig.RoadMachine2TaskMode == RoadMachineTaskModel.OnlyOut)
                    {
                        if (RoadMachine2OutTaskQueue.Count > 0)
                        {
                            RoadMachine2CenterTaskQueue.Enqueue(RoadMachine2OutTaskQueue.Dequeue());
                        }
                    }
                    else if (ModeConfig.RoadMachine2TaskMode == RoadMachineTaskModel.OnlyIn)
                    {
                        if (RoadMachine2InTaskQueue.Count > 0)
                        {
                            RoadMachine2CenterTaskQueue.Enqueue(RoadMachine2InTaskQueue.Dequeue());
                        }
                    }
                    #endregion
                }
                Queue queue = roadMachineIndex == 1 ? RoadMachine1CenterTaskQueue : RoadMachine2CenterTaskQueue;
                if (queue != null)
                {
                    if (queue.Count == 0)
                    {
                        return null;
                    }

                    if((queue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                    {
                        queue.Dequeue();
                        return null;
                    }
                    else
                    {
                        if (queue.ToArray().Count(s => (s as StockTaskItem).IsInBuffingState) == 0)
                        {
                            return null;
                        }


                        StockTaskItem taskItem = queue.Peek() as StockTaskItem;
                        if (taskItem.IsInBuffingState)
                        {
                            if (taskItem.StockTaskType == StockTaskType.IN)
                            {
                                Position position = GetPositionForDispatch(roadMachineIndex);

                                taskItem.PositionNr = position.Nr;
                                taskItem.PositionFloor = position.Floor;
                                taskItem.PositionColumn = position.Column;
                                taskItem.PositionRow = position.Row;
                            }
                        }
                        return taskItem;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 取消巷道机任务
        /// </summary>
        /// <param name="roadMacineIndex"></param>
        private void CancelRoadMachineTask(StockTaskItem taskItem)
        {
            try {
                lock (roadMachineTaskQueueLocker)
                {
                    if (taskItem.RoadMachineIndex == 1)
                    {
                        var t = RoadMachine1CenterTaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                        if (t != null)
                        {
                            (t as StockTaskItem).State = StockTaskState.Canceled;

                            if ((RoadMachine1CenterTaskQueue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                            {
                                RoadMachine1CenterTaskQueue.Dequeue();
                            }
                        }
                        t = RoadMachine1InTaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                        if (t != null)
                        {
                            (t as StockTaskItem).State = StockTaskState.Canceled;

                            if ((RoadMachine1InTaskQueue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                            {
                                RoadMachine1InTaskQueue.Dequeue();
                            }

                        }
                        t = RoadMachine1OutTaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                        if (t != null)
                        {
                            (t as StockTaskItem).State = StockTaskState.Canceled;

                            if ((RoadMachine1OutTaskQueue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                            {
                                RoadMachine1OutTaskQueue.Dequeue();
                            }
                        }
                    }
                    else if (taskItem.RoadMachineIndex == 2)
                    {
                        var t = RoadMachine2CenterTaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                        if (t != null)
                        {
                            (t as StockTaskItem).State = StockTaskState.Canceled;
                            if ((RoadMachine2CenterTaskQueue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                            {
                                RoadMachine2CenterTaskQueue.Dequeue();
                            }
                        }
                        t = RoadMachine2InTaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                        if (t != null)
                        {
                            (t as StockTaskItem).State = StockTaskState.Canceled;
                            if ((RoadMachine2InTaskQueue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                            {
                                RoadMachine2InTaskQueue.Dequeue();
                            }
                        }
                        t = RoadMachine2OutTaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                        if (t != null)
                        {
                            (t as StockTaskItem).State = StockTaskState.Canceled;
                            if ((RoadMachine2OutTaskQueue.Peek() as StockTaskItem).ShouldDequeueStockTask)
                            {
                                RoadMachine2OutTaskQueue.Dequeue();
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                LogUtil.Logger.Error(ex.Message, ex);
                
            }
        }
        /// <summary>
        /// 初始化队列
        /// </summary>
        private void InitRoadMachineTaskQueue()
        {
            #region 初始化巷道机任务队列
          //  RoadMachine1TaskQueue = new Queue();
           // RoadMachine2TaskQueue = new Queue();

            RoadMachine1CenterTaskQueue = new Queue();

            RoadMachine1InTaskQueue = new Queue();

            RoadMachine1OutTaskQueue = new Queue();

            RoadMachine2CenterTaskQueue = new Queue();

            RoadMachine2InTaskQueue = new Queue();

            RoadMachine2OutTaskQueue = new Queue();
            #endregion
        }
    }
}
