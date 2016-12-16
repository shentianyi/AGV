using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Service
{
    public class StockMovementService : ServiceBase
    {

        public StockMovementService(string dbString) : base(dbString)
        {

        }

        public IQueryable<StockMovement> Search(StockMovementSearchModel searchModel)
        {
            return new StockMovementRepository(this.Context).Search(searchModel);
        }
    }
}
