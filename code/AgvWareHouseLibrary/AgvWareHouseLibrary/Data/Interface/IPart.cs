using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvWareHouseLibrary.Model;

namespace AgvWareHouseLibrary.Data.Interface
{
    public interface IPart
    {
        IQueryable<Part> Search(PartSearchModel partSearchModel);
    }
}
