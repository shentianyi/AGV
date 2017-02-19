using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AGVCenterLib.Model.ViewModel;
using AGVCenterLib.Service;
using Brilliantech.Framwork.Utils.ConvertUtil;
using TestCon.Properties;

namespace TestCon
{
    class Program
    {
        public byte B { get; set; }
        static void Main(string[] args)
        {
            //int i = 1327671;

            ////new Program().C();
            ////Console.WriteLine(new Program().B);
            //byte b = 0x12;

            //Console.WriteLine(  ScaleConvertor.DecimalToByte(12));

            //Console.WriteLine(b==Convert.ToByte("18",10));

            //Console.WriteLine((byte)148);

            //Console.WriteLine("XTest".Contains("XT"));

            //string A = "aaaaa";
            //string B = A;
            //Console.WriteLine(B == A);
            //A = "bbbb";

            //Console.WriteLine(B == A);


            //Dictionary<string, int> dic = new Dictionary<string, int>();
            //dic.Add("a", 1);
            //dic.Add("d", 4);
            //dic.Add("e", 5);
            //dic.Add("b", 2);
            //dic.Add("c", 3);

            //foreach (var d in dic)
            //{
            //    Console.WriteLine(string.Format("{0}-{1}", d.Key, d.Value));
            //}
            //Console.WriteLine("1".PadLeft(2, '0'));

            //    string r = "^[0-9]{1,3}$";
            //Console.WriteLine(    new Regex(r).IsMatch("1"));
            //    Console.WriteLine(new Regex(r).IsMatch("23"));
            //    Console.WriteLine(new Regex(r).IsMatch("123"));
            //    Console.WriteLine(new Regex(r).IsMatch("1234"));
            //    Console.WriteLine(new Regex(r).IsMatch("a"));
            // TestCreateProduct.Test();
            //TestGetInStockPosition.Test(true);
            //var l = DeliveryStorageViewModel.Converts(new DeliveryService(Settings.Default.db).GetDeliveryStorageByNr("20161211180345"));

            // var d = new DeliveryService(Settings.Default.db).SearchList(new AGVCenterLib.Model.SearchModel.DeliverySearchModel(), 50);
            // var d = new DeliveryService(Settings.Default.db).GetDeliveryStorageByNr("20161219134802");
            //var p=  new PingTest();
            //  p.Test();
            TestStorageService.TestMoveStock();
            Console.Read();
       
        }

        public void C()
        {
            var c = this.GetType().ToString();
            Console.WriteLine(this.GetType().ToString());
            
        }
    }
}
