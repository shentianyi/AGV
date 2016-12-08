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
        AGVCenterLib.Service.DeliveryService ds = new AGVCenterLib.Service.DeliveryService(SqlHelper.connectStr);

        public bool DeliveryExists(string nr)
        {
            return ds.DeliveryExsits(nr);
        }


        public ResultMessage CanDeliverySend(string nr)
        {
            return ds.CanDeliverySend(nr);
        }

        public ResultMessage CanItemAddToDelivery(string uniqNr)
        {
            DeliveryItemService dis = new DeliveryItemService(SqlHelper.connectStr);
            return dis.CanItemAddToDelivery(uniqNr);
        }

        public ResultMessage CanItemAddToTray(string uniqNr)
        {
            TrayItemService tis = new TrayItemService(SqlHelper.connectStr);
            return tis.CanItemAddToTray(uniqNr);
        }
        

        public ResultMessage CreateDelivery(string delieryNr, List<string> uniqItemsNrs)
        {
            return ds.CreateDelivery(delieryNr, uniqItemsNrs);
        }
        

        public List<UniqueItemModel> GetDeliveryUniqItemsByNr(string nr)
        {
            return UniqueItemModel.Converts(ds.GetDeliveryUniqItemsByNr(nr));
        }
    }
}
