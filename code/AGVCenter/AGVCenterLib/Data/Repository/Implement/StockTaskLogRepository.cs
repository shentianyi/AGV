using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StockTaskLogRepository : RepositoryBase<StockTaskLog>, IStockTaskLogRepository
    {
        private AgvWarehouseDataContext context;
        public StockTaskLogRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(StockTaskLog entity)
        {
            this.context.StockTaskLog.InsertOnSubmit(entity);
        }

        public void Creates(List<StockTaskLog> entities)
        {
            this.context.StockTaskLog.InsertAllOnSubmit(entities);
        }




        public IQueryable<StockTaskLog> Search(StockTaskLogSearchModel searchModel)
        {
            var q = this.context.StockTaskLog as IQueryable<StockTaskLog>;

            if (!string.IsNullOrEmpty(searchModel.UniqItemNr))
            {
                q = q.Where(s => s.BarCode.Contains(searchModel.UniqItemNr));
            }

            if (!string.IsNullOrEmpty(searchModel.PositionNr))
            {
                q = q.Where(s => s.PositionNr.Contains(searchModel.PositionNr));
            }

            if (searchModel.StockTaskId.HasValue)
            {
                q = q.Where(s => s.StockTaskId == searchModel.StockTaskId);
            }


            if (searchModel.CreatedAtStart.HasValue)
            {
                q = q.Where(s => s.CreatedAt >= searchModel.CreatedAtStart);
            }

            if (searchModel.CreatedAtEnd.HasValue)
            {
                q = q.Where(s => s.CreatedAt <= searchModel.CreatedAtEnd);
            }

            return q;
        }
    }
}
