using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IStockTaskLogRepository
    {
        void Create(StockTaskLog entity);

        void Creates(List<StockTaskLog> entities);


        IQueryable<StockTaskLog> Search(StockTaskLogSearchModel searchModel);
    }
}
