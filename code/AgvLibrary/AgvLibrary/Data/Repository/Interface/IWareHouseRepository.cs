using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Interface
{
    public interface IWareHouseRepository
    {
        IQueryable<WareHouse> Search(WareHouseSearchModel wareHouseSearchModel);

        WareHouse SearchById(int AgvId);


    }
}
