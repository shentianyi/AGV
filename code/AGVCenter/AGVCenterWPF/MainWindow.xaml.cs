using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.OPC;
using AGVCenterLib.Service;
using AGVCenterWPF.Config;
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
        private object WriteTaskCenterQueueLocker = new object();
        private object AgvScanTaskQueueLocker = new object();

        //小车放行队列
        Queue AgvInStockPassQueue;
        //机械手抓取队列
        Queue InRobootPickQueue;
         

        private object WriteRoadMachineTaskQueueLocker = new object();
        private object RoadMachineTaskQueueLocker = new object();

        /// <summary>
        /// 入库任务队列，以条码为键
        /// </summary>
       // Dictionary<string, OPCSetStockTask> InStockTaskQueue;
        private object WriteStockTaskLocker = new object();
        private object StockTaskQueueLocker = new object();

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
        private object DispatchTrayOutStockTaskLocker = new object();

        /// <summary>
        /// 写入OPC库存任务定时器
        /// </summary>
        System.Timers.Timer SetOPCStockTaskTimer;
        #endregion

        SynchronizationContext uiContext;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            uiContext = SynchronizationContext.Current;

            #region 加载初始化数据
            if (BaseConfig.IsOPCConnector)
            {
                // 初始化并从数据库加载任务
                this.InitQueueAndLoadTaskFromDb();
            }
            else
            {
                this.InitMonitorFields();
            }
            #endregion

            // 初始化OPC变量
            this.InitOPC();

            if (BaseConfig.IsOPCConnector)
            {
                // 开始新线程更新任务状态
                this.InitAndStartUpdateStackTaskStateComponent();
                Thread.Sleep(2000);
            }

            // 自动连接OPC
            if (BaseConfig.AutoConnectOPC)
            {
                this.ConnectOPC();
            }

            /// 初始化定时器，用以分发任务
            if (BaseConfig.IsOPCConnector)
            {
                this.InitAndStartOPCConnectTimers();
            }
            else
            {
                this.StartMonitorTimers();
            }

            /// 是否显示基本设置
            if (BaseConfig.IsOPCConnector)
            {
                SettingTabItem.Visibility = Visibility.Visible;
            }
            else
            {
                SettingTabItem.Visibility = Visibility.Collapsed;
            }

            // 启动RM通信
            this.OpenRabbitMQConnect();

            // 加载定时任务
            this.InitAndStartQuartzTaskSchedule();
        }


        /// <summary>
        /// 初始化并启动定时器
        /// </summary>
        private void InitAndStartOPCConnectTimers()
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

            if (AgvInStockPassQueue.Count > 0)
            {
                StockTaskItem taskItem = AgvInStockPassQueue.Peek() as StockTaskItem;
                if (taskItem.ShouldDequeueStockTask)
                {
                    AgvInStockPassQueue.Dequeue();
                }
            }

            if (AgvInStockPassQueue.Count > 0 && OPCAgvInStockPassData.CanWrite)
            {
                StockTaskItem taskItem = AgvInStockPassQueue.Peek() as StockTaskItem;
                if (taskItem.ShouldDequeueStockTask)
                {
                    AgvInStockPassQueue.Dequeue();
                }
                else
                {
                    OPCAgvInStockPassData.AgvPassFlag = taskItem.AgvPassFlag;
                    if (taskItem.AgvPassFlag == (byte)AgvPassFlag.Pass)
                    {
                        taskItem.State = StockTaskState.AgvInStcoking;
                    }
                    else
                    {
                       // taskItem.State = StockTaskState.AgvPassFail;
                    }

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
                        else
                        {
                            AgvInStockPassQueue.Dequeue();
                        }
                        //# RefreshList();
                    }
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

            if (InRobootPickQueue.Count > 0)
            {
                StockTaskItem taskItem = InRobootPickQueue.Peek() as StockTaskItem;
                if (taskItem.ShouldDequeueStockTask)
                {
                    InRobootPickQueue.Dequeue();
                }
            }

            if (InRobootPickQueue.Count > 0 && OPCInRobootPickData.CanWrite)
            {
                StockTaskItem taskItem = InRobootPickQueue.Peek() as StockTaskItem;
                // taskItem.State = StockTaskState.RobootInStocking;
                if (taskItem.ShouldDequeueStockTask)
                {
                    InRobootPickQueue.Dequeue();
                }
                else
                {
                    // OPCInRobootPickData.BoxType = taskItem.BoxType;
                    int roadMachineIndex = 0;



                    #region V2
                    if (RoadMachine1InTaskQueue.Count == 0 && RoadMachine2InTaskQueue.Count == 0)
                    {
                        if (prevRoadMahineIndex == 1)
                        {
                            if (this.CanRoadMachineInStock(1) && this.CanRoadMachineInStock(2))
                            {
                                // 比较库存量
                                StorageService ss = new StorageService(OPCConfig.DbString);
                                int r1 = ss.CountStorage(1, (int)taskItem.BoxType);
                                int r2 = ss.CountStorage(2, (int)taskItem.BoxType);
                                if (r1 > r2)
                                {
                                    roadMachineIndex = 2;
                                }
                                else
                                {
                                    roadMachineIndex = 1;
                                }
                            }
                            else if (this.CanRoadMachineInStock(2))
                            {
                                roadMachineIndex = 2;
                            }
                            else if (this.CanRoadMachineInStock(1))
                            {
                                roadMachineIndex = 1;
                            }
                        }
                        else
                        {
                            if (this.CanRoadMachineInStock(1) && this.CanRoadMachineInStock(2))
                            {
                                // 比较库存量
                                StorageService ss = new StorageService(OPCConfig.DbString);
                                int r1 = ss.CountStorage(1, (int)taskItem.BoxType);
                                int r2 = ss.CountStorage(2, (int)taskItem.BoxType);
                                if (r1 > r2)
                                {
                                    roadMachineIndex = 2;
                                }
                                else
                                {
                                    roadMachineIndex = 1;
                                }
                            }
                            else if (this.CanRoadMachineInStock(1))
                            {
                                roadMachineIndex = 1;
                            }
                            else if (this.CanRoadMachineInStock(2))
                            {
                                roadMachineIndex = 2;
                            }
                        }
                    }
                    else if (CanRoadMachineInStock(1))
                    {
                        roadMachineIndex = 1;
                    }
                    else if (this.CanRoadMachineInStock(2))
                    {
                        roadMachineIndex = 2;
                    }
                    else
                    {

                    }

                    #endregion
                    if (roadMachineIndex == 1)
                    {
                        OPCInRobootPickData.BoxType = taskItem.BoxType;
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
                    }

                    if (roadMachineIndex != 0)
                    {
                        taskItem.RoadMachineIndex = roadMachineIndex;

                        // 停止自动移库模式
                        this.StopMoveMode(roadMachineIndex);

                        if (OPCInRobootPickData.SyncWrite(OPCInRobootPickOPCGroup,true,true))
                        {
                            InRobootPickQueue.Dequeue();
                            taskItem.State = StockTaskState.RoadMachineStockBuffing;
                            this.EnqueueRoadMachineTask(taskItem);
                            prevRoadMahineIndex = roadMachineIndex;
                        }
                    }
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


            if (OPCSetStockTaskRoadMachine1Data.CanWrite)
            {
                StockTaskItem taskItem = this.DequeueRoadmMachineTaskToCenter(1);
                if (taskItem == null)
                {

                }
                else if (taskItem.IsInBuffingState)
                {
                    OPCSetStockTaskRoadMachine1Data.StockTaskType = (byte)(taskItem.StockTaskType);//(byte)( taskItem.StockTaskType== StockTaskType.MOVE ? StockTaskType.AUTO_MOVE : taskItem.StockTaskType);

                    OPCSetStockTaskRoadMachine1Data.BoxType = taskItem.BoxType;
                    OPCSetStockTaskRoadMachine1Data.PositionFloor = taskItem.PositionFloor;
                    OPCSetStockTaskRoadMachine1Data.PositionColumn = taskItem.PositionColumn;
                    OPCSetStockTaskRoadMachine1Data.PositionRow = taskItem.PositionRow;


                    OPCSetStockTaskRoadMachine1Data.RestPositionFlag = taskItem.RestPositionFlag;

                    OPCSetStockTaskRoadMachine1Data.TrayReverseNo = taskItem.TrayReverseNo;
                    OPCSetStockTaskRoadMachine1Data.TrayNum = taskItem.TrayNum;
                    OPCSetStockTaskRoadMachine1Data.DeliveryItemNum = taskItem.PickItemNum;


                    OPCSetStockTaskRoadMachine1Data.Barcode = taskItem.Barcode;


                    OPCSetStockTaskRoadMachine1Data.ToPositionFloor = taskItem.ToPositionFloor;
                    OPCSetStockTaskRoadMachine1Data.ToPositionColumn = taskItem.ToPositionColumn;
                    OPCSetStockTaskRoadMachine1Data.ToPositionRow = taskItem.ToPositionRow;


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
                        else if (taskItem.StockTaskType == StockTaskType.AUTO_MOVE)
                        {
                            taskItem.State = StockTaskState.RoadMachineMoveStocking;
                        }
                    }
                }

            }

            if (OPCSetStockTaskRoadMachine2Data.CanWrite )
            {
                StockTaskItem taskItem = this.DequeueRoadmMachineTaskToCenter(2);
                if (taskItem == null)
                {

                }
                else if (taskItem.IsInBuffingState)
                {
                    OPCSetStockTaskRoadMachine2Data.StockTaskType = (byte)(taskItem.StockTaskType);//(byte)(taskItem.StockTaskType == StockTaskType.MOVE ? StockTaskType.AUTO_MOVE : taskItem.StockTaskType);

                    OPCSetStockTaskRoadMachine2Data.BoxType = taskItem.BoxType;
                    OPCSetStockTaskRoadMachine2Data.PositionFloor = taskItem.PositionFloor;
                    OPCSetStockTaskRoadMachine2Data.PositionColumn = taskItem.PositionColumn;
                    OPCSetStockTaskRoadMachine2Data.PositionRow = taskItem.PositionRow;


                    OPCSetStockTaskRoadMachine2Data.RestPositionFlag = taskItem.RestPositionFlag;

                    OPCSetStockTaskRoadMachine2Data.TrayReverseNo = taskItem.TrayReverseNo;
                    OPCSetStockTaskRoadMachine2Data.TrayNum = taskItem.TrayNum;
                    OPCSetStockTaskRoadMachine2Data.DeliveryItemNum = taskItem.PickItemNum;


                    OPCSetStockTaskRoadMachine2Data.Barcode = taskItem.Barcode;

                    OPCSetStockTaskRoadMachine2Data.ToPositionFloor = taskItem.ToPositionFloor;
                    OPCSetStockTaskRoadMachine2Data.ToPositionColumn = taskItem.ToPositionColumn;
                    OPCSetStockTaskRoadMachine2Data.ToPositionRow = taskItem.ToPositionRow;

                    if (OPCSetStockTaskRoadMachine2Data.SyncWrite(OPCSetStockTaskRoadMachine2OPCGroup))
                    {
                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.RoadMachineInStocking;
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.RoadMachineOutStocking;
                        }else if (taskItem.StockTaskType == StockTaskType.AUTO_MOVE)
                        {
                            taskItem.State = StockTaskState.RoadMachineMoveStocking;
                        }

                    }
                }
            }

            // 刷新列表
            //# RefreshList() ;
            SetOPCStockTaskTimer.Start();

        }
        
        private string firstBarIgnore = string.Empty;
        private object scanLocker = new object();
        /// <summary>
        /// 读取入库条码信息读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCCheckInStockBarcodeData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            lock (scanLocker)
            {
                if (b.CanRead)
                {
                    // 读取条码，获取放行信息写入队列
                    LogUtil.Logger.InfoFormat("【根据-条码-判断放行】{0}", OPCCheckInStockBarcodeData.ScanedBarcode);
                    if (BaseConfig.IsOPCConnector)
                    {
                        try
                        {
                            LogUtil.Logger.InfoFormat("【扫描到条码内容】{0}:", OPCCheckInStockBarcodeData.ScanedBarcode);
                            if (!string.IsNullOrEmpty(OPCCheckInStockBarcodeData.ScanedBarcode))
                            {
                                if (new Regex(BaseConfig.BarcodeReg).IsMatch(OPCCheckInStockBarcodeData.ScanedBarcode))
                                {
                                    if (string.IsNullOrEmpty(firstBarIgnore))
                                    {
                                        firstBarIgnore = OPCCheckInStockBarcodeData.ScanedBarcode;
                                        // BaseConfig.PreScanBar = firstBarIgnore;
                                    }
                                    else
                                    {
                                        this.currentScanedBarcode.Content = OPCCheckInStockBarcodeData.ScanedBarcode;
                                        this.CreateInTaskIntoAgvScanTaskQueue(OPCCheckInStockBarcodeData.ScanedBarcode);
                                    }
                                }
                                else
                                {
                                    LogUtil.Logger.Info("【扫描到条码内容】不符合规范！");
                                }
                            }
                            else
                            {
                                LogUtil.Logger.Info("【扫描到条码内容】为空！");
                            }
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
            if ((StockTaskActionFlag)taskFeed.ActionFlagWas == StockTaskActionFlag.Excuting)
            {
                LogUtil.Logger.Info("********************************************************************");
                LogUtil.Logger.InfoFormat("{0}->{1}", taskFeed.ActionFlagWas, taskFeed.ActionFlag);
                LogUtil.Logger.Info("********************************************************************");
                if (BaseConfig.IsOPCConnector)
                {
                    UpdateTaskByFeed(taskFeed.RoadMachineIndex,
                    (StockTaskActionFlag)taskFeed.ActionFlag,
                    taskFeed.CurrentBarcode);
                }
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

            var dicQ = roadMachineIndex == 1 ? RoadMachine1CenterTaskQueue : RoadMachine2CenterTaskQueue;

            if (dicQ.Count > 0)
            {
                StockTaskItem taskItem = dicQ.Peek() as StockTaskItem; //dicQ.FirstOrDefault().Value;

                switch ((StockTaskActionFlag)((int)actionFlag))
                {
                    case StockTaskActionFlag.InSuccess:
                        taskItem.State = StockTaskState.InStocked;
                        new StorageService(OPCConfig.DbString).InStockByUniqItemNr(taskItem.PositionNr, taskItem.Barcode);
                        //dicQ.Remove(barcode);
                        dicQ.Dequeue();
                        break;
                    case StockTaskActionFlag.InFailPositionWasStored:
                    case StockTaskActionFlag.InFailPositionNotExists:
                        throw new NotImplementedException("入库反馈错误未实现!");
                        break;
                    case StockTaskActionFlag.OutSuccess:
                        taskItem.State = StockTaskState.OutStocked;
                        new StorageService(OPCConfig.DbString).OutStockByUniqItemNr(taskItem.Barcode);
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

                        var msg = new ResultMessage();

                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.InStocked;
                            if (TestConfig.InStockCreateStorage)
                            {
                                msg = new StorageService(OPCConfig.DbString)
                                      .InStockByUniqItemNr(taskItem.PositionNr, taskItem.Barcode);
                            }
                            dicQ.Dequeue();
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.OutStocked;
                            if (TestConfig.OutStockTaskDelStorage)
                            {
                                msg = new StorageService(OPCConfig.DbString).OutStockByUniqItemNr(taskItem.Barcode);
                            }
                            dicQ.Dequeue();
                        }
                        else if (taskItem.StockTaskType == StockTaskType.AUTO_MOVE)
                        {
                            taskItem.State = StockTaskState.RoadMachineMoveStocked;
                            LogUtil.Logger.InfoFormat("【移库】{0}：{1}---->{2}",taskItem.Barcode,taskItem.PositionNr, taskItem.ToPositionNr);

                            msg = new StorageService(OPCConfig.DbString).MoveStockByUniqItemNr(taskItem.Barcode, taskItem.ToPositionNr);
                            dicQ.Dequeue();
                        }

                        LogUtil.Logger.InfoFormat("【巷道机任务执行操作库存返回消息】{0}----{1}",msg.Success,msg.Content);

                        break;
                    case StockTaskActionFlag.Fail:
                        if (taskItem.StockTaskType == StockTaskType.IN)
                        {
                            taskItem.State = StockTaskState.ErrorInStock;
                            dicQ.Dequeue();
                        }
                        else if (taskItem.StockTaskType == StockTaskType.OUT)
                        {
                            taskItem.State = StockTaskState.ErrorOutStock;
                            dicQ.Dequeue();
                        }else if (taskItem.StockTaskType == StockTaskType.AUTO_MOVE)
                        {
                            taskItem.State = StockTaskState.ErrorAutoMoveStock;
                            dicQ.Dequeue();
                        }
                        break;
                    default: break;
                }


            }
        }

        private string prevScanedBarcode = string.Empty;
        /// <summary>
        /// 将入库任务写入AGV扫描任务队列，并派发到AGV放行队列
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private bool CreateInTaskIntoAgvScanTaskQueue(string barcode)
        {
            lock (WriteTaskCenterQueueLocker)
            {
                StockTaskItem taskItem = new StockTaskItem(this.uiContext)
                {
                    Barcode = barcode,
                    StockTaskType = StockTaskType.IN
                };
                taskItem.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);

                if (!string.IsNullOrEmpty(barcode))
                {
                    UniqueItemService uniqItemService = new UniqueItemService(OPCConfig.DbString);
                    // UniqueItem item = uniqItemService.FindByCheckCode(barcode);
                    UniqueItem item = uniqItemService.FindByNr(barcode);
                    StockTaskService ts = new StockTaskService(OPCConfig.DbString);
                    StockTask lastTask = ts.UniqLastSotck(barcode);
                    //// 是否是重复扫描
                    if (BaseConfig.PreScanBar == barcode)
                    {

                        BaseConfig.PreScanBar = barcode;
                        if (item != null && (lastTask != null && lastTask.State == (int)StockTaskState.ErrorUniqNotExsits))
                        {
                            // 如果线存在，且上一个任务状态是不存在状态，那么不提示重复扫描
                        }else if( item==null && (lastTask != null && lastTask.State == (int)StockTaskState.ErrorUniqNotExsits))
                        {
                            taskItem.State = StockTaskState.ErrorUniqNotExsits;
                            if (TestConfig.ShowRescanErrorBarcode)
                            {
                                this.AddOrUpdateItemToTaskDisplay(taskItem);
                                // TaskCenterForDisplayQueue.Add(taskItem);
                            }
                            return false;
                        }
                        else
                        {
                            // 重复扫描的不再生成任务
                            taskItem.State = StockTaskState.ErrorBarcodeReScan;
                            if (TestConfig.ShowRescanErrorBarcode)
                            {
                                this.AddOrUpdateItemToTaskDisplay(taskItem);
                                // TaskCenterForDisplayQueue.Add(taskItem);
                            }
                            return false;
                        }
                    }

                    #region 入库
                    if (item != null)
                    { 
                        // 是否可以入库
                        if (this.CanBarCodeInstock(barcode) && uniqItemService.CanUniqInStock(barcode))
                        {
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
                        taskItem.AgvPassFlag = (byte)AgvPassFlag.Alarm;
                        taskItem.State = StockTaskState.ErrorUniqNotExsits;
                    }

                    // 先插入数据库Task再加入队列，最后置可读
                  //  StockTaskService ts = new StockTaskService(OPCConfig.DbString);
                    if (!ts.CreateInStockTask(taskItem))
                    {
                        taskItem.AgvPassFlag = (byte)AgvPassFlag.ReScan;
                        taskItem.State = StockTaskState.ErrorCreateDbTask;
                    }
                }
                else
                {
                    taskItem.AgvPassFlag = (byte)AgvPassFlag.ReScan;
                    return false;
                }
                #endregion
                BaseConfig.PreScanBar = barcode;
                // prevScanedBarcode = barcode;
                /// 加入到AGV扫描队列
                EnqueueAgvScanTaskQueue(taskItem);
                return true;
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
                //加入AGV扫描队列
                if (AgvScanTaskQueue.Keys.Contains(taskItem.Barcode))
                {
                    AgvScanTaskQueue[taskItem.Barcode] = taskItem;
                }
                else
                {
                    AgvScanTaskQueue.Add(taskItem.Barcode, taskItem);
                }

                this.AddOrUpdateItemToTaskDisplay(taskItem);

                //  TaskCenterForDisplayQueue.Add(taskItem);
                if (taskItem.StockTaskType == StockTaskType.IN)
                {
                    // 立刻加入到放行队列
                    StockTaskItem passTaskItem = AgvScanTaskQueue
                        .Where(s => s.Value.IsInProcessing == false)
                        .Select(s => s.Value).FirstOrDefault();
                    if (passTaskItem != null)
                    {
                        passTaskItem.IsInProcessing = true;
                        if (taskItem.AgvPassFlag == (byte)AgvPassFlag.Pass)
                        {
                            passTaskItem.State = StockTaskState.AgvWaitPassing;
                        }
                        // 进入agv放行
                        AgvInStockPassQueue.Enqueue(passTaskItem);
                    }
                }
                // 刷新界面列表
                //# RefreshList();
            }
        }
        #endregion

        #region 获得最优先的库存操作任务
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
                    //   StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                    //   sts.UpdateTaskState(receiveMessageQueue.Dequeue() as StockTask);
                    try
                    {
                        StockTask st = receiveMessageQueue.Dequeue() as StockTask;
                        StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                        sts.UpdateTaskState(st);
                        //Thread.Sleep(100);

                        LogUtil.Logger.InfoFormat("【后台更新任务】任务ID:{0}", st.Id);
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Logger.Error(ex.Message, ex);

                    }

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

                updatedStockTask.ToPositionNr = taskItem.ToPositionNr;
                updatedStockTask.ToPositionFloor = taskItem.ToPositionFloor;
                updatedStockTask.ToPositionColumn = taskItem.ToPositionColumn;
                updatedStockTask.ToPositionRow = taskItem.ToPositionRow;



                receiveMessageQueue.Enqueue(updatedStockTask);
                receivedEvent.Set();
                //StockTaskService sts = new StockTaskService(OPCConfig.DbString);
                //sts.UpdateTaskState(updatedStockTask);
            }
        }
        #endregion






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
            if (BaseConfig.IsOPCConnector)
            {
                this.StopTimers();
                this.ShutDownUpdateStackTaskStateComponent();
            }
            else
            {
                this.StopMonitorTimers();
            }
            this.ShutDownRabbitMQConnect();
            this.DisconnectOPCServer();
            this.ShutDownQuartzTaskSchedule();
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
        /// 关闭窗口时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(MessageBox.Show("PWS OPC Connnector在运行中，确认关闭？","关闭确认",MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ShutDownComponents();

                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }


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
                    if (this.RoadMachine1InTaskQueue.Count == 0) return true;
                    return RoadMachine1InTaskQueue.ToArray().
                         FirstOrDefault(s => (s as StockTaskItem).State == StockTaskState.RoadMachineStockBuffing) == null;
                case 2:
                    if (RoadMachine2InTaskQueue.Count == 0) return true;
                    return RoadMachine2InTaskQueue.ToArray().
                        FirstOrDefault(s => (s as StockTaskItem).State == StockTaskState.RoadMachineStockBuffing) == null;
                default:
                    return false;

            }
        }



        private bool CanBarCodeInstock(string barcode)
        {
            try
            {
                var queues = new List<Queue>()
            {
                AgvInStockPassQueue,
                InRobootPickQueue,
                RoadMachine1InTaskQueue,
                RoadMachine1CenterTaskQueue,
                RoadMachine2InTaskQueue,
                RoadMachine2CenterTaskQueue
            };

                foreach (var q in queues)
                {
                    if (q.ToArray().FirstOrDefault(s => (s as StockTaskItem).Barcode == barcode) != null)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
            return false;
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
                    if (this.RoadMachine1InTaskQueue.Count > 0)
                    {
                        dispatchedPositions.AddRange(
                         this.RoadMachine1InTaskQueue.ToArray().Select(s => (s as StockTaskItem).PositionNr).ToList()
                         );
                    } 
                }
                else if (roadMachineIndex == 2)
                {
                    if (this.RoadMachine2InTaskQueue.Count > 0)
                    {
                        dispatchedPositions.AddRange(
                         this.RoadMachine2InTaskQueue.ToArray().Select(s => (s as StockTaskItem).PositionNr).ToList()
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
        private object LoadOutStockTaskFromDbLocker = new object();
        private void LoadOutStockTaskFromDb(List<StockTaskState> states = null)
        {
            lock (LoadOutStockTaskFromDbLocker)
            {
               
                isLoadingDbTask = true;
                try
                {
                    int c1 = RoadMachine1OutTaskQueue.ToArray().Where(s => (s as StockTaskItem).StockTaskType == StockTaskType.OUT).Count();
                    int c2 = RoadMachine2OutTaskQueue.ToArray().Where(s => (s as StockTaskItem).StockTaskType == StockTaskType.OUT).Count();

                    if (c1 > 0 || c2 > 0)
                    {
                        // 当存在出库任务时，不加载出库
                    }
                    else
                    {
                        List<StockTask> stockTasks = new List<StockTask>();
                        if (states == null)
                        {
                            stockTasks = new StockTaskService(OPCConfig.DbString)
                                .GetInitOutStockTasksAndUpdateState(this.OutStockCenterQueue.Keys.ToList());
                        }
                        else
                        {
                            stockTasks = new StockTaskService(OPCConfig.DbString).GetTaskByStates(states);
                        }

                        if (stockTasks.Count > 0)
                        {
                            // 停止自动移库模式
                            #region 停止自动移库模式
                            if (stockTasks.Where(s => s.RoadMachineIndex == 1).Count() > 0)
                            {
                                this.StopMoveMode(1);
                            }
                            if (stockTasks.Where(s => s.RoadMachineIndex == 2).Count() > 0)
                            {
                                this.StopMoveMode(2);
                            }
                            #endregion

                            foreach (var st in stockTasks)
                            {

                                StockTaskItem taskItem = this.InitTaskItemByStockTask(st, true);

                                taskItem.State = StockTaskState.RoadMachineWaitOutStockDispatch;
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
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                }
                isLoadingDbTask = false;
            }

        }



        /// <summary>
        /// 将数据库中的任务加载到Task中,#TO-DO#
        /// </summary>
        private void InitQueueAndLoadTaskFromDb()
        {
            AgvInStockPassQueue = new Queue();
            InRobootPickQueue = new Queue();
           
            AgvScanTaskQueue = new Dictionary<string, StockTaskItem>();

            #region 初始化巷道机任务队列
            this.InitRoadMachineTaskQueue();
            #endregion


            TaskCenterForDisplayQueue = new List<StockTaskItem>();


            List<StockTask> tasks = new StockTaskService(OPCConfig.DbString)
                .GetTaskByStates(StockTaskItem.ShouldLoadFromDbStates);
            foreach (var task in tasks)
            {
                StockTaskItem item = this.InitTaskItemByStockTask(task);
                item.TaskStateChangeEvent += new StockTaskItem.TaskStateChangeEventHandler(TaskItem_TaskStateChangeEvent);
                AddOrUpdateItemToTaskDisplay(item);
                switch (item.State)
                {
                    case StockTaskState.AgvInStcoking:
                        InRobootPickQueue.Enqueue(item);
                        break;
                    case StockTaskState.RoadMachineStockBuffing:
                        this.EnqueueRoadMachineTask(item);
                        if (prevRoadMahineIndex == 0)
                        {
                            prevRoadMahineIndex = item.RoadMachineIndex;
                        }
                        break;
                    case StockTaskState.RoadMachineWaitOutStock:
                        this.EnqueueRoadMachineTask(item);
                        break;
                    default:
                        break;
                }
            }

            this.LoadOutStockTaskFromDb(new List<StockTaskState>() {
                StockTaskState.RoadMachineWaitOutStockDispatch
            });
            /// 设置显示

            // this.RefreshList();
            this.Dispatcher.Invoke(new Action(() =>
            {
                CenterStockTaskDisplayDG.ItemsSource = TaskCenterForDisplayQueue;  // CenterStockTaskDisplayDG.Items.Refresh();
            }));
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
                var f = OutStockCenterQueue.FirstOrDefault();
                this.OPCOutRobootPickData.BoxType = f.Value.FirstOrDefault().BoxType;
                this.OPCOutRobootPickData.TrayNum = f.Value.FirstOrDefault().TrayNum;//f.Value.Count();
                foreach (var taskItem in f.Value)
                {
                    taskItem.State = StockTaskState.RoadMachineWaitOutStock;
                    this.EnqueueRoadMachineTask(taskItem);
                    this.AddOrUpdateItemToTaskDisplay(taskItem);
                }
                OutStockCenterQueue.Remove(f.Key);
                return true;
            }
        }


        /// <summary>
        /// 更新显示列表
        /// </summary>
        private void RefreshList()
        {
            //this.Dispatcher.Invoke(new Action(() =>
            //{
               CenterStockTaskDisplayDG.Items.Refresh();

            //}));
        }

        private object addOrUpdateItemLocker = new object();
        /// <summary>
        /// 将任务放入显示列表
        /// </summary>
        /// <param name="taskItem"></param>
        private void AddOrUpdateItemToTaskDisplay(StockTaskItem taskItem, bool refresh = true)
        {
            lock (addOrUpdateItemLocker)
            {
                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    if (TaskCenterForDisplayQueue.Where(s => s.DbId == taskItem.DbId && taskItem.DbId > 0).FirstOrDefault() != null)
                    {
                        var i = TaskCenterForDisplayQueue.Where(s => s.DbId == taskItem.DbId && taskItem.DbId > 0).FirstOrDefault();
                    //  i = taskItem;

                    i.RoadMachineIndex = taskItem.RoadMachineIndex;

                        i.BoxType = taskItem.BoxType;

                        i.PositionNr = taskItem.PositionNr;
                        i.PositionFloor = taskItem.PositionFloor;
                        i.PositionColumn = taskItem.PositionColumn;
                        i.PositionRow = taskItem.PositionRow;

                        i.AgvPassFlag = taskItem.AgvPassFlag;
                        i.RestPositionFlag = taskItem.RestPositionFlag;
                        i.Barcode = taskItem.Barcode;
                        i.State = taskItem.State;
                        i.StockTaskType = taskItem.StockTaskType;
                        i.TrayReverseNo = taskItem.TrayReverseNo;
                        i.TrayNum = taskItem.TrayNum;
                        i.PickItemNum = taskItem.PickItemNum;
                        i.DbId = taskItem.DbId;
                        i.CreatedAt = taskItem.CreatedAt;


                        i.ToPositionNr = taskItem.ToPositionNr;
                        i.ToPositionFloor = taskItem.ToPositionFloor;
                        i.ToPositionColumn = taskItem.ToPositionColumn;
                        i.ToPositionRow = taskItem.ToPositionRow;

                        i.IsInProcessing = true;
                    }
                    else
                    {
                        TaskCenterForDisplayQueue.Add(taskItem);
                        if (refresh)
                        {
                            if (TaskCenterForDisplayQueue.Count > BaseConfig.MaxMonitorTaskNum)
                            {
                                TaskCenterForDisplayQueue.RemoveRange(0, TaskCenterForDisplayQueue.Count - BaseConfig.KeepMonitorTaskNum);
                            }
                            RefreshList();
                        }
                    }
                });
            }
        }


        private object getPostionLocker = new object();
        private Position GetPositionForDispatch(int roadMachineIndex)
        {
            lock (getPostionLocker)
            {
                PositionService ps = new PositionService(OPCConfig.DbString);

                Position position = ps.FindInStockPosition(roadMachineIndex,null, true,BaseConfig.IsUsePositionPriority);

                return position;
            }
        }


        #endregion

        private bool CanRoadMachineInStock(int roadMachineIndex)
        {
            bool can = false;
            if (roadMachineIndex == 2)
            {
                if ((BaseConfig.RoadMachine2Enabled
                    && (!OPCDataResetData.Xdj2InPaltformIsBuff)
                    && (ModeConfig.RoadMachine2TaskMode != RoadMachineTaskMode.OnlyOut))
                    && this.NotHasRoadMachineBuffingTask(roadMachineIndex))
                {
                    PositionService ps = new PositionService(OPCConfig.DbString);
                    bool hasAvaliablePosition = ps.FindInStockPosition(2, this.GetDispatchedPositions(2)) != null;
                    if (hasAvaliablePosition)
                    {
                        can = true;
                    }
                }
            }
            else if (roadMachineIndex == 1)
            {
                if ((BaseConfig.RoadMachine1Enabled
                        && (!OPCDataResetData.Xdj1InPaltformIsBuff)
                        && (ModeConfig.RoadMachine1TaskMode != RoadMachineTaskMode.OnlyOut))
                        && this.NotHasRoadMachineBuffingTask(roadMachineIndex))
                {

                    PositionService ps = new PositionService(OPCConfig.DbString);
                    bool hasAvaliablePosition = ps.FindInStockPosition(1,this.GetDispatchedPositions(1))!=null;
                    if (hasAvaliablePosition)
                    {
                        can = true;
                    }
                }
                //if ((BaseConfig.RoadMachine1Enabled
                //      && (!OPCDataResetData.Xdj1InPaltformIsBuff)
                //      && (ModeConfig.RoadMachine1TaskMode != RoadMachineTaskMode.OnlyOut)
                //    && (ModeConfig.RoadMachine1TaskMode != RoadMachineTaskMode.AutoMoveOnly))
                //      && this.NotHasRoadMachineBuffingTask(roadMachineIndex))
                //{ 
                //    can = true;
                //}
            }
            return can;
        }

        private void operatePanelButton_Click(object sender, RoutedEventArgs e)
        {
            new OperatePanelWindow(this).Show();
        }

    }
}