using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgvClientWPF.Helper
{
  public  class UniqueHelper
    {
        public static object locker = new object();
        public static string GenerateUniqString()
        {
            lock (locker)
            {
                return DateTime.Now.ToString("yyMMddHHmmsss");
            }
        }
    }
}
