using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Interface
{
    public interface IRecordRepository
    {
        bool Create(Record rd);

        IQueryable<Record> Search(RecordSearchModel recordSearchModel);

        bool delete(Record rd);
    }
}
