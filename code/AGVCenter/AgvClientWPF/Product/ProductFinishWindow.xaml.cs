using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
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
            ResultMessage message=new ResultMessage();
            try
            {
                if (CrossCheck())
                {

                    UniqueItemModel item = new UniqueItemModel()
                    {
                        Nr = QrTB.Text,
                        QR=QrTB.Text,
                        KNr = KnrTB.Text,
                        KNrWithYear = KNrWithYearTB.Text,
                        CheckCode = CheckCodeTB.Text,
                        KskNr = KskNrTB.Text,
                        BoxTypeId = BoxTypeLRB.IsChecked == true ?
                        (int)AGVCenterLib.Enum.BoxType.Big : (int)AGVCenterLib.Enum.BoxType.Small
                    };

                    ProductServiceClient ps = new ProductServiceClient();
                    message = ps.CreateUniqItem(item);

                }
            }
            catch (Exception ex)
            {
                message.Content = ex.Message;
                message.MessageType = MessageType.Exception;
            }
            MessageBox.Show(message.Content);
        }
    

        /// <summary>
        /// cross check 电测标签上的二维码和外箱标签上的验证码
        /// </summary>
        /// <returns></returns>
        private bool CrossCheck()
        {
            return true;
        }
    }
}
