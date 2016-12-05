using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Data.Repository.Interface;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class UniqueItemRepository :RepositoryBase<UniqueItem>, IUniqueItemRepository
    {
        private AgvWarehouseDataContext context;

        public UniqueItemRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }


        public UniqueItem FindByNr(string nr)
        {
            return this.context.UniqueItem.FirstOrDefault(s => s.Nr == nr);
        }
        public UniqueItem FindByCheckCode(string checkCode)
        {
            return this.context.UniqueItem.FirstOrDefault(s => s.CheckCode == checkCode);
        }

        public void Create(UniqueItem item)
        {
            this.context.UniqueItem.InsertOnSubmit(item);
        }

        public void Delete(UniqueItem item)
        {
            throw new NotImplementedException();
        }

     

        public void Update(UniqueItem item)
        {
            throw new NotImplementedException();
        }
    }
}
