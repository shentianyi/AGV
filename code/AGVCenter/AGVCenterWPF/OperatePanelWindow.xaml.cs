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

namespace AGVCenterWPF
{
    /// <summary>
    /// OperatePanelWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OperatePanelWindow : Window
    {
        public OperatePanelWindow()
        {
            InitializeComponent();
        }

        MainWindow mainWindow = null;
        public OperatePanelWindow(MainWindow mainWindow)
        {
            InitializeComponent();
           this.mainWindow = mainWindow;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (fromPositionTB.Text.Length > 0 && toPositionTB.Text.Length > 0)
                {
                    mainWindow.ManualMoveStockStock(fromPositionTB.Text, toPositionTB.Text, int.Parse(roadMachineTB.Text));
                } }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
