using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace AGVCenterLib.Data.Repository.Implement
{
    class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        private AgvWarehouseDataContext context;


        public PositionRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

      
    }
}
