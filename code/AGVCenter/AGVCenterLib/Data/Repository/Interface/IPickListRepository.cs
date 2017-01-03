using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IPickListRepository
    {
        void Create(PickList entity);

        PickList FindByNr(string nr);
        
        IQueryable<PickList> Search(PickListSearchModel searchModel);

        void DeletePickListForTest(string pickListNr);

        /// <summary>
        /// 获取择货单的库存列表，
        /// </summary>
        /// <param name="nr"></param>
        /// <param name="all">默认false，带有库存的；true，全部</param>
        /// <returns></returns>
        List<PickListStorageView> GetStorageList(string nr, bool all = false);

    }
}
