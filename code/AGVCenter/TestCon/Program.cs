using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConvertUtil;

namespace TestCon
{
    class Program
    {
        public byte B { get; set; }
        static void Main(string[] args)
        {
            int i = 1327671;

            //new Program().C();
            //Console.WriteLine(new Program().B);
            byte b = 0x12;

            Console.WriteLine(  ScaleConvertor.DecimalToByte(12));

            Console.WriteLine(b==Convert.ToByte("18",10));

            Console.WriteLine((byte)148);

            Console.WriteLine("XTest".Contains("XT"));

            string A = "aaaaa";
            string B = A;
            Console.WriteLine(B == A);
            A = "bbbb";

            Console.WriteLine(B == A);
            Console.Read();
        }

        public void C()
        {
            var c = this.GetType().ToString();
            Console.WriteLine(this.GetType().ToString());
        }
    }
}
