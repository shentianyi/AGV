using AGVCenterLib.Model.Command;
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


        string rm_host = "192.168.2.116";
        int rm_port = 5672;
        string rm_username = "agv";
        string rm_pwd = "agv";

        ConnectionFactory rmFactory;
        IConnection rmConnection;

        IModel operateChannel;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            OpenRabbitMQConnect();
        }

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                new MsgDialog(MsgLevel.Loading, "设置中,请稍后...",false, 2).ShowDialog();

                   Button b = sender as Button;
                if (b.Tag != null)
                {
                    string[] t = b.Tag.ToString().Split('-');
                    SendOperateCmd(int.Parse(t[0]), int.Parse(t[1]));
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ShutDownRabbitMQConnect();
        }

         
    }
}
