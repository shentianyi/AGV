using AGVCenterLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCon.Properties;

namespace TestCon
{
public    class TestPositionService
    {
        public  static void TestFindPostinToStock()
        {
            var ss = new PositionService(Settings.Default.db);
            var p = ss.FindInStockPosition(1, null, false, true);
            Console.Write(p.Nr);
        }
    }
}
