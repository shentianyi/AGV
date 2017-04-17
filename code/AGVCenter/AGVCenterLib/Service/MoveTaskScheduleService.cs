using AGVCenterLib.Data;
using AGVCenterLib.Model.Message;
using AGVCenterLib.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Service
{
    public class MoveTaskScheduleService : ServiceBase
    {
        public MoveTaskScheduleService(string dbString) : base(dbString)
        {

        }

        public List<MoveTaskSchedule> GetListGreaterThan(DateTime dateTime)
        {
            var context = this.Context.Context as AgvWarehouseDataContext;
            return context.MoveTaskSchedule.Where(s => s.EndTime >= dateTime).OrderBy(s => s.StartTime).ToList();
        }

        /// <summary>
        /// 获取第一个应该加载的任务
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public MoveTaskSchedule GetFirstTaskSchedule(DateTime dateTime)
        {
            var context = this.Context.Context as AgvWarehouseDataContext;
            return context.MoveTaskSchedule.Where(s => s.EndTime > dateTime).OrderBy(s => s.StartTime).FirstOrDefault();
        }

        public ResultMessage Create(MoveTaskScheduleModel model)
        {
            ResultMessage msg = new ResultMessage();
            try
            {
                var context = this.Context.Context as AgvWarehouseDataContext;
                context.MoveTaskSchedule.InsertOnSubmit(new MoveTaskSchedule()
                {
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    CreatedAt = DateTime.Now,
                    CreateUserNr = model.CreateUserNr
                });
                this.Context.SaveAll();
                msg.Success = true;
                return msg;
            }
            catch (Exception ex)
            {
                msg.Content = ex.Message;
            }
            return msg;
        }

        public ResultMessage Delete(int id)
        {
            ResultMessage msg = new ResultMessage();
            try
            {
                var context = this.Context.Context as AgvWarehouseDataContext;
                var task = context.MoveTaskSchedule.FirstOrDefault(s => s.Id == id);
                if (task != null)
                {
                    context.MoveTaskSchedule.DeleteOnSubmit(task);
                    this.Context.SaveAll();
                }
                msg.Success = true;
                return msg;
            }
            catch (Exception ex)
            {
                msg.Content = ex.Message;
            }
            return msg;
        }
    }
}
