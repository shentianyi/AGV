using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.LogUtil;
using AGVCenterLib.Model.SearchModel;
using AGVCenterLib.Model.ViewModel;

namespace AGVCenterLib.Service
{
    public class PickListService : ServiceBase
    {

        public PickListService(string dbString) : base(dbString)
        {

        }

        public bool PickListExsits(string nr)
        {
            return this.FindByNr(nr) != null;
        }

        public PickList FindByNr(string nr)
        {
            return new PickListRepository(this.Context).FindByNr(nr);
        }

        /// <summary>
        /// 获取择货单产品列表
        /// </summary>
        /// <param name="nr">择货单号</param>
        /// <returns></returns>
        public List<UniqueItem> GetPickListUniqItemsByNr(string nr)
        {
            return new UniqueItemRepository(this.Context).ListByPickListNr(nr);
        }

        
        /// <summary>
        /// 创建择货单
        /// </summary>
        /// <param name="pickListNr"></param>
        /// <param name="uniqItemNrs"></param>
        /// <returns></returns>
        public ResultMessage CreatePickList(string pickListNr, List<string> uniqItemNrs)
        {

            ResultMessage message = new ResultMessage();
            try
            {
                if (uniqItemNrs == null || uniqItemNrs.Count == 0)
                {
                    message.Content = "择货单项不可为空";
                    return message;
                }

                if (this.PickListExsits(pickListNr))
                {
                    message.Content = "择货单已存在不可重复创建";
                    return message;
                }
                 
                PickList pickList = new PickList()
                {
                    Nr = pickListNr,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                List<PickListItem> items = new List<PickListItem>();

                PickListItemService pis = new PickListItemService(this.DbString);

                foreach (var uniqNr in uniqItemNrs)
                {
                    var msg = pis.CanItemAddToPickList(uniqNr);
                    if (msg.Success)
                    {
                        items.Add(new PickListItem()
                        {
                            PickListNr = pickListNr,
                            UniqItemNr = uniqNr,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now
                        });
                    }
                    else
                    {
                        message.Content = string.Format("{0} 不可加入此择货单: {1}", uniqNr, msg.Content);
                        return message;
                    }
                }
                IPickListRepository pickRep = new PickListRepository(this.Context);
                pickRep.Create(pickList);
                IPickListItemRepository pickListItemRep = new PickListItemRepository(this.Context);
                pickListItemRep.Creates(items);
                this.Context.SaveAll();
                message.Success = true;
                message.MessageType = MessageType.OK;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                message.MessageType = MessageType.Exception;
                message.Content = ex.Message;
            }
            return message;
        }

     
        /// <summary>
        /// 根据择货单获取出库任务
        /// </summary>
        /// <param name="nr"></param>
        /// <returns></returns>
        public List<StockTask> GetPickListOutStockTasks(string nr)
        {
            return new StockTaskRepository(this.Context).GetOutStockTaskByPickList(nr);
        }

        public List<PickList> SearchList(PickListSearchModel searchModel,int limit=50)
        {
            IPickListRepository pickListRep = new PickListRepository(this.Context);

            return pickListRep.Search(searchModel).OrderByDescending(s => s.CreatedAt).Take(limit).ToList();
        }

        public IQueryable<PickList> Search(PickListSearchModel searchModel)
        {
            return new PickListRepository(this.Context).Search(searchModel);
        }

        public void DeletePickListForTest(string pickListNr){
              new PickListRepository(this.Context).DeletePickListForTest(pickListNr);
        }

        public List<PickListStorageView> GetPickListStorageByNr(string nr)
        {
            return new PickListRepository(this.Context).GetStorageList(nr,true);
        }
    }
}
