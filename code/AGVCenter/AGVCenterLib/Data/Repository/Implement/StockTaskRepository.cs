using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StockTaskRepository : RepositoryBase<StockTask>, IStockTaskRepository
    {
        private AgvWarehouseDataContext context;
        public StockTaskRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(StockTask entity)
        {
            this.context.StockTask.InsertOnSubmit(entity);
        }

        public void Creates(List<StockTask> entities)
        {

            this.context.StockTask.InsertAllOnSubmit(entities);
        }

        public StockTask FindById(int id)
        {
            return this.context.StockTask.FirstOrDefault(s => s.Id == id);
        }

        public StockTask FindLastByCheckCode(string checkCode)
        {
            return this.context.StockTask.OrderByDescending(s=>s.Id).FirstOrDefault(s => s.BarCode == checkCode);
        }

        public List<StockTask> GetByState(StockTaskState state)
        {
            return this.context.StockTask.Where(s => s.State == (int)state).ToList();
        }


        public void UpdateTasksState(List<int> taskIds, StockTaskState state)
        {
            string cmd = string.Format("update stocktask set state={0} where Id in ({1});",
                (int)state,
                string.Join(",", taskIds.ToArray()));
            this.context.ExecuteCommand(cmd);
        }



        public List<StockTask> GetOutStockTaskByDelivery(string deliveryNr)
        {
            return this.context.StockTask.Where(s => s.DeliveryBatchId == deliveryNr).ToList();
        }
    }
}
