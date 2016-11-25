using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Model;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Data.Repository.Implement;

namespace AgvLibrary.Services.Implement
{
    public class RecordServices : ServiceBase, IRecordServices
    {
        private IRecordRepository RdRep;
        public RecordServices(string dbString) : base(dbString) {
            RdRep = new  RecordRepository(this.Context);
        }
        public bool Create(Record rd)
        {
            return RdRep.Create(rd);
        }

        public bool delete(Record rd)
        {
            return RdRep.delete(rd);
        }

        public IQueryable<Record> Search(RecordSearchModel recordSearchModel)
        {
            return RdRep.Search(recordSearchModel);
        }
    }
}
