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
    /// <summary>
    /// RoadMachineModeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RoadMachineModeWindow : Window
    {
        public RoadMachineModeWindow()
        {
            InitializeComponent();
        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.OpenRabbitMQConnect();
            this.LoadTaskScheduleList();
        }

        string rm_host = RabbitMQConfig.Host;
        int rm_port = RabbitMQConfig.Port;
        string rm_username = RabbitMQConfig.UserName;
        string rm_pwd = RabbitMQConfig.Pwd;


        ConnectionFactory rmFactory;
        IConnection rmConnection;

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
                operateChannel.QueueDeclare(queue: "agv_center_operate_queue",
                    durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                #endregion

                // 状态发布
                #region
                stateInfoChannel = rmConnection.CreateModel();
                stateInfoChannel.ExchangeDeclare(exchange: "agv_center_state_info_exchange", type: ExchangeType.Fanout);

                QueueDeclareOk queueOK = stateInfoChannel.QueueDeclare();
                string queueName = queueOK.QueueName;

                LogUtil.Logger.Info("queue name:" + queueName);

                stateInfoChannel.QueueBind(queueName, "agv_center_state_info_exchange", "agv_center_state_info_exchange");

                var rmStateInfoConsumer = new EventingBasicConsumer(stateInfoChannel);

                rmStateInfoConsumer.Received += Consumer_CenterInfo_Received; ;

                stateInfoChannel.BasicConsume(queue: queueName,
                                     noAck: true,
                                     consumer: rmStateInfoConsumer);
                #endregion

                /// 发送新的客户端上线通知
                this.SendOperateCmd(0, 700);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        private void Consumer_CenterInfo_Received(object sender, BasicDeliverEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    var message = Encoding.UTF8.GetString(e.Body);
                    LogUtil.Logger.InfoFormat("【接收到状态信息】{0} ", message);
                    var cmd = JsonUtil.parse<AgvCenterStateInfoCmd>(message);
                    string[] tags = new string[2]
                    {
                    "1-"+cmd.RoadMachine1Mode.ToString(),
                    "2-"+cmd.RoadMachine2Mode.ToString()
                    };
                    foreach (var tag in tags) { 

                    }
                    foreach (var tag in tags)
                    {
                        foreach (var c in roadMachineControlGrid.Children)
                        {
                            SetControlButton(tag, c as Grid);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    LogUtil.Logger.Error(ex.Message, ex);
                }
            });
        }

        private void SetControlButton(string tag, Grid grid)
        {
            
            foreach (var c in grid.Children)
            {
                if (c is Grid) {
                    SetControlButton(tag, c as Grid);
                }
                else
                {
                    if (c is Button)
                    {
                        var b = (c as Button);
                        var t = b.Tag;

                        if (t != null && t.ToString() == tag)
                        {
                            b.Opacity = 1;
                        }
                        else
                        {
                            if (t.ToString().StartsWith(tag.Split('-')[0]))
                            {
                                b.Opacity = 0.4;
                            }
                        }
                    }
                }
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
                MessageBox.Show(ex.Message);
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// 点击按钮操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeWorkModeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                if (b.Tag != null)
                {
                    string[] t = b.Tag.ToString().Split('-');
                    SendOperateCmd(int.Parse(t[0]), int.Parse(t[1]));
                }
                new MsgDialog(MsgLevel.Loading, "设置中,请稍后...", false, 2).ShowDialog();
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
                    operateChannel.BasicPublish(exchange: "",
                        routingKey: "agv_center_operate_queue",
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



        private void LoadTaskScheduleList()
        {
            try
            {
                taskScheduleDG.ItemsSource =
                    new SystemServiceClient().GetListGreaterThan(DateTime.Now);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ShutDownRabbitMQConnect();
        }

        private void addTaskScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (startTimeDTP.Value == null || endTimeDTP.Value == null) {
                    MessageBox.Show("请填写开始时间和结束时间");
                }
                else
                {
                    new SystemServiceClient().CreateMoveTaskSchdule(new AGVCenterLib.Model.ViewModel.MoveTaskScheduleModel()
                    {
                        StartTime = startTimeDTP.Value.Value,
                        EndTime = endTimeDTP.Value.Value
                    });
                  
                    this.SendReloadTaskScheduleCmd();

                    this.LoadTaskScheduleList();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteTaskScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (taskScheduleDG.SelectedIndex > -1)
                {
                    foreach (var i in taskScheduleDG.SelectedItems.OfType<MoveTaskScheduleModel>())
                    {
                        new SystemServiceClient().DeleteMoveTaskSchedule(i.Id);
                    }
                    this.SendReloadTaskScheduleCmd();

                    this.LoadTaskScheduleList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addTasksScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (startTimeDTP.Value == null || endTimeDTP.Value == null || string.IsNullOrEmpty(daysTB.Text))
            {
                MessageBox.Show("请填写开始时间,结束时间和连续天数");
            }
            else
            { int days = 0;
                if (int.TryParse(daysTB.Text, out days))
                {
                    for (int i = 0; i < days; i++)
                    {
                        new SystemServiceClient().CreateMoveTaskSchdule(new AGVCenterLib.Model.ViewModel.MoveTaskScheduleModel()
                        {
                            StartTime = startTimeDTP.Value.Value.AddDays(i),
                            EndTime = endTimeDTP.Value.Value.AddDays(i)
                        });
                    }
                    this.LoadTaskScheduleList();
                }
            }
        }

        private void SendReloadTaskScheduleCmd()
        {
            try
            {
                SendOperateCmd(0, 600);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
