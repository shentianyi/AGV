using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brilliantech.Framwork.Utils.ConvertUtil;
using Brilliantech.Framwork.Utils.LogUtil;

namespace ReadMessage
{
    public class Class1
    {

        public static string readMessage(byte[] MessageBytes)
        {
            int MessageCount = MessageBytes.Count();
            string mean = "";
            if (MessageCount> 9)
            {

                //string name = "FF FF 12 00 01 01 01 01 05 48 65 6C 6C 6F 0A 0B 0B 0B 01  09 08  0A 0B";

                // string name = "FF FF 08 00 01 02 01 01 01  09 08  0A 0B";

                //定义指令头
                string head = "FF FF";
                byte[] heads = ScaleConvertor.HexStringToHexByte(head);
                //定义停止位
                string end = "0A 0B";
                byte[] ends = ScaleConvertor.HexStringToHexByte(end);
                //指令类型          
                string TypeMean = "";
                string MessageId = "第 " + (MessageBytes[3] <<8| MessageBytes[4]) + " 条";
                
                //定义CRC校验
                byte[] CRC = new byte[2] { MessageBytes[MessageCount - 4], MessageBytes[MessageCount - 3] };
                //是否通过CRC校验
                bool CRCpass = ScaleConvertor.IsCrc16Good(CRC);
                string BoxType = "箱型为：";
                string LoadInfo = "装载信息：";
                string CarNr = "小车编号: ";
                string CallingWorkstation = "呼唤工位为: ";
                string StartWorkstation = "出发点的工位编号为： ";
                string LocationNow = "小车当前位置: ";
                string AimedPlace = "小车目标位置: ";
                string speed = "小车速度: ";
                string direction = "小车方向: ";
                string route = "小车路线: ";
                string FailureReason = "";

                //判断指令头和指令我ie
                if (MessageBytes[0] == heads[0] && MessageBytes[1] == heads[1] && MessageBytes[MessageCount - 2] == ends[0] && MessageBytes[MessageCount - 1] == ends[1])
                {

                    switch (MessageBytes[5])
                    {
                        case (byte)01:
                            {

                                TypeMean = "呼唤小车的指令，";
                                

                                CallingWorkstation = CallingWorkstation + MessageBytes[7] + ",";
                                string Ctx = "条码内容为:";
                                char CtxContext = new char();
                                string StoreNr = "仓库号" + MessageBytes[MessageCount - 9] + ",";
                                string StoreFloor = "仓库层" + MessageBytes[MessageCount - 8] + ",";
                                string StoreColum = "仓库列" + MessageBytes[MessageCount - 7] + ",";
                                string StoreRow = "仓库排" + MessageBytes[MessageCount - 6] + ",";
                                string Priority = "优先级" + MessageBytes[MessageCount - 5];



                                switch (MessageBytes[6])
                                {
                                    case (byte)01: BoxType += "小箱，"; break;
                                    case (byte)02: BoxType += "大箱，"; break;
                                    default: BoxType += "未知箱型"; break;

                                }
                                for (int i = 1; i <= MessageBytes[8]; i++)
                                {
                                    CtxContext = Convert.ToChar(MessageBytes[8 + i]);
                                    Ctx += CtxContext;

                                }
                                Ctx += ",";
                                mean = MessageId + TypeMean + BoxType + CallingWorkstation + Ctx + StoreNr + StoreFloor + StoreColum + StoreRow + Priority;

                                return mean;

                            }

                        case (byte)02:
                            {
                                TypeMean = "呼唤小车响应";
                                string SuccessOrFailure = "";
                                CarNr = CarNr + MessageBytes[7] + "。";
                                switch (MessageBytes[6])
                                {
                                    case (byte)01:
                                        {
                                            SuccessOrFailure = "指令成功,";
                                            mean = MessageId + TypeMean + SuccessOrFailure + CarNr;

                                        }
                                        break;


                                    case (byte)02:
                                        {
                                            SuccessOrFailure = "指令失败，";
                                            switch (MessageBytes[8])
                                            {
                                                case (byte)01: FailureReason = "失败原因：小车未响应"; break;
                                                case (byte)02: FailureReason = "失败原因：工位未设置"; break;
                                                case (byte)255: FailureReason = "失败原因：未知"; break;
                                                default: FailureReason = "失败原因：未知错误"; break;
                                            }
                                            mean = MessageId + TypeMean + SuccessOrFailure + CarNr + FailureReason;
                                        }
                                        break;
                                    default: SuccessOrFailure = "未知"; break;

                                }
                                return mean;
                            }


                        case (byte)03:
                            {
                                TypeMean = "小车出发";
                                StartWorkstation = StartWorkstation + MessageBytes[6] + ",";
                                CarNr = CarNr + MessageBytes[7] + "。";
                                mean = MessageId + TypeMean + StartWorkstation + CarNr;
                                return mean;
                            }

                        case (byte)04:
                            {
                                TypeMean = "小车出发响应指令";
                                CarNr = CarNr + MessageBytes[6] + "。";
                                mean = MessageId + TypeMean + CarNr;
                                return mean;
                            }
                        case (byte)05:
                            {
                                TypeMean = "小车状态指令";
                                CarNr = CarNr + MessageBytes[6] + "。";
                                switch (MessageBytes[7])
                                {
                                    case (byte)01: LoadInfo += "未装箱，"; break;
                                    case (byte)02: LoadInfo += "装满箱，"; break;
                                    case (byte)03: LoadInfo += "装空箱，"; break;
                                    default: LoadInfo += "未知装载信息,"; break;

                                }


                                switch (MessageBytes[8])
                                {
                                    case (byte)01: BoxType += "小箱，"; break;
                                    case (byte)02: BoxType += "大箱，"; break;
                                    default: BoxType += "未知箱型,"; break;

                                }
                                LocationNow += MessageBytes[9]+" , ";
                                AimedPlace += MessageBytes[10] + ", ";
                                speed += MessageBytes[11] + ",";
                                direction += (MessageBytes[12] << 8 | MessageBytes[13]) + ",";
                                route += MessageBytes[14] + "。";
                                mean = MessageId + TypeMean + CarNr + LoadInfo + BoxType + LocationNow + AimedPlace + speed + direction + route;


                                return mean;
                            }

                        case (byte)06:
                            {
                                TypeMean = "小车到达指令,";
                                CallingWorkstation = CallingWorkstation + MessageBytes[6] + ",";
                                CarNr = CarNr + MessageBytes[7] + "。";
                                mean = MessageId + TypeMean + CallingWorkstation + CarNr;
                                return mean;
                            }
                        case (byte)07:
                            {
                                TypeMean = "小车到达响应指令,";
                                CarNr = CarNr + MessageBytes[6] + "。";
                                mean = MessageId + TypeMean + CarNr;
                                return mean;

                            }


                        case (byte)08: TypeMean = "取消小车呼唤"; break;
                        case (byte)09: TypeMean = "取消小车呼唤响应"; break;
                        case (byte)10: TypeMean = "获出库物库位信息"; break;
                        case (byte)11: TypeMean = "获出库物库位信息响应"; break;
                        case (byte)12: TypeMean = "入库操作完成"; break;
                        case (byte)13: TypeMean = "入库操作完成响应"; break;
                        case (byte)14: TypeMean = "出库"; break;
                        case (byte)15: TypeMean = "出库响应"; break;
                        case (byte)16: TypeMean = "出库操作完成"; break;
                        case (byte)17: TypeMean = "出库操作完成响应"; break;
                        case (byte)18: TypeMean = "码垛（整个托盘）完成"; break;
                        case (byte)19: TypeMean = "码垛（整个托盘）完成响应"; break;
                        case (byte)20: TypeMean = "请求启动或停止设备"; break;
                        case (byte)21: TypeMean = "请求启动或停止设备响应"; break;
                        case (byte)22: TypeMean = "启动或停止设备"; break;
                        case (byte)23: TypeMean = "启动或停止设备响应"; break;
                        case (byte)24: TypeMean = "警报"; break;
                        case (byte)25: TypeMean = "警报响应"; break;
                        default: TypeMean = "未知类型"; break;


                    }




                }

                else
                {
                    mean = ("指令格式有误");

                }

               
            }
            else
            {
                mean = ("指令格式有误");
            }

            return mean;

        }
    }
}
