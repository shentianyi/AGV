using AGVCenterLib.Data;
using AGVCenterLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCon.Properties;

namespace TestCon
{
 public   class TestGetMoveService
    {
        public static void Test()
        {
            StockTask st = new StockTaskService(Settings.Default.db).CreateAutoMoveStockTask(1, false);

            Console.WriteLine(st.BarCode);
        }
    }
}
