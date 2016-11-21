using Brilliantech.Framwork.Utils.ConvertUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Brilliantech.Framwork.Utils.LogUtil;

namespace TcpDemoWPF
{
    /// <summary>
    /// ClientWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow()
        {
            InitializeComponent();
        }
        public static bool runflag = true;
        //private static byte[] buf = new byte[1024];
        Socket tcpClient;
        Thread ClientRecieveThread;
        private void MakeConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!ConnectServer())
            {
                return;
            }

            ClientRecieveThread = new Thread( Listen);
            ClientRecieveThread.IsBackground = true;
            ClientRecieveThread.Start();

        }

        private void Listen()
        {
            while (runflag)
            {
                try
                {
                    byte[] result = new byte[1024];
                    int dataLength = tcpClient.Receive(result);
                    tcpClient.ReceiveTimeout = -1;
                    byte[] MessageBytes = result.Take(dataLength).ToArray();
                    string Receivemeans = ReadMessage.Parser.readMessage(MessageBytes);
                    this.Dispatcher.Invoke(new Action(() => { ReceiveMessageText.AppendText(Receivemeans + "\n"); }));
                }
                catch (SocketException ex)
                {
                    LogUtil.Logger.Error(ex.Message, ex);
                    if (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        sendMsg(currentCmd, true);
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    runflag = false;
                }

            }
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <returns></returns>
        private bool ConnectServer()
        {
            try
            {
                if (tcpClient == null)
                {
                    tcpClient=  new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                if (tcpClient.Connected)
                {
                    return true;
                }
                //   Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse(serverIPTB.Text), int.Parse(serverPortTB.Text)));
                
                MessageBox.Show("连接服务器成功");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
        private void CallingCar_Click(object sender, RoutedEventArgs e)
        {

            #region 
            //string name = "FF FF 12 00 01 01 01 01 05 48 65 6C 6C 6F 0A 0B 0B 0B 01 09 08 0A 0B";
            //byte[] msg =new byte[]{ 0xFF, 0xFF , 0x12 , 0x00, 0x01 , 0x01 , 0x01 , 0x01 , 0x05 , 0x48 , 0x65 , 0x6C , 0x6C , 0x6F , 0x0A , 0x0B , 0x0B , 0x0B , 0x01 , 0x09 , 0x08 , 0x0A , 0x0B};
            #endregion
            byte[] msg = new byte[] { 0x12, 0x00, 0x01, 0x01, 0x01, 0x01, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x0A, 0x0B, 0x0B, 0x0B, 0x01 };
            sendMsg(msg);
        }



        private void CarArriving_Click(object sender, RoutedEventArgs e)
        {
            #region 
            //string name = "FF FF 07 00 01 06 01 01 09 08  0A 0B";
            #endregion
            byte[] msg = new byte[] { 0x07, 0x00, 0x01, 0x06, 0x01, 0x01 };
            sendMsg(msg);
        }

        byte[] currentCmd = null;
        int resendCount = 0;
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBody"></param>
        private void sendMsg(byte[] msgBody, bool isResent = false)
        {
            byte[] msg =null;
            if (isResent)
            {
                msg = msgBody;
                resendCount++;
            }
            else {
                resendCount = 0;
               msg= AGVMessageHelper.GenMsg(msgBody);
            }
            if (resendCount > 2)
            {
                LogUtil.Logger.Error("【发送超时且超出最大发送次数】");
            }
            else {
                currentCmd = msg;
                if (ConnectServer())
                {
                    tcpClient.Send(msg, msg.Length, SocketFlags.None);
                    // tcpClient.ReceiveTimeout = 5000;

                    SendMessageText.AppendText(ReadMessage.Parser.readMessage(msg) + "\n");
                }
            }
        }

        private void CarStart_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 07 00 01 03 01 01 09 08  0A 0B";

            byte[] msg = new byte[] { 0x07, 0x00, 0x01, 0x03, 0x01, 0x01 };
            sendMsg(msg);
        }

        private void CarStatus_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 0D 00 01 05 01 02 01 03 41 01  01 13 7B 09 08 0A 0B";

            byte[] msg = new byte[] { 0x0D, 0x00, 0x01, 0x05, 0x01, 0x02, 0x01, 0x03, 0x41, 0x01,0x01, 0x13, 0x7B };
            sendMsg(msg);
        }

        private void CancelCalling_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 08 00 01 08 01 02 01 09 08 0A 0B";

            byte[] msg = new byte[] { 0x08, 0x00, 0x01, 0x08, 0x01, 0x02, 0x01 };
            sendMsg(msg);
        }

        private void StorageLocation_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 0B 00 01 0A 05 48 65 6C 6C 6F 09 08 0A 0B";
            byte[] msg = new byte[] { 0x0B, 0x00, 0x01, 0x0A, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F };
            sendMsg(msg);

        }



        private void Incoming_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 11 00 01 0C 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0x11, 0x00, 0x01, 0x0C, 0x04, 0x01, 0x13, 0x02, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x01, 0x01 };
            sendMsg(msg);
        }

        private void OutComing_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 13 00 01 0E 02 32 05 05 48 65 6C 6C 6F 0A 0B 0B 0B 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0x13, 0x00, 0x01, 0x0E, 0x02, 0xC8, 0x32,0x2E, 0x0A, 0x0B, 0x0B, 0x0B, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F };
            sendMsg(msg);
        }

        private void OutComingFinish_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 13 00 01 10 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0x13, 0x00, 0x01, 0x10, 0x04, 0x01, 0x13, 0x02, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x01, 0x01 };
            sendMsg(msg);
        }

        private void StackWhole_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 0C 01 02 12 01 01 13 02 32 02 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0x0C, 0x01, 0x02, 0x12, 0x01, 0x01, 0x13, 0x02, 0x32, 0x02, 0x01 };
            sendMsg(msg);
        }

        private void AskStartOrStop_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 09 01 02 14 01 01 13 09 08 0A 0B";
            byte[] msg = new byte[] { 0x09, 0x01, 0x02, 0x14, 0x01, 0x01, 0x13 };
            sendMsg(msg);
        }

        private void StartOrStop_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 09 01 02 16 01 01 13 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0x09, 0x01, 0x02, 0x16, 0x01, 0x01, 0x13, 0x01 };
            sendMsg(msg);
        }

        private void Warning_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 09 01 02 18 01 01 13 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0x09, 0x01, 0x02, 0x18, 0x01, 0x01, 0x13, 0x01 };
            sendMsg(msg);
        }

        private void ReceiveMessageText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SendMessageText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /// 停止线程
            if (ClientRecieveThread != null && ClientRecieveThread.IsAlive)
            {
                ClientRecieveThread.Abort();
            } 

            /// 关闭client
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
        }
    }
}
