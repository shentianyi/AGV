using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class DeliveryItemRepository:RepositoryBase<DeliveryItem>,IDeliveryItemRepository
    {
        private AgvWarehouseDataContext context;


        public DeliveryItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Creates(List<DeliveryItem> entities)
        {
            this.context.DeliveryItem.InsertAllOnSubmit(entities);
        }

        public DeliveryItem FindByUniqNr(string uniqNr)
        {
            return this.context.DeliveryItem.FirstOrDefault(s => s.UniqItemNr == uniqNr);
        }

        public DeliveryItem FindByUniqNr(string uniqNr, string deliveryNr)
        {
            return this.context.DeliveryItem.FirstOrDefault(s => s.UniqItemNr == uniqNr && s.DeliveryNr==deliveryNr);
        }
    }
}
