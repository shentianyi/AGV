using AGVCenterLib.Config;
using AGVCenterLib.Model.Command;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvStorageService;
using AgvClientWPF.AgvSystemService;
using Brilliantech.Framwork.Utils.JsonUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AgvClientWPF.Control
{
    public enum AlarmType
    {
        NeedCharge,
        InStockStoped
    };
    /// <summary>
    /// RoadMachineModeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AgvMonitorWindow : Window
    {
        public AgvMonitorWindow()
        {
            InitializeComponent();
        }

        List<AgvCarInfo> agvCarInfos = new List<AgvCarInfo>();
        ConveyerBeltInfo conveyerBeltInfo = new ConveyerBeltInfo();

        SoundPlayer player = new System.Media.SoundPlayer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           this.OpenRabbitMQConnect();
            player.SoundLocation =
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\alarm.wav");

            this.InitAndLoadInfo();
        }

        string rm_host = RabbitMQConfig.Host;
        int rm_port = RabbitMQConfig.Port;
        string rm_username = RabbitMQConfig.UserName;
        string rm_pwd = RabbitMQConfig.Pwd;


        ConnectionFactory rmFactory;
        IConnection rmConnection;

        IModel operateChannel;
        IModel stateInfoChannel;
        IModel conveyerBeltInfoChannel;
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
                operateChannel.QueueDeclare(queue: "monitor_operate_queue",
                    durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                #endregion

                // 状态发布 订阅
                #region
                stateInfoChannel = rmConnection.CreateModel();
                stateInfoChannel.ExchangeDeclare(exchange: "agv_car_monitor_exchange", type: ExchangeType.Fanout);

                QueueDeclareOk queueOK = stateInfoChannel.QueueDeclare();
                string queueName = queueOK.QueueName;

                LogUtil.Logger.Info("agv info queue name:" + queueName);

                stateInfoChannel.QueueBind(queueName, "agv_car_monitor_exchange", "agv_car_monitor_exchange");

                var rmStateInfoConsumer = new EventingBasicConsumer(stateInfoChannel);

                rmStateInfoConsumer.Received += Consumer_Monitor_Received; ;

                stateInfoChannel.BasicConsume(queue: queueName,
                                     noAck: true,
                                     consumer: rmStateInfoConsumer);



                conveyerBeltInfoChannel = rmConnection.CreateModel();
                conveyerBeltInfoChannel.ExchangeDeclare(exchange: "conveyer_belt_monitor_exchange", type: ExchangeType.Fanout);
                QueueDeclareOk queue1OK = conveyerBeltInfoChannel.QueueDeclare();
                string queue1Name = queue1OK.QueueName;
                LogUtil.Logger.Info("converybelt info queue name:" + queue1Name);


                conveyerBeltInfoChannel.QueueBind(queueName, "conveyer_belt_monitor_exchange", "conveyer_belt_monitor_exchange");

                var rmBeltStateInfoConsumer = new EventingBasicConsumer(conveyerBeltInfoChannel);

                rmBeltStateInfoConsumer.Received += RmBeltStateInfoConsumer_Received; ; ;

                conveyerBeltInfoChannel.BasicConsume(queue: queue1Name,
                                     noAck: true,
                                     consumer: rmBeltStateInfoConsumer);
                #endregion

                /// 发送新的客户端上线通知
                this.SendOperateCmd(0, 701);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        private void RmBeltStateInfoConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(e.Body);

                    LogUtil.Logger.InfoFormat("【接收到状态信息】{0} ", message);
                    var infoMeta = JsonUtil.parse<ConveyerBeltInfoMeta>(message);

                    conveyerBeltInfo.BigBoxBeltEmptyState = infoMeta.BigBoxBeltEmptyState;
                    conveyerBeltInfo.SmallBoxBeltEmptyState = infoMeta.SmallBoxBeltEmptyState;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    LogUtil.Logger.Error(ex.Message, ex);
                }
            });
        }

        private void ConveyerBeltInfo_BigBoxBeltStateChangedEvent(ConveyerBeltInfo conveyerBeltInfo)
        {
            this.Alarm();
          //  throw new NotImplementedException();
        }

        private void ConveyerBeltInfo_SmallBoxBeltStateChangedEvent(ConveyerBeltInfo conveyerBeltInfo)
        {
            this.Alarm();
            // throw new NotImplementedException();
        }

        private bool isAlarmPlaying = false;
        private void Consumer_Monitor_Received(object sender, BasicDeliverEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(e.Body);

                    LogUtil.Logger.InfoFormat("【接收到状态信息】{0} ", message);
                    var infoMeta = JsonUtil.parse<AgvCarInfoMeta>(message);
                    var info = this.agvCarInfos.FirstOrDefault(s => s.Id == infoMeta.Id);
                    if (info != null)
                    {
                        info.State = infoMeta.State;//"2";//
                        info.Point = infoMeta.Point;//"21" ;//
                        info.Route = infoMeta.Route;
                        info.Voltage = infoMeta.Voltage;
                    }
                    else
                    {
                        info = new AgvCarInfo(5)
                        {
                            Id = infoMeta.Id,
                            State = "2",// infoMeta.State,
                            Point = "21",// infoMeta.Point,
                            Route = infoMeta.Route,
                            Voltage = infoMeta.Voltage
                        };

                        //info = new AgvCarInfo(5)
                        //{
                        //    Id = infoMeta.Id,
                        //    State = infoMeta.State,
                        //    Point = infoMeta.Point,
                        //    Route = infoMeta.Route,
                        //    Voltage = infoMeta.Voltage
                        //};
                        info.AgvStopInStockEvent += Info_AgvStopInStockEvent;
                        info.AgvNeedChargeEvent += Info_AgvNeedChargeEvent;

                        this.agvCarInfos.Add(info);
                    }

                    this.StopAlarm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    LogUtil.Logger.Error(ex.Message, ex);
                }
            });
        }
        // 停止播放声音
        private void StopAlarm()
        {
            this.player.Stop();
            isAlarmPlaying = false;
        }
        private void Info_AgvNeedChargeEvent(AgvCarInfo agvCarInfo)
        {
            LogUtil.Logger.InfoFormat("{0}号小车触发需要充电事件", agvCarInfo.Id);
            this.Alarm();
        }

        private void Info_AgvStopInStockEvent(AgvCarInfo agvCarInfo)
        {
            LogUtil.Logger.InfoFormat("{0}号小车触发入库等待超时事件", agvCarInfo.Id);
            this.Alarm();
        }


        private void Alarm(AlarmType? alarmType=null)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (this.HasAlarm())
                    {
                        if (isAlarmPlaying == false)
                        {
                            player.LoadAsync();
                            player.PlayLooping();
                            isAlarmPlaying = true;
                        }
                    }
                    else
                    {
                        if (this.isAlarmPlaying == true)
                        {
                            this.StopAlarm();
                            this.isAlarmPlaying = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                    MessageBox.Show(ex.Message);
                }
            });
        }

        private bool HasAlarm()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (this.conveyerBeltInfo.IsBigBoxBeltEmpty 
                || this.conveyerBeltInfo.IsSmallBoxBeltEmpty)
                {
                    this.beltAlarmBtn.Background = Brushes.Red;
                }
                else
                {
                    this.beltAlarmBtn.Background = Brushes.Green;
                }

                if (this.agvCarInfos.Count(s => s.IsInStockScanStopOverTime) > 0)
                {
                    this.agvStopAlarmBtn.Background = Brushes.Red;
                }
                else
                {
                    this.agvStopAlarmBtn.Background = Brushes.Green;
                }

                if (this.agvCarInfos.Count(s => s.IsNeedCharge) > 0)
                {
                    this.agvNeedChargeBtn.Background = Brushes.Red;
                }
                else
                {
                    this.agvNeedChargeBtn.Background = Brushes.Green;
                }
            });
            return
                this.conveyerBeltInfo.IsBigBoxBeltEmpty ||
                this.conveyerBeltInfo.IsSmallBoxBeltEmpty ||
                (this.agvCarInfos.Count(s => s.IsInStockScanStopOverTime || s.IsNeedCharge) > 0);
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
                MessageBox.Show(ex.Message);
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        /// <summary>
        /// 发送操作命令，改变工作模式
        /// </summary>
        /// <param name="roadMachindeIndex"></param>
        /// <param name="cmdType"></param>
        private void SendOperateCmd(int roadMachindeIndex, int cmdType)
        {
            try
            {
                LogUtil.Logger.InfoFormat("【发送设置命令】{0}---{1}", roadMachindeIndex, cmdType);
                AgvCenterCmd cmd = new AgvCenterCmd()
                {
                    CmdType = cmdType,
                    RoadMachindeIndex = roadMachindeIndex
                };

                string message = JsonUtil.stringify(cmd);
                if (operateChannel != null)
                {
                    operateChannel.BasicPublish(
                        exchange: "",
                        routingKey: "monitor_operate_queue",
                        basicProperties: null,
                        body: Encoding.UTF8.GetBytes(message));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        private void InitAndLoadInfo()
        {
            this.avgStateInfoDG.ItemsSource = this.agvCarInfos;
            this.conveyerBeltInfo.SmallBoxBeltStateChangedEvent += ConveyerBeltInfo_SmallBoxBeltStateChangedEvent;
            this.conveyerBeltInfo.BigBoxBeltStateChangedEvent += ConveyerBeltInfo_BigBoxBeltStateChangedEvent;
        }

      

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShutDownRabbitMQConnect();
            try
            {
                this.player.Stop();
            }
            catch(Exception ex) {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var info in agvCarInfos)
            {
                info.State = "1";
                info.Route = "11";
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.agvCarInfos[new Random().Next(0, 5)].State = "2";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            this.agvCarInfos[new Random().Next(0, 5)].Route = "13";
        }
    }
}
