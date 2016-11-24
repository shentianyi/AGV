using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Data.Repository.Implement;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;

namespace AgvLibrary.Services.Implement
{
    public class PosationServices:ServiceBase,IPosationServices
    {
        private IPosationRepository PoRep;

        public PosationServices(string dbString) : base(dbString)
        {
            PoRep = new PosationRepository(this.Context);
        }

        public IQueryable<Posation> Search(PosationSearchModel posationSearchModel)
        {
            return PoRep.Search(posationSearchModel);
        }
    }
}
