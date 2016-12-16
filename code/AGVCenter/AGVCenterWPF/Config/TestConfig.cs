using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF.Config
{
  public  class TestConfig
    {
        private static ConfigUtil config;
        private static bool inStockCreateStorage = true;
        private static bool outStockTaskDelStorage = true;
        private static bool showRescanErrorBarcode = true;

        static TestConfig()
        {
            try
            {
                config = new ConfigUtil("TEST", "Config/test.ini");
                inStockCreateStorage = bool.Parse(config.Get("inStockCreateStorage"));
                outStockTaskDelStorage = bool.Parse(config.Get("outStockTaskDelStorage"));
                showRescanErrorBarcode = bool.Parse(config.Get("showRescanErrorBarcode"));

            }
            catch(Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        public static bool InStockCreateStorage
        {
            get
            {
                return inStockCreateStorage;
            }

            set
            {
                inStockCreateStorage = value;
                config.Set("inStockCreateStorage", value);
                config.Save();
            }
        }

        public static bool OutStockTaskDelStorage
        {
            get
            {
                return outStockTaskDelStorage;
            }

            set
            {
                outStockTaskDelStorage = value;
                config.Set("outStockTaskDelStorage", value);
                config.Save();
            }
        }

        public static bool ShowRescanErrorBarcode
        {
            get
            {
                return showRescanErrorBarcode;
            }

            set
            {
                outStockTaskDelStorage = value;
                config.Set("outStockTaskDelStorage", value);
                config.Save();
            }
        }
    }
}
