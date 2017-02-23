using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{
    public class OPCSetStockTask : OPCDataBase
    {
        public OPCSetStockTask()
            : base()
        {
          
            RestPositionFlag = 0x00;
        }

        public OPCSetStockTask(string OPCAddressKey,int RoadMachineIndex):base(OPCAddressKey)
        {
            
            RestPositionFlag = 0x00;
            this.RoadMachineIndex = RoadMachineIndex;
        }
       
        /// <summary>
        /// 任务类型,2
        /// </summary>
        public byte StockTaskType { get; set; }

        /// <summary>
        /// 库位，层，3，移库中是来源库位
        /// </summary>
        public int PositionFloor { get; set; }

        /// <summary>
        /// 库位，列，4，移库中是来源库位
        /// </summary>
        public int PositionColumn { get; set; }

        /// <summary>
        /// 库位，排，5，移库中是来源库位
        /// </summary>
        public int PositionRow { get; set; }

        /// <summary>
        /// 箱型，6
        /// </summary>
        public byte BoxType { get; set; }


        /// <summary>
        /// 重置库位标记，7
        /// </summary>
        public byte RestPositionFlag { get; set; }


        /// <summary>
        /// 托的剩余第几箱，8，从n...1
        /// </summary>
        public int TrayReverseNo { get; set; }

        /// <summary>
        /// 运单中托的个数，9
        /// </summary>
        public int TrayNum { get; set; }

        /// <summary>
        /// 运单项的个数，10
        /// </summary>
        public int DeliveryItemNum { get; set; }


        /// <summary>
        /// 条码，11
        /// </summary>
        public string Barcode { get; set; }



        /// <summary>
        /// 目标库位，层，12，移库中是目标库位
        /// </summary>
        public int ToPositionFloor { get; set; }

        /// <summary>
        /// 库位，列，13，移库中是目标库位
        /// </summary>
        public int ToPositionColumn { get; set; }

        /// <summary>
        /// 库位，排，14，移库中是目标库位
        /// </summary>
        public int ToPositionRow { get; set; }



        /// <summary>
        /// 巷道机编号，不写入OPC
        /// </summary>
        public int RoadMachineIndex { get; set; }




        
        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group, bool resetReadableFlag = true)
        {
            /// 写入条码信息
            int[] SyncItemServerHandles = new int[11];
            object[] SyncItemValues = new object[11];
            Array SyncItemServerErrors;

            // 库位，层 index 是2
            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);
            SyncItemServerHandles[2] = (int)this.ItemServerHandles.GetValue(3);
            SyncItemServerHandles[3] = (int)this.ItemServerHandles.GetValue(4);
            SyncItemServerHandles[4] = (int)this.ItemServerHandles.GetValue(5);
            SyncItemServerHandles[5] = (int)this.ItemServerHandles.GetValue(6);
            SyncItemServerHandles[6] = (int)this.ItemServerHandles.GetValue(7);
            SyncItemServerHandles[7] = (int)this.ItemServerHandles.GetValue(8);
            SyncItemServerHandles[8] = (int)this.ItemServerHandles.GetValue(9);
            SyncItemServerHandles[9] = (int)this.ItemServerHandles.GetValue(10);
            SyncItemServerHandles[10] = (int)this.ItemServerHandles.GetValue(11);
            // 目标库位
            SyncItemServerHandles[11] = (int)this.ItemServerHandles.GetValue(12);
            SyncItemServerHandles[12] = (int)this.ItemServerHandles.GetValue(13);
            SyncItemServerHandles[13] = (int)this.ItemServerHandles.GetValue(14);

            SyncItemValues[1] = this.StockTaskType;
            SyncItemValues[2] = this.PositionFloor;
            SyncItemValues[3] = this.PositionColumn;
            SyncItemValues[4] = this.PositionRow;
            SyncItemValues[5] = this.BoxType;
            SyncItemValues[6] = this.RestPositionFlag;
            SyncItemValues[7] = this.TrayReverseNo;
            SyncItemValues[8] = this.TrayNum;
            SyncItemValues[9] = this.DeliveryItemNum;
            SyncItemValues[10] = this.Barcode;
            // 目标库位
            SyncItemValues[11] = this.ToPositionFloor;
            SyncItemValues[12] = this.ToPositionColumn;
            SyncItemValues[13] = this.ToPositionRow;


            //   group.SyncWrite(10, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
            group.SyncWrite(13, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);

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
        public override void SetValue(int NumItems, Array clientHandles, Array ItemValues)
        {
            for (int i = NumItems; i >= 1; i--)
            {
                switch ((int)clientHandles.GetValue(i) )
                {
                    case 1:
                        this.OPCRwFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.StockTaskType = (byte)ItemValues.GetValue(i);
                        break;
                    case 3:
                        this.PositionFloor = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 4:
                        this.PositionColumn = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 5:
                        this.PositionRow = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 6:
                        this.BoxType = (byte)ItemValues.GetValue(i);
                        break;
                    case 7:
                        this.RestPositionFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 8:
                        this.TrayReverseNo = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 9:
                        this.TrayNum = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 10:
                        this.DeliveryItemNum = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 11:
                        this.Barcode = ParseBarcode(ItemValues.GetValue(i));
                        break;
                        // 目标库位
                    case 12:
                        this.ToPositionFloor = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 13:
                        this.ToPositionColumn = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 14:
                        this.ToPositionRow = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    default:
                        break;
                }
            }
        }


    }
}
