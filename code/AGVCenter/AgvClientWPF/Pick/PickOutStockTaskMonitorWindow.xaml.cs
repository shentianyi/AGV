using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using AgvClientWPF.AgvDeliveryService;
using AgvClientWPF.AgvPickService;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AgvClientWPF.Pick
{
    /// <summary>
    /// DeliveryOutStockTaskMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class PickOutStockTaskMonitorWindow : Window
    {
        Timer loadTaskTimer;

        public PickOutStockTaskMonitorWindow()
        {
            InitializeComponent();
        }

        public PickOutStockTaskMonitorWindow(string deliveryNr)
        {
            InitializeComponent();
            this.pickListNrTB.Text = deliveryNr;
            InitLoadTask();
        }

        private void loadOutStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(pickListNrTB.Text))
            {
                InitLoadTask();
            }
        }

        private void InitLoadTask()
        {
            this.LoadPickOutStockTask(pickListNrTB.Text);
            if (loadTaskTimer != null)
            {
                loadTaskTimer.Stop();
            }
            loadTaskTimer = new Timer();
            loadTaskTimer.Interval = 2000;
            loadTaskTimer.Enabled = true;
            loadTaskTimer.Elapsed += LoadTaskTimer_Elapsed;
        }

        private void LoadTaskTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            loadTaskTimer.Stop();

            this.Dispatcher.Invoke(new Action(() =>
            {
                LoadPickOutStockTask(pickListNrTB.Text);
            }));

            loadTaskTimer.Start();
        }

        List<StockTaskModel> tasks = new List<StockTaskModel>();
        private void LoadPickOutStockTask(string pickListNr)
        {
            if (string.IsNullOrEmpty(pickListNr))
            {
                return;
            }
            try
            {

                PickServiceClient dsc = new PickServiceClient();
                List<StockTaskModel> stockTasks = dsc.GetPickListOutStockTasks(pickListNr).ToList();
                    picklistStockTaskDG.ItemsSource = stockTasks;

                foreach (var st in stockTasks)
                {
                    var t = tasks.FirstOrDefault(s => s.Id == s.Id);
                    if (t == null)
                    {
                        tasks.Add(st);
                    }
                    else
                    {
                        t.State = st.State;
                    }
                }

                picklistStockTaskDG.ItemsSource = tasks;
                outStockedNumLab.Content = stockTasks.Count(s => s.State==(int)StockTaskState.ManOutStocked || s.State == (int)StockTaskState.OutStocked);

                totalDeliveryItemNumLab.Content = stockTasks.Count();

                outStockedTotalTrayNumLab.Content = stockTasks.Select(s => s.TrayBatchId).Distinct().Count();

                int finishTrayNum = 0;
                foreach (var i in stockTasks.Select(s => s.TrayBatchId).Distinct())
                {
                    if (stockTasks.Count(s => s.TrayBatchId == i) == stockTasks.Count(s => s.TrayBatchId == i &&(s.State == (int)StockTaskState.ManOutStocked || s.State == (int)StockTaskState.OutStocked)))
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



        private void _chkSelected_OnClick(object sender, RoutedEventArgs e)
        {
            CheckBox chkSelected = e.OriginalSource as CheckBox;

            if (null == chkSelected)
            {
                return;
            }

            var cbItem = chkSelected.DataContext as StockTaskModel;
            bool isChecked = chkSelected.IsChecked.HasValue ? chkSelected.IsChecked.Value : true;
            FrameworkElement templateParent = chkSelected.TemplatedParent is FrameworkElement
                                                  ? (chkSelected.TemplatedParent as FrameworkElement).TemplatedParent as FrameworkElement
                                                  : null;

            if (templateParent is DataGridColumnHeader)
            {
                List<StockTaskModel> mvm = this.picklistStockTaskDG.ItemsSource as List<StockTaskModel>;
                if (null != mvm)
                {
                    foreach (var sm in mvm)
                    {
                        sm.IsSelected = isChecked;
                    }
                }

            }
            else if (templateParent is DataGridCell)
            {
                if (null != cbItem && null != this.picklistStockTaskDG.SelectedItems && this.picklistStockTaskDG.SelectedItems.Contains(cbItem))
                {
                    foreach (var otherSelected in this.picklistStockTaskDG.SelectedItems.OfType<StockTaskModel>())
                    {
                        otherSelected.IsSelected = isChecked;
                    }
                }
            }
        }

        private void cancelStockTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<StockTaskModel> tasks = GetSelectedTasks();
                if (tasks.Count > 0)
                {
                    PickServiceClient psc = new PickServiceClient();
                    ResultMessage msg = psc.CancelPickOutStockTask(tasks.Select(s => s.Id).ToArray());
                    if (!msg.Success)
                    {
                        MessageBox.Show(msg.Content);
                        LogUtil.Logger.Error(msg.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                MessageBox.Show(ex.Message);
            }
        }


        private List<StockTaskModel> GetSelectedTasks()
        {
            return (this.picklistStockTaskDG.ItemsSource as List<StockTaskModel>)
                 .Where(s => s.IsSelected == true)
                 .ToList();
        }

    }
}