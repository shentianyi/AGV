using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AgvServiceLib
{
    public class DeliveryService : IDeliveryService
    {
        public bool DeliveryExists(string nr)
        {
            return nr=="1";
        }
    }
}
