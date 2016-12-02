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

        public Position FindByPositionNr(string PositionNr)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.PositionNr.Equals(PositionNr));
        }

        public Position FindByPosition(string WHNr, int Floor, int Column, int Row)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.WHNr.Equals(WHNr) && c.Floor.Equals(Floor) && c.Column.Equals(Column) && c.Row.Equals(Row));
        }

        public Position FindByState(int State)
        {
            return this.context.GetTable<Position>().FirstOrDefault(c => c.State.Equals(State));
        }

        public bool PositionNrExist(string PositionNr)
        {
            return FindByPositionNr(PositionNr) != null;
        }

        public bool PositionExist(string WHNr, int Floor, int Column, int Row)
        {
            return FindByPosition(WHNr, Floor, Column, Row) != null;
        }

        public bool StateExist(int State)
        {
            return FindByState(State)!=null;
        }
    }
}
