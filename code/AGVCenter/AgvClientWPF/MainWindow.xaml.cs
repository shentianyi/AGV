using AgvClientWPF.Product;
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
using AgvClientWPF.Delivery;
using AGVCenterLib.Model;

namespace AgvClientWPF
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

        private void productOffLineBtn_Click(object sender, RoutedEventArgs e)
        {
            new ProductFinishWindow().Show();
        }

        private void createDevlieryBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreateDeliveryWindow().Show();
        }

        private void createTrayBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreateTrayWindow().Show();
        }

        private void createDeliveryOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreateDeliveryOutStockTaskWindow().Show();
        }

        private void sendDeliveryBtn_Click(object sender, RoutedEventArgs e)
        {
            new SendDeliveryWindow().Show();
        }
    }
}
