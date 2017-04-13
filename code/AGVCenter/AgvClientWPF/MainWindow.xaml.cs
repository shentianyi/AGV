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
using AgvClientWPF.Storage;
using AgvClientWPF.Pick;
using AgvClientWPF.Control;

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
            new ProductFinishWindow().ShowDialog();
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

        private void deliveryTaskMonitorBtn_Click(object sender, RoutedEventArgs e)
        {
            new DeliveryOutStockTaskMonitorWindow().Show();
        }

        private void deliveryListBtn_Click(object sender, RoutedEventArgs e)
        {
            new DeliveryListWindow().Show();
        }

        private void storageListBtn_Click(object sender, RoutedEventArgs e)
        {
            new StorageListWindow().Show();
        }

        private void createPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreatePickListWindow().Show();
        }

        private void createPickListTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreatePickOutStockTaskWindow().Show();
        }

        private void pickListTaskMonitorBtn_Click(object sender, RoutedEventArgs e)
        {
            new PickOutStockTaskMonitorWindow().Show();
        }

        private void pickListBtn_Click(object sender, RoutedEventArgs e)
        {
            new  PickListWindow().Show();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (this.WindowStyle == WindowStyle.SingleBorderWindow)
            //{
            //    this.WindowStyle = WindowStyle.None;
            //}
            //else{
            //    this.WindowStyle = WindowStyle.SingleBorderWindow;
            //}
        }

        private void roadMachineModeBtn_Click(object sender, RoutedEventArgs e)
        {
            new RoadMachineModeWindow().Show();
        }
    }
}
