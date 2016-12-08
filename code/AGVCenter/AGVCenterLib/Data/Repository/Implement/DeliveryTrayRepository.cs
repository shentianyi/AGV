using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class DeliveryTrayRepository : RepositoryBase<DeliveryTray>, IDeliveryTrayRepository
    {
        private AgvWarehouseDataContext context;


        public DeliveryTrayRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(DeliveryTray entity)
        {
            this.context.DeliveryTray.InsertOnSubmit(entity);
        }

        public void Creates(List<DeliveryTray> entities)
        {
            this.context.DeliveryTray.InsertAllOnSubmit(entities);
        }
    }
}
