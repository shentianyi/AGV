using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Service
{
    public class PickListItemService : ServiceBase
    {

        public PickListItemService(string dbString) : base(dbString)
        {

        }

        public bool PickListItemExsits(string uniqNr)
        {
            return this.FindByUniqNr(uniqNr) != null;
        }

        public PickListItem FindByUniqNr(string uniqNr)
        {
            return new PickListItemRepository(this.Context).FindByUniqNr(uniqNr);
        }

        public ResultMessage CanItemAddToPickList(string uniqNr)
        {
            ResultMessage message = new ResultMessage();
            UniqueItemService service = new UniqueItemService(this.DbString);
            UniqueItem item = service.FindByNr(uniqNr);

            if (item == null)
            {
                message.Content = "产品不存在不可添加";
                return message;
            }

            StorageService storageService = new StorageService(this.DbString);
            Storage storage = storageService.FindStorageByUniqNr(uniqNr);

            if (storage == null)
            {

                message.Content = "库存不存在不可添加";
                return message;
            }
            message.MessageType = MessageType.OK;
            message.Success = true;
            
            return message;
        }



        public IQueryable<PickListItemStorageView> SearchDetail(PickListItemSearchModel searchModel)
        {
            return new PickListItemRepository(this.Context).SearchDetail(searchModel);
        }
    }
}
