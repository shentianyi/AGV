using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib
{
    public class OPCAddressMap
    {
        private static string INOUT_AddrPrefix = "S7:[S7 connection_1]DB308,";

        //public static Dictionary<string, string> BaseNameAddress = new Dictionary<string, string>() {

        //    #region 获取条码
        //    { "scan_get_inposi_rw_flag",INOUT_AddrPrefix+"B0.scan_get_inposi_rw_flag"},
        //    { "scan_get_inposi_barcode",INOUT_AddrPrefix+"STRING2.254.scan_get_inposi_barcode"},
        //    #endregion

        //    #region  入库指令
        //    { "in_stock_rw_flag",""},
        //    { "in_stock_position",""},
        //    {"in_stock_box_type","in_stock_box_type" },
        //    { "in_stock_agv_pass_flag",""},
        //    {"in_stock_reset_position_flag","" },
        //    { "in_stock_barcode",""}
        //    #endregion
        //};

        /// <summary>
        /// OPC 变量名与地址的对应
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> GroupNameAddress = new Dictionary<string, Dictionary<string, string>>()
        {
            #region 获取条码
            {
                "OPCGetInStockPosition",
                new Dictionary<string, string>()
                {
                    { "scan_get_inposi_rw_flag",INOUT_AddrPrefix+"B0.scan_get_inposi_rw_flag"},
                    { "scan_get_inposi_barcode",INOUT_AddrPrefix+"STRING2.254.scan_get_inposi_barcode"}
                }
            },
            #endregion
            #region 写入入库任务
            {
                "OPCSetInStockTask",
                new Dictionary<string,string>()
                {
                    { "in_stock_rw_flag",INOUT_AddrPrefix+""},
                    { "in_stock_position",INOUT_AddrPrefix+""},
                    { "in_stock_box_type",INOUT_AddrPrefix+""},
                    { "in_stock_agv_pass_flag",INOUT_AddrPrefix+""},
                    { "in_stock_reset_position_flag",INOUT_AddrPrefix+""},
                    { "in_stock_barcode",INOUT_AddrPrefix+""}
                }
            }
           #endregion
        };

    }
}
