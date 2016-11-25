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
                    { "scan_get_inposi_barcode",INOUT_AddrPrefix+"B2.254.String.scan_get_inposi_barcode"}
                }
            },
            #endregion
            #region 写入入库任务
            {
                "OPCSetInStockTask",
                new Dictionary<string,string>()
                {
                    { "in_stock_rw_flag",INOUT_AddrPrefix+"B258.in_stock_rw_flag"},
                    { "in_stock_position_floor",INOUT_AddrPrefix+"B259.in_stock_position1"},
                    { "in_stock_position_column",INOUT_AddrPrefix+"B260.in_stock_position2"},
                    { "in_stock_position_row",INOUT_AddrPrefix+"B261.in_stock_position3"},
                    { "in_stock_box_type",INOUT_AddrPrefix+"B262.in_stock_box_type"},
                    { "in_stock_agv_pass_flag",INOUT_AddrPrefix+"B263.in_stock_agv_pass_flag"},
                    { "in_stock_reset_position_flag",INOUT_AddrPrefix+"B264.in_stock_reset_position_flag"},
                    { "in_stock_barcode",INOUT_AddrPrefix+"B266.254.String.in_stock_barcode"}
                }
            }
           #endregion
        };

    }
}
