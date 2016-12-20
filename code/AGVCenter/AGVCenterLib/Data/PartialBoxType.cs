using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Data
{
   public partial class BoxType
    {
     
        public static string GetStr(int? boxTypeId)
        {
            return boxTypeId == 1 ? "Big" : (boxTypeId == 2 ? "Small" : "N/A");
        }
    }
}
