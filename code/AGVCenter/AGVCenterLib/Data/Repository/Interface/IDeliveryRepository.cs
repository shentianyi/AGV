using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IDeliveryRepository
    {
        void Create(Delivery entity);

        Delivery FindByNr(string nr);

        /// <summary>
        /// 获取运单的库存列表，
        /// </summary>
        /// <param name="nr"></param>
        /// <param name="all">默认false，带有库存的；true，全部</param>
        /// <returns></returns>
        List<DeliveryStorageView> GetStorageList(string nr, bool all = false);

        IQueryable<Delivery> Search(DeliverySearchModel searchModel);
    }
}
