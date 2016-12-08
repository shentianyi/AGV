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
    public class TrayItemService : ServiceBase
    {

        public TrayItemService(string dbString) : base(dbString)
        {

        }

        public bool TrayItemExsits(string uniqNr)
        {
            return this.FindByUniqNr(uniqNr) != null;
        }

        public TrayItem FindByUniqNr(string uniqNr)
        {
            return new TrayItemRepository(this.Context).FindByUniqNr(uniqNr);
        }

        public ResultMessage CanItemAddToTray(string uniqNr)
        {
            ResultMessage message = new ResultMessage();
            UniqueItemService service = new UniqueItemService(this.DbString);
            UniqueItem item = service.FindByNr(uniqNr);

            if (item == null)
            {
                message.Content = "产品不存在不可添加";
                return message;
            }

            if (this.TrayItemExsits(uniqNr))
            {
                message.Content = "产品已在其它托中，不可重复添加！";
                return message;
            }

            message.MessageType = MessageType.OK;
            message.Success = true;

            return message;
        }
    }
}
