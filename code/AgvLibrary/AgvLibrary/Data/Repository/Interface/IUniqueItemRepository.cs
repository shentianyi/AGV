using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
  public  interface IUniqueItemRepository
    {
        UniqueItem SearchByUniqNr(string UniqNr);
        UniqueItem SearchByPartNr(string PartNr);
        UniqueItem SearchByStatus(int State);
        UniqueItem SearchByCreatedAt(DateTime CreatedAt);

        bool Create(UniqueItem Uniqitem);
        bool Delete(UniqueItem Uniqitem);
        bool Update(UniqueItem Uniqitem);

    }
}
