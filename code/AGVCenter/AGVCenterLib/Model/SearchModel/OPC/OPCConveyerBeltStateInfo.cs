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
    public class OPCConveyerBeltStateInfo : OPCDataBase
    {
        public OPCConveyerBeltStateInfo()
            : base()
        {
        }
         
        /// <summary>
        /// 大箱传送带是否空箱？1-没有箱子，0-有箱子，1
        /// </summary>
        public int BigBoxBeltEmptyState { get; set; }


        /// <summary>
        /// 小箱传送带是否空箱？1-没有箱子，0-有箱子，2
        /// </summary>
        public int SmallBoxBeltEmptyState { get; set; }

    

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
                        this.BigBoxBeltEmptyState = int.Parse( ItemValues.GetValue(i).ToString());
                        break;
                    case 2:
                        this.SmallBoxBeltEmptyState = int.Parse(ItemValues.GetValue(i).ToString());
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
