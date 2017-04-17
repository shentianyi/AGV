using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using AGVCenterLib.Service;
using AgvServiceLib.Helper;
using Brilliantech.Framwork.Utils.LogUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AgvServiceLib
{
    public class SystemService : ISystemService
    {
        public ResultMessage CreateMoveTaskSchdule(MoveTaskScheduleModel model)
        {
            return new MoveTaskScheduleService(SqlHelper.ConnectStr).Create(model);
        }

        public ResultMessage DeleteMoveTaskSchedule(int id)
        {
            return new MoveTaskScheduleService(SqlHelper.ConnectStr).Delete(id);
        }

        public List<MoveTaskScheduleModel> GetListGreaterThan(DateTime dateTime)
        {
            return MoveTaskScheduleModel.Converts(new MoveTaskScheduleService(SqlHelper.ConnectStr).GetListGreaterThan(dateTime));
        }
    }
}
