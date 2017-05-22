using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;
using Brilliantech.Framwork.Utils.EnumUtil;

namespace AGVCenterLib.Data
{
    public partial class StockTask
    { 
        public static List<StockTaskState> CannotInStockStates = new List<StockTaskState>()
        {
            StockTaskState.AgvWaitPassing,
            StockTaskState.AgvInStcoking,
            StockTaskState.RobootInStocking,
            StockTaskState.RoadMachineStockBuffing,
            StockTaskState.RoadMachineInStocking,
            StockTaskState.RoadMachineOutStockInit,
            StockTaskState.RoadMachineWaitOutStock,
            StockTaskState.RoadMachineOutStocking
        };

        public static List<StockTaskState> CanCancelStates= new List<StockTaskState>()
        {
            StockTaskState.Init,
            StockTaskState.AgvWaitPassing,
            StockTaskState.AgvInStcoking,
            StockTaskState.RoadMachineStockBuffing,
            StockTaskState.RoadMachineInStocking,
            StockTaskState.RoadMachineOutStockInit,
            StockTaskState.RoadMachineWaitOutStock,
            StockTaskState.RoadMachineOutStocking,
            StockTaskState.RoadMachineMoveStocking,
            StockTaskState.RoadMachineMoveStockInit
        };


        public static List<StockTaskState> IgnoreRescanStates = new List<StockTaskState>()
        {
           StockTaskState.ErrorUniqNotExsits
        };

        public bool IsCannotInStockState
        {
            get { return CannotInStockStates.Contains((StockTaskState)this.State); }
        }
        
        public bool IsIgnoreRescanState
        {
            get { return IgnoreRescanStates.Contains((StockTaskState)this.State); }
        }
        public string StateStr
        {
            get
            {
                return EnumUtil.GetDescription((StockTaskState)this.State);
            }
        }

        public string BoxTypeStr
        {
            get
            {
                return AGVCenterLib.Data.BoxType.GetStr(this.BoxType);
            }
        }


        public bool CanCancel
        {
            get { return CanCancelStates.Contains((StockTaskState)this.State); }
        }
    }
}
