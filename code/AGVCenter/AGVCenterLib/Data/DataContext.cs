using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace AGVCenterLib.Data
{
    public class DataContext : IDataContextFactory
    {
        private string connStr;
        public DataContext(string connStr)
        {
            this.connStr = connStr;
        }
        private AgvWarehouseDataContext context;
  

      public System.Data.Linq.DataContext  Context
        {
            get
            {
                if (context == null)
                {
                    context = new AgvWarehouseDataContext(this.connStr);
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
