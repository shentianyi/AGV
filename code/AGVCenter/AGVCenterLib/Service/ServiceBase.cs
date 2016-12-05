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
        private DataContext context;

        public ServiceBase(string dbString)
        {
            this.dbString = dbString;
        }


        public DataContext Context
        {
            get
            {
                if (null == context)
                {
                    lock (syncObj)
                    {
                        if (null == context)
                        {
                            context = new DataContext(this.DbString);
                        }
                    }
                }
                return context;
            }
        }   
    }
}
