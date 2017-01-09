using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class PickListRepository : RepositoryBase<PickList>, IPickListRepository
    {
        private AgvWarehouseDataContext context;


        public PickListRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(PickList entity)
        {
            this.context.PickList.InsertOnSubmit(entity);
        }
         
        public void DeletePickListForTest(string pickListNr)
        {
            string cmd = string.Format("delete from PickListItem where PickListNr='{0}';delete from PickList where nr = '{1}'; ", pickListNr, pickListNr);
            this.context.ExecuteCommand(cmd);
        }

        public PickList FindByNr(string nr)
        {
            return this.context.PickList.FirstOrDefault(s => s.Nr==nr);
        }

        public List<PickListStorageView> GetStorageList(string nr, bool all = false)
        {
            var q = this.context.PickListStorageView.Where(s => s.Nr == nr);
            q = all ? q : q.Where(s => s.StorageId != null);

            return q.ToList();
        }

        public IQueryable<PickList> Search(PickListSearchModel searchModel)
        {
            var q = this.context.PickList as IQueryable<PickList>;

            if (!string.IsNullOrEmpty(searchModel.Nr))
            {
                q = q.Where(s => s.Nr.Contains(searchModel.Nr));
            }

            if (!string.IsNullOrEmpty(searchModel.NrAct))
            {
                q = q.Where(s => s.Nr == searchModel.NrAct);
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
