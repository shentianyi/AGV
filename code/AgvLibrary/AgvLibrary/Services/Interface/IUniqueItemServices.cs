using AgvLibrary.Data;
using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Services.Interface
{
    public interface IUniqueItemServices
    {
        IQueryable<UniqueItem> Search(UniqueItemSearchModel uniqueItemSearchModel);



        UniqueItem SearchByUniqueId(int UniqueItem);


        bool Create(UniqueItem item);

        bool Delete(UniqueItem item);

        bool Update(UniqueItem item);
    }
}
