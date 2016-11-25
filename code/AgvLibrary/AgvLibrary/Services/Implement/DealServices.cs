using AgvLibrary.Data.Repository.Implement;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Model;

namespace AgvLibrary.Services.Implement
{
    public class DealServices : ServiceBase, IDealServices
    {

        private IDealRepository DealRep;

        public DealServices(string dbString) : base(dbString)
        {
            DealRep = new DealRepository(this.Context);
        }

        public IQueryable<Deal> Search(DealSearchModel dealSearchModel)
        {
            return DealRep.Search(dealSearchModel);
        }
    }
}
