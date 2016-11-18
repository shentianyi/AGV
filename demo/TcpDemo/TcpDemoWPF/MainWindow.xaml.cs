using Brilliantech.Framwork.Utils.LogUtil;
using Brilliantech.Framwork.Utils.ConvertUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TcpDemoWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private static byte[] result = new byte[1024];
        private static int myProt = 8080;   //端口  
        static Socket SocketTcp;

        private void LogBtn_Click(object sender, RoutedEventArgs e)
        {
            LogUtil.Logger.Info("hello log world....");
           
               
            

        }

        private void MakeConnection_Click(object sender, RoutedEventArgs e)
        {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        SocketTcp.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
        SocketTcp.Listen(10);    //设定最多10个排队连接请求  
        MessageBox.Show("启动监听{0}成功", SocketTcp.LocalEndPoint.ToString());


            bool runflag = true;
            Socket SocketTcpAccept = SocketTcp.Accept();
           // byte[] serversay = Encoding.UTF8.GetBytes("Can I hellp u");

           // SocketTcpAccept.Send(serversay);
            while (runflag)
        {
                try
                {

                    
                    SocketTcpAccept.Receive(result);
                    
                    string serverResponse = System.Text.Encoding.UTF8.GetString(result).Trim(("\0".ToCharArray()));
                    byte[] MessageBytes = ScaleConvertor.HexStringToHexByte(serverResponse);
                    string means = ReadMessage.Class1.readMessage(MessageBytes);
                    MessageBox.Show(means);
                    LogUtil.Logger.Info(means);
                    
                    switch(MessageBytes[5])
                    {
                        case (byte)01://呼唤小车
                            {
                                string name = "FF FF 08" + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 02 01 01 01  09 08  0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                            case (byte)06://小车到达
                            {
                                string name = "FF FF 06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 07 01  09 08  0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)03://小车出发
                            {
                                string name = "FF FF 06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 04 01  09 08  0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)08://取消小车呼唤
                            {
                                string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 09 02 02 01 09 08 0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)10://获取货物库位信息
                            {
                                string name = "FF FF 11 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0B 04 01 05 48 65 6C 6C 6F 01 02 03 04 09 08 0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)12://入库完成指令0X0C
                            {
                                string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0D 04 01 13  09 08 0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)13://出库指令0X0D 
                            {

                                string name = "FF FF 07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01 09 08 0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)14://出库指令0X0E
                            {

                                string name = "FF FF 07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01  09 08 0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        case (byte)16://出库完成指令0X10
                            {

                                string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 11 02 01 13 09 08 0A 0B";
                                byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                name = null;
                                break;
                            }
                        default: break;

                            }
                    


                    /*
                if (MessageBytes[5]==(byte)01)//呼唤小车
                    {
                        string name = "FF FF 08" + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 02 01 01 01  09 08  0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if(MessageBytes[5] == (byte)06)//小车到达
                    {
                       
                        string name = "FF FF 06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 07 01  09 08  0A 0B";
                        //string name = "FF FF 06 00 01 07 01  09 08  0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)03)//小车出发
                    {

                        string name = "FF FF 06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 04 01  09 08  0A 0B";
                       
                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)08)//取消小车呼唤
                    {

                        string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 09 02 02 01 09 08 0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)10)//获取货物库位信息
                    {

                        string name = "FF FF 11 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0B 04 01 05 48 65 6C 6C 6F 01 02 03 04 09 08 0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)12)//入库完成指令0X0C
                    {

                        string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0D 04 01 13  09 08 0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)13)//出库指令0X0D
                    {

                        string name = "FF FF 07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01 09 08 0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)14)//出库指令0X0E
                    {

                        string name = "FF FF 07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01  09 08 0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    else if (MessageBytes[5] == (byte)16)//出库完成指令0X10
                    {

                        string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 11 02 01 13 09 08 0A 0B";

                        byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                        SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                        name = null;
                    }
                    */




                    result = new byte[1024];
                    serverResponse = "";

                    //SocketTcpAccept.Send(Encoding.Unicode.GetBytes(serverResponse));
                }
                catch(Exception ee)
                {
                    SocketTcpAccept.Close();
                    MessageBox.Show(ee.Message);
                    LogUtil.Logger.Info(ee.Message);
                    runflag = false;
                    
                   
                }
               
            }
        }


      

    }
}
