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
            #region 小车入库扫描放行
            {
                "OPCAgvInStockPass",
                new Dictionary<string, string>()
                {
                    { "stock_pass_agv_rw_flag",INOUT_AddrPrefix+"B258.stock_pass_agv_rw_flag"},
                    { "stock_pass_agv_flag",INOUT_AddrPrefix+"B259.stock_pass_agv_flag"},
                }
            },
            #endregion
            #region 入库机械手任务信息
            {
                "OPCRobotInStockTask",
                 new Dictionary<string, string>()
                {
                    { "inrobot_pick_rw_flag",INOUT_AddrPrefix+"B260.inrobot_pick_rw_flag"},
                    { "inrobot_pick_box_type",INOUT_AddrPrefix+"B261.inrobot_pick_box_type"},
                }
            },
            #endregion
            #region 巷道机1库存任务
            {
                "OPCXDJ1StockTask",
                new Dictionary<string,string>()
                {
                    { "stock_xdj1_rw_flag",INOUT_AddrPrefix+"B262.stock_xdj1_rw_flag"},
                    { "stock_xdj1_action_type",INOUT_AddrPrefix+"B263.stock_xdj1_action_type"},
                    { "stock_xdj1_position1",INOUT_AddrPrefix+"B264.stock_xdj1_position1"},
                    { "stock_xdj1_position2",INOUT_AddrPrefix+"B265.stock_xdj1_position2"},
                    { "stock_xdj1_position3",INOUT_AddrPrefix+"B266.stock_xdj1_position3"},
                    { "stock_xdj1_box_type",INOUT_AddrPrefix+"B267.stock_xdj1_box_type"},
                    { "stock_xdj1_reset_position_flag",INOUT_AddrPrefix+"B268.stock_xdj1_reset_position_flag"},
                    { "stock_xdj1_tray_reverse_no",INOUT_AddrPrefix+"B270.Int.stock_xdj1_tray_reverse_no"},
                    { "stock_xdj1_tray_num",INOUT_AddrPrefix+"B272.Int.stock_xdj1_tray_num"},
                    { "stock_xdj1_delivery_num",INOUT_AddrPrefix+"B274.Int.stock_xdj1_delivery_num"},
                    { "stock_xdj1_barcode",INOUT_AddrPrefix+"B276.254.String.stock_xdj1_barcode"}
                }
            },
           #endregion
           #region 巷道机2库存任务
            {
                "OPCXDJ2StockTask",
                new Dictionary<string,string>()
                {
                   { "stock_xdj2_rw_flag",INOUT_AddrPrefix+"B532.stock_xdj2_rw_flag"},
                    { "stock_xdj2_action_type",INOUT_AddrPrefix+"B533.stock_xdj2_action_type"},
                    { "stock_xdj2_position1",INOUT_AddrPrefix+"B534.stock_xdj2_position1"},
                    { "stock_xdj2_position2",INOUT_AddrPrefix+"B535.stock_xdj2_position2"},
                    { "stock_xdj2_position3",INOUT_AddrPrefix+"B536.stock_xdj2_position3"},
                    { "stock_xdj2_box_type",INOUT_AddrPrefix+"B537.stock_xdj2_box_type"},
                    { "stock_xdj2_reset_position_flag",INOUT_AddrPrefix+"B538.stock_xdj2_reset_position_flag"},
                    { "stock_xdj2_tray_reverse_no",INOUT_AddrPrefix+"B540.Int.stock_xdj2_tray_reverse_no"},
                    { "stock_xdj2_tray_num",INOUT_AddrPrefix+"B542.Int.stock_xdj2_tray_num"},
                    { "stock_xdj2_delivery_num",INOUT_AddrPrefix+"B544.Int.stock_xdj2_devlivery_num"},
                    { "stock_xdj2_barcode",INOUT_AddrPrefix+"B546.254.stock_xdj2_barcode"}
                }
            }
           #endregion
        };

    }
}
