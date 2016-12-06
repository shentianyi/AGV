using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data.Repository.Implement
{
  public  class PositionRepository : RepositoryBase<Position>,IPosationRepository
    {
        private AgvWareHouseDataContext context;

        public PositionRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public Position SearchByPositionNr(string PositionNr)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.PositionNr.Equals(PositionNr));
        }

        public Position SearchByPosition(string WHNr, int Floor, int Column, int Row)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.WHNr.Equals(WHNr) && c.Floor.Equals(Floor) && c.Column.Equals(Column) && c.Row.Equals(Row));
        }

        public Position SearchByState(int State)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.State.Equals(State));
        }

        public bool PositionNrExist(string PositionNr)
        {
            return SearchByPositionNr(PositionNr) != null;
        }

        public bool PositionExist(string WHNr, int Floor, int Column, int Row)
        {
            return SearchByPosition(WHNr, Floor, Column, Row) != null;
        }

        public bool StateExist(int State)
        {
            return SearchByState(State)!=null;
        }
    }
}
