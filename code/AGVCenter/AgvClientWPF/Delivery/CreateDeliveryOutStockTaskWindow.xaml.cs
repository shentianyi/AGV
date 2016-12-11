using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvDeliveryService;

namespace AgvClientWPF.Delivery
{
    /// <summary>
    /// DeliveryCreateOutStockTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDeliveryOutStockTaskWindow : Window
    {
        public CreateDeliveryOutStockTaskWindow()
        {
            InitializeComponent();
        }

        private void loadStorageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (deliveryNrTB.Text.Length > 0)
            {
                this.LoadDeliveryStorage();
            }
        }

        private void LoadDeliveryStorage()
        {
            DeliveryServiceClient dsc = new DeliveryServiceClient();
            List<DeliveryStorageViewModel> models = dsc.GetDeliveryStorageByNr(deliveryNrTB.Text).ToList();
            deliveryStorageDG.ItemsSource=models;
        }

        private void createOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultMessage message = new DeliveryServiceClient().CreateOutStockTaskByNr(this.deliveryNrTB.Text);
            if (message.Success)
            {
                MessageBox.Show("出库已开始，请等待...");
            }
            else
            {
                MessageBox.Show(message.Content);
            }
        }
    }
}
