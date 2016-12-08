using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVCenterLib.Model;

namespace AGVCenterLib.Data.Repository.Interface
{
    public interface IDeliveryRepository
    {
        void Create(Delivery entity);

        Delivery FindByNr(string nr);
    }
}
