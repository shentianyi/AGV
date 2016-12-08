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
    public class DeliveryService : ServiceBase
    {

        public DeliveryService(string dbString) : base(dbString)
        {

        }

        public bool DeliveryExsits(string nr)
        {
            return this.FindByNr(nr) != null;
        }

        public Delivery FindByNr(string nr)
        {
            return new DeliveryRepository(this.Context).FindByNr(nr);
        }

        public ResultMessage CreateDelivery(string deliveryNr, List<string> uniqItemNrs)
        {

            ResultMessage message = new ResultMessage();
            try
            {
                if (uniqItemNrs == null || uniqItemNrs.Count == 0)
                {
                    message.Content = "运单不可为空";
                    return message;
                }

                if (this.DeliveryExsits(deliveryNr))
                {
                    message.Content = "运单已存在不可重复创建";
                    return message;
                }
                 
                Delivery delivery = new Delivery()
                {
                    Nr = deliveryNr,
                    State = (int)DeliveryState.Init,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                List<DeliveryItem> items = new List<DeliveryItem>();

                DeliveryItemService dis = new DeliveryItemService(this.DbString);

                foreach (var uniqNr in uniqItemNrs)
                {
                    var msg = dis.CanItemAddToDelivery(uniqNr);
                    if (msg.Success)
                    {
                        items.Add(new DeliveryItem()
                        {
                            DeliveryNr = deliveryNr,
                            UniqItemNr = uniqNr,
                            CreatedAt = DateTime.Now
                        });
                    }
                    else
                    {
                        message.Content = string.Format("{0} 不可加入此运单: {1}", uniqNr, msg.Content);
                        return message;
                    }
                }
                IDeliveryRepository deliveryRep = new DeliveryRepository(this.Context);
                deliveryRep.Create(delivery);
                IDeliveryItemRepository deliveryItemRep = new DeliveryItemRepository(this.Context);
                deliveryItemRep.Creates(items);
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
