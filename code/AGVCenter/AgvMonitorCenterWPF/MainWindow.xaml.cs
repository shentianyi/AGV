
using AGVCenterLib.Config;
using AGVCenterLib.Model.Command;
using AGVCenterLib.Model.OPC;
using AgvMonitorCenterWPF.Properties;
using Brilliantech.Framwork.Utils.JsonUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using OPCAutomation;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AgvMonitorCenterWPF
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


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitOPC();
             this.ConnectOPC();
            this.OpenRabbitMQConnect();
        }

        #region OPC 组件

        #region OPC变量
        OPCServer ConnectedOPCServer;
        List<OPCAgvStateInfo> opcAgvInfos;
        List<OPCGroup> opcAgvInfoGrops;
        #endregion

        private void ConnectOPCServerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.InitOPC();
            this.ConnectOPC();
        }

        private void DisConnectOPCServerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DisconnectOPCServer();
        }

        /// <summary>
        ///  初始化OPC
        /// </summary>
        private void InitOPC()
        {
            OPCServerTB.Text = Settings.Default.OPCServerName;
            OPCNodeNameTB.Text = Settings.Default.OPCNodeName;

            this.opcAgvInfos = new List<OPCAgvStateInfo>();
            for (int i = 1; i <= 6; i++)
            {
                this.opcAgvInfos.Add(new OPCAgvStateInfo(string.Format("AGV{0}StateInfo", i), i));
            }
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

                if (ConnectedOPCServer.ServerState == (int)OPCServerState.OPCRunning)
                {
                    /// 初始化OPC组
                    if (InitOPCGroup())
                    {
                     //   ConnectOPCServerBtn.Content = "已连接OPC服务器";
                        ConnectOPCServerBtn.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
              //  ConnectOPCServerBtn.Content = "连接服务";

                ConnectedOPCServer = null;
                ConnectOPCServerBtn.IsEnabled = true;
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 初始化OPC组
        /// </summary>
        private bool InitOPCGroup()
        {
            try
            {
                opcAgvInfoGrops = new List<OPCGroup>();
                for (int i = 1; i <= 6; i++)
                {
                    OPCGroup opcAgvInfoGroup = ConnectedOPCServer.OPCGroups.Add(string.Format("OPCAgvInfoGroup-{0}", i));

                    opcAgvInfoGroup.UpdateRate = 100;
                    opcAgvInfoGroup.DeadBand = 0;
                    opcAgvInfoGroup.IsSubscribed = true;
                    opcAgvInfoGroup.IsActive = true;
                    if (i == 1)
                    {
                        opcAgvInfoGroup.DataChange += OpcAgvInfoGroup1_DataChange;
                    }
                    else if (i == 2)
                    {
                        opcAgvInfoGroup.DataChange += OpcAgvInfoGroup2_DataChange;
                    }
                    else if (i == 3)
                    {
                        opcAgvInfoGroup.DataChange += OpcAgvInfoGroup3_DataChange;
                    }
                    else if (i == 4)
                    {
                        opcAgvInfoGroup.DataChange += OpcAgvInfoGroup4_DataChange;
                    }
                    else if (i == 5)
                    {
                        opcAgvInfoGroup.DataChange += OpcAgvInfoGroup5_DataChange;
                    }
                    else if (i == 6)
                    {
                        opcAgvInfoGroup.DataChange += OpcAgvInfoGroup6_DataChange;
                    }

                    // 添加item
                    opcAgvInfos[i-1].AddItemToGroup(opcAgvInfoGroup);

                    opcAgvInfoGrops.Add(opcAgvInfoGroup);
                 
                }
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void OpcAgvInfoGroup1_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            this.SetOPCDataValue(1, TransactionID, NumItems, ref ClientHandles, ref ItemValues, ref Qualities, ref TimeStamps);
        }
        private void OpcAgvInfoGroup2_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            this.SetOPCDataValue(2, TransactionID, NumItems, ref ClientHandles, ref ItemValues, ref Qualities, ref TimeStamps);
        }
        private void OpcAgvInfoGroup3_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            this.SetOPCDataValue(3, TransactionID, NumItems, ref ClientHandles, ref ItemValues, ref Qualities, ref TimeStamps);
        }
        private void OpcAgvInfoGroup4_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            this.SetOPCDataValue(4, TransactionID, NumItems, ref ClientHandles, ref ItemValues, ref Qualities, ref TimeStamps);
        }
        private void OpcAgvInfoGroup5_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            this.SetOPCDataValue(5, TransactionID, NumItems, ref ClientHandles, ref ItemValues, ref Qualities, ref TimeStamps);
        }
        private void OpcAgvInfoGroup6_DataChange(int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            this.SetOPCDataValue(6, TransactionID, NumItems, ref ClientHandles, ref ItemValues, ref Qualities, ref TimeStamps);
        }

        private void SetOPCDataValue(int agvId, int TransactionID, int NumItems, ref Array ClientHandles, ref Array ItemValues, ref Array Qualities, ref Array TimeStamps)
        {
            try
            {
                for (var i = 1; i <= NumItems; i++)
                {
                    LogUtil.Logger.InfoFormat("【AGV状态数据改变】【{0}】{1}",
                        agvId,
                        ItemValues.GetValue(i));
                }
                var info = GetAgvStateInfoById(agvId);
                if (info != null)
                {
                    info.SetValue(NumItems, ClientHandles, ItemValues);
                    // 发布消息
                    this.PublishAgvStateInfos(agvId);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        public OPCAgvStateInfo GetAgvStateInfoById(int agvId)
        {
            return this.opcAgvInfos.Where(s => s.Id == agvId).FirstOrDefault();
        }

        public List<OPCAgvStateInfo> GetAgvStateInfosById(int agvId)
        {
            return this.opcAgvInfos.Where(s => s.Id == agvId).ToList();
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
               // ConnectOPCServerBtn.Content = "连接服务";
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                ConnectedOPCServer = null;
                ConnectOPCServerBtn.IsEnabled = true;
              //  ConnectOPCServerBtn.Content = "连接服务";
            }
        }
        #endregion

        #region 运行时变量

        #endregion

        #region RabbitMQ 组件
        string rm_host = RabbitMQConfig.Host;
        int rm_port = RabbitMQConfig.Port;
        string rm_username = RabbitMQConfig.UserName;
        string rm_pwd = RabbitMQConfig.Pwd;

        ConnectionFactory rmFactory;
        IConnection rmConnection;

        EventingBasicConsumer rmOperateConsumer;
        IModel operateChannel;
        IModel stateInfoChannel;

        /// <summary>
        /// 开启RM连接
        /// </summary>
        public void OpenRabbitMQConnect()
        {
            try
            {
                rmFactory = new ConnectionFactory() { HostName = rm_host, Port = rm_port };
                rmFactory.UserName = rm_username;
                rmFactory.Password = rm_pwd;

                rmFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

                rmConnection = rmFactory.CreateConnection();
                // 控制设置
                #region 控制设置
                operateChannel = rmConnection.CreateModel();
                operateChannel.QueueDeclare(queue: "agv_car_operate_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                rmOperateConsumer = new EventingBasicConsumer(operateChannel);
                rmOperateConsumer.Received += RmOperateConsumer_Received; ;

                operateChannel.BasicConsume(queue: "agv_car_operate_queue",
                                     noAck: true,
                                     consumer: rmOperateConsumer);
                #endregion


                // 状态发布
                #region
                stateInfoChannel = rmConnection.CreateModel();
                stateInfoChannel.ExchangeDeclare(exchange: "agv_car_state_info_exchange", type: ExchangeType.Fanout);
                #endregion
                // 发送状态消息
                this.PublishAgvStateInfos();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 关闭RM连接
        /// </summary>
        public void ShutDownRabbitMQConnect()
        {
            try
            {
                if (operateChannel != null)
                {
                    operateChannel.Close();
                }
                if (stateInfoChannel != null)
                {
                    stateInfoChannel.Close();
                }
                if (rmConnection != null)
                {
                    rmConnection.Close();
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 接收到消息处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RmOperateConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                var message = Encoding.UTF8.GetString(e.Body);
                var cmd = JsonUtil.parse<AgvCenterCmd>(message);
                LogUtil.Logger.InfoFormat("【接收到设置命令】{0}---{1}", cmd.RoadMachindeIndex, cmd.CmdType);
                LogUtil.Logger.InfoFormat("【接收到设置命令信息】 ", message);

                // 其它任务
                switch (cmd.CmdType)
                {
                    // 新的监控客户端连入
                    case 701:
                        PublishAgvStateInfos();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 发布状态消息
        /// </summary>
        private void PublishAgvStateInfos(int? agvId = null)
        {
            try
            {
                if (stateInfoChannel != null)
                {
                    var infos = agvId.HasValue ? GetAgvStateInfosById(agvId.Value) : opcAgvInfos;

                    foreach (var info in infos)
                    {
                        string message = JsonUtil.stringify(new AgvCarInfoMeta() {
                            Id=info.Id,
                            State=info.State,
                            Point=info.Point,
                            Route=info.Route,
                            Voltage=info.Voltage
                        });

                        LogUtil.Logger.InfoFormat("【发送状态消息】{0}", message);
                        stateInfoChannel.BasicPublish(exchange: "agv_car_state_info_exchange",
                                             routingKey: "agv_car_state_info_exchange",
                                             basicProperties: null,
                                             body: Encoding.UTF8.GetBytes(message));
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        #endregion

        #region 消息队列数据处理组件
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("确认关闭？", "确认关闭？", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                this.ShutDownRabbitMQConnect();
                this.DisconnectOPCServer();
            }
        }
    }
}
