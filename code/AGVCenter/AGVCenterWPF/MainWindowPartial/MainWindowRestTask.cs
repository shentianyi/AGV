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
using AGVCenterWPF.Config;
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
                SqlHelper.ExecuteNonQuery(OPCConfig.DbString,
                    CommandType.StoredProcedure, "TASK_RestAllOutTaskToWaitingState",new SqlParameter("@toState",(int)StockTaskState.RoadMachineWaitOutStock));
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
                //for (var i = 0; i < 100; i++)
                //{
                LogUtil.Logger.Info("【执行-修改箱型】");
                string boxTypeId = (sender as Button).Name == "setBigBoxTypeButton" ? "1" : "2";
                SqlHelper.ExecuteNonQuery(OPCConfig.DbString, CommandType.StoredProcedure,
                    "SetUniqueItemBoxType", new SqlParameter("@boxTypeId", boxTypeId),
                    new SqlParameter("@nr", lbBoxTypeTB.Text));

                // string boxTypeId = i % 2 == 0 ? "1" : "2";

                //SqlHelper.ExecuteNonQuery(OPCConfig.DbString, CommandType.StoredProcedure,
                //                    "SetUniqueItemBoxType", new SqlParameter("@boxTypeId", boxTypeId),
                //                     new SqlParameter("@nr", i.ToString()));


                //}
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        /// <summary>
        /// 取消任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            //prevScanedBarcode = string.Empty;
            if (CenterStockTaskDisplayDG.SelectedIndex > -1)
            {
                StockTaskItem taskItem = CenterStockTaskDisplayDG.SelectedItem as StockTaskItem;
                if (BaseConfig.IsOPCConnector)
                {
                    AgvScanTaskQueue.Remove(taskItem.Barcode);
                    if (taskItem.IsCanCancel)
                    {
                        if (taskItem.State == StockTaskState.AgvWaitPassing)
                        {
                            var t1 = AgvInStockPassQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                            if (t1 != null)
                            {
                                (t1 as StockTaskItem).State = StockTaskState.Canceled;
                            }

                        }
                        else if (taskItem.State == StockTaskState.AgvInStcoking)
                        {
                            var t1 = InRobootPickQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                            if (t1 != null)
                            {
                                (t1 as StockTaskItem).State = StockTaskState.Canceled;
                            }
                        }
                        else
                        {
                            if (taskItem.RoadMachineIndex == 1)
                            {
                                var t1 = RoadMachine1TaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                                if (t1 != null)
                                {
                                    (t1 as StockTaskItem).State = StockTaskState.Canceled;
                                }
                            }
                            else if (taskItem.RoadMachineIndex == 2)
                            {
                                var t1 = RoadMachine2TaskQueue.ToArray().FirstOrDefault(s => (s as StockTaskItem).DbId == taskItem.DbId);
                                if (t1 != null)
                                {
                                    (t1 as StockTaskItem).State = StockTaskState.Canceled;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void manInButton_Click(object sender, RoutedEventArgs e)
        {
            if (CenterStockTaskDisplayDG.SelectedIndex > -1)
            {
                StockTaskItem taskItem = CenterStockTaskDisplayDG.SelectedItem as StockTaskItem;

                if (taskItem.IsCanCancel)
                {
                   // this.prevScanedBarcode = string.Empty;
                    taskItem.State = StockTaskState.ManInStocked;
                  //  this.UpdateDbTask(taskItem);
                    //if (taskItem.RoadMachineIndex == 1)
                    //{
                       // RoadMachine1TaskQueue.Dequeue();
                   // }
                    //else if (taskItem.RoadMachineIndex == 2)
                    //{
                    //    RoadMachine2TaskQueue.Dequeue();
                    //}
                }

            }
        }



        private void manOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (CenterStockTaskDisplayDG.SelectedIndex > -1)
            {
                StockTaskItem taskItem = CenterStockTaskDisplayDG.SelectedItem as StockTaskItem;

                if (taskItem.IsCanCancel)
                {
                    taskItem.State = StockTaskState.ManOutStocked;
                   // this.UpdateDbTask(taskItem);

                    if (taskItem.RoadMachineIndex == 1)
                    {
                        OPCDataResetData.IncrOutrootPickCount(OPCDataResetOPCGroup);
                    }
                    else if (taskItem.RoadMachineIndex == 2)
                    {
                        OPCDataResetData.IncrOutrootPickCount(OPCDataResetOPCGroup);
                    }
                }

            }
        }
        /// <summary>
        /// 重置入库平台1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBuff1InStockBtn_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("确定重置1？", "确定重置1?", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                OPCDataResetData.RestXdj1InPaltformIsBuff(OPCDataResetOPCGroup);
            }
        }


        /// <summary>
        /// 重置入库平台2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBuff2InStockBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定重置2？", "确定重置2?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                OPCDataResetData.RestXdj2InPaltformIsBuff(OPCDataResetOPCGroup);
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
          //  prevScanedBarcode = string.Empty;
            if (AgvInStockPassQueue != null)
            {
                AgvInStockPassQueue.Clear();
            }
           
            if (AgvScanTaskQueue != null)
            {
                AgvScanTaskQueue.Clear();
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
            return;
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
       //     taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);
            UniqueItemService ui = new UniqueItemService(OPCConfig.DbString);
            //if (ui.FindByCheckCode(taskItem.Barcode) == null)
            if (ui.FindByNr(taskItem.Barcode) == null)
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
              //  RoadMachine1TaskQueue.Enqueue(taskItem);
            }
            else if (int.Parse(RoadMachineIndexTB.Text) == 2)
            {
                // RoadMachine2TaskQueue.Add(taskItem.Barcode, taskItem);
            //    RoadMachine2TaskQueue.Enqueue(taskItem);
            }

            TaskCenterForDisplayQueue.Add(taskItem);

          //  AddOrUpdateItemToTaskDisplay(taskItem);

            if (LipRoadMachineCheckBox.IsChecked.Value)
            {
                RoadMachineIndexTB.Text = RoadMachineIndexTB.Text == "1" ? "2" : "1";
            }
            //# RefreshList();
        }


        /// <summary>
        /// 重置前一个扫描记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restRescanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AgvScanTaskQueue.Remove(BaseConfig.PreScanBar);
            }
            catch (Exception ex)
            {

            }
            BaseConfig.PreScanBar = string.Empty;
        }
        #endregion
    }
}
