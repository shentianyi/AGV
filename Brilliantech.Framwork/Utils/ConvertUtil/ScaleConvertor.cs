using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.Framwork.Utils.ConvertUtil
{
   public class ScaleConvertor
    { /// <summary>
      /// 十进制数字转十六进制字符串
      /// </summary>
      /// <param name="deci">十进制数</param>
      /// <param name="upcase">是否大写字符</param>
      /// <param name="hexStringLength">十六进制字符串长度，长度为4的例子00FF,0001,FFFF...</param>
      /// <returns></returns>
        public static string DecimalToHexString(int deci, bool upcase = true, int? hexStringLength = 4)
        {
            string hexString = Convert.ToString(deci, 16);
            if (hexStringLength.HasValue)
            {
                hexString = hexString.PadLeft(hexStringLength.Value, '0');
            }
            return upcase ? hexString.ToUpper() : hexString;
        }

        //功能同DecimalToHexString 不自动补零
        public static string DecimalToHex(int deci, bool upcase = true, int? hexStringLength = 4)
        {
            string hexString = Convert.ToString(deci, 16);
            if (hexStringLength.HasValue)
            {
               // hexString = hexString.PadLeft(hexStringLength.Value, '0');
            }
            return upcase ? hexString.ToUpper() : hexString;
        }

        /// <summary>
        /// 十进制数字转换为bytes
        /// </summary>
        /// <param name="deci"></param>
        /// <returns></returns>
        public static byte[] DecimalToByte(UInt16 deci)
        {
            return HexStringToHexByte(DecimalToHexString(deci));
        }
        /// <summary>
        /// 十六进制字符转十进制数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int HexByteToDecimal(byte data)
        {
            return Convert.ToInt32(HexBytesToString(new byte[1] { data }, false), 16);
        }


        /// <summary>
        /// 十六进制字符数组转十进制数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int HexBytesToDecimal(byte[] data)
        {
            return Convert.ToInt32(HexBytesToString(data, false), 16);
        }

        /// <summary>
        /// HEX字符串转换为HEX数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStringToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        /// <summary>
        /// HEX数值转换为HEX字符串
        /// </summary>
        /// <param name="hexBytes"></param>
        /// <param name="withBlank">是否已空格分隔</param>
        /// <returns></returns>
        public static string HexBytesToString(byte[] hexBytes, bool withBlank = true)
        {
            string hexString = string.Empty;
            if (hexBytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < hexBytes.Length; i++)
                {
                    if (withBlank)
                    {
                        strB.Append(hexBytes[i].ToString("X2") + " ");
                    }
                    else {
                        strB.Append(hexBytes[i].ToString("X2"));
                    }
                }

                hexString = strB.ToString().TrimEnd();
            }
            return hexString;
        }

        /// <summary>
        /// 计算字节累加和，保留从低位到高位2字节数的结果
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] CalculateFcc(byte[] bytes)
        {

            int num = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                num = (num + bytes[i]) % 0xffff;
            }
            bytes = BitConverter.GetBytes(num);
            return new byte[] { bytes[1], bytes[0] };
        }


        static UInt16[] crctab16 ={
            0x0000, 0x1189, 0x2312, 0x329b, 0x4624, 0x57ad, 0x6536, 0x74bf,
            0x8c48, 0x9dc1, 0xaf5a, 0xbed3, 0xca6c, 0xdbe5, 0xe97e, 0xf8f7,
            0x1081, 0x0108, 0x3393, 0x221a, 0x56a5, 0x472c, 0x75b7, 0x643e,
            0x9cc9, 0x8d40, 0xbfdb, 0xae52, 0xdaed, 0xcb64, 0xf9ff, 0xe876,
            0x2102, 0x308b, 0x0210, 0x1399, 0x6726, 0x76af, 0x4434, 0x55bd,
            0xad4a, 0xbcc3, 0x8e58, 0x9fd1, 0xeb6e, 0xfae7, 0xc87c, 0xd9f5,
            0x3183, 0x200a, 0x1291, 0x0318, 0x77a7, 0x662e, 0x54b5, 0x453c,
            0xbdcb, 0xac42, 0x9ed9, 0x8f50, 0xfbef, 0xea66, 0xd8fd, 0xc974,
            0x4204, 0x538d, 0x6116, 0x709f, 0x0420, 0x15a9, 0x2732, 0x36bb,
            0xce4c, 0xdfc5, 0xed5e, 0xfcd7, 0x8868, 0x99e1, 0xab7a, 0xbaf3,
            0x5285, 0x430c, 0x7197, 0x601e, 0x14a1, 0x0528, 0x37b3, 0x263a,
            0xdecd, 0xcf44, 0xfddf, 0xec56, 0x98e9, 0x8960, 0xbbfb, 0xaa72,
            0x6306, 0x728f, 0x4014, 0x519d, 0x2522, 0x34ab, 0x0630, 0x17b9,
            0xef4e, 0xfec7, 0xcc5c, 0xddd5, 0xa96a, 0xb8e3, 0x8a78, 0x9bf1,
            0x7387, 0x620e, 0x5095, 0x411c, 0x35a3, 0x242a, 0x16b1, 0x0738,
            0xffcf, 0xee46, 0xdcdd, 0xcd54, 0xb9eb, 0xa862, 0x9af9, 0x8b70,
            0x8408, 0x9581, 0xa71a, 0xb693, 0xc22c, 0xd3a5, 0xe13e, 0xf0b7,
            0x0840, 0x19c9, 0x2b52, 0x3adb, 0x4e64, 0x5fed, 0x6d76, 0x7cff,
            0x9489, 0x8500, 0xb79b, 0xa612, 0xd2ad, 0xc324, 0xf1bf, 0xe036,
            0x18c1, 0x0948, 0x3bd3, 0x2a5a, 0x5ee5, 0x4f6c, 0x7df7, 0x6c7e,
            0xa50a, 0xb483, 0x8618, 0x9791, 0xe32e, 0xf2a7, 0xc03c, 0xd1b5,
            0x2942, 0x38cb, 0x0a50, 0x1bd9, 0x6f66, 0x7eef, 0x4c74, 0x5dfd,
            0xb58b, 0xa402, 0x9699, 0x8710, 0xf3af, 0xe226, 0xd0bd, 0xc134,
            0x39c3, 0x284a, 0x1ad1, 0x0b58, 0x7fe7, 0x6e6e, 0x5cf5, 0x4d7c,
            0xc60c, 0xd785, 0xe51e, 0xf497, 0x8028, 0x91a1, 0xa33a, 0xb2b3,
            0x4a44, 0x5bcd, 0x6956, 0x78df, 0x0c60, 0x1de9, 0x2f72, 0x3efb,
            0xd68d, 0xc704, 0xf59f, 0xe416, 0x90a9, 0x8120, 0xb3bb, 0xa232,
            0x5ac5, 0x4b4c, 0x79d7, 0x685e, 0x1ce1, 0x0d68, 0x3ff3, 0x2e7a,
            0xe70e, 0xf687, 0xc41c, 0xd595, 0xa12a, 0xb0a3, 0x8238, 0x93b1,
            0x6b46, 0x7acf, 0x4854, 0x59dd, 0x2d62, 0x3ceb, 0x0e70, 0x1ff9,
            0xf78f, 0xe606, 0xd49d, 0xc514, 0xb1ab, 0xa022, 0x92b9, 0x8330,
            0x7bc7, 0x6a4e, 0x58d5, 0x495c, 0x3de3, 0x2c6a, 0x1ef1, 0x0f78,
        };
        /// <summary>
        /// CRC ITU
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public static UInt16 GetCrc16(byte[] pData)
        {
            UInt16 fcs = 0xffff;
            int nLength = pData.Length;
            int i = 0;
            while (nLength > 0)
            {
                fcs = (UInt16)((fcs >> 8) ^ crctab16[(fcs ^ pData[i]) & 0xff]);
                nLength--;
                i++;
            }
            return (UInt16)(~fcs);
        }

        /// <summary>
        /// 检查CRC是否符合规范
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        public static bool IsCrc16Good(byte[] pData)
        {
            var crc = GetCrc16(pData.Take(pData.Length - 2).ToArray());

            var crcTo = HexBytesToDecimal(pData.Skip(pData.Length - 2).Take(2).ToArray());
            return crc == crcTo;
        }
        //public static bool IsCrc16Good(byte[] pData)
        //{
        //    UInt16 fcs = 0xffff;    // 初始化
        //    int nLength = pData.Length;
        //    int i = 0;
        //    while (nLength > 0)
        //    {
        //        fcs = (UInt16)((fcs >> 8) ^ crctab16[(fcs ^ pData[i]) & 0xff]);
        //        nLength--;
        //        i++;
        //    }

        //    return (fcs == 0xf0b8);  // 0xf0b8是CRC-ITU的"Magic Value"
        //}
    }
}
