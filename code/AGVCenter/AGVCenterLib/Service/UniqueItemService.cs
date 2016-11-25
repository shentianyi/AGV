using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;

namespace AGVCenterLib.Service
{
    public class UniqueItemService: ServiceBase
    {

        public UniqueItemService(string dbString):base(dbString)
        { 
        }

        /// <summary>
        /// 判断唯一件是否可以入库
        /// </summary>
        /// <param name="uniqNr"></param>
        /// <returns></returns>
        public bool CanUniqInStock(string uniqNr)
        {
            return true;
        }

        public UniqueItemView FindDetail(string uniqNr)
        {
           return this.Context.UniqueItemView.FirstOrDefault(s => s.UniqNr == uniqNr);
        }
    }
}
