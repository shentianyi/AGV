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
using AgvClientWPF.AgvPickService;

namespace AgvClientWPF.Pick
{
    /// <summary>
    /// DeliveryListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PickListWindow : Window
    {
        public PickListWindow()
        {
            InitializeComponent();
        }
         
        List<PickListModel> picks;
        private void LoadPickList()
        {
            try
            {
                PickServiceClient dsc = new PickServiceClient();

                picks = dsc.SearchList(new PickListSearchModel()
                {
                    Nr = pickListNrTB.Text
                }, 50).ToList();

                pickListDG.ItemsSource = picks;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPickList();
        }

        private void pickItemListBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetPickListModelItem();
            if (d != null)
            {
                new PickItemListWindow(d.Nr).Show();
            }
        }

        private PickListModel GetPickListModelItem()
        {
            if (this.pickListDG.SelectedIndex > -1)
            {
                return this.pickListDG.SelectedItem as PickListModel;
            }
            else
            {
                return null;
            }
        }

        private void pickStockTaskMonitorBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetPickListModelItem();
            if (d != null)
            {
                new PickOutStockTaskMonitorWindow(d.Nr).Show();
            }
        }
        private void pickCreateStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetPickListModelItem();
            if (d != null)
            {
                new CreatePickOutStockTaskWindow(d.Nr).Show();
            }
        } 

        private void pickListDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var d = GetPickListModelItem();
            if (d != null)
            {
                if (MessageBox.Show("确定？", "确定？", MessageBoxButton.YesNo, MessageBoxImage.Question)==MessageBoxResult.Yes) {
                    PickServiceClient dsc = new PickServiceClient();
                    dsc.DeletePickListForTest(d.Nr);
                    LoadPickList();
                } 
            }
        }

        private void loadPickBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadPickList();
        } 
    }
}
