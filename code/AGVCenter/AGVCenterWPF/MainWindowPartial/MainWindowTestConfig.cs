using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AGVCenterLib.Enum;
using AGVCenterWPF.Config;
using Brilliantech.Framwork.Models;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {



        private void SettingTabItem_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadBasicConfig();
            this.LoadRMWorkModeSetting();
        }

        


        private void SaveBarcodeRegBtn_Click(object sender, RoutedEventArgs e)
        {
            BaseConfig.BarcodeReg = this.barcodeRegex.Text; 
        }


        /// <summary>
        /// 加载工作模式
        /// </summary>
        private void LoadRMWorkModeSetting()
        {
            /// load roadmahine setting
            List<EnumItem> modes = EnumUtil.GetList(typeof(RoadMachineTaskModel));

            this.roadMachine1ModeCB.ItemsSource = modes;
            this.roadMachine2ModeCB.ItemsSource = modes;

            this.roadMachine1ModeCB.Items.Refresh();
            this.roadMachine2ModeCB.Items.Refresh();

            for (var i = 0; i < this.roadMachine1ModeCB.Items.Count; i++)
            {
                if (this.roadMachine1ModeCB.Items.OfType<EnumItem>().ToList()[i].Value.Equals(((int)ModeConfig.RoadMachine1TaskMode).ToString()))
                {
                    this.roadMachine1ModeCB.SelectedIndex = i;
                    break;
                }
            }

            for (var i = 0; i < this.roadMachine2ModeCB.Items.Count; i++)
            {
             
                if (this.roadMachine2ModeCB.Items.OfType<EnumItem>().ToList()[i].Value.Equals(((int)ModeConfig.RoadMachine2TaskMode).ToString()))
                {
                    this.roadMachine2ModeCB.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// 加载基本设置
        /// </summary>
        /// <returns></returns>
        private void LoadBasicConfig()
        {

            this.autoConnectOPCCB.IsChecked = BaseConfig.AutoConnectOPC;
            this.autoLoadDbTaskOnStartCB.IsChecked = BaseConfig.AutoLoadDbTaskOnStart;

            this.roadMacine1EnabledCB.IsChecked = BaseConfig.RoadMachine1Enabled;
            this.roadMacine2EnabledCB.IsChecked = BaseConfig.RoadMachine2Enabled;


            this.inStockCreateStorageCB.IsChecked = TestConfig.InStockCreateStorage;
            this.outStockTaskDelStorageCB.IsChecked = TestConfig.OutStockTaskDelStorage;
            this.showRescanErrorBarcodeCB.IsChecked = TestConfig.ShowRescanErrorBarcode;

            this.barcodeRegex.Text = BaseConfig.BarcodeReg;
            this.isUsePositionPriorityCB.IsChecked = BaseConfig.IsUsePositionPriority;

            this.isSelfAreaMoveCB.IsChecked = BaseConfig.IsSelfAreaMove;



        }

        private void roadMachine1ModeCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if(cb.Name== "roadMachine1ModeCB")
            {
                ModeConfig.RoadMachine1TaskMode =(RoadMachineTaskModel) int.Parse( roadMachine1ModeCB.SelectedValue.ToString());
            }else if(cb.Name== "roadMachine2ModeCB")
            {
                ModeConfig.RoadMachine2TaskMode = (RoadMachineTaskModel)int.Parse(roadMachine2ModeCB.SelectedValue.ToString());
            }
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
                case "isUsePositionPriorityCB":
                    BaseConfig.IsUsePositionPriority = this.isUsePositionPriorityCB.IsChecked.Value;
                    break;
                case "isSelfAreaMoveCB":
                    BaseConfig.IsSelfAreaMove = this.isSelfAreaMoveCB.IsChecked.Value;
                    break;
                default:
                    break;
            }
        }
    }
}