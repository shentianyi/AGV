using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Services;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Data.Repository.Implement;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data;
using AgvLibrary.Model;

namespace AgvLibrary.Services.Implement
{
   public class WareHouseServices: ServiceBase,IWareHouseServices
    {
        private IWareHouseRepository WHRep;

        public WareHouseServices(string dbString) : base(dbString) {
            WHRep = new WareHouseRepository(this.Context);
        }

        public List<WareHouse> All()
        {
            return this.Context.Context.GetTable<WareHouse>().ToList();
        }

        public IQueryable<WareHouse> Search(WareHouseSearchModel wareHouseSearchModel)
        {
            return WHRep.Search(wareHouseSearchModel);
        }

        public WareHouse SearchById(int AgvId)
        {
            return WHRep.SearchById(AgvId);
        }
    }
}
