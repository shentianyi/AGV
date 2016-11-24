using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.OPC
{
    public class OPCSetInStockTask : Base
    {
        static OPCSetInStockTask()
        {
            OPCItemCount = 2;
        }
        public OPCSetInStockTask()
            : base()
        {

        }

        /// <summary>
        /// 条码
        /// </summary>
        public string InposiBarcode { get; set; }
        
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
                       // this.ScanGetInposiBarcode = (string)ItemValues.GetValue(i);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
