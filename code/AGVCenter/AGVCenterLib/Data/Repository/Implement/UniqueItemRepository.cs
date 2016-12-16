using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class UniqueItemRepository :RepositoryBase<UniqueItem>, IUniqueItemRepository
    {
        private AgvWarehouseDataContext context;

        public UniqueItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }


        public UniqueItem FindByNr(string nr)
        {
            return this.context.UniqueItem.FirstOrDefault(s => s.Nr == nr);
        }

        public UniqueItem FindByCheckCode(string checkCode)
        {
            return this.context.UniqueItem.FirstOrDefault(s => s.CheckCode == checkCode);
        }

        public void Create(UniqueItem entity)
        {
            this.context.UniqueItem.InsertOnSubmit(entity);
        }

        public void Delete(UniqueItem entity)
        {
            throw new NotImplementedException();
        }

     

        public void Update(UniqueItem entity)
        {
            throw new NotImplementedException();
        }

        public List<UniqueItem> ListByDeliveryNr(string deliveryNr)
        {

            return (from i in context.UniqueItem
                    join d in context.DeliveryItem on i.Nr equals d.UniqItemNr
                    where d.DeliveryNr == deliveryNr
                    select i).ToList();
        }

        public IQueryable<UniqueItem> Search(UniqueItemSearchModel searchModel)
        {
            return this.context.UniqueItem;
        }
    }
}
