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
        private static int myProt = 8080;   //端口  
        static Socket SocketTcp;

        private void LogBtn_Click(object sender, RoutedEventArgs e)
        {
            LogUtil.Logger.Info(ReceiveText.Text);
            LogUtil.Logger.Info(SendText.Text);
            MessageBox.Show("指令写入日志成功");





        }
        Thread ClientRecieveThread;
            bool runflag = true;
        private void MakeConnection_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketTcp.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            SocketTcp.Listen(10);    //设定最多10个排队连接请求  
            MessageBox.Show("启动监听{0}成功", SocketTcp.LocalEndPoint.ToString());


            Socket SocketTcpAccept = SocketTcp.Accept();
            // byte[] serversay = Encoding.UTF8.GetBytes("Can I hellp u");

            // SocketTcpAccept.Send(serversay);
            ClientRecieveThread = new Thread(() =>
            {
                while (runflag)
                {
                    try
                    {
                        byte[] result = new byte[1024];
                        int dataLength = SocketTcpAccept.Receive(result);
                        byte[] MessageBytes = result.Take(dataLength).ToArray();
                        string Receivemeans = ReadMessage.Parser.readMessage(MessageBytes);
                        // ReceiveMessageText.AppendText(Receivemeans);  
                        //  this.Dispatcher.Invoke(new Action(() => {ReceiveMessageText.AppendText(Receivemeans+"\n"); }));
                        this.Dispatcher.Invoke(new Action(() => { ReceiveText.AppendText(Receivemeans + "\n"); }));
                        // LogUtil.Logger.Info(Receivemeans);

                        switch (MessageBytes[5])
                        {
                            case 0x01://呼唤小车
                                {
                                    string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 02 01 01 01  09 08  0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);

                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";

                                    break;
                                }
                            case (byte)06://小车到达
                                {
                                    string name = "FF FF 06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 07 01  09 08  0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);

                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);

                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)03://小车出发
                                {
                                    string name = "FF FF 06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 04 01  09 08  0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)08://取消小车呼唤
                                {
                                    string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 09 02 02 01 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)10://获取货物库位信息
                                {
                                    string name = "FF FF 11 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0B 04 01 05 48 65 6C 6C 6F 01 02 03 04 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)12://入库完成指令0X0C
                                {
                                    string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0D 04 01 13  09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)13://出库指令0X0D 
                                {

                                    string name = "FF FF 07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)14://出库指令0X0E
                                {

                                    string name = "FF FF 07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01  09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)16://出库完成指令0X10
                                {

                                    string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 11 02 01 13 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)18://码垛（整个托盘）完成指令0X12
                                {
                                    string name = "FF FF 08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 13 01 01 13 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";

                                    break;

                                }
                            case (byte)20://请求启动或停止设备指令0X14
                                {
                                    string name = "FF FF 09 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 15 01 01 13 01 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)22://启动或停止设备指令0X16
                                {
                                    string name = "FF FF 0B " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 17 01 01 13 01 02 01 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }
                            case (byte)24://警报指令0X19
                                {

                                    string name = "FF FF 0B " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 19 01 01 13 01 02 01 09 08 0A 0B";
                                    byte[] nameBuf = Encoding.UTF8.GetBytes(name);
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
                                    this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
                                    SocketTcpAccept.Send(nameBuf, nameBuf.Length, SocketFlags.None);
                                    SendMeans = "";
                                    name = "";
                                    break;
                                }



                            default: break;

                        }


                    }
                    catch (Exception ee)
                    {
                        SocketTcpAccept.Close();
                        MessageBox.Show(ee.Message);
                        LogUtil.Logger.Info(ee.Message);
                        runflag = false;


                    }


                }

            });
            ClientRecieveThread.Start();
        }

        private void SendText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ReceiveText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            runflag = false;
            if (ClientRecieveThread!=null&& ClientRecieveThread.IsAlive)
            {
                ClientRecieveThread.Abort();
            }
        }
    }
}