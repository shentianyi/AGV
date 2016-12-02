using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Interface
{
   public interface IMovementRepository
    {
        Movement SearchBySourcePosition(string SourcePosition);
        Movement SearchByAimedPosition(string AimedPosition);
        Movement SearchByOperationType(int OperationType);
        Movement SearchByOperator(string Operator);
        bool OperatorExist(string Operator);
        bool SourcePositionExist(string SourcePosition);
        bool AimedPositionExist(string AimedPosition);
        bool OperationTypeExist(int OperationType);
       



    }
}
