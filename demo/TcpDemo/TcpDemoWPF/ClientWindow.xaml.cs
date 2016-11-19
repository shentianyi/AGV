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
                    byte[] MessageBytes = result.Take(dataLength).ToArray();
                    string Receivemeans = ReadMessage.Parser.readMessage(MessageBytes);
                    this.Dispatcher.Invoke(new Action(() => { ReceiveMessageText.AppendText(Receivemeans + "\n"); }));
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                    runflag = false;
                }

            }
        }

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
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
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

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBody"></param>
        private void sendMsg(byte[] msgBody)
        {
            byte[] msg = AGVMessageHelper.GenMsg(msgBody);
            if (ConnectServer())
            {
                tcpClient.Send(msg, msg.Length, SocketFlags.None);
                SendMessageText.AppendText(ReadMessage.Parser.readMessage(msg) + "\n");
            }
        }

        private void CarStart_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 07 00 01 03 01 01 09 08  0A 0B";

            byte[] msg = new byte[] { 0X07, 0X00, 0X01, 0X03, 0X01, 0X01 };
            sendMsg(msg);
        }

        private void CarStatus_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 0D 00 01 05 01 02 01 03 41 01  01 13 7B 09 08 0A 0B";

            byte[] msg = new byte[] { 0X0D, 0X00, 0X01, 0X05, 0X01, 0X02, 0X01, 0X03, 0X41, 0X01,0X01, 0X13, 0X7B };
            sendMsg(msg);
        }

        private void CancelCalling_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 08 00 01 08 01 02 01 09 08 0A 0B";

            byte[] msg = new byte[] { 0X08, 0X00, 0X01, 0X08, 0X01, 0X02, 0X01 };
            sendMsg(msg);
        }

        private void StorageLocation_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 0B 00 01 0A 05 48 65 6C 6C 6F 09 08 0A 0B";
            byte[] msg = new byte[] { 0X0B, 0X00, 0X01, 0X0A, 0X05, 0X48, 0X65, 0X6C, 0X6C, 0X6F };
            sendMsg(msg);

        }



        private void Incoming_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 11 00 01 0C 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0X11, 0X00, 0X01, 0X0C, 0X04, 0X01, 0X13, 0X02, 0X05, 0X48, 0X65, 0X6C, 0X6C, 0X6F, 0X01, 0X01 };
            sendMsg(msg);
        }

        private void OutComing_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 13 00 01 0E 02 32 05 05 48 65 6C 6C 6F 0A 0B 0B 0B 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0X13, 0X00, 0X01, 0X0E, 0X02, 0X32, 0X05, 0X05, 0X48, 0X65, 0X6C, 0X6C, 0X6F, 0X0A, 0X0B, 0X0B, 0X0B, 0X01 };
            sendMsg(msg);
        }

        private void OutComingFinish_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 13 00 01 10 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0X13, 0X00, 0X01, 0X10, 0X04, 0X01, 0X13, 0X02, 0X05, 0X48, 0X65, 0X6C, 0X6C, 0X6F, 0X01, 0X01 };
            sendMsg(msg);
        }

        private void StackWhole_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 0C 01 02 12 01 01 13 02 32 02 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0X0C, 0X01, 0X02, 0X12, 0X01, 0X01, 0X13, 0X02, 0X32, 0X02, 0X01 };
            sendMsg(msg);
        }

        private void AskStartOrStop_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 09 01 02 14 01 01 13 09 08 0A 0B";
            byte[] msg = new byte[] { 0X09, 0X01, 0X02, 0X14, 0X01, 0X01, 0X13 };
            sendMsg(msg);
        }

        private void StartOrStop_Click(object sender, RoutedEventArgs e)
        {
           // string name = "FF FF 09 01 02 16 01 01 13 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0X09, 0X01, 0X02, 0X16, 0X01, 0X01, 0X13, 0X01 };
            sendMsg(msg);
        }

        private void Warning_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 09 01 02 18 01 01 13 01 09 08 0A 0B";
            byte[] msg = new byte[] { 0X09, 0X01, 0X02, 0X18, 0X01, 0X01, 0X13, 0X01 };
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
