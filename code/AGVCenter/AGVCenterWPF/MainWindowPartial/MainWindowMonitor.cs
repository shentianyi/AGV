using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AGVCenterLib.Data;
using AGVCenterLib.Model;
using AGVCenterLib.Service;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        System.Timers.Timer monitorTaskTimer;
        private void InitMonitorFields()
        {
         TaskCenterForDisplayQueue = new List<StockTaskItem>();
            //     TaskCenterForDisplayQueue = new System.Collections.ObjectModel.ObservableCollection<StockTaskItem>();

            this.Dispatcher.Invoke(new Action(() =>
            {
                CenterStockTaskDisplayDG.ItemsSource = TaskCenterForDisplayQueue;
            }));

            this.monitorTaskTimer = new System.Timers.Timer();
            this.monitorTaskTimer.Enabled = true;
            this.monitorTaskTimer.Interval = 1000;
            this.monitorTaskTimer.Elapsed += MonitorTaskTimer_Elapsed;
           
        }

        private void StartMonitorTimers()
        {
            this.monitorTaskTimer.Start();
        }


        private void StopMonitorTimers()
        {
            if (this.monitorTaskTimer != null)
            {
                this.monitorTaskTimer.Enabled = false;
                this.monitorTaskTimer.Stop();
            }
        }
        private void MonitorTaskTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.monitorTaskTimer.Stop();
            this.LoadTasksFromDbToMonitor();
            this.monitorTaskTimer.Start();
        }


        private void LoadTasksFromDbToMonitor()
        {
            List<StockTask> tasks= new StockTaskService(OPCConfig.DbString).GetLastTasks().OrderBy(s=>s.Id).ToList();

            foreach(var task in tasks)
            {
                StockTaskItem item = this.InitTaskItemByStockTask(task);
                this.AddOrUpdateItemToTaskDisplay(item);
            }
           // this.RefreshList();
        }



        /// <summary>
        /// 更新Monitor的任务状态
        /// </summary>
        /// <param name="taskItem"></param>
        private void MonitorUpdateTaskState(StockTaskItem taskItem)
        {
            try
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

                    StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                    sts.UpdateTaskState(updatedStockTask);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }


    }
}
