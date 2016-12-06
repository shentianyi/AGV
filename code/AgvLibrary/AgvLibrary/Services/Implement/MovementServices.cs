using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;
using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Services.Interface;
using AgvLibrary.Data.Repository.Implement;

namespace AgvLibrary.Services.Implement
{
    public class MovementServices : ServiceBase, IMovementServices
    {
        private IMovementRepository MoRep;

        public MovementServices(string dbString) : base(dbString) {
            MoRep = new MovementRepository(this.Context);
        }

        public bool AimedPositionExist(string AimedPosition)
        {
            return MoRep.AimedPositionExist(AimedPosition);
        }

        public bool Create(Movement Mo)
        {
            return MoRep.Create(Mo);
        }

        public bool Delete(Movement Mo)
        {
            return MoRep.Delete(Mo);
        }

        public bool OperationTypeExist(int OperationType)
        {
            return MoRep.OperationTypeExist(OperationType);
        }

        public bool OperatorExist(string Operator)
        {
            return MoRep.OperatorExist(Operator);
        }

        public Movement SearchByAimedPosition(string AimedPosition)
        {
            return MoRep.SearchByAimedPosition(AimedPosition);
        }

        public Movement SearchByOperationType(int OperationType)
        {
            return MoRep.SearchByOperationType(OperationType);
        }

        public Movement SearchByOperator(string Operator)
        {
            return MoRep.SearchByOperator(Operator);
        }

        public Movement SearchBySourcePosition(string SourcePosition)
        {
            return MoRep.SearchBySourcePosition(SourcePosition);
        }

        public bool SourcePositionExist(string SourcePosition)
        {
            return MoRep.SourcePositionExist(SourcePosition);
        }
    }
}
