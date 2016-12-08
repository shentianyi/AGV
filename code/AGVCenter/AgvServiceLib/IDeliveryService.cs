using AGVCenterLib.Model.Message;
using AgvServiceLib.DataModel;
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
        ResultMessage CanItemAddToDelivery(string uniqNr);

        [OperationContract]
        ResultMessage CreateDelivery(string delieryNr, List<string> uniqItemsNrs);
    }
}
