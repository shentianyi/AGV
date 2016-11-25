using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region 监控数据
        OPCGetInStockPosition OPCGetInStockPositionData;
        OPCGroup OPCGetInStockPositionOPCGroup;
        OPCSetInStockTask OPCSetInStockTaskData;
        OPCGroup OPCSetInStockTaskOPCGroup;
        /// <summary>
        /// 入库任务队列，以条码为键
        /// </summary>
        Dictionary<string, OPCSetInStockTask> InStockTaskQueue;
        private static object WriteInStockTaskLocker = new object();
        private static object InStockTaskQueueLocker = new object();
        #endregion

        #region 监控定时器
        /// <summary>
        /// 写入OPC入库任务定时器，将队列InStockTaskQueue的任务按序写入OPC
        /// </summary>
        Timer SetOPCInStockTaskTimer;
        #endregion

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            #region 加载初始化数据
            this.LoadTaskFromDb();
            #endregion

            #region 初始化基本数据
            foreach (var i in OPCConfig.OPCServers)
            {
                OPCServersLB.Items.Add(i);
            }
            OPCServerTB.Text = OPCConfig.OPCServer;
            OPCNodeNameTB.Text = OPCConfig.OPCNodeName;

            #endregion

            #region 初始化OPC数据
            OPCGetInStockPositionData = new OPCGetInStockPosition();
            OPCGetInStockPositionData.RwFlagChangedEvent += new OPCGetInStockPosition.RwFlagChangedEventHandler(OPCGetInStockPositionData_RwFlagChangedEvent);
            OPCSetInStockTaskData = new OPCSetInStockTask();
            OPCSetInStockTaskData.RwFlagChangedEvent += new OPCSetInStockTask.RwFlagChangedEventHandler(OPCSetInStockTaskData_RwFlagChangedEvent);


            #endregion

            #region 初始化定时器
            SetOPCInStockTaskTimer = new Timer();
            SetOPCInStockTaskTimer.Interval = 100;
            SetOPCInStockTaskTimer.Enabled = true;
            SetOPCInStockTaskTimer.Elapsed += SetOPCInStockTaskTimer_Elapsed;
            SetOPCInStockTaskTimer.Start();
            #endregion
        }




        /// <summary>
        /// 验证可读和读取入库条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetOPCInStockTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            SetOPCInStockTaskTimer.Stop();

            // 是否存在任务？ 并OPC可写
            if (OPCSetInStockTaskData.CanWrite && this.HasWatingInStockTask())
            {
                OPCSetInStockTask task = this.DequeueInStockTaskQueue();

                LogUtil.Logger.InfoFormat("【派发任务】条码为{0}, 库位为{1}-{2}-{3}", task.Barcode, task.PositionFloor, task.PositionColumn, task.PositionRow);
                if (task.SyncWrite(OPCSetInStockTaskOPCGroup))
                {
                    task.State = InStockTaskState.InStocking;
                    // 置为可读，有新的任务
                    task.SyncSetReadableFlag(OPCSetInStockTaskOPCGroup);
                }
            }

            SetOPCInStockTaskTimer.Start();
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
                ConnectedOPCServer.ServerShutDown += ConnectedOPCServer_ServerShutDown;

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
        private void ConnectedOPCServer_ServerShutDown(string Reason)
        {
            LogUtil.Logger.Info(string.Format("【OPC Sever 自停止】{0}", Reason));
            #region 关闭Timer\线程 等活动
            ShutDownComponents();
            #endregion
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
        /// 初始化组
        /// </summary>
        private bool InitOPCGroup()
        {
            try
            {
                #region 初始化扫描入库组
                // 初始化扫描入库组
                OPCGetInStockPositionOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCGetInStockPositionOPCGroupName);
                OPCGetInStockPositionOPCGroup.UpdateRate = OPCConfig.OPCGetInStockPositionOPCGroupRate;
                OPCGetInStockPositionOPCGroup.DeadBand = OPCConfig.OPCGetInStockPositionOPCGroupDeadBand;
                OPCGetInStockPositionOPCGroup.IsSubscribed = true;
                OPCGetInStockPositionOPCGroup.IsActive = true;

                // 添加item
                OPCGetInStockPositionData.AddItemToGroup(OPCGetInStockPositionOPCGroup);
                OPCGetInStockPositionOPCGroup.DataChange +=  OPCGetInStockPositionOPCGroup_DataChange;
                #endregion


                #region 初始化入库任务组
                // 初始化入库任务组
                OPCSetInStockTaskOPCGroup = ConnectedOPCServer.OPCGroups.Add(OPCConfig.OPCSetInStockTaskOPCGroupName);
                OPCSetInStockTaskOPCGroup.UpdateRate = OPCConfig.OPCSetInStockTaskOPCGroupRate;
                OPCSetInStockTaskOPCGroup.DeadBand = OPCConfig.OPCSetInStockTaskOPCGroupDeadBand;
                OPCSetInStockTaskOPCGroup.IsSubscribed = true;
                OPCSetInStockTaskOPCGroup.IsActive = true;
                // 添加item
                OPCSetInStockTaskData.AddItemToGroup(OPCSetInStockTaskOPCGroup);
                OPCSetInStockTaskOPCGroup.DataChange += OPCSetInStockTaskOPCGroup_DataChange;
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


        // ' This sub handles the 'DataChange' call back event which returns data that has
        //' been detected as changed within the OPC Server.  This call back should be
        //' used primarily to receive the data.  Do not make any other calls back into
        //' the OPC server from this call back.  The other item related functions covered
        //' in this example have shown how the ItemServerHandle is used to control and
        //' manipulate individual items in the OPC server.  The 'DataChange' event allows
        //' us to see how the 'ClientHandles we gave the OPC Server when adding items are
        //' used.  As you can see here the server returns the 'ClientHandles' as an array.
        //' The number of item returned in this event can change from trigger to trigger
        //' so don't count on always getting a 1 to 1 match with the number of items
        //' you have registered.  That where the 'ClientHandles' come into play.  Using
        //' the 'ClientHandles' returned here you can determine what data has changed and
        //' where in your application the data should go.  In this example the
        //' 'ClientHandles' were the Index number of each item we added to the group.
        //' Using this returned index number the 'DataChange' handler shown here knows
        //' what controls need to be updated with new data.  In your application you can
        //' make the client handles anything you like as long as they allow you to
        //' uniquely identify each item as it returned in this event.
        // 第一次会获取到opcserver的数据，即使没有触发，相当于初始化
        // 扫描入库的信息获取
        private void OPCGetInStockPositionOPCGroup_DataChange(
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
                    LogUtil.Logger.InfoFormat("【数据改变】{0}", ItemValues.GetValue(i));
                }
                OPCGetInStockPositionData.SetValue(NumItems,ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// 设置入库任务的数据改变事件处理
        /// </summary>
        /// <param name="TransactionID"></param>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        /// <param name="Qualities"></param>
        /// <param name="TimeStamps"></param>
        private void OPCSetInStockTaskOPCGroup_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                // 从1开始
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【数据改变】{0}", ItemValues.GetValue(i));
                }
                OPCSetInStockTaskData.SetValue(NumItems, ClientHandles, ItemValues);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 读取入库条码信息读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCGetInStockPositionData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            if (b.CanRead)
            {
                // 读取条码，获取库位写入任务队列
                LogUtil.Logger.InfoFormat("【根据-入库条码-获取库位】{0}", OPCGetInStockPositionData.ScanGetInposiBarcode);
                this.WriteOPCSetInStockTaskData(OPCGetInStockPositionData.ScanGetInposiBarcode);
            }
        }

        /// <summary>
        /// 入库任务读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCSetInStockTaskData_RwFlagChangedEvent(OPCDataBase b, byte toFlag)
        {
            LogUtil.Logger.InfoFormat("【OPC入库任务读写标记改变】{0}",OPCSetInStockTaskData.OPCRwFlag);
            //if (b.CanWrite)
            //{
            // 这边就不写了，使用定时器写入库任务！
            //}
        }

        private bool WriteOPCSetInStockTaskData(string barcode)
        {
            lock (WriteInStockTaskLocker)
            {
                OPCSetInStockTask inStockTask = new OPCSetInStockTask() { Barcode = barcode };
                inStockTask.OPCItemIDs = OPCSetInStockTaskData.OPCItemIDs;
                inStockTask.ClientHandles = OPCSetInStockTaskData.ClientHandles;
                inStockTask.ItemServerHandles = OPCSetInStockTaskData.ItemServerHandles;
                inStockTask.AddItemServerErrors = OPCSetInStockTaskData.AddItemServerErrors;

                UniqueItemService uniqItemService = new UniqueItemService(OPCConfig.DbString);
                UniqueItemView item = uniqItemService.FindDetail(barcode);
                if (item == null)
                {
                    // 是否可以入库
                    if (uniqItemService.CanUniqInStock(barcode))
                    {
                        // 是否是重复扫描
                        if (InStockTaskQueue.Keys.Contains(barcode))
                        {
                            return true;
                        }
                        inStockTask.AgvPassFlag = (byte)AgvPassFlag.Pass;

                        // 查询可用库位！
                        PositionService ps = new PositionService(OPCConfig.DbString);
                        Position position = ps.FindInStockPosition();
                        if (position == null)
                        {
                            inStockTask.AgvPassFlag = (byte)AgvPassFlag.Err;
                            inStockTask.State = InStockTaskState.ErrorNoPositoin;
                        }
                        else
                        {
                            inStockTask.PositionFloor = (byte)position.Floor;
                            inStockTask.PositionColumn = (byte)position.Column;
                            inStockTask.PositionRow = (byte)position.Row;
                            inStockTask.BoxType = (byte)item.BoxType;
                            inStockTask.RestPositionFlag = 0x00;
                            inStockTask.State = InStockTaskState.WaitingStcok;
                        }
                    }
                    else
                    {
                        // 不可入库
                        inStockTask.AgvPassFlag = (byte)AgvPassFlag.Err;
                        inStockTask.State = InStockTaskState.ErrorUniqCannotInStock;
                    }
                }
                else
                {
                    inStockTask.AgvPassFlag = (byte)AgvPassFlag.Err;
                    inStockTask.State = InStockTaskState.ErrorUniqNotExsits;
                }

                // 先插入数据库Task再加入队列，最后置可读
                StockTaskService ts = new StockTaskService(OPCConfig.DbString);
                if (ts.CreateInStockTask(inStockTask))
                {
                    if (inStockTask.AgvPassFlag == (byte)AgvPassFlag.Pass)
                    {
                        // 加入队列
                        inStockTask.State = InStockTaskState.WaitingStcok;
                        this.EnqueueInStockTaskQueue(inStockTask);
                    }
                }
                return false;
            }
        }

        #region 入库任务队列
        /// <summary>
        /// 是否存在需要入库的任务
        /// </summary>
        /// <returns></returns>
        private bool HasWatingInStockTask()
        {
            lock (InStockTaskQueueLocker)
            {
                if (this.InStockTaskQueue != null 
                    && this.InStockTaskQueue.Count(s => s.Value.State == InStockTaskState.WaitingStcok) > 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 出栈入库任务
        /// </summary>
        /// <returns></returns>
        private OPCSetInStockTask DequeueInStockTaskQueue()
        {
            lock (InStockTaskQueueLocker)
            {
                if (this.HasWatingInStockTask())
                {
                    var task = InStockTaskQueue.FirstOrDefault(s => s.Value.State == InStockTaskState.WaitingStcok).Value;
                    if (task != null)
                    {
                        task.State = InStockTaskState.InStocking;
                        RefreshList();
                        return task;
                    }
                }
                return null;
            }
        }


        /// <summary>
        /// 入栈入库任务
        /// </summary>
        /// <returns></returns>
        private void EnqueueInStockTaskQueue(OPCSetInStockTask task)
        {
            lock (InStockTaskQueueLocker)
            {
                task.State = InStockTaskState.WaitingStcok;
                InStockTaskQueue.Add(task.Barcode, task);
               
                ///将扫描OPC设置为可写
                ///task.SyncSetWriteableFlag(OPCSetInStockTaskOPCGroup);
                OPCGetInStockPositionData.SyncSetWriteableFlag(OPCGetInStockPositionOPCGroup);
                RefreshList();
            }
        }
        #endregion

        /// <summary>
        /// 将数据库中的任务加载到Task中
        /// </summary>
        private void LoadTaskFromDb()
        {
            InStockTaskQueue = new Dictionary<string, OPCSetInStockTask>();
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
            this.DisconnectOPCServer();
            this.StopTimers();
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
                    ShutDownComponents();
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
           if(SetOPCInStockTaskTimer != null)
            {
                SetOPCInStockTaskTimer.Enabled = false;
                SetOPCInStockTaskTimer.Stop();
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
            if (OPCGetInStockPositionData.CanWrite)
            {
                OPCGetInStockPositionData.ScanGetInposiBarcode = ScanedBarCodeTB.Text;
                if (OPCGetInStockPositionData.SyncWrite(OPCGetInStockPositionOPCGroup))
                {
                    MessageBox.Show("条码读取成功");
                }
            }
            else
            {
                MessageBox.Show("存在旧任务，OPC暂不可以写入数据");
            }
        }


        /// <summary>
        /// 读取入库条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadScanedBarCodeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (OPCGetInStockPositionData.CanRead)
            {
                if (OPCGetInStockPositionData.SyncSetWriteableFlag(OPCGetInStockPositionOPCGroup))
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
            if (OPCSetInStockTaskData.CanRead)
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
            foreach(var t in InStockTaskQueue)
            {
                InStockTaskLB.Items.Add(t.Value.ToDisplay());
            }


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

       
    }
}
