using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvWareHouseLibrary.Data.Interface;

namespace AgvWareHouseLibrary.Data.Implement
{
    public class DataContextRepository:IDataContext
    {
        private string connStr;
        public DataContextRepository(string connStr)
        {
            this.connStr = connStr;
        }
        private AgvWareHouseDataContext context;
       

        public System.Data.Linq.DataContext Context
        {
            get
            {
                if (context == null)
                {
                    context = new AgvWareHouseDataContext(this.connStr);
                }
                return context;
            }
        }

        public void SaveAll()
        {
            context.SubmitChanges();
        }
  
}
}
