using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;
using AgvLibrary.Data;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Implement;


namespace AgvLibrary.Services.Implement
{
    public class UniqueItemServices : ServiceBase, IUniqueItemServices
    {
        private IUniqueItemRepository UIRep;

      
        public UniqueItemServices(string dbString) : base(dbString) {
            UIRep = new UniqueItemRepository(this.Context);
        }

        public bool Create(UniqueItem Uniqitem)
        {
            return UIRep.Create(Uniqitem);
        }

        public bool Delete(UniqueItem Uniqitem)
        {
            return UIRep.Delete(Uniqitem);
        }

        public UniqueItem SearchByCreatedAt(DateTime CreatedAt)
        {
            return UIRep.SearchByCreatedAt(CreatedAt);
        }

        public UniqueItem SearchByPartNr(string PartNr)
        {
            return UIRep.SearchByPartNr(PartNr);
        }

        public UniqueItem SearchByStatus(int State)
        {
            return UIRep.SearchByStatus(State);
        }

        public UniqueItem SearchByUniqNr(string UniqNr)
        {
            return UIRep.SearchByUniqNr(UniqNr);
        }

        public bool Update(UniqueItem Uniqitem)
        {
            return UIRep.Update(Uniqitem);
        }
    }
}
