using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class PickListItemRepository:RepositoryBase<PickListItem>, IPickListItemRepository
    {
        private AgvWarehouseDataContext context;


        public PickListItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Creates(List<PickListItem> entities)
        {
            this.context.PickListItem.InsertAllOnSubmit(entities);
        }

        public PickListItem FindByUniqNr(string uniqNr)
        {
            return this.context.PickListItem.FirstOrDefault(s => s.UniqItemNr == uniqNr);
        }

        public PickListItem FindByUniqNr(string uniqNr, string pickListNr)
        {
            return this.context.PickListItem.FirstOrDefault(s => s.UniqItemNr == uniqNr && s.PickListNr==pickListNr);
        }


        public IQueryable<PickListItemStorageView> SearchDetail(PickListItemSearchModel searchModel)
        {
            var q = this.context.PickListItemStorageView as IQueryable<PickListItemStorageView>;


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

            if (!string.IsNullOrEmpty(searchModel.PickListNrAct))
            {
                q = q.Where(s => s.PickListNr == searchModel.PickListNrAct);
            }
            if (!string.IsNullOrEmpty(searchModel.PickListNr))
            {
                q = q.Where(s => s.PickListNr.Contains(searchModel.PickListNr));
            }

            return q;
        }

    }
}
