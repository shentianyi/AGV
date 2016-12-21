using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using AGVCenterLib.Model.ViewModel;
using AgvServiceLib.Helper;

namespace AgvServiceLib
{
    public class StorageService : IStorageService
    {
        AGVCenterLib.Service.StorageService ss;

        public StorageService()
        {
            ss = new AGVCenterLib.Service.StorageService(SqlHelper.ConnectStr);
        }

        public List<StorageModel> GetAll()
        {
            return StorageModel.Converts(ss.All());
        }
         
    }
}
