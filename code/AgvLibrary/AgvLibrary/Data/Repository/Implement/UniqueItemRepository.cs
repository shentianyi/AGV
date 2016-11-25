using AgvLibrary.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Implement
{
    public class UniqueItemRepository :RepositoryBase<UniqueItem>, IUniqueItemRepository
    {
        private AgvWareHouseDataContext context;

        public UniqueItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public bool Delete(UniqueItem item)
        {
            try
            {
                this.context.GetTable<UniqueItem>().DeleteOnSubmit(item);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<UniqueItem> Search(UniqueItemSearchModel uniqueItemSearchModel)
        {
            IQueryable<UniqueItem> UniqueItems = this.context.UniqueItem;
            if(!string.IsNullOrEmpty(uniqueItemSearchModel.PartNr))
            {
                UniqueItems = UniqueItems.Where(c => c.PartNr.Equals(uniqueItemSearchModel.PartNr));
            }
            if(!string.IsNullOrEmpty(uniqueItemSearchModel.Status))
            {
                UniqueItems = UniqueItems.Where(c => c.Status.Equals(uniqueItemSearchModel.Status));
            }
            if(!string.IsNullOrEmpty(uniqueItemSearchModel.CreatTime))
            {
                UniqueItems = UniqueItems.Where(c => c.CreateTime.Equals(uniqueItemSearchModel.CreatTime));
            }
            
            return UniqueItems;
        }

     

        public UniqueItem SearchByUniqueId(int ItemUnique)
        {
            return this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.ItemUnique.Equals(ItemUnique));
        }

        public bool Update(UniqueItem item)
        {
            try
            {
                UniqueItem ui = this.context.GetTable<UniqueItem>().FirstOrDefault(c => c.ItemUnique.Equals(item.ItemUnique));

                if (ui != null)
                {
                    ui.ItemUnique = item.ItemUnique;
                    ui.PartNr = item.PartNr;
                    ui.CreateTime = item.CreateTime;
                    ui.Status = item.Status;

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

        bool IUniqueItemRepository.Create(UniqueItem item)
        {
            try
            {
                this.context.GetTable<UniqueItem>().InsertOnSubmit(item);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }




    }
}
