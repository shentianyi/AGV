using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AGVCenterLib.Data;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterLib.Service;
using TestWPF.Properties;

namespace TestWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void createPositionsBtn_Click(object sender, RoutedEventArgs e)
        {
            new CreatePositionWindow().Show();
        }

        private void inStockBtn_Click(object sender, RoutedEventArgs e)
        {
            new InStockOrOutStockWindow().Show();
        }

        private void createRemoteMsgBtn_Click(object sender, RoutedEventArgs e)
        {
            //MessageQueue q = new MessageQueue(".\\private$\\agvtaskqueue");

          MessageQueue q = new MessageQueue("FormatName:Direct=TCP:192.168.0.99\\private$\\agvtaskqueue");
            q.Send(new Message {
                Body = "23",
                Formatter = new XmlMessageFormatter(new Type[] { typeof(string) })
            });
        }

        private void autoComTest_Click(object sender, RoutedEventArgs e)
        {
            new AutoComScanTest().Show();
        }

        System.Timers.Timer t = new System.Timers.Timer();

        private void createOutTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            t.Elapsed += T_Elapsed;
            t.Interval = 2000;
            t.Enabled = true;
            t.Start();
        }
        int i = 0;
        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            i++;
            for (int j = 0; j < 20; j++)
            {
                StockTaskService ts = new StockTaskService(Settings.Default.db);
                StockTask taskItem = new StockTask()
                {
                    BarCode = Guid.NewGuid().ToString(),
                    Type = (int)StockTaskType.OUT,
                    TrayBatchId = i.ToString(),
                    RoadMachineIndex = 1,
                    State =90,
                    BoxType = 1,
                    PositionFloor = 1,
                    PositionColumn = 1,
                    PositionRow = 1
                };
                ts.CreateTask(taskItem);
            }
            t.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t.Enabled = false;
            t.Stop();
        }

        private void updatePositionPriority_Click(object sender, RoutedEventArgs e)
        {
            for (int j = 1; j< 3; j++)
            {
                AgvWarehouseDataContext context = new AgvWarehouseDataContext(Settings.Default.db);
                var ps = context.PositionStorageView
                    .Where(s => s.RoadMachineIndex==j)
                    .OrderBy(s => s.WarehouseAreaInStorePriority)
                    .ThenByDescending(s => s.Column)
                   
                    .ThenBy(s => s.Floor) .ThenBy(s => s.Row).ToList();
                var i = 1;
                foreach (var pss in ps)
                {
                    AgvWarehouseDataContext context1 = new AgvWarehouseDataContext(Settings.Default.db);

                    var p = context1.Position.FirstOrDefault(s => s.Nr == pss.Nr);
                    p.InStorePriority = i++;
                    context1.SubmitChanges();
                    
                }
            }
            MessageBox.Show("OK");

        }
    }
}
