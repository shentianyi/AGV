using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;

namespace AGVCenterLib.Model
{
    public class StockTaskItem
    {
        public StockTaskItem()
        {
            this.State = StockTaskState.Init;
            this.IsInProcessing = false;
        }
        #region 状态改变事件
        /// <summary>
        /// 状态改变事件委托
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void TaskStateChangeEventHandler(StockTaskItem taskItem, StockTaskState toState);
        /// <summary>
        /// 状态改变事件
        /// </summary>
        public event TaskStateChangeEventHandler TaskStateChangeEvent;
         
        #endregion

        /// <summary>
        /// 任务类型
        /// </summary>
        public StockTaskType StockTaskType { get; set; }
        
        /// <summary>
        /// 巷道机序号，从1开始，目前使用1号或2号巷道机，
        /// 和库存中的库位AreaIndex对应，相当于1或2的分区
        /// </summary>
        public int RoadMachineIndex { get; set; }

        /// <summary>
        /// 库位编号
        /// </summary>
        public string PositionNr { get; set; }

        /// <summary>
        /// 库位，层，2
        /// </summary>
        public int PositionFloor { get; set; }

        /// <summary>
        /// 库位，列，3
        /// </summary>
        public int PositionColumn { get; set; }

        /// <summary>
        /// 库位，排，4
        /// </summary>
        public int PositionRow { get; set; }

        /// <summary>
        /// 箱型，5
        /// </summary>
        public byte BoxType { get; set; }

        /// <summary>
        /// AGV 放行标记，6
        /// </summary>
        public byte AgvPassFlag { get; set; }

        /// <summary>
        /// 重置库位标记，7
        /// </summary>
        public byte RestPositionFlag { get; set; }


        /// <summary>
        /// 托的剩余第几箱，从n...1
        /// </summary>
        public int TrayReverseNo { get; set; }

        /// <summary>
        /// 运单中托的个数
        /// </summary>
        public int TrayNum { get; set; }

        /// <summary>
        /// 运单项的个数
        /// </summary>
        public int DeliveryItemNum { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 状态，不写入OPC
        /// </summary>
        private StockTaskState stateWas;
        public StockTaskState StateWas
        {
            get { return stateWas; }
            private set { stateWas = value; }
        }
        private StockTaskState state;
        public StockTaskState State
        {
            get { return state; }
            set
            {
                stateWas = state;
                state = value;
                if (stateWas != state)
                {
                    this.TaskStateChangeEvent(this, value);
                }
            }
        }

        public bool IsInProcessing
        {
            get;
            set;
        }

        /// <summary>
        /// DB id
        /// </summary>
        public int DbId { get; set; }
         
        
        public string ToDisplay()
        {
            return string.Format("【任务类型：{0}】条码：{1},库位：{2}-{3}-{4},箱型：{5},AGV放行标记:{6},Rest标记{7},状态：{8},DbId:{9}",
                this.StockTaskType,
                this.Barcode,
                this.PositionFloor, this.PositionColumn, this.PositionRow,
                this.BoxType,
                this.AgvPassFlag,
                this.RestPositionFlag,
                this.State,
                this.DbId);
        }
    }
}
