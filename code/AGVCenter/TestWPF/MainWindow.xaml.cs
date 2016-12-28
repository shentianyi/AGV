using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWPF
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

        private void createPositionsBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreatePositionWindow().Show();
        }

        private void inStockBtn_Click(object sender, RoutedEventArgs e)
        {
            new InStockOrOutStockWindow().Show();
        }

        private void createRemoteMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            //MessageQueue q = new MessageQueue(".\\private$\\agvtaskqueue");

          MessageQueue q = new MessageQueue("FormatName:Direct=TCP:192.168.0.99\\private$\\agvtaskqueue");
            q.Send(new Message {
                Body = "23",
                Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
            });
        }

        private void autoComTest_Click(object sender, RoutedEventArgs e)
        {
            new AutoComScanTest().Show();
        }
    }
}
