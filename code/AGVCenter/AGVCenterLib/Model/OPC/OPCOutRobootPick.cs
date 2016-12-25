using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{
    /// <summary>
    /// 
    /// </summary>
    public class OPCOutRobootPick : OPCDataBase
    {
        public OPCOutRobootPick()
            : base()
        {
        }
        
        /// <summary>
        /// 箱型，2
        /// </summary>
        public byte BoxType { get; set; }


        /// <summary>
        /// 整托托盘数量，2
        /// </summary>
        public int TrayNum { get; set; }




        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group,bool resetReadableFlag=true)
        {
            /// 写入箱型信息
            int[] SyncItemServerHandles = new int[3];
            object[] SyncItemValues = new object[3];
            Array SyncItemServerErrors;

            // 箱型 index 是2
            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);
            SyncItemServerHandles[2] = (int)this.ItemServerHandles.GetValue(3);
            SyncItemValues[1] = this.BoxType;
            SyncItemValues[2] = this.TrayNum;

            group.SyncWrite(2, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);

            if (SyncItemServerErrors != null && ((int)SyncItemServerErrors.GetValue(1) == 0))
            {
                if (resetReadableFlag)
                {
                    if (this.SyncSetReadableFlag(group))
                    {
                        return true;
                    }
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
                switch ((int)clientHandles.GetValue(i))
                {
                    case 1:
                        this.OPCRwFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.BoxType = (byte)ItemValues.GetValue(i);
                        break;
                    case 3:
                        this.TrayNum = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
