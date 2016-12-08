using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Service;
using AgvServiceLib.DataModel;
using AgvServiceLib.Helper;
using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AgvServiceLib
{
    public class ProductService : IProductService
    {
        public ResultMessage CreateUniqItem(UniqueItemModel item)
        {
            UniqueItemService service = new UniqueItemService(SqlHelper.connectStr);
            ResultMessage message = new ResultMessage();
            UniqueItem uitem = new UniqueItem()
            {
                Nr = item.Nr,
                QR = item.QR,
                KNr = item.KNr,
                KNrWithYear = item.KNrWithYear,
                CheckCode = item.CheckCode,
                KskNr = item.KskNr,
                BoxTypeId = item.BoxTypeId
            };
            message = service.Create(uitem);

            return message;
        }

        public UniqueItemModel FindUniqItemByNr(string nr)
        {
            UniqueItemService service = new UniqueItemService(SqlHelper.connectStr);
            UniqueItem item = service.FindByNr(nr);
            return UniqueItemModel.Convert(item);
        }
    }
}
