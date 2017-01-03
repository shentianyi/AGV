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
            ServiceHost storageService = null;
            ServiceHost pickService = null;

            try
            {
                productSercice = new ServiceHost(typeof(ProductService));
                deliveryService = new ServiceHost(typeof(DeliveryService));
                trayService = new ServiceHost(typeof(TrayService));
                storageService = new ServiceHost(typeof(StorageService));
                pickService = new ServiceHost(typeof(PickService));

                productSercice.Open();
                deliveryService.Open();
                trayService.Open();
                storageService.Open();


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
