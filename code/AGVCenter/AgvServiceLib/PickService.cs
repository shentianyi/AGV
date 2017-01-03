using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.SearchModel;
using AGVCenterLib.Model.ViewModel;
using AGVCenterLib.Service;
using AgvServiceLib.Helper;

namespace AgvServiceLib
{
    public class PickService : IPickService
    {
        AGVCenterLib.Service.PickListService ps;

        public PickService()
        {
            ps = new AGVCenterLib.Service.PickListService(SqlHelper.ConnectStr);
        }

        public ResultMessage CreateOutStockTaskByNr(string nr)
        {
            return new StockTaskService(SqlHelper.ConnectStr).CreateOutStockTaskByPickList(nr);
        }

        public ResultMessage CreatePickList(string pickListNr, List<string> uniqItemsNrs)
        {
            return ps.CreatePickList(pickListNr, uniqItemsNrs);
        }

        public void DeletePickListForTest(string pickListNr)
        {
            ps.DeletePickListForTest(pickListNr);
        }

        public List<StockTaskModel> GetPickListOutStockTasks(string nr)
        {
            return StockTaskModel.Converts(ps.GetPickListOutStockTasks(nr));
        }

        public List<PickListStorageViewModel> GetPickListStorageByNr(string nr)
        {
            return PickListStorageViewModel
                .Converts(new PickListService(SqlHelper.ConnectStr).GetPickListStorageByNr(nr));
        }

        public List<UniqueItemModel> GetPickListUniqItemsByNr(string nr)
        {
            return UniqueItemModel.Converts(ps.GetPickListUniqItemsByNr(nr));
        }

        public bool PickListExists(string nr)
        {
            return ps.PickListExsits(nr);
        }

        public List<PickListModel> SearchList(PickListSearchModel searchModel, int limit)
        {
            return PickListModel.Converts(ps.SearchList(searchModel, limit));
        }
    }
}
