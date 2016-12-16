using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IStockMovementRepository
    {
        void Create(StockMovement entity);

        IQueryable<StockMovement> Search(StockMovementSearchModel searchModel);
    }
}
