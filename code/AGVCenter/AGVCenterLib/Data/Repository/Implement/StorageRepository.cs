using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StorageRepository : RepositoryBase<Storage>, IStorageRepository
    {
        private AgvWarehouseDataContext context;


        public StorageRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(Storage entity)
        {
            this.context.Storage.InsertOnSubmit(entity);
        }

        public void Delete(Storage entity)
        {
            this.context.Storage.DeleteOnSubmit(entity);
        }

        public Storage FindByPositionNr(string positionNr)
        {
            return this.context.Storage.FirstOrDefault(s => s.PositionNr == positionNr);
        }

        public StorageUniqueItemView FindViewByPositionNr(string positionNr)
        {
            return this.context.StorageUniqueItemView.FirstOrDefault(s => s.PositionNr == positionNr);
        }

        public Storage FindByUniqNr(string uniqNr)
        {
            return this.context.Storage.FirstOrDefault(s => s.UniqItemNr == uniqNr);
        }

        public Storage FindByPositionNrOrUniqNr(string positionNr, string uniqNr)
        {
            return this.context.Storage.FirstOrDefault(s => s.PositionNr == positionNr || s.UniqItemNr==uniqNr);
        }

        public IQueryable<StorageUniqueItemView> SearchDetail(StorageSearchModel searchModel)
        {
            var q = this.context.StorageUniqueItemView as IQueryable<StorageUniqueItemView>;

            if (!string.IsNullOrEmpty(searchModel.Nr))
            {
                q = q.Where(s => s.UniqItemNr.Contains(searchModel.Nr));
            }


            if (!string.IsNullOrEmpty(searchModel.KNr))
            {
                q = q.Where(s => s.UniqueItemKNr.Contains(searchModel.KNr));
            }

            if (!string.IsNullOrEmpty(searchModel.PartNrAct))
            {
                q = q.Where(s => s.PartNr==searchModel.PartNrAct);
            }

            if (!string.IsNullOrEmpty(searchModel.PositionNr))
            {
                q = q.Where(s => s.PositionNr.Contains(searchModel.PositionNr));
            }

            if (searchModel.BoxTypeId.HasValue)
            {
                q = q.Where(s => s.UniqueItemBoxTypeId == searchModel.BoxTypeId);
            }

            if (searchModel.RoadMachineIndex.HasValue)
            {
                q = q.Where(s => s.PositionRoadMachineIndex == searchModel.RoadMachineIndex);
            }

            if (searchModel.FIFOStart.HasValue)
            {
                q = q.Where(s => s.FIFO >= searchModel.FIFOStart);
            }

            if (searchModel.FIFOEnd.HasValue)
            {
                q = q.Where(s => s.FIFO <= searchModel.FIFOEnd);
            }

            return q;
        }

        public List<Storage> All()
        {
            return this.context.Storage.ToList();
        }

        //public MoveStockModel FindMoveStockForAutoMove(int roadmachineIndex, bool isSelfAreaMove = false)
        //{
        //    //IPositionRepository positionRepo = new PositionRepository(this._dataContextFactory);

        //    //MoveStockModel moveModel = null;
        //    //StorageUniqueItemView storageUniqueItemView = null;
        //    //Position storagePosition = null;
        //    //WarehouseArea storageWarehouseArea = null;

        //    //var storagePositionList = this.context.StoragePositionView.ToList()
        //    //    .Select(s => new
        //    //    {
        //    //        Id = s.Id,
        //    //        PositionNr = s.PositionNr,
        //    //        Fifo = s.FIFO,
        //    //        WarehouseAreaNr = s.WarehouseAreaNr,
        //    //        Floor = s.Floor,
        //    //        Cloumn = s.Column
        //    //    })
        //    //    .Distinct().ToList().OrderBy(s => s.Fifo).ToList();

        //    //var firstEmptyPosition = this.context.PositionStorageView.ToList()
        //    //    .Where(s => (s.StorageId == null) && (s.IsLocked == false))
        //    //    .OrderBy(s => s.WarehouseAreaNr).ThenBy(s => s.Floor).ThenBy(s => s.Column).FirstOrDefault();

        //    //Position toPosition = positionRepo.FindByNr(firstEmptyPosition.Nr);
        //    //WarehouseArea toPositionWarehouseArea = toPosition.WarehouseArea;

        //    //foreach (var storage in storagePositionList)
        //    //{
        //    //    // 判断当前库存是否需要调库至该轮最优库位
        //    //    storagePosition = positionRepo.FindByNr(storage.PositionNr);
        //    //    storageWarehouseArea = storagePosition.WarehouseArea;

        //    //    if (!this.IsStorageBestPosition(storagePosition, storageWarehouseArea, toPosition, toPositionWarehouseArea))
        //    //    {
        //    //        storageUniqueItemView = this.context.StorageUniqueItemView.Where(s => s.Id == storage.Id).FirstOrDefault();
        //    //        moveModel = new MoveStockModel()
        //    //        {
        //    //            FromStorage = storageUniqueItemView,
        //    //            FromPosition = storagePosition,
        //    //            ToPosition = toPosition
        //    //        };
        //    //    }
        //    //}

        //    //return moveModel;
        //}

        public MoveStockModel FindMoveStockForAutoMove(int roadmachineIndex, bool isSelfAreaMove = false)
        {
            MoveStockModel moveModel = null;
            var warehouseAreas = this.context.WarehouseArea.OrderBy(s => s.InStorePriority).ToList();

            IPositionRepository positionRepo = new PositionRepository(this._dataContextFactory);

            foreach (var area in warehouseAreas)
            {

                List<string> toAreas = warehouseAreas.Where(s => s.InStorePriority < area.InStorePriority).Select(s => s.Nr).ToList();
                List<string> fromAreas = warehouseAreas.Where(s => s.InStorePriority >= area.InStorePriority).Select(s => s.Nr).ToList();

                if (toAreas.Count > 0 && fromAreas.Count > 0)
                {
                    moveModel = this.FindFromStorageAndToPosition(fromAreas, toAreas, roadmachineIndex);
                    if (moveModel != null)
                    {
                        break;
                    }
                }
            }
            return moveModel;
        }

        public StorageUniqueItemView FindFirstStorageByWarehouseAreaNr(string warehouseAreaNr, int roadMachineIndex)
        {
            return this.context.StorageUniqueItemView
                .Where(s => s.PositionWarehouseAreaNr == warehouseAreaNr && s.PositionRoadMachineIndex==roadMachineIndex)
                .OrderBy(s => s.FIFO)
                .FirstOrDefault();
            
        }

        public StorageUniqueItemView FindFirstStorageByWarehouseAreaNrs(List<string> warehouseAreaNrs,int roadMachineIndex)
        {
            return this.context.StorageUniqueItemView
                .Where(s => warehouseAreaNrs.Contains(s.PositionWarehouseAreaNr) && s.PositionRoadMachineIndex==roadMachineIndex)
                .OrderBy(s => s.FIFO)
                .FirstOrDefault();
        }



        private bool IsStorageBestPosition(Position storagePosition, WarehouseArea storageWarehouseArea,
            Position toPosition, WarehouseArea toPositionWarehouseArea)
        {
            bool result = false;

            if (storageWarehouseArea.InStorePriority < toPositionWarehouseArea.InStorePriority)
            {
                result = true;
            }
            else if (storageWarehouseArea.InStorePriority == toPositionWarehouseArea.InStorePriority)
            {
                if (storagePosition.Floor < toPosition.Floor)
                {
                    result = true;
                }else if (storagePosition.Floor == toPosition.Floor)
                {
                    if (storagePosition.Column < toPosition.Column)
                    {
                        result = true;
                    }
                }

            }

            return result;
        }

        private MoveStockModel SearchFromStorageAndToPosition(string fromPositionNr, string fromStorageId, int toPositionNr)
        {

            return null;
        }

        private MoveStockModel FindFromStorageAndToPosition(List<string> fromAreaNrs,
            List<string> toAreaNrs,
            int roadmachineIndex)
        {
            PositionRepository positionRepo = new PositionRepository(this._dataContextFactory);

            var storage = this.FindFirstStorageByWarehouseAreaNrs(fromAreaNrs,roadmachineIndex);
            var toPosition = positionRepo.FindCanInStockByRoadMachineAndSortPrority(roadmachineIndex, toAreaNrs);

            if (storage != null && toPosition != null
                && storage.PositionNr != toPosition.Nr
                && storage.PositionWarehouseAreaNr != toPosition.WarehouseAreaNr)
            {
                var fromPosition = positionRepo.FindByNr(storage.PositionNr);
                if (toPosition.InStorePriority <= fromPosition.InStorePriority)
                {
                    return new MoveStockModel()
                    {
                        FromStorage = storage,
                        FromPosition = fromPosition,
                        ToPosition = toPosition
                    };
                }
            }
            return null;

        }

        private MoveStockModel FindFromStorageAndToPosition(string fromAreaNr,
            string toAreaNr,
            int roadmachineIndex)
        {
            PositionRepository positionRepo = new PositionRepository(this._dataContextFactory);

            var storage = this.FindFirstStorageByWarehouseAreaNr(fromAreaNr,roadmachineIndex);
            var toPosition = positionRepo.FindCanInStockByRoadMachineAndSortPrority(roadmachineIndex, toAreaNr);
            if (storage != null && toPosition != null && storage.PositionNr != toPosition.Nr)
            {
                var fromPosition = positionRepo.FindByNr(storage.PositionNr);
                if (fromAreaNr != toAreaNr || toPosition.InStorePriority <= fromPosition.InStorePriority)
                {
                    return new MoveStockModel()
                    {
                        FromStorage = storage,
                        FromPosition = fromPosition,
                        ToPosition = toPosition
                    };
                }
            }
            return null;
        }
    }
}
