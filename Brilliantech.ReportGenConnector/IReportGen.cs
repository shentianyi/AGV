using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.ReportGenConnector
{
    public interface IReportGen
    {
        void Print(RecordSet records, ReportGenConfig config);
    }
}
