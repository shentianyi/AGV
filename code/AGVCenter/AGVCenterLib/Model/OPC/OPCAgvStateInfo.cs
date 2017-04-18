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
    public class OPCAgvStateInfo : OPCDataBase
    {
        public OPCAgvStateInfo()
            : base()
        {
        }
        public OPCAgvStateInfo(string OPCAddressKey, int id)
            : base(OPCAddressKey)
        {
            this.Id = id;
        }
        public int Id { get; set; }
        /// <summary>
        /// 状态，2-停止：读到卡主动停止，1-运行，4-锁定：阻塞，被动停止；1
        /// </summary>
        public string State { get; set; }


        /// <summary>
        /// 路线，2
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// 位置，3
        /// </summary>
        public string Position { get; set; }


        /// <summary>
        /// 电压，4
        /// </summary>
        public string Voltage { get; set; }
         

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
                        this.State = ItemValues.GetValue(i).ToString();
                        break;
                    case 2:
                        this.Route =  ItemValues.GetValue(i).ToString();
                        break;
                    case 3:
                        this.Position = ItemValues.GetValue(i).ToString();
                        break;
                    case 4:
                        this.Voltage = ItemValues.GetValue(i).ToString();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
