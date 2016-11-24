using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Implement
{
    public class PartRepository : RepositoryBase<Part>, IPartRepository
    {
        private AgvWareHouseDataContext context;
             

        public PartRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public IQueryable<Data.Part> Search(PartSearchModel partSearchModel)
        {
            IQueryable<Data.Part> Parts = this.context.Part;

            if (!string.IsNullOrEmpty(partSearchModel.PartNr))
            {
                Parts = Parts.Where(c => c.PartNr.Equals(partSearchModel.PartNr));
            }
            return Parts;
        }
    }
}
