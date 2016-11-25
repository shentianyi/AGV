using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Interface
{
    public interface IStorageRepository
    {
        IQueryable<Storage> Search(StorageSearchModel storageSearchModel);
        
        Storage SearchByUniqueId(int UniqueItem);

        bool Create(Storage sg);

        bool Delete(Storage sg);

        bool Update(Storage sg);

    }
}
