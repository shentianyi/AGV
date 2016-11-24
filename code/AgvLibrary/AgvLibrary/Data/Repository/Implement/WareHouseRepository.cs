using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Implement
{
    public class WareHouseRepository : RepositoryBase<WareHouse>, IWareHouseRepository
    {
        private AgvWareHouseDataContext context;
        public WareHouseRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public IQueryable<WareHouse> Search(WareHouseSearchModel wareHouseSearchModel)
        {
            IQueryable<WareHouse> WareHouses = this.context.WareHouse;
            if (!string.IsNullOrEmpty(wareHouseSearchModel.WHNr))
            {
                WareHouses = WareHouses.Where(c => c.WHNr.Equals(wareHouseSearchModel.WHNr));
            }
            return WareHouses;
        }

        public WareHouse SearchById(int AgvId)
        {
            return this.context.GetTable<WareHouse>().FirstOrDefault(c => c.AgvId.Equals(AgvId));
        }
    }
}
