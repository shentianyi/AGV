using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IDeliveryItemRepository
    {
        void Creates(List<DeliveryItem> entities);
        DeliveryItem FindByUniqNr(string uniqNr);
        DeliveryItem FindByUniqNr(string uniqNr, string deliveryNr);
        IQueryable<DeliveryItemStorageView> SearchDetail(DeliveryItemSearchModel searchModel);
    }
}
