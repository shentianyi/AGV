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
using AGVCenterLib.Model.ViewModel;
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
        List<DeliveryModel> deliveries;
        private void LoadDeliveryList()
        {
            try
            {
                DeliveryServiceClient dsc = new DeliveryServiceClient();

                deliveries = dsc.SearchList(new DeliverySearchModel()
                {
                    Nr = deliveryNrTB.Text
                }, 50).ToList();

                deliveryDG.ItemsSource = deliveries;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDeliveryList();
        }

        private void deliveryItemListBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetDeliveryModelItem();
            if (d != null)
            {
                new DeliveryItemListWindow(d.Nr).Show();
            }
        }

        private DeliveryModel GetDeliveryModelItem()
        {
            if (this.deliveryDG.SelectedIndex > -1)
            {
                return this.deliveryDG.SelectedItem as DeliveryModel;
            }
            else
            {
                return null;
            }
        }

        private void deliveryStockTaskMonitorBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetDeliveryModelItem();
            if (d != null)
            {
                new DeliveryOutStockTaskMonitorWindow(d.Nr).Show();
            }
        }
        private void deliveryCreateStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetDeliveryModelItem();
            if (d != null)
            {
                new CreateDeliveryOutStockTaskWindow(d.Nr).Show();
            }
        }

        private void deliveryCreateTrayBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetDeliveryModelItem();
            if (d != null)
            {
                new CreateTrayWindow(d.Nr).Show();
            }
        }

        private void deliveryTrayListBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetDeliveryModelItem();
            if (d != null)
            {
                new DeliveryTrayListWindow(d.Nr).Show();
            }
        }

        private void deliveryDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetDeliveryModelItem();
            if (d != null)
            {
                if (MessageBox.Show("确定？", "确定？", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes) {
                    DeliveryServiceClient dsc = new DeliveryServiceClient();
                    dsc.DeleteDeliveryForTest(d.Nr);
                    LoadDeliveryList();
                } 
            }
        }
        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    deliveries.LastOrDefault().Nr = "sdfff";
        //    var d = new DeliveryModel() { Nr = "ssssss" };
        //    deliveries.Add(d);
        //    deliveryDG.Items.Refresh();
        //}

    }
}
