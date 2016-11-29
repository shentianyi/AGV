using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{
    /// <summary>
    /// 巷道机任务反馈，没有读写标记位
    /// </summary>
    public class OPCRoadMachineTaskFeed : OPCDataBase
    {
        public OPCRoadMachineTaskFeed()
          : base()
        {

        }

        public OPCRoadMachineTaskFeed(string OPCAddressKey,int RoadMachineIndex)
            : base(OPCAddressKey)
        {
            this.RoadMachineIndex = RoadMachineIndex;
        }

        #region 反馈状态改变事件
        /// <summary>
        /// 反馈动作改变事件委托
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void ActionFlagChangeEventHandler(OPCRoadMachineTaskFeed taskFeed, byte toActionFlag);
        /// <summary>
        /// 反馈动作改变事件
        /// </summary>
        public event ActionFlagChangeEventHandler ActionFlagChangeEvent;

        #endregion

        /// <summary>
        /// 当前位置，层，1
        /// </summary>
        public byte CurrentPositionFloor { get; set; }

        /// <summary>
        /// 当前位置，列，2
        /// </summary>
        public byte CurrentPositionColumn { get; set; }

        /// <summary>
        /// 当前位置，排，3
        /// </summary>
        public byte CurrentPositionRow { get; set; }

        /// <summary>
        /// 目标出库位置，层，4
        /// </summary>
        public byte TargetOutPositionFloor { get; set; }

        /// <summary>
        /// 目标出库位置，列，5
        /// </summary>
        public byte TargetOutPositionColumn { get; set; }

        /// <summary>
        /// 目标出库位置，排，6
        /// </summary>
        public byte TargetOutPositionRow { get; set; }

        /// <summary>
        /// 目标入库位置，层，7
        /// </summary>
        public byte TargetInPositionFloor { get; set; }

        /// <summary>
        /// 目标入库位置，列，8
        /// </summary>
        public byte TargetInPositionColumn { get; set; }
        
        /// <summary>
        /// 目标入库位置，排，9
        /// </summary>
        public byte TargetInPositionRow { get; set; }

        /// <summary>
        /// 当前状态，10
        /// </summary>
        public int CurrentState { get; set; }

        /// <summary>
        /// 故障信息，11
        /// </summary>
        public int Error { get; set; }

        /// <summary>
        /// 当前任务条码信息，12
        /// </summary>
        public string CurrentBarcode { get; set; }

        /// <summary>
        /// 当前库位条码信息，13
        /// </summary>
        public string PositionBarcode { get; set; }

        /// <summary>
        /// 动作结果状态,14
        /// </summary>
        private byte actionFlagWas;
        public byte ActionFlagWas
        {
            get { return this.actionFlagWas; }
           private set { this.actionFlagWas = value; }
        }
        private byte actionFlag;
        public byte ActionFlag
        {
            get
            {
                return actionFlag;
            }
            set
            {
                this.actionFlagWas = this.actionFlag;
                this.actionFlag = value;
                if (this.actionFlag != actionFlagWas)
                {
                    this.ActionFlagChangeEvent(this, value);
                }
            }
        }

        public int RoadMachineIndex { get; set; }


        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group)
        {

            int[] SyncItemServerHandles = new int[11];
            object[] SyncItemValues = new object[11];
            Array SyncItemServerErrors;


            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(1);
            SyncItemServerHandles[2] = (int)this.ItemServerHandles.GetValue(2);
            SyncItemServerHandles[3] = (int)this.ItemServerHandles.GetValue(3);
            SyncItemServerHandles[4] = (int)this.ItemServerHandles.GetValue(4);
            SyncItemServerHandles[5] = (int)this.ItemServerHandles.GetValue(5);
            SyncItemServerHandles[6] = (int)this.ItemServerHandles.GetValue(6);
            SyncItemServerHandles[7] = (int)this.ItemServerHandles.GetValue(7);
            SyncItemServerHandles[8] = (int)this.ItemServerHandles.GetValue(8);
            SyncItemServerHandles[9] = (int)this.ItemServerHandles.GetValue(9);
            SyncItemServerHandles[10] = (int)this.ItemServerHandles.GetValue(10);
            SyncItemServerHandles[11] = (int)this.ItemServerHandles.GetValue(11);
            SyncItemServerHandles[12] = (int)this.ItemServerHandles.GetValue(12);
            SyncItemServerHandles[13] = (int)this.ItemServerHandles.GetValue(13);
            SyncItemServerHandles[14] = (int)this.ItemServerHandles.GetValue(14);

            SyncItemValues[1] = this.CurrentPositionFloor;
            SyncItemValues[2] = this.CurrentPositionColumn;
            SyncItemValues[3] = this.CurrentPositionRow;
            SyncItemValues[4] = this.TargetOutPositionFloor;
            SyncItemValues[5] = this.TargetOutPositionColumn;
            SyncItemValues[6] = this.TargetOutPositionRow;
            SyncItemValues[7] = this.TargetInPositionFloor;
            SyncItemValues[8] = this.TargetInPositionColumn;
            SyncItemValues[9] = this.TargetInPositionRow;
            SyncItemValues[10] = this.CurrentState;
            SyncItemValues[11] = this.Error;
            SyncItemValues[12] = this.CurrentBarcode;
            SyncItemValues[13] = this.PositionBarcode;
            SyncItemValues[14] = this.ActionFlag;

            group.SyncWrite(14, SyncItemServerHandles,
                SyncItemValues, out SyncItemServerErrors);
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
            for (int i = NumItems; i >= 1; i--)
            {
                switch ((int)ClientHandles.GetValue(i))
                {
                    case 1:
                        this.CurrentPositionFloor = (byte)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.CurrentPositionColumn = (byte)ItemValues.GetValue(i);
                        break;
                    case 3:
                        this.CurrentPositionRow = (byte)ItemValues.GetValue(i);
                        break;
                    case 4:
                        this.TargetOutPositionFloor = (byte)ItemValues.GetValue(i);
                        break;
                    case 5:
                        this.TargetOutPositionColumn = (byte)ItemValues.GetValue(i);
                        break;
                    case 6:
                        this.TargetOutPositionRow = (byte)ItemValues.GetValue(i);
                        break;
                    case 7:
                        this.TargetInPositionFloor = (byte)ItemValues.GetValue(i);
                        break;
                    case 8:
                        this.TargetInPositionColumn = (byte) ItemValues.GetValue(i);
                        break;
                    case 9:
                        this.TargetInPositionRow = (byte)ItemValues.GetValue(i);
                        break;
                    case 10:
                        this.CurrentState = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 11:
                        this.Error= int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    case 12:
                        this.CurrentBarcode = ParseBarcode(ItemValues.GetValue(i));
                        break;
                    case 13:
                        this.PositionBarcode = ParseBarcode(ItemValues.GetValue(i));
                        break;
                    case 14:
                        this.ActionFlag = (byte)ItemValues.GetValue(i);
                        break;
                    default:
                        break;
                }
            }
        }
        
    }
}
