using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.SearchModel
{
   public class PositionSearchModel
    {
        public string Nr { get; set; }
        public string NrAct { get; set; }
        public bool? IsLocked { get; set; }
    }
}
