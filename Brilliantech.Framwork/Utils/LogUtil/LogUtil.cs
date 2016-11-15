using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace Brilliantech.Framwork.Utils.LogUtil
{
    public class LogUtil
    {
        private static log4net.ILog logger;

        static LogUtil()
        {
          //  log4net.Config.XmlConfigurator.Configure();
            XmlConfigurator.Configure();

            logger=LogManager.GetLogger(typeof(LogUtil));
        }

        public static log4net.ILog Logger
        {
            get { return LogUtil.logger; }
        }
    }
}
