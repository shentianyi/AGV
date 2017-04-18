using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AGVCenterLib.Model.Command
{
    public class AgvCarInfoMeta
    {
        public int Id { get; set; }
        public int State { get; set; }
        public string Route { get; set; }
        public string Position { get; set; }
      //  public int Charge { get; set; }
        public int Voltage { get; set; }
    }
}