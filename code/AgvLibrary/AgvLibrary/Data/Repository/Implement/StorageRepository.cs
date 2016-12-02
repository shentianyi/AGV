using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data.Repository.Implement
{
   public class StorageRepository : RepositoryBase<Storage>, IStorageRepository
    {
        private AgvWareHouseDataContext context;


        public StorageRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }
        public bool Create(Storage sg)
        {
            try
            {
                this.context.GetTable<Storage>().InsertOnSubmit(sg);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(Storage sg)
        {
            try
            {
                this.context.GetTable<Storage>().DeleteOnSubmit(sg);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PartNrExist(string PartNr)
        {
            return SearchByPartNr(PartNr) != null;
        }

        public bool PositionNrExist(string PositionNr)
        {
            return SearchByPositionNr(PositionNr) != null;
        }

        public Storage SearchByPartNr(string PartNr)
        {
            return this.context.GetTable<Storage>().FirstOrDefault(c => c.PartNr.Equals(PartNr));
        }

        public Storage SearchByPositionNr(string PositionNr)
        {
            return this.context.GetTable<Storage>().FirstOrDefault(c => c.PositionNr.Equals(PositionNr));
        }

        public Storage SearchByUniqueNr(string UniqueItemNr)
        {
            return this.context.GetTable<Storage>().FirstOrDefault(c => c.UniqItemNr.Equals(UniqueItemNr));
        }

        public bool UniqueItemNrExist(string UniqueItemNr)
        {
            return SearchByUniqueNr(UniqueItemNr) != null;
        }

        public bool Update(Storage sg)
        {
            try
            {
                Storage sgi = this.context.GetTable<Storage>().FirstOrDefault(c => c.PositionNr.Equals(sg.PositionNr));

                if (sgi != null)
                {
                    sgi.UniqItemNr = sg.UniqItemNr;
                    sgi.FIFO = sg.FIFO;
                    sgi.PartNr = sg.PartNr;
                    sgi.PositionNr = sg.PositionNr;
                    sgi.CreatedAt = DateTime.Now;

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
