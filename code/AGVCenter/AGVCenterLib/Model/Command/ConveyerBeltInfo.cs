using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.Command
{
    public class ConveyerBeltInfo : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        private int bigBoxBeltEmptyStateWas;
        private int bigBoxBeltEmptyState;
        public int BigBoxBeltEmptyState { get { return this.bigBoxBeltEmptyState;
            } set {
                this.bigBoxBeltEmptyStateWas = this.bigBoxBeltEmptyState;
                this.bigBoxBeltEmptyState = value;
                if (this.bigBoxBeltEmptyState != this.bigBoxBeltEmptyStateWas)
                {
                    this.BigBoxBeltStateChangedEvent(this);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("BigBoxBeltEmptyState"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsBigBoxBeltEmpty"));
            }
        }

        #region 大箱传送带空箱事件
        /// <summary>
        /// 大箱传送带空箱事件
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void BigBoxBeltStateChangedEventHandler(ConveyerBeltInfo conveyerBeltInfo);

        /// <summary>
        /// 大箱传送带空箱事件
        /// </summary>
        public event BigBoxBeltStateChangedEventHandler BigBoxBeltStateChangedEvent;
        #endregion

        public bool IsBigBoxBeltEmpty
        {
            get
            {
                return this.bigBoxBeltEmptyState == 1;
            }
        }

        private int smallBoxBeltEmptyStateWas;
        private int smallBoxBeltEmptyState;
        public int SmallBoxBeltEmptyState
        {
            get
            {
                return this.smallBoxBeltEmptyState;
            }
            set
            {

                this.smallBoxBeltEmptyStateWas = this.smallBoxBeltEmptyState;
                this.smallBoxBeltEmptyState = value;
                if (this.smallBoxBeltEmptyState != this.smallBoxBeltEmptyStateWas)
                {
                    this.SmallBoxBeltStateChangedEvent(this);
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SmallBoxBeltEmptyState"));
                OnPropertyChanged(new PropertyChangedEventArgs("IsSmallBoxBeltEmpty"));
            }
        }

        #region 小箱传送带空箱事件
        /// <summary>
        /// 小箱传送带空箱事件
        /// </summary>
        /// <param name="sensor"></param>
        /// <param name="toFlag"></param>
        public delegate void SmallBoxBeltStateChangedEventHandler(ConveyerBeltInfo conveyerBeltInfo);

        /// <summary>
        /// 小箱传送带空箱事件
        /// </summary>
        public event SmallBoxBeltStateChangedEventHandler SmallBoxBeltStateChangedEvent;
        #endregion
        public bool IsSmallBoxBeltEmpty
        {
            get
            {
                return this.smallBoxBeltEmptyState == 1;
            }
        }


    }
}