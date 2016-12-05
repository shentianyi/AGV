using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using Brilliantech.Framwork.Utils.LogUtil;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Enum;

namespace AGVCenterLib.Service
{
    public class UniqueItemService: ServiceBase
    {

        public UniqueItemService(string dbString):base(dbString)
        { 
        }

        /// <summary>
        /// 创建唯一件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ResultMessage Create(UniqueItem item)
        {
            ResultMessage messge = new ResultMessage();
            try
            {
                IUniqueItemRepository rep = new UniqueItemRepository(this.Context);
                if (rep.FindByNr(item.Nr) != null)
                {
                    messge.MessageType = MessageType.Warn;
                    messge.Content = "产品已经存在，不可重复创建！";
                }
                else
                {
                    item.State = (int)UniqueItemState.Created;
                    item.CreatedAt = DateTime.Now;
                    rep.Create(item);
                    this.Context.Context.SubmitChanges();
                    messge.Content = "创建成功";
                }
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                messge.MessageType = MessageType.Exception;
                messge.Content = ex.Message;
            }
            return messge;
        }
        /// <summary>
        /// 判断唯一件是否可以入库
        /// </summary>
        /// <param name="uniqNr"></param>
        /// <returns></returns>
        public bool CanUniqInStock(string uniqNr)
        {
            return true;
        }



        public UniqueItem FindByCheckCode(string checkCode)
        {
            IUniqueItemRepository rep = new UniqueItemRepository(this.Context);

            return rep.FindByCheckCode(checkCode);
        }
    }
}
