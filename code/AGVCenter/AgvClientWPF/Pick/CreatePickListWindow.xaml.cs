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
using AgvClientWPF.AgvPickService;

namespace AgvClientWPF.Pick
{
    /// <summary>
    /// CreateDelivery.xaml 的交互逻辑
    /// </summary>
    public partial class CreatePickListWindow : Window
    { 
        public CreatePickListWindow()
        {
            InitializeComponent();
        }

        public CreatePickListWindow(List<string> uniqNrs)
        {
            InitializeComponent();
            this.pickListNrTB.Text = UniqueHelper.GenerateUniqString();
            foreach(var s in uniqNrs)
            {
                AddItemToPickList(s);
            }
        }

        private void createPickListBtn_Click(object sender, RoutedEventArgs e)
        {
            string nr = pickListNrTB.Text;

            if (string.IsNullOrEmpty(nr))
            {
                MessageBox.Show("择货单号不可为空！");
                return;
            }

            if (pickListItemDG.Items.Count == 0)
            {
                MessageBox.Show("择货单项不可为空，请添加！");
                return;
            }

            PickServiceClient dsc = new PickServiceClient();
            ResultMessage message = dsc.CreatePickList(nr, this.GetCurrentUniqItems().Select(s => s.Nr).ToArray());
            if (message.Success)
            {
                MessageBox.Show("择货单创建成功");
                pickListItemDG.Items.Clear();
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
                this.CheckPickListExsits();
            }
        }

        private bool CheckPickListExsits()
        {
            PickServiceClient client = new PickServiceClient();
            return client.PickListExists(pickListNrTB.Text);
        }

        private void generatePickListNrBtn_Click(object sender, RoutedEventArgs e)
        {
            this.pickListNrTB.Text = UniqueHelper.GenerateUniqString();
        }

        private void addDeliveryItemBtn_Click(object sender, RoutedEventArgs e)
        {
            AddItemToPickList(uniqItemNrTB.Text);
        }
        
        private void AddItemToPickList(string uniqItemNr)
        {
            if(!string.IsNullOrEmpty(uniqItemNr))
            {
                if (GetCurrentUniqItem(uniqItemNr) == null)
                {
                    PickServiceClient dsClient = new PickServiceClient();
                    ProductServiceClient psClient = new ProductServiceClient();

                    ResultMessage message = dsClient.CanItemAddToPickList(uniqItemNr);
                    if (message.Success)
                    {
                         UniqueItemModel item = psClient.FindUniqItemByNr(uniqItemNr);
                        if (item != null)
                        {
                            pickListItemDG.Items.Add(item);
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
                    MessageBox.Show("择货单项不可重复加入此择货单");
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
            foreach(var i in pickListItemDG.Items)
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
            foreach(var i in pickListItemDG.Items)
            {
                items.Add(i as UniqueItemModel);
            }
            return items;
        }

        private void uniqItemNrTB_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter && !string.IsNullOrEmpty(uniqItemNrTB.Text))
            {
                this.AddItemToPickList(uniqItemNrTB.Text);
            }
        }

        private void removePickListItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.pickListItemDG.SelectedIndex > -1)
            {
                this.pickListItemDG.Items.RemoveAt(this.pickListItemDG.SelectedIndex);
            }
        }
    }
}
