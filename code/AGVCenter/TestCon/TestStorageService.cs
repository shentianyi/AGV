using AGVCenterLib.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestCon.Properties;

namespace TestCon
{
 public   class TestStorageService
    {

        public static void TestMoveStock()
        {
            StorageService ss = new StorageService(Settings.Default.db);
            var msg = ss.MoveStockByUniqItemNr("4BEI000080EN", "C 04 29");
            Console.WriteLine(string.Format("{0}-{1}", msg.Success, msg.Content));

            msg = ss.MoveStockByUniqItemNr("4BEI000080sEN", "C 04 29");
            Console.WriteLine(string.Format("{0}-{1}", msg.Success, msg.Content));

            msg = ss.MoveStockByUniqItemNr("4BEI000080EN", "cC 04 29");
            Console.WriteLine(string.Format("{0}-{1}", msg.Success, msg.Content));

            msg = ss.MoveStockByUniqItemNr("4BEI000080EN", "C 05 29");
            Console.WriteLine(string.Format("{0}-{1}", msg.Success, msg.Content));
        }
    }
}
