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
using AgvLibrary.Model;
using AgvLibrary.Services.Interface;
using AgvLibrary.Services.Implement;
using AgvLibrary.Data;

namespace AGVWPF
{
    /// <summary>
    /// OfflineWin.xaml 的交互逻辑
    /// </summary>
    public partial class OfflineWin : Window
    {
        public OfflineWin()
        {
            InitializeComponent();
        }

        private void PartNrtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Offbutton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UniqueItemtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            PartSearchModel p = new PartSearchModel();


            ///装好SqlServer后链接 并更改db的值
            IPartServices q = new PartServices(Properties.Settings.Default.db);
            IUniqueItemServices UIS = new UniqueItemServices(Properties.Settings.Default.db);

            string input_PartNr = PartNrtextBox.Text;
            int input_UniqueItemId = int.Parse(UniqueItemtextBox.Text);
            if (!string.IsNullOrEmpty(input_PartNr) && !string.IsNullOrEmpty(input_UniqueItemId.ToString()))
            {
                ///查看Part内容
                Part pak = q.SearchByNr(PartNrtextBox.Text);
                if (!string.IsNullOrEmpty(pak.PartNr))
                {
                    UniqueItem UI = UIS.SearchByUniqueId(input_UniqueItemId);
                    if (string.IsNullOrEmpty(UI.ItemUnique.ToString()))
                    {

                        UniqueItem InsertUI = new UniqueItem
                        {
                            PartNr = input_PartNr,
                            ItemUnique = input_UniqueItemId,
                            CreateTime = DateTime.Now.ToString(),
                            Status="已下线"
                            
                        };
                       bool IsInsertOk=UIS.Create(InsertUI);
                        if(IsInsertOk)
                        {
                            MessageBox.Show("下线成功");
                        }
                        else
                        {
                            string mean = "下线失败";
                            MessageBox.Show(mean);
                        }
                    }
                }


            }
        }

    }
}
