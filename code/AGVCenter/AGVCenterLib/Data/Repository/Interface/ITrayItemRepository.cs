using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface ITrayItemRepository
    {
        void Creates(List<TrayItem> entities);
        TrayItem FindByUniqNr(string uniqNr);
    }
}
