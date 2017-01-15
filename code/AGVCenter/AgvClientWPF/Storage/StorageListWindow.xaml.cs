using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
using Brilliantech.Framwork.Utils.LogUtil;

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
                if (RoadMachineIndexCB.SelectedIndex > -1 && ((int)RoadMachineIndexCB.SelectedValue) > -1)
                {
                    q.RoadMachineIndex = (int)RoadMachineIndexCB.SelectedValue;
                }
                if (BoxTypeCB.SelectedIndex > -1 && ((int)BoxTypeCB.SelectedValue) > -1)
                {
                    q.BoxTypeId = (int)BoxTypeCB.SelectedValue;
                }
                if (!string.IsNullOrEmpty(KskNrTB.Text))
                {
                    q.Nr = KskNrTB.Text;
                }

                StorageServiceClient dsc = new StorageServiceClient();
          
                storages = dsc.SearchDetail(q).ToList();
                
                storageDG.ItemsSource = storages;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
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
            List<StorageUniqueItemViewModel> storages= this.pickStorageDG.Items.OfType<StorageUniqueItemViewModel>().ToList();
            //= new List<StorageUniqueItemViewModel>();
            //foreach (var i in storageDG.SelectedItems)
            //{
            //    StorageUniqueItemViewModel storage = i as StorageUniqueItemViewModel;
            //    storages.Add(storage);
            //}
            if (storages.Count > 0)
            {
                new CreatePickListWindow(storages.Select(s => s.UniqItemNr)
                    .ToList()).Show();
            }
        }

        /// <summary>
        /// 添加到picklist中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addToPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            var l = GetSelectedStorages();
            if (l != null)
            {
                foreach (var ll in l)
                {
                    if (!this.pickStorageDG.Items.OfType<StorageUniqueItemViewModel>().Select(s=>s.UniqItemNr).Contains(ll.UniqItemNr))
                    {
                        this.pickStorageDG.Items.Add(ll);
                        this.SetCount();
                    }
                }
            }
        }


        private void SetCount()
        {
            BigCountLab.Content = this.pickStorageDG.Items.OfType<StorageUniqueItemViewModel>().Count(s => s.UniqueItemBoxTypeId == 1);

            SmallCountLab.Content = this.pickStorageDG.Items.OfType<StorageUniqueItemViewModel>().Count(s => s.UniqueItemBoxTypeId == 2);
        }

        /// <summary>
        /// 从picklist中移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeFromPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            var l = GetPickSelectedStorages();
            if (l != null)
            {
                foreach (var ll in l)
                {
                    if (this.pickStorageDG.Items.Contains(ll))
                    {
                        this.pickStorageDG.Items.Remove(ll);
                        this.SetCount();
                    }
                }
            }
        }

        private List<StorageUniqueItemViewModel> GetSelectedStorages()
        {
           return (this.storageDG.ItemsSource as List<StorageUniqueItemViewModel>)
                .Where(s => s.IsSelected == true)
                .ToList();
        }

        private List<StorageUniqueItemViewModel> GetPickSelectedStorages()
        {
            return this.pickStorageDG.SelectedItems.OfType<StorageUniqueItemViewModel>().ToList();
        }


        private void _chkSelected_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox chkSelected = e.OriginalSource as CheckBox;

            if (null == chkSelected)
            {
                return;
            } 

            var cbItem = chkSelected.DataContext as StorageUniqueItemViewModel; 
            bool isChecked = chkSelected.IsChecked.HasValue ? chkSelected.IsChecked.Value : true;
            FrameworkElement templateParent = chkSelected.TemplatedParent is FrameworkElement 
                                                  ? (chkSelected.TemplatedParent as FrameworkElement).TemplatedParent as FrameworkElement
                                                  : null;

            if (templateParent is DataGridColumnHeader)
            {
                List<StorageUniqueItemViewModel> mvm = this.storageDG.ItemsSource as List<StorageUniqueItemViewModel>;
                if (null != mvm)
                {
                    foreach (var sm in mvm)
                    { 
                        sm.IsSelected = isChecked;
                    }
                }

            }
            else if (templateParent is DataGridCell)
            {
                if (null != cbItem && null != this.storageDG.SelectedItems && this.storageDG.SelectedItems.Contains(cbItem))
                {
                    foreach (var otherSelected in this.storageDG.SelectedItems.OfType<StorageUniqueItemViewModel>())
                    {
                        otherSelected.IsSelected = isChecked;
                    }
                }
            }
        }



    }
}
