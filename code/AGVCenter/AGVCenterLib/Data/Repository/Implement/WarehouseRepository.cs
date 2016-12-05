using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class WarehouseRepository : RepositoryBase<Warehouse>, IWarehouseRepository
    {
        private AgvWarehouseDataContext context;
        public WarehouseRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

      
    }
}
