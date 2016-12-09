using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvDeliveryService;
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
            CheckAndLoadDelivery();
        }

        private void deliveryNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CheckAndLoadDelivery();
            }
        }

        List<UniqueItemModel> deliveryItems;
        private bool CheckAndLoadDelivery()
        {
            deliveryItems = null;
            if (!string.IsNullOrEmpty(deliveryNrTB.Text))
            {
                DeliveryServiceClient dsc = new DeliveryServiceClient();
                if (dsc.DeliveryExists(deliveryNrTB.Text))
                {
                    deliveryItems = dsc.GetDeliveryUniqItemsByNr(deliveryNrTB.Text).ToList();
                    trayNrTB.IsEnabled = false;
                    trayNrTB.Text = UniqueHelper.GenerateUniqString();
                    checkCodeTB.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("运单不存在");
                    return false;
                }
            }
            return false;
        }

        private void createTrayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(deliveryNrTB.Text))
            {
                MessageBox.Show("运单号不可为空！");
                return;
            }

            if (string.IsNullOrEmpty(trayNrTB.Text))
            {
                MessageBox.Show("托盘号不可为空！");
                return;
            }


            if (trayItemDG.Items.Count == 0)
            {
                MessageBox.Show("托盘项不可为空，请添加！");
                return;
            }


            DeliveryServiceClient dsc = new DeliveryServiceClient();

            ResultMessage message = dsc.CreateTray(
                deliveryNrTB.Text,
                trayNrTB.Text,
                this.GetCurrentUniqItems().Select(s => s.Nr).ToArray());

            if (message.Success)
            {
                MessageBox.Show("托盘创建成功");
                trayItemDG.Items.Clear();
                trayNrTB.Text = string.Empty;
                checkCodeTB.Text = string.Empty;
            }
            else
            {
                MessageBox.Show(message.Content);
            }
        }

        private void checkCodeText_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter && !string.IsNullOrEmpty(checkCodeTB.Text))
            {
                if (CanItemAddToTray())
                {
                    ProductServiceClient psClient = new ProductServiceClient();
                    UniqueItemModel item = psClient.FindUniqItemByNr(checkCodeTB.Text);

                    if (item != null)
                    {
                        trayItemDG.Items.Add(item);
                    }
                }
            }
        }

        private bool CanItemAddToTray()
        {
            if (GetCurrentUniqItem(checkCodeTB.Text) == null)
            {

                DeliveryServiceClient dsc = new DeliveryServiceClient();
                ResultMessage message = dsc.CanItemAddToTray(checkCodeTB.Text, deliveryNrTB.Text);
                if (message.Success)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(message.Content);
                    return false;
                }
            }
            else {
                MessageBox.Show("产品已在托中，不可重复添加！");
                return false;
            }
        }

        private UniqueItemModel GetCurrentUniqItem(string checkCode)
        {
            foreach(var i in trayItemDG.Items)
            {
                if ((i as UniqueItemModel).CheckCode == checkCode)
                    return i as UniqueItemModel;
            }
            return null;
        }

        List<UniqueItemModel> GetCurrentUniqItems()
        {
            List<UniqueItemModel> items = new List<UniqueItemModel>();
            foreach (var i in trayItemDG.Items)
            {
                items.Add(i as UniqueItemModel);
            }
            return items;
        }

        private void removeTrayItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.trayItemDG.SelectedIndex > -1)
            {
                this.trayItemDG.Items.RemoveAt(this.trayItemDG.SelectedIndex);
            }
        }
    }
}
