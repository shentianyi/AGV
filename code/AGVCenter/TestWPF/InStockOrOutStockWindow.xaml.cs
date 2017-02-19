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
using AGVCenterLib.Service;
using TestWPF.Properties;

namespace TestWPF
{
    /// <summary>
    /// InStockOrOutStockWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InStockOrOutStockWindow : Window
    {
        public InStockOrOutStockWindow()
        {
            InitializeComponent();
        }

        private void inStockBtn_Click(object sender, RoutedEventArgs e)
        {
            StorageService ss = new StorageService(Settings.Default.db);
            ResultMessage message = ss.InStockByUniqItemNr(positionNrTB.Text, checkCodeTB.Text);
            if (message.Success)
            {
                MessageBox.Show("In OK");
            }
            else
            {
                MessageBox.Show(message.Content);
            }
        }

        private void outStockBtn_Click(object sender, RoutedEventArgs e)
        {
            StorageService ss = new StorageService(Settings.Default.db);
            ResultMessage message = ss.OutStockByUniqItemNr(checkCodeTB.Text);
            if (message.Success)
            {
                MessageBox.Show("Out OK");
            }
            else
            {
                MessageBox.Show(message.Content);
            }
        }
    }
}
