using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class PartRepository : RepositoryBase<Part>, IPartRepository
    {
        private AgvWarehouseDataContext context;
             

        public PartRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

    
        public Part FindByNr(string PartNr)
        {
            return this.context.GetTable<Part>().FirstOrDefault(c => c.Nr.Equals(PartNr));
        }
    }
}
