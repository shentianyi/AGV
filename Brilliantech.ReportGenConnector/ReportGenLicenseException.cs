using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brilliantech.ReportGenConnector
{
    public class ReportGenLicenseException : Exception
    {
        public ReportGenLicenseException(Exception innEx)
            : base("InvalidRight", innEx)
        {
            Console.WriteLine("------ Reg Ex-------");
            Console.WriteLine(innEx.Message);
            Console.WriteLine(innEx.Source);
            Console.WriteLine(innEx.TargetSite);
            Console.WriteLine(innEx.StackTrace);
                   }
    }
}
