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
using AGVCenterLib.Model.OPC;
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
         
        #endregion
        #region 监控定时器
        Timer OPCGetInStockStockDataTimer;
        #endregion

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            OPCGetInStockPositionData.RwFlagChangedEvent += OPCGetInStockPositionData_RwFlagChangedEvent;
            #endregion

            #region 初始化定时器
            OPCGetInStockStockDataTimer = new Timer();
            OPCGetInStockStockDataTimer.Interval = 100;
            OPCGetInStockStockDataTimer.Enabled = true;
            OPCGetInStockStockDataTimer.Elapsed += OPCGetInStockStockDataTimer_Elapsed;
            // OPCGetInStockStockDataTimer.Start();
            #endregion
        }


        /// <summary>
        /// 验证可读和读取入库条码信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OPCGetInStockStockDataTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //OPCGetInStockStockDataTimer.Stop();

            //OPCGetInStockStockDataTimer.Start();
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
                OPCGetInStockPositionOPCGroup.DataChange += OPCGetInStockPositionOPCGroup_DataChange;
                
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
        /// 读取入库条码信息读写标记改变处理
        /// </summary>
        /// <param name="b"></param>
        /// <param name="toFlag"></param>
        private void OPCGetInStockPositionData_RwFlagChangedEvent(Base b, byte toFlag)
        {
            if (b.CanRead)
            {
                // 读取条码，获取库位写入OPC
                LogUtil.Logger.InfoFormat("【根据-入库条码-获取库位】", OPCGetInStockPositionData.ScanGetInposiBarcode);
            }
        }

        private void WriteOPCSetInStockTaskData()
        {

        }

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
    }
}
