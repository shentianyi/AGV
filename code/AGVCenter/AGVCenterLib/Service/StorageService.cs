using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AGVCenterLib.Data;
using AGVCenterLib.Data.Repository.Implement;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.SearchModel;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterLib.Service
{
    public class StorageService : ServiceBase
    {
        public StorageService(string dbString) : base(dbString)
        {

        }


        public StorageService(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="positionNr">库位号</param>
        /// <param name="uniqItemNr">产品标签码</param>
        /// <returns></returns>
        public ResultMessage InStockByUniqItemNr(string positionNr, string uniqItemNr)
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
            //  UniqueItem item = itemRep.FindByCheckCode(barCode);
            UniqueItem item = itemRep.FindByNr(uniqItemNr);
            if (item == null)
            {
                message.Content = string.Format("产品{0}不存在", uniqItemNr);
                return message;
            }
            message = this.InStock(posi, item);

            return message;
        }

        public Storage FindByPositionNr(string positionNr)
        {
            return new StorageRepository(this.Context).FindByPositionNr(positionNr);
        }


        public StorageUniqueItemView FindViewByPositionNr(string positionNr)
        {
            return new StorageRepository(this.Context).FindViewByPositionNr(positionNr);
        }

        /// <summary>
        /// 入库
        /// </summary>
        /// <param name="position">库位</param>
        /// <param name="item">产品</param>
        /// <returns></returns>
        public ResultMessage InStock(Position position, UniqueItem item)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                //if (!item.IsCanInStockState)
                //{
                //    message.Content = "当前状态不可入库！";
                //    return message;
                //}

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
                    UniqItemNr=item.Nr,
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
        /// 根据产品标签码出库
        /// </summary>
        /// <param name="uniqItemNr">产品标签码</param>
        /// <returns></returns>
        public ResultMessage OutStockByUniqItemNr(string uniqItemNr)
        {
          //  return null;
            ResultMessage message = new ResultMessage();
            IUniqueItemRepository itemRep = new UniqueItemRepository(this.Context);
            UniqueItem item = itemRep.FindByNr(uniqItemNr);

            if (item == null)
            {
                message.Content = string.Format("产品{0}不存在", uniqItemNr);
                return message;
            }

            IStorageRepository storageRep = new StorageRepository(this.Context);
            Storage storage = storageRep.FindByUniqNr(item.Nr);
            if (storage == null)
            {
                message.Content = string.Format("产品{0}未入库，不可出库", uniqItemNr);
                return message;
            }

            message = this.OutStock(storage);

            return message;
        }

        /// <summary>
        /// 根据库位出库
        /// </summary>
        /// <param name="positionNr">库位</param>
        /// <returns></returns>
        public ResultMessage OutStockByPositionNr(string positionNr)
        {
            ResultMessage message = new ResultMessage();
            IPositionRepository posiRep = new PositionRepository(this.Context);
            Position posi = posiRep.FindByNr(positionNr);
            if (posi == null)
            {
                message.Content = string.Format("库位{0}不存在", positionNr);
                return message;
            }

            IStorageRepository storageRep = new StorageRepository(this.Context);
            Storage storage = storageRep.FindByPositionNr(positionNr);
            if (storage == null)
            {
                message.Content = string.Format("库位{0}不存在库存，不可出库", positionNr);
                return message;
            }

            message = this.OutStock(storage);

            return message;
        }

        /// <summary>
        /// 根据库存出库
        /// </summary>
        /// <param name="storage">库存</param>
        /// <returns></returns>
        public ResultMessage OutStock(Storage storage)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                IStorageRepository storageRep = new StorageRepository(this.Context);
                IStockMovementRepository smRep = new StockMovementRepository(this.Context);

                UniqueItem item = storage.UniqueItem;
                Position position = storage.Position;

                item.State = (int)UniqueItemState.OutStocked;
                item.UpdatedAt = DateTime.Now;
                position.IsLocked = false;

                #region 
                StockMovement movement = new StockMovement()
                {
                    UniqItemNr = item.Nr,
                    SourcePosition = storage.PositionNr,
                    Type = (int)StockMovementType.Out,
                    Time = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
                #endregion


                storageRep.Delete(storage);
                smRep.Create(movement);

                this.Context.SaveAll();
                message.Success = true;
                message.MessageType = MessageType.OK;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                message.Content = ex.Message;
                message.MessageType = MessageType.Exception;
            }
            return message;
        }

        public ResultMessage MoveStockByUniqItemNr(string uniqItemNr, string toPositionNr)
        {
            ResultMessage message = new ResultMessage();
            IUniqueItemRepository itemRep = new UniqueItemRepository(this.Context);
            UniqueItem item = itemRep.FindByNr(uniqItemNr);

            if (item == null )
            {
                message.Content = string.Format("产品{0}不存在", uniqItemNr);
                return message;
            }

            if (!item.IsCanMoveStockState)
            {
                message.Content = string.Format("产品:{0}当前状态为:{1}, 不可移库", uniqItemNr, item.StateStr);
                return message;
            }

            IStorageRepository storageRep = new StorageRepository(this.Context);
            Storage storage = storageRep.FindByUniqNr(item.Nr);
            if (storage == null)
            {
                message.Content = string.Format("产品:{0}未入库，不可移库", uniqItemNr);
                return message;
            }

            IPositionRepository posiRep = new PositionRepository(this.Context);
            Position posi = posiRep.FindByNr(toPositionNr);
            if (posi == null)
            {
                message.Content = string.Format("库位:{0}不存在", toPositionNr);
                return message;
            }

           
            Storage targetStorage = storageRep.FindByPositionNr(toPositionNr);
            if (targetStorage != null)
            {
                message.Content = string.Format("库位:{0}已使用，不可入库！", posi.Nr);
                return message;
            }

            message = this.MoveStock(storage, posi);

            return message;
        }

        public ResultMessage MoveStock(Storage  storage, Position toPosition)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                IStorageRepository storageRep = new StorageRepository(this.Context);
                IStockMovementRepository smRep = new StockMovementRepository(this.Context);

                UniqueItem item = storage.UniqueItem;
                Position fromPosition = new PositionRepository(this.Context).FindByNr(storage.PositionNr); //storage.Position;
                
                item.UpdatedAt = DateTime.Now;

                fromPosition.IsLocked = false;
                toPosition.IsLocked = true;
                storage.PositionNr = toPosition.Nr;

                #region 
                StockMovement movement = new StockMovement()
                {
                    UniqItemNr = item.Nr,
                    SourcePosition = fromPosition.Nr,
                    AimedPosition= toPosition.Nr,
                    Type = (int)StockMovementType.Move,
                    Time = DateTime.Now,
                    CreatedAt = DateTime.Now
                };
                #endregion
                
                smRep.Create(movement);

                this.Context.SaveAll();
                message.Success = true;
                message.MessageType = MessageType.OK;
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
                message.Content = ex.Message;
                message.MessageType = MessageType.Exception;
            }
            return message;

        }

        /// <summary>
        /// 查找库存详细
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public IQueryable<StorageUniqueItemView> SearchDetail(StorageSearchModel searchModel)
        {
            return new StorageRepository(this.Context).SearchDetail(searchModel);
        }

        public List<Storage> All()
        {
            return new StorageRepository(this.Context).All();
        }

        public Storage FindStorageByUniqNr(string uniqItemNr)
        {
            return new StorageRepository(this.Context).FindByUniqNr(uniqItemNr);
        }
    }
}
