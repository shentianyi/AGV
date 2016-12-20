using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.SearchModel;

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

        public List<DeliveryStorageView> GetStorageList(string nr, bool all = false)
        {
            var q = this.context.DeliveryStorageView.Where(s => s.Nr == nr);
            q= all ? q  : q.Where(s => s.StorageId != null);

            return q.ToList();
        }

        public IQueryable<Delivery> Search(DeliverySearchModel searchModel)
        {
            var q = this.context.Delivery as IQueryable<Delivery>;

            if (!string.IsNullOrEmpty(searchModel.Nr))
            {
                q = q.Where(s => s.Nr.Contains(searchModel.Nr));
            }

            if (!string.IsNullOrEmpty(searchModel.NrAct))
            {
                q = q.Where(s => s.Nr == searchModel.NrAct);
            }


            return q;
        }
    }
}
