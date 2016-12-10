using AGVCenterLib.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace AGVCenterLib.Data.Repository.Implement
{
    class PositionRepository : RepositoryBase<Position>, IPositionRepository
    {
        private AgvWarehouseDataContext context;


        public PositionRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
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

        public Position FindByRoadMachineAndSort(int roadMachineIndex, List<string> exceptsNrs)
        {
            PositionStorage ps = this.context.PositionStorage
                .Where(s => (!exceptsNrs.Contains(s.Nr))
                && s.RoadMachineIndex==roadMachineIndex
                && s.StorageId==null)
                .OrderBy(s => s.Row).ThenBy(s => s.Column).ThenBy(s => s.Floor).FirstOrDefault();
            if (ps != null)
            {
                return new Position()
                {
                    Nr = ps.Nr,
                    Floor = ps.Floor,
                    Column = ps.Column,
                    Row = ps.Row,
                    RoadMachineIndex = ps.RoadMachineIndex,
                    State = ps.State,
                    WarehouseNr = ps.WarehouseNr
                };
            }
            else
            {
                return null;
            }
        }
    }
}
