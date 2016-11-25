using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{
    public class OPCSetInStockTask : OPCDataBase
    {
        static OPCSetInStockTask()
        {
            OPCItemCount = 2;
        }
        public OPCSetInStockTask()
            : base()
        {
            State = InStockTaskState.Init;
        }
       
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
        public string InposiBarcode { get; set; }

        /// <summary>
        /// 状态，不写入OPC
        /// </summary>
        public InStockTaskState State { get; set; }

        /// <summary>
        /// DB id
        /// </summary>
        public int DbId { get; set; }



        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group)
        {
            /// 写入条码信息
            int[] SyncItemServerHandles = new int[8];
            object[] SyncItemValues = new object[8];
            Array SyncItemServerErrors;

            // 库位，层 index 是2
            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);
            SyncItemServerHandles[2] = (int)this.ItemServerHandles.GetValue(3);
            SyncItemServerHandles[3] = (int)this.ItemServerHandles.GetValue(4);
            SyncItemServerHandles[4] = (int)this.ItemServerHandles.GetValue(5);
            SyncItemServerHandles[5] = (int)this.ItemServerHandles.GetValue(6);
            SyncItemServerHandles[6] = (int)this.ItemServerHandles.GetValue(7);
            SyncItemServerHandles[7] = (int)this.ItemServerHandles.GetValue(8);

            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
            if (SyncItemServerErrors != null && ((int)SyncItemServerErrors.GetValue(1) == 0))
            {
                if (this.SyncSetReadableFlag(group))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="ClientHandles"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        public override void SetValue(int NumItems, Array ClientHandles, Array ItemValues)
        {
            for (int i = 1; i <=NumItems; i++)
            {
                switch ((int)ClientHandles.GetValue(i))
                {
                    case 1:
                        this.OPCRwFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.PositionFloor = (byte)ItemValues.GetValue(i);
                        break;
                    case 3:
                        this.PositionColumn = (byte)ItemValues.GetValue(i);
                        break;
                    case 4:
                        this.PositionRow = (byte)ItemValues.GetValue(i);
                        break;
                    case 5:
                        this.BoxType = (byte)ItemValues.GetValue(i);
                        break;
                    case 6:
                        this.AgvPassFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 7:
                        this.RestPositionFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 8:
                        this.InposiBarcode = (string)ItemValues.GetValue(i);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
