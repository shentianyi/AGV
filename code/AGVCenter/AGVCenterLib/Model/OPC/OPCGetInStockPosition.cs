using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{
    public class OPCGetInStockPosition : OPCDataBase
    {
        static OPCGetInStockPosition()
        {
            
        }
        public OPCGetInStockPosition()
            : base()
        {
            OPCItemCount = 2;
        }

        public string scanGetInposiBarcodeWas;
        public string ScanGetInposiBarcodeWas {
            get { return scanGetInposiBarcodeWas; }
            set
            {
                scanGetInposiBarcodeWas = value;
            }
        }
        /// <summary>
        /// 条码 2
        /// </summary>
        private string scanGetInposiBarcode;
        public string ScanGetInposiBarcode { get { return scanGetInposiBarcode; } set {
                scanGetInposiBarcodeWas = scanGetInposiBarcode;
                scanGetInposiBarcode = value;
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
            // OPCItem AnOpcItem = group.OPCItems.GetOPCItem((int)this.ItemServerHandles.GetValue(2));
            SyncItemValues[1] = this.ScanGetInposiBarcode;
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
                this.ScanGetInposiBarcode = this.ScanGetInposiBarcodeWas;
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
            for (int i = 1; i <= NumItems; i++)
            {
                switch ((int)ClientHandles.GetValue(i))
                {
                    case 1:
                        this.OPCRwFlag = (byte)ItemValues.GetValue(i);
                        break;
                    case 2:
                        this.ScanGetInposiBarcode = (string)ItemValues.GetValue(i);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
