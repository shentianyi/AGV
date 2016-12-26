using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Model;
using AGVCenterLib.Service;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        System.Timers.Timer monitorTaskTimer;
        private void InitMonitorFields()
        {
            TaskCenterForDisplayQueue = new List<StockTaskItem>();
      

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
    }
}
