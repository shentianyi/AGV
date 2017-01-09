using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StockMovementRepository :RepositoryBase<StockMovement>, IStockMovementRepository
    {
        private AgvWarehouseDataContext context;

        public StockMovementRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(StockMovement entity)
        {
            this.context.StockMovement.InsertOnSubmit(entity);
        }

        public IQueryable<StockMovement> Search(StockMovementSearchModel searchModel)
        {    
            var q = this.context.StockMovement as IQueryable<StockMovement>;

            if (!string.IsNullOrEmpty(searchModel.UniqItemNr))
            {
                q = q.Where(s => s.UniqItemNr.Contains(searchModel.UniqItemNr));
            }


            if (!string.IsNullOrEmpty(searchModel.PositionNr))
            {
                q = q.Where(s => s.SourcePosition.Contains(searchModel.PositionNr)
                || s.AimedPosition.Contains(searchModel.PositionNr));
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
