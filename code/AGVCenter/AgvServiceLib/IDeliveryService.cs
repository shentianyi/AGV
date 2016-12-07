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
    }
}
