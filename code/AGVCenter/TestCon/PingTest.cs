using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace TestCon
{
 public   class PingTest
    {
        System.Timers.Timer t;

        public PingTest()
        {
            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += T_Elapsed; 
        }

        public void Test()
        {
            t.Start();
        }

        public void EndTest()
        {
            t.Enabled = false;
            t.Stop();
        }

        private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            t.Stop();
            DoTest();
            t.Start();
        }

        private  void DoTest()
        { 
            Ping ping = new Ping();
            
            PingReply pingReply = ping.Send("192.168.1.27");
            if (pingReply.Status == IPStatus.Success)
            { 
                Console.WriteLine("当前在线，已ping通！");
            }
            else
            {
                Console.WriteLine("不在线，ping不通！");
            }
        }
         
    }
}
