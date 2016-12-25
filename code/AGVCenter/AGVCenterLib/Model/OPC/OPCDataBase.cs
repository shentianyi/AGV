using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.LogUtil;
using OPCAutomation;

namespace AGVCenterLib.Model.OPC
{

    public class OPCDataBase
    {
        #region 读写标记常量
        public static byte NONE_ABLE_FLAG = 0x00;
        public static byte READ_ABLE_FLAG = 0x01;
        public static byte WRITE_ABLE_FLAG = 0x02;
        #endregion

        #region 变量
        public int OPCItemCount = 2;
        public string OPCAddressKey;
        public Array OPCItemIDs;
        public Array ClientHandles;
        public Array ItemServerHandles;
        public Array AddItemServerErrors;
        #endregion



        public OPCDataBase()
        {
            OPCAddressKey = this.GetType().Name;
            this.Init();
        }

        public OPCDataBase(string OPCAddressKey)
        {
            this.OPCAddressKey = OPCAddressKey;
            this.Init();
        }

        private void Init()
        {
            OPCItemCount = OPCAddressMap.GroupNameAddress[OPCAddressKey].Count;
            OPCItemIDs = new string[OPCItemCount + 1];
            ClientHandles = new int[OPCItemCount + 1];
        }
        #region 读写标记事件
        /// <summary>
        /// 读写标记改变事件委托
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void RwFlagChangedEventHandler(OPCDataBase b, byte toFlag);

        /// <summary>
        /// 读写标记改变事件
        /// </summary>
        public event RwFlagChangedEventHandler RwFlagChangedEvent;


        //   public RwFlagChangedEventHandler RwFlagChanged;
        #endregion


        #region 读写标记
        private byte? opcRwFlagWas;
        public byte? OPCRwFlagWas { get; set; }
        private byte? opcRwFlag;
        /// <summary>
        /// 读写标记 1
        /// </summary>
        public byte? OPCRwFlag
        {
            get
            {
                return opcRwFlag;
            }
            set
            {
                this.opcRwFlagWas = opcRwFlag;
                opcRwFlag = value;
                if (this.opcRwFlagWas != this.opcRwFlag && this.RwFlagChangedEvent != null)
                {
                    this.RwFlagChangedEvent(this, value.Value);
                }
            }
        }

        /// <summary>
        /// 是否可读
        /// </summary>
        public bool CanRead
        {
            get
            {
                return this.opcRwFlag == READ_ABLE_FLAG;
            }
        }

        /// <summary>
        /// 是否可写
        /// </summary>
        public bool CanWrite
        {
            get
            {
                return this.opcRwFlag == WRITE_ABLE_FLAG;
            }
        }
        #endregion

        #region 添加item到Group
        /// <summary>
        /// 添加item到Group
        /// </summary>
        /// <param name="group"></param>
        public void AddItemToGroup(OPCGroup group)
        {
            int i = 1;
            // 从1开始
            foreach (var kv in OPCAddressMap.GroupNameAddress[this.OPCAddressKey])
            {
                this.OPCItemIDs.SetValue(  kv.Value, i);
                this.ClientHandles.SetValue(i, i);
                i++;
            }

            group.OPCItems.AddItems(
                OPCItemCount,
                 ref this.OPCItemIDs,
              ref this.ClientHandles,
                 out this.ItemServerHandles,
                 out this.AddItemServerErrors);

        }
        #endregion

        #region 写入值
        /// <summary>
        /// 写入值
        /// </summary>
        /// <param name="group"></param>
        public virtual bool SyncWrite(OPCGroup group, bool resetReadableFlag = true)
        {
            return false;
        }
        #endregion

        #region 设置可读
        /// <summary>
        /// 设置可读
        /// </summary>
        /// <param name="group"></param>
        public bool SyncSetReadableFlag(OPCGroup group)
        {
            return SyncSetRWFlag(group, READ_ABLE_FLAG);
        }
        #endregion


        #region 设置可写
        /// <summary>
        /// 设置可写
        /// </summary>
        /// <param name="group"></param>
        public bool SyncSetWriteableFlag(OPCGroup group)
        {
            return SyncSetRWFlag(group, WRITE_ABLE_FLAG);
        }
        #endregion

        #region
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="NumItems"></param>
        /// <param name="ClientHandles"></param>
        /// <param name="ItemValues"></param>
        public virtual void SetValue(int NumItems, Array ClientHandles, Array ItemValues)
        {

        }
        #endregion




        #region 设置可读、可写标记
        /// <summary>
        /// 设置可读
        /// </summary>
        /// <param name="group"></param>
        protected bool SyncSetRWFlag(OPCGroup group, byte flag)
        {
            /// 写入flag
            int[] SyncItemServerHandles = new int[2];
            object[] SyncItemValues = new object[2];
            Array SyncItemServerErrors;

            // FLAG index 是1
            SyncItemServerHandles[1] = (int)this.ItemServerHandles.GetValue(1);
            SyncItemValues[1] = flag;
            group.SyncWrite(1, SyncItemServerHandles, SyncItemValues, out SyncItemServerErrors);
            if (SyncItemServerErrors != null && ((int)SyncItemServerErrors.GetValue(1) == 0))
            {
                return true;
            }
            return false;
        }
        #endregion


        public string ParseBarcode(object o)
        {

            if (o == null)
            {
                return string.Empty;
            }
            else
            {
                LogUtil.Logger.InfoFormat("【扫描条码内容】{0}", o.ToString());
                List<string> s = o.ToString().Trim('\n').Trim('\r').Split('\r').ToList();
                List<string> ss = new List<string>();
                foreach(var sss in s)
                {
                    if (!ss.Contains(sss.Trim('\n'))){
                        ss.Add(sss.Trim('\n'));
                    }
                }
                return ss.FirstOrDefault();
            }
            //return o == null ? string.Empty : o.ToString().Split('\r').Trim('\n').Trim('\r');
        }

        public virtual string GetSimpleOpcKey(int index, Array clientHandles)
        {
            return this.OPCItemIDs.GetValue(int.Parse(clientHandles.GetValue(index).ToString())).ToString().Replace(OPCAddressMap.INOUT_AddrPrefix, "");
        }
    }
}
