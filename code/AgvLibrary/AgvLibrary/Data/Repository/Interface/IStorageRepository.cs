using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
   public interface IStorageRepository
    {
        Storage SearchByUniqueNr(string UniqueItemNr);
        Storage SearchByPositionNr(string PositionNr);
        Storage SearchByPartNr(string PartNr);

        bool UniqueItemNrExist(string UniqueItemNr);
        bool PositionNrExist(string PositionNr);
        bool PartNrExist(string PartNr);
        bool Create(Storage sg);

        bool Delete(Storage sg);

        bool Update(Storage sg);
    }
}
