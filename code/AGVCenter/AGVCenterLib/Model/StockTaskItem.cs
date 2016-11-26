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
        }
        /// <summary>
        /// 任务类型
        /// </summary>
        public StockTaskType StockTaskType { get; set; }
        
        /// <summary>
        /// 库位，层，2
        /// </summary>
        public byte PositionFloor { get; set; }

        /// <summary>
        /// 库位，列，3
        /// </summary>
        public byte PositionColumn { get; set; }

        /// <summary>
        /// 库位，排，4
        /// </summary>
        public byte PositionRow { get; set; }

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
        /// 条码，8
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 状态，不写入OPC
        /// </summary>
        public StockTaskState State { get; set; }

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
