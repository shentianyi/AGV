using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvWareHouseLibrary.Data.Interface
{
    public interface IDataContext
    {
        System.Data.Linq.DataContext Context { get; }
        void SaveAll();
    }
}
