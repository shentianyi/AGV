using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.OPC
{
    public class OPCRoadMachineTaskFeed:OPCDataBase
    {
          public OPCRoadMachineTaskFeed()
            : base()
        {
           
        }

          public OPCRoadMachineTaskFeed(string OPCAddressKey)
              : base(OPCAddressKey)
        {
            
        }
       
    }
}
