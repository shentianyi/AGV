using AGVCenterLib.Enum;
using AGVCenterLib.Model.Command;
using AGVCenterWPF.Config;
using Brilliantech.Framwork.Utils.JsonUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        #region 调库模式设置方法
        /// <summary>
        /// 是否是调库模式
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <returns></returns>
        public bool IsMoveStockMode(int roadMachineIndex)
        {
            return (roadMachineIndex == 1 ? ModeConfig.RoadMachine1TaskMode : ModeConfig.RoadMachine2TaskMode)
                == AGVCenterLib.Enum.RoadMachineTaskMode.AutoMoveOnly;
        }

        /// <summary>
        /// 转为调库模式
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        public void SwitchToMoveMode(int roadMachineIndex)
        {
            if (ModeConfig.SetMode(roadMachineIndex, RoadMachineTaskMode.AutoMoveOnly))
            {
                this.PublishStateInfos();
            }
            //if (roadMachineIndex == 1)
            //{
            //    if (!this.IsMoveStockMode(1)) { 
            //        ModeConfig.RoadMachine1PrevTaskMode = ModeConfig.RoadMachine1TaskMode;
            //    }
            //    ModeConfig.RoadMachine1TaskMode = RoadMachineTaskMode.AutoMoveOnly;
            //}
            //else if (roadMachineIndex == 2)
            //{
            //    if (!this.IsMoveStockMode(2))
            //    {
            //        ModeConfig.RoadMachine2PrevTaskMode = ModeConfig.RoadMachine2TaskMode;
            //        ModeConfig.RoadMachine2TaskMode = RoadMachineTaskMode.AutoMoveOnly;
            //    }
            //}
        }

        /// <summary>
        /// 停止移库模式
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        public void StopMoveMode(int roadMachineIndex)
        {
            //if (this.IsMoveStockMode(roadMachineIndex))
            //{
            //    if (roadMachineIndex == 1)
            //    {
            //        ModeConfig.RoadMachine1TaskMode = ModeConfig.RoadMachine1PrevTaskMode;
            //    }else if (roadMachineIndex == 2)
            //    {
            //        ModeConfig.RoadMachine2TaskMode = ModeConfig.RoadMachine2PrevTaskMode;
            //    }
            //}  
            if (ModeConfig.RecoverMode(roadMachineIndex))
            {
                this.PublishStateInfos();
            }
        }
        #endregion

        #region RabbitMQ 操作

          string rm_host = "192.168.2.116";
          int rm_port = 5672;
          string rm_username = "agv";
          string rm_pwd = "agv";

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
                operateChannel.QueueDeclare(queue: "agv_center_operate_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                rmOperateConsumer = new EventingBasicConsumer(operateChannel);
                rmOperateConsumer.Received += RmOperateConsumer_Received; ;

                operateChannel.BasicConsume(queue: "agv_center_operate_queue",
                                     noAck: true,
                                     consumer: rmOperateConsumer);
                #endregion

                // 状态发布
                #region
                stateInfoChannel = rmConnection.CreateModel();
                stateInfoChannel.ExchangeDeclare( exchange:"agv_center_state_info_exchange",type: ExchangeType.Fanout);
                #endregion
                // 发送状态消息
                this.PublishStateInfos();
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

                if (cmd.CmdType < 600)
                {
                    // 设置工作模式

                    if (ModeConfig.SetMode(cmd.RoadMachindeIndex, (RoadMachineTaskMode)cmd.CmdType))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.LoadRMWorkModeSetting();
                        });
                        // 发布状态消息
                        PublishStateInfos();
                    }
                }
                else
                {
                    // 其它任务
                    switch (cmd.CmdType)
                    {
                        case 700:
                            PublishStateInfos();
                            break;
                        default:
                            break;
                    }
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
        private void PublishStateInfos()
        {
            try
            {
                if (stateInfoChannel != null)
                {
                    LogUtil.Logger.Info("【发送状态消息】");

                    AgvCenterStateInfoCmd infoCmd = new AgvCenterStateInfoCmd()
                    {
                        RoadMachine1Mode = (int)ModeConfig.RoadMachine1TaskMode,
                        RoadMachine2Mode = (int)ModeConfig.RoadMachine2TaskMode
                    };

                    string message = JsonUtil.stringify(infoCmd);

                    stateInfoChannel.BasicPublish(exchange: "agv_center_state_info_exchange",
                                         routingKey: "agv_center_state_info_exchange",
                                         basicProperties: null,
                                         body: Encoding.UTF8.GetBytes(message));
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
    #endregion
}
}
