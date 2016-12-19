using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterLib.Service;
using AGVCenterWPF.Helper;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {

        #region 重发命令-重发的都是当前的命令 测试版
        /// <summary>
        /// AGV 放行重发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AGVRestBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            { 
                // OPCAgvInStockPassData.SyncWrite(OPCAgvInStockPassOPCGroup);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        #endregion

        #region 重置任务

        /// <summary>
        /// 重置所有数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestAllDataAndCleanBtn_Click(object sender, RoutedEventArgs e)
        {
            this.RestAllDataAndClean();
        }

        private void RestAllDataAndClean()
        {
            try
            {
                LogUtil.Logger.Info("【执行-重置所有数据】");
                SqlHelper.ExecuteNonQuery(OPCConfig.DbString, CommandType.StoredProcedure, "TASK_RestAllData");
                this.ClearTaskQueue();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 重置入库出库操作，重新开始
        /// </summary>
        private void RestAllTaskAndCleanBtn_Click(object sender, RoutedEventArgs e)
        {
            this.RestAllTaskAndStorage();
            this.ClearTaskQueue();
        }
      
        /// <summary>
        /// 重置出库任务状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestOutTaskAndCleanBtn_Click(object sender, RoutedEventArgs e)
        {
            this.RestOutStockTask();
            this.ClearTaskQueue();
        }


      

        /// <summary>
        /// 重置入库出库操作，重新开始
        /// </summary>
        private void RestAllTaskAndStorage()
        {
            try
            {
                LogUtil.Logger.Info("【执行-重置所以入库出库操作，重新开始】");
                SqlHelper.ExecuteNonQuery(OPCConfig.DbString, CommandType.StoredProcedure, "TASK_RestAllTaskAndStorage");
            }
            catch (Exception ex)
            {

                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 重置出库任务状态
        /// </summary>
        private void RestOutStockTask()
        {
            try
            {
                LogUtil.Logger.Info("【执行-重置出库任务状态】");
                SqlHelper.ExecuteNonQuery(OPCConfig.DbString, CommandType.StoredProcedure, "TASK_RestAllOutTaskToWaitingState");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
        /// <summary>
        /// 设置箱型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBoxTypeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LogUtil.Logger.Info("【执行-修改箱型】");
                string boxTypeId = (sender as Button).Name == "setBigBoxTypeButton" ? "1" : "2";
                SqlHelper.ExecuteNonQuery(OPCConfig.DbString, CommandType.StoredProcedure,
                    "SetUniqueItemBoxType", new SqlParameter("@boxTypeId", boxTypeId),
                    new SqlParameter("@nr", lbBoxTypeTB.Text));
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
        #endregion



            #region DEMO
            /// <summary>
            /// 设置OPC条码可以写
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
        private void SetAgvBarcodeCheckWritableBtn_Click(object sender, RoutedEventArgs e)
        {
            OPCCheckInStockBarcodeData.SyncSetWriteableFlag(OPCCheckInstockBarcodeOPCGroup);
        }

        private void SetAgvBarcodeCheckReadableBtn_Click(object sender, RoutedEventArgs e)
        {
            OPCCheckInStockBarcodeData.SyncSetReadableFlag(OPCCheckInstockBarcodeOPCGroup);
        }

        private void SetInRobootWritableBtn_Click(object sender, RoutedEventArgs e)
        {
            OPCInRobootPickData.SyncSetWriteableFlag(OPCInRobootPickOPCGroup);
        }


        private void SetInRobootReadableBtn_Click(object sender, RoutedEventArgs e)
        {
            OPCInRobootPickData.SyncSetReadableFlag(OPCInRobootPickOPCGroup);
        }

        /// <summary>
        /// 【测试所用】 清空任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearTasksForTestBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearTaskQueue();  
        }

        private void ClearTaskQueue()
        {
            prevScanedBarcode = string.Empty;
            if (AgvInStockPassQueue != null)
            {
                AgvInStockPassQueue.Clear();
            }
            if (InRobootPickQueue != null)
            {
                InRobootPickQueue.Clear();
            }
            if (AgvScanTaskQueue != null)
            {
                AgvScanTaskQueue.Clear();
            }
            if (RoadMachine1TaskQueue != null)
            {
                RoadMachine1TaskQueue.Clear();
            }

            if (RoadMachine2TaskQueue != null)
            {
                RoadMachine2TaskQueue.Clear();
            }

            if (OutStockCenterQueue != null)
            {
                OutStockCenterQueue.Clear();
            }

            if (TaskCenterForDisplayQueue != null)
            {
                TaskCenterForDisplayQueue.Clear();
            }

            RefreshList();
        }

       

        /// <summary>
        /// 手动创建入库任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createInStockBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IncreaseBarcodeCheckBox.IsChecked.Value)
            {
                ScanedBarCodeTB.Text = (int.Parse(ScanedBarCodeTB.Text) + 1).ToString();
            }
            StockTaskItem taskItem = new StockTaskItem()
            {
                Barcode = ScanedBarCodeTB.Text,
                StockTaskType = StockTaskType.IN,
                RoadMachineIndex = int.Parse(RoadMachineIndexTB.Text),
                State = StockTaskState.RoadMachineStockBuffing,
                BoxType = (byte)1
            };
            taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);
            UniqueItemService ui = new UniqueItemService(OPCConfig.DbString);
            if (ui.FindByCheckCode(taskItem.Barcode) == null)
            {
                ui.Create(new UniqueItem()
                {
                    Nr = ScanedBarCodeTB.Text,
                    CheckCode = ScanedBarCodeTB.Text,
                    BoxTypeId = 1
                });
            }

            StockTaskService ts = new StockTaskService(OPCConfig.DbString);
            ts.CreateInStockTask(taskItem);
            if (int.Parse(RoadMachineIndexTB.Text) == 1)
            {
                //  RoadMachine1TaskQueue.Add(taskItem.Barcode, taskItem);
                RoadMachine1TaskQueue.Enqueue(taskItem);
            }
            else if (int.Parse(RoadMachineIndexTB.Text) == 2)
            {
                // RoadMachine2TaskQueue.Add(taskItem.Barcode, taskItem);
                RoadMachine2TaskQueue.Enqueue(taskItem);
            }
            TaskCenterForDisplayQueue.Add(taskItem);
            if (LipRoadMachineCheckBox.IsChecked.Value)
            {
                RoadMachineIndexTB.Text = RoadMachineIndexTB.Text == "1" ? "2" : "1";
            }
            RefreshList();
        }


        #endregion
    }
}
