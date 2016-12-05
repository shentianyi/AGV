using AgvServiceLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace AgvServiceHostCon
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost productSercice = null;
            try
            {
                productSercice = new ServiceHost(typeof(ProductService));
                productSercice.Open();
                Console.WriteLine("生产服务已启动");
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.Read();
        }
    }
}
