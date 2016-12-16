using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvDeliveryService;

namespace AgvClientWPF.Delivery
{
    /// <summary>
    /// DeliveryOutStockTaskMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class DeliveryOutStockTaskMonitorWindow : Window
    {
        Timer loadTaskTimer;

        public DeliveryOutStockTaskMonitorWindow()
        {
            InitializeComponent();
        }

        private void loadOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(deliveryNrTB.Text))
            {
                this.LoadDeliveryOutStockTask(deliveryNrTB.Text);
                if (loadTaskTimer != null)
                {
                    loadTaskTimer.Stop();
                }
                loadTaskTimer = new Timer();
                loadTaskTimer.Interval = 2000;
                loadTaskTimer.Enabled = true;
                loadTaskTimer.Elapsed += LoadTaskTimer_Elapsed;
            }
        }

        private void LoadTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            loadTaskTimer.Stop();

            this.Dispatcher.Invoke(new Action(() =>
            {

                LoadDeliveryOutStockTask(deliveryNrTB.Text);

            }));

            loadTaskTimer.Start();
        }

        private void LoadDeliveryOutStockTask(string deliveryNr)
        {
            if (string.IsNullOrEmpty(deliveryNr))
            {
                return;
            }
            try
            {
                DeliveryServiceClient dsc = new DeliveryServiceClient();
                List<StockTaskModel> stockTasks = dsc.GetDeliveryOutStockTasks(deliveryNr).ToList();
                deliveryStockTaskDG.ItemsSource = stockTasks;

                outStockedNumLab.Content = stockTasks.Count(s => s.State == (int)StockTaskState.OutStocked);
                totalDeliveryItemNumLab.Content = stockTasks.Count();

                outStockedTotalTrayNumLab.Content = stockTasks.Select(s => s.TrayBatchId).Distinct().Count();

                int finishTrayNum = 0;
                foreach (var i in stockTasks.Select(s => s.TrayBatchId).Distinct())
                {
                    if (stockTasks.Count(s => s.TrayBatchId == i) == stockTasks.Count(s => s.TrayBatchId == i && s.State == (int)StockTaskState.OutStocked))
                    {
                        finishTrayNum++;
                    }
                }

                outStockedTrayNumLab.Content = finishTrayNum;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}