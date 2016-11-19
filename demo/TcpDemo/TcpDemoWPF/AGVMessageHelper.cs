using Brilliantech.Framwork.Utils.ConvertUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpDemoWPF
{
   public class AGVMessageHelper
    {
        public static byte[] GenMsg(byte[] msgBody)
        {
            /// ? 如何快速赋值呢？
            byte[] crc = ScaleConvertor.DecimalToByte(ScaleConvertor.GetCrc16(msgBody));
            byte[] msg = new byte[msgBody.Length + 6];
            msg[0] = 0xFF;
            msg[1] = 0xFF;
            for (int i = 0; i < msgBody.Length; i++)
            {
                msg[2 + i] = msgBody[i];
            }
            msg[2 + msgBody.Length] = crc[0];

            msg[3 + msgBody.Length] = crc[1];

            msg[4 + msgBody.Length] = 0x0A;

            msg[5 + msgBody.Length] = 0x0B;

            return msg;
        }
    }
}
