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
using System.Text.RegularExpressions;

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
            ProductOffLine();
        }

        private void ProductOffLine()
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (this.InputCheck())
                {
                    if (CrossCheck())
                    {
                        UniqueItemModel item = new UniqueItemModel()
                        {
                            Nr = KskNrTB.Text, //QrTB.Text,
                            QR = QrTB.Text,
                            KNr = KnrTB.Text,
                          //  KNrWithYear = KNrWithYearTB.Text,
                            CheckCode = CheckCodeTB.Text,
                            KskNr = KskNrTB.Text,
                            BoxTypeId = GetBoxTypeId(),
                            PartNr = LayoutNrTB.Text
                        };
                        if (item.BoxTypeId != 0)
                        {
                            ProductServiceClient ps = new ProductServiceClient();
                            message = ps.CreateUniqItem(item);

                        }
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
        }
        private void RestInput()
        {
            QrTB.Text = string.Empty;
            KnrTB.Text = string.Empty;
           // KNrWithYearTB.Text = string.Empty;
            CheckCodeTB.Text = string.Empty;
            KskNrTB.Text = string.Empty;
            LayoutNrTB.Text = string.Empty;
            if (Settings.Default.TestModel)
            {
                KnrTB.Focus();
            }
            else
            {
                QrTB.Focus();
            }
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

        private bool InputCheck()
        {
            if (this.InputCheckQr())
            {
                if (this.InputCheckKnr())
                {
                    //if (this.InputCheckKnrWithYear())
                    //{
                        if (this.InputCheckCheckCode())
                        {
                            if (this.InputCheckKskNr())
                            {
                                if (this.InputCheckLayout())
                                {
                                    return true;
                                }
                            }
                        }
                  //  }
                }
            }
            return false;
        }

        /// <summary>
        /// cross check 电测标签上的二维码和外箱标签上的验证码
        /// </summary>
        /// <returns></returns>
        private bool CrossCheck()
        {
            if (Settings.Default.TestModel)
            {
                return true;
            }
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
            if (Settings.Default.TestModel)
            {
                QrTB.IsEnabled = false;
                KnrTB.Focus();
            }
            else
            {
                QrTB.Focus();
            }
        }




        private void AutoChangeFocusTB_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                if (tb.Text.Length == 0)
                {
                    return;
                }
                switch (tb.Name)
                {
                    case "QrTB":
                        if (this.InputCheckQr())
                        {
                            KnrTB.Focus();
                        }
                        else
                        {
                            QrTB.SelectAll();
                        }
                        break;
                    case "KnrTB":
                        if (this.InputCheckKnr())
                        {
                            // KNrWithYearTB.Focus();
                            CheckCodeTB.Focus();
                        }
                        else
                        {
                            KnrTB.SelectAll();
                        }
                        break;
                    //case "KNrWithYearTB":
                    //    if (this.InputCheckKnrWithYear())
                    //    {
                    //        CheckCodeTB.Focus();
                    //    }
                    //    else
                    //    {
                    //       // KNrWithYearTB.SelectAll();
                    //    }
                    //    break;
                    case "CheckCodeTB":
                        if (this.InputCheckCheckCode())
                        {
                            KskNrTB.Focus();
                        }
                        else
                        {
                            CheckCodeTB.SelectAll();
                        }
                        break;
                    case "KskNrTB":
                        //QrTB.Focus();
                        //if (Settings.Default.TestModel)
                        //{
                        //    KnrTB.Focus();
                        //}
                        //this.ProductOffLine();
                        if (this.InputCheckKskNr())
                        {
                            this.LayoutNrTB.Focus();
                        }
                        else
                        {
                            this.LayoutNrTB.SelectAll();
                        }
                        break;
                    case "LayoutNrTB":
                        if (this.InputCheckLayout())
                        {
                            this.ProductOffLine();
                        }
                        else
                        {
                            this.LayoutNrTB.SelectAll();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private bool InputCheckQr()
        {
            if (string.IsNullOrEmpty(QrTB.Text))
            {
                MessageBox.Show("电测二维码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.QrReg).IsMatch(QrTB.Text))
                {
                    MessageBox.Show("请扫描电测二维码");
                    return false;
                }
            }
            return true;
        }

        private bool InputCheckKnr()
        {
            if (string.IsNullOrEmpty(KnrTB.Text))
            {
                MessageBox.Show("大众生产号不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.KnrReg).IsMatch(KnrTB.Text))
                {
                    MessageBox.Show("请扫描大众生产号");
                    return false;
                }
            }
            return true;    
        }

        private bool InputCheckKnrWithYear()
        {
            //if (string.IsNullOrEmpty(KNrWithYearTB.Text))
            //{
            //    MessageBox.Show("前缀大众生产号不可以为空");
            //    return false;
            //}
            //else
            //{

            //    if (!new Regex(Settings.Default.KnrWithYearReg).IsMatch(KNrWithYearTB.Text))
            //    {
            //        MessageBox.Show("请扫描前缀大众生产号");
            //        return false;
            //    }
            //}
            return true;    
        }

        private bool InputCheckCheckCode()
        {
            if (string.IsNullOrEmpty(CheckCodeTB.Text))
            {
                MessageBox.Show("验证码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.CheckCodeReg).IsMatch(CheckCodeTB.Text))
                {
                    MessageBox.Show("请扫描验证码");
                    return false;
                }
            }
            return true;
        }


        private bool InputCheckKskNr()
        {
            if (string.IsNullOrEmpty(KskNrTB.Text))
            {
                MessageBox.Show("KSK号不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.KskRge).IsMatch(KskNrTB.Text))
                {
                    MessageBox.Show("请扫描KSK号");
                    return false;
                }
            }
            return true;
        }

        private bool InputCheckLayout()
        {
            if (string.IsNullOrEmpty(LayoutNrTB.Text))
            {
                MessageBox.Show("配置代码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.LayoutReg).IsMatch(LayoutNrTB.Text))
                {
                    MessageBox.Show("请扫描配置代码");
                    return false;
                }
            }
            return true;
        }
    }
}
