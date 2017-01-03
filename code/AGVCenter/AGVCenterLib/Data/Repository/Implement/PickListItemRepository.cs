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

      
    }
}
