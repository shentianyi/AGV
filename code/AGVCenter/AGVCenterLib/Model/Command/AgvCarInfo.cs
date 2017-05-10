using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.Command
{
    public class AgvCarInfo : INotifyPropertyChanged
    {
        #region 状态，位置，路线对应关系
        public static Dictionary<string, string> stateDic = new Dictionary<string, string>() {
            { "1","运行"}, { "2","停止"}, { "4","遮堵"}
        };
        public static Dictionary<string, string> pointDic = new Dictionary<string, string>() {
            {"1","入库工位" }, { "3","充电工位"}, { "4","充电完成待机工位"},
            { "6","总待机工位"}, { "8","大空箱工位"}, { "9","小空箱工位"},
            { "11","空小箱等待点"}, { "13","小箱成品出管制位置"}, { "14","小箱成品工位"},
            { "15","空大箱等待点"}, { "17","大箱成品出管制位置"}, { "18","大箱成品工位"},
            { "19","成品管制解除位置"}, { "21","入库扫描处"}
        };

        ///路线说明                                                               对应卡号点
        /// 1-到空大箱工位                                                         6号卡到8号卡
        /// 2-到空小箱工位                                                         6号卡到9号卡
        /// 3-到大箱成品进管制位置                                                 8号卡到15号卡
        /// 4-到小箱成品进管制位置                                                 9号卡到11号卡
        /// 5-到大箱成品工位                                                       15号卡到18号卡
        /// 6-到小箱成品工位                                                       11号卡到14号卡
        /// 7-到大箱成品出管制位置                                                 18号卡到17号卡
        /// 8-到小箱成品出管制位置                                                 14号卡到13号卡
        /// 9-从大箱成品出管制位置到扫描工位                                       17号卡到21号卡
        /// 10-从小箱成品出管制位置到扫描工位                                      13号卡到21号卡
        /// 11-到入库工位                                                          21号卡到1号卡
        /// 12-到待机工位                                                          1号卡到6号卡
        /// 13-到充电工位                                                          1号卡到3号卡
        public static Dictionary<string, string> routeDic = new Dictionary<string, string>()
        {
            {"1","到空大箱工位"},{"2","到空小箱工位"},{"3","到大箱成品进管制位置"},
            { "4","到小箱成品进管制位置"},{"5","到大箱成品工位"},{"6","到小箱成品工位"},
            { "7","到大箱成品出管制位置"},{ "8","到小箱成品出管制位置"},{"9","从大箱成品出管制位置到扫描工位"},
            { "10","从小箱成品出管制位置到扫描工位"},{"11","到入库工位"},{"12","到待机工位"},
            { "13","到充电工位"}
        };
        #endregion

        public AgvCarInfo() {
            inStockScanStopTimer = new System.Timers.Timer();
            inStockScanStopTimer.Interval = 1000;
            inStockScanStopTimer.Elapsed += ScanStopTimer_Elapsed;
            inStockScanStopTimer.Enabled = true;
            inStockScanStopTimer.Start();
        }


        public AgvCarInfo(int scanStopSecondsToWarn):this()
        {
            this.maxScanStopSecondsToWarn = 20; 
           // this.maxScanStopSecondsToWarn = scanStopSecondsToWarn;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private int id;
        public int Id { get {
                return this.id;
            } set {
                this.id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }
        private string stateWas;
        private string state;
        public string State {
            get
            {
                return this.state;
            }
            set
            {
                this.stateWas = state;
                this.state = value;
                if (this.state != this.stateWas)
                {
                    this.inStockScanStopSeconds = 0;
                    this.IsInStockScanStopOverTime = false;
                }
                if (!this.IsStop)
                {
                    this.inStockScanStopSeconds = 0;
                    this.IsInStockScanStopOverTime = false;
                }
                if (this.AgvStopInStockEvent != null)
                {
                    this.AgvStopInStockEvent(this);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("State"));
                OnPropertyChanged(new PropertyChangedEventArgs("StateStr"));
            }
        }
        private string route;
        public string Route {
            get
            {
                return this.route;
            }
            set
            {
                this.route = value;
                this.IsNeedCharge = this.route == "13";
                OnPropertyChanged(new PropertyChangedEventArgs("Route"));
                OnPropertyChanged(new PropertyChangedEventArgs("RouteStr"));

            }

        }
        private string pointWas;
        private string point;
        public string Point {
            get
            {
                return this.point;
            }
            set
            {
                this.pointWas = this.point;
                this.point = value;
                if (point != pointWas)
                {
                    this.inStockScanStopSeconds = 0;
                }
                this.IsInStockScanStop = this.point == "21";
                if (this.IsInStockScanStop == false)
                {
                    this.IsInStockScanStopOverTime = false;
                }

                if (this.AgvStopInStockEvent != null)
                {
                    this.AgvStopInStockEvent(this);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Point"));
                OnPropertyChanged(new PropertyChangedEventArgs("PointStr"));
            }
        }
        private string voltage;
        public string Voltage {
            get
            {
                return this.voltage;
            }
            set
            {
                this.voltage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Voltage"));
            }
        }

        public string StateStr
        {
            get
            {
                if (this.State!=null && stateDic.ContainsKey(this.State))
                {
                    return stateDic[this.State];
                }
                return "#未知";
            }
        }

        public string PointStr
        {
            get
            {
                if (this.Point != null && pointDic.ContainsKey(this.Point))
                {
                    return pointDic[this.Point];
                }
                return "#未知";
            }
        }

        public string RouteStr
        {
            get
            {
                if (this.Route != null && routeDic.ContainsKey(this.Route))
                {
                    return routeDic[this.Route];
                }
                return "#未知";
            }
        }


        #region 小车入库扫描堵塞事件
        /// <summary>
        /// 小车入库扫描堵塞事件委托
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void AgvStopInStockEventHandler(AgvCarInfo agvCarInfo);

        /// <summary>
        /// 小车入库扫描堵塞事件
        /// </summary>
        public event AgvStopInStockEventHandler AgvStopInStockEvent;
        #endregion
        System.Timers.Timer inStockScanStopTimer;
        private int maxScanStopSecondsToWarn = 20;
        public int MaxScanStopSecondsToWarn
        {
            get
            {
               return this.maxScanStopSecondsToWarn;
            }
            set {
                this.maxScanStopSecondsToWarn = value;
            }
        }


        private bool isInStockScanStop = false;
        public bool IsInStockScanStop
        {
            get
            {
                return this.isInStockScanStop;
            }
            set {

                this.isInStockScanStop = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsInStockScanStop"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsInStockScanStopStr"));
            }
        }

        public string IsInStockScanStopStr
        {
            get
            {
                return this.IsInStockScanStop ? "是" : "否";
            }
        }
        public bool IsStop {
            get
            {
                return this.state == "2" || this.state=="4";
            }
        }
        int inStockScanStopSeconds = 0;
        private bool isInStockScanStopOverTime = false;
        public bool IsInStockScanStopOverTime {
            get
            {
                return this.isInStockScanStopOverTime;
            }
            set
            {
                this.isInStockScanStopOverTime = value;

                OnPropertyChanged(new PropertyChangedEventArgs("IsInStockScanStopOverTime"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsInStockScanStopOverTimeStr"));
            }
        }

        public string IsInStockScanStopOverTimeStr
        {
            get
            {
                return this.IsInStockScanStopOverTime ? "是" : "否";
            }
        }

        private void ScanStopTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (this.IsStop)
                {
                    inStockScanStopSeconds += 1;
                    LogUtil.Logger.InfoFormat("{0}小车停止{1}秒", this.id, this.inStockScanStopSeconds);

                    if (inStockScanStopSeconds > this.maxScanStopSecondsToWarn)
                    {
                        LogUtil.Logger.InfoFormat("{0}小车停止超过{1}秒", this.id, this.maxScanStopSecondsToWarn);
                        if (this.isInStockScanStop)
                        {
                            this.IsInStockScanStopOverTime = true;
                            
                                this.AgvStopInStockEvent(this);
                             
                            LogUtil.Logger.InfoFormat("【触发{0}号AGV入库停止时间过长事件】", this.id);
                            
                        }else
                        {
                            this.IsInStockScanStopOverTime = false;
                        }
                    }
                    else
                    {
                        this.IsInStockScanStopOverTime = false;
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }

        #region 需要充电事件（是否在充电区）
        /// <summary>
        /// 需要充电事件（是否在充电区）委托
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void AgvNeedChargeEventHandler(AgvCarInfo agvCarInfo);

        /// <summary>
        /// 需要充电事件（是否在充电区）
        /// </summary>
        public event AgvNeedChargeEventHandler AgvNeedChargeEvent;
        #endregion
        private bool isNeedChargeWas = false;
        private bool isNeddCharge = false;
        public bool IsNeedCharge
        {
            get
            {
                return isNeddCharge;
            }
            set
            {
                isNeedChargeWas = isNeddCharge;
                isNeddCharge = value;
                if (isNeddCharge != isNeedChargeWas)
                {
                    this.AgvNeedChargeEvent(this);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("IsNeedCharge"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsNeedChargeStr"));
            }
        }

        public string IsNeedChargeStr
        {
            get
            {
                return this.IsNeedCharge ? "是" : "否";
            }
        }

        public bool IsAlarm
        {
            get
            {
                return this.isNeddCharge || this.isInStockScanStopOverTime;
            }
        }
    }
}