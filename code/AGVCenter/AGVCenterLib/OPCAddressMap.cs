using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib
{
    public class OPCAddressMap
    {
        public static string INOUT_AddrPrefix = "S7:[S7 connection_1]DB308,";

        /// <summary>
        /// OPC 变量名与地址的对应
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> GroupNameAddress = new Dictionary<string, Dictionary<string, string>>()
        {
            #region 获取条码
            {
                "OPCCheckInStockBarcode",
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
                "OPCInRobootPick",
                 new Dictionary<string, string>()
                {
                    { "inrobot_pick_rw_flag",INOUT_AddrPrefix+"B260.inrobot_pick_rw_flag"},
                    { "inrobot_pick_box_type",INOUT_AddrPrefix+"B261.inrobot_pick_box_type"},
                }
            },
            #endregion
            #region 巷道机1库存任务
            {
                "SetStockTaskRoadMachine1",
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
                "SetStockTaskRoadMachine2",
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
            },
           #endregion

             #region 巷道机1库存任务反馈
            {
                "OPCRoadMachine1TaskFeed",
                new Dictionary<string,string>()
                {
                    {"xdj1_state_current_position1", INOUT_AddrPrefix+"B802.xdj1_state_current_position1"},
                    {"xdj1_state_current_position2", INOUT_AddrPrefix+"B803.xdj1_state_current_position2"},
                    {"xdj1_state_current_position3", INOUT_AddrPrefix+"B804.xdj1_state_current_position3"},
                    {"xdj1_state_target_out_position1", INOUT_AddrPrefix+"B805.xdj1_state_target_out_position1"},
                    {"xdj1_state_target_out_position2", INOUT_AddrPrefix+"B806.xdj1_state_target_out_position2"},
                    {"xdj1_state_target_out_position3", INOUT_AddrPrefix+"B807.xdj1_state_target_out_position3"},
                    {"xdj1_state_target_in_position1", INOUT_AddrPrefix+"B808.xdj1_state_target_in_position1"},
                    {"xdj1_state_target_in_position2", INOUT_AddrPrefix+"B809.xdj1_state_target_in_position2"},
                    {"xdj1_state_target_in_position3", INOUT_AddrPrefix+"B810.xdj1_state_target_in_position3"},
                    {"xdj1_state_current_state", INOUT_AddrPrefix+"B812.Word.xdj1_state_current_state"},
                    {"xdj1_state_err", INOUT_AddrPrefix+"B814.Word.xdj1_state_err"},
                    {"xdj1_state_current_barcode", INOUT_AddrPrefix+"B816.254.String.xdj1_state_current_barcode"},
                    {"xdj1_state_position_barcode", INOUT_AddrPrefix+"B1072.254.String.xdj1_state_position_barcode"},
                    {"xdj1_state_action_flag", INOUT_AddrPrefix+"B1328.xdj1_state_action_flag"}
                }
            },
           #endregion

             #region 巷道机2库存任务反馈
            {
                "OPCRoadMachine2TaskFeed",
                 new Dictionary<string,string>()
                {
                    {"xdj2_state_current_position1", INOUT_AddrPrefix+"B1330.xdj2_state_current_position1"},
                    {"xdj2_state_current_position2", INOUT_AddrPrefix+"B1331.xdj2_state_current_position2"},
                    {"xdj2_state_current_position3", INOUT_AddrPrefix+"B1332.xdj2_state_current_position3"},
                    {"xdj2_state_target_out_position1", INOUT_AddrPrefix+"B1333.xdj2_state_target_out_position1"},
                    {"xdj2_state_target_out_position2", INOUT_AddrPrefix+"B1334.xdj2_state_target_out_position2"},
                    {"xdj2_state_target_out_position3", INOUT_AddrPrefix+"B1335.xdj2_state_target_out_position3"},
                    {"xdj2_state_target_in_position1", INOUT_AddrPrefix+"B1336.xdj2_state_target_in_position1"},
                    {"xdj2_state_target_in_position2", INOUT_AddrPrefix+"B1337.xdj2_state_target_in_position2"},
                    {"xdj2_state_target_in_position3", INOUT_AddrPrefix+"B1338.xdj2_state_target_in_position3"},
                    {"xdj2_state_current_state", INOUT_AddrPrefix+"B1340.Word.xdj2_state_current_state"},
                    {"xdj2_state_err", INOUT_AddrPrefix+"B1342.Word.xdj2_state_err"},
                    {"xdj2_state_current_barcode", INOUT_AddrPrefix+"B1344.254.String.xdj2_state_current_barcode"},
                    {"xdj2_state_position_barcode", INOUT_AddrPrefix+"B1600.254.String.xdj2_state_position_barcode"},
                    {"xdj2_state_action_flag", INOUT_AddrPrefix+"B1856.xdj2_state_action_flag"}
                }
            }
           #endregion

        };

    }
}
