using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Enum;

namespace AGVCenterLib.Data
{
    public partial class StockTask
    { 
        public static List<StockTaskState> CannotInStockStates = new List<StockTaskState>()
        {
            StockTaskState.AgvInStcoking,
            StockTaskState.RobootInStocking,
            StockTaskState.RoadMachineStockBuffing,
            StockTaskState.RoadMachineInStocking,
            StockTaskState.RoadMachineOutStockInit,
            StockTaskState.RoadMachineWaitOutStock,
            StockTaskState.RoadMachineOutStocking
        };

        public bool IsCannotInStockState
        {
            get { return CannotInStockStates.Contains((StockTaskState)this.State); }
        }
    }
}
