using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{ 

    /// <summary>
    /// 扫码入库检验
    /// </summary>
    public class OPCCheckInStockBarcode : OPCDataBase
    {
        public OPCCheckInStockBarcode()
            : base()
        {
        }

        public string scanedBarcodeWas;
        public string ScanedBarcodeWas {
            get { return scanedBarcodeWas; }
            set
            {
                scanedBarcodeWas = value;
            }
        }
        /// <summary>
        /// 条码 2
        /// </summary>
        private string scanedBarcode;
        public string ScanedBarcode { get { return scanedBarcode; } set {
                scanedBarcodeWas = scanedBarcode;
                scanedBarcode = value;
            } }


        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public override bool SyncWrite(OPCGroup group)
        {
            /// 写入条码信息
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            // 条码 index 是2
            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(2);
            SyncItemValues[1] = this.ScanedBarcode;
            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
            if (SyncItemServerErrors != null && ((int)SyncItemServerErrors.GetValue(1) == 0))
            {
                if (this.SyncSetReadableFlag(group))
                {
                    return true;
                }
            }
            else
            {
                this.ScanedBarcode = this.ScanedBarcodeWas;
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
                        this.ScanedBarcode = ParseBarcode(ItemValues.GetValue(i));
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
