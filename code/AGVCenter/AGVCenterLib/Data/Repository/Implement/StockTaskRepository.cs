using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
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

        public StockTask FindById(int id)
        {
            return this.context.StockTask.FirstOrDefault(s => s.id == id);
        }

        public StockTask FindLastByCheckCode(string checkCode)
        {
            return this.context.StockTask.LastOrDefault(s => s.BarCode == checkCode);
        } 
    }
}
