using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Interface
{
    public interface IUniqueItemRepository
    {
        IQueryable<UniqueItem> Search(UniqueItemSearchModel uniqueItemSearchModel);



        UniqueItem SearchByUniqueId(int UniqueItem);

 
        bool Create(UniqueItem item);

        bool Delete(UniqueItem item);

        bool Update(UniqueItem item);
    }
}
