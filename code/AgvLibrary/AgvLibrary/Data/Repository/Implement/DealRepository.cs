using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Implement
{
    public class DealRepository:RepositoryBase<Deal>,IDealRepository
    {
        private AgvWareHouseDataContext context;


        public DealRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public IQueryable<Deal> Search(DealSearchModel dealSearchModel)
        {
            IQueryable<Deal> Deals = this.context.Deal;
            if(!string.IsNullOrEmpty(dealSearchModel.DealNr))
            {
                Deals = Deals.Where(c => c.DealNr.Equals(dealSearchModel.DealNr));

            }
            return Deals;
        }
    }
}
