using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;

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

        public IQueryable<Storage> Search(StorageSearchModel storageSearchModel)
        {
            IQueryable<Storage> Storages = this.context.Storage;
            if (!string.IsNullOrEmpty(storageSearchModel.PosationNr))
            {
                Storages = Storages.Where(c => c.PosationNr.Equals(storageSearchModel.PosationNr));
            }
            if(!string.IsNullOrEmpty(storageSearchModel.PartNr))
            {
                Storages = Storages.Where(c => c.PartNr.Equals(storageSearchModel.PartNr));
            }
            return Storages;
        }

        public Storage SearchByUniqueId(int UniqueItem)
        {
            return this.context.GetTable<Storage>().FirstOrDefault(c => c.ItemNr.Equals(UniqueItem));
        }

        public bool Update(Storage sg)
        {
            try
            {
                Storage sgi = this.context.GetTable<Storage>().FirstOrDefault(c => c.PosationNr.Equals(sg.PosationNr));

                if (sgi != null)
                {
                    sgi.ItemNr = sg.ItemNr;
                    sgi.FIFO = sg.FIFO;
                    sgi.PartNr = sg.PartNr;
                    sgi.PosationNr = sg.PosationNr;

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
