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

        public MoveStockModel FindMoveStockForMove(int roadmachineIndex, bool isSelfAreaMove = false)
        {
            MoveStockModel moveModel = null();
            var warehouseAreas = this.context.StorageUniqueItemView
                .Select(s => new
                {
                    WarehouseAreaNr = s.PositionWarehouseAreaNr,
                    WarehouseAreaInStorePriority = s.WarehouseAreaInStorePriority
                })
                .Distinct().ToList().OrderBy(s => s.WarehouseAreaInStorePriority).ToList();

            var firstArea = warehouseAreas.OrderBy(s => s.WarehouseAreaInStorePriority).FirstOrDefault();

            IPositionRepository positionRepo = new PositionRepository(this._dataContextFactory);

            foreach (var area in warehouseAreas)
            {
                // 最高级进行是否自我移库的判断
                if (firstArea.WarehouseAreaNr == area.WarehouseAreaNr)
                {
                    if (isSelfAreaMove)
                    {
                        moveModel= this.FindFromStorageAndToPosition(area.WarehouseAreaNr, area.WarehouseAreaNr, roadmachineIndex);
                        break;
                    }
                }
                else
                {
                    var highAreas = this.context.WarehouseArea
                        .Where(s => s.InStorePriority < area.WarehouseAreaInStorePriority)
                        .OrderBy(s => s.InStorePriority).ToList();

                }
            }
            return moveModel;
        }

        public StorageUniqueItemView FindFirstStorageByWarehouseAreaNr(string warehouseAreaNr)
        {
            return this.context.StorageUniqueItemView
                .Where(s => s.PositionWarehouseAreaNr == warehouseAreaNr)
                .OrderBy(s => s.FIFO)
                .FirstOrDefault();
            
        }

        private MoveStockModel FindFromStorageAndToPosition(string fromAreaNr,string toAreaNr,int roadmachineIndex)
        {
            IPositionRepository positionRepo = new PositionRepository(this._dataContextFactory);

            var storage = this.FindFirstStorageByWarehouseAreaNr(fromAreaNr);
            var toPosition = positionRepo.FindByRoadMachineBySortPrority(roadmachineIndex, toAreaNr);
            if (storage != null && toPosition != null)
            {
               return new MoveStockModel()
                {
                    FromStorage = storage,
                    ToPosition = toPosition
                };
            }
            return null;
        }
    }
}
