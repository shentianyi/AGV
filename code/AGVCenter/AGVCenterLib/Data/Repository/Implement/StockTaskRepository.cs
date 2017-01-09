using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Data.Repository.Interface;
using AGVCenterLib.Enum;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Implement
{
    public class StockTaskRepository : RepositoryBase<StockTask>, IStockTaskRepository
    {
        private AgvWarehouseDataContext context;
        public StockTaskRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWarehouseDataContext;
        }

        public void Create(StockTask entity)
        {
            this.context.StockTask.InsertOnSubmit(entity);
        }

        public void Creates(List<StockTask> entities)
        {

            this.context.StockTask.InsertAllOnSubmit(entities);
        }

        public StockTask FindById(int id)
        {
            return this.context.StockTask.FirstOrDefault(s => s.Id == id);
        }

        public StockTask FindLastByCheckCode(string checkCode)
        {
            return this.context.StockTask.OrderByDescending(s=>s.Id).FirstOrDefault(s => s.BarCode == checkCode);
        }

        public List<StockTask> GetByState(StockTaskState state)
        {
            return this.context.StockTask.Where(s => s.State == (int)state).ToList();
        }


        public void UpdateTasksState(List<int> taskIds, StockTaskState state)
        {
            string cmd = string.Format("update stocktask set state={0} where Id in ({1});",
                (int)state,
                string.Join(",", taskIds.ToArray()));
            this.context.ExecuteCommand(cmd);
        }



        public List<StockTask> GetOutStockTaskByPickList(string pickListNr)
        {
            return this.context.StockTask.Where(s => s.PickBatchId == pickListNr).ToList();
        }

        public StockTask FindLastByNr(string nr)
        {
            return this.context.StockTask.OrderByDescending(s => s.Id).FirstOrDefault(s => s.BarCode == nr);
        }

        public List<StockTask> GetByStates(List<StockTaskState> states)
        {
            return this.context.StockTask.Where(s => states.Contains((StockTaskState)s.State)).ToList();
        }



        public StockTask GetByStatesAndRoadMachine(List<StockTaskState> states, int? roadMachineIndex=null)
        {
            var q= this.context.StockTask.Where(s => states.Contains((StockTaskState)s.State) );
            if (roadMachineIndex.HasValue)
            {
                q = q.Where(s => s.RoadMachineIndex == roadMachineIndex.Value);
            }

            return q.FirstOrDefault();
        }

        public List<StockTask> GetLast(int take = 300)
        {
            return this.context.StockTask.OrderByDescending(s => s.Id).Take(take).ToList();
        }


        public IQueryable<StockTask> Search(StockTaskSearchModel searchModel)
        { 
            var q = this.context.StockTask as IQueryable<StockTask>;

            if (!string.IsNullOrEmpty(searchModel.UniqItemNr))
            {
                q = q.Where(s => s.BarCode.Contains(searchModel.UniqItemNr));
            }

            if (!string.IsNullOrEmpty(searchModel.PositionNr))
            {
                q = q.Where(s => s.PositionNr.Contains(searchModel.PositionNr));
            }

            if (searchModel.CreatedAtStart.HasValue)
            {
                q = q.Where(s => s.CreatedAt >= searchModel.CreatedAtStart);
            }

            if (searchModel.CreatedAtEnd.HasValue)
            {
                q = q.Where(s => s.CreatedAt <= searchModel.CreatedAtEnd);
            }


            return q;
        }
    }
}
