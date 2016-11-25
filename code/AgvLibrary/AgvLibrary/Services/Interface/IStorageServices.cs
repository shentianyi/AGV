using AgvLibrary.Data;
using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Services.Interface
{
    public interface IStorageServices
    {
        IQueryable<Storage> Search(StorageSearchModel storageSearchModel);

        Storage SearchByUniqueId(int UniqueItem);

        bool Create(Storage sg);

        bool Delete(Storage sg);

        bool Update(Storage sg);
    }
}
