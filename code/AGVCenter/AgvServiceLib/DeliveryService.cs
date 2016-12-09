using AgvServiceLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Service;
using AGVCenterLib.Data;
using AGVCenterLib.Model.ViewModel;

namespace AgvServiceLib
{
    public class DeliveryService : IDeliveryService
    {
        AGVCenterLib.Service.DeliveryService ds = new AGVCenterLib.Service.DeliveryService(SqlHelper.ConnectStr);

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
            DeliveryItemService dis = new DeliveryItemService(SqlHelper.ConnectStr);
            return dis.CanItemAddToDelivery(uniqNr);
        }

        public ResultMessage CanItemAddToTray(string uniqNr, string deliveryNr)
        {
            TrayItemService tis = new TrayItemService(SqlHelper.ConnectStr);
            return tis.CanItemAddToTray(uniqNr, deliveryNr);
        }


        public ResultMessage CreateDelivery(string delieryNr, List<string> uniqItemsNrs)
        {
            return ds.CreateDelivery(delieryNr, uniqItemsNrs);
        }


        public List<UniqueItemModel> GetDeliveryUniqItemsByNr(string nr)
        {
            return UniqueItemModel.Converts(ds.GetDeliveryUniqItemsByNr(nr));
        }

        public ResultMessage CreateTray(string delieryNr, string trayNr, List<string> uniqItemsNrs)
        {
            TrayService ts = new TrayService(SqlHelper.ConnectStr);
            return ts.CreateTray(delieryNr, trayNr, uniqItemsNrs);
        }
    }
}