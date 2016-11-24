using AgvLibrary.Data;
using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Services.Interface
{
    public interface IWareHouseServices
    {
        List<WareHouse> All();
        IQueryable<WareHouse> Search(WareHouseSearchModel wareHouseSearchModel);

        WareHouse SearchById(int AgvId);
    }
}
