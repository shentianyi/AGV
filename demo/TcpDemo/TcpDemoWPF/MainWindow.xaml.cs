using Brilliantech.Framwork.Utils.LogUtil;
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


        Socket SocketTcpAccept = SocketTcp.Accept();
        while (true)
        {
                try
                {
                    byte[] serversay = Encoding.UTF8.GetBytes("Can I hellp u");

                    SocketTcpAccept.Send(serversay);
                    //serversay = null;
                    SocketTcpAccept.Receive(result);
                    string serverResponse = System.Text.Encoding.UTF8.GetString(result);
                    MessageBox.Show(serverResponse);
                    //SocketTcpAccept.Send(Encoding.Unicode.GetBytes(serverResponse));
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }
    }
}
