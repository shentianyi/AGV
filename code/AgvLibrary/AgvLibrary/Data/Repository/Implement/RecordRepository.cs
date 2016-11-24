using AgvLibrary.Data.Repository.Interface;
using AgvLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvLibrary.Data.Repository.Implement
{
    public class RecordRepository : IRecord
    {
        private AgvWareHouseDataContext context;
        public BasicMessage msg = new BasicMessage();

        public bool delete(Record rd)
        {
            try
            {
                context.Record.DeleteOnSubmit(rd);
                context.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Record> Search(RecordSearchModel recordSearchModel)
        {

            IQueryable<Data.Record> Records = this.context.Record;
            if (!string.IsNullOrEmpty(recordSearchModel.MovementId.ToString()))
            {
                Records = Records.Where(r => r.MovementId.Equals(recordSearchModel.MovementId));
            }


            if (!string.IsNullOrEmpty(recordSearchModel.SourcePosation))
            {
                Records = Records.Where(r => r.SourcePosation.Equals(recordSearchModel.SourcePosation));
            }

            if (!string.IsNullOrEmpty(recordSearchModel.AimedPosation))
            {
                Records = Records.Where(r => r.AimedPosation.Equals(recordSearchModel.AimedPosation));
            }

            if (!string.IsNullOrEmpty(recordSearchModel.Operation))
            {
                Records = Records.Where(r => r.Operation.Equals(recordSearchModel.Operation));
            }

            if (!string.IsNullOrEmpty(recordSearchModel.Operator))
            {
                Records = Records.Where(r => r.Operator.Equals(recordSearchModel.Operator));
            }
            if (!string.IsNullOrEmpty(recordSearchModel.Time))
            {
                Records = Records.Where(r => r.Time.Equals(recordSearchModel.Time));
            }



            return Records;


        }

        public bool Create(Record rd)
        {
            try
            {
                context.Record.InsertOnSubmit(rd);
                context.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
