using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StockMovementRepository :RepositoryBase<StockMovement>, IStockMovementRepository
    {
        private AgvWarehouseDataContext context;

        public StockMovementRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }
         

    }
}
