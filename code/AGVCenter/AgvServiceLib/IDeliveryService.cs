using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AgvServiceLib
{
    [ServiceContract]
    public interface IDeliveryService
    {
        [OperationContract]
        bool DeliveryExists(string nr);

        [OperationContract]
        ResultMessage CanDeliverySend(string nr);

        [OperationContract]
        ResultMessage CanItemAddToDelivery(string uniqNr);

        [OperationContract]
        ResultMessage CanItemAddToTray(string uniqNr, string deliveryNr);

        [OperationContract]
        ResultMessage CreateDelivery(string delieryNr, List<string> uniqItemsNrs);

        [OperationContract]
        List<UniqueItemModel> GetDeliveryUniqItemsByNr(string nr);

        [OperationContract]
        List<DeliveryStorageViewModel> GetDeliveryStorageByNr(string nr);

        [OperationContract]
        ResultMessage CreateTray(string delieryNr, string trayNr, List<string> uniqItemsNrs);

        [OperationContract]
        ResultMessage CreateOutStockTaskByNr(string nr);
    }
}
