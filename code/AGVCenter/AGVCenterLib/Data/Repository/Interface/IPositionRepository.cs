using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
   public interface IPositionRepository
    {
        /// <summary>
        /// 获取仓库库位，排序
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <param name="exceptsNrs">不包含的库位号列表</param>
        /// <param name="lockPosition">是否锁定返回的库位，默认为false</param>
        /// <returns></returns>
        Position FindAvaliableByRoadMachineAndSort(int roadMachineIndex, List<string> exceptsNrs,bool lockPosition=false,bool byInStorePriority=false);

        Position FindByNr(string nr);

        void Create(Position entity);

        void Creates(List<Position> entities);

        void DeleteAll();

        IQueryable<Position> Search(PositionSearchModel searchModel);

        /// <summary>
        /// 根据分区、巷道机并且进行优先级从高到低排序，获取库位
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <param name="warehouseAreaNr"></param>
        /// <returns></returns>
          Position FindByRoadMachineBySortPrority(int roadMachineIndex, string warehouseAreaNr);

        IQueryable<Position> GetSortedPositionsList();

    }
}
