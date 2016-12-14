﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterLib.Model.OPC;
using AGVCenterLib.Service;
using Brilliantech.Framwork.Utils.LogUtil;
using OPCAutomation;

namespace AGVCenterWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region 服务变量
        OPCServer AnOPCServer;
        OPCServer ConnectedOPCServer;
        #endregion

        #region
        /// AGV扫描任务队列
        // 显示
        List<StockTaskItem> TaskCenterForDisplayQueue;
        // 任务
        Dictionary<string, StockTaskItem> AgvScanTaskQueue;
        private static object WriteTaskCenterQueueLocker = new object();
        private static object AgvScanTaskQueueLocker = new object();

        //小车放行队列
        Queue AgvInStockPassQueue;
        //机械手抓取队列
        Queue InRobootPickQueue;

        // 巷道机1任务队列
        /// Dictionary<string, StockTaskItem> RoadMachine1TaskQueue;
        Queue RoadMachine1TaskQueue;
        ///Dictionary<string, StockTaskItem> RoadMachine2TaskQueue;
        Queue RoadMachine2TaskQueue;

        private static object WriteRoadMachineTaskQueueLocker = new object();
        private static object RoadMachineTaskQueueLocker = new object();

        /// <summary>
        /// 入库任务队列，以条码为键
        /// </summary>
       // Dictionary<string, OPCSetStockTask> InStockTaskQueue;
        private static object WriteStockTaskLocker = new object();
        private static object StockTaskQueueLocker = new object();

        #endregion
        #region 入库信息数据 OPC

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
        #endregion

        #region 监控定时器
        /// <summary>
        /// 写入OPC小车放行定时器，将队列AgvInStockPassQueue的任务写入OPC
        /// </summary>
        System.Timers.Timer SetOPCAgvPassTimer;

        /// <summary>
        /// 写入OPC入库机械手信息，将对列InRobootPickQueue的任务写入OPC
        /// </summary>
        System.Timers.Timer SetOPCInRobootPickTimer;

        /// <summary>
        /// 逐托分发出库任务定时器
        /// </summary>
        System.Timers.Timer DispatchTrayOutStockTaskTimer;
        private static object DispatchTrayOutStockTaskLocker = new object();

        /// <summary>
        /// 写入OPC库存任务定时器
        /// </summary>
        System.Timers.Timer SetOPCStockTaskTimer;
        #endregion

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载初始化数据
            // 初始化并从数据库加载任务
            this.InitQueueAndLoadTaskFromDb();
            #endregion

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
            OPCOutRobootPickData.RwFlagChangedEvent +=new OPCOutRobootPick.RwFlagChangedEventHandler( OPCOutRobootPickData_RwFlagChangedEvent);
            #endregion

            this.InitAndStartTimers();
            this.InitAndStartUpdateStackTaskStateComponent();
        }


        /// <summary>
        /// 初始化并启动定时器
        /// </summary>
        private void InitAndStartTimers()
        {
            #region 初始化定时器
            // AGV入库放行Timer
            SetOPCAgvPassTimer = new System.Timers.Timer();
            SetOPCAgvPassTimer.Interval = OPCConfig.SetOPCAgvInStockPassTimerInterval;
            SetOPCAgvPassTimer.Enabled = true;
            SetOPCAgvPassTimer.Elapsed += SetOPCAgvPassTimer_Elapsed;

            // 入库机械手Timer
            SetOPCInRobootPickTimer = new System.Timers.Timer();
            SetOPCInRobootPickTimer.Interval = OPCConfig.SetOPCInRobootPickTimerInterval;
            SetOPCInRobootPickTimer.Enabled = true;
            SetOPCInRobootPickTimer.Elapsed += SetOPCInRobootPickTimer_Elapsed;


            // 从数据库加载出库任务定时器
            LoadOutStockTaskFromDbTimer = new System.Timers.Timer();
            LoadOutStockTaskFromDbTimer.Interval = OPCConfig.LoadOutStockTaskFromDbTimerInterval;
            LoadOutStockTaskFromDbTimer.Enabled = true;
            LoadOutStockTaskFromDbTimer.Elapsed += LoadOutStockTaskFromDbTimer_Elapsed;

            // 逐托分发出库任务定时器，查看是可以分发
            DispatchTrayOutStockTaskTimer = new System.Timers.Timer();
            DispatchTrayOutStockTaskTimer.Interval = OPCConfig.DispatchTrayOutStockTaskTimerInterval;
            DispatchTrayOutStockTaskTimer.Enabled = true;
            DispatchTrayOutStockTaskTimer.Elapsed += DispatchTrayOutStockTaskTimer_Elapsed;

            // 库存任务定时器，查看巷道机是否可以工作
            SetOPCStockTaskTimer = new System.Timers.Timer();
            SetOPCStockTaskTimer.Interval = OPCConfig.SetOPCStockTaskTimerInterval;
            SetOPCStockTaskTimer.Enabled = true;
            SetOPCStockTaskTimer.Elapsed += SetOPCStockTaskTimer_Elapsed;

            /// 启动定时器
            LogUtil.Logger.Info("【启动SetOPCAgvPassTimer定时器】");
            SetOPCAgvPassTimer.Start();
            LogUtil.Logger.Info("【启动SetOPCInRobootPickTimer定时器】");
            SetOPCInRobootPickTimer.Start();
            LogUtil.Logger.Info("【启动LoadOutStockTaskFromDbTimer定时器】");
            LoadOutStockTaskFromDbTimer.Start();
            LogUtil.Logger.Info("【启动DispatchTrayOutStockTaskTimer定时器】");
            DispatchTrayOutStockTaskTimer.Start();
            LogUtil.Logger.Info("【启动SetOPCStockTaskTimer定时器】");
            SetOPCStockTaskTimer.Start();
            #endregion
        }


        /// <summary>
        /// 读取AGV放行队列中的任务，写入OPC值并可读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetOPCAgvPassTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetOPCAgvPassTimer.Stop();
            if (AgvInStockPassQueue.Count > 0 && OPCAgvInStockPassData.CanWrite)
            {
                StockTaskItem taskItem = AgvInStockPassQueue.Peek() as StockTaskItem;
                OPCAgvInStockPassData.AgvPassFlag = taskItem.AgvPassFlag;
                taskItem.State = StockTaskState.AgvInStcoking;
                if (OPCAgvInStockPassData.SyncWrite(OPCAgvInStockPassOPCGroup))
                {
                    // 进入机械手抓取
                    if (taskItem.AgvPassFlag == (byte)AgvPassFlag.Pass)
                    {
                        /// 进入机械手队列
                        InRobootPickQueue.Enqueue(AgvInStockPassQueue.Dequeue());
                        /// 从AGV扫描任务中移除
                        this.RemoveTaskFromAgvScanTaskQueue(taskItem.Barcode);
                    }
                    else if (taskItem.AgvPassFlag == (byte)AgvPassFlag.Alarm)
                    {
                        AgvInStockPassQueue.Dequeue();
                        /// 从AGV扫描任务中移除
                        this.RemoveTaskFromAgvScanTaskQueue(taskItem.Barcode);
                    }
                    else {
                        AgvInStockPassQueue.Dequeue();
                    }
                    RefreshList();
                }
            }
            SetOPCAgvPassTimer.Start();
        }

        private int prevRoadMahineIndex = 0;
        /// <summary>
        /// 读取入库机械手数据队列中的任务，写入OPC值并可读
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetOPCInRobootPickTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetOPCInRobootPickTimer.Stop();
            if (InRobootPickQueue.Count > 0 && OPCInRobootPickData.CanWrite)
            {
                StockTaskItem taskItem = InRobootPickQueue.Peek() as StockTaskItem;
                // taskItem.State = StockTaskState.RobootInStocking;

                // OPCInRobootPickData.BoxType = taskItem.BoxType;
                int roadMachineIndex = 0;

                // 判断将其写入巷道机的任务队列
                // 两个都空闲的话使用平均原则，1,2,1,2,1,2 间隔入库
                if(RoadMachine1TaskQueue.Count==0 && RoadMachine2TaskQueue.Count == 0)
                {
                    if (prevRoadMahineIndex == 1)
                    {
                        roadMachineIndex = 2;
                    }
                    else
                    {
                        roadMachineIndex = 1;
                    }
                } else if (RoadMachine1TaskQueue.Count == 0)
                {
                    roadMachineIndex = 1;
                }
                else if (RoadMachine2TaskQueue.Count == 0)
                {
                    roadMachineIndex = 2;
                }
                else if (this.NotHasRoadMachineBuffingTask(1))
                {
                    roadMachineIndex = 1;
                }
                else if (this.NotHasRoadMachineBuffingTask(2))
                {
                    roadMachineIndex = 2;
                }
                else
                {
                    /// 无可用的巷道机缓冲
                    /// 默认给1
                    roadMachineIndex = 1;
                    //return;
                }

                prevRoadMahineIndex = roadMachineIndex;

                taskItem.State = StockTaskState.RoadMachineStockBuffing;
                taskItem.RoadMachineIndex = roadMachineIndex;

                if (roadMachineIndex == 1)
                {
                    OPCInRobootPickData.BoxType = taskItem.BoxType;

                    // RoadMachine1TaskQueue.Add(taskItem.Barcode, taskItem);
                    RoadMachine1TaskQueue.Enqueue(taskItem);
                }
                else if (roadMachineIndex == 2)
                {
                    if (taskItem.BoxType == (byte)1)
                    {
                        OPCInRobootPickData.BoxType = (byte)3;
                    }
                    else
                    {
                        OPCInRobootPickData.BoxType = (byte)4;
                    }
                    //RoadMachine2TaskQueue.Add(taskItem.Barcode, taskItem);
                    RoadMachine2TaskQueue.Enqueue(taskItem);
                }

                if (OPCInRobootPickData.SyncWrite(OPCInRobootPickOPCGroup))
                {
                    InRobootPickQueue.Dequeue();
                    RefreshList();
                }
            }
            SetOPCInRobootPickTimer.Start();
        }


        /// <summary>
        /// 验证可读和读取入库条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetOPCStockTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetOPCStockTaskTimer.Stop();

            if (OPCSetStockTaskRoadMachine1Data.CanWrite && RoadMachine1TaskQueue.Count>0 && (RoadMachine1TaskQueue.Peek() as StockTaskItem).IsInBuffingState)
            {
                StockTaskItem taskItem = this.DequeueRoadMachineTaskQueueForStcok(1);
                if (taskItem != null)
                {
                    OPCSetStockTaskRoadMachine1Data.StockTaskType = (byte)taskItem.StockTaskType;

                    OPCSetStockTaskRoadMachine1Data.BoxType = taskItem.BoxType;
                    OPCSetStockTaskRoadMachine1Data.PositionFloor =  taskItem.PositionFloor;
                    OPCSetStockTaskRoadMachine1Data.PositionColumn =  taskItem.PositionColumn;
                    OPCSetStockTaskRoadMachine1Data.PositionRow =  taskItem.PositionRow;


                    OPCSetStockTaskRoadMachine1Data.RestPositionFlag = taskItem.RestPositionFlag;

                    OPCSetStockTaskRoadMachine1Data.TrayReverseNo = taskItem.TrayReverseNo;
                    OPCSetStockTaskRoadMachine1Data.TrayNum = taskItem.TrayNum;
                    OPCSetStockTaskRoadMachine1Data.DeliveryItemNum = taskItem.DeliveryItemNum;


                    OPCSetStockTaskRoadMachine1Data.Barcode = taskItem.Barcode;
                    if (OPCSetStockTaskRoadMachine1Data.SyncWrite(OPCSetStockTaskRoadMachine1OPCGroup))
                    {
                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.RoadMachineInStocking;
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.RoadMachineOutStocking;
                        }
                    }
                }
            }

            if (OPCSetStockTaskRoadMachine2Data.CanWrite && RoadMachine2TaskQueue.Count>0 && (RoadMachine2TaskQueue.Peek() as StockTaskItem).IsInBuffingState)
            {
                StockTaskItem taskItem = this.DequeueRoadMachineTaskQueueForStcok(2);
                if (taskItem != null)
                {
                    OPCSetStockTaskRoadMachine2Data.StockTaskType = (byte)taskItem.StockTaskType;

                    OPCSetStockTaskRoadMachine2Data.BoxType = taskItem.BoxType;
                    OPCSetStockTaskRoadMachine2Data.PositionFloor =  taskItem.PositionFloor;
                    OPCSetStockTaskRoadMachine2Data.PositionColumn =  taskItem.PositionColumn;
                    OPCSetStockTaskRoadMachine2Data.PositionRow =  taskItem.PositionRow;


                    OPCSetStockTaskRoadMachine2Data.RestPositionFlag = taskItem.RestPositionFlag;

                    OPCSetStockTaskRoadMachine2Data.TrayReverseNo = taskItem.TrayReverseNo;
                    OPCSetStockTaskRoadMachine2Data.TrayNum = taskItem.TrayNum;
                    OPCSetStockTaskRoadMachine2Data.DeliveryItemNum = taskItem.DeliveryItemNum;


                    OPCSetStockTaskRoadMachine2Data.Barcode = taskItem.Barcode;
                    if (OPCSetStockTaskRoadMachine2Data.SyncWrite(OPCSetStockTaskRoadMachine2OPCGroup))
                    {
                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.RoadMachineInStocking;
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.RoadMachineOutStocking;
                        }
                    }
                }
            }

            SetOPCStockTaskTimer.Start();

            #region 巷道机1是否空闲
            ///// 巷道机1是否空闲
            //if (OPCSetStockTaskRoadMachine1Data.CanWrite)
            //{
            //    StockTaskItem taskItem = this.DequeueTaskCenterQueueForStcok(1);

            //    if (taskItem != null)
            //    {
            //        OPCSetStockTaskRoadMachine1Data.StockTaskType = (byte)taskItem.StockTaskType;

            //        OPCSetStockTaskRoadMachine1Data.BoxType = taskItem.BoxType;
            //        OPCSetStockTaskRoadMachine1Data.PositionFloor = (byte)taskItem.PositionFloor;
            //        OPCSetStockTaskRoadMachine1Data.PositionColumn = (byte)taskItem.PositionColumn;
            //        OPCSetStockTaskRoadMachine1Data.PositionRow = (byte)taskItem.PositionRow;


            //        OPCSetStockTaskRoadMachine1Data.RestPositionFlag = taskItem.RestPositionFlag;

            //        OPCSetStockTaskRoadMachine1Data.StockTaskType = (byte)StockTaskType.OUT;

            //        OPCSetStockTaskRoadMachine1Data.TrayReverseNo = taskItem.TrayReverseNo;
            //        OPCSetStockTaskRoadMachine1Data.TrayNum = taskItem.TrayNum;
            //        OPCSetStockTaskRoadMachine1Data.DeliveryItemNum = taskItem.DeliveryItemNum;


            //        OPCSetStockTaskRoadMachine1Data.Barcode = taskItem.Barcode;
            //        if (OPCSetStockTaskRoadMachine1Data.SyncWrite(OPCSetStockTaskRoadMachine1OPCGroup))
            //        {
            //            if (taskItem.StockTaskType == StockTaskType.IN)
            //            {
            //                taskItem.State = StockTaskState.RoadMachineInStocking;
            //            }
            //            else if (taskItem.StockTaskType == StockTaskType.OUT)
            //            {
            //                taskItem.State = StockTaskState.RoadMachineOutStocking;
            //            }
            //        }
            //    }
            //}
            #endregion


            #region 巷道机2是否空闲
            /// 巷道机2是否空闲
            //if (OPCSetStockTaskRoadMachine2Data.CanWrite)
            //{
            //    StockTaskItem taskItem = this.DequeueTaskCenterQueueForStcok(2);
            //    if (taskItem != null)
            //    {
            //        OPCSetStockTaskRoadMachine2Data.BoxType = taskItem.BoxType;
            //        OPCSetStockTaskRoadMachine2Data.StockTaskType = (byte)taskItem.StockTaskType;
            //        OPCSetStockTaskRoadMachine2Data.PositionFloor = (byte)taskItem.PositionFloor;
            //        OPCSetStockTaskRoadMachine2Data.PositionColumn = (byte)taskItem.PositionColumn;
            //        OPCSetStockTaskRoadMachine2Data.PositionRow = (byte)taskItem.PositionRow;


            //        OPCSetStockTaskRoadMachine2Data.RestPositionFlag = taskItem.RestPositionFlag;

            //        OPCSetStockTaskRoadMachine2Data.TrayReverseNo = taskItem.TrayReverseNo;
            //        OPCSetStockTaskRoadMachine2Data.TrayNum = taskItem.TrayNum;
            //        OPCSetStockTaskRoadMachine2Data.DeliveryItemNum = taskItem.DeliveryItemNum;


            //        OPCSetStockTaskRoadMachine2Data.Barcode = taskItem.Barcode;
            //        if (OPCSetStockTaskRoadMachine2Data.SyncWrite(OPCSetStockTaskRoadMachine2OPCGroup))
            //        {
            //            if (taskItem.StockTaskType == StockTaskType.IN)
            //            {
            //                taskItem.State = StockTaskState.RoadMachineInStocking;
            //            }
            //            else if (taskItem.StockTaskType == StockTaskType.OUT)
            //            {
            //                taskItem.State = StockTaskState.RoadMachineOutStocking;
            //            }
            //        }
            //    }
            //}
            // 刷新列表
            RefreshList();
            #endregion
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
        /// 连接OPC服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectOPCServerBtn_Click(object sender, RoutedEventArgs e)
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
        /// OPC 服务关闭事件处理
        /// </summary>
        /// <param name="Reason"></param>
        //private void ConnectedOPCServer_ServerShutDown(string Reason)
        //{
        //    LogUtil.Logger.Info(string.Format("【OPC Sever 自停止】{0}", Reason));
        //    #region 关闭Timer\线程 等活动
        //    ShutDownComponents();
        //    #endregion
        //}

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
        /// 初始化组
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
                //for(int j = 1; j <= NumItems;j++)
                //{
                //    string s = ClientHandles.GetValue(j).ToString();

                //    string ss = "23";
                //}

                //string sss = "23";
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
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
        /// 读取入库条码信息读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCCheckInStockBarcodeData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            if (b.CanRead)
            {
                // 读取条码，获取放行信息写入队列
                LogUtil.Logger.InfoFormat("【根据-条码-判断放行】{0}", OPCCheckInStockBarcodeData.ScanedBarcode);
                try
                {
                    this.CreateInTaskIntoAgvScanTaskQueue(OPCCheckInStockBarcodeData.ScanedBarcode);
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                }
                finally
                {
                    // 置为可写
                    this.OPCCheckInStockBarcodeData.SyncSetWriteableFlag(OPCCheckInstockBarcodeOPCGroup);
                }
            }
        }

        /// <summary>
        /// agv入库放行读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCAgvInStockPassData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            //if (b.CanWrite)
            //{
            /// 将放行信息队列中的信息写入OPC
            /// 使用定时器去写
            /// throw new NotImplementedException();
            //}
        }

        /// <summary>
        /// 入库机械手抓取信息读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCInRobootPickData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            if (b.CanWrite)
            {
                /// 将入库机械手抓取信息写入OPC
                /// 使用定时器去写
            }
        }


        /// <summary>
        /// 入库任务 巷道机1
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCSetStockTaskRoadMachine1Data_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            LogUtil.Logger.InfoFormat("【OPC  巷道机 1 入库任务读写标记改变】{0}->{1}", b.OPCRwFlagWas, b.OPCRwFlag);
            //if (b.CanWrite)
            //{
            // 这边就不写了，使用定时器写入库任务！
            //}
        }

        /// <summary>
        /// 入库任务 巷道机2
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCSetStockTaskRoadMachine2Data_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            LogUtil.Logger.InfoFormat("【OPC  巷道机 2 入库任务读写标记改变】{0}->{1}", b.OPCRwFlagWas, b.OPCRwFlag);
        }

        /// <summary>
        /// 任务动作反馈 巷道机1
        /// </summary>
        /// <param name="taskFeed"></param>
        /// <param name="toActionFlag"></param>
        private void OPCRoadMachine1TaskFeedData_ActionFlagChangeEvent(OPCRoadMachineTaskFeed taskFeed, byte toActionFlag)
        {
            LogUtil.Logger.InfoFormat("【OPC  巷道机 {0} 库存动作标记改变】【{1}】{2}->{3}",
                taskFeed.RoadMachineIndex,
                taskFeed.CurrentBarcode,
                taskFeed.ActionFlagWas,
                taskFeed.ActionFlag);
            // 更改任务状态
            if((StockTaskActionFlag)taskFeed.ActionFlagWas== StockTaskActionFlag.Excuting){
                LogUtil.Logger.Info("********************************************************************");
                LogUtil.Logger.InfoFormat("{0}->{1}", taskFeed.ActionFlagWas, taskFeed.ActionFlag);
                LogUtil.Logger.Info("********************************************************************");
                UpdateTaskByFeed(taskFeed.RoadMachineIndex, (StockTaskActionFlag)taskFeed.ActionFlag, taskFeed.CurrentBarcode);
            }
        }

        /// <summary>
        /// 出库机械手可写标记改变事件
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCOutRobootPickData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            LogUtil.Logger.InfoFormat("【OPC  入库机械手 读写标记改变】{0}->{1}", b.OPCRwFlagWas, b.OPCRwFlag);
            //if (b.CanWrite)
            //{
               
            //}
        }


        /// <summary>
        /// 根据巷道机反馈返回
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <param name="actionFlag"></param>
        /// <param name="barCode"></param>
        public void UpdateTaskByFeed(int roadMachineIndex, StockTaskActionFlag actionFlag, string barcode)
        {
            // string barcode = string.Empty;
            if (roadMachineIndex <= 0)
            {
                return;
            }
            //if (feed.RoadMachineIndex == 1) {
            //    barcode = OPCSetStockTaskRoadMachine1Data.Barcode;
            //}
            //else if (feed.RoadMachineIndex == 2) {
            //    barcode = OPCSetStockTaskRoadMachine2Data.Barcode;
            //}
           
                var dicQ = roadMachineIndex == 1 ? RoadMachine1TaskQueue : RoadMachine2TaskQueue;

                if (dicQ.Count>0)
                {
                StockTaskItem taskItem = dicQ.Peek() as StockTaskItem; //dicQ.FirstOrDefault().Value;

            

                switch ((StockTaskActionFlag)((int)actionFlag))
                {
                    case StockTaskActionFlag.InSuccess:
                        taskItem.State = StockTaskState.InStocked;
                        new StorageService(OPCConfig.DbString).InStockByCheckCode(taskItem.PositionNr, taskItem.Barcode);
                        //dicQ.Remove(barcode);
                        dicQ.Dequeue();
                        break;
                    case StockTaskActionFlag.InFailPositionWasStored:
                    case StockTaskActionFlag.InFailPositionNotExists:
                        throw new NotImplementedException("入库反馈错误未实现!");
                        break;
                    case StockTaskActionFlag.OutSuccess:
                        taskItem.State = StockTaskState.OutStocked;
                        new StorageService(OPCConfig.DbString).OutStockByCheckCode(taskItem.Barcode);
                        dicQ.Dequeue();
                        break;
                    case StockTaskActionFlag.OutFailStoreNotFound:
                    case StockTaskActionFlag.OutFailBarNotMatch:
                    case StockTaskActionFlag.OutFailPositionNotExists:
                        throw new NotImplementedException("出库反馈错误未实现!");
                        break;
                    case StockTaskActionFlag.Success:

                        LogUtil.Logger.Info("********【成功状态反馈】************************************************************");
                        LogUtil.Logger.InfoFormat("条码: {0}---DbId:{1} -- 库位:{2}", taskItem.Barcode, taskItem.DbId, taskItem.PositionNr);
                        LogUtil.Logger.Info("********************************************************************");


                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.InStocked;
                            new StorageService(OPCConfig.DbString)
                                .InStockByCheckCode(taskItem.PositionNr, taskItem.Barcode);
                            dicQ.Dequeue();
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.OutStocked;
                            new StorageService(OPCConfig.DbString).OutStockByCheckCode(taskItem.Barcode);
                            dicQ.Dequeue();
                        }
                        break;
                    case StockTaskActionFlag.Fail:
                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.ErrorInStock;
                            //  new StorageService(OPCConfig.DbString).InStockByCheckCode(taskItem.PositionNr, taskItem.Barcode);
                            dicQ.Dequeue();
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.ErrorOutStock;
                            //  new StorageService(OPCConfig.DbString).OutStockByCheckCode(taskItem.Barcode);
                            dicQ.Dequeue();
                        }
                        break;
                    default: break;
                }

                    //StockTaskService sts = new StockTaskService(OPCConfig.DbString);

                    //sts.UpdateTaskState(taskItem);

                    //RefreshList();
               
                #region OLD
                //if (!string.IsNullOrEmpty(barcode))
                //{
                //if (dicQ.Keys.Contains(barcode))
                //{
                //    StockTaskItem taskItem = dicQ[barcode];
                //    switch ((StockTaskActionFlag)((int)actionFlag))
                //    {
                //        case StockTaskActionFlag.InSuccess:
                //            taskItem.State = StockTaskState.InStocked;
                //            new StorageService(OPCConfig.DbString).InStockByCheckCode(taskItem.PositionNr, taskItem.Barcode);
                //            dicQ.Remove(barcode);
                //            break;
                //        case StockTaskActionFlag.InFailPositionWasStored:
                //        case StockTaskActionFlag.InFailPositionNotExists:
                //            throw new NotImplementedException("入库反馈错误未实现!");
                //            break;
                //        case StockTaskActionFlag.OutSuccess:
                //            taskItem.State = StockTaskState.OutStocked;
                //            new StorageService(OPCConfig.DbString).OutStockByCheckCode(taskItem.Barcode);
                //            dicQ.Remove(barcode);
                //            break;
                //        case StockTaskActionFlag.OutFailStoreNotFound:
                //        case StockTaskActionFlag.OutFailBarNotMatch:
                //        case StockTaskActionFlag.OutFailPositionNotExists:
                //            throw new NotImplementedException("出库反馈错误未实现!");
                //            break;
                //        case StockTaskActionFlag.Success:
                //            if (taskItem.StockTaskType == StockTaskType.IN)
                //            {
                //                taskItem.State = StockTaskState.InStocked;
                //                new StorageService(OPCConfig.DbString)
                //                    .InStockByCheckCode(taskItem.PositionNr, taskItem.Barcode);
                //                dicQ.Remove(barcode);
                //            }
                //            else if(taskItem.StockTaskType == StockTaskType.OUT)
                //            {
                //                taskItem.State = StockTaskState.OutStocked;
                //                new StorageService(OPCConfig.DbString).OutStockByCheckCode(taskItem.Barcode);
                //                dicQ.Remove(barcode);
                //            }
                //            break;
                //        default: break;
                //    }

                //StockTaskService sts = new StockTaskService(OPCConfig.DbString);

                //sts.UpdateTaskState(taskItem);

                //RefreshList();
                //}
                #endregion
            }
        }

        private string prevScanedBarcode=string.Empty;
        /// <summary>
        /// 将入库任务写入AGV扫描任务队列，并派发到AGV放行队列
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private bool CreateInTaskIntoAgvScanTaskQueue(string barcode)
        {
            lock (WriteTaskCenterQueueLocker)
            {
                StockTaskItem taskItem = new StockTaskItem()
                {
                    Barcode = barcode,
                    StockTaskType = StockTaskType.IN
                };
                taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);

                if (!string.IsNullOrEmpty(barcode))
                {
                    

                    #region 入库
                    UniqueItemService uniqItemService = new UniqueItemService(OPCConfig.DbString);
                    UniqueItem item = uniqItemService.FindByCheckCode(barcode);
                    if (item != null)
                    {
                        //// 是否是重复扫描
                        if (prevScanedBarcode == barcode)
                        {
                            prevScanedBarcode = barcode;
                            // 重复扫描的不再生成任务
                            taskItem.State = StockTaskState.ErrorBarcodeReScan;
                            TaskCenterForDisplayQueue.Add(taskItem);
                            return true;
                        }

                        // 是否可以入库
                        if (uniqItemService.CanUniqInStock(barcode))
                        {
                            // 是否是重复扫描
                            if (AgvScanTaskQueue.Keys.Contains(barcode))
                            {
                                prevScanedBarcode = barcode;
                                // 重复扫描的不再生成任务
                                taskItem.State = StockTaskState.ErrorBarcodeReScan;
                                TaskCenterForDisplayQueue.Add(taskItem);
                                return true;
                            }


                            // 查询可用库位！
                            PositionService ps = new PositionService(OPCConfig.DbString);
                            bool hasAvaliablePosition = ps.HasAvaliablePosition(this.GetDispatchedPositions());
                            if (hasAvaliablePosition)
                            {
                                taskItem.AgvPassFlag = (byte)AgvPassFlag.Pass;
                                // 先放小车，不计算库位!
                                taskItem.BoxType = (byte)item.BoxTypeId;
                            }
                            else
                            {
                                taskItem.AgvPassFlag = (byte)AgvPassFlag.Alarm;
                                taskItem.State = StockTaskState.ErrorNoPositoin;
                            }
                        }
                        else
                        {
                            // 不可入库
                            taskItem.AgvPassFlag = (byte)AgvPassFlag.Alarm;
                            taskItem.State = StockTaskState.ErrorUniqCannotInStock;
                        }
                    }
                    else
                    {
                        //// 条码不存在
                        //if (AgvScanTaskQueue.Keys.Contains(barcode))
                        //{
                        taskItem.AgvPassFlag = (byte)AgvPassFlag.Alarm;
                        //}
                        //else
                        //{
                        //    taskItem.AgvPassFlag = (byte)AgvPassFlag.ReScan;
                        //}
                        taskItem.State = StockTaskState.ErrorUniqNotExsits;
                    }


                    // 先插入数据库Task再加入队列，最后置可读
                    StockTaskService ts = new StockTaskService(OPCConfig.DbString);
                    if (!ts.CreateInStockTask(taskItem))
                    {
                        taskItem.AgvPassFlag = (byte)AgvPassFlag.ReScan;
                        taskItem.State = StockTaskState.ErrorCreateDbTask;
                    }
                }
                else
                {
                    taskItem.AgvPassFlag = (byte)AgvPassFlag.ReScan;
                }
                #endregion

                prevScanedBarcode = barcode;
                /// 加入到AGV扫描队列
                EnqueueAgvScanTaskQueue(taskItem);
                return false;
            }
        }


        #region AGV扫描队列任务
        /// <summary>
        /// 进入AGV扫描任务队列
        /// </summary>
        /// <param name="taskItem"></param>
        private void EnqueueAgvScanTaskQueue(StockTaskItem taskItem)
        {
            lock (AgvScanTaskQueueLocker)
            {
                // 加入AGV扫描队列
                if (AgvScanTaskQueue.Keys.Contains(taskItem.Barcode))
                {
                    AgvScanTaskQueue[taskItem.Barcode] = taskItem;
                }
                else
                {
                    AgvScanTaskQueue.Add(taskItem.Barcode, taskItem);
                }
                TaskCenterForDisplayQueue.Add(taskItem);
                if (taskItem.StockTaskType == StockTaskType.IN)
                {
                    // 立刻加入到放行队列
                    StockTaskItem passTaskItem = AgvScanTaskQueue
                        .Where(s => s.Value.IsInProcessing == false)
                        .Select(s => s.Value).FirstOrDefault();
                    if (passTaskItem != null)
                    {
                        passTaskItem.IsInProcessing = true;

                        passTaskItem.State = StockTaskState.AgvInStcoking;
                        // 进入agv放行
                        AgvInStockPassQueue.Enqueue(passTaskItem);
                    }
                }
                // 刷新界面列表
                RefreshList();
            }
        }
        #endregion

        #region 获得最优先的库存操作任务
        /// <summary>
        /// 获得最优先的库存操作任务 #TO-BETTER#
        /// </summary>
        /// <param name="roadMachineIndex">巷道机序号</param>
        /// <returns></returns>
        //private StockTaskItem DequeueTaskCenterQueueForStcok(int roadMachineIndex) {
        //    lock (TaskCenterQueueLocker)
        //    {
        //        StockTaskItem item = null;
        //        for (var i = 0; i < this.AgvScanTaskQueue.Values.Count; i++)
        //        {
        //            StockTaskItem _item = this.AgvScanTaskQueue.Values.ElementAt(i);
        //            if (_item.StockTaskType == StockTaskType.OUT)
        //            {
        //                // 先找出库的任务
        //                if (_item.RoadMachineIndex == roadMachineIndex
        //                    && _item.State == StockTaskState.RoadMachineWaitOutStock)
        //                {
        //                    item = _item;
        //                    break;
        //                }
        //            }
        //            else if (_item.StockTaskType == StockTaskType.IN)
        //            {
        //                if (_item.State == StockTaskState.AgvInStcoking || _item.State == StockTaskState.RobootInStocking)
        //                {
        //                    // 计算库位
        //                    PositionService ps = new PositionService(OPCConfig.DbString);
        //                    Position position = ps.FindInStockPosition(roadMachineIndex);

        //                    _item.PositionNr = position.Nr;
        //                    _item.PositionFloor = position.Floor;
        //                    _item.PositionColumn = position.Column;
        //                    _item.PositionRow = position.Row;
        //                    _item.RoadMachineIndex = roadMachineIndex;
        //                    _item.State = StockTaskState.RoadMachineInStocking;

        //                    item = _item;
        //                    break;
        //                }
        //            }
        //        }

        //        return item;
        //    }
        //}


        /// <summary>
        /// 分发任务给巷道机1 或 2
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <returns></returns>
        private StockTaskItem DequeueRoadMachineTaskQueueForStcok(int roadMachineIndex)
        {
            //Dictionary<string, StockTaskItem> queue = new Dictionary<string, StockTaskItem>();
            Queue queue = new Queue();
            if (roadMachineIndex == 1)
            {
                queue = RoadMachine1TaskQueue;
            }
            else if (roadMachineIndex == 2)
            {
                queue = RoadMachine2TaskQueue;
            }
            if (queue.Count == 0)
            {
                return null;
            }

            if (queue.ToArray().Count(s => (s as StockTaskItem).IsInBuffingState) == 0)
            {
                return null;
            }

            //if (queue.Count(s => s.Value.State == StockTaskState.RoadMachineStockBuffing) == 0)
            //{
            //    return null;
            //}
            // StockTaskItem taskItem = queue.FirstOrDefault(s=>s.Value.State==StockTaskState.RoadMachineStockBuffing).Value;
            StockTaskItem taskItem = queue.ToArray().FirstOrDefault(s => (s as StockTaskItem).IsInBuffingState) as StockTaskItem;
            if (taskItem.StockTaskType == StockTaskType.IN)
            {
                /// 计算库位
                PositionService ps = new PositionService(OPCConfig.DbString);
                Position position = ps.FindInStockPosition(roadMachineIndex,
                    this.GetDispatchedPositions(roadMachineIndex));

                taskItem.PositionNr = position.Nr;
                taskItem.PositionFloor = position.Floor;
                taskItem.PositionColumn = position.Column;
                taskItem.PositionRow = position.Row;
            }
            return taskItem;
        }

        /// <summary>
        /// 将任务从队列中移除
        /// </summary>
        /// <param name="barcode"></param>
        private void RemoveTaskFromAgvScanTaskQueue(string barcode)
        {
            lock (AgvScanTaskQueueLocker)
            {
                if (this.AgvScanTaskQueue.ContainsKey(barcode))
                {
                    this.AgvScanTaskQueue.Remove(barcode);
                }
            }
        }
        #endregion


        #region 任务状态改变队列处理
        private Queue comDataQ = new Queue();
        private Queue receiveMessageQueue;
        private Thread receiveMessageThread;
        private ManualResetEvent receivedEvent = new ManualResetEvent(false);

        /// <summary>
        /// 初始化并启动 任务状态改变队列处理组件
        /// </summary>
        private void InitAndStartUpdateStackTaskStateComponent()
        {
            receiveMessageThread = new Thread(this.ReceiveMessageThread);
            receiveMessageQueue = Queue.Synchronized(comDataQ);
            receiveMessageThread.IsBackground = true;

            receiveMessageThread.Start();
        }
        /// <summary>
        /// 停止 任务状态改变队列处理组件
        /// </summary>
        private void ShutDownUpdateStackTaskStateComponent()
        {
            receiveMessageThread.Abort();
        }

        private void ReceiveMessageThread()
        {
            while (true)
            {
                while (receiveMessageQueue.Count > 0)
                {
                     StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                      sts.UpdateTaskState(receiveMessageQueue.Dequeue() as StockTask);
                }

                receivedEvent.WaitOne();
                receivedEvent.Reset();
            }
        }

        /// <summary>
        /// 任务状态改变,#TO-BETTER#
        /// </summary>
        /// <param name="taskItem"></param>
        /// <param name="toState"></param>
        private void TaskItem_TaskStateChangeEvent(StockTaskItem taskItem, StockTaskState toState)
        {
            // 更新数据库中的task的状态
            if (taskItem.DbId > 0)
            {
                StockTask updatedStockTask = new StockTask();

                updatedStockTask.Id = taskItem.DbId;
                updatedStockTask.State = (int)taskItem.State;

                updatedStockTask.RoadMachineIndex = taskItem.RoadMachineIndex;
                updatedStockTask.PositionNr = taskItem.PositionNr;
                updatedStockTask.PositionFloor = taskItem.PositionFloor;
                updatedStockTask.PositionColumn = taskItem.PositionColumn;
                updatedStockTask.PositionRow = taskItem.PositionRow;

                receiveMessageQueue.Enqueue(updatedStockTask);
                receivedEvent.Set();
                //StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                //sts.UpdateTaskState(updatedStockTask);
            }
        }
        #endregion



        /// <summary>
        /// 将数据库中的任务加载到Task中,#TO-DO#
        /// </summary>
        private void InitQueueAndLoadTaskFromDb()
        {
            AgvInStockPassQueue = new Queue();
            InRobootPickQueue = new Queue();
            // RoadMachine1TaskQueue = new Dictionary<string, StockTaskItem>();
            RoadMachine1TaskQueue = new Queue();
            // RoadMachine2TaskQueue = new Dictionary<string, StockTaskItem>();
            AgvScanTaskQueue = new Dictionary<string, StockTaskItem>();
            RoadMachine2TaskQueue = new Queue();

            TaskCenterForDisplayQueue = new List<StockTaskItem>();
        }


        #region 组件操作
        /// <summary>
        /// 启动组件
        /// </summary>
        private void StartComponents()
        {

        }

        /// <summary>
        /// 关闭运行的组件
        /// </summary>
        private void ShutDownComponents()
        {
            this.StopTimers();
            this.ShutDownUpdateStackTaskStateComponent();
            this.DisconnectOPCServer();
        }

        /// <summary>
        /// 关闭OPC服务
        /// </summary>
        private void DisconnectOPCServer()
        {
            try
            {
                if (ConnectedOPCServer != null)
                {
                    ConnectedOPCServer.Disconnect();
                }
            }
            catch (Exception ex)
            {
                ConnectedOPCServer = null;
                ConnectOPCServerBtn.IsEnabled = true;
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConnectedOPCServer = null;
                ConnectOPCServerBtn.IsEnabled = true;
            }
        }

        /// <summary>
        /// 停止定时器
        /// </summary>
        private void StopTimers()
        {
            if (SetOPCAgvPassTimer != null)
            {
                SetOPCAgvPassTimer.Enabled = false;
                SetOPCAgvPassTimer.Stop();
            }
            if (SetOPCInRobootPickTimer != null)
            {
                SetOPCInRobootPickTimer.Enabled = false;
                SetOPCInRobootPickTimer.Stop();
            }
            if (DispatchTrayOutStockTaskTimer != null)
            {
                DispatchTrayOutStockTaskTimer.Enabled = false;
                DispatchTrayOutStockTaskTimer.Stop();
            }
            if (SetOPCStockTaskTimer != null)
            {
                SetOPCStockTaskTimer.Enabled = false;
                SetOPCStockTaskTimer.Stop();
            }
            if (LoadOutStockTaskFromDbTimer != null)
            {
                LoadOutStockTaskFromDbTimer.Enabled = false;
                LoadOutStockTaskFromDbTimer.Stop();
            }
        }
        #endregion

        /// <summary>
        /// 写入入库条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WirteGetPisitionBarBtn_Click(object sender, RoutedEventArgs e)
        {
            //if (OPCCheckInStockBarcodeData.CanWrite)
            if (true)
            {
                OPCCheckInStockBarcodeData.ScanedBarcode = ScanedBarCodeTB.Text;
                if (OPCCheckInStockBarcodeData.SyncWrite(OPCCheckInstockBarcodeOPCGroup))
                {
                    LogUtil.Logger.Info("【条码读取成功】");
                    // MessageBox.Show("条码读取成功");
                }
            }
            else
            {
                MessageBox.Show("OPC暂不可以写入数据");
            }
        }


        /// <summary>
        /// 读取入库条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadScanedBarCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OPCCheckInStockBarcodeData.CanRead)
            {
                if (OPCCheckInStockBarcodeData.SyncSetWriteableFlag(OPCCheckInstockBarcodeOPCGroup))
                {
                    MessageBox.Show("条码读取成功");
                }
            }
            else
            {
                MessageBox.Show("不存在任务，OPC暂不可以读取数据");
            }

        }
        
        /// <summary>
        /// 读取任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadInStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OPCSetStockTaskRoadMachine1Data.CanRead)
            {

            }
            else
            {
                MessageBox.Show("不存在任务，OPC暂不可以读取入库数据");
            }
        }

        /// <summary>
        /// 更新显示列表
        /// </summary>
        private void RefreshList()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                // AGV扫描任务列表
                CenterStockTaskDisplayLB.Items.Clear();
                if (TaskCenterForDisplayQueue != null)
                {
                    foreach (var t in TaskCenterForDisplayQueue)
                    {
                        CenterStockTaskDisplayLB.Items.Add(t.ToDisplay());
                    }
                }
                // 入库任务列表
                CenterInStockTaskLB.Items.Clear();
                if (AgvScanTaskQueue != null)
                {
                    foreach (var t in TaskCenterForDisplayQueue)
                    {
                        if (t.StockTaskType == StockTaskType.IN)
                        {
                            CenterInStockTaskLB.Items.Add(t.ToDisplay());
                        }
                    }
                }
                // 出库任务列表
                CenterOutStockTaskLB.Items.Clear();
                if (AgvScanTaskQueue != null)
                {
                    foreach (var t in TaskCenterForDisplayQueue)
                    {
                        if (t.StockTaskType == StockTaskType.OUT)
                        {
                            CenterOutStockTaskLB.Items.Add(t.ToDisplay());
                        }
                    }
                }
            }));
        }

        /// <summary>
        /// 关闭窗口时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShutDownComponents();
        }

  
        /// <summary>
        /// 判断是否有缓冲的入库任务
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <returns></returns>
        private bool NotHasRoadMachineBuffingTask(int roadMachineIndex)
        {
            switch (roadMachineIndex)
            {
                case 1:
                    if (RoadMachine1TaskQueue.Count == 0) return false;
                   return RoadMachine1TaskQueue.ToArray().
                        FirstOrDefault(s => (s as StockTaskItem).State == StockTaskState.RoadMachineStockBuffing) == null;
                case 2:
                    if (RoadMachine2TaskQueue.Count == 0) return false;
                    return RoadMachine2TaskQueue.ToArray().
                        FirstOrDefault(s => (s as StockTaskItem).State == StockTaskState.RoadMachineStockBuffing) == null;
                default:
                    return false;

            }
        }

        /// <summary>
        /// 获取已分配的入库库位
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <returns></returns>
        public List<string> GetDispatchedPositions(int? roadMachineIndex = null)
        {
            List<string> dispatchedPositions = new List<string>();
            if (roadMachineIndex.HasValue)
            {
                if (roadMachineIndex == 1)
                {
                    if (this.RoadMachine1TaskQueue.Count > 0)
                    {
                        dispatchedPositions.AddRange(
                         this.RoadMachine1TaskQueue.ToArray().Select(s => (s as StockTaskItem).PositionNr).ToList()
                         );
                    }
                    //dispatchedPositions.AddRange(
                    //    this.RoadMachine1TaskQueue.Select(s => s.Value.PositionNr).ToList()
                    //    );
                }
                else if (roadMachineIndex == 2)
                {
                    if (this.RoadMachine2TaskQueue.Count > 0)
                    {
                        dispatchedPositions.AddRange(
                         this.RoadMachine2TaskQueue.ToArray().Select(s => (s as StockTaskItem).PositionNr).ToList()
                         );
                    }
                }
            }
            else
            {
                dispatchedPositions.AddRange(this.GetDispatchedPositions(1));
                dispatchedPositions.AddRange(this.GetDispatchedPositions(2));
            }
            return dispatchedPositions;
        }





        #region 从数据库加载出库任务
        /// <summary>
        /// 出库任务总队列
        /// </summary>
        Dictionary<string, List<StockTaskItem>> OutStockCenterQueue = new Dictionary<string, List<StockTaskItem>>();
        System.Timers.Timer LoadOutStockTaskFromDbTimer;


        private void LoadOutStockTaskFromDbTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            LoadOutStockTaskFromDbTimer.Stop();
           
            LoadOutStockTaskFromDb();

            LoadOutStockTaskFromDbTimer.Start();
        }

        /// <summary>
        /// 从数据库加载出库任务，定时器来做此事
        /// </summary>
        bool isLoadingDbTask = false;
        static object LoadOutStockTaskFromDbLocker = new object();
        private void LoadOutStockTaskFromDb()
        {
            lock (LoadOutStockTaskFromDbLocker)
            {
                isLoadingDbTask = true;
                try
                {
                    List<StockTask> stockTasks = new StockTaskService(OPCConfig.DbString)
                        .GetInitOutStockTasksAndUpdateState(this.OutStockCenterQueue.Keys.ToList());
                    if (stockTasks.Count > 0)
                    {
                        foreach (var st in stockTasks)
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

                                TrayReverseNo = st.TrayReverseNo.Value,
                                TrayNum = st.TrayNum.Value,
                                DeliveryItemNum = st.DeliveryItemNum.Value,
                                State = (StockTaskState)st.State,
                                DbId = st.Id,
                                IsInProcessing = true
                            };
                            taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);

                            taskItem.State = StockTaskState.RoadMachineWaitOutStock;
                            ///
                            if (!OutStockCenterQueue.Keys.Contains(st.TrayBatchId))
                            {
                                OutStockCenterQueue.Add(st.TrayBatchId, new List<StockTaskItem>() { taskItem });
                            }
                            else
                            {
                                OutStockCenterQueue[st.TrayBatchId].Add(taskItem);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                }
                isLoadingDbTask = false;
            }

        }



        /// <summary>
        /// 逐托分发出库任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatchTrayOutStockTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DispatchTrayOutStockTaskTimer.Stop();

            if (OPCOutRobootPickData.CanWrite)
            {
                if (this.DispatchOutStockTaskByTray())
                {
                    this.OPCOutRobootPickData.SyncWrite(this.OPCOutRobootPickOPCGroup);
                }
            }

            DispatchTrayOutStockTaskTimer.Start();
        }
        /// <summary>
        /// 逐托分发任务
        /// </summary>
        private bool DispatchOutStockTaskByTray()
        {
            lock (DispatchTrayOutStockTaskLocker)
            {
                if (isLoadingDbTask)
                {
                    return false;
                }
                if (OutStockCenterQueue.Count == 0)
                {
                    return false;
                }
                var f= OutStockCenterQueue.FirstOrDefault();
                this.OPCOutRobootPickData.BoxType = f.Value.FirstOrDefault().BoxType;
                this.OPCOutRobootPickData.TrayNum = f.Value.Count();
                foreach (var taskItem in f.Value)
                {
                    if (taskItem.RoadMachineIndex == 1)
                    {
                        // RoadMachine1TaskQueue.Add(taskItem.Barcode, taskItem);
                        RoadMachine1TaskQueue.Enqueue(taskItem);
                    }
                    else if (taskItem.RoadMachineIndex == 2)
                    {
                        //    RoadMachine2TaskQueue.Add(taskItem.Barcode, taskItem);
                        RoadMachine2TaskQueue.Enqueue(taskItem);
                    }

                    TaskCenterForDisplayQueue.Add(taskItem);
                }
                OutStockCenterQueue.Remove(f.Key);
                return true;
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
        /// 创建出库任务，Demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WaitOutStockTaskLB.SelectedIndex > -1)
            {
                string barcode = WaitOutStockTaskLB.SelectedValue.ToString();

                StockTaskItem taskItem = new StockTaskItem()
                {
                    StockTaskType = StockTaskType.OUT,
                    Barcode = barcode,
                    BoxType = (byte)new Random().Next(1, 3),
                    PositionFloor = new Random().Next(5),
                    PositionColumn = new Random().Next(5),
                    PositionRow = new Random().Next(5),
                    RoadMachineIndex = new Random().Next(1, 3),

                    TrayReverseNo = 1,
                    TrayNum = 2,
                    DeliveryItemNum = 3,

                    IsInProcessing = true
                };
                taskItem.State = StockTaskState.RoadMachineWaitOutStock;
                taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);

                EnqueueAgvScanTaskQueue(taskItem);
            }
        }

        /// <summary>
        /// 加载出库任务，Demo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            WaitOutStockTaskLB.Items.Clear();
            foreach (var b in DemoData.OutStockBarcodes)
            {
                WaitOutStockTaskLB.Items.Add(b);
            }
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
            taskItem.TaskStateChangeEvent+= new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);
            UniqueItemService ui = new UniqueItemService(OPCConfig.DbString);
            if (ui.FindByCheckCode(taskItem.Barcode)==null)
            {
                ui.Create(new UniqueItem() { Nr = ScanedBarCodeTB.Text,
                    CheckCode = ScanedBarCodeTB.Text, BoxTypeId = 1 });
            }

            StockTaskService ts = new StockTaskService(OPCConfig.DbString);
            ts.CreateInStockTask(taskItem);
            if (int.Parse(RoadMachineIndexTB.Text) == 1)
            {
                //  RoadMachine1TaskQueue.Add(taskItem.Barcode, taskItem);
                RoadMachine1TaskQueue.Enqueue(taskItem);
            }
            else if(int.Parse(RoadMachineIndexTB.Text)==2)
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