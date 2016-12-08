using AgvServiceLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Service;
using AGVCenterLib.Data;
using AgvServiceLib.DataModel;

namespace AgvServiceLib
{
    public class DeliveryService : IDeliveryService
    {
        public bool DeliveryExists(string nr)
        {
            AGVCenterLib.Service.DeliveryService ds = new AGVCenterLib.Service.DeliveryService(SqlHelper.connectStr);
            return ds.DeliveryExsits(nr);
        }

        public ResultMessage CanItemAddToDelivery(string uniqNr)
        {
            DeliveryItemService dis = new DeliveryItemService(SqlHelper.connectStr);
            return dis.CanItemAddToDelivery(uniqNr);
        }

        public ResultMessage CreateDelivery(string delieryNr, List<string> uniqItemsNrs)
        {
            return new AGVCenterLib.Service.DeliveryService(SqlHelper.connectStr)
                .CreateDelivery(delieryNr, uniqItemsNrs);
        }
    }
}
