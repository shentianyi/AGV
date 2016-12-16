using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IUniqueItemRepository
    {
        UniqueItem FindByCheckCode(string checkCode);
        UniqueItem FindByNr(string nr);
        void Create(UniqueItem entity);

        void Delete(UniqueItem entity);

        void Update(UniqueItem entity);


        List<UniqueItem> ListByDeliveryNr(string deliveryNr);

        IQueryable<UniqueItem> Search(UniqueItemSearchModel searchModel);
    }
}
