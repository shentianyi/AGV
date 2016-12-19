using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AgvClientWPF.Properties;

namespace AgvClientWPF.Product
{
    /// <summary>
    /// ProductFinishWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProductFinishWindow : Window
    {
        public ProductFinishWindow()
        {
            InitializeComponent();
        }


        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (CrossCheck())
                {

                    UniqueItemModel item = new UniqueItemModel()
                    {
                        Nr = KskNrTB.Text, //QrTB.Text,
                        QR = QrTB.Text,
                        KNr = KnrTB.Text,
                        KNrWithYear = KNrWithYearTB.Text,
                        CheckCode = CheckCodeTB.Text,
                        KskNr = KskNrTB.Text,
                        BoxTypeId = GetBoxTypeId()
                    };
                    if (item.BoxTypeId != 0)
                    {
                        ProductServiceClient ps = new ProductServiceClient();
                        message = ps.CreateUniqItem(item);

                    }
                }
            }
            catch (Exception ex)
            {
                message.Content = ex.Message;
                message.MessageType = MessageType.Exception;
            }
            if (!string.IsNullOrEmpty(message.Content))
            {
                MessageBox.Show(message.Content);
            }
            this.RestInput();
            QrTB.Focus();
        }

        private void RestInput()
        {
            QrTB.Text = string.Empty;
            KnrTB.Text = string.Empty;
            KNrWithYearTB.Text = string.Empty;
            CheckCodeTB.Text = string.Empty;
            KskNrTB.Text = string.Empty;
        }

        private int GetBoxTypeId()
        {

            if (BoxTypeLRB.IsChecked.Value)
            {
                return (int)AGVCenterLib.Enum.BoxType.Big;
            }
            else if (BoxTypeSRB.IsChecked.Value)
            {
                return (int)AGVCenterLib.Enum.BoxType.Small;
            }
            else
            {
                MessageBox.Show("大小箱设置错误！请设置,\n 1为大箱，2为小箱");
                return 0;
            }
        }

        /// <summary>
        /// cross check 电测标签上的二维码和外箱标签上的验证码
        /// </summary>
        /// <returns></returns>
        private bool CrossCheck()
        {
            if (("1" + QrTB.Text) == CheckCodeTB.Text)
            {
                return true;
            }
            else
            {
                MessageBox.Show("未能通过CrossCheck!");
                RestInput();
                return false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Settings.Default.BoxType == "1")
            {
                BoxTypeLRB.IsChecked = true;
            }
            else if (Settings.Default.BoxType == "2")
            {
                BoxTypeSRB.IsChecked = true;
            }
            else
            {
                BoxTypeLRB.IsChecked = false;
                BoxTypeSRB.IsChecked = false;
                MessageBox.Show("大小箱设置错误！请设置,\n 1为大箱，2为小箱");
            }
        }



        private void AutoChangeFocusTB_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                switch (tb.Name)
                {
                    case "QrTB":
                        KnrTB.Focus();
                        break;
                    case "KnrTB":
                        KNrWithYearTB.Focus();
                        break;
                    case "KNrWithYearTB":
                        CheckCodeTB.Focus();
                        break;
                    case "CheckCodeTB":
                        KskNrTB.Focus();
                        break;
                    case "KskNrTB":
                        QrTB.Focus();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
