using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Service
{
    public class DeliveryItemService : ServiceBase
    {

        public DeliveryItemService(string dbString) : base(dbString)
        {

        }

        public bool DeliveryItemExsits(string uniqNr)
        {
            return this.FindByUniqNr(uniqNr) != null;
        }

        public DeliveryItem FindByUniqNr(string uniqNr)
        {
            return new DeliveryItemRepository(this.Context).FindByUniqNr(uniqNr);
        }

        public ResultMessage CanItemAddToDelivery(string uniqNr)
        {
            ResultMessage message = new ResultMessage();
            UniqueItemService service = new UniqueItemService(this.DbString);
            UniqueItem item = service.FindByNr(uniqNr);

            if (item == null)
            {
                message.MessageType = MessageType.Warn;
                message.Success = false;
                message.Content = "产品不存在不可添加";
                return message;
            }

            //if (item.State == (int)UniqueItemState.Sent)
            //{

            //}

            if (this.DeliveryItemExsits(uniqNr))
            {
                message.MessageType = MessageType.Warn;
                message.Success = false;
                message.Content = "产品已在其它运单中，不可重复添加！";
                return message;
            }

            message.MessageType = MessageType.OK;
            message.Success = true;
            
            return message;
        }
    }
}
