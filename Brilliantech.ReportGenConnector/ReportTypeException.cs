using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECIT.TFORMer;

namespace Brilliantech.ReportGenConnector
{
    public class ReportTypeException : TFORMerException
    {
        public ReportTypeException(Exception innEx)
            : base("打印机类型错误,请重新设置",13, innEx.Source)
        {

        }

    }
}
