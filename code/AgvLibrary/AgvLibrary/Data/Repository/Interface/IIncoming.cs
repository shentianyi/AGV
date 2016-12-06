using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
  public  interface IIncoming
    {
        /// <summary>
        /// 查询唯一吗在UniqueItem表中是否存在
        /// </summary>
        /// <param name="UniqNr"></param>
        /// <returns></returns>
        UniqueItem SearchByUniqNr(string UniqNr);
        bool UnqiNrExist(string UniqNr);

        /// <summary>
        /// WHNr在Warehouse表中是否存在
        /// </summary>
        /// <param name="WHNr"></param>
        /// <returns></returns>
        Warehouse SearchByWHNr(string WHNr);       
        bool WHNrExist(string WHNr);


        /// <summary>
        /// 查询该UniqueItemNr是否在Storage中存在 即是否占用库位
        /// </summary>
        /// <param name="UniqueItemNr"></param>
        /// <returns></returns>
        Storage SearchByUniqueNr(string UniqueItemNr);
        bool UniqueItemNrExist(string UniqueItemNr);

        Position SearchByPositionNr(string PositionNr);

        Position SearchByPosition(string WHNr, int Floor, int Column, int Row);
       
        bool PositionNrExist(string PositionNr);
        bool PositionExist(string WHNr, int Floor, int Column, int Row);

        bool Create(Storage sg);
    }
}
