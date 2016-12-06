using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgvLibrary.Data;

namespace AgvLibrary.Services.Interface
{
   public interface IMovementServices
    {
        /// <summary>
        /// 根据SourcePosition查询
        /// </summary>
        /// <param name="SourcePosition"></param>
        /// <returns></returns>
        Movement SearchBySourcePosition(string SourcePosition);

        /// <summary>
        /// 根据目标位置查询
        /// </summary>
        /// <param name="AimedPosition"></param>
        /// <returns></returns>
        Movement SearchByAimedPosition(string AimedPosition);


        /// <summary>
        /// 根绝操作类型查询
        /// </summary>
        /// <param name="OperationType"></param>
        /// <returns></returns>
        Movement SearchByOperationType(int OperationType);


        /// <summary>
        /// 根据操作者查询
        /// </summary>
        /// <param name="Operator"></param>
        /// <returns></returns>
        Movement SearchByOperator(string Operator);


        /// <summary>
        /// 该操作者是否存在
        /// </summary>
        /// <param name="Operator"></param>
        /// <returns></returns>
        bool OperatorExist(string Operator);


        /// <summary>
        /// SourcePosition是否存在
        /// </summary>
        /// <param name="SourcePosition"></param>
        /// <returns></returns>
        bool SourcePositionExist(string SourcePosition);

        /// <summary>
        /// AimedPosition是否存在
        /// </summary>
        /// <param name="AimedPosition"></param>
        /// <returns></returns>
        bool AimedPositionExist(string AimedPosition);

        /// <summary>
        /// 该操作类型是否存在
        /// </summary>
        /// <param name="OperationType"></param>
        /// <returns></returns>
        bool OperationTypeExist(int OperationType);

        bool Create(Movement Mo);

        bool Delete(Movement Mo);
    }
}
