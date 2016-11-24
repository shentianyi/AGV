using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data
{

   
    public class DataContextFactory : IDataContextFactory
    {
        private string connStr;
        public DataContextFactory(string connStr)
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