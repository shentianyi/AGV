using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF.Config
{
    public class BaseConfig
    {
        private static ConfigUtil config;
        private static bool autoConnectOPC = true;
        //    private static string connectString = string.Empty;
        private static bool roadMachine1Enabled = true;
        private static bool roadMachine2Enabled = true;
        private static int maxMonitorTaskNum = 300;
        private static int keepMonitorTaskNum = 200;
        private static bool autoLoadDbTaskOnStart = true;
        private static string preScanBar = string.Empty;
        private static bool isOPCConnector = true;
        private static string barcodeReg = "^[0-9]{10}[A-Z]{2}$";
        // 是否使用库存入库优先级
        private static bool isUsePositionPriority = false;
        private static bool isSelfAreaMove = false;
        static BaseConfig()
        {
            try
            {
                config = new ConfigUtil("BASE", "Config/base.ini");
                autoConnectOPC = bool.Parse(config.Get("autoConnectOPC"));
                //    connectString = config.Get("connectString");

                roadMachine1Enabled = bool.Parse(config.Get("roadMachine1Enabled"));
                roadMachine2Enabled = bool.Parse(config.Get("roadMachine2Enabled"));
                maxMonitorTaskNum = int.Parse(config.Get("maxMonitorTaskNum"));
                keepMonitorTaskNum = int.Parse(config.Get("keepMonitorTaskNum"));
                autoLoadDbTaskOnStart = bool.Parse(config.Get("autoLoadDbTaskOnStart"));
                preScanBar = config.Get("preScanBar");
                isOPCConnector = bool.Parse(config.Get("isOPCConnector"));
                barcodeReg = config.Get("barcodeReg");

                isUsePositionPriority = bool.Parse(config.Get("isUsePositionPriority"));

                isSelfAreaMove = bool.Parse(config.Get("isSelfAreaMove"));
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        public static bool AutoConnectOPC
        {
            get
            {
                return autoConnectOPC;
            }

            set
            {
                autoConnectOPC = value;
                config.Set("autoConnectOPC", value);
                config.Save();
            }
        }


        public static bool RoadMachine1Enabled
        {
            get
            {
                return roadMachine1Enabled;
            }

            set
            {
                roadMachine1Enabled = value;
                config.Set("roadMachine1Enabled", value);
                config.Save();
            }
        }

        public static bool RoadMachine2Enabled
        {
            get
            {
                return roadMachine2Enabled;
            }

            set
            {
                roadMachine2Enabled = value;
                config.Set("roadMachine2Enabled", value);
                config.Save();
            }
        }

        public static int MaxMonitorTaskNum
        {
            get
            {
                return maxMonitorTaskNum;
            }
            set
            {
                maxMonitorTaskNum = value;
                config.Set("maxMonitorTaskNum", value);
                config.Save();
            }
        }

        public static int KeepMonitorTaskNum
        {
            get
            {
                return keepMonitorTaskNum;
            }
            set
            {
                keepMonitorTaskNum = value;
                config.Set("keepMonitorTaskNum", value);
                config.Save();

            }
        }

        public static bool AutoLoadDbTaskOnStart
        {
            get
            {
                return autoLoadDbTaskOnStart;
            }

            set
            {
                autoLoadDbTaskOnStart = value;
                config.Set("autoLoadDbTaskOnStart", value);
                config.Save();
            }
        }

        public static string PreScanBar
        {
            get
            {
                return preScanBar;
            }

            set
            {
                preScanBar = value;
                config.Set("preScanBar", value);
                config.Save();
            }
        }


        public static bool IsOPCConnector
        {
            get
            {
                return isOPCConnector;
            }

            set
            {
                isOPCConnector = value;
                config.Set("isOPCConnector", value);
                config.Save();
            }
        }

        public static string BarcodeReg
        {
            get
            {
                return barcodeReg;
            }

            set
            {
                barcodeReg = value;
                config.Set("barcodeReg", value);
                config.Save();
            }
        }

        public static bool IsUsePositionPriority
        {
            get
            {
                return isUsePositionPriority;
            }

            set
            {
                isUsePositionPriority = value;
                config.Set("isUsePositionPriority", value);
                config.Save();
            }
        }

        public static bool IsSelfAreaMove
        {
            get
            {
                return isSelfAreaMove;
            }

            set
            {
                isSelfAreaMove = value;
                config.Set("isSelfAreaMove", value);
                config.Save();
            }
        }


    }
}
