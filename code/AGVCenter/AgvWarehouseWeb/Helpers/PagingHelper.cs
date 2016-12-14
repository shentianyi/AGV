using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgvWarehouseWeb.Helpers
{
    public class PagingHelper
    {
        /// <summary>
        /// get page index, start from 0
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static int GetPageIndex(int? page)
        {
            return page.HasValue ? (page.Value <= 0 ? 0 : page.Value - 1) : 0;
        }
    }
}