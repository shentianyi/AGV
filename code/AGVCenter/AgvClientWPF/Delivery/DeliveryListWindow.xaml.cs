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
using AGVCenterLib.Model.SearchModel;
using AgvClientWPF.AgvDeliveryService;

namespace AgvClientWPF.Delivery
{
    /// <summary>
    /// DeliveryListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeliveryListWindow : Window
    {
        public DeliveryListWindow()
        {
            InitializeComponent();
        }

        private void loadStorageBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadDeliveryList();
        }

        private void LoadDeliveryList()
        {
            DeliveryServiceClient dsc = new DeliveryServiceClient();

            dsc.SearchList(new DeliverySearchModel() {
                Nr=deliveryNrTB.Text
            }, 50);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDeliveryList();
        }
    }
}
