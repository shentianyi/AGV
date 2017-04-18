using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.Command
{
  public  class AgvCenterCmd
    {
        /// <summary>
        /// 命令类型：100- 改为 出库优先模式；
        ///           200- 改为 入库优先模式；
        ///           300- 改为只出库；
        ///           400- 改为 只入库
        ///           500- 改为 调库模式
        ///           600- 重新加载定时任务
        ///           700- 新的调库 client连接
        ///           701- 新的设备状态监控 client连接
        /// </summary>
        public int CmdType { get; set; }
        public int RoadMachindeIndex { get; set; }
    }
}
