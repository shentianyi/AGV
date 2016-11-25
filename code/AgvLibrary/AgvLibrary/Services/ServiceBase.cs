using AgvLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Services
{
    public class ServiceBase
    {
        private string dbString;

        private Data.DataContextFactory _context;

        private static object syncObj = new object();

        public string DbString { get { return dbString; } }

        public DataContextFactory Context
        {
            get
            {
                if (null == _context)
                {
                    lock (syncObj)
                    {
                        if (null == _context)
                        {
                            _context = new Data.DataContextFactory(this.DbString);
                        }
                    }
                }
                return _context;
            }
        }

        public ServiceBase() { }

        public ServiceBase(string dbString)
        {
            this.dbString = dbString;
        }
    }

}
