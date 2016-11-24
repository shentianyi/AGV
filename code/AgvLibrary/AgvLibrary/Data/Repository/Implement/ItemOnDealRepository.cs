using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Implement
{
    public class ItemOnDealRepository:RepositoryBase<ItemOnDeal>,IItemOnDealRepository
    {
        private AgvWareHouseDataContext context;


        public ItemOnDealRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public IQueryable<ItemOnDeal> Search(ItemOnDealSearchModel itemOnDealSearchModel)
        {
            IQueryable<ItemOnDeal> ItemOnDeals = this.context.ItemOnDeal;
            if (!string.IsNullOrEmpty(itemOnDealSearchModel.DarlNr))
            {
                ItemOnDeals = ItemOnDeals.Where(c => c.DealNr.Equals(itemOnDealSearchModel.DarlNr));
            }
            return ItemOnDeals;
        }

        public ItemOnDeal SearchByUniqueId(int UniqueItemId)
        {
            return this.context.GetTable<ItemOnDeal>().FirstOrDefault(c => c.ItemNr.Equals(UniqueItemId));
        }
    }
}
