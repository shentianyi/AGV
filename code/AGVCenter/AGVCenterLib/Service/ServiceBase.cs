using AGVCenterLib.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Service
{
    public class ServiceBase
    {
        private static object syncObj = new object();

        private string dbString;
        public string DbString
        {
            get
            {
                return dbString;
            }
            set { dbString = value; }
        }
        private AgvWarehouseDataContext context;

        public ServiceBase(string dbString)
        {
            this.dbString = dbString;
        }


        public AgvWarehouseDataContext Context
        {
            get
            {
                if (null == context)
                {
                    lock (syncObj)
                    {
                        if (null == context)
                        {
                            context = new AgvWarehouseDataContext(this.DbString);
                        }
                    }
                }
                return context;
            }
        }   
    }
}
