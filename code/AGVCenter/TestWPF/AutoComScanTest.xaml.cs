using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace TestWPF
{
    /// <summary>
    /// AutoComScanTest.xaml 的交互逻辑
    /// </summary>
    public partial class AutoComScanTest : Window
    {
        public AutoComScanTest()
        {
            InitializeComponent();
        }

        SerialPort sp;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                sp = new SerialPort("COM24");
                sp.DataReceived += Sp_DataReceived;
                sp.Open();
            }catch(Exception ex)
            {

            }
        }


        private void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(()=> {
                string s = sp.ReadLine();
                listBox.Items.Add(s.TrimEnd('\n').TrimEnd('\r'));
            }));
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                sp.Close();
            }catch(Exception ex) { }
        }
    }
}
