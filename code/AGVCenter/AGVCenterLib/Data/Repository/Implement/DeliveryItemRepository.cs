using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.SearchModel;

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

        public IQueryable<DeliveryItemStorageView> SearchDetail(DeliveryItemSearchModel searchModel)
        {
            var q = this.context.DeliveryItemStorageView as IQueryable<DeliveryItemStorageView>;


            if (!string.IsNullOrEmpty(searchModel.Nr))
            {
                q = q.Where(s => s.UniqItemNr.Contains(searchModel.Nr));
            }


            if (!string.IsNullOrEmpty(searchModel.KNr))
            {
                q = q.Where(s => s.UniqueItemKNr.Contains(searchModel.KNr));
            }

            if (!string.IsNullOrEmpty(searchModel.PositionNr))
            {
                q = q.Where(s => s.StoragePositionNr.Contains(searchModel.PositionNr));
            }
            if (!string.IsNullOrEmpty(searchModel.DeliveryNrAct))
            {
                q = q.Where(s => s.DeliveryNr==searchModel.DeliveryNrAct);
            }
            if (!string.IsNullOrEmpty(searchModel.DeliveryNr))
            {
                q = q.Where(s => s.DeliveryNr.Contains(searchModel.DeliveryNr));
            }

            if (!string.IsNullOrEmpty(searchModel.TrayNr))
            {
                q = q.Where(s => s.TrayItemTrayNr.Contains(searchModel.TrayNr));
            }

            if (!string.IsNullOrEmpty(searchModel.TrayNrAct))
            {
                q = q.Where(s => s.TrayItemTrayNr == searchModel.TrayNrAct);
            }


            if (searchModel.CreatedAtStart.HasValue)
            {
                q = q.Where(s => s.CreatedAt >= searchModel.CreatedAtStart);
            }

            if (searchModel.CreatedAtEnd.HasValue)
            {
                q = q.Where(s => s.CreatedAt <= searchModel.CreatedAtEnd);
            }

            return q;
        }
    }
}
