using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class BoxTypeRepository : RepositoryBase<BoxType>, IBoxTypeRepository
    {
        private AgvWarehouseDataContext context;
        public BoxTypeRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public List<BoxType> All()
        {
            return this.context.BoxType.ToList();
        }
    }
}
