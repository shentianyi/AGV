using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data.Repository.Implement
{
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        private AgvWareHouseDataContext context;
        public WarehouseRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public bool AgvIdExist(int AgvId)
        {
            return SearchByAgvId(AgvId) != null;
        }

        public bool WHNrExist(string WHNr)
        {
            return SearchByWHNr(WHNr) != null;
        }

        public Warehouse SearchByAgvId(int AgvId)
        {
            return this.context.GetTable<Warehouse>().FirstOrDefault(c => c.AgvId.Equals(AgvId));
        }

        public Warehouse SearchByWHNr(string WHNr)
        {
            return this.context.GetTable<Warehouse>().FirstOrDefault(c => c.WHNr.Equals(WHNr));
        }
    }
}
