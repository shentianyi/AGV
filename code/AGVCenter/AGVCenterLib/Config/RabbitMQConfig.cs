using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Brilliantech.Framwork.Utils.ConfigUtil;
using Brilliantech.Framwork.Utils.LogUtil;

namespace AGVCenterLib.Config
{
    public class RabbitMQConfig
    {
        private static ConfigUtil config;
        private static string host;
        private static int port;
        private static string userName;
        private static string pwd;

      

        static RabbitMQConfig()
        {
            try
            {
                config = new ConfigUtil("BASE", "Config/rabbitmq.ini");

                Host = config.Get("Host");
                Port = int.Parse(config.Get("Port"));
                UserName = config.Get("UserName");
                Pwd = config.Get("Pwd");
            }
            catch (Exception ex)
            {
                LogUtil.Logger.Error(ex.Message, ex);
            }
        }


        public static string Host
        {
            get
            {
                return host;
            }

            set
            {
                host = value;
            }
        }

        public static int Port
        {
            get
            {
                return port;
            }

            set
            {
                port = value;
            }
        }

        public static string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public static string Pwd
        {
            get
            {
                return pwd;
            }

            set
            {
                pwd = value;
            }
        }

    }
}
