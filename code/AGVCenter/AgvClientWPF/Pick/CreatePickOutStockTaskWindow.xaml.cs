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

namespace AgvClientWPF.Pick
{
    /// <summary>
    /// DeliveryCreateOutStockTaskWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreatePickOutStockTaskWindow : Window
    {
        public CreatePickOutStockTaskWindow()
        {
            InitializeComponent();
        }
        public CreatePickOutStockTaskWindow(string pickListNr)
        {
            InitializeComponent();
            this.pickListNrTB.Text = pickListNr;

            this.LoadPickListStorage();
        }

        private void loadStorageBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pickListNrTB.Text.Length > 0)
            {
                this.LoadPickListStorage();
            }
        }

        private void LoadPickListStorage()
        {
            try
            {
                PickServiceClient dsc = new PickServiceClient();
                List<PickListStorageViewModel> models = dsc.GetPickListStorageByNr(pickListNrTB.Text).ToList();
                pickListStorageDG.ItemsSource = models;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void createOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResultMessage message = new PickServiceClient().CreateOutStockTaskByNr(this.pickListNrTB.Text);
                if (message.Success)
                {
                    MessageBox.Show("出库已开始，请等待...");
                }
                else
                {
                    MessageBox.Show(message.Content);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
