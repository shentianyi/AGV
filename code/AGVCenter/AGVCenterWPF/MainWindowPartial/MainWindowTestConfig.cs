using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AGVCenterWPF.Config;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        private void SettingTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.autoConnectOPCCB.IsChecked = BaseConfig.AutoConnectOPC;
            this.autoLoadDbTaskOnStartCB.IsChecked = BaseConfig.AutoLoadDbTaskOnStart;

            this.roadMacine1EnabledCB.IsChecked = BaseConfig.RoadMachine1Enabled;
            this.roadMacine2EnabledCB.IsChecked = BaseConfig.RoadMachine2Enabled;


            this.inStockCreateStorageCB.IsChecked = TestConfig.InStockCreateStorage;
            this.outStockTaskDelStorageCB.IsChecked = TestConfig.OutStockTaskDelStorage;
            this.showRescanErrorBarcodeCB.IsChecked = TestConfig.ShowRescanErrorBarcode;
        }

        private void SettingOPCCB_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            switch(cb.Name) {
                case "autoConnectOPCCB":
                    BaseConfig.AutoConnectOPC = this.autoConnectOPCCB.IsChecked.Value;
                    break;
                case "autoLoadDbTaskOnStartCB":
                    BaseConfig.AutoLoadDbTaskOnStart = this.autoLoadDbTaskOnStartCB.IsChecked.Value;
                    break;
                case "roadMacine1EnabledCB":
                    BaseConfig.RoadMachine1Enabled = this.roadMacine1EnabledCB.IsChecked.Value;
                    break;
                case "roadMacine2EnabledCB":
                    BaseConfig.RoadMachine2Enabled = this.roadMacine2EnabledCB.IsChecked.Value;
                    break;
                case "inStockCreateStorageCB":
                    TestConfig.InStockCreateStorage = this.inStockCreateStorageCB.IsChecked.Value;
                    break;
                case "outStockTaskDelStorageCB":
                    TestConfig.OutStockTaskDelStorage = this.outStockTaskDelStorageCB.IsChecked.Value;
                    break;
                case "showRescanErrorBarcodeCB":
                    TestConfig.ShowRescanErrorBarcode = this.showRescanErrorBarcodeCB.IsChecked.Value;
                    break;
            }
        }
    }
}