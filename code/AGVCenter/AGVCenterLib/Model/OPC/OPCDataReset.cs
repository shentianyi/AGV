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
         
        public int OutrootPickCount { get; set; }
        public bool Xdj1PaltformIsBuff { get; set; }
        public bool Xdj2PaltformIsBuff { get; set; }

        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group, bool resetReadableFlag = true)
        {
            ///// 写入条码信息
            //int[] SyncItemServerHandles = new int[2];
            //object[] SyncItemValues = new object[2];
            //Array SyncItemServerErrors;

            //// 条码 index 是2
            //SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);
            //SyncItemValues[1] = this.OutrootPickCount;
            //group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
            //if (SyncItemServerErrors != null && ((int)SyncItemServerErrors.GetValue(1) == 0))
            //{
            //    if (this.SyncSetReadableFlag(group))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
        #endregion

        public void IncrOutrootPickCount(OPCGroup group)
        {
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;
             
            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(1);
            
            SyncItemValues[1] = this.OutrootPickCount+1;
            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
        }


        public void RestXdj1PaltformIsBuff(OPCGroup group)
        {
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            SyncItemValues[1] = false;

            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);

            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
        }


        public void RestXdj2PaltformIsBuff(OPCGroup group)
        {
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            SyncItemValues[1] = false;

            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(3);

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
                        this.OutrootPickCount = (int)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.Xdj1PaltformIsBuff = (bool)ItemValues.GetValue(i);
                        break;
                    case 3:
                        this.Xdj2PaltformIsBuff = (bool)ItemValues.GetValue(i);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
