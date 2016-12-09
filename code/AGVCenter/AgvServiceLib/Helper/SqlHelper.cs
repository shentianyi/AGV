using AgvServiceLib.Properties;
using Brilliantech.Framwork.Utils.ConfigUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgvServiceLib.Helper
{
    public class SqlHelper
    {
        public static string ConnectStr = new ConfigUtil("DB", "Config/db.ini").Get("ConnectString");
    }
}
