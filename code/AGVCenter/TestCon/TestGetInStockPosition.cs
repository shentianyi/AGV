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
        public static void Test()
        {
            PositionService ps = new PositionService(Settings.Default.db);
            List<string> d = new List<string>();
            for (int i = 0; i < 10000; i++)
            {
                var p = ps.FindInStockPosition(1, d);
                if (p == null)
                {
                    Console.WriteLine("no position");
                }
                else
                {
                    d.Add(p.Nr);
                    Console.WriteLine(p.Nr);
                }
            }
        }
    }
}
