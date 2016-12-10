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
using AGVCenterLib.Service;
using TestWPF.Properties;

namespace TestWPF
{
    /// <summary>
    /// CreatePositionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CreatePositionWindow : Window
    {
        public CreatePositionWindow()
        {
            InitializeComponent();
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            PositionService ps = new PositionService(Settings.Default.db);
            ps.FastCreatePositions(warehouseTB.Text,
                int.Parse(roadMachineIndexTB.Text),
                int.Parse(rowNumTB.Text),
                int.Parse(columnNumTB.Text),
                int.Parse(floorNumTB.Text));
        }
    }
}
