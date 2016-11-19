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
        Socket tcpServer;
        Thread ClientRecieveThread;
        bool runflag = true;

        Dictionary<string, Socket> clients = new Dictionary<string, Socket>();
        Dictionary<string, Thread> clientThreads = new Dictionary<string, Thread>();


        private void MakeConnection_Click(object sender, RoutedEventArgs e)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpServer.Bind(new IPEndPoint(ip, myProt));  //绑定IP地址：端口  
            tcpServer.Listen(10);    //设定最多10个排队连接请求  

            MessageBox.Show(string.Format("启动监听{0}成功", tcpServer.LocalEndPoint.ToString()));
            MakeConnection.Content = "【服务器已启动】";
            // SocketTcpAccept.Send(serversay);
            ClientRecieveThread = new Thread(ListenConnnect);
            ClientRecieveThread.IsBackground = true;
            ClientRecieveThread.Start();
        }

        private void ListenConnnect()
        {
            while (runflag)
            {
                Socket client = tcpServer.Accept();
                try
                {
                    this.Dispatcher.Invoke(new Action(() => { clientLB.Items.Add(client.RemoteEndPoint.ToString()); }));
                    string key = GetClientKey(client);

                    clients.Add(key, client);
                    Thread clientThread = new Thread(ListenClientMsg);
                    clientThread.IsBackground = true;
                    clientThread.Start(client);
                    clientThreads.Add(key, clientThread);

                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    LogUtil.Logger.Info(ee.Message);
                    RemoveClient(client);
                }

            }

        }

        private void ListenClientMsg(object cliento)
        {
            Socket client = cliento as Socket;
            try
            {
                while (true)
                {
                    byte[] result = new byte[1024];
                    int dataLength = client.Receive(result);
                    if (dataLength > 0)
                    {
                        byte[] MessageBytes = result.Take(dataLength).ToArray();
                        string Receivemeans = ReadMessage.Parser.readMessage(MessageBytes);
                        this.Dispatcher.Invoke(new Action(() => { ReceiveText.AppendText(Receivemeans + "\n"); }));

                        LogUtil.Logger.Info("【数据】"+ScaleConvertor.HexBytesToString(MessageBytes));
                        LogUtil.Logger.Info("【解析】" + Receivemeans);

                        #region parse
                        switch (MessageBytes[5])
                        {
                            case 0x01://呼唤小车
                                {
                                    string name = "08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 02 01 01 01";

                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)06://小车到达
                                {
                                    string name = "06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 07 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)03://小车出发
                                {
                                    string name = "06 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 04 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)08://取消小车呼唤
                                {
                                    string name = "08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 09 02 02 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)10://获取货物库位信息
                                {
                                    string name = "11 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0B 04 01 05 48 65 6C 6C 6F 01 02 03 04";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)12://入库完成指令0X0C
                                {
                                    string name = "08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0D 04 01 13";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)13://出库指令0X0D 
                                {

                                    string name = "07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)14://出库指令0X0E
                                {

                                    string name = "07 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 0F 02 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)16://出库完成指令0X10
                                {

                                    string name = "08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 11 02 01 13";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)18://码垛（整个托盘）完成指令0X12
                                {
                                    string name = "08 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 13 01 01 13";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;

                                }
                            case (byte)20://请求启动或停止设备指令0X14
                                {
                                    string name = "09 " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 15 01 01 13 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)22://启动或停止设备指令0X16
                                {
                                    string name = "0B " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 17 01 01 13 01 02 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }
                            case (byte)24://警报指令0X19
                                {

                                    string name = "0B " + MessageBytes[3].ToString("X2") + " " + MessageBytes[4].ToString("X2") + " 19 01 01 13 01 02 01";
                                    byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
                                    sendMsgToClient(GetClientKey(client), SendMessageBytes);

                                    break;
                                }



                            default: break;

                        }
                        #endregion
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                LogUtil.Logger.Info(ee.Message);
                RemoveClient(client);
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="clientIP"></param>
        /// <param name="msgBody"></param>
        private void sendMsgToClient(string clientIP, byte[] msgBody)
        {
            byte[] msg = AGVMessageHelper.GenMsg(msgBody);
            clients[clientIP].Send(msg, msg.Length, SocketFlags.None);
            string SendMeans = ReadMessage.Parser.readMessage(msg);
            this.Dispatcher.Invoke(new Action(() => { SendText.AppendText(SendMeans + "\n"); }));
        }

        /// <summary>
        /// 移除Client
        /// </summary>
        /// <param name="client"></param>
        private void RemoveClient(Socket client)
        {
            string key = GetClientKey(client);
            if (this.clients.Keys.Contains(key))
            {
                if (this.clients[key].Connected)
                {
                    this.clients[key].Close();
                }
                this.clients.Remove(key);
                this.Dispatcher.Invoke(new Action(() =>
                {
                    clientLB.Items.Remove(key);
                }));

            }

            if (this.clientThreads.Keys.Contains(key))
            {
                if (this.clientThreads[key].IsAlive)
                {
                    this.clientThreads[key].Abort();
                }
                this.clientThreads.Remove(key);
            }
        }

        /// <summary>
        /// 窗口关闭时停止线程等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /// 停止Client监听
            foreach (var t in clientThreads)
            {
                if (t.Value.IsAlive)
                {
                    t.Value.Abort();
                }
            }

            /// 停止线程
            if (ClientRecieveThread != null && ClientRecieveThread.IsAlive)
            {
                ClientRecieveThread.Abort();
            }

            /// 关闭server
            if (tcpServer != null)
            {
                tcpServer.Close();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new ClientWindow().Show();
        }

        private string GetClientKey(Socket client)
        {
            return client.RemoteEndPoint.ToString();
        }
    }
}