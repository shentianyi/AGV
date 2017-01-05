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
        private static List<SelectItem> roadMachines = new List<SelectItem>()
        {
            new SelectItem() {Value=-1,Text="全部" },
            new SelectItem() {Value=1,Text="1号" },
            new SelectItem() {Value=2,Text="2号" }
        };

        private static List<SelectItem> boxTypes = new List<SelectItem>()
        {
            new SelectItem() {Value=-1,Text="全部" },
            new SelectItem() {Value=1,Text="大箱" },
            new SelectItem() {Value=2,Text="小箱" }
        };
        public StorageListWindow()
        {
            InitializeComponent();
            RoadMachineIndexCB.ItemsSource = roadMachines;
            BoxTypeCB.ItemsSource = boxTypes;
        }

        private void loadStorageBtn_Click(object sender, RoutedEventArgs e)
        {
           LoadStorageList();
        }
        List<StorageUniqueItemViewModel> storages;
        private void LoadStorageList()
        {
            try
            {
                StorageSearchModel q = new StorageSearchModel();
                if(RoadMachineIndexCB.SelectedIndex>-1 && ((int)RoadMachineIndexCB.SelectedValue) > -1)
                {
                    q.RoadMachineIndex = (int)RoadMachineIndexCB.SelectedValue;
                }
                if (BoxTypeCB.SelectedIndex > -1 && ((int)BoxTypeCB.SelectedValue) > -1)
                {
                    q.BoxTypeId = (int)BoxTypeCB.SelectedValue;
                }

                StorageServiceClient dsc = new StorageServiceClient();
                storages = dsc.SearchDetail(q).ToList();
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
            List<StorageUniqueItemViewModel> storages = new List<StorageUniqueItemViewModel>();
            foreach(var i in storageDG.SelectedItems)
            {
                StorageUniqueItemViewModel storage = i as StorageUniqueItemViewModel;
                storages.Add(storage);
            }
            if (storages.Count > 0)
            {
                new CreateDeliveryWindow(storages.Select(s => s.UniqItemNr).ToList()).Show();
            }
        }

        private void createPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            List<StorageUniqueItemViewModel> storages = new List<StorageUniqueItemViewModel>();
            foreach (var i in storageDG.SelectedItems)
            {
                StorageUniqueItemViewModel storage = i as StorageUniqueItemViewModel;
                storages.Add(storage);
            }
            if (storages.Count > 0)
            {
                new CreatePickListWindow(storages.Select(s => s.UniqItemNr).ToList()).Show();
            }
        }
    }
}
