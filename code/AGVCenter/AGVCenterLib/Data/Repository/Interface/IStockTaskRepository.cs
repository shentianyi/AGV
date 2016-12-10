using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IStockTaskRepository
    {
        StockTask FindLastByCheckCode(string checkCode);

        StockTask FindById(int id);

        void Create(StockTask entity);

    }
}
