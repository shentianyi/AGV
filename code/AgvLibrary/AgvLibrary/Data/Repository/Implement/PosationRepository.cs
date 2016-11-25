using AgvLibrary.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Model;

namespace AgvLibrary.Data.Repository.Implement
{
    class PosationRepository : RepositoryBase<Posation>, IPosationRepository
    {
        private AgvWareHouseDataContext context;


        public PosationRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public IQueryable<Posation> Search(PosationSearchModel posationSearchModel)
        {
            IQueryable<Posation> Posations = this.context.Posation;
            if (!string.IsNullOrEmpty(posationSearchModel.PosationNr))
            {
                Posations = Posations.Where(c => c.PosationNr.Equals(posationSearchModel.PosationNr));
            }
            if(!(string.IsNullOrEmpty(posationSearchModel.WHNr)&&string.IsNullOrEmpty(posationSearchModel.Floor.ToString())&&string.IsNullOrEmpty(posationSearchModel.Row.ToString())&&string.IsNullOrEmpty(posationSearchModel.Column.ToString()) ));
            {
                Posations = Posations.Where(c => c.WHNr.Equals(posationSearchModel.WHNr) && c.Floor.Equals(posationSearchModel.Floor) && c.Row.Equals(posationSearchModel.Row) && c.Coloumn.Equals(posationSearchModel.Column));
            }
            return Posations;
        }
    }
}
