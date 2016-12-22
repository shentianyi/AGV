using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AGVCenterLib.Model.OPC;
using Brilliantech.Framwork.Utils.LogUtil;
using OPCAutomation;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
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
    }
}
