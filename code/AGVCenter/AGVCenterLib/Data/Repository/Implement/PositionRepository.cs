using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        private AgvWarehouseDataContext context;


        public PositionRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(Position entity)
        {
            this.context.Position.InsertOnSubmit(entity);
        }

        public void Creates(List<Position> entities)
        {
            this.context.Position.InsertAllOnSubmit(entities);
        }

        public void DeleteAll()
        {
            this.context.Storage.DeleteAllOnSubmit(this.context.Storage.Where(s => true));
            this.context.Position.DeleteAllOnSubmit(this.context.Position.Where(s=>true));
        }

        public Position FindByNr(string nr)
        {
            return this.context.Position.FirstOrDefault(s => s.Nr == nr);
        }

        public Position FindByRoadMachineAndSort(int roadMachineIndex, List<string> exceptsNrs, bool lockPosition = false)
        {
            //PositionStorageView ps = this.context.PositionStorageView
            //    .Where(s => (!exceptsNrs.Contains(s.Nr))
            //    && s.RoadMachineIndex==roadMachineIndex
            //    && (s.StorageId==null && s.isLocked==false))
            //    .OrderBy(s => s.Row).ThenBy(s => s.Column).ThenBy(s => s.Floor).FirstOrDefault();

            //PositionStorageView ps = this.context.PositionStorageView
            //  .Where(s => (!exceptsNrs.Contains(s.Nr))
            //  && s.RoadMachineIndex == roadMachineIndex
            //  && (s.StorageId == null && s.isLocked == false))
            //  .OrderByDescending(s => s.Column)
            //  .ThenBy(s => s.Row)
            //  .ThenBy(s => s.Floor)
            //  .FirstOrDefault();


            PositionStorageView ps = this.context.PositionStorageView
              .Where(s => (!exceptsNrs.Contains(s.Nr))
              && s.RoadMachineIndex == roadMachineIndex
              && (s.StorageId == null && s.IsLocked == false))
              .OrderBy(s => s.Floor)
              .ThenByDescending(s => s.Column)
              .ThenBy(s => s.Row)
              .FirstOrDefault();


            if (ps != null)
            {
                if (lockPosition)
                {
                    Position p = this.context.Position.FirstOrDefault(s => s.Nr == ps.Nr);
                    if (p != null)
                    {
                        p.IsLocked = true;
                        this.context.SubmitChanges();
                    }
                }
                return new Position()
                {
                    Nr = ps.Nr,
                    Floor = ps.Floor,
                    Column = ps.Column,
                    Row = ps.Row,
                    RoadMachineIndex = ps.RoadMachineIndex,
                    State = ps.State,
                    WarehouseNr = ps.WarehouseNr,
                    IsLocked = ps.IsLocked
                };
            }
            else
            {
                return null;
            }
        }

        public IQueryable<Position> Search(PositionSearchModel searchModel)
        {
            var q = this.context.Position as IQueryable<Position>;

            if (!string.IsNullOrEmpty(searchModel.Nr))
            {
                q = q.Where(s => s.Nr.Contains(searchModel.Nr));
            }

            if (!string.IsNullOrEmpty(searchModel.NrAct))
            {
                q = q.Where(s => s.Nr == searchModel.NrAct);
            }

            if (searchModel.IsLocked.HasValue)
            {
                q = q.Where(s => s.IsLocked == searchModel.IsLocked);
            }

            return q;
        }
    }
}
