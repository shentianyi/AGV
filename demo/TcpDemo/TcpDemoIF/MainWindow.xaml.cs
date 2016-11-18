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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Brilliantech.Framwork.Utils.ConvertUtil;
using Brilliantech.Framwork.Utils.LogUtil;
using ReadMessage;

namespace TcpDemoIF
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
        public static bool runflag = true;
        //private static byte[] buf = new byte[1024];
        Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Thread ClientRecieveThread;
        private void MakeConnect_Click(object sender, RoutedEventArgs e)
        {
            if (!ConnectServer())
            {
                return;
            }
            
          ClientRecieveThread = new Thread(() =>
            {
                while (runflag)
                {
                 try
                  {
                        // SocketTcp.Receive(buf);
                        // string serverResponse = System.Text.Encoding.UTF8.GetString(buf).Trim(("\0".ToCharArray()));

                        // byte[] MessageBytes = ScaleConvertor.HexStringToHexByte(serverResponse);
                        // string Receivemeans = ReadMessage.Parser.readMessage(MessageBytes);
                        // this.Dispatcher.Invoke(new Action(() => { ReceiveMessageText.AppendText(Receivemeans+"\n"); })); 
                        //// MessageBox.Show(means);
                        //// LogUtil.Logger.Info(Receivemeans);                     
                        // buf = new byte[1024];
                        // serverResponse = "";
                        // Receivemeans = "";

                        
                        byte[] buf = new byte[1024];
                        int dataLength = SocketTcp.Receive(buf);
                        byte[] MessageBytes = buf.Take(dataLength).ToArray();
                        string Receivemeans = ReadMessage.Parser.readMessage(MessageBytes);
                        this.Dispatcher.Invoke(new Action(() => { ReceiveMessageText.AppendText(Receivemeans + "\n"); }));
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.Message);
                        runflag = false;
                    }
                   
                }
            });
            ClientRecieveThread.Start();

        }

        private bool ConnectServer()
        {
            try
            {
                //   Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SocketTcp.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
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


            //string name = "FF FF 12 00 01 01 01 01 05 48 65 6C 6C 6F 0A 0B 0B 0B 01 09 08 0A 0B";
            //byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            //byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            //string SendMeans = ReadMessage.Class1.readMessage(SendMessageBytes);
            //SendMessageText.AppendText(SendMeans+"\n");
            //SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            //nameBuf = null;
            //SendMeans = null;
            //name = null;

            //byte[] msg =new byte[]{ 0xFF, 0xFF , 0x12 , 0x00, 0x01 , 0x01 , 0x01 , 0x01 , 0x05 , 0x48 , 0x65 , 0x6C , 0x6C , 0x6F , 0x0A , 0x0B , 0x0B , 0x0B , 0x01 , 0x09 , 0x08 , 0x0A , 0x0B};
            
            byte[] msg = new byte[] { 0x12, 0x00, 0x01, 0x01, 0x01, 0x01, 0x05, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x0A, 0x0B, 0x0B, 0x0B, 0x01};
            sendMsg(msg);
        }

       
       
        private void CarArriving_Click(object sender, RoutedEventArgs e)
        {
            //string name = "FF FF 07 00 01 06 01 01 09 08  0A 0B";

            //byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            //byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            //string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            //this.Dispatcher.Invoke(new Action(() => { SendMessageText.AppendText(SendMeans + "\n"); }));

            //SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            //nameBuf = null;
            //SendMeans = null;
            //name = null;

            byte[] msg = new byte[] { 0x07, 0x00, 0x01, 0x06, 0x01, 0x01 };
            sendMsg(msg);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgBody"></param>
        private void sendMsg(byte[] msgBody)
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

            SocketTcp.Send(msg, msg.Length, SocketFlags.None);
            SendMessageText.AppendText(ReadMessage.Parser.readMessage(msg) + "\n");
        }

        private void CarStart_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 07 00 01 03 01 01 09 08  0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void CarStatus_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 0D 00 01 05 01 02 01 03 41 01  01 13 7B 09 08 0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void CancelCalling_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 08 00 01 08 01 02 01 09 08 0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void StorageLocation_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 0B 00 01 0A 05 48 65 6C 6C 6F 09 08 0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;

        }

          

        private void Incoming_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 11 00 01 0C 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void OutComing_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 13 00 01 0E 02 32 05 05 48 65 6C 6C 6F 0A 0B 0B 0B 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void OutComingFinish_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 13 00 01 10 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void StackWhole_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 0C 01 02 12 01 01 13 02 32 02 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void AskStartOrStop_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 09 01 02 14 01 01 13 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void StartOrStop_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 09 01 02 16 01 01 13 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void Warning_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 09 01 02 18 01 01 13 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            byte[] SendMessageBytes = ScaleConvertor.HexStringToHexByte(name);
            string SendMeans = ReadMessage.Parser.readMessage(SendMessageBytes);
            SendMessageText.AppendText(SendMeans+"\n");
            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            nameBuf = null;
            SendMeans = null;
            name = null;
        }

        private void ReceiveMessageText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SendMessageText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            runflag = false;
            if (ClientRecieveThread != null && ClientRecieveThread.IsAlive)
            {
                ClientRecieveThread.Abort();
            }
        }
    }
}
