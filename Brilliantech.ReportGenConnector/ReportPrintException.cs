using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.ReportGenConnector
{
    public class ReportPrintException : Exception
    {
        public ReportPrintException(Exception innEx)
            : base("生成打印标签或报表时出错", innEx)
        {
        }

    }
}
