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
using AGVCenterLib.Data;
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
            int roadMachineIndex = int.Parse(roadMachineIndexTB.Text);
            string warehouseNr = warehouseTB.Text;
            int startRow = int.Parse(rowNumStartTB.Text);
            int endRow = int.Parse(rowNumEndTB.Text);

            int startColumn = int.Parse(columnNumStartTB.Text);
            int endColumn = int.Parse(columnNumEndTB.Text);

            int startFloor = int.Parse(floorNumStartTB.Text);
            int endFloor = int.Parse(floorNumEndTB.Text);
            string[] p = new string[5] { "","A", "B", "C", "D" };
            for (int i = 1; i <= 4; i++)
            {
                for (int j = startColumn; j <= endColumn; j++)
                {
                    for (int k = startFloor; k <= endFloor; k++)
                    {
                        Position position = new Position()
                        {
                            Nr = string.Format("{0} {1} {2}",p[i],
                             k.ToString().PadLeft(2, '0'), j.ToString().PadLeft(2, '0')),
                            WarehouseNr = warehouseNr,
                            Row = i,
                            Column = j,
                            Floor = k,
                            RoadMachineIndex = i<=2 ? 1: 2,
                        };
                        ps.CreatePosition(position);
                        
                    }
                }
            }

        }
    }
}
