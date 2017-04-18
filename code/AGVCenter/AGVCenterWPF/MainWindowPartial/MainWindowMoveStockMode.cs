using AGVCenterLib.Config;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Command;
using AGVCenterLib.Service;
using AGVCenterWPF.Config;
using Brilliantech.Framwork.Utils.JsonUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using Quartz;
using Quartz.Impl;
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
            try
            {
                LogUtil.Logger.Info("【开启 - 移库模式】");

                if (ModeConfig.SetMode(roadMachineIndex, RoadMachineTaskMode.AutoMoveOnly))
                {
                    this.PublishStateInfos();
                }

                this.Dispatcher.Invoke(() =>
                {
                    this.LoadRMWorkModeSetting();
                });
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);

            }
        }

        /// <summary>
        /// 停止移库模式
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        public void StopMoveMode(int roadMachineIndex)
        {
            try
            {
                LogUtil.Logger.Info("【停止 - 移库模式】");

                if (ModeConfig.RecoverMode(roadMachineIndex))
                {
                    this.PublishStateInfos();
                }
                this.Dispatcher.Invoke(() =>
                {
                    this.LoadRMWorkModeSetting();
                });
            }catch(Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);

            }
        }
        #endregion

        #region RabbitMQ 操作

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
                stateInfoChannel.ExchangeDeclare(exchange: "agv_center_state_info_exchange", type: ExchangeType.Fanout);
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
                        // 发布状态消息
                        PublishStateInfos();
                        this.Dispatcher.Invoke(() =>
                        {
                            this.LoadRMWorkModeSetting();
                        });
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
                        case 600:
                            this.LoadMoveTaskScheduleJob(DateTime.Now);
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


        #region 定时任务
        private IScheduler taskScheduler;
        /// <summary>
        /// 打开自动移库定时任务
        /// </summary>
        public void InitAndStartQuartzTaskSchedule()
        {
            try
            {
                LogUtil.Logger.Info("【开启自动移库定时任务组件】");

                taskScheduler = new StdSchedulerFactory().GetScheduler();
                this.LoadMoveTaskScheduleJob(DateTime.Now);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);

            }
        }

        /// <summary>
        /// 停止自动移库定时任务
        /// </summary>
        private void ShutDownQuartzTaskSchedule()
        {
            try
            {
                LogUtil.Logger.Info("【关闭自动移库定时任务组件】");

                if (taskScheduler != null)
                {
                    taskScheduler.Clear();
                    taskScheduler.Shutdown();
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
      
        
        /// <summary>
        /// 加载任务
        /// </summary>
        public void LoadMoveTaskScheduleJob(DateTime fromDateTime)
        {
            try
            {
                taskScheduler.Clear();

                LogUtil.Logger.Info("【加载自动移库定时任务】");
                var task = new MoveTaskScheduleService(OPCConfig.DbString).GetFirstTaskSchedule(fromDateTime);
                if (task != null)
                {
                    //if (task.StartTime <= DateTime.Now)
                    //{
                    //    this.SwitchToMoveMode(1);
                    //    this.SwitchToMoveMode(2);
                    //}
                    // 开始任务
                    IJobDetail startJob = JobBuilder.Create<MoveTaskScheduleStartJob>()
                        .WithIdentity("MoveTaskScheduleStartJob1", "MoveTaskScheduleStartGroup1")
                        .Build();
                    startJob.JobDataMap.Add("center", this);
                    var startTime = DateTimeOffset.Parse(DateTime.SpecifyKind(task.StartTime, DateTimeKind.Utc).ToString());
                    ITrigger startTrigger = TriggerBuilder.Create()
                        .WithIdentity("MoveTaskScheduleStartTrigger1", "MoveTaskScheduleStartGroup1")
                        .StartAt(startTime).Build();
                    // 结束任务
                    IJobDetail endJob = JobBuilder.Create<MoveTaskScheduleStopJob>()
                   .WithIdentity("MoveTaskScheduleStopJob1", "MoveTaskScheduleStopGroup1")
                   .Build();
                    endJob.JobDataMap.Add("center", this);

                    var stopTime = DateTimeOffset.Parse(DateTime.SpecifyKind(task.EndTime, DateTimeKind.Utc).ToString());
                    ITrigger stopTrigger = TriggerBuilder.Create()
                        .WithIdentity("MoveTaskScheduleStopTrigger1", "MoveTaskScheduleStopGroup1")
                        .StartAt(stopTime).Build();


                    this.taskScheduler.ScheduleJob(startJob, startTrigger);
                    this.taskScheduler.ScheduleJob(endJob, stopTrigger);
                }
                this.taskScheduler.Start();
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
        #endregion
    }


    /// <summary>
    /// 开始调库任务
    /// </summary>
    public class MoveTaskScheduleStartJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                LogUtil.Logger.Info("【触发开启的定时任务】");
                var center = context.JobDetail.JobDataMap["center"] as MainWindow;
                if (BaseConfig.RoadMachine1AutoMoveEnabled)
                {
                    center.SwitchToMoveMode(1);
                }

                if (BaseConfig.RoadMachine2AutoMoveEnabled)
                {
                    center.SwitchToMoveMode(2);
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
    }
    /// <summary>
    /// 停止调库任务
    /// </summary>
    public class MoveTaskScheduleStopJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                LogUtil.Logger.Info("【触发停止的定时任务】");
                var center = context.JobDetail.JobDataMap["center"] as MainWindow;
                center.StopMoveMode(1);
                center.StopMoveMode(2);
                /// load next job
                center.LoadMoveTaskScheduleJob(context.Trigger.StartTimeUtc.DateTime);
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }
    }

}
