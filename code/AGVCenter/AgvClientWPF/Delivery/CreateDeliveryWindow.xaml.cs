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
    /// CreateDelivery.xaml 的交互逻辑
    /// </summary>
    public partial class CreateDeliveryWindow : Window
    { 
        public CreateDeliveryWindow()
        {
            InitializeComponent();
        }

        public CreateDeliveryWindow(List<string> uniqNrs)
        {
            InitializeComponent();
            this.deliveryNrTB.Text = UniqueHelper.GenerateUniqString();
            foreach(var s in uniqNrs)
            {
                AddItemToDelivery(s);
            }
        }

        private void createDeliveryBtn_Click(object sender, RoutedEventArgs e)
        {
            string nr = deliveryNrTB.Text;

            if (string.IsNullOrEmpty(nr))
            {
                MessageBox.Show("运单号不可为空！");
                return;
            }

            if (deliveryItemDG.Items.Count == 0)
            {
                MessageBox.Show("运单项不可为空，请添加！");
                return;
            }

            DeliveryServiceClient dsc = new DeliveryServiceClient();
            ResultMessage message = dsc.CreateDelivery(nr, this.GetCurrentUniqItems().Select(s => s.Nr).ToArray());
            if (message.Success)
            {
                MessageBox.Show("运单创建成功");
                deliveryItemDG.Items.Clear();
            }
            else
            {
                MessageBox.Show(message.Content);
            }
        }

        private void deliveryNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.CheckDeliveryExsits();
            }
        }

        private bool CheckDeliveryExsits()
        {
            DeliveryServiceClient client = new DeliveryServiceClient();
            return client.DeliveryExists(deliveryNrTB.Text);
        }

        private void generateDeliveryNrBtn_Click(object sender, RoutedEventArgs e)
        {
            this.deliveryNrTB.Text = UniqueHelper.GenerateUniqString();
        }

        private void addDeliveryItemBtn_Click(object sender, RoutedEventArgs e)
        {
            AddItemToDelivery(uniqItemNrTB.Text);
        }
        
        private void AddItemToDelivery(string uniqItemNr)
        {
            if(!string.IsNullOrEmpty(uniqItemNr))
            {
                if (GetCurrentUniqItem(uniqItemNr) == null)
                {
                    DeliveryServiceClient dsClient = new DeliveryServiceClient();
                    ProductServiceClient psClient = new ProductServiceClient();

                    ResultMessage message = dsClient.CanItemAddToDelivery(uniqItemNr);
                    if (message.Success)
                    {
                         UniqueItemModel item = psClient.FindUniqItemByNr(uniqItemNr);
                        if (item != null)
                        {
                            deliveryItemDG.Items.Add(item);
                            uniqItemNrTB.Text = string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show(message.Content);
                        uniqItemNrTB.Text = string.Empty;
                    }
                }
                else
                {
                    MessageBox.Show("运单项不可重复加入此运单");
                    uniqItemNrTB.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 在当前运单中到UniqItem
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        private  UniqueItemModel GetCurrentUniqItem(string nr)
        {
            foreach(var i in deliveryItemDG.Items)
            {
                if((i as  UniqueItemModel).Nr == nr)
                {
                    return i as  UniqueItemModel;
                }
            }
            return null;
            //return deliveryItemDG.ItemsSource==null ? null :(deliveryItemDG.ItemsSource as List<UniqueItemModel>).FirstOrDefault(s => s.Nr == nr);
        }

        List<UniqueItemModel> GetCurrentUniqItems()
        {
            List<UniqueItemModel> items = new List<UniqueItemModel>();
            foreach(var i in deliveryItemDG.Items)
            {
                items.Add(i as UniqueItemModel);
            }
            return items;
        }

        private void uniqItemNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter && !string.IsNullOrEmpty(uniqItemNrTB.Text))
            {
                this.AddItemToDelivery(uniqItemNrTB.Text);
            }
        }

        private void removeDeliveryItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.deliveryItemDG.SelectedIndex > -1)
            {
                this.deliveryItemDG.Items.RemoveAt(this.deliveryItemDG.SelectedIndex);
            }
        }
    }
}
