using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class TrayItemRepository : RepositoryBase<TrayItem>, ITrayItemRepository
    {
        private AgvWarehouseDataContext context;


        public TrayItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Creates(List<TrayItem> entities)
        {
            this.context.TrayItem.InsertAllOnSubmit(entities);
        }

        public TrayItem FindByUniqNr(string uniqNr)
        {
            return this.context.TrayItem.FirstOrDefault(s => s.UniqItemNr == uniqNr);
        }
    }
}
