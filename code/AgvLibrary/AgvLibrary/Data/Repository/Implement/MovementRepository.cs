using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data.Repository.Interface;

namespace AgvLibrary.Data.Repository.Implement
{
    public class MovementRepository : RepositoryBase<Movement>, IMovementRepository
    {
        private AgvWareHouseDataContext context;
        public MovementRepository(IDataContextFactory dataContextFactory) : base(dataContextFactory)
        {
            this.context = dataContextFactory.Context as AgvWareHouseDataContext;
        }

        public bool AimedPositionExist(string AimedPosition)
        {
            return SearchByAimedPosition(AimedPosition) != null;
        }

        public bool OperationTypeExist(int OperationType)
        {
            return SearchByOperationType(OperationType) != null;
        }

        public bool OperatorExist(string Operator)
        {
            return SearchByOperator(Operator) != null;
        }

        public Movement SearchByAimedPosition(string AimedPosition)
        {
            return this.context.GetTable<Movement>().FirstOrDefault(c => c.AimedPosition.Equals(AimedPosition));
        }

        public Movement SearchByOperationType(int OperationType)
        {
            return this.context.GetTable<Movement>().FirstOrDefault(c => c.OperationType.Equals(OperationType));
        }

        public Movement SearchByOperator(string Operator)
        {
            return this.context.GetTable<Movement>().FirstOrDefault(c => c.Operator.Equals(Operator));
        }

        public Movement SearchBySourcePosition(string SourcePosition)
        {
            return this.context.GetTable<Movement>().FirstOrDefault(c => c.SourcePosition.Equals(SourcePosition));
        }

        public bool SourcePositionExist(string SourcePosition)
        {
            return SearchBySourcePosition(SourcePosition) != null;
        }
    }
}
