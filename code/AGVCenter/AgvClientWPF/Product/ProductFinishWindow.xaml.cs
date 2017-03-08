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
                            CheckCode2=CheckCode2TB.Text,
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
                if (message.Success)
                {
                    //MessageBox.Show(message.Content);
                    SuccessMsg(message.Content);
                }
                else
                { 
                    MistakerMsg(message.Content);
                }
            }
            this.RestInput();
        }
        private void RestInput()
        {
            QrTB.Text = string.Empty;
            KnrTB.Text = string.Empty;
           // KNrWithYearTB.Text = string.Empty;
            CheckCodeTB.Text = string.Empty;
            CheckCode2TB.Text = string.Empty;
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
                //  MessageBox.Show("大小箱设置错误！请设置,\n 1为大箱，2为小箱");

                MistakerMsg("大小箱设置错误！请设置,\n 1为大箱，2为小箱");
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
            if (("1" + QrTB.Text) == CheckCodeTB.Text && (string.Format(Settings.Default.CheckCode2KskFormat, CheckCode2TB.Text)==KskNrTB.Text))
            {
                return true;
            }
            else
            {
             //   MessageBox.Show("未能通过CrossCheck!");
                MistakerMsg("未能通过CrossCheck! 电测二维码或一维码不匹配！");

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
               // MessageBox.Show("大小箱设置错误！请设置,\n 1为大箱，2为小箱");
                MistakerMsg("大小箱设置错误！请设置,\n 1为大箱，2为小箱");
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
                            CheckCode2TB.Focus();
                        }
                        else
                        {
                            QrTB.SelectAll();
                        }
                        break;
                    case "CheckCode2TB":
                        if (this.InputCheckCheckCode2())
                        {
                            KnrTB.Focus();
                        }
                        else {
                            CheckCode2TB.SelectAll();
                        }
                        break;
                    case "KnrTB":
                        if (this.InputCheckKnr())
                        {
                            CheckCodeTB.Focus();
                        }
                        else
                        {
                            KnrTB.SelectAll();
                        }
                        break;
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
                MistakerMsg("电测二维码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.QrReg).IsMatch(QrTB.Text))
                {
                    MistakerMsg("请扫描电测二维码");
                    return false;
                }
            }
            return true;
        }



        private bool InputCheckCheckCode2()
        {
            if (string.IsNullOrEmpty(CheckCode2TB.Text))
            {
                MistakerMsg("电测一维码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.CheckCode2Reg).IsMatch(CheckCode2TB.Text))
                {
                    MistakerMsg("请扫描电测一维码");
                    return false;
                }
            }
            return true;
        }


        private bool InputCheckKnr()
        {
            if (string.IsNullOrEmpty(KnrTB.Text))
            {
                MistakerMsg("大众生产号不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.KnrReg).IsMatch(KnrTB.Text))
                {
                    MistakerMsg("请扫描大众生产号");
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
                MistakerMsg("验证码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.CheckCodeReg).IsMatch(CheckCodeTB.Text))
                {
                    MistakerMsg("请扫描验证码");
                    return false;
                }
            }
            return true;
        }



        private bool InputCheckKskNr()
        {
            if (string.IsNullOrEmpty(KskNrTB.Text))
            {
                MistakerMsg("KSK号不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.KskRge).IsMatch(KskNrTB.Text))
                {
                    MistakerMsg("请扫描KSK号");
                    return false;
                }
            }
            return true;
        }

        private bool InputCheckLayout()
        {
            if (string.IsNullOrEmpty(LayoutNrTB.Text))
            {
                MistakerMsg("配置代码不可以为空");
                return false;
            }
            else
            {
                if (!new Regex(Settings.Default.LayoutReg).IsMatch(LayoutNrTB.Text))
                {
                    MistakerMsg("请扫描配置代码");
                    return false;
                }
            }
            return true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(GetBoxTypeId().ToString());
            MsgDialog.CMsgDlg(MsgLevel.Info,"",true,null,3).ShowDialog();
            MsgDialog.CMsgDlg(MsgLevel.Mistake, "", true).ShowDialog();
            MsgDialog.CMsgDlg(MsgLevel.Successful, "", true).ShowDialog();
            MsgDialog.CMsgDlg(MsgLevel.Warning, "", true).ShowDialog();
        }

        public void MistakerMsg(string msg)
        {
            MsgDialog.CMsgDlg(MsgLevel.Mistake, msg, true).ShowDialog();
        }

        public void SuccessMsg(string msg)
        {
            MsgDialog.CMsgDlg(MsgLevel.Successful, msg, true).ShowDialog();
        }
    }
}
