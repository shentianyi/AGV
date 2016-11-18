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
        private static byte[] buf = new byte[1024];
        Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private void MakeConnect_Click(object sender, RoutedEventArgs e)
        {
            //   Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketTcp.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
            MessageBox.Show("连接服务器成功");

            Thread ClientRecieveThread = new Thread(() =>
            {
                while (runflag)
                {
                  try
                  {
                        SocketTcp.Receive(buf);
                        string serverResponse = System.Text.Encoding.UTF8.GetString(buf).Trim(("\0".ToCharArray()));
                      
                        byte[] MessageBytes = ScaleConvertor.HexStringToHexByte(serverResponse);
                        string means = ReadMessage.Class1.readMessage(MessageBytes);
                        MessageBox.Show(means);
                        LogUtil.Logger.Info(means);                     
                        buf = new byte[1024];
                        serverResponse = "";
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
       

        private void CallingCar_Click(object sender, RoutedEventArgs e)
        {

           
            string name = "FF FF 12 00 01 01 01 01 05 48 65 6C 6C 6F 0A 0B 0B 0B 01  09 08  0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);
            
            SocketTcp.Send(nameBuf,nameBuf.Length,SocketFlags.None);
            name = null;

        }

       

        private void CarArriving_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 07 00 01 06 01 01 09 08  0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }

        private void CarStart_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 07 00 01 03 01 01 09 08  0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }

        private void CarStatus_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 0D 00 01 05 01 02 01 03 41 01  01 13 7B 09 08 0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }

        private void CancelCalling_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 08 00 01 08 01 02 01 09 08 0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }

        private void StorageLocation_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 0B 00 01 0A 05 48 65 6C 6C 6F 09 08 0A 0B";

            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;

        }

          

        private void Incoming_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 11 00 01 0C 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }

        private void OutComing_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 13 00 01 0E 02 32 05 05 48 65 6C 6C 6F 0A 0B 0B 0B 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }

        private void OutComingFinish_Click(object sender, RoutedEventArgs e)
        {
            string name = "FF FF 13 00 01 10 04 01 13 02 05 48 65 6C 6C 6F 01 01 09 08 0A 0B";
            byte[] nameBuf = Encoding.UTF8.GetBytes(name);

            SocketTcp.Send(nameBuf, nameBuf.Length, SocketFlags.None);
            name = null;
        }
    }
}
