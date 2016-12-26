using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{
    /// <summary>
    /// AGV 入库放行
    /// </summary>
    public class OPCDataReset : OPCDataBase
    {
        public OPCDataReset()
            : base()
        {
        }
        // 巷道机1入库平台是否有箱子 1
        public bool Xdj1InPaltformIsBuff { get; set; }
        // 巷道机2入库平台是否有箱子 2
        public bool Xdj2InPaltformIsBuff { get; set; }

        // 巷道机1出库平台是否有大箱子 3
        public bool Xdj1OutPaltformIsBuffBig { get; set; }
        // 巷道机1出库平台是否有小箱子 4
        public bool Xdj1OutPaltformIsBuffSmall { get; set; }

        // 巷道机2出库平台是否有大箱子 5
        public bool Xdj2OutPaltformIsBuffBig { get; set; }
        // 巷道机2出库平台是否有小箱子 6
        public bool Xdj2OutPaltformIsBuffSmall { get; set; }


        // 出库机械手需抓取托总量 7
        public int OutrootPickTrayCount { get; set; }

        // 出库机械手已抓取量  8
        public int OutrootPickCount { get; set; }

        // 巷道机1当前状态 9
        public int Xdj1CurentState { get; set; }
        // 巷道机1当前错误 10
        public int Xdj1CurentError { get; set; }

        // 巷道机1当前层 11
        public int Xdj1CurentFloor { get; set; }
        // 巷道机1当前列 12
        public int Xdj1CurentColumn  { get; set; }
        // 巷道机1当前出入库任务标记 13
        public int Xdj1CurentPickupOrOutState { get; set; }



        // 巷道机2当前状态 14
        public int Xdj2CurentState { get; set; }
        // 巷道机2当前错误 15 
        public int Xdj2CurentError { get; set; }

        // 巷道机2当前层 16
        public int Xdj2CurentFloor { get; set; }
        // 巷道机2当前列 17
        public int Xdj2CurentColumn { get; set; }
        // 巷道机2当前出入库任务标记 18
        public int Xdj2CurentPickupOrOutState { get; set; }

        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group, bool resetReadableFlag = true)
        {
            return false;
        }
        #endregion


        public void RestXdj1InPaltformIsBuff(OPCGroup group)
        {
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            SyncItemValues[1] = false;

            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(1);

            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
        }


        public void RestXdj2InPaltformIsBuff(OPCGroup group)
        {
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            SyncItemValues[1] = false;

            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);

            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
        }



        public void IncrOutrootPickCount(OPCGroup group)
        {
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(8);

            SyncItemValues[1] = this.OutrootPickCount + 1;
            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
        }


        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="ClientHandles"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        public override void SetValue(int NumItems, Array clientHandles, Array ItemValues)
        {
            for (int i = NumItems; i >= 1; i--)
            {
                switch ((int)clientHandles.GetValue(i))
                {
                    case 1:
                        this.Xdj1InPaltformIsBuff = (bool)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.Xdj2InPaltformIsBuff = (bool)ItemValues.GetValue(i);
                        break;
                    case 3:
                        this.Xdj1OutPaltformIsBuffBig = (bool)ItemValues.GetValue(i);
                        break;
                    case 4:
                        this.Xdj1OutPaltformIsBuffSmall = (bool)ItemValues.GetValue(i);
                        break;
                    case 5:
                        this.Xdj2OutPaltformIsBuffBig = (bool)ItemValues.GetValue(i);
                        break;
                    case 6:
                        this.Xdj2OutPaltformIsBuffSmall = (bool)ItemValues.GetValue(i);
                        break;
                    case 7:
                        this.OutrootPickTrayCount = (int)ItemValues.GetValue(i);
                        break;
                    case 8:
                        this.OutrootPickCount = (int)ItemValues.GetValue(i);
                        break;

                    case 9:
                        this.Xdj1CurentState = (int)ItemValues.GetValue(i);
                        break;
                    case 10:
                        this.Xdj1CurentError = (int)ItemValues.GetValue(i);
                        break;
                    case 11:
                        this.Xdj1CurentFloor = (int)ItemValues.GetValue(i);
                        break;
                    case 12:
                        this.Xdj1CurentColumn = (int)ItemValues.GetValue(i);
                        break;

                    case 13:
                        this.Xdj2CurentState = (int)ItemValues.GetValue(i);
                        break;
                    case 14:
                        this.Xdj2CurentError = (int)ItemValues.GetValue(i);
                        break;
                    case 15:
                        this.Xdj2CurentFloor = (int)ItemValues.GetValue(i);
                        break;
                    case 16:
                        this.Xdj2CurentColumn = (int)ItemValues.GetValue(i);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
