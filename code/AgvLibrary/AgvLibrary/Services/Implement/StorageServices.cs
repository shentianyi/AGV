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
    class StorageServices : ServiceBase, IStorageServices
    {
        private IStorageRepository SgRep;
        public StorageServices(string dbString) : base(dbString)
        {
            SgRep = new StorageRepository(this.Context);
        }

        public bool Create(Storage sg)
        {
            return SgRep.Create(sg);
        }

        public bool Delete(Storage sg)
        {
            return SgRep.Delete(sg);
        }

        public IQueryable<Storage> Search(StorageSearchModel storageSearchModel)
        {
            return SgRep.Search(storageSearchModel);
        }

        public Storage SearchByUniqueId(int UniqueItem)
        {
            return SgRep.SearchByUniqueId(UniqueItem);
        }

        public bool Update(Storage sg)
        {
            return SgRep.Update(sg);
        }
    }
}
