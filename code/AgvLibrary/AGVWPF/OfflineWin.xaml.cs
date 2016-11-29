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


        private void Offbutton_Click(object sender, RoutedEventArgs e)
        {
            IPartServices IPS = new PartServices(Properties.Settings.Default.db);
            IUniqueItemServices IUIS = new UniqueItemServices(Properties.Settings.Default.db);

            string input_PartNr = PartNrtextBox.Text;
            string input_UniqueNr =UniqueNrtextBox.Text;
            if (string.IsNullOrEmpty(input_PartNr))
            {
                MessageBox.Show("零件编号不能为空");
            }
            if(string.IsNullOrEmpty(input_UniqueNr))
            {
                MessageBox.Show("零件唯一吗不能为空");
            }

            Part pak = IPS.SearchByNr(input_PartNr);

            if (string.IsNullOrEmpty(pak.PartNr))
            {
                MessageBox.Show("该零件不存在");
            }

            UniqueItem UIK = IUIS.SearchByUniqNr(input_UniqueNr);
            
            if(UIK!=null)
            {
                MessageBox.Show("该唯一码已被占用");
            }

            UniqueItem InsertUI = new UniqueItem
            {
                PartNr = input_PartNr,
                UniqNr = input_UniqueNr,
                CreatedAt = DateTime.Now,
                State = 0

            };
            bool IsInsertOk = IUIS.Create(InsertUI);
            if (IsInsertOk)
            {
                string mean = "下线成功";
                MessageBox.Show(mean);
            }
            else
            {
                string mean = "下线失败";
                MessageBox.Show(mean);
            }
        }
    



        private void PartNrtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       

        private void UniqueNrtextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
