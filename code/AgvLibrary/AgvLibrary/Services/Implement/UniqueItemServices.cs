using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Model;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Implement;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Services.Implement
{
    public class UniqueItemServices :ServiceBase, IUniqueItemServices
    {
        private IUniqueItemRepository UIRep;
        public UniqueItemServices(string dbString) : base(dbString) {
            UIRep = new UniqueItemRepository(this.Context);
        }

        
        public bool Create(UniqueItem item)
        {
            return UIRep.Create(item);
        }

        public bool Delete(UniqueItem item)
        {
            return UIRep.Delete(item);
        }

        public IQueryable<UniqueItem> Search(UniqueItemSearchModel uniqueItemSearchModel)
        {
            return UIRep.Search(uniqueItemSearchModel);
        }

        public UniqueItem SearchByUniqueId(int UniqueItem)
        {
            return UIRep.SearchByUniqueId(UniqueItem);
        }

        public bool Update(UniqueItem item)
        {
            return UIRep.Update(item);
        }
    }
}
