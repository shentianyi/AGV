using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StorageRepository : RepositoryBase<Storage>, IStorageRepository
    {
        private AgvWarehouseDataContext context;


        public StorageRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(Storage entity)
        {
            this.context.Storage.InsertOnSubmit(entity);
        }

        public void Delete(Storage entity)
        {
            this.context.Storage.DeleteOnSubmit(entity);
        }

        public Storage FindByPositionNr(string positionNr)
        {
            return this.context.Storage.FirstOrDefault(s => s.PositionNr == positionNr);
        }

        public Storage FindByUniqNr(string uniqNr)
        {
            return this.context.Storage.FirstOrDefault(s => s.UniqItemNr == uniqNr);
        }

        public Storage FindByPositionNrOrUniqNr(string positionNr, string uniqNr)
        {
            return this.context.Storage.FirstOrDefault(s => s.PositionNr == positionNr || s.UniqItemNr==uniqNr);
        }

        public IQueryable<StorageUniqueItemView> SearchDetail(StorageSearchModel searchModel)
        {
            return this.context.StorageUniqueItemView;
        }
    }
}
