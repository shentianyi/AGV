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
            ServiceHost deliveryService = null;
            ServiceHost trayService = null;
            try
            {
                productSercice = new ServiceHost(typeof(ProductService));
                deliveryService = new ServiceHost(typeof(DeliveryService));
                trayService = new ServiceHost(typeof(TrayService));

                productSercice.Open();
                deliveryService.Open();
                trayService.Open();

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
