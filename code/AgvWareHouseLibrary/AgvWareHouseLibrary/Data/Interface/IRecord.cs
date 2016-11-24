using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvWareHouseLibrary.Data;
using AgvWareHouseLibrary.Model;

namespace AgvWareHouseLibrary.Data.Interface
{
   public interface IRecord
    {
        bool Create(Record rd);

        IQueryable<Record> Search(RecordSearchModel recordSearchModel);

        bool delete(Record rd);
    }
}
