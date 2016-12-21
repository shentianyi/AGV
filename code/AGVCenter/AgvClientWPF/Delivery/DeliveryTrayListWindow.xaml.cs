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
using AgvClientWPF.AgvTrayService;
using AgvClientWPF.Helper;

namespace AgvClientWPF.Delivery
{
    /// <summary>
    /// SendDeliveryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DeliveryTrayListWindow : Window
    {
        public DeliveryTrayListWindow()
        {
            InitializeComponent();
        }
        public DeliveryTrayListWindow(string deliveryNr)
        {
            InitializeComponent();
            deliveryNrTB.Text = deliveryNr;
            LoadDeliveryTray(deliveryNr);
        }

        private void loadTrayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (deliveryNrTB.Text.Length > 0)
            {
                this.LoadDeliveryTray(deliveryNrTB.Text);
            }
        }

        private void LoadDeliveryTray(string deliveryNr)
        {
            try
            {
                TrayServiceClient dsc = new TrayServiceClient();
                List<TrayDeliveryViewModel> models = dsc.GetTrayListByDeliveryNr(deliveryNr).ToList();
                deliveryTrayDG.ItemsSource = models;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void printTrayBtn_Click(object sender, RoutedEventArgs e)
        {
            TrayDeliveryViewModel item = this.GetTray();
            if (item != null)
            {
                PrintHelper.PrintTray(item.Nr);
            }
        }

        private TrayDeliveryViewModel GetTray()
        {
            if (deliveryTrayDG.SelectedIndex > -1)
            {
                return deliveryTrayDG.SelectedValue as TrayDeliveryViewModel;
            }
            return null;
        }
    }
}
