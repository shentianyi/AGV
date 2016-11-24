using AgvLibrary.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Implement
{
    public class UniqueItemRepository : IUniqueItem
    {
        private AgvWareHouseDataContext context;

        bool IUniqueItem.Create(UniqueItem item)
        {
            try
            {
                this.context.GetTable<UniqueItem>().InsertOnSubmit(item);
                this.context.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }




    }
}
