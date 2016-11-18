using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brilliantech.Framwork.Utils.ConvertUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using ReadMessage;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // byte[] MessageBytes = new byte[1024];
            // MessageBytes[3] = 1;
            // MessageBytes[4] = 2;
            //string name = "FF FF 13 01 02 12 01 01 13 02 32 02 01 09 08 0A 0B";
            string name = "FF FF 09 01 02 17 01 01 13 01 02 01 09 08 0A 0B";

            byte[] MessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string means = ReadMessage.Class1.readMessage(MessageBytes);
            Console.WriteLine(means);
            Console.ReadLine();
        }
    }
}
