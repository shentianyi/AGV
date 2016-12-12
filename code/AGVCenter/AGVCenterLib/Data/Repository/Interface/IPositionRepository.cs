using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Interface
{
   public interface IPositionRepository
    {
        /// <summary>
        /// 获取仓库库位，排序
        /// </summary>
        /// <param name="roadMachineIndex"></param>
        /// <param name="exceptsNrs">不包含的库位号列表</param>
        /// <returns></returns>
        Position FindByRoadMachineAndSort(int roadMachineIndex, List<string> exceptsNrs);

        Position FindByNr(string nr);

        void Create(Position entity);

        void Creates(List<Position> entities);

        void DeleteAll();
        
    }
}
