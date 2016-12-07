using AgvClientWPF.AgvDeliveryService;
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
    /// CreateDelivery.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDeliveryWindow : Window
    {
        int count = 0;
        public CreateDeliveryWindow()
        {
            InitializeComponent();
        }

        private void createDeliveryBtn_Click(object sender, RoutedEventArgs e)
        {
            count++;
            string nr = deliveryNrTB.Text;

            if (string.IsNullOrEmpty(nr))
            {
                MessageBox.Show("运单号不可为空！"+count.ToString());
            }
        }

        private void deliveryNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.CheckDeliveryExsits();
            }
        }

        private void CheckDeliveryExsits()
        {
            if (!string.IsNullOrEmpty(deliveryNrTB.Text))
            {
                DeliveryServiceClient client = new DeliveryServiceClient();
                if (client.DeliveryExists(deliveryNrTB.Text)) {
                    MessageBox.Show("YES");
                }
                else
                {
                    MessageBox.Show("NO");
                }
            }
        }
    }
}
