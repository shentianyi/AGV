using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterLib.Service
{
    public class StorageService : ServiceBase
    {
        public StorageService(string dbString) : base(dbString)
        {

        }

        public ResultMessage InStockByCheckCode(string positionNr, string checkCode)
        {
            ResultMessage message = new ResultMessage();
            IPositionRepository posiRep = new PositionRepository(this.Context);
            Position posi = posiRep.FindByNr(positionNr);
            if (posi == null)
            {
                message.Content = string.Format("库位{0}不存在", positionNr);
                return message;
            }

            IUniqueItemRepository itemRep = new UniqueItemRepository(this.Context);
            UniqueItem item = itemRep.FindByCheckCode(checkCode);

            if (item == null)
            {
                message.Content = string.Format("产品{0}不存在", checkCode);
                return message;
            }
            message = this.InStock(posi, item);

            return message;
        }

        public ResultMessage InStock(Position position, UniqueItem item)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!item.IsCanInStockState)
                {
                    message.Content = "当前状态不可入库！";
                    return message;
                }

                IStorageRepository storageRep = new StorageRepository(this.Context);
                Storage storage = storageRep.FindByPositionNr(position.Nr);
                if (storage != null)
                {
                    message.Content = string.Format("库位{0}已使用，不可入库！", position.Nr);
                    return message;
                }

                #region init instance
                Storage newStorage = new Storage()
                {
                    PositionNr = position.Nr,
                    PartNr = item.PartNr,
                    UniqItemNr = item.Nr,
                    FIFO = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                StockMovement movement = new StockMovement()
                {
                    AimedPosition = position.Nr,
                    Type = (int)StockMovementType.In,
                    Time = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
                #endregion
                IStockMovementRepository smRep = new StockMovementRepository(this.Context);
                item.State = (int)UniqueItemState.InStocked;
                item.UpdatedAt = DateTime.Now;

                storageRep.Create(newStorage);
                smRep.Create(movement);

                this.Context.SaveAll();
                message.Success = true;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                message.MessageType = MessageType.Exception;
                message.Content = ex.Message;
            }
            return message;
        }
    }
}
