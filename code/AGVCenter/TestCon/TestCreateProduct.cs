using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Service;
using TestCon.Properties;

namespace TestCon
{
   public class TestCreateProduct
    {
        public static void Test()
        {
            UniqueItemService uis = new UniqueItemService(Settings.Default.db);
            for (int i = 9; i < 1000; i++)
            {
                Console.WriteLine(i);
                var iss = i.ToString();
                uis.Create(new UniqueItem()
                {
                    Nr = iss,
                    QR = iss,
                    KNr = iss,
                    KNrWithYear = iss,
                    CheckCode = iss,
                    KskNr = iss,
                    BoxTypeId = i % 2 == 0 ?
                        (int)AGVCenterLib.Enum.BoxType.Big : (int)AGVCenterLib.Enum.BoxType.Small

                });
            }
        }
    }
}
