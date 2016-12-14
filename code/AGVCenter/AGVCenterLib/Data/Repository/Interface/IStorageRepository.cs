using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model.SearchModel;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IStorageRepository
    {
        void Create(Storage entity);
        void Delete(Storage entity);

        Storage FindByPositionNr(string positionNr);
        Storage FindByUniqNr(string uniqNr);
        Storage FindByPositionNrOrUniqNr(string positionNr, string uniqNr);

        IQueryable<StorageUniqueItemView> SearchDetail(StorageSearchModel searchModel);
    }
}
