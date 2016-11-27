using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterWPF.Properties;

namespace AGVCenterWPF
{
   public class OPCConfig
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string DbString = Settings.Default.dbString;
        /// <summary>
        /// OPC 服务器列表
        /// </summary>
        public static List<string> OPCServers = new List<string>() { "OPC.SimaticNET.DP.1", "OPC.SimaticNET.1", "OPC.SimaticNET.PD.1" };

        /// <summary>
        /// OPC 服务
        /// </summary>
        public static string OPCServer = "OPC.SimaticNET.1";

        public static string OPCNodeName = "192.168.1.105";

        #region OPC组
        /// <summary>
        /// OPCCheckInStockBarcodeOPCGroup , 扫码入库
        /// </summary>
        public static string OPCCheckInStockBarcodeOPCGroupName = "OPCCheckInStockBarcodeOPCGroup";
        public static int OPCCheckInStockBarcodeOPCGroupRate = 10;
        public static int OPCCheckInStockBarcodeOPCGroupDeadBand = 0;

        /// <summary>
        /// OPCAgvInStockPassOPCGroup ，AGV 放行
        /// </summary>
        public static string OPCAgvInStockPassOPCGroupName = "OPCAgvInStockPassOPCGroup";
        public static int OPCAgvInStockPassOPCGroupRate = 10;
        public static int OPCAgvInStockPassOPCGroupDeadBand = 0;
        public static int SetOPCAgvInStockPassTimerInterval = 100;

        /// <summary>
        /// OPCInRobootPickOPCGroup ，入库机械手抓取
        /// </summary>
        public static string OPCInRobootPickOPCGroupName = "OPCInRobootPickOPCGroup";
        public static int OPCInRobootPickOPCGroupRate = 10;
        public static int OPCInRobootPickOPCGroupDeadBand = 0;
        public static int SetOPCInRobootPickTimerInterval = 100;


        /// <summary>
        /// OPCSetInStockTaskRM1OPCGroup， 巷道机1
        /// </summary>
        public static string OPCSetInStockTaskRM1OPCGroupName = "OPCSetInStockTaskRM1OPCGroup";
        public static int OPCSetInStockTaskRM1OPCGroupRate = 10;
        public static int OPCSetInStockTaskRM1OPCGroupDeadBand = 0;

        /// <summary>
        /// OPCSetInStockTaskRM2OPCGroup， 巷道机2
        /// </summary>
        public static string OPCSetInStockTaskRM2OPCGroupName = "OPCSetInStockTaskRM2OPCGroup";
        public static int OPCSetInStockTaskRM2OPCGroupRate = 10;
        public static int OPCSetInStockTaskRM2OPCGroupDeadBand = 0;
        #endregion
    }
}
