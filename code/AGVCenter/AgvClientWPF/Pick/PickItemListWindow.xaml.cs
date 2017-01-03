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
using AgvClientWPF.AgvPickService;
using AgvClientWPF.Helper;

namespace AgvClientWPF.Pick
{
    /// <summary>
    /// SendDeliveryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PickItemListWindow : Window
    {
        public PickItemListWindow()
        {
            InitializeComponent();
        }
        public PickItemListWindow(string pickListNr)
        {
            InitializeComponent();
            pickListNrTB.Text = pickListNr;
            LoadPickListStorage(pickListNr);
        }

        private void loadStorageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pickListNrTB.Text.Length > 0)
            {
                this.LoadPickListStorage(pickListNrTB.Text);
            }
        }

        private void LoadPickListStorage(string pickListNr)
        {
            try {
               PickServiceClient dsc = new PickServiceClient();
                List<PickListStorageViewModel> models = dsc.GetPickListStorageByNr(pickListNr).ToList();
                pickListStorageDG.ItemsSource = models;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

         

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
        }

        private void printPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(pickListNrTB.Text))
            {
                PrintHelper.PrintDelivery(pickListNrTB.Text);
            }
        }
    }
}