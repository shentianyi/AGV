using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Service;
using TestCon.Properties;

namespace TestCon
{
    public class TestGetInStockPosition
    {
        public static void Test(bool doInStock=false)
        {
            PositionService ps = new PositionService(Settings.Default.db);
            List<string> d = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                var p = ps.FindInStockPosition(i % 2 == 0 ? 1 : 2, d);

                if (p == null)
                {
                    Console.WriteLine("no position");
                }
                else
                {
                    d.Add(p.Nr);
                    if (doInStock)
                    {
                        StorageService ss = new StorageService(Settings.Default.db);
                        var m = ss.InStockByCheckCode(p.Nr, i.ToString());
                        Console.WriteLine(m.Content);
                    }
                    Console.WriteLine(p.Nr);
                }
            }
        }
    }
}
