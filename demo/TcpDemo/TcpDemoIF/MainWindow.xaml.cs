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
        private static byte[] buf = new byte[1024];
 Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private void MakeConnect_Click(object sender, RoutedEventArgs e)
        {
         //   Socket SocketTcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            SocketTcp.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
            MessageBox.Show("建立连接成功");

        }
       

        private void CallingCar_Click(object sender, RoutedEventArgs e)
        {
           
           // SocketTcp.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080));
        //    MessageBox.Show("建立连接成功");
            // string name = "0xFF 0xFF  0x 0x01 0x01 0x01 0x0A 0x02  0xFF 0xFE 0x09 0x08 0x07 0x01 0x09 0x09 0x0A 0x0B";
           
            byte[] nameBuf = Encoding.UTF8.GetBytes("hello server");
            SocketTcp.Receive(buf);
            string serverResponse = System.Text.Encoding.UTF8.GetString(buf);
            MessageBox.Show(serverResponse);
            SocketTcp.Send(nameBuf);

            
             
        }

       





    }
}
