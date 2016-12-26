using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IStockTaskRepository
    {

        StockTask FindLastByNr(string nr);

        StockTask FindLastByCheckCode(string checkCode);

        StockTask FindById(int id);

        void Create(StockTask entity);

        void Creates(List<StockTask> entities);

        List<StockTask> GetByState(StockTaskState state);

        void UpdateTasksState(List<int> taskIds, StockTaskState state);

        List<StockTask> GetOutStockTaskByDelivery(string deliveryNr);

        List<StockTask> GetByStates(List<StockTaskState> states);

        StockTask GetByStatesAndRoadMachine(List<StockTaskState> states, int? roadMachineIndex = null);

        List<StockTask> GetLast(int take = 300);
    }
}
