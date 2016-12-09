using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AgvServiceLib
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IProductService
    {
        /// <summary>
        /// 创建唯一件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [OperationContract]
        ResultMessage CreateUniqItem(UniqueItemModel item);

        [OperationContract]
        UniqueItemModel FindUniqItemByNr(string nr);

        [OperationContract]
        UniqueItemModel FindUniqItemByCheckCode(string checkCode);
    }

    
}
