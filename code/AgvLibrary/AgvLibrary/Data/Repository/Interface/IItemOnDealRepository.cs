using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Interface
{
   public interface IItemOnDealRepository
    {
        IQueryable<ItemOnDeal> Search(ItemOnDealSearchModel itemOnDealSearchModel);

        ItemOnDeal SearchByUniqueId(int UniqueItemId);
    }
}
