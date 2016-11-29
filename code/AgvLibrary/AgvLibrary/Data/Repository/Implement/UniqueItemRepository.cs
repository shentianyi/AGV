using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data.Repository.Implement
{
   public class UniqueItemRepository : RepositoryBase<UniqueItem>, IUniqueItemRepository
    {
        private AgvWareHouseDataContext context;

        public UniqueItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public bool Create(UniqueItem Uniqitem)
        {
            try
            {
                this.context.GetTable<UniqueItem>().InsertOnSubmit(Uniqitem);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(UniqueItem Uniqitem)
        {
            try
            {
                this.context.GetTable<UniqueItem>().DeleteOnSubmit(Uniqitem);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public UniqueItem SearchByCreatedAt(DateTime CreatedAt)
        {
            return this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.CreatedAt.Equals(CreatedAt));
        }

        public UniqueItem SearchByPartNr(string PartNr)
        {
            return this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.PartNr.Equals(PartNr));
        }

        public UniqueItem SearchByStatus(int State)
        {
            return this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.State.Equals(State));
        }

        public UniqueItem SearchByUniqNr(string UniqNr)
        {
            return this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.UniqNr.Equals(UniqNr));
        }

        public bool Update(UniqueItem item)
        {
            try
            {
                UniqueItem ui = this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.UniqNr.Equals(item.UniqNr));

                if (ui != null)
                {
                    ui.UniqNr = item.UniqNr;
                    ui.PartNr = item.PartNr;
                    ui.CreatedAt = item.CreatedAt;
                    ui.State = item.State;

                    this.context.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
