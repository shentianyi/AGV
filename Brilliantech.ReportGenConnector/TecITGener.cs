using System;
using System.Collections.Generic;
using TECIT.TFORMer;

namespace Brilliantech.ReportGenConnector
{
    public class TecITGener : IReportGen
    {
        public TecITGener()
        {
        }

        public void Print(RecordSet records, ReportGenConfig config)
        {
            if (records == null | config == null)
            {
                throw new ArgumentNullException("打印参数或打印数据不能为空");
            }

            try
            {
                TECITLicense.Register();
            }
            catch (Exception ex)
            {
                throw new ReportGenLicenseException(ex);
            }

            Job job = default(Job);
            JobDataRecordSet jobdata = default(JobDataRecordSet);
            job = new Job();
            jobdata = new JobDataRecordSet();
            job.JobData = jobdata;
            job.RepositoryName = config.Template;
            job.PrinterName = config.Printer;
            job.PrinterType = config.PrinterType;
            job.NumberOfCopies = config.NumberOfCopies;
         
            for (int i = 0; i <= records.Count - 1; i++)
            {
                Record rec = new Record();
                foreach (KeyValuePair<string, string> ent in records[i])
                {
                    rec.Data.Add(ent.Key, ent.Value);
                }
                jobdata.Records.Add(rec);
            }

            try
            {
                job.Print();
                job.Dispose();
            }
            catch (TFORMerException ex)
            {
                if (ex.ErrorCode == 13)
                {
                    throw new ReportTypeException(ex);
                }
                else { throw new ReportPrintException(ex); }
            }
            catch (Exception ex)
            {
                throw new ReportPrintException(ex);
            }
            finally {
                if (job != null)
                    job.Dispose();
            }
        }
    }
}
