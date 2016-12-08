using AgvClientWPF.AgvProductService;
using AgvClientWPF.Helper;
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

namespace AgvClientWPF.Delivery
{
    /// <summary>
    /// CreateTrayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreateTrayWindow : Window
    {
        public CreateTrayWindow()
        {
            InitializeComponent();
        }

        private void refreshTrayNrBtn_Click(object sender, RoutedEventArgs e)
        {
            trayNrTB.Text = UniqueHelper.GenerateUniqString();
        }

        private void checkAndLoadDeliveryBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void deliveryNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

            }
        }

        List<UniqueItemModel> deliveryItems;
        private bool CheckAndLoadDelivery()
        {
            deliveryItems = null;
            if (!string.IsNullOrEmpty(deliveryNrTB.Text))
            {

            }
            return false;
        }

    }
}
