using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TECIT.TFORMer;
using System.Runtime.InteropServices;

namespace Brilliantech.ReportGenConnector
{
     
    public class TECITLicense
    {
        private static readonly string pf_licensee = "Mem: Cai Zhuo Information & Technology Co.,Ltd";
        private static readonly string pf_licenseKey = "45D392D071FA5767B4D85D2EE7ED7E76";
        private static readonly int pf_numberOfLicense = 1;

        private static readonly LicenseKind pf_licenseKind = LicenseKind.Developer;
        public static void Register()
        {
            TFORMer.License(pf_licensee, pf_licenseKind, pf_numberOfLicense, pf_licenseKey);
        }
    }
}
