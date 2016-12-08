using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Service
{
    public class TrayService : ServiceBase
    {

        public TrayService(string dbString) : base(dbString)
        {

        }

       

        public ResultMessage CreateTray(string deliveryNr,string trayNr, List<string> uniqItemNrs)
        {

            ResultMessage message = new ResultMessage();
            try
            {
                if (uniqItemNrs == null || uniqItemNrs.Count == 0)
                {
                    message.Content = "托盘项不可为空";
                    return message;
                }
                 
                Tray tray = new Tray()
                {
                    Nr = trayNr,
                    State = (int)DeliveryState.Init,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                DeliveryTray deliveryTray = new DeliveryTray()
                {
                    DeliveryNr = deliveryNr,
                    TrayNr = trayNr,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                List<TrayItem> items = new List<TrayItem>();
                
                TrayItemService tis = new TrayItemService(this.DbString);

                foreach (var uniqNr in uniqItemNrs)
                {
                    var msg = tis.CanItemAddToTray(uniqNr);
                    if (msg.Success)
                    {
                        items.Add(new TrayItem()
                        {
                            TrayNr = trayNr,
                            UniqItemNr = uniqNr,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }
                    else
                    {
                        message.Content = string.Format("{0} 不可加入此运单: {1}", uniqNr, msg.Content);
                        return message;
                    }
                }
                ITrayRepository trayRep = new TrayRepository(this.Context);
                trayRep.Create(tray);

                IDeliveryTrayRepository deliveryTrayRep = new DeliveryTrayRepository(this.Context);
                deliveryTrayRep.Create(deliveryTray);

                ITrayItemRepository trayItemRep = new TrayItemRepository(this.Context);
                trayItemRep.Creates(items);

                this.Context.SaveAll();
                message.Success = true;
                message.MessageType = MessageType.OK;
            }
            catch (Exception ex)
            {
                message.MessageType = MessageType.Exception;
                message.Content = ex.Message;
            }
            return message;
        }
    }
}
