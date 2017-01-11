using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AgvClientWPF
{
    /// <summary>
    /// MsgDialog.xaml 的交互逻辑
    /// </summary>
    public partial class MsgDialog : Window
    {

        private static string infoImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\Info.png");
        private static string warningImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\Warning.png");
        private static string successImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\ok.png");
        private static string errorImage = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\Error.png");


        private bool focusIdleIndicator = false;
        private MsgLevel currentLevel;

        private System.Timers.Timer autoCloseTimer;
        public MsgDialog()
        {
            InitializeComponent();

        }


        public MsgDialog(MsgLevel level, string msg, bool setIdle = false, int autoCloseTime = 0) {
            InitializeComponent();
            this.AssignPicture(level);
            this.AssignText(msg);
            focusIdleIndicator = setIdle;
            currentLevel = level;
            setClickMode();
            if (autoCloseTime > 0) {
                this.autoCloseTimer = new System.Timers.Timer();
                this.autoCloseTimer.Interval = autoCloseTime * 1000;
                this.autoCloseTimer.Elapsed += AutoCloseTimer_Elapsed;

                autoCloseTimer.Enabled = true;
                autoCloseTimer.Start();
            }
        }

        public MsgDialog(MsgLevel level, string msg, bool setIdle = false)
        {
            InitializeComponent();
            this.AssignPicture(level);
            this.AssignText(msg);
            focusIdleIndicator = setIdle;
            currentLevel = level;
            setClickMode();
        }

        private void AutoCloseTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() => {
                    try
                    {
                        this.Close();
                    } catch (Exception ex)
                    {

                    }
                }));
            } catch (Exception ex)
            {

            }
        }

        private void setClickMode() {
            this.Button_no.ClickMode = ClickMode.Press;
            this.Button_yes.ClickMode = ClickMode.Press;
            this.Button_no.IsDefault = true;
            this.Button_yes.IsDefault = true;
        }


        public void AssignPicture(MsgLevel level) {
            string imagePath = "";
            try {
                switch (level)
                {
                    case MsgLevel.Info:
                        imagePath = infoImage;
                        break;

                    case MsgLevel.Mistake:
                        imagePath = errorImage;
                        break;
                    case MsgLevel.Successful:
                        imagePath = successImage;
                        break;
                    case MsgLevel.Warning:
                        imagePath = warningImage;
                        break;
                    default:
                        imagePath = infoImage;
                        break;
                }
                this.Indicator.Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute));


            } catch (Exception ex)
            {

            }
        }


        public static MsgDialog CMsgDlg(MsgLevel level   , string msg, bool setIdle = false, Window Owner = null, int autoCloseTime = 0) {
            MsgDialog returned = new MsgDialog(level, msg, setIdle, autoCloseTime);
            if (Owner == null) {
                returned.Owner = Owner;
            }
            return returned;
        }


        private void AssignText(string msg) {
            this.TextBox_Message.Text = msg;
        }


        public static string ArrayToString(string[] str)
        {
            string combined = "";
            if (str != null)
            {
                if ((str.Length > 0))
                {
                    foreach (string st in str)
                    {
                        combined = (combined + ("\r\n" + st));
                    }

                }

            }

            return combined.TrimStart(new char[] { '\r', '\n' });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (autoCloseTimer != null)
            {
                autoCloseTimer.Stop();
                autoCloseTimer = null;
            }
        }

        private void Button_yes_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            try {
                this.DialogResult = true;
            } catch (Exception ex)
            {

            }
            this.Close();
        }

        private void Button_no_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            try
            {
                this.DialogResult = false;
            }
            catch (Exception ex)
            {

            }
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if ((this.focusIdleIndicator == true))
            {
                this.TextBox_Message.Focus();
            }
            else
            {
                this.Button_no.Focus();
            }

            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.PlaySound), this.currentLevel);
        }


        public void PlaySound(object level)
        {
            try
            {
                SoundPlayer player = new System.Media.SoundPlayer();
                switch (((MsgLevel)(level)))
                {
                    case MsgLevel.Mistake:
                        player.SoundLocation = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\alarm.wav");
                        break;
                    case MsgLevel.Info:
                        player.SoundLocation = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\info.wav");
                        break;
                    case MsgLevel.Successful:
                        player.SoundLocation = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\info.wav");
                        break;
                    case MsgLevel.Warning:
                        player.SoundLocation = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sources\\alarm.wav");
                        break;
                }
                player.LoadAsync();
                player.PlaySync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
