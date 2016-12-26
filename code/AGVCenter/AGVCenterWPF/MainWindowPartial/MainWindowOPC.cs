using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AGVCenterLib.Model.OPC;
using Brilliantech.Framwork.Utils.LogUtil;
using OPCAutomation;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {

        #region  OPC 基础数据变量

        // 入库条码扫描
        OPCCheckInStockBarcode OPCCheckInStockBarcodeData;
        OPCGroup OPCCheckInstockBarcodeOPCGroup;
        // 小车入库放行
        OPCAgvInStockPass OPCAgvInStockPassData;
        OPCGroup OPCAgvInStockPassOPCGroup;
        // 入库机械手抓取
        OPCInRobootPick OPCInRobootPickData;
        OPCGroup OPCInRobootPickOPCGroup;

        // 库存任务, 巷道机1&2
        // 1
        OPCSetStockTask OPCSetStockTaskRoadMachine1Data;
        OPCGroup OPCSetStockTaskRoadMachine1OPCGroup;
        // 2
        OPCSetStockTask OPCSetStockTaskRoadMachine2Data;
        OPCGroup OPCSetStockTaskRoadMachine2OPCGroup;

        // 巷道机任务反馈，巷道机1&2
        // 1
        OPCRoadMachineTaskFeed OPCRoadMachine1TaskFeedData;
        OPCGroup OPCRoadMachine1TaskFeedOPCGroup;
        // 2
        OPCRoadMachineTaskFeed OPCRoadMachine2TaskFeedData;
        OPCGroup OPCRoadMachine2TaskFeedOPCGroup;


        // 出库机械手码垛
        OPCOutRobootPick OPCOutRobootPickData;
        OPCGroup OPCOutRobootPickOPCGroup;


        OPCDataReset OPCDataResetData;
        OPCGroup OPCDataResetOPCGroup;

        #endregion

        /// <summary>
        ///  初始化OPC
        /// </summary>
        private void InitOPC()
        {
            #region 初始化基本数据
            foreach (var i in OPCConfig.OPCServers)
            {
                OPCServersLB.Items.Add(i);
            }
            OPCServerTB.Text = OPCConfig.OPCServerName;
            OPCNodeNameTB.Text = OPCConfig.OPCNodeName;

            #endregion

            #region 初始化OPC数据
            // 扫描入库码
            OPCCheckInStockBarcodeData = new OPCCheckInStockBarcode();
            OPCCheckInStockBarcodeData.RwFlagChangedEvent += new OPCCheckInStockBarcode.RwFlagChangedEventHandler(OPCCheckInStockBarcodeData_RwFlagChangedEvent);

            // Agv小车入库放行
            OPCAgvInStockPassData = new OPCAgvInStockPass();
            OPCAgvInStockPassData.RwFlagChangedEvent += new OPCAgvInStockPass.RwFlagChangedEventHandler(OPCAgvInStockPassData_RwFlagChangedEvent);

            // 入库机械手抓取
            OPCInRobootPickData = new OPCInRobootPick();
            OPCInRobootPickData.RwFlagChangedEvent += new OPCInRobootPick.RwFlagChangedEventHandler(OPCInRobootPickData_RwFlagChangedEvent);

            //库存操作任务  巷道机1
            OPCSetStockTaskRoadMachine1Data = new OPCSetStockTask("SetStockTaskRoadMachine1", 1);
            OPCSetStockTaskRoadMachine1Data.RwFlagChangedEvent += new OPCSetStockTask.RwFlagChangedEventHandler(OPCSetStockTaskRoadMachine1Data_RwFlagChangedEvent);

            //库存操作任务 巷道机2
            OPCSetStockTaskRoadMachine2Data = new OPCSetStockTask("SetStockTaskRoadMachine2", 2);
            OPCSetStockTaskRoadMachine2Data.RwFlagChangedEvent += new OPCSetStockTask.RwFlagChangedEventHandler(OPCSetStockTaskRoadMachine2Data_RwFlagChangedEvent);

            //库存任务反馈 巷道机1
            OPCRoadMachine1TaskFeedData = new OPCRoadMachineTaskFeed("OPCRoadMachine1TaskFeed", 1);
            OPCRoadMachine1TaskFeedData.ActionFlagChangeEvent += new OPCRoadMachineTaskFeed.ActionFlagChangeEventHandler(OPCRoadMachine1TaskFeedData_ActionFlagChangeEvent);

            //库存任务反馈 巷道机2
            OPCRoadMachine2TaskFeedData = new OPCRoadMachineTaskFeed("OPCRoadMachine2TaskFeed", 2);
            OPCRoadMachine2TaskFeedData.ActionFlagChangeEvent += new OPCRoadMachineTaskFeed.ActionFlagChangeEventHandler(OPCRoadMachine1TaskFeedData_ActionFlagChangeEvent);

            //出库机械手
            OPCOutRobootPickData = new OPCOutRobootPick();
            OPCOutRobootPickData.RwFlagChangedEvent += new OPCOutRobootPick.RwFlagChangedEventHandler(OPCOutRobootPickData_RwFlagChangedEvent);

            // 设置OPC值
            OPCDataResetData = new OPCDataReset();

            #endregion
        }
       
        /// <summary>
        /// 连接OPC服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectOPCServerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ConnectOPC();
        }

        /// <summary>
        /// 连接OPC
        /// </summary>
        private void ConnectOPC()
        {
            try
            {
                ConnectedOPCServer = new OPCServer();
                //  ConnectedOPCServer.ServerShutDown += ConnectedOPCServer_ServerShutDown;

                ConnectedOPCServer.Connect(OPCServerTB.Text, OPCNodeNameTB.Text);
                ConnectedOPCServer.OPCGroups.DefaultGroupIsActive = true;
                ConnectedOPCServer.OPCGroups.DefaultGroupDeadband = 0;

                /// 初始化OPC组
                if (InitOPCGroup())
                {
                    ConnectOPCServerBtn.IsEnabled = false;
                    StartComponents();
                }
            }
            catch (Exception ex)
            {
                ConnectedOPCServer = null;
                ConnectOPCServerBtn.IsEnabled = true;
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 列出OPC服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOPCServerBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AnOPCServer = new OPCServer();
                OPCServersLB.Items.Clear();
                dynamic allServerList = OPCNodeNameTB.Text.Length == 0 ? AnOPCServer.GetOPCServers() : AnOPCServer.GetOPCServers(OPCNodeNameTB.Text);

                foreach (var s in (allServerList as Array))
                {
                    OPCServersLB.Items.Add(s);
                }
                AnOPCServer = null;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 选择OPC服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OPCServersLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OPCServerTB.Text = OPCServersLB.SelectedValue.ToString();
        }

        /// <summary>
        /// 断开OPC服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisConnectOPCServerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DisconnectOPCServer();
        }

        /// <summary>
        /// 初始化OPC组
        /// </summary>
        private bool InitOPCGroup()
        {
            try
            {
                #region 初始化入库扫描验证组
                // 初始化入库扫描验证组
                OPCCheckInstockBarcodeOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCCheckInStockBarcodeOPCGroupName);
                OPCCheckInstockBarcodeOPCGroup.UpdateRate = OPCConfig.OPCCheckInStockBarcodeOPCGroupRate;
                OPCCheckInstockBarcodeOPCGroup.DeadBand = OPCConfig.OPCCheckInStockBarcodeOPCGroupDeadBand;
                OPCCheckInstockBarcodeOPCGroup.IsSubscribed = true;
                OPCCheckInstockBarcodeOPCGroup.IsActive = true;

                // 添加item
                OPCCheckInStockBarcodeData.AddItemToGroup(OPCCheckInstockBarcodeOPCGroup);
                OPCCheckInstockBarcodeOPCGroup.DataChange += OPCCheckInStockBarcodeOPCGroup_DataChange;
                #endregion

                #region Agv入库放行组
                OPCAgvInStockPassOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCAgvInStockPassOPCGroupName);
                OPCAgvInStockPassOPCGroup.UpdateRate = OPCConfig.OPCAgvInStockPassOPCGroupRate;
                OPCAgvInStockPassOPCGroup.DeadBand = OPCConfig.OPCAgvInStockPassOPCGroupDeadBand;
                OPCAgvInStockPassOPCGroup.IsSubscribed = true;
                OPCAgvInStockPassOPCGroup.IsActive = true;

                // 添加item
                OPCAgvInStockPassData.AddItemToGroup(OPCAgvInStockPassOPCGroup);
                OPCAgvInStockPassOPCGroup.DataChange += OPCAgvInStockPassOPCGroup_DataChange;
                #endregion

                #region 入库机械手抓取信息组
                OPCInRobootPickOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCInRobootPickOPCGroupName);
                OPCInRobootPickOPCGroup.UpdateRate = OPCConfig.OPCInRobootPickOPCGroupRate;
                OPCInRobootPickOPCGroup.DeadBand = OPCConfig.OPCInRobootPickOPCGroupDeadBand;
                OPCInRobootPickOPCGroup.IsSubscribed = true;
                OPCInRobootPickOPCGroup.IsActive = true;

                // 添加item
                OPCInRobootPickData.AddItemToGroup(OPCInRobootPickOPCGroup);
                OPCInRobootPickOPCGroup.DataChange += OPCInRobootPickOPCGroup_DataChange;

                #endregion

                #region 初始化巷道机入库任务组
                // 初始化 巷道机1 入库任务组
                OPCSetStockTaskRoadMachine1OPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCSetStockTaskRM1OPCGroupName);
                OPCSetStockTaskRoadMachine1OPCGroup.UpdateRate = OPCConfig.OPCSetStockTaskRM1OPCGroupRate;
                OPCSetStockTaskRoadMachine1OPCGroup.DeadBand = OPCConfig.OPCSetStockTaskRM1OPCGroupDeadBand;
                OPCSetStockTaskRoadMachine1OPCGroup.IsSubscribed = true;
                OPCSetStockTaskRoadMachine1OPCGroup.IsActive = true;
                // 添加item
                OPCSetStockTaskRoadMachine1Data.AddItemToGroup(OPCSetStockTaskRoadMachine1OPCGroup);
                OPCSetStockTaskRoadMachine1OPCGroup.DataChange += OPCSetStockTaskRoadMachine1OPCGroup_DataChange;

                // 初始化 巷道机2 入库任务组
                OPCSetStockTaskRoadMachine2OPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCSetStockTaskRM2OPCGroupName);
                OPCSetStockTaskRoadMachine2OPCGroup.UpdateRate = OPCConfig.OPCSetStockTaskRM2OPCGroupRate;
                OPCSetStockTaskRoadMachine2OPCGroup.DeadBand = OPCConfig.OPCSetStockTaskRM2OPCGroupDeadBand;
                OPCSetStockTaskRoadMachine2OPCGroup.IsSubscribed = true;
                OPCSetStockTaskRoadMachine2OPCGroup.IsActive = true;
                // 添加item
                OPCSetStockTaskRoadMachine2Data.AddItemToGroup(OPCSetStockTaskRoadMachine2OPCGroup);
                OPCSetStockTaskRoadMachine2OPCGroup.DataChange += OPCSetStockTaskRoadMachine2OPCGroup_DataChange;

                // 初始化 巷道机1 入库任务反馈组
                OPCRoadMachine1TaskFeedOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCTaskFeedRM1OPCGroupName);
                OPCRoadMachine1TaskFeedOPCGroup.UpdateRate = OPCConfig.OPCTaskFeedRM1OPCGroupRate;
                OPCRoadMachine1TaskFeedOPCGroup.DeadBand = OPCConfig.OPCTaskFeedRM1OPCGroupDeadBand;
                OPCRoadMachine1TaskFeedOPCGroup.IsSubscribed = true;
                OPCRoadMachine1TaskFeedOPCGroup.IsActive = true;
                // 添加item
                OPCRoadMachine1TaskFeedData.AddItemToGroup(OPCRoadMachine1TaskFeedOPCGroup);
                OPCRoadMachine1TaskFeedOPCGroup.DataChange += OPCRoadMachine1TaskFeedOPCGroup_DataChange;

                // 初始化 巷道机2 入库任务反馈组
                OPCRoadMachine2TaskFeedOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCTaskFeedRM2OPCGroupName);
                OPCRoadMachine2TaskFeedOPCGroup.UpdateRate = OPCConfig.OPCTaskFeedRM2OPCGroupRate;
                OPCRoadMachine2TaskFeedOPCGroup.DeadBand = OPCConfig.OPCTaskFeedRM2OPCGroupDeadBand;
                OPCRoadMachine2TaskFeedOPCGroup.IsSubscribed = true;
                OPCRoadMachine2TaskFeedOPCGroup.IsActive = true;
                // 添加item
                OPCRoadMachine2TaskFeedData.AddItemToGroup(OPCRoadMachine2TaskFeedOPCGroup);
                OPCRoadMachine2TaskFeedOPCGroup.DataChange += OPCRoadMachine2TaskFeedOPCGroup_DataChange;


                // 初始化 出库抓手 任务
                OPCOutRobootPickOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCOutRobootPickOPCGroupName);
                OPCOutRobootPickOPCGroup.UpdateRate = OPCConfig.OPCOutRobootPickOPCGroupRate;
                OPCOutRobootPickOPCGroup.DeadBand = OPCConfig.OPCOutRobootPickOPCGroupDeadBand;
                OPCOutRobootPickOPCGroup.IsSubscribed = true;
                OPCOutRobootPickOPCGroup.IsActive = true;
                // 添加item
                OPCOutRobootPickData.AddItemToGroup(OPCOutRobootPickOPCGroup);
                OPCOutRobootPickOPCGroup.DataChange += OPCOutRobootPickOPCGroup_DataChange;

                // 设置OPC
                OPCDataResetOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCDataResetOPCGroupName);
                OPCDataResetOPCGroup.UpdateRate = OPCConfig.OPCDataResetOPCGroupRate;
                OPCDataResetOPCGroup.DeadBand = OPCConfig.OPCInRobootPickOPCGroupDeadBand;
                OPCDataResetOPCGroup.IsSubscribed = true;
                OPCDataResetOPCGroup.IsActive = true;
                // 添加item
                OPCDataResetData.AddItemToGroup(OPCDataResetOPCGroup);
                OPCDataResetOPCGroup.DataChange += OPCDataResetOPCGroup_DataChange;

                #endregion


                return true;

            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
            return false;
        }


        #region OPC 数据变化事件

        // 第一次会获取到opcserver的数据，即使没有触发，相当于初始化
        // 扫描入库的信息获取
        private void OPCCheckInStockBarcodeOPCGroup_DataChange(
            int TransactionID,
            int NumItems,
            ref Array ClientHandles,
            ref Array ItemValues,
            ref Array Qualities,
            ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【入库条码扫描】【{0}】{1}",
                        OPCCheckInStockBarcodeData.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i));
                }
                OPCCheckInStockBarcodeData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// AGV放行数据改变事件
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCAgvInStockPassOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【AGV放行】【{0}】{1}",
                        OPCAgvInStockPassData.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i));
                }
                OPCAgvInStockPassData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 入库机械手抓手信息
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCInRobootPickOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【机械手】【{0}】{1}",
                       OPCInRobootPickData.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i));
                }
                OPCInRobootPickData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置 巷道机1 入库任务的数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCSetStockTaskRoadMachine1OPCGroup_DataChange(int TransactionID,
            int NumItems,
            ref Array ClientHandles,
            ref Array ItemValues,
            ref Array Qualities,
            ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【任务接受】【巷道机1】【{0}】{1}",
                        OPCSetStockTaskRoadMachine1Data.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i));
                }

                OPCSetStockTaskRoadMachine1Data.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置 巷道机2 入库任务的数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCSetStockTaskRoadMachine2OPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【任务接受】【巷道机 2】【{0}】{1}",
                        OPCSetStockTaskRoadMachine2Data.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i).ToString());
                }
                OPCSetStockTaskRoadMachine2Data.SetValue(NumItems, ClientHandles, ItemValues);

            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                try
                {
                    for (var i = 1; i <= NumItems; i++)
                    {
                        LogUtil.Logger.InfoFormat("【程序错误】【数据改变】【任务接受】【巷道机 2】{0}",
                            ItemValues.GetValue(i).ToString());
                    }
                }
                catch (Exception eex)
                {
                    LogUtil.Logger.Error(eex.Message, eex);
                }
                //MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 设置 巷道机1 入库任务的反馈 数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCRoadMachine1TaskFeedOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                //for (var i = 1; i <= NumItems; i++)
                //{
                //    LogUtil.Logger.InfoFormat("【数据改变】【任务反馈】【巷道机 1】【{0}】{1}",
                //        OPCRoadMachine1TaskFeedData.GetSimpleOpcKey(i),
                //        ItemValues.GetValue(i));
                //}
                OPCRoadMachine1TaskFeedData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置 巷道机2 入库任务的反馈 数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCRoadMachine2TaskFeedOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                //// 从1开始
                //for (var i = 1; i <= NumItems; i++)
                //{
                //    LogUtil.Logger.InfoFormat("【数据改变】【任务反馈】【巷道机 2】【{0}】{1}",
                //        OPCRoadMachine2TaskFeedData.GetSimpleOpcKey(i),
                //        ItemValues.GetValue(i));
                //}
                OPCRoadMachine2TaskFeedData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                // MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 出库机械手 数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCOutRobootPickOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【出库机械手】【{0}】{1}",
                        OPCOutRobootPickData.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i));
                }
                OPCOutRobootPickData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //   MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// 出库机械手 数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCDataResetOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】【OPC 设置】【{0}】{1}",
                        OPCDataResetData.GetSimpleOpcKey(i, ClientHandles),
                        ItemValues.GetValue(i));
                }
                OPCDataResetData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                //   MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
