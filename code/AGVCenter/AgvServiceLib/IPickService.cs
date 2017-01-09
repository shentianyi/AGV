using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.SearchModel;
using AGVCenterLib.Model.ViewModel;

namespace AgvServiceLib
{
    [ServiceContract]
    public interface IPickService
    {
        [OperationContract]
        bool PickListExists(string nr);
         
        [OperationContract]
        ResultMessage CreatePickList(string pickListNr, List<string> uniqItemsNrs);

        [OperationContract]
        List<UniqueItemModel> GetPickListUniqItemsByNr(string nr);

       
     
        [OperationContract]
        ResultMessage CreateOutStockTaskByNr(string nr);
         

        [OperationContract]
        List<StockTaskModel> GetPickListOutStockTasks(string nr);

        [OperationContract]
        List<PickListModel> SearchList(PickListSearchModel searchModel, int limit);

      
        [OperationContract]
        void DeletePickListForTest(string pickListNr);

        [OperationContract]
        List<PickListStorageViewModel> GetPickListStorageByNr(string nr);

        [OperationContract]
        ResultMessage CanItemAddToPickList(string uniqNr);

        [OperationContract]
        ResultMessage CancelPickOutStockTask(List<int> taskIds);
    }
}
