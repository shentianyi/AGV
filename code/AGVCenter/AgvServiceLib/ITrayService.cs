using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using AGVCenterLib.Model.ViewModel;

namespace AgvServiceLib
{
    [ServiceContract]
    public interface ITrayService
    {
        [OperationContract]
        List<TrayDeliveryViewModel> GetTrayListByDeliveryNr(string deliveryNr);
    }
}
