using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class DeliveryRepository:RepositoryBase<Delivery>,IDeliveryRepository
    {
        private AgvWarehouseDataContext context;


        public DeliveryRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(Delivery entity)
        {
            this.context.Delivery.InsertOnSubmit(entity);
        }

        public Delivery FindByNr(string nr)
        {
            return this.context.Delivery.FirstOrDefault(s => s.Nr==nr);
        }
    }
}
