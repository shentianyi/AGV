using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IPickListItemRepository
    {
        void Creates(List<PickListItem> entities);
        PickListItem FindByUniqNr(string uniqNr);
        PickListItem FindByUniqNr(string uniqNr, string pickListNr);
    }
}
