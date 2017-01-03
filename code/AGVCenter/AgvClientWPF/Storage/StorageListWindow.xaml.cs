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
using AgvClientWPF.AgvStorageService;
using AgvClientWPF.Delivery;
using AgvClientWPF.Pick;

namespace AgvClientWPF.Storage
{
    /// <summary>
    /// DeliveryListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StorageListWindow : Window
    {
        public StorageListWindow()
        {
            InitializeComponent();
        }

        private void loadStorageBtn_Click(object sender, RoutedEventArgs e)
        {
           LoadStorageList();
        }
        List<StorageModel> storages;
        private void LoadStorageList()
        {
            try
            {
                StorageServiceClient dsc = new StorageServiceClient();
                storages = dsc.GetAll().ToList();
                storageDG.ItemsSource = storages;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStorageList();
        }

        private void createDeliveryListBtn_Click(object sender, RoutedEventArgs e)
        {
            List<StorageModel> storages = new List<StorageModel>();
            foreach(var i in storageDG.SelectedItems)
            {
                StorageModel storage = i as StorageModel;
                storages.Add(storage);
            }
            if (storages.Count > 0)
            {
                new CreateDeliveryWindow(storages.Select(s => s.UniqItemNr).ToList()).Show();
            }
        }

        private void createPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            List<StorageModel> storages = new List<StorageModel>();
            foreach (var i in storageDG.SelectedItems)
            {
                StorageModel storage = i as StorageModel;
                storages.Add(storage);
            }
            if (storages.Count > 0)
            {
                new CreatePickListWindow(storages.Select(s => s.UniqItemNr).ToList()).Show();
            }
        }
    }
}
