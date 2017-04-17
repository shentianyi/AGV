using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AgvServiceLib
{
    [ServiceContract]
    public interface ISystemService
    {
        [OperationContract]
        List<MoveTaskScheduleModel> GetListGreaterThan(DateTime dateTime);

        [OperationContract]
        ResultMessage CreateMoveTaskSchdule(MoveTaskScheduleModel model);

        [OperationContract]
        ResultMessage DeleteMoveTaskSchedule(int id);
    }
}
