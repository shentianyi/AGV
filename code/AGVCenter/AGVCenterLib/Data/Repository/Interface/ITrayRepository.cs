using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface ITrayRepository
    {
        void Create(Tray entity);

        Tray FindByNr(string nr);

        List<TrayDeliveryView> GetTrayListByDeliveryNr(string deliveryNr);
    }
}
