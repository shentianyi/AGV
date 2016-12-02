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

        public bool PartNrExist(string PartNr)
        {
            return SearchByNr(PartNr) != null;
        }

        public IQueryable<Data.Part> Search(PartSearchModel partSearchModel)
        {
            IQueryable<Part> Part = this.context.Part;

            if (!string.IsNullOrEmpty(partSearchModel.PartNr))
            {
                Part = Part.Where(c => c.PartNr.Equals(partSearchModel.PartNr));
            }
            return Part;
        }

        public Part SearchByNr(string PartNr)
        {
            return this.context.GetTable<Part>().FirstOrDefault(c => c.PartNr.Equals(PartNr));
        }
    }
}
