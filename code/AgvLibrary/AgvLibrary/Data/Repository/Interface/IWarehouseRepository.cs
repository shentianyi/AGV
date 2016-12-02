using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
   public interface IWarehouseRepository
    {
        Warehouse SearchByAgvId(int AgvId);
        Warehouse SearchByWHNr(string WHNr);

        bool WHNrExist(string WHNr);
        bool AgvIdExist(int AgvId);

    }
}
