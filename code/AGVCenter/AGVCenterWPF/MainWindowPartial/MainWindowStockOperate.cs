using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using AGVCenterLib.Model;
using AGVCenterLib.Service;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterWPF
{
    public partial class MainWindow
    {
        public bool ManualMoveStockStock(string fromPositionNr,string toPositionNr,int roadMachineIndex)
        {
            try {
                var fromStorage = new StorageService(OPCConfig.DbString).FindViewByPositionNr(fromPositionNr);
                var toStorage = new StorageService(OPCConfig.DbString).FindViewByPositionNr(toPositionNr);
                if (fromStorage == null)
                {
                    MessageBox.Show("源库位没有库存！");
                    return false;
                }
                if (toStorage != null)
                {
                    MessageBox.Show("目标库位有库存！");
                    return false;
                }

                var fromPosition = new PositionService(OPCConfig.DbString).FindByNr(fromPositionNr);
                var toPosition = new PositionService(OPCConfig.DbString).FindByNr(toPositionNr);
                if (fromPosition == null)
                {
                    MessageBox.Show("源库位不存在！");
                    return false;
                }
                if (toPosition == null)
                {
                    MessageBox.Show("目标库位不存在！");
                    return false;
                }
                MoveStockModel ms = new MoveStockModel()
                {
                    FromStorage = fromStorage,
                    FromPosition = fromPosition,
                    ToPosition = toPosition
                };
                var st = new StockTaskService(OPCConfig.DbString).CreateAutoMoveStockTask(ms);

                var taskItem = this.InitTaskItemByStockTask(st, true);
                // 加入移库队列
                this.EnqueueRoadMachineTask(taskItem);
                this.AddOrUpdateItemToTaskDisplay(taskItem);

                return true;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogUtil.Logger.Error(ex.Message, ex);
            }

            return false;
        }
    }
}
