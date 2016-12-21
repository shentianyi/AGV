using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using AGVCenterLib.Model.ViewModel;
using AgvServiceLib.Helper;

namespace AgvServiceLib
{
    public class TrayService : ITrayService
    {
        AGVCenterLib.Service.TrayService ts;

        public TrayService()
        {
            ts = new AGVCenterLib.Service.TrayService(SqlHelper.ConnectStr);
        }

        public List<TrayDeliveryViewModel> GetTrayListByDeliveryNr(string deliveryNr)
        {
            return TrayDeliveryViewModel.Converts(ts.GetTrayListByDeliveryNr(deliveryNr));
        }

        public List<DeliveryItemStorageViewModel> GetTrayItemDetails(string trayNr)
        {
            return DeliveryItemStorageViewModel.Converts(
                new AGVCenterLib.Service.DeliveryItemService(SqlHelper.ConnectStr).SearchDetail(new AGVCenterLib.Model.SearchModel.DeliveryItemSearchModel()
                {
                    TrayNrAct = trayNr
                }).ToList());
        }
    }
}
